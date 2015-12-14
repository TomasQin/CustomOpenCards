using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace TestProject
{
    /// <summary>
    /// Interaction logic for StarControl.xaml
    /// </summary>
    public partial class StarControl 
    {
        public StarControl()
        {
            InitializeComponent();
            Loaded += StarControl_Loaded;
        }

        void StarControl_Loaded(object sender, RoutedEventArgs e)
        {
            var sb = Resources["HideGridStoryboard"] as Storyboard;
            if (sb == null) return;
            sb.Begin();


          
        }
    }
}
