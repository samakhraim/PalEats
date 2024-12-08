using PalEats.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;

namespace PalEats.Services
{
    internal class ShoppingListPageServices
    {
        string connectionString = "Server=tcp:paleatserver.database.windows.net,1433;Initial Catalog = PalEatsDB; Persist Security Info=False;User ID = sama; Password=S11a7m2!a000; MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30;\r\n";

        public async Task<List<Ingredients>> getShoppingList()
        {
            List<Ingredients> shoppingList = new List<Ingredients>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    // Select all existing ingredients in shopping list for the current user
                    SqlCommand retrieveCommand = new SqlCommand("SELECT * FROM ShoppingList WHERE UserId = @UserId", connection);
                    retrieveCommand.Parameters.AddWithValue("@UserId", ((App)App.Current).currentUser);

                    SqlDataReader reader = await retrieveCommand.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        Ingredients ingredient = new Ingredients
                        {
                            Amount = Convert.ToSingle(reader["Amount"]),
                            Id = Convert.ToInt32(reader["IngredientsId"]),
                            Unit = reader["Unit"].ToString()
                        };
                        shoppingList.Add(ingredient);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error fetching ingredients: " + ex.Message);
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to fetch ingredients", "OK");
            }
            return shoppingList;
        }
        public async Task AddToShoppingList(List<Ingredients> ingredients)
        {
            try
            {
                var checkedIngredients = new List<Ingredients>();
                foreach (var ingredient in ingredients)
                {
                    if (ingredient.IsChecked == true)
                    {
                        checkedIngredients.Add(ingredient);
                    }
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var oldList = await getShoppingList();
                    var newList = oldList.Concat(checkedIngredients).GroupBy(item => item.Id).Select(group => new Ingredients
                    {
                        Id = group.Key,
                        Amount = group.Sum(item => item.Amount),
                        Unit = group.First().Unit
                    }).ToList();

                    SqlCommand deleteCommand = new SqlCommand("DELETE FROM ShoppingList WHERE UserId = @UserId", connection);
                    deleteCommand.Parameters.AddWithValue("@UserId", ((App)App.Current).currentUser);
                    await deleteCommand.ExecuteNonQueryAsync();


                    // Insert or update new ingredients in shopping list for the current user
                    foreach (var ingredient in newList)
                    {
                        SqlCommand insertCommand = new SqlCommand("INSERT INTO ShoppingList (UserId, IngredientsId, Amount, Unit) VALUES (@UserId, @IngredientsId, @Amount, @Unit)", connection);
                        insertCommand.Parameters.AddWithValue("@UserId", ((App)App.Current).currentUser);
                        insertCommand.Parameters.AddWithValue("@IngredientsId", ingredient.Id);
                        insertCommand.Parameters.AddWithValue("@Amount", ingredient.Amount);
                        insertCommand.Parameters.AddWithValue("@Unit", ingredient.Unit);
                        await insertCommand.ExecuteNonQueryAsync();
                    }
                }
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
            catch (SqlException ex)
            {
                Debug.WriteLine($"SqlException in AddToShoppingList: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while updating your shopping list. Please try again later.", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in AddToShoppingList: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while updating your shopping list. Please try again later.", "OK");
            }
        }
    }
}