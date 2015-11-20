using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CustomOpenCards.Entity
{
    public class Card
    {
        public BitmapImage CardImage { get; set; }
        public string CardName { get; set; }
        public PackageType PackageType { get; set; }
        public OccupationalCategory OccupationalCategory { get; set; }
        public CardQualityType CardQualityType { get; set; }

        public string DisplayCategory
        {
            get
            {
                string displayValue = string.Empty;
                switch (OccupationalCategory)
                {
                    case OccupationalCategory.Druid:
                        displayValue = "德鲁伊";
                        break;
                    case OccupationalCategory.Hunter:
                        displayValue = "猎人";
                        break;
                    case OccupationalCategory.Mage:
                        displayValue = "法师";
                        break;
                    case OccupationalCategory.Neutral:
                        displayValue = "所有职业";
                        break;
                    case OccupationalCategory.Paladin:
                        displayValue = "圣骑士";
                        break;
                    case OccupationalCategory.Priest:
                        displayValue = "牧师";
                        break;
                    case OccupationalCategory.Rogue:
                        displayValue = "盗贼";
                        break;
                    case OccupationalCategory.Shaman:
                        displayValue = "萨满";
                        break;
                    case OccupationalCategory.Warlock:
                        displayValue = "术士";
                        break;
                    case OccupationalCategory.Worrior:
                        displayValue = "战士";
                        break;
                }
                return displayValue;
            }
        }

        public Color CardColor
        {
            get
            {
                Color cardColor = Colors.Transparent; 
                switch (CardQualityType)
                {
                    case CardQualityType.Legend:
                        cardColor = Colors.Orange;
                        break;
                    case CardQualityType.Epic:
                        cardColor = Colors.Violet;
                        break;
                    case CardQualityType.Rare:
                        cardColor = Colors.Blue;
                        break;
                    case CardQualityType.Normal:
                        cardColor = Colors.Transparent;
                        break;
                }
                return cardColor;
            }
        }

    }
}
