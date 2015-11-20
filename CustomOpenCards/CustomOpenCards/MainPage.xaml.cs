using System.Windows;
using CustomOpenCards.Converter;

namespace CustomOpenCards
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CardShopPage());
            HeightWidthConvter.Instance.Convter(ActualHeight, ActualWidth);
        }
    }
}
