using CaloriesCounter.DataAccess.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloriesCounter.DataAccess.Repository
{
    class DayRepository : IDayRepository
    {
        private readonly SQLiteAsyncConnection _database;

        public DayRepository(IDatabase database)
        {
            _database =  database.GetAsyncConnection();
        }

        /// <summary>
        /// Creates or updates day on db - if id is 0,
        /// it creates a row in dn
        /// </summary>
        /// <param name="day">Day to save</param>
        /// <returns></returns>
        public async Task SaveAsync(Day day)
        {
            if (day.Id == 0)
                await _database.InsertAsync(day);
            else
                await _database.UpdateAsync(day);
        }

        /// <summary>
        /// Deletes given day on db
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Day day)
        {
            await _database.DeleteAsync(day);
        }

        /// <summary>
        /// Returns all days on db
        /// </summary>
        /// <returns></returns>
        public async Task<List<Day>> GetAllAsync()
        {
            var days = await _database.Table<Day>().ToListAsync();
            return days;
        }
    }
}
