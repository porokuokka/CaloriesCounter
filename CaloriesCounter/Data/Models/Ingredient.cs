using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace CaloriesCounter.Data.Models
{
    class Ingredient
    {
        [PrimaryKey, AutoIncrement]
        public int IntakeId { get; set; }

        public int ItemId { get; set; }

        public int DayId { get; set; }

        public float Portionweight { get; set; }

        public float PortionCalories { get; set; }

        public string Portion { get; set; }

        public int Calories { get; set; }

        public float Carbohydrates { get; set; }

        public float Protein { get; set; }

        public float Fat { get; set; }

        public float Fibre { get; set; }
    }
}
