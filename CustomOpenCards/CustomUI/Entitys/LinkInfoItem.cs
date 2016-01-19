namespace CustomUI.Entitys
{
    public class LinkInfoItem
    {
        public LinkInfoItem(string controlName, string urlName)
        {
            ControlName = controlName;
            UrlName = urlName;
        }

        public string ControlName { get; set; }

        public string UrlName { get; set; }
    }
}
