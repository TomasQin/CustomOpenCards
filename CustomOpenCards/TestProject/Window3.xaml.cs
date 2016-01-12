using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CustomUI.AuthorSelect;

namespace TestProject
{
    /// <summary>
    /// Interaction logic for Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        public Window3()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Window3_Loaded);
        }

        void Window3_Loaded(object sender, RoutedEventArgs e)
        {
            Picker.Text = DateTime.Now.ToString();
            //var aa = new AuthorSelectControl();
            //aa.Init();
            //RootGrid.Children.Add(aa);
            //var bb = GetDateTimes(DateTime.Now);
        }

        public DateTime[] GetDateTimes(DateTime standardDateTime)
        {
            DateTime[] list = new DateTime[6];

            list[0] = standardDateTime;
            MatchQuarter(standardDateTime, ref list[1], ref list[2]);
            list[3] = standardDateTime.AddDays(-3);
            list[4] = standardDateTime.AddDays(-14);
            list[5] = standardDateTime.AddMonths(-1);
            return list;
        }

        private void MatchQuarter(DateTime dateTime, ref DateTime quarterStart, ref DateTime quarterEnd)
        {
            switch (dateTime.Month)
            {
                case 1:
                case 2:
                case 3:
                    quarterStart = DateTime.Parse(String.Format("{0}-12-31", dateTime.AddYears(-1).Year));
                    quarterEnd = DateTime.Parse(String.Format("{0}-3-31", dateTime.Year));
                    break;
                case 4:
                case 5:
                case 6:
                    quarterStart = DateTime.Parse(String.Format("{0}-3-31", dateTime.Year));
                    quarterEnd = DateTime.Parse(String.Format("{0}-6-30", dateTime.Year));
                    break;
                case 7:
                case 8:
                case 9:
                    quarterStart = DateTime.Parse(String.Format("{0}-6-30", dateTime.Year));
                    quarterEnd = DateTime.Parse(String.Format("{0}-9-30", dateTime.Year));
                    break;
                case 10:
                case 11:
                case 12:
                    quarterStart = DateTime.Parse(String.Format("{0}-9-30", dateTime.Year));
                    quarterEnd = DateTime.Parse(String.Format("{0}-12-31", dateTime.Year));
                    break;
            }
        }

        private void Picker_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var aa = Picker;

        }
    }
}
