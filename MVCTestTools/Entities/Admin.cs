using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace MVCTestTools.Entities
{
    public class Admin
    {
        [Key]
        public int AdminID { get; set; }
        [Display(Name = "Login")]
        [Required(ErrorMessage = "Login is required")]
        [Remote("IsAdminExist", "Add", ErrorMessage = "This Login already in use")]
        public string UserName { get; set; }
        [EmailAddress(ErrorMessage = "Not a valid email")]
        [Required(ErrorMessage = "Email is required")]
        [Remote("IsEmailExist", "Add", ErrorMessage = "This Email already in use")]
        public string Email { get; set; }

        public virtual ICollection<Application> Applications { get; set; }

        public Admin()
        {
            Applications = new List<Application>();
        }
    }
}