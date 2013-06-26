using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloriesCounter.DataAccess.Entities
{
        public class Day
        {
            [PrimaryKey, AutoIncrement]
            public int Id { get; set; }

            public DateTime Date { get; set; }

            [MaxLength(30)]
            public string Testi { get; set; }

        }
}
