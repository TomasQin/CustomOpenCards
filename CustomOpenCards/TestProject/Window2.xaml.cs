using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using CustomUI.PannelBL;
using CustomUI.PannelControls;

namespace TestProject
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Window2_Loaded);
        }

        void Window2_Loaded(object sender, RoutedEventArgs e)
        {
            //ParametersManagement.CurrentUiDispatcher = Dispatcher;
            //ParametersManagement.Init();
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var aa = new ParameterPannelControl();
            RootGrid1.Children.Add(aa);
        }

        private void ButtonBase1_OnClick(object sender, RoutedEventArgs e)
        {
            var aa = new ParameterPannelControl();
            RootGrid2.Children.Add(aa);
        }
    }
}
