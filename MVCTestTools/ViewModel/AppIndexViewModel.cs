using System.Collections.Generic;
using MVCTestTools.Entities;

namespace MVCTestTools.ViewModels
{
    public class AppIndexViewModel
    {
        public IEnumerable<Admin> AdminsCollection { get; set; }
        public IEnumerable<Test> TestsCollection { get; set; }
        public IEnumerable<Application> AppsCollection { get; set; }
    }
}