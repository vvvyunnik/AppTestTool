namespace MVCTestTools.Migrations
{
    using Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MVCTestTools.Contex.MVCTestToolsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MVCTestTools.Contex.MVCTestToolsContext context)
        {
            var admins = new List<Admin>
            {
                new Admin {AdminID=1, UserName = "Axel", Email = "sdghf@gmail.com" },
                new Admin {AdminID=2, UserName = "John", Email = "sdhf@gmail.com" },
                new Admin {AdminID=3, UserName = "Tony", Email = "sghf@gmail.com"},
                new Admin {AdminID=4, UserName = "Axel", Email = "sdghf@gmail.com"},
                new Admin {AdminID=5, UserName = "Gerard", Email = "sdghf@gmail.com"}
            };
            admins.ForEach(s => context.Admins.AddOrUpdate(s));
            context.SaveChanges();

            var apps = new List<Application>
            {
                new Application {ApplicationID =1, ProjectName = "Epol", Url = "https://intranet-epby.office.int/epby/en/" },
                new Application {ApplicationID =2, ProjectName = "Epol01", Url = "https://lunchtool-epby.office.int/Order/Create"  },
                new Application {ApplicationID =3, ProjectName = "Epol02", Url = "https://wtr-epby.office.int/worktimereport/myweekreports.do"  }
            };
            apps[0].Admins.Add(context.Admins.Where(a => a.AdminID == 1).FirstOrDefault());
            apps[0].Admins.Add(context.Admins.Where(a => a.AdminID == 2).FirstOrDefault());
            apps[1].Admins.Add(context.Admins.Where(a => a.AdminID == 3).FirstOrDefault());
            apps[2].Admins.Add(context.Admins.Where(a => a.AdminID == 4).FirstOrDefault());
            apps[2].Admins.Add(context.Admins.Where(a => a.AdminID == 5).FirstOrDefault());
            apps.ForEach(s => context.Applications.AddOrUpdate(p => p.ProjectName, s));
            context.SaveChanges();

            var tests = new List<Test>
            {
                new Test {TestID=1, Name = TestNames.Redirect_To_LogIn_Page_Test, Runtime=15, IsSuccessful=true, SearchParameter="https://intranet-epby.office.int/epby/en/", TestDate=DateTime.Parse("30-03-2017"), Browser = "chrome", AppId = 1 },
                new Test {TestID=2, Name = TestNames.Find_Element_By_Id_Test, Runtime=9, IsSuccessful=true, SearchParameter="Password", TestDate=DateTime.Parse("30-03-2017"), Browser = "chrome", AppId = 1 },
                new Test {TestID=3, Name = TestNames.Redirect_To_LogIn_Page_Test, Runtime=1, IsSuccessful=false, SearchParameter="https://lunchtool-epby.office.int/Order/Create", TestDate=DateTime.Parse("29-03-2017"), Browser = "chrome", AppId = 2 },
                new Test {TestID=4, Name = TestNames.Find_Element_By_Id_Test, Runtime=9, IsSuccessful=true, SearchParameter="Password", TestDate=DateTime.Parse("30-03-2017"), Browser = "chrome", AppId = 2 },
                new Test {TestID=5, Name = TestNames.Redirect_To_LogIn_Page_Test, Runtime=9, IsSuccessful=true, SearchParameter="https://wtr-epby.office.int/worktimereport/myweekreports.do", TestDate=DateTime.Parse("30-03-2017"), Browser = "chrome", AppId = 3 },
                new Test {TestID=6, Name = TestNames.Find_Element_By_Id_Test, Runtime=9, IsSuccessful=true, SearchParameter="Password", TestDate=DateTime.Parse("30-03-2017"), Browser = "chrome", AppId = 3 }
            };
            foreach (Test e in tests)
            {
                var testInDataBase = context.Tests.Where(
                    s => s.Application.ApplicationID == e.AppId).FirstOrDefault();
                if (testInDataBase == null)
                {
                    context.Tests.Add(e);
                }
            }
            context.SaveChanges();
        }
    }
}
