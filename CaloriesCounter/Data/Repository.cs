using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaloriesCounter.lib;
using SQLite;
using CaloriesCounter.Data.Models;

namespace CaloriesCounter.Data
{
        public class Repositority<T> where T : new()
        {
            private SQLiteAsyncConnection db;
            public Repositority(SQLiteAsyncConnection db)
            {
                this.db = db;
            }

            public AsyncTableQuery<T> Items
            {
                get
                {
                    return db.Table<T>();
                }
            }

            public async Task<int> Create(T newEntity)
            {
                return await db.InsertAsync(newEntity);
            }

            public async Task<int> Update(T entity)
            {
                return await db.UpdateAsync(entity);
            }

            public async Task<int> Delete(T entity)
            {
                return await db.DeleteAsync(entity);
            }
        }
    
}
