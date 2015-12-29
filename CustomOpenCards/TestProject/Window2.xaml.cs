using System;
using System.Collections.Generic;
using System.IO;
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
using Win32Hook;
using XmlFileTransferHandle;
using XmlFileTransferHandle.XmlEntitys.MenuDefEntity;

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
            var hook = new Hook();

            hook.Start();
            //var aaa = "abcdefghj";

            //var list = new List<string>();
            //if (list.Any(item => aaa.Contains(item)))
            //{
            //    return ;
            //}

            //using (FileStream fs = new FileStream("MenuDef.xml", FileMode.Open))
            //{
            //    StreamReader sr = new StreamReader(fs, Encoding.Default);
            //    String xmlInfo = sr.ReadToEnd();
            //    sr.Close();
            //    var root = XmlSerialize.DeserializeXml<Tabs>(xmlInfo);
            //}
        }
    }
}
