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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;
using System.Windows.Markup;
using System.Xml;

namespace TestProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TestGrid.MouseMove += Element_MouseMove;
            TestGrid.MouseLeftButtonDown += Element_MouseLeftButtonDown;
            TestGrid.MouseLeftButtonUp += Element_MouseLeftButtonUp;
            var bb = new TestTest();

        }

        bool isDragDropInEffect = false;
        Point pos = new Point();

        void Element_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragDropInEffect)
            {
                FrameworkElement currEle = sender as FrameworkElement;
                double xPos = e.GetPosition(null).X - pos.X + currEle.Margin.Left;
                double yPos = e.GetPosition(null).Y - pos.Y + currEle.Margin.Top;
                currEle.Margin = new Thickness(xPos, yPos, 0, 0);
                pos = e.GetPosition(null);
            }
        }

        void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            FrameworkElement fEle = sender as FrameworkElement;
            isDragDropInEffect = true;
            pos = e.GetPosition(null);
            fEle.CaptureMouse();
            fEle.Cursor = Cursors.Hand;
        }

        void Element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragDropInEffect)
            {
                FrameworkElement ele = sender as FrameworkElement;
                isDragDropInEffect = false;
                ele.ReleaseMouseCapture();
            }
        }

    }

    public class TestTest : ITestInterface
    {

        public void Add()
        {

        }
    }

    public interface ITestInterface
    {
        void Add();
    }

    public class ServiceHelper
    {
        /// <summary>
        /// 从目录中获取服务实体，根据泛型的完整类名作为key获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>()
        {
            var key = typeof(T).ToString();
            return GetService<T>(key);
        }

        /// <summary>
        /// 从目录中获取服务实体，根据指定key获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetService<T>(string key)
        {
            ////根据key拿到classname
            //object theObj = GetObject(key);
            //if (theObj != null)
            //{
            //    return (T)theObj;
            //}
            return default(T);
        }
    }
}
