using CaloriesCounter.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloriesCounter.ViewModels
{
    public class IntakesViewModel : ViewModelBase
    {
        private ObservableCollection<IntakeViewModel> intakes;
        public ObservableCollection<IntakeViewModel> Intakes
        {
            get
            {
                return intakes;
            }

            set
            {
                intakes = value;
                RaisePropertyChanged("Intakes");
            }
        }

       // private CaloriesCounter.App app = (Application.Current as App);
        /*
        public ObservableCollection<IntakesViewModel> GetIntakes()
        {
            intakes = new ObservableCollection<IntakesViewModel>();
            using (var db = new SQLite.SQLiteConnection(app.DBPath))
            {
                var query = db.Table<Intake>();
                foreach (var _intake in query)
                {
                    var intake = new IntakeViewModel()
                    {
                        Id = _intake.Id,
                        DayId = _intake.DayId
                        //TODO: other properties too
                    };
                    intakes.Add(intake);
                }
            }
            return intakes;
        }
        */
        /*
        /// <summary>
        /// Gets intakes to collection by dayId
        /// </summary>
        /// <param name="dayId"></param>
        /// <returns></returns>
        public ObservableCollection<IntakeViewModel> GetIntakes(int dayId)
        {
            intakes = new ObservableCollection<IntakeViewModel>();
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var query = (db.Table<Intake>().Where(
                            i => i.DayId == dayId));
                foreach (var _intake in query)
                {
                    var intake = new IntakeViewModel()
                    {
                        Id = _intake.Id,
                        DayId = _intake.DayId,
                        Name = _intake.Name
                        //TODO: other properties too
                    };
                    intakes.Add(intake);
                }
            }
            return intakes;
        }*/

        /// <summary>
        /// Gets intakes to collection by dayId
        /// </summary>
        /// <param name="dayId"></param>
        /// <returns></returns>
        public List<IntakeViewModel> GetIntakesList(int dayId)
        {
            List<IntakeViewModel> list = new List<IntakeViewModel>();
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var query = (db.Table<Intake>().Where(
                            i => i.DayId == dayId));

                foreach (var _intake in query)
                {
                    var intake = new IntakeViewModel()
                    {
                        Id = _intake.Id,
                        DayId = _intake.DayId,
                        Name = _intake.Name,
                        Carbohydrates = _intake.Carbohydrates,
                        Protein = _intake.Protein,
                        Fat = _intake.Fat,
                        Fibre = _intake.Fibre,
                        Calories = _intake.Calories,
                        Grams = _intake.Grams
                    };
                    list.Add(intake);
                }
            }
            return list;
        }

    }
}
