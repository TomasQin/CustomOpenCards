using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomOpenCards.Entity;
using CustomOpenCards.BusinessLogic.ProbabilityControl;

namespace CustomOpenCards.BusinessLogic
{
    /// <summary>
    /// 得到卡牌的品质，（普通，稀有，史诗，传说）
    /// </summary>
    public static class CardQualityArithmetic
    {
        private static Random ran = new Random();

        public static List<CardQualityType> GetCardsType(float[] prob, int cardNumber = 5)
        {
            List<CardQualityType> cardList = new List<CardQualityType>();
            for (int i = 0; i < cardNumber; i++)
            {
                cardList.Add(Get(ran, prob));
            }
            return cardList;
        }

        /// <summary>
        /// 当前卡牌的类型
        /// </summary>
        /// <param name="prob">各卡牌的概率</param>
        /// <returns>当前卡牌的类型</returns>
        private static CardQualityType Get(Random r, float[] prob)
        {
            int result = 0;
            CardQualityType type;
            int n = (int)(prob.Sum() * 1000);           //计算概率总和，放大1000倍

            float x = (float)r.Next(0, n) / 1000;       //随机生成0~概率总和的数字

            for (int i = 0; i < prob.Count(); i++)
            {
                float pre = prob.Take(i).Sum();         //区间下界
                float next = prob.Take(i + 1).Sum();    //区间上界
                if (x >= pre && x < next)               //如果在该区间范围内，就返回结果退出循环
                {
                    result = i;
                    break;
                }
            }
            switch (result)
            {
                case 1:
                    type = CardQualityType.Rare;
                    break;
                case 2:
                    type = CardQualityType.Epic;
                    break;
                case 3:
                    type = CardQualityType.Legend;
                    break;
                case 0:
                default:
                    type = CardQualityType.Normal;
                    break;
            }
            return type;
        }
    }
}
