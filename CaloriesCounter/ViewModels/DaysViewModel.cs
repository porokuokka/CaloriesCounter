using CaloriesCounter.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloriesCounter.ViewModels
{
    public class DaysViewModel : ViewModelBase
    {
        private ObservableCollection<DayViewModel> days;
        public ObservableCollection<DayViewModel> Days
        {
            get
            {
                return days;
            }

            set
            {
                days = value;
                RaisePropertyChanged("Days");
            }
        }

        //private CaloriesCounter.App app = (Application.Current as App);

        public ObservableCollection<DayViewModel> GetDays()
        {
            days = new ObservableCollection<DayViewModel>();
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var query = db.Table<Day>();
                foreach (var _day in query)
                {
                    var day = new DayViewModel()
                    {
                        Id = _day.Id,
                        Total = _day.Total,
                        Date = _day.Date,
                        Carbohydrates = _day.Carbohydrates,
                        Fibre = _day.Fibre,
                        Fat = _day.Fat,
                        Protein = _day.Protein
                        //TODO: other properties too
                    };
                    days.Add(day);
                }
            }
            return days;
        }
    }
}
