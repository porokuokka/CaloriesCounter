using CaloriesCounter.Models;
using CaloriesCounter.DataAccess.Repository;
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
using CaloriesCounter.ViewModels;
using SQLite;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CaloriesCounter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DiaryPage : Page
    {
        private DayViewModel dayViewModel { get; set; }
        private IntakesViewModel Intakes;
        private List<IntakeViewModel> intks;
        private MessageBox msgbox;

        public DiaryPage()
        {
            this.InitializeComponent();
            msgbox = new MessageBox();
            msgbox.ShowMessage("Tultiin diarypagelle");
            LayoutRoot.Children.Add(msgbox);

            DatePickerDiary.Date = App.CurrentDay.Date;
            changeDate(App.CurrentDay.Date);
        }

        private void changeDate(DateTime date)
        {
            //DatePickerDiary.Date = date;
            App.CurrentDay = DayViewModel.GetDayByDate(date); //tuleeko vähän ristivetoa?
            dayViewModel = App.CurrentDay;
            GridDayTotal.DataContext = dayViewModel;
            TextBlockTotal.DataContext = dayViewModel.Total.ToString();
            TextBlockCarbohydrates.DataContext = dayViewModel.Carbohydrates.ToString();
            TextBlockDate.Text = convertDate(date);
            Intakes = new IntakesViewModel();
            intks = Intakes.GetIntakesList(dayViewModel.Id);
            ListViewDays.ItemsSource = intks;
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

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

        }




    }
}
