using System.Collections;
using System.Globalization;
using System.IO;
using System.Resources;

namespace TestProject
{
    //测试提交
    public class ImageManager
    {
        public void Open()
        {
            string resourceName = GetType().Assembly.GetName().Name + ".g";

            ResourceManager mgr = new ResourceManager(resourceName, GetType().Assembly);

            using (ResourceSet set = mgr.GetResourceSet(CultureInfo.CurrentCulture, true, true))
            {

                foreach (DictionaryEntry each in set)
                {

                    if (".png" == Path.GetExtension(each.Key.ToString()).ToLower())
                    {

                        // 得到所有jpg格式的图片

                    }

                }

            }
        }
    }
}
