using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using PalEats.Models;
namespace PalEats.Services
{
    public class FavoriteServices
    {
        string connectionString = "Data Source=paleatserver.database.windows.net;Initial Catalog=PalEatsDB;Persist Security Info=True;User ID=sama;Password=S11a7m2!a000";
        public async Task<int> AddFavoriteAsync(int userId,int dishId)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                   await connection.OpenAsync() ;
                    SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM [Favorite] WHERE DishId = @DishId AND UserId = @UserId", connection);
                    checkCommand.Parameters.AddWithValue("@DishId",dishId);
                    checkCommand.Parameters.AddWithValue("@UserId", userId);

                    int count = (int)await checkCommand.ExecuteScalarAsync();

                    if (count > 0)
                    {
                        // given user has recipe as favorite, so return 0 to indicate failure
                        return 0;
                    }


                    SqlCommand insertCommand = new SqlCommand("INSERT INTO [Favorite] (UserId, DishId) VALUES (@UserId, @DishId)", connection);
                    insertCommand.Parameters.AddWithValue("@DishId", dishId);
                    insertCommand.Parameters.AddWithValue("@UserId", userId);

                    rowsAffected = await insertCommand.ExecuteNonQueryAsync();

                    connection.Close();
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine("SQL Error adding to favorite: " + ex.Message);
                throw new Exception("SQL Error: Unable to add to favorite. Message: " + ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error adding to favorite: " + ex.Message);
                throw new Exception(" Unable to add to favorite. Message: " + ex.Message);
            }

            return rowsAffected;
        }



        public async Task<bool> IsSelectedAsync(int userId, int dishId)
        {
            bool output =false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM [Favorite] WHERE DishId = @DishId AND UserId = @UserId", connection);
                    checkCommand.Parameters.AddWithValue("@DishId", dishId);
                    checkCommand.Parameters.AddWithValue("@UserId", userId);

                    int count = (int)await checkCommand.ExecuteScalarAsync();

                    if (count > 0)
                    {
                        // given user has recipe as favorite, so return 0 to indicate failure
                        output =true;
                    }

                    connection.Close();
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine("SQL Error fetching favorite status: " + ex.Message);
                throw new Exception("SQL Error: Failed to fetch favorite status. Message: " + ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error fetching favorite status: " + ex.Message);
                throw new Exception("Failed to fetch favorite status. Message: " + ex.Message);
            }
            Debug.WriteLine(output);
            return output;
        }


        public async Task<bool> RemoveFavoriteAsync(int userId,int dishId)
        {
            bool output = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                   await connection.OpenAsync() ;
                    SqlCommand deleteCommand = new SqlCommand("DELETE FROM [Favorite] WHERE DishId = @DishId AND UserId = @UserId", connection);
                    deleteCommand.Parameters.AddWithValue("@DishId",dishId);
                    deleteCommand.Parameters.AddWithValue("@UserId", userId);

                     await deleteCommand.ExecuteScalarAsync();
                    output = true;

                    connection.Close();
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine("SQL Error adding to favorite: " + ex.Message);
                throw new Exception("SQL Error: Unable to add to favorite. Message: " + ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error adding to favorite: " + ex.Message);
                throw new Exception(" Unable to add to favorite. Message: " + ex.Message);
            }

            return output;
        }


    }

}