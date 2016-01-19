using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using CustomUI.Common;
using XmlFileTransferHandle;
using XmlFileTransferHandle.XmlEntitys;
using XmlFileTransferHandle.XmlEnum;
using System;
using System.Threading;
using CustomUI.Controls.AuthorSelect;
using CustomUI.Controls.DateTimePickerGroup;

namespace PanelBusniessLogic
{
    public class ParametersManagement
    {
        public static Dispatcher CurrentUiDispatcher;
        readonly List<IParameterInterface> _controlList = new List<IParameterInterface>();

        public FrameworkElement Init()
        {
            var panel = new StackPanel();
            var root = XmlSerialize.GetSerializeResult<Params>(@"XmlFiles\Paramdef.xml");

            foreach (var item in root.Param)
            {
                UserControl control = null;
                switch (item.Type)
                {
                    case ParamControlType.AuthorSelectControl:
                        control = new AuthorSelectControl();
                        break;
                    case ParamControlType.DateTimePickerGroupControl:
                        control = new DateTimePickerGroupControl();
                        break;
                }
                if (control == null) continue;
                var baseControl = (IParameterInterface)control;
                baseControl.ObtainParameters(item);
                _controlList.Add(baseControl);
                panel.Children.Add(control);
            }

            var th = new Thread(() =>
             {
                 foreach (var item in _controlList)
                 {
                     Thread.Sleep(500);
                     var item1 = item;
                     CurrentUiDispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                     {
                         item1.InitData();
                     }));
                 }
             });
            th.Start();
            return panel;
        }

        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <typeparam name="T">要创建对象的类型</typeparam>
        /// <param name="assemblyName">类型所在程序集名称</param>
        /// <param name="nameSpace">类型所在命名空间</param>
        /// <param name="className">类型名</param>
        /// <returns></returns>
        public static T CreateInstance<T>(string assemblyName, string nameSpace, string className)
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

    }
}
