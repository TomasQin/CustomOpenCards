using System.Windows;
using System.Windows.Controls;
using CustomUI.AuthorSelect;
using CustomUI.Common;
using CustomUI.DateTimePickerGroup;
using XmlFileTransferHandle;
using XmlFileTransferHandle.XmlEntitys;
using XmlFileTransferHandle.XmlEnum;

namespace PanelBusniessLogic
{
    public class ParametersManagement
    {
        public FrameworkElement Init()
        {
            var panel = new StackPanel();
            var root = XmlSerialize.GetSerializeResult<Params>(@"XmlFiles\Paramdef.xml");

            foreach (var item in root.Param)
            {
                IParameterInterface control = null;

                //todo 这里可用反射来实现解耦，具体验证一下性能问题
                switch (item.Type)
                {
                    case ParamControlType.AuthorSelectControl:
                        control = new AuthorSelectControl();
                        break;
                    case ParamControlType.DateTimePickerGroup:
                        control = new DateTimePickerGroupControl();
                        break;
                }
                if (control == null) continue;
                var initControl = control.GenerateControlInstance(item);
                panel.Children.Add(initControl);
            }
            return panel;
        }
    }
}
