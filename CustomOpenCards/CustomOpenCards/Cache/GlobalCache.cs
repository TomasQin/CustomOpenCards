using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomOpenCards.Entity;
using CustomOpenCards.BusinessLogic.ProbabilityControl;

namespace CustomOpenCards.Cache
{
    /// <summary>
    /// 全局缓存
    /// </summary>
    public class GlobalCache
    {
        private static GlobalCache _instance = new GlobalCache();
        public static GlobalCache Instance
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

        public GlobalCache()
        {
            PackageNumber = new Dictionary<PackageType, int>();
        }

        // Methods
        public Uri MakePackUri(string relativeFile)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("/" + AssemblyShortName + ";component/" + relativeFile);
            return new Uri(builder.ToString(), UriKind.RelativeOrAbsolute);
        }

        // Properties
        private string _assemblyShortName;
        public string AssemblyShortName
        {
            get
            {
                if (_assemblyShortName == null)
                {
                    _assemblyShortName = typeof(GlobalCache).Assembly.ToString().Split(new char[] { ',' })[0];
                }
                return _assemblyShortName;
            }
        }

        /// <summary>
        /// 记录了当前已购买开包的数量
        /// </summary>
        public Dictionary<PackageType, int> PackageNumber { get; set; }

        /// <summary>
        /// 把卡包的数量置为0
        /// </summary>
        private void InitPackageNumber()
        {
            PackageNumber.Clear();
            foreach (PackageType item in Enum.GetValues(typeof(PackageType)))
            {
                PackageNumber.Add(item, 0);
            }
        }

        public CardTypeProb CurrentCardTypeProb { get; set; }
    }
}
