using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaloriesCounter.Models;

namespace CaloriesCounter.ViewModels
{
        public class IntakeViewModel : ViewModelBase
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

            private string name;
            public string Name
            {
                get { return name; }
                set
                {
                    if (name == value) return;
                    name = value;
                    RaisePropertyChanged("Name");
                }
            }

            private int dayId;
            public int DayId
            {
                get { return dayId; }
                set
                {
                    if (dayId == value) { return; }
                    dayId = value;
                    RaisePropertyChanged("DayId");
                }
            }

            private int calories;
            public int Calories
            {
                get { return calories; }
                set
                {
                    if (calories == value) { return; }
                    calories = value;
                    RaisePropertyChanged("Calories");
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

            private float grams;
            public float Grams
            {
                get { return grams; }
                set
                {
                    if (grams == value) { return; }
                    grams = value;
                    RaisePropertyChanged("Grams");
                }
            }

            #endregion "Properties"

            //private CaloriesCounter.App app = (Application.Current as App);

            public IntakeViewModel GetIntake(int intakeId)
            {
                var intake = new IntakeViewModel();
                using (var db = new SQLite.SQLiteConnection(App.DBPath))
                {
                    var _intake = (db.Table<Intake>().Where(
                        i => i.Id == intakeId)).Single();
                    intake.Id = _intake.Id;
                    intake.Name = _intake.Name;
                    intake.DayId = _intake.DayId;
                    intake.Calories = _intake.Calories;
                    intake.Carbohydrates = _intake.Carbohydrates;
                    intake.Protein = _intake.Protein;
                    intake.Fat = _intake.Fat;
                    intake.Fibre = _intake.Fibre;
                }
                return intake;
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

            ///Saves an intake WARNING: must have dayid which exists on db!
            public string SaveIntake(IntakeViewModel intake)
            {
                string result = string.Empty;
                using (var db = new SQLite.SQLiteConnection(App.DBPath))
                {
                    string change = string.Empty;
                    var day = (db.Table<Day>().Where(d => d.Id == intake.DayId)).SingleOrDefault();

                        try
                        {
                            var existingIntake = (db.Table<Intake>().Where(
                                i => i.Id == intake.Id)).SingleOrDefault();

                            if (existingIntake != null)
                            {
                                existingIntake.DayId = intake.DayId; //not needed?
                                existingIntake.Name = intake.Name; //not needed?
                                existingIntake.Grams = intake.Grams;
                                existingIntake.Carbohydrates = intake.Carbohydrates;
                                existingIntake.Protein = intake.Protein;
                                existingIntake.Fat = intake.Fat;
                                existingIntake.Fibre = intake.Fibre;
                                existingIntake.Calories = intake.Calories;

                                //Refresh the total values
                                int success = db.Update(existingIntake);
                            }

                            else
                            {
                                int success = db.Insert(new Intake()
                                {
                                    Id = this.GetNewDayId(),
                                    DayId = intake.DayId,
                                    Name = intake.Name,
                                    Grams = intake.Grams,
                                    Carbohydrates = intake.Carbohydrates,
                                    Protein = intake.Protein,
                                    Fat = intake.Fat,
                                    Fibre = intake.Fibre,
                                    Calories = intake.Calories
                                });

                            }
                            result = "Success";
                        }
                        catch (Exception)
                        {
                            result = "This intake was not saved.";
                        }

                   // refreshDayTotals(day);
                }
                return result;
            }

            public void DeleteIntake(IntakeViewModel intake)
            {
                using (var db = new SQLite.SQLiteConnection(App.DBPath))
                {
                    Intake delete = db.Table<Intake>().Where(i => i.Id == intake.Id).SingleOrDefault();
                    var day = (db.Table<Day>().Where(d => d.Id == intake.DayId)).SingleOrDefault();
                    reduceDayTotals(day, intake);
                    db.Delete(delete);
                }
            }

            /// <summary>
            /// Refreshes days total values,
            /// </summary>
            /// <param name="day">Day that has been queryed from db before</param>
            public void reduceDayTotals(Day day, IntakeViewModel intake)
            {
                using (var db = new SQLite.SQLiteConnection(App.DBPath))
                {
                    day.Total -= intake.Calories;
                    day.Carbohydrates -= intake.Carbohydrates;
                    day.Protein -= intake.Protein;
                    day.Fat -= intake.Fat;
                    day.Fibre -= intake.Fibre;
                    db.Update(day);
                }
            }

            public string CreateIntake(IntakeViewModel intake)
            {
                string result = string.Empty;
                using (var db = new SQLite.SQLiteConnection(App.DBPath))
                {
                    string change = string.Empty;
                    var day = (db.Table<Day>().Where(d => d.Id == intake.DayId)).SingleOrDefault();

                    try
                    {
                            int success = db.Insert(new Intake()
                            {
                                Id = this.GetNewDayId(),
                                DayId = intake.DayId,
                                Name = intake.Name,
                                Grams = intake.Grams,
                                Carbohydrates = intake.Carbohydrates,
                                Protein = intake.Protein,
                                Fat = intake.Fat,
                                Fibre = intake.Fibre,
                                Calories = intake.Calories
                            });

                        result = "Success";
                    }
                    catch (Exception)
                    {
                        result = "This intake was not saved.";
                    }

                    addUpDayTotals(day, intake);
                }
                return result;
            }

            /// <summary>
            /// Refreshes days total values,
            /// </summary>
            /// <param name="day">Day that has been queryed from db before</param>
            public void addUpDayTotals(Day day, IntakeViewModel intake)
            {
                using (var db = new SQLite.SQLiteConnection(App.DBPath))
                {
                    day.Total += intake.Calories;
                    day.Carbohydrates += intake.Carbohydrates;
                    day.Protein += intake.Protein;
                    day.Fat += intake.Fat;
                    day.Fibre += intake.Fibre;
                    db.Update(day);
                }
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
