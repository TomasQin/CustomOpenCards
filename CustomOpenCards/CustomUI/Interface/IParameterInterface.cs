using XmlFileTransferHandle.XmlEntitys;

namespace CustomUI.Interface
{
    public interface IParameterBasicInterface
    {
        Param ParamItem { get; set; }

        void ObtainParameters(Param item);

        void SaveParameter();

        void InitData();
    }
}
