using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace CaloriesCounter.Models
{
    [Table("Intake")]
    class Intake
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int DayId { get; set; }

       // public int IngredientId { get; set; }

        public string Name { get; set; }

        public float Grams { get; set; }

        public int Calories { get; set; }

        public float Carbohydrates { get; set; }

        public float Protein { get; set; }

        public float Fat { get; set; }

        public float Fibre { get; set; }
    }
}
