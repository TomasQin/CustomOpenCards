using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomOpenCards.Entity;

namespace CustomOpenCards.BusinessLogic.ProbabilityControl
{
    public class MoreLegendProb : CardTypeProb
    {
        #region [Field]
        private static MoreLegendProb _instance = new MoreLegendProb();
        #endregion

        public static MoreLegendProb Instance
        {
            get
            {
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        public MoreLegendProb()
        {
            prob = new float[]{
                0.830f,
                0.120f,
                0.040f,
                0.010f
            };
        }
    }
}
