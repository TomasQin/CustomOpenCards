using System.Windows;
using System.Windows.Controls;
using CustomUI.PannelBL;

namespace CustomUI.PannelControls
{
    /// <summary>
    /// Interaction logic for ParameterPannelControl.xaml
    /// </summary>
    public partial class ParameterPannelControl
    {
        private ParametersManagement _parametersManagement;

        public ParameterPannelControl()
        {
            InitializeComponent();
            Loaded += ParameterPannelControl_Loaded;
        }

        private void ParameterPannelControl_Loaded(object sender, RoutedEventArgs e)
        {
            _parametersManagement = new ParametersManagement();
            _parametersManagement.Init(Dispatcher);

            foreach (var item in _parametersManagement.GetCurrentControls())
            {
                var control = new ContentControl();

                if (!item.ParamItem.IsCaptionLabelNeeded)
                {
                    control.Template = FindResource("GroupParameterTemplate") as ControlTemplate;
                }
                else
                {
                    control.Template = FindResource("ParameterTemplate") as ControlTemplate;
                }
                control.DataContext = item;
                RootStackPanel.Children.Add(control);
            }
            _parametersManagement.InitControlData();
        }
         
        private void SaveBtn_OnClick(object sender, RoutedEventArgs e)
        {
            _parametersManagement.SaveParameter();
        }

    }
}
