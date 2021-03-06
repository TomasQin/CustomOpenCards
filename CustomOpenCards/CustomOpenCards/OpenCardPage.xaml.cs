﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CustomOpenCards.BusinessLogic.ProbabilityControl;
using CustomOpenCards.Converter;
using CustomOpenCards.Entity;

namespace CustomOpenCards
{
    /// <summary>
    /// Interaction logic for OpenCardPage.xaml
    /// </summary>
    public partial class OpenCardPage
    {
        public int PackageNumber { get; set; }

        public CardTypeProb CurrentProb { get; set; }

        public PackageType CurrentPackageType { get; set; }

        private int _currentPackageNumber;

        private TextBlock _packNumberTb;

        public OpenCardPage()
        {
            InitializeComponent();
        }

        void OpenCardPage_Loaded(object sender, RoutedEventArgs e)
        {
            EffectivePositionGrid.SetValue(Canvas.LeftProperty, HeightWidthConvter.Instance.CardSlotLeft);
            EffectivePositionGrid.SetValue(Canvas.TopProperty, HeightWidthConvter.Instance.CardSlotTop);
            EffectivePositionGrid.Width = HeightWidthConvter.Instance.CardSlotWdith;
            EffectivePositionGrid.Height = HeightWidthConvter.Instance.CardSlotHeight;

            _currentPackageNumber = PackageNumber;
            for (int i = 0; i < PackageNumber; i++)
            {
                var package = new SingleCardPackageControl();
                package.SetValue(Canvas.LeftProperty, HeightWidthConvter.Instance.PackageLeft);
                package.SetValue(Canvas.TopProperty, HeightWidthConvter.Instance.PackageTop);
                package.Width = HeightWidthConvter.Instance.PackageWdith;
                package.Height = HeightWidthConvter.Instance.PackageHeight;

                package.MouseMove += Element_MouseMove;
                package.MouseLeftButtonDown += Element_MouseLeftButtonDown;
                package.MouseLeftButtonUp += Element_MouseLeftButtonUp;
                LayoutRoot.Children.Add(package);
            }

            _packNumberTb = new TextBlock();
            _packNumberTb.Foreground = new SolidColorBrush(Colors.White);
            var left = HeightWidthConvter.Instance.PackageWdith + HeightWidthConvter.Instance.PackageLeft - 20;
            var top = HeightWidthConvter.Instance.PackageTop - 20;
            _packNumberTb.SetValue(Canvas.LeftProperty, left);
            _packNumberTb.SetValue(Canvas.TopProperty, top);
            _packNumberTb.Text = _currentPackageNumber.ToString();
            LayoutRoot.Children.Add(_packNumberTb);

            BackBtn.Width = HeightWidthConvter.Instance.BackBtnWdith;
            BackBtn.Height = HeightWidthConvter.Instance.CardSlotHeight;

            BackBtn.SetValue(Canvas.LeftProperty, HeightWidthConvter.Instance.BackBtnLeft);
            BackBtn.SetValue(Canvas.TopProperty, HeightWidthConvter.Instance.BackBtnTop);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        #region 卡包拖拽的事件
        bool _isDragDropInEffect;
        Point _pos;

        void Element_MouseMove(object sender, MouseEventArgs e)
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

        void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement fEle = sender as FrameworkElement;
            if (fEle == null) return;
            _isDragDropInEffect = true;
            _pos = e.GetPosition(null);
            fEle.CaptureMouse();

        }

        void Element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement ele = sender as FrameworkElement;
            if (ele == null) return;
            if (_isDragDropInEffect)
            {
                _isDragDropInEffect = false;
                ele.ReleaseMouseCapture();
            }
            var left = (double)ele.GetValue(Canvas.LeftProperty);
            var top = (double)ele.GetValue(Canvas.TopProperty);



            if (left > HeightWidthConvter.Instance.MinLeft &&
                left < HeightWidthConvter.Instance.MaxLeft &&
                top > HeightWidthConvter.Instance.MinTop &&
                top < HeightWidthConvter.Instance.MaxTop)
            {
                ele.SetValue(Canvas.LeftProperty, HeightWidthConvter.Instance.CardSlotLeft + 20);
                ele.SetValue(Canvas.TopProperty, HeightWidthConvter.Instance.CardSlotTop + 20);
                _currentPackageNumber--;
                _packNumberTb.Text = _currentPackageNumber.ToString();
                if (_currentPackageNumber < 2)
                {
                    _packNumberTb.Visibility = Visibility.Collapsed;
                }

                ele.Visibility = Visibility.Collapsed;
                ShowCardsGrid.Width = LayoutRoot.ActualWidth;
                ShowCardsGrid.Height = LayoutRoot.ActualHeight;
                ShowCardsGrid.Visibility = Visibility.Visible;
                var control = new ShowCardsHorizontal(CurrentProb, CurrentPackageType);
                control.CloseEvent += (o, b) => { ShowCardsGrid.Visibility = Visibility.Collapsed; };
                ShowCardsControl.Content = control;
            }
            else
            {
                ele.SetValue(Canvas.LeftProperty, HeightWidthConvter.Instance.PackageLeft);
                ele.SetValue(Canvas.TopProperty, HeightWidthConvter.Instance.PackageTop);
            }
        }
        #endregion

    }
}
