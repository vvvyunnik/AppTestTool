using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCTestTools.Entities
{
    public enum TestNames
    {
        Redirect_To_LogIn_Page_Test,
        Find_Element_By_Id_Test
    }
    public class Test
    {
        [Key]
        public int TestID { get; set; }
        public TestNames Name { get; set; }
        public double? Runtime { get; set; }
        public bool? IsSuccessful { get; set; }
        [StringLength(100, ErrorMessage = "Max length"), MinLength(1)]
        public string SearchParameter { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TestDate { get; set; }
        [MaxLength(20, ErrorMessage = "Max length")]
        public string Browser { get; set; }

        public int AppId { get; set; }
        [ForeignKey("AppId")]
        public virtual Application Application { get; set; }
    }
}