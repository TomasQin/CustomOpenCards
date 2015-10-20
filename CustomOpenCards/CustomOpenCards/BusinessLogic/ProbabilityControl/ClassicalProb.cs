using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomOpenCards.Entity;

namespace CustomOpenCards.BusinessLogic.ProbabilityControl
{
    public class ClassicalProb : CardTypeProb
    {
        #region [Field]
        private static ClassicalProb _instance = new ClassicalProb();

        #endregion

        public static ClassicalProb Instance
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

        public ClassicalProb()
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
