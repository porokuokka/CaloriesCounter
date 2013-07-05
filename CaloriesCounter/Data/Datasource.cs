using CaloriesCounter.lib;
using CaloriesCounter.Data.Models;
using System.Linq;
using SQLite;
using System;
using System.IO;
using Windows.Storage;
using System.Collections.Generic;
//http://sympletech.com/using-sqlite-in-windows-rt-store-apps-2/
namespace CaloriesCounter.Data
{
    public class DataSource : IDisposable
    {
        private const string DBFILENAME = "mySQLite.db";
        protected StorageFolder UserFolder { get; set; }
        protected SQLiteAsyncConnection Db { get; set; }

        public Repositority<Day> Days { get; set; }
        public Repositority<Intake> Intakes { get; set; }

        public DataSource()
        {
            this.UserFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var dbPath = Path.Combine(UserFolder.Path, DBFILENAME);
            this.Db = new SQLiteAsyncConnection(dbPath);

            Days = new Repositority<Day>(Db);
            Intakes = new Repositority<Intake>(Db);
        }

        public void Dispose()
        {
            this.Db = null;
        }

        public void InitDatabase()
        {
            //Check to ensure db file exists
            try
            {
                //Try to read the database file
                UserFolder.GetFileAsync(DBFILENAME).GetAwaiter().GetResult();
            }
            catch
            {
                //Will throw an exception if not found
                UserFolder.CreateFileAsync(DBFILENAME).GetAwaiter().GetResult();
            }
        }

        public void CreateTables()
        {
            var existingTables =
                Db.QueryAsync<sqlite_master>("SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;")
                  .GetAwaiter()
                  .GetResult();

            //-- Day
            if (existingTables.Any(x => x.name == "Day") != true)
            {
                Db.CreateTableAsync<Day>().GetAwaiter().GetResult();
            }

            //-- Intake
            if (existingTables.Any(x => x.name == "Intake") != true)
            {
                Db.CreateTableAsync<Intake>().GetAwaiter().GetResult();
            }
        }
    }


}

