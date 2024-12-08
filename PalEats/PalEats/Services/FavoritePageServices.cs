using PalEats.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
namespace PalEats.Services
{
    public class FavoritePageServices
    {
        string connectionString = "Data Source=paleatserver.database.windows.net;Initial Catalog=PalEatsDB;Persist Security Info=True;User ID=sama;Password=S11a7m2!a000";

        public async Task<List<Dish>> GetFavoritepageAsync()
        {
            List<Dish> dish = new List<Dish>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = "SELECT Dish.DishId, Dish.DishName, Dish.DishImgUrl FROM Favorite JOIN Dish ON Favorite.DishId = Dish.DishId WHERE Favorite.UserId = @currentUser";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    int currentUserId = ((App)App.Current).currentUser;
                    command.Parameters.AddWithValue("@currentUser", currentUserId);
                    await connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    { 
                        Dish dishes = new Dish
                        {
                            DishId = Convert.ToInt32(reader["DishId"]),
                            DishName = reader["DishName"].ToString(),
                            DishImgUrl = reader["DishImgUrl"].ToString(),
                        };
                        dish.Add(dishes);

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
            return dish;
        }
    }
}