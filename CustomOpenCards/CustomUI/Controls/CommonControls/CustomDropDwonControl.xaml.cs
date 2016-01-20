using System.Windows;

namespace CustomUI.Controls.CommonControls
{
    /// <summary>
    /// 因为它的CustomContent 里的usercontrol不能取名字（x:name=“contrlname”之类的），
    /// 如果需要这个控件来做基类就只有2种办法
    /// 1，用mvvm的binding的方式不写名字
    /// 2，用附加属性来获得这个名字
    /// </summary>
    public partial class CustomDropDwonControl
    {
        public CustomDropDwonControl()
        {
            InitializeComponent();
        }

        private void ShowPopupBtn_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedPopup.IsOpen = true;
        }

        public object CustomContent
        {
            get { return GetValue(CustomContentProperty); }
            set { SetValue(CustomContentProperty, value); }
        }

        public static readonly DependencyProperty CustomContentProperty =
            DependencyProperty.Register("CustomContent", typeof(object), typeof(CustomDropDwonControl),
            new PropertyMetadata(OnCustomContentChange));

        private static void OnCustomContentChange(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var currentControl = obj as CustomDropDwonControl;
            if (currentControl == null) return;
            var temp = e.NewValue as UIElement;
            if (temp == null) return;
            currentControl.Border.Child=temp;
        }

        public string DisplayValue
        {
            get { return (string)GetValue(DisplayValueProperty); }
            set { SetValue(DisplayValueProperty, value); }
        }

        public static readonly DependencyProperty DisplayValueProperty =
            DependencyProperty.Register("DisplayValue", typeof(string), typeof(CustomDropDwonControl),
            new PropertyMetadata(OnDisplayValueChange));

        private static void OnDisplayValueChange(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var currentControl = obj as CustomDropDwonControl;
            if (currentControl == null) return;
            var temp = e.NewValue as string;
            if (temp == null) return;
            currentControl.ContentLabel.Content = temp;
        }

        public double PopupHeight
        {
            get { return (double)GetValue(PopupHeightProperty); }
            set { SetValue(PopupHeightProperty, value); }
        }

        public static readonly DependencyProperty PopupHeightProperty =
            DependencyProperty.Register("PopupHeight", typeof(double), typeof(CustomDropDwonControl),
            new PropertyMetadata(OnPopupHeightChange));

        private static void OnPopupHeightChange(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var currentControl = obj as CustomDropDwonControl;
            if (currentControl == null) return;
            double temp;
            double.TryParse(e.NewValue.ToString(), out temp);
            if (temp > 0)
            {
                currentControl.SelectedPopup.Height = temp;
            }
        }

        public void ClosePopup()
        {
            SelectedPopup.IsOpen = false;
        }
    }
}
