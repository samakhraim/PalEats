using PalEats.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace PalEats.Services
{
    internal class SearchPageServices
    {
        string connectionString = "Data Source=paleatserver.database.windows.net;Initial Catalog=PalEatsDB;Persist Security Info=True;User ID=sama;Password=S11a7m2!a000";

        public async Task<List<Dish>> Search(string searchText)
        {
            List<Dish> searchResults = new List<Dish>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = "SELECT * FROM Dish WHERE DishName LIKE @SearchText";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Dish dish = new Dish
                            {
                                DishId = Convert.ToInt32(reader["DishId"]),
                                DishImgUrl = reader.GetString(reader.GetOrdinal("DishImgUrl")),
                                DishName = reader.GetString(reader.GetOrdinal("DishName"))
                            };
                            searchResults.Add(dish);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error message: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error message: {ex.Message}");
                throw;
            }

            return searchResults;
        }
    }
}
