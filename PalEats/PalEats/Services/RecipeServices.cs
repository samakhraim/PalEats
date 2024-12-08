using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using PalEats.Models;
namespace PalEats.Services
{
    public class RecipeServices
    {
        string connectionString = "Data Source=paleatserver.database.windows.net;Initial Catalog=PalEatsDB;Persist Security Info=True;User ID=sama;Password=S11a7m2!a000";
        public async Task<Recipe> GetRecipesAsync(int id)
        {
            Recipe recipe = new Recipe();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = "SELECT * FROM Dish WHERE DishId=" + id;
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    await connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        recipe = new Recipe
                        {
                            DishId = Convert.ToInt32(reader["DishId"]),
                            DishName = reader["DishName"].ToString(),
                            Description = reader["Description"].ToString(),
                            Preparation = reader["Preparation"].ToString(),
                            NumberOfPeople = reader["NumberOfPeople"].ToString(),
                            Calories = reader["Calories"].ToString(),
                            DueTime = reader["DueTime"].ToString(),
                            DishImgUrl = reader["DishImgUrl"].ToString(),
                            CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        };
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine("SQL Error fetching recipes: " + ex.Message);
                throw new Exception("SQL Error: Failed to fetch recipes. Message: " + ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error fetching recipes: " + ex.Message);
                throw new Exception("Failed to fetch recipes. Message: " + ex.Message);
            }

            return recipe;
        }
        public async Task<List<Ingredients>> GetIngredientsAsync(int id)
        {
            List<Ingredients> ingredients = new List<Ingredients>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = "SELECT Ingredients.IngredientsName, DishIngredients.Amount, DishIngredients.Unit, Ingredients.IngredientsId FROM DishIngredients INNER JOIN Ingredients ON DishIngredients.IngredientsId = Ingredients.IngredientsId  where DishId = " + id;
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    await connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        Ingredients ingredient = new Ingredients
                        {
                            Amount = Convert.ToSingle(reader["Amount"]),
                            Unit = reader["Unit"].ToString(),
                            Name = reader["IngredientsName"].ToString(),
                            Id = Convert.ToInt32(reader["IngredientsId"])
                        };
                        ingredients.Add(ingredient);
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine("SQL Error fetching recipes: " + ex.Message);
                throw new Exception("SQL Error: Failed to fetch recipes. Message: " + ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error fetching recipes: " + ex.Message);
                throw new Exception("Failed to fetch recipes. Message: " + ex.Message);
            }
            return ingredients;
        }
    }
}