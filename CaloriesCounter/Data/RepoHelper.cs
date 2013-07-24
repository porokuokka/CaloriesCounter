using CaloriesCounter.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloriesCounter.Data
{
    public class RepoHelper
    {
        private DataSource ds { get; set; }

        public RepoHelper(DataSource dst)
        {
            ds = dst;
        }
        
        /// <summary>
        /// Checks if the day exists in db,
        /// if doesn't, returns null
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public Day checkDay(DateTime date)
        {
            try
            {
                var lookup = App.dataSource.Days.Items.Where(d => d.Date == date).ToListAsync().GetAwaiter().GetResult().First();
                return (Day)lookup;
            }

            catch (InvalidOperationException)
            {
                return null;
            }

        }

        /// <summary>
        /// Creates a day in db an puts reference to newDay
        /// </summary>
        /// <param name="newDay"></param>
        public async void createDay(Day newDay)
        {
            
            int success;
            try
            {
                success = await App.dataSource.Days.Create(newDay);
            }

            catch (InvalidOperationException)
            {
                //kukkuu
            }

            try
            {
                var lookup = ds.Days.Items.Where(d => d.Date == newDay.Date)
                    .ToListAsync().GetAwaiter().GetResult().First();
                newDay = (Day)lookup;
            }

            catch (InvalidOperationException)
            {
                //kukkuu
            }

        }

        /// <summary>
        /// Adds a new intake to diary refreshing daytotals
        /// @Throws ArgumentException if creation or refreshing is not successful
        /// </summary>
        /// <param name="newIntake"></param>
        public async void addIntake(Intake newIntake)
        {
           int success = await App.dataSource.Intakes.Create(newIntake);
           if (success == 0)
           {
               throw new ArgumentException();
           }

           App.CurrentDay.Total += newIntake.Calories;
           App.CurrentDay.Carbohydrates += newIntake.Carbohydrates;
           App.CurrentDay.Protein += newIntake.Protein;
           App.CurrentDay.Fat += newIntake.Fat;
           App.CurrentDay.Fibre += newIntake.Fibre;

           success = await App.dataSource.Days.Update(App.CurrentDay);

           if (success == 0)
           {
               throw new ArgumentException();
           }
        }

        /// <summary>
        /// Deletes intake from the day its added in,
        /// refreshes daytotals
        /// </summary>
        /// <param name="delete"></param>
        public async void deleteIntake(Intake delete)
        {
            App.CurrentDay.Total -= delete.Calories;
            App.CurrentDay.Carbohydrates -= delete.Carbohydrates;
            App.CurrentDay.Protein -= delete.Protein;
            App.CurrentDay.Fat -= delete.Fat;
            App.CurrentDay.Fibre -= delete.Fibre;

            int success = await App.dataSource.Intakes.Delete(delete);
            
            if (success == 0)
            {
                throw new ArgumentException();
            }

        }
        
    }
}
