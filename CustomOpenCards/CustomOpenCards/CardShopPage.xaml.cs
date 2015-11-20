using System.Windows;
using CustomOpenCards.BusinessLogic;
using CustomOpenCards.BusinessLogic.ProbabilityControl;
using CustomOpenCards.Entity;

namespace CustomOpenCards
{
    /// <summary>
    /// Interaction logic for CardShopPage.xaml
    /// </summary>
    public partial class CardShopPage 
    {
        public CardShopPage()
        {
            InitializeComponent();
            Loaded += CardShopPage_Loaded;
        }

        void CardShopPage_Loaded(object sender, RoutedEventArgs e)
        {
            CardContentArithmetic.Init();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CardTypeProb cardTypeProb;
            PackageType packageType;
            int result;
            var page = new OpenCardPage();

            switch (TypeCb.SelectedIndex)
            {
                case 1:
                    cardTypeProb = MoreLegendProb.Instance;
                    break;
                default:
                    cardTypeProb = ClassicalProb.Instance;
                    break;
            }

            switch (PackageTypeCb.SelectedIndex)
            {
                case 1:
                    packageType = PackageType.TGT;
                    break;
                default:
                    packageType = PackageType.CG;
                    break;
            }

            int.TryParse(NumberCb.SelectionBoxItem.ToString(), out result);
            if (result != 0)
            {
                page.PackageNumber = result;
                page.CurrentProb = cardTypeProb;
                page.CurrentPackageType = packageType;
                NavigationService.Navigate(page);
            }
        }
    }
}
