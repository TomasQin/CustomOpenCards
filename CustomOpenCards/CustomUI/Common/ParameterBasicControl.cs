using System.Windows;
using System.Windows.Controls;
using CustomUI.Interface;
using XmlFileTransferHandle.XmlEntitys;

namespace CustomUI.Common
{
    public class ParameterBasicControl : UserControl, IParameterBasicInterface
    {
        public Param ParamItem { get; set; }

        public virtual void ObtainParameters(Param item)
        {
            ParamItem = item;
        }

        public virtual void SaveParameter()
        {

        }

        public virtual void InitData()
        {

        }

        public virtual void ReLoadData(object param)
        {

        }

    }
}
