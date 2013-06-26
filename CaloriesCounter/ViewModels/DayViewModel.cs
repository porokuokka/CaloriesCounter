using CaloriesCounter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloriesCounter.ViewModels
{
    public class DayViewModel : ViewModelBase
    {
        #region Properties

        private int id = 0;
        public int Id
        {
            get
            { return id; }

            set
            {
                if (id == value)
                { return; }

                id = value;
                RaisePropertyChanged("Id");
            }
        }

        /// <summary>
        /// Consider if needed - the date should only
        /// be set once
        /// </summary>
        private DateTime date;
        public DateTime Date
        {
            get { return date; }

            set
            {
                if (date == value) { return; }

                date = value;
                RaisePropertyChanged("Date");
            }
        }

        private int total;
        public int Total
        {
            get { return total; }
            set
            {
                if (total == value) { return; }
                total = value;
                RaisePropertyChanged("Total");
            }
        }

        private float carbohydrates;
        public float Carbohydrates
        {
            get { return carbohydrates; }
            set
            {
                if (carbohydrates == value) { return; }
                carbohydrates = value;
                RaisePropertyChanged("Carbohydrates");
            }
        }

        private float protein;
        public float Protein
        {
            get { return protein; }
            set 
            {
                if (protein == value) { return; }
                protein = value;
                RaisePropertyChanged("Protein");
            }
        }

        private float fat;
        public float Fat
        {
            get { return fat; }
            set
            {
                if (fat == value) { return; }
                fat = value;
                RaisePropertyChanged("Fat");
            }
        }

        private float fibre;
        public float Fibre
        {
            get { return fibre; }
            set
            {
                if (fibre == value) { return; }
                fibre = value;
                RaisePropertyChanged("Fibre");
            }
        }

        #endregion "Properties"

        //private CaloriesCounter.App app = (Application.Current as App);

        public DayViewModel GetDay(int dayId)
        {
            var day = new DayViewModel();
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var _day = (db.Table<Day>().Where(
                    d => d.Id == dayId)).Single();
                day.Id = _day.Id;
                day.Total = _day.Total;
                day.Date = _day.Date;
                day.Carbohydrates = _day.Carbohydrates;
                day.Protein = _day.Protein;
                day.Fat = _day.Fat;
                day.Fibre = _day.Fibre;
            }
            return day;
        }

        /// <summary>
        /// Gets day by date, if date doesn't exist,
        /// a new day by given date will be created to db
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DayViewModel GetDayByDate(DateTime date)
        {
            var day = new DayViewModel();
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                try {

                    var _day = (db.Table<Day>().Where(d => d.Date == date)).Single();
                    day.Id = _day.Id;
                    day.Total = _day.Total;
                    day.Date = _day.Date;
                    day.Carbohydrates = _day.Carbohydrates;
                    day.Protein = _day.Protein;
                    day.Fat = _day.Fat;
                    day.Fibre = _day.Fibre;
                }
                catch (Exception)
                {
                    Day dayrow = new Day();
                    dayrow.Date = date;
                    day.Date = date;
                    db.Insert(dayrow);
                }
                return day;
            }
        }

        /**
        public string GetCustomerName(int customerId)
        {
            string customerName = "Unknown";
            using (var db = new SQLite.SQLiteConnection(app.DBPath))
            {
                var customer = (db.Table<Customer>().Where(
                    c => c.Id == customerId)).Single();
                customerName = customer.Name;
            }
            return customerName;
        }**/

        public string SaveDay(DayViewModel day)
        {
            string result = string.Empty;
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                string change = string.Empty;
                try
                {
                    var existingDay = (db.Table<Day>().Where(
                        d => d.Id == day.Id)).SingleOrDefault();

                    if (existingDay != null)
                    {
                        existingDay.Date = day.Date;
                        existingDay.Carbohydrates = day.Carbohydrates;
                        existingDay.Protein = day.Protein;
                        existingDay.Fat = day.Fat;
                        existingDay.Fibre = day.Fibre;
                        existingDay.Total = day.Total;

                        int success = db.Update(existingDay);
                    }
                    else
                    {
                        int success = db.Insert(new Day()
                        {
                            Id = day.id,
                            Date = day.Date,
                            Carbohydrates = day.Carbohydrates,
                            Protein = day.Protein,
                            Fat = day.Fat,
                            Fibre = day.Fibre,
                            Total = day.Total
                        });
                    }
                    result = "Success";
                }
                catch (Exception)
                {
                    result = "This day was not saved.";
                }
            }
            return result;
        }

        /**
        public string DeleteCustomer(int customerId)
        {
            string result = string.Empty;
            using (var db = new SQLite.SQLiteConnection(app.DBPath))
            {
                var projects = db.Table<Project>().Where(
                    p => p.CustomerId == customerId);
                foreach (Project project in projects)
                {
                    db.Delete(project);
                }
                var existingCustomer = (db.Table<Customer>().Where(
                    c => c.Id == customerId)).Single();

                if (db.Delete(existingCustomer) > 0)
                {
                    result = "Success";
                }
                else
                {
                    result = "This customer was not removed";
                }
            }
            return result;
        }**/

        public int GetNewDayId()
        {
            int dayId = 0;
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var days = db.Table<Day>();
                if (days.Count() > 0)
                {
                    dayId = (from d in db.Table<Day>()
                                  select d.Id).Max();
                    dayId += 1;
                }
                else
                {
                    dayId = 1;
                }
            }
            return dayId;
        }

    }
}
