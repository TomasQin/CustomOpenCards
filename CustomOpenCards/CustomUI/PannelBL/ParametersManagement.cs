using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;
using CustomUI.Common;
using CustomUI.Controls;
using CustomUI.Entitys;
using CustomUI.Interface;
using XmlFileTransferHandle;
using XmlFileTransferHandle.XmlEntitys;
using XmlFileTransferHandle.XmlEnum;

namespace CustomUI.PannelBL
{
    public class ParametersManagement
    {
        #region [Field]
        private readonly List<IParameterBasicInterface> _currentControlList;
        private readonly Dictionary<string, IParameterBasicInterface> _totalControlList;
        private readonly Thread _initControlDataThread;
        private Dispatcher _currentUiDispatcher;
        private Params _paramsRoot;
        #endregion

        #region[Event]
        public event EventHandler DataLoadedEventHandler;
        private void OnDataLoadedEventHandler()
        {
            var handler = DataLoadedEventHandler;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
        #endregion

        public ParametersManagement()
        {
            _currentControlList = new List<IParameterBasicInterface>();
            _totalControlList = new Dictionary<string, IParameterBasicInterface>();

            _initControlDataThread = new Thread(() =>
            {
                //先初始化所有控件的datasource
                foreach (var item in _currentControlList)
                {
                    Thread.Sleep(500);
                    var item1 = item;

                    _currentUiDispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        item1.InitData();
                    }));
                }
               //关联控件再根据被关联的控件默认值去取默认显示的值
                foreach (var item in _currentControlList)
                {
                    Thread.Sleep(500);
                    var relatedControl = item as IParameterRelatedInterface;
                    if (relatedControl != null)
                    {
                        _currentUiDispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            relatedControl.ReLoadData();
                        }));
                    }
                }
                OnDataLoadedEventHandler();
            });
        }

        /// <summary>
        /// 创建在配置文件里所有的参数控件的实例
        /// </summary>
        /// <param name="currentUiDispatcher"></param>
        public void Init(Dispatcher currentUiDispatcher)
        {
            _currentUiDispatcher = currentUiDispatcher;
            _paramsRoot = XmlSerialize.GetSerializeResult<Params>(@"XmlFiles\Paramdef.xml");
            foreach (var item in _paramsRoot.Param)
            {
                IParameterBasicInterface control = null;
                switch (item.Type)
                {
                    case ParamControlType.AuthorSelectControl:
                        control = new AuthorSelectControl();
                        break;
                    case ParamControlType.DateTimePickerGroupControl:
                        control = new DateTimePickerGroupControl();
                        break;
                    case ParamControlType.ParameterCombobox:
                        control = new ParameterCombobox();
                        break;
                }
                if (control == null) continue;

                control.ObtainParameters(item);
                _totalControlList.Add(item.ID, control);
            }
        }

        /// <summary>
        /// 后台线程去取参数设置面板的数据
        /// </summary>
        public void InitControlData()
        {
            _initControlDataThread.Start();
        }

        /// <summary>
        /// 保存当前模板对应的参数
        /// </summary>
        public void SaveParameter()
        {
            //如果有效率问题也可以改为后台线程进行保存
            foreach (var item in _currentControlList)
            {
                item.SaveParameter();
            }
        }

        /// <summary>
        /// 根据当前模板的高级属性来加载对应显示的参数
        /// </summary>
        /// <returns></returns>
        public List<ParameterItemEntity> GetCurrentControls()
        {
            var list = new List<ParameterItemEntity>();

            if (_paramsRoot.ShowAllParams)
            {
                foreach (var item in _totalControlList)
                {
                    var control = item.Value;
                    _currentControlList.Add(control);
                    BuildControlsRelationship(control);
                    var uiControl = control as Control;
                    list.Add(new ParameterItemEntity(control.ParamItem.Caption, uiControl, control.ParamItem));
                }
            }
            else
            {
                var advancedPropertyDic = AdvancedPropertyHelp.GetAllProerty();

                _currentControlList.Clear();
                foreach (var item in advancedPropertyDic)
                {
                    if (_totalControlList.ContainsKey(item.Key))
                    {
                        var control = _totalControlList[item.Key];
                        _currentControlList.Add(control);
                        BuildControlsRelationship(control);
                        var uiControl = control as Control;
                        list.Add(new ParameterItemEntity(control.ParamItem.Caption, uiControl, control.ParamItem));
                    }
                }
            }

            return list;
        }

        #region [Private Method]
        private void BuildControlsRelationship(IParameterBasicInterface control)
        {
            var parentLinkInfo = AnalysisLinkInfo(control.ParamItem.ParentID);
            var childrenLinkInfo = AnalysisLinkInfo(control.ParamItem.ChildID);
            if (parentLinkInfo != null)
            {
                var relatedcontrol = control as IParameterRelatedInterface;
                if (relatedcontrol != null)
                {
                    foreach (var parentLinkInfoItem in parentLinkInfo)
                    {
                        //todo 把错误写到logo日志里
                        relatedcontrol.ParentParameters.Add(
                            _totalControlList[parentLinkInfoItem.ControlName] as IParameterRelatedInterface);
                    }
                }
            }
            if (childrenLinkInfo != null)
            {
                var relatedcontrol = control as IParameterRelatedInterface;
                if (relatedcontrol != null)
                {
                    foreach (var parentLinkInfoItem in childrenLinkInfo)
                    {
                        relatedcontrol.ChilderenParameter.Add(
                            _totalControlList[parentLinkInfoItem.ControlName] as IParameterRelatedInterface);
                    }
                }
            }
        }

        private List<LinkInfoItem> AnalysisLinkInfo(string info)
        {
            if (string.IsNullOrEmpty(info)) return null;

            return info.Split(',').Select(item => item.Split(':')).Select(temp => new LinkInfoItem(temp[0], temp[1])).ToList();
        }

        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <typeparam name="T">要创建对象的类型</typeparam>
        /// <param name="assemblyName">类型所在程序集名称</param>
        /// <param name="nameSpace">类型所在命名空间</param>
        /// <param name="className">类型名</param>
        /// <returns></returns>
        private static T CreateInstance<T>(string assemblyName, string nameSpace, string className)
        {
            try
            {
                string fullName = nameSpace + "." + className;//命名空间.类型名
                //此为第一种写法
                object ect = Assembly.Load(assemblyName).CreateInstance(fullName);//加载程序集，创建程序集里面的 命名空间.类型名 实例
                return (T)ect;//类型转换并返回
                //下面是第二种写法
                //string path = fullName + "," + assemblyName;//命名空间.类型名,程序集
                //Type o = Type.GetType(path);//加载类型
                //object obj = Activator.CreateInstance(o, true);//根据类型创建实例
                //return (T)obj;//类型转换并返回
            }
            catch
            {
                //发生异常，返回类型的默认值
                return default(T);
            }
        }
        #endregion

    }
}
