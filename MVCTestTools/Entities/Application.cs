using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCTestTools.Entities
{
    public class Application
    {
        [Key]
        public int ApplicationID { get; set; }
        [Required(ErrorMessage = "Project name is required")]
        [MaxLength(50, ErrorMessage = "Max length")]
        public string ProjectName { get; set; }
        [MaxLength(200, ErrorMessage = "Max length")]
        [Display(Name = "URL")]
        [DataType(DataType.Url)]
        public string Url { get; set; }
        public virtual ICollection<Admin> Admins { get; set; }
        public virtual ICollection<Test> Tests { get; set; }

        public Application()
        {
            Admins = new List<Admin>();
            Tests = new List<Test>();
        }
    }
}