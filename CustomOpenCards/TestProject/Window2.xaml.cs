using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

using XmlFileTransferHandle.XmlEntitys;
using PanelBusniessLogic;

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
           
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var t1 = DateTime.Now;
            var aa = new ParametersManagement();
            var ss = aa.Init();
          
           RootGrid.Children.Add(ss);
            var t2 = DateTime.Now;

            var tpSpan= t1.CompareTo(t2);
        }
    }
}
