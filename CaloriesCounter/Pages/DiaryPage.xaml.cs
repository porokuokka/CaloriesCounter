using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SQLite;
using System.Collections.ObjectModel;
using CaloriesCounter.Data.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CaloriesCounter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DiaryPage : Page
    {
        private List<Intake> xintakes;

        public DiaryPage()
        {
            this.InitializeComponent();

            DatePickerDiary.Date = App.CurrentDay.Date;
            changeDate(App.CurrentDay.Date);
        }

        /// <summary>
        /// Changes date to given one,
        /// creates day to database if it doesn't
        /// exist yet
        /// </summary>
        /// <param name="date"></param>
        private async void changeDate(DateTime date)
        {
            Day day = new Day
            {
                Date = date
            };

            try
            {
                var lookup = App.dataSource.Days.Items.Where(d => d.Date == date)
                    .ToListAsync().GetAwaiter().GetResult().First();
                day = (Day)lookup;
            }

            catch (ArgumentNullException)
            {
                App.dataSource.Days.Create(day).GetAwaiter().GetResult();
            }

            App.CurrentDay = day;
            xintakes = await App.dataSource.Intakes.Items.Where(i => i.DayId == day.Id).ToListAsync();
            ListViewDays.ItemsSource = xintakes;
            TextBlockDate.Text = convertDate(date);

            /*
            //DatePickerDiary.Date = date;
            App.CurrentDay = DayViewModel.GetDayByDate(date);
            dayViewModel = App.CurrentDay;
            GridDayTotal.DataContext = dayViewModel;
 
            TextBlockDate.Text = convertDate(date);
            Intakes = new IntakesViewModel();
            intks = Intakes.GetIntakesList(dayViewModel.Id);
            ListViewDays.ItemsSource = intks;
             * */
        }

        private string convertDate(DateTime date)
        {
            return date.Day + "." + date.Month + "." + date.Year;
        }

        private void DatePickerDiary_SelectionChanged(object sender, RoutedEventArgs e)
        {
            changeDate(DatePickerDiary.Date);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }


        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        #region navigation


        private void HomeBorder_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void SearchBorder_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SearchPage));
        }

        private void CreateNewBorder_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreateNewPage));
        }

        #endregion

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            Intake intake = (Intake)(sender as Button).DataContext;
            await App.dataSource.Intakes.Delete(intake);
           // changeDate(App.CurrentDay.Date);
        }



    }
}
