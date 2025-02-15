using Dapper;
using TaskManagement.Models;

namespace TaskManagement.Repositories
{
    public class UserRepository
    {

        private readonly DatabaseHelper _databaseHelper;

        public UserRepository(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            using var connection = _databaseHelper.CreateConnection();
            string query = "SELECT * FROM Users WHERE Username = @Username";
            return await connection.QueryFirstOrDefaultAsync<User>(query, new { Username = username });
        }

        public async Task<int> RegisterUserAsync(User user)
        {
            using var connection = _databaseHelper.CreateConnection();
            string query = "INSERT INTO Users (Username, PasswordHash, Role) VALUES (@Username, @PasswordHash, @Role)";
            return await connection.ExecuteAsync(query, user);
        }
    }
}
