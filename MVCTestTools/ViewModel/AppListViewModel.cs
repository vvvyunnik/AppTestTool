using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace MVCTestTools.ViewModels
{
    public class AppListViewModel
    {
        public int AppID { get; set; }
        public string ProjectName { get; set; }
        public string Url { get; set; }
        public string SearchParameter { get; set; }
        [ScriptIgnore]
        public virtual IEnumerable<string> UserName { get; set; }
    }
}