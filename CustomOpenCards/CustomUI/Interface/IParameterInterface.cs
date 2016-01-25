using XmlFileTransferHandle.XmlEntitys;

namespace CustomUI.Interface
{
    public interface IParameterBasicInterface
    {
        Param ParamItem { get; set; }

        void SaveParameter();

        void InitData();
    }
}
