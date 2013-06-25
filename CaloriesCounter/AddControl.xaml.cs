using CaloriesCounter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace CaloriesCounter
{
    /// <summary>
    /// Counter-class which handles counting calories
    /// by Items properties and usercontrols values
    /// </summary>
    public sealed class CounterClass : INotifyPropertyChanged
    {
        public CounterClass() { }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Property to indicate whether to count
        /// by grams or portion
        /// </summary>
        private bool isGramsSelected;
        public bool IsGramsSelected
        {
            get { return isGramsSelected; }
            set { isGramsSelected = value; countCalories(); }
        }

        /// <summary>
        /// Property to indicate the amount of portion/grams
        /// </summary>
        private float amount;
        public float Amount
        {
            get { return amount; }
            set
            {
                if (value >= 0.01)

                    amount = value;
                NotifyPropertyChanged("Amount");
                countCalories();
            }
        }

        /// <summary>
        /// Item to the get the info
        /// </summary>
        private Item _item;
        public Item item
        {
            get { return _item; }
            set { _item = value; }
        }

        /// <summary>
        /// Property to store the value of
        /// counted calories
        /// </summary>
        private float countedCalories;
        public float CountedCalories
        {
            get { return countedCalories; }
            set { countedCalories = value; NotifyPropertyChanged("CountedCalories"); }
        }

        private float grams;
        public float Grams
        {
            get { return grams; }
            set { grams = value; NotifyPropertyChanged("Grams"); }
        }

        /// <summary>
        /// Counts calories by grams or portions by checking
        /// the isGramsSelected -property
        /// </summary>
        public void countCalories()
        {
            if (item != null)
            {
                if (isGramsSelected)
                {
                    CountedCalories = Amount * (item.Calories / 100F);
                    Grams = Amount;
                }

                if (!isGramsSelected)
                {
                    CountedCalories = Amount * item.PortionCalories;
                    Grams = Amount * item.Portionweight;
                }
            }
        }
    }

    public sealed partial class AddControl : UserControl
    {
        private CounterClass CounterData;

        public CounterClass getCounterClass()
        {
            return CounterData;
        }

        public AddControl()
        {
            InitializeComponent();
        }

        public AddControl(Item item)
        {
            CounterData = new CounterClass();
            CounterData.Amount = 100;
            CounterData.item = item;

            InitializeComponent();
            Counted.DataContext = CounterData;
            TextBoxAmount.DataContext = CounterData;
            RadioPortion.Content = item.Portion;
            RadioGrams.IsChecked = true;
        }

        private void RadioGrams_Checked(object sender, RoutedEventArgs e)
        {
            CounterData.Amount = 100;
            CounterData.IsGramsSelected = true;
        }

        private void RadioPortion_Checked(object sender, RoutedEventArgs e)
        {
            CounterData.Amount = 1;
            CounterData.IsGramsSelected = false;
        }

        private void ButtonMinus_Click(object sender, RoutedEventArgs e)
        {
            CounterData.Amount--;
        }

        private void ButtinPlus_Click(object sender, RoutedEventArgs e)
        {
            CounterData.Amount++;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}
