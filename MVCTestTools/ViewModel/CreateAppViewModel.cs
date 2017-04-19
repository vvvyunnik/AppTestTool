using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MVCTestTools.ViewModels
{
    public class CreateAppViewModel
    {
        [Display(Name = "Application Name")]
        [Required(ErrorMessage = "Application name is required")]
        [MinLength(1, ErrorMessage = "String length is short")]
        [Remote("IsApplicationExist", "Add", ErrorMessage = "This Application name already in use")]
        public string ProjectName { get; set; }
        [Required(ErrorMessage = "Please enter URL")]
        [RegularExpression(@"((?:https?\:\/\/|\/.)(?:[-a-z0-9]+\.)*[-a-z0-9]+.*)", ErrorMessage = "URL is not valid")]
        [Display(Name = "URL")]
        [Remote("IsAdminExist", "Add", ErrorMessage = "This URL already in use")]
        public string Url { get; set; }
        [Display(Name = "Search Parameter")]
        [Required(ErrorMessage = "Search parameter is required")]
        [MinLength(1, ErrorMessage = "String length is short")]
        public string SearchParameter { get; set; }
        [Display(Name = "Browser")]
        [Required(ErrorMessage = "Please select a browser")]
        public string Browser { get; set; }
        public IEnumerable<string> UserName { get; set; }
    }
}