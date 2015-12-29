using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace TestProject
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1
    {
        #region Field

        private bool _isDragDropInEffect;
        private Point _pos;

        private List<StarControl> _list;
        private Task _cleanTask;

        private readonly Random _ran = new Random();
        private double[] _normalDistributionArray;
        #endregion

        public Window1()
        {
            InitializeComponent();
            Loaded += Window1_Loaded;
        }

        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            _normalDistributionArray = NormalDistribution();

            _list = new List<StarControl>();
            _cleanTask = new Task(CleanStars);
            _cleanTask.Start();

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
                var isShowLeft = e.GetPosition(null).X - _pos.X > 0;
                double yPos = e.GetPosition(null).Y - _pos.Y + (double)currEle.GetValue(Canvas.TopProperty);
                currEle.SetValue(Canvas.LeftProperty, xPos);
                currEle.SetValue(Canvas.TopProperty, yPos);
                GenerateStars(xPos, yPos, isShowLeft);
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

        private void GenerateStars(double x, double y, bool isShowLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                var offsetX = _ran.Next(2, 15);
                var control = new StarControl
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top
                };
                _list.Add(control);
                if (isShowLeft)
                {
                    control.SetValue(Canvas.LeftProperty, x - offsetX);
                }
                else
                {
                    control.SetValue(Canvas.LeftProperty, x + offsetX + 100);
                }
                //control.SetValue(Canvas.LeftProperty, x + offsetX + 100);
                control.SetValue(Canvas.TopProperty, y + _normalDistributionArray[i]);
                RootCanvas.Children.Add(control);
            }
        }

        private void CleanStars()
        {
            while (true)
            {
                _cleanTask.Wait(5000);
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    foreach (var item in _list.Where(p => p.Opacity == 0))
                    {
                        RootCanvas.Children.Remove(item);
                    }
                }));
            }
        }

        internal static double[] NormalDistribution()
        {
            double E = 60;//正态分布期望
            double D = 500;//正态分布方差
            double[] normalDistributionArr = new double[30];
            Random u1 = new Random();
            for (int i = 0; i < 30; i++)
            {
                double? result = GetNormalDistribution(u1.NextDouble(), u1.NextDouble(), E, D);
                if (result != null)
                    normalDistributionArr[i] = (double)result;
            }
            return normalDistributionArr;
        }

        /// <summary>
        /// 返回正态分布的值
        /// </summary>
        /// <param name="u1">第一个均匀分布值</param>
        /// <param name="u2">第二个均匀分布值</param>
        /// <param name="e">正态期望</param>
        /// <param name="d">正态方差</param>
        /// <returns>分布值或者null</returns>
        private static double? GetNormalDistribution(double u1, double u2, double e, double d)
        {
            double? result = null;
            try
            {
                result = e + Math.Sqrt(d) * Math.Sqrt((-2) * (Math.Log(u1) / Math.Log(Math.E))) * Math.Cos(2 * Math.PI * u2);
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }
    }
}
