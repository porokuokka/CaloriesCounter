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
    public sealed partial class CreateNewPage : Page
    {
        private Item newItem;

        public CreateNewPage()
        {
            this.InitializeComponent();
            newItem = new Item();
            this.LayoutRoot.DataContext = newItem;
            this.GridNutrition.DataContext = newItem;
            this.GridPortion.DataContext = newItem;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            newItem = new Item();
            this.LayoutRoot.DataContext = newItem;
            this.GridNutrition.DataContext = newItem;
            this.GridPortion.DataContext = newItem;
        }

        #region navigation

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        private void HomeBorder_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void SearchBorder_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SearchPage));
        }


        private void DiaryBorder_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DiaryPage));
        }

        #endregion

        private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            newItem.Portion = getPortion();
            newItem.countPortion();
            //Debug.Text = newItem.PortionCalories.ToString();
            InsertItem(newItem);
        }

        /// <summary>
        /// Returns the portion name from portionselector
        /// </summary>
        /// <returns></returns>
        private string getPortion()
        {
            string portion = "tädää";
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            int index = PortionSelector.SelectedIndex;
            switch (index)
            {
                case 0: portion = loader.GetString("Piece"); break;
                case 1: portion = loader.GetString("Tblsp"); break;
                case 2: portion = loader.GetString("Tsp"); break;
                case 3: portion = loader.GetString("Jar"); break;
                default: portion = loader.GetString("Piece"); break;
            }

            return portion;
        }

        /// <summary>
        /// Inserts item to mobileservice table, removes
        /// progressbar when finished and clears the new item
        /// space from additem grid
        /// </summary>
        /// <param name="item">Item to insert</param>
        private async void InsertItem(Item item)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            try
            {
                await App.MobileService.GetTable<Item>().InsertAsync(item);
            }
            catch (HttpRequestException)
            {
                string s = loader.GetString("HttpError");
                ShowError(s);
                return;
            }
           
            //the old add item goes to garbage automatically?
            newItem = new Item();
            this.GridNutrition.DataContext = newItem;
            this.GridPortion.DataContext = newItem;
            LayoutRoot.DataContext = newItem;
            textBlock.Visibility = Visibility.Visible;
            textBlock.Text = loader.GetString("AddingSuccess");
            var f = this.Resources["Storyboard1"] as Storyboard;
            if (f != null) f.Begin();
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

            }
        }

    }
}
