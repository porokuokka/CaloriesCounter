using CaloriesCounter.Models;
using CaloriesCounter.Usercontrols;
using CaloriesCounter.ViewModels;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CaloriesCounter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        private List<Item> search;
        //private ProgressBar Progressbar = new ProgressBar();
        private AddControl add;

        public SearchPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GridItemDetails.Visibility = Visibility.Collapsed;
            
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

        #region search

        private async void Search(string hakusana)
        {
            search = null;
            ListViewItems.ItemsSource = null;
            try
            {
                search = await App.MobileService.GetTable<Item>().Where(item => item.Name.Contains(hakusana)).ToListAsync();
            }
            catch (HttpRequestException)
            {
                var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
                string s = loader.GetString("HttpError");
                ShowError(s);
            }

            if (search == null || search.Count == 0) TextBlockSearch.Visibility = Visibility.Visible;

            ListViewItems.ItemsSource = search;

        }

        /// <summary>
        /// Takes the keywords from search-textbox and
        /// delegates searching for Search-method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxSearch_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            TextBox text = (TextBox)sender;
            
            if (!String.IsNullOrWhiteSpace(text.Text))
            {
                Debug.Text = text.Text;
                TextBlockSearch.Visibility = Visibility.Collapsed;
                Search(text.Text);
            }

        }


        private void TextBoxSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).SelectAll();
            GridItemDetails.Visibility = Visibility.Collapsed;
            GridAdd.Children.Remove(add);
            add = null;
            GridItemDetails.DataContext = null;
            search = null;
            ListViewItems.ItemsSource = null;
        }

        #endregion


        #region navigation
        private void HomeBorder_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void DiaryBorder_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DiaryPage));
        }

        private void CreateNewBorder_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreateNewPage));
        }
        #endregion

        #region Item

        private void ListViewItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((ListViewItems.SelectedItem) == null) return;
            GridAdd.Children.Remove(add);
            add = null;
            GridItemDetails.DataContext = ListViewItems.SelectedItem;
            GridItemDetails.Visibility = Visibility.Visible;
            add = new AddControl(ListViewItems.SelectedItem as Item);
            ButtonAddToDiary.DataContext = add.getCounterClass();
            GridAdd.Children.Add(add);
        }

        private void ButtonAddToDiary_Click(object sender, RoutedEventArgs e)
        {
            IntakeViewModel intake = new IntakeViewModel();
            Item item = ListViewItems.SelectedItem as Item;
            intake.Grams = add.getCounterClass().Grams;
            intake.Carbohydrates = item.Carbohydrates / 100F * intake.Grams;
            intake.Protein = item.Protein / 100F * intake.Grams;
            intake.Fat = item.Fat / 100F * intake.Grams;
            intake.Fibre = item.Fibre / 100F * intake.Grams;
            intake.Id = 0;
            intake.DayId = App.CurrentDay.Id;
            intake.Name = item.Name;
            intake.Calories = (int)add.getCounterClass().CountedCalories;
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            if (intake.CreateIntake(intake))
            {
                textBlock.Text = loader.GetString("AddingSuccess");
                textBlock.Visibility = Visibility.Visible;
                var f = this.Resources["Storyboard1"] as Storyboard;
                if (f != null) f.Begin();
            }
            else
            {
                ShowError(loader.GetString("AddingError"));
            }
        }
        #endregion

      
    }
}
