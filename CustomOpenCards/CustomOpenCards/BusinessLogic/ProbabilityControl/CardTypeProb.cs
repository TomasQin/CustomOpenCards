using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomOpenCards.Entity;

namespace CustomOpenCards.BusinessLogic.ProbabilityControl
{
    public abstract class CardTypeProb
    {
        protected float[] prob;

        protected Random ran = new Random();

        public virtual List<Card> GetAllCards(PackageType type)
        {
            var cardTypeList = CardQualityArithmetic.GetCardsType(prob);
            if (!cardTypeList.Exists(p => (int)p > 0))
            {
                var index = ran.Next(0, 5);
                cardTypeList[index] = CardQualityType.Rare;
            }

            var cardList = new List<Card>();
            foreach (var item in cardTypeList)
            {
                var card=CardContentArithmetic.DataSource[type].GetRandomCard(item);
                cardList.Add(card);
            }
            return cardList;
        }
    }
}
