using System.Windows;
using XmlFileTransferHandle.XmlEntitys;

namespace CustomUI.Common
{
    public interface IParameterInterface
    {
        FrameworkElement GenerateControlInstance(Param param);

        void SaveParameter();
    }
}
