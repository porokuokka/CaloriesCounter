using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloriesCounter.Models
{
    [Table("Day")]
        public class Day
        {
            [PrimaryKey, AutoIncrement]
            public int Id { get; set; }

            public DateTime Date { get; set; }

            public int Total { get; set; }

            public float Carbohydrates { get; set; }

            public float Protein { get; set; }

            public float Fat { get; set; }

            public float Fibre { get; set; }

        }
}
