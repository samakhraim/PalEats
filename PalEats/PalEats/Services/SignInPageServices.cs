using System.Data.SqlClient;
using System.Threading.Tasks;

using PalEats.Models;

namespace PalEats.Services
{
    public class SignInServices
    {
        private string connectionString = "Server=tcp:paleatserver.database.windows.net,1433;Initial Catalog=PalEatsDB;Persist Security Info=False;User ID=sama;Password=S11a7m2!a000;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public async Task<int> AuthenticateUserAsync(SignUpModel signInModel)
        {
            int userId = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand checkCommand = new SqlCommand("SELECT UserId FROM [Users] WHERE Email = @Email AND Password = @Password", connection);
                checkCommand.Parameters.AddWithValue("@Email", signInModel.Email);
                checkCommand.Parameters.AddWithValue("@Password", signInModel.Password);

                userId = (int)await checkCommand.ExecuteScalarAsync();
            }

            return userId;
        }
    }
}
