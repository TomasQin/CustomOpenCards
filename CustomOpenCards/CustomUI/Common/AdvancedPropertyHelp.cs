using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CustomUI.Common
{
    public class AdvancedPropertyHelp
    {
        public static Dictionary<string,string> GetAllProerty()
        {
            var aa =new Dictionary<string, string>
                {
                    {"AuthorSelect", "Default"},
                    {"DatePickersGroup", "Default"},
                    {"IndustrySelect", "Default"},
                    {"ReportDate", "Default"},
                    {"5", ""},
                    {"6", ""}, 
                    {"7", ""},
                    {"8", ""},
                    {"9", ""}
                };
            return aa;
        }
    }
}
