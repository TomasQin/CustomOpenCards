using System;
using System.Windows;
using System.Windows.Media.Animation;
using CustomOpenCards.BusinessLogic.ProbabilityControl;
using CustomOpenCards.Entity;

namespace CustomOpenCards
{
    /// <summary>
    /// 水平显示5张等待翻转的卡牌
    /// </summary>
    public partial class ShowCardsHorizontal
    {
        #region [Field]
        private readonly CardTypeProb _localCardTypeProb;

        private readonly PackageType _localPackageType;

        public event EventHandler CloseEvent;

        private int _flipCount;

        #endregion

        public ShowCardsHorizontal(CardTypeProb cardTypeProb, PackageType type)
        {
            InitializeComponent();
            _localCardTypeProb = cardTypeProb;
            _localPackageType = type;
            Loaded += ShowCards_Horizontal_Loaded;
        }


        void ShowCards_Horizontal_Loaded(object sender, RoutedEventArgs e)
        {
            FinishBtn.Opacity = 0;
            _flipCount = 0;
            var cardList = _localCardTypeProb.GetAllCards(_localPackageType);
            for (int i = 0; i < 5; i++)
            {
                var control = new SingleCardControl(cardList[i]) {Margin = new Thickness(20, 12, 20, 12)};
                control.FlipEvent += control_FlipEvent;
                CardListStackPanel.Children.Add(control);
            }
        }

        void control_FlipEvent(object sender, EventArgs e)
        {
            _flipCount++;
            if (_flipCount == 5)
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
