using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CustomOpenCards.Cache;
using CustomOpenCards.BusinessLogic.ProbabilityControl;
using System.IO;
using CustomOpenCards.BusinessLogic;
using System.Reflection;
using CustomOpenCards.Entity;

namespace CustomOpenCards
{
    /// <summary>
    /// Interaction logic for CardShopPage.xaml
    /// </summary>
    public partial class CardShopPage : Page
    {
        public CardShopPage()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(CardShopPage_Loaded);
        }

        void CardShopPage_Loaded(object sender, RoutedEventArgs e)
        {
            CardContentArithmetic.Init();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CardTypeProb cardTypeProb;
            PackageType packageType;
            int result = 0;
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
