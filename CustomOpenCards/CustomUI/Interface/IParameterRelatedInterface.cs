using System.Collections.Generic;

namespace CustomUI.Interface
{
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
