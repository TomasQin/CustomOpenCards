using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
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
            Savebtn.IsEnabled = false;
            Refreshbtn.IsEnabled = false;
            _parametersManagement = new ParametersManagement();
            _parametersManagement.Init(Dispatcher);
            _parametersManagement.DataLoadedEventHandler += _parametersManagement_DataLoadedEventHandler;
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

        private void _parametersManagement_DataLoadedEventHandler(object sender, EventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                Savebtn.IsEnabled = true;
                Refreshbtn.IsEnabled = true;
            }));

        }

        private void SaveBtn_OnClick(object sender, RoutedEventArgs e)
        {
            _parametersManagement.SaveParameter();
        }

    }
}
