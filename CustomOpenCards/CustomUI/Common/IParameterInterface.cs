using System.Collections.Generic;
using XmlFileTransferHandle.XmlEntitys;

namespace CustomUI.Common
{
    public interface IParameterBasicInterface
    {
        Param ParamItem { get; set; }

        void ObtainParameters(Param item);

        void SaveParameter();

        void InitData();

    }

    /// <summary>
    /// 用来实现控件直接的联动
    /// </summary>
    public interface IParameterRelatedInterface : IParameterBasicInterface
    {
        void ReLoadData();

        string GetCurrentData();

        List<IParameterRelatedInterface> ParentParameters { get; set; }

        List<IParameterRelatedInterface> ChilderenParameter { get; set; }
    }
}
