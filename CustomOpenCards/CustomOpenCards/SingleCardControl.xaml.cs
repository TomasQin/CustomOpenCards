using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using CustomOpenCards.Entity;

namespace CustomOpenCards
{
    /// <summary>
    /// Interaction logic for SingleCardControl.xaml
    /// </summary>
    public partial class SingleCardControl
    {
        #region [Field]
        public event EventHandler FlipEvent;
        private bool _isFliped;
        private Storyboard _flipSb;
        #endregion

        public SingleCardControl(Card card)
        {
            InitializeComponent();

            CardInfoGrid.Opacity = 0;
            CardInfo.Text = card.DisplayCategory;
            GridEffect.Color = card.CardColor;
            if (card.CardQualityType == CardQualityType.Normal)
            {
                GridEffect.BlurRadius = 0;
                XFrontGrid.Background = new ImageBrush(card.CardImage);
                FrontImage.Visibility = Visibility.Collapsed;
            }
            else
            {
                FrontImage.Source = card.CardImage;
                FrontImage.Visibility = Visibility.Visible;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _flipSb = FindResource("fanzhuan") as Storyboard;
        }

        /// <summary>
        /// 点击翻转效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (BackGridScaleTransform.ScaleX != 0)
            {
                GridEffect.Opacity = 1;
                _flipSb.Begin();
                _isFliped = true;
                FlipEvent(sender, e);
            }
        }

        /// <summary>
        /// 鼠标移入时的效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!_isFliped)
            {
                ScaleEasingAnimation(MyGrid, 1, 1.05);
                OpacityAnimation(GridEffect, 0, 1);
            }
        }

        /// <summary>
        /// 鼠标移出时的效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!_isFliped)
            {
                ScaleEasingAnimation(MyGrid, 1.05, 1);
                OpacityAnimation(GridEffect, 1, 0);
            }
        }

        #region 动画函数
        public void ScaleEasingAnimation(FrameworkElement element, double from, double to)
        {
            ScaleTransform scale = new ScaleTransform();
            element.RenderTransform = scale;
            element.RenderTransformOrigin = new Point(0.5, 0.5);//定义圆心位置
            DoubleAnimation scaleAnimation = new DoubleAnimation()
            {
                From = from,                                 //起始值
                To = to,                                     //结束值
                //EasingFunction = easing,                    //缓动函数
                Duration = new TimeSpan(0, 0, 0, 0, 200)    //动画播放时间
            };
            AnimationClock clock = scaleAnimation.CreateClock();
            scale.ApplyAnimationClock(ScaleTransform.ScaleXProperty, clock);
            scale.ApplyAnimationClock(ScaleTransform.ScaleYProperty, clock);
        }

        private void OpacityAnimation(DropShadowEffect gridEffect, int from, int to)
        {
            DoubleAnimation scaleAnimation = new DoubleAnimation
            {
                From = from,                                 //起始值
                To = to,                                     //结束值
                //EasingFunction = easing,                    //缓动函数
                Duration = new TimeSpan(0, 0, 0, 0, 600)    //动画播放时间
            };
            AnimationClock clock = scaleAnimation.CreateClock();
            gridEffect.ApplyAnimationClock(DropShadowEffect.OpacityProperty, clock);
        }
        #endregion
    }

}
