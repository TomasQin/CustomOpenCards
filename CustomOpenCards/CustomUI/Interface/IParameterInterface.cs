using System.Threading.Tasks;
using XmlFileTransferHandle.XmlEntitys;

namespace CustomUI.Interface
{
    public interface IParameterBasicInterface
    {
        Param ParamItem { get; set; }

        void SaveParameter();

        Task InitData();
    }
}
