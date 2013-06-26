using CaloriesCounter.DataAccess;
using CaloriesCounter.DataAccess.Entities;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CaloriesCounter
{
    

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DiaryPage : Page
    {
        private Day _day;
        private IDayRepository _dayRepository;
        private ListView ListViewDays2;

        public DiaryPage()
        {
            this.InitializeComponent();
            DatePickerDiary.Date = DateTime.Today;
            ListViewDays2 = new ListView();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            InitializeDay();
            await InitializeDatabase();
            await UpdateContacts();   
        }

        private async Task UpdateContacts()
        {
            ListViewDays2.ItemsSource = await _dayRepository.GetAllAsync();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            await _dayRepository.SaveAsync(_day);
            await UpdateContacts();
            //Status.Text = string.Format("{0} has been saved to your contacts.", _contact);
        }

        private void InitializeDay()
        {
            _day = new Day();
            _day.Date = DateTime.Today;
            _day.Testi = "Testipäivä";
            TextBlockDate.Text = _day.Date.ToString();
            TextBlockName.Text = _day.Testi;
        }

        private async Task InitializeDatabase()
        {
            string datbasePath = Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\contacts.db";
            Database database = new Database(datbasePath);
            await database.Initialize();
            _dayRepository = new DayRepository(database);
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
