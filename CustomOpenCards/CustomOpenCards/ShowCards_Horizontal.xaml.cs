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
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using CustomOpenCards.Entity;
using CustomOpenCards.BusinessLogic;
using CustomOpenCards.BusinessLogic.ProbabilityControl;
using CustomOpenCards.Cache;

namespace CustomOpenCards
{
    /// <summary>
    /// 水平显示5张等待翻转的卡牌
    /// </summary>
    public partial class ShowCards_Horizontal
    {
        #region [Field]
        private CardTypeProb _localCardTypeProb;

        private PackageType _localPackageType;

        public event EventHandler CloseEvent;

        private int FlipCount = 0;

        #endregion

        public ShowCards_Horizontal(CardTypeProb cardTypeProb, PackageType type)
        {
            InitializeComponent();
            _localCardTypeProb = cardTypeProb;
            _localPackageType = type;
            Loaded += new RoutedEventHandler(ShowCards_Horizontal_Loaded);
        }


        void ShowCards_Horizontal_Loaded(object sender, RoutedEventArgs e)
        {
            FinishBtn.Opacity = 0;
            FlipCount = 0;
            var cardList = _localCardTypeProb.GetAllCards(_localPackageType);
            for (int i = 0; i < 5; i++)
            {
                var control = new SingleCardControl(cardList[i]);
                control.Margin = new Thickness(20, 12, 20, 12);
                control.FlipEvent += new EventHandler(control_FlipEvent);
                CardListStackPanel.Children.Add(control);
            }
        }

        void control_FlipEvent(object sender, EventArgs e)
        {
            FlipCount++;
            if (FlipCount == 5)
            {
                var sb = FindResource("ShowBtn") as Storyboard;
                sb.Begin();

            }
        }

        void Button_Click(object sender, EventArgs e)
        {
            if (FinishBtn.Opacity > 0)
            {
                CloseEvent(sender, e);
            }
        }
    }
}
