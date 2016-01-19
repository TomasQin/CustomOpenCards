using System.Windows.Controls;
using XmlFileTransferHandle.XmlEntitys;

namespace CustomUI.Entitys
{
    public class ParameterItemEntity
    {
        public ParameterItemEntity(string displayName, Control control,Param paramItem )
        {
            Control = control;
            DisplayName = displayName;
            ParamItem = paramItem;
        }

        public Control  Control { get; set; }

        public string DisplayName { get; set; }

        public Param ParamItem { get; set; }
        
    }
}
