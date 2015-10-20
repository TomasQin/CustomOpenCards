using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomOpenCards.Entity;
using System.IO;
using System.Windows;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CustomOpenCards.Cache;
using System.Resources;
using System.Globalization;
using System.Collections;


namespace CustomOpenCards.BusinessLogic
{
    public static class CardContentArithmetic
    {
        public static Dictionary<PackageType, PackageQualityCache> DataSource = new Dictionary<PackageType, PackageQualityCache>();

        public static void Init()
        {
            DataSource.Add(PackageType.CG, new PackageQualityCache());
            DataSource.Add(PackageType.TGT, new PackageQualityCache());
            GetAllCardsFromResource();
        }

        /// <summary>
        /// 从文件路径中加载所有的图片,暂时不用
        /// </summary>
        /// <param name="packageType"></param>
        /// <returns></returns>
        private static List<Card> GetAllCards(string packageType)
        {
            var cardList = new List<Card>();

            foreach (OccupationalCategory category in Enum.GetValues(typeof(OccupationalCategory)))
            {
                foreach (CardQualityType item in Enum.GetValues(typeof(CardQualityType)))
                {
                    var folderPath = string.Format("..//..//Images//{0}//{1}//{2}", packageType, category.ToString(), item.ToString());
                    DirectoryInfo TheFolder = new DirectoryInfo(folderPath);

                    if (TheFolder.Exists)
                    {
                        foreach (var file in TheFolder.GetFiles())
                        {
                            var uri = string.Format("pack://application:,,,/Images/{0}/{1}/{2}/{3}", packageType, category.ToString(), item.ToString(), file.Name);
                            var image = new BitmapImage(new Uri(uri, UriKind.Absolute));
                            cardList.Add(new Card()
                            {
                                CardImage = image,
                                CardName = file.Name,
                                CardQualityType = item,
                                OccupationalCategory = category,
                            });
                        }
                    }
                }

            }
            return cardList;
        }

        private static void GetAllCardsFromResource()
        {
            string resourceName = GlobalCache.Instance.AssemblyShortName + ".g";

            ResourceManager mgr = new ResourceManager(resourceName, typeof(CardContentArithmetic).Assembly);

            using (ResourceSet set = mgr.GetResourceSet(CultureInfo.CurrentCulture, true, true))
            {
                foreach (DictionaryEntry each in set)
                {
                    // 得到所有png格式的图片
                    if (Path.GetExtension(each.Key.ToString()).ToLower() != ".png") continue;

                    var cardItem = CardMatch(each);
                    if (cardItem == null) continue;
                    DataSource[cardItem.PackageType].AddCard(cardItem);
                }
            }
        }

        private static Card CardMatch(DictionaryEntry each)
        {
            //目标文件的格式为 "images/cg/worrior/normal/common_504.png";
            string[] s = each.Key.ToString().Split(new char[] { '/' });
            if (s.Count() == 5)
            {
                var currentCard = new Card();

                currentCard.CardName = s[4];
                currentCard.CardQualityType = EnumMatch<CardQualityType>(s[3]);
                currentCard.OccupationalCategory = EnumMatch<OccupationalCategory>(s[2]);
                currentCard.PackageType = EnumMatch<PackageType>(s[1]);

                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = (each.Value as UnmanagedMemoryStream);
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                currentCard.CardImage = image;

                return currentCard;
            }
            else
            {
                return null;
            }

        }

        private static T EnumMatch<T>(string key)
        {
            foreach (T item in Enum.GetValues(typeof(T)))
            {
                if (item.ToString().ToLower() == key.ToLower())
                {
                    return item;
                }
            }
            return default(T);
        }
    }

    public class PackageQualityCache
    {
        public List<Card> LegendCardList { get; set; }
        public List<Card> EpicCardList { get; set; }
        public List<Card> RareCardList { get; set; }
        public List<Card> NormalCardList { get; set; }
        private Random ran = new Random();

        public PackageQualityCache()
        {
            LegendCardList = new List<Card>();
            EpicCardList = new List<Card>();
            RareCardList = new List<Card>();
            NormalCardList = new List<Card>();
        }

        public void AddCard(Card item)
        {
            switch (item.CardQualityType)
            {
                case CardQualityType.Legend:
                    LegendCardList.Add(item);
                    break;
                case CardQualityType.Epic:
                    EpicCardList.Add(item);
                    break;
                case CardQualityType.Rare:
                    RareCardList.Add(item);
                    break;
                case CardQualityType.Normal:
                    NormalCardList.Add(item);
                    break;
            }

        }

        public Card GetRandomCard(CardQualityType type)
        {
            Card card = null;
            int index = 0;
            switch (type)
            {
                case CardQualityType.Legend:
                    index = ran.Next(0, LegendCardList.Count - 1);
                    card = LegendCardList[index];
                    break;
                case CardQualityType.Epic:
                    index = ran.Next(0, EpicCardList.Count - 1);
                    card = EpicCardList[index];
                    break;
                case CardQualityType.Rare:
                    index = ran.Next(0, RareCardList.Count - 1);
                    card = RareCardList[index];
                    break;
                case CardQualityType.Normal:
                    index = ran.Next(0, NormalCardList.Count - 1);
                    card = NormalCardList[index];
                    break;
            }
            return card;
        }

    }

}
