using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace TestProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        #region Field

        private bool _isDragDropInEffect;
        private Point _pos;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in canvas2.Children)
            {
                var control = item as Rectangle;
                if (control == null) continue;

                control.MouseMove += Element_MouseMove;
                control.MouseLeftButtonDown += Element_MouseLeftButtonDown;
                control.MouseLeftButtonUp += control_MouseLeftButtonUp;
            }
        }

        void control_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDragDropInEffect = false;
            FrameworkElement fEle = sender as FrameworkElement;
            if (fEle == null) return;
            fEle.ReleaseMouseCapture();
        }

        private void Element_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragDropInEffect)
            {
                FrameworkElement currEle = sender as FrameworkElement;
                if (currEle == null) return;
                double xPos = e.GetPosition(null).X - _pos.X + (double)currEle.GetValue(Canvas.LeftProperty);
                double yPos = e.GetPosition(null).Y - _pos.Y + (double)currEle.GetValue(Canvas.TopProperty);
                currEle.SetValue(Canvas.LeftProperty, xPos);
                currEle.SetValue(Canvas.TopProperty, yPos);
                _pos = e.GetPosition(null);
            }
        }

        private void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement fEle = sender as FrameworkElement;
            if (fEle == null) return;
            _isDragDropInEffect = true;
            _pos = e.GetPosition(null);
            fEle.CaptureMouse();

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
