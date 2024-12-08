using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using PalEats.Models;

namespace PalEats.Services
{
    public class SignUpServices
    {
        string connectionString = "Server=tcp:paleatserver.database.windows.net,1433;Initial Catalog=PalEatsDB;Persist Security Info=False;User ID=sama;Password=S11a7m2!a000;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public async Task<int> InsertNewUsersAsync(SignUpModel signUpModel)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Check if a user with the same email already exists
                    SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM [Users] WHERE Email = @Email", connection);
                    checkCommand.Parameters.AddWithValue("@Email", signUpModel.Email);

                    int count = (int)await checkCommand.ExecuteScalarAsync();

                    if (count > 0)
                    {
                        // A user with the same email already exists, so return 0 to indicate failure
                        return 0;
                    }

                    // Insert the new user and return the new user's ID
                    SqlCommand insertCommand = new SqlCommand("INSERT INTO [Users] (Email, Password) OUTPUT INSERTED.UserId VALUES (@Email, @Password)", connection);
                    insertCommand.Parameters.AddWithValue("@Email", signUpModel.Email);
                    insertCommand.Parameters.AddWithValue("@Password", signUpModel.Password);

                    int newUserId = (int)await insertCommand.ExecuteScalarAsync();

                    return newUserId;
                }
            }
            catch (Exception ex)
            {
                // Log the exception and rethrow it to be caught in the SignUpAsync method
                Console.WriteLine($"Error in InsertNewUsersAsync: {ex.Message}");
                throw ex;
            }
        }
    }
}
