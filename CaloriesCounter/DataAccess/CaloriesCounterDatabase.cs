using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using CaloriesCounter.Models;
///http://visualstudiomagazine.com/articles/2013/02/27/build-a-windows-store-app-using-sqlite.aspx
namespace CaloriesCounter.DataAccess
{
    public interface IDatabase
    {
        Task Initialize();
        SQLiteAsyncConnection GetAsyncConnection();
    }

    public class Database : IDatabase
    {
        private readonly SQLiteAsyncConnection _dbConnection;

        public Database(string databasePath)
        {
            _dbConnection = new SQLiteAsyncConnection(databasePath);
        }

        public async Task Initialize()
        {
            await _dbConnection.CreateTableAsync<Day>();
            await _dbConnection.CreateTableAsync<Intake>();
            
        }

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            return _dbConnection;
        }
    }
}
