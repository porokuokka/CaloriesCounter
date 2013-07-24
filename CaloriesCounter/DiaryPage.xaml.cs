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
using Windows.UI.Popups;

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
            RefreshDay();
        }

        /// <summary>
        /// Refreshes day totals and intakelist
        /// </summary>
        public async void RefreshDay()
        {
            TextBlockDate.Text = convertDate(App.CurrentDay.Date);
            GridDayTotal.DataContext = App.CurrentDay;
            xintakes = await App.dataSource.Intakes.Items.Where(i => i.DayId == App.CurrentDay.Id).ToListAsync();
            ListViewDays.ItemsSource = xintakes;
        }

        /// <summary>
        /// Changes date to given one,
        /// creates day to database if it doesn't
        /// exist yet
        /// </summary>
        /// <param name="date"></param>
        private void changeDate(DateTime date)
        {
            Day day;
            day = App.Helper.checkDay(date);
            if (day == null)
            {
                day = new Day();
                day.Date = date;
                App.Helper.createDay(day);
            }

            App.CurrentDay = day;
            RefreshDay();
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

            try
            {
                App.Helper.deleteIntake(intake);
            }

            catch (ArgumentException)
            {
                var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
                string s = loader.GetString("DeleteError");
                ShowError(s);
            }

            xintakes = null;
            await App.dataSource.Days.Update(App.CurrentDay);
            RefreshDay();
            changeDate(App.CurrentDay.Date);

        }

        /// <summary>
        /// Shows error message in a messagedialog
        /// </summary>
        /// <param name="error"></param>
        private async void ShowError(string error)
        {
            try
            {
                await new MessageDialog(error).ShowAsync();
            }
            catch (UnauthorizedAccessException)
            {
                //TODO: do something
            }
        }

    }
}
