using PalEats.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PalEats.Services
{
    public class CategoryPageServices
    {
      
        string connectionString = "Server=tcp:paleatserver.database.windows.net,1433;Initial Catalog = PalEatsDB; Persist Security Info=False;User ID = sama; Password=S11a7m2!a000; MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30;\r\n";
    public async Task<List<Category>> GetCategoriesAsync()
        {
            
            List<Category> categories = new List<Category>(); 
           
            try
            {
               
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = "SELECT CategoryId, CategoryImgUrl, CategoryName FROM Category";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    await connection.OpenAsync();

                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        Category categoryData = new Category
                        {
                            CategoryId = Convert.ToInt32(reader["CategoryId"]),
                            ImgUrl = reader["CategoryImgUrl"].ToString(),
                            CategoryName = reader["CategoryName"].ToString()
                        };
                        categories.Add(categoryData);
                    }

                    reader.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error fetching categories: " + ex.Message);
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to fetch categories", "OK");
            }

            return categories;
        }
    }
}