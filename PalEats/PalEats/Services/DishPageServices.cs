using System;
using System.Collections.Generic;
using System.Text;
using PalEats.Models;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace PalEats.Services
{
    public class DishPageServices
    {
        string connectionString = "Data Source=paleatserver.database.windows.net;Initial Catalog=PalEatsDB;Persist Security Info=True;User ID=sama;Password=S11a7m2!a000";

        public async Task<List<Dish>> GetDishesByCategoryId(int categoryId)
        {
            List<Dish> dishes = new List<Dish>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = "SELECT DishId, DishName, DishImgUrl " +
                                      "FROM Dish " +
                                      "JOIN Category ON Dish.CategoryId = Category.CategoryId " +
                                      "WHERE Dish.CategoryId = @categoryId";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@categoryId", categoryId);
                    await connection.OpenAsync();

                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        Dish dishData = new Dish
                        {
                            DishId = Convert.ToInt32(reader["DishId"]),
                            DishName = reader["DishName"].ToString(),
                            DishImgUrl = reader["DishImgUrl"].ToString(),
                        };
                        dishes.Add(dishData);
                    }

                    reader.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error fetching dishes: " + ex.Message);
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to fetch dishes", "OK");
            }

            return dishes;
        }

        
    }
}
