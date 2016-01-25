using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using CustomUI.Interface;
using XmlFileTransferHandle.XmlEntitys;

namespace CustomUI.Controls
{
    public class ParameterCombobox : ComboBox, IParameterRelatedInterface
    {
        #region [Field]
        private bool _isInint;
        private List<string> _currentItemsource;

        //测试数据
        private List<string> _firstLevel = new List<string> { "A", "B", "C" };
        private List<string> _stockGrade = new List<string> { "买入", "持有", "卖出" };
        private List<string> _resultList = new List<string>
        {
            "A持有", "A卖出", "A买入",
            "B买入", "B持有", "B卖出" ,
            "C买入", "C持有", "C卖出" 
        };

        #endregion

        #region [Property]

        public Param ParamItem { get; set; }

        private List<IParameterRelatedInterface> _parentParameters = new List<IParameterRelatedInterface>();

        public List<IParameterRelatedInterface> ParentParameters
        {
            get { return _parentParameters; }
            set { _parentParameters = value; }
        }

        private List<IParameterRelatedInterface> _childerenParameter = new List<IParameterRelatedInterface>();

        public List<IParameterRelatedInterface> ChilderenParameter
        {
            get { return _childerenParameter; }
            set { _childerenParameter = value; }
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            SelectedIndex = 0;
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            if (!_isInint) return;

            foreach (var item in ChilderenParameter)
            {
                item.GetRelationData();
            }
        }

        public void ObtainParameters(Param item)
        {
            ParamItem = item;
        }

        public void SaveParameter()
        {

        }

        public void InitData()
        {
            _currentItemsource = GetDataByID(ParamItem.DataSourceID);
            ItemsSource = _currentItemsource;
            _isInint = true;
        }

        public void GetRelationData()
        {
            //当前控件还没有赋初始值，不能进行联动
            //当前控件没有关联控件是，不能进行联动
            if (ItemsSource == null || ParentParameters.Count < 1) return;
            var list = ParentParameters.Select(item => item.GetCurrentData()).ToList();
            //关联控件还没有赋初始值，不能进行联动
            if (list.Contains(string.Empty)) return;

            SelectedItem = GetRelatedData(list);
        }

        public string GetCurrentData()
        {
            //todo 根据对应的类型来返回选中的值
            if (SelectedItem == null)
            {
                return string.Empty;
            }

            return SelectedItem.ToString();
        }

        #region [Private Method]
        private string GetRelatedData(List<string> paramList)
        {
            var key = string.Empty;
            foreach (var item in paramList)
            {
                if (string.IsNullOrEmpty(key))
                {
                    key = item;
                }
                else
                {
                    key = string.Format("{0}{1}", key, item);
                }
            }

            var result = _currentItemsource.FirstOrDefault(p => p == key);
            return result;
        }

        private List<string> GetDataByID(string id)
        {
            switch (id)
            {
                case "A":
                    return _firstLevel;
                case "B":
                    return _stockGrade;
                default:
                    return _resultList;
            }
        }
        #endregion
    }
}
