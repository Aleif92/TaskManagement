using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TaskManagement.Models;

namespace TaskManagement.Repositories
{
    public class TaskRepository
    {
        private readonly DatabaseHelper _databaseHelper;

        public TaskRepository(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public async Task<IEnumerable<TaskModel>> GetTasksByUsernameAsync(string username)
        {
            using var connection = _databaseHelper.CreateConnection();
            string query = "SELECT * FROM Tasks WHERE UserId = (SELECT Id FROM Users WHERE Username = @Username)";
            return await connection.QueryAsync<TaskModel>(query, new { Username = username });
        }


        //  Get All Tasks
        public async Task<IEnumerable<TaskModel>> GetAllTasksAsync()
        {
            using var connection = _databaseHelper.CreateConnection();
            string query = "SELECT * FROM Tasks";
            return await connection.QueryAsync<TaskModel>(query);
        }

        //  Get Task by ID
        public async Task<TaskModel> GetTaskByIdAsync(int id)
        {
            using var connection = _databaseHelper.CreateConnection();
            string query = "SELECT * FROM Tasks WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<TaskModel>(query, new { Id = id });
        }

        //  Add a New Task
        public async Task<int> AddTaskAsync(TaskModel task)
        {
            using var connection = _databaseHelper.CreateConnection();
            string query = "INSERT INTO Tasks (Title, Description, Status, UserId) VALUES (@Title, @Description, @Status, @UserId)";
            return await connection.ExecuteAsync(query, task);
        }

        //  Update an Existing Task
        public async Task<int> UpdateTaskAsync(TaskModel task)
        {
            using var connection = _databaseHelper.CreateConnection();
            string query = "UPDATE Tasks SET Title = @Title, Description = @Description, Status = @Status WHERE Id = @Id";
            return await connection.ExecuteAsync(query, task);
        }

        // 🛠️ Delete a Task
        public async Task<int> DeleteTaskAsync(int id)
        {
            using var connection = _databaseHelper.CreateConnection();
            string query = "DELETE FROM Tasks WHERE Id = @Id";
            return await connection.ExecuteAsync(query, new { Id = id });
        }
    }
}
