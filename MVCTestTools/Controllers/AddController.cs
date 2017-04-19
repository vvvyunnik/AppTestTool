using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MVCTestTools.Contex;
using MVCTestTools.Entities;
using MVCTestTools.ViewModels;
using System.Data.Entity.Validation;
using System.Collections.ObjectModel;
using System;

namespace MVCTestTools.Controllers
{
    public class AddController : Controller
    {
        MVCTestToolsContext context = new MVCTestToolsContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAllApps()
        {
            var resultAppList = (from application in context.Applications                                      
                                 select new 
                                 {  
                                     AppID = application.ApplicationID,
                                     ProjectName = application.ProjectName,
                                     Url = application.Url,                                   
                                     SearchParameter = application.Tests.Select(tst => tst.SearchParameter).FirstOrDefault(),
                                     UserName = application.Admins.Select(a => a.UserName).ToList()
                                 })                              
                                 .ToList().Distinct();
            return Json(resultAppList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllAdmins()
        {
            var summaryAdminList = context.Admins.Select(itemOfList => new {
                AdminID = itemOfList.AdminID,
                UserName = itemOfList.UserName,
                Email = itemOfList.Email
            });
            return Json(summaryAdminList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult CreateNewAdmin(Admin model)
        {
            if (ModelState.IsValid)
            {
                var adminObject = new Admin();
                adminObject.Email = model.Email;
                adminObject.UserName = model.UserName;
                context.Admins.Add(adminObject);
                context.SaveChanges();
                return PartialView("_AdminList", new Admin());
            }
            return PartialView("_AdminList", model);
        }

        [HttpPost]
        public PartialViewResult CreateNewTest(CreateAppViewModel viewModel)
        {
            List<Admin> fullAdminList = context.Admins.ToList();
            var app = new Application()
            {
                ProjectName = viewModel.ProjectName,
                Url = viewModel.Url,
            };

            for (int i = 0; i < Enum.GetNames(typeof(TestNames)).Length; i++)
            {
                var Test = new Test()
                {
                    SearchParameter = viewModel.SearchParameter,
                    Browser = viewModel.Browser,
                    AppId = app.ApplicationID,
                    Name = (TestNames)i
                };
                if (ModelState.IsValid)
                {
                    context.Tests.Add(Test);
                    app.Tests.Add(Test);
                }
            }

            foreach (var adminUsername in viewModel.UserName)
            {
                var adminInDatabase = context.Admins.Where(x => x.UserName == adminUsername).FirstOrDefault();
                var admin = app.Admins.Where(x => x == adminInDatabase).FirstOrDefault();
                if (admin == null)
                    app.Admins.Add(adminInDatabase);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Applications.Add(app);
                    context.SaveChanges();
                    ModelState.Clear();
                    return PartialView("_InputForm", new CreateAppViewModel());
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult validationError in ex.EntityValidationErrors)
                    {
                        Response.Write("Object: " + validationError.Entry.Entity.ToString());
                        Response.Write(" ");
                        foreach (DbValidationError err in validationError.ValidationErrors)
                        {
                            Response.Write(err.ErrorMessage + " ");
                        }
                    }
                }
            }
            return PartialView("_InputForm", viewModel);
        }

        [HttpDelete]
        public JsonResult DeleteAdmin(int adminId)
        {
            Admin adminToDelete = context.Admins.Find(adminId);
            context.Admins.Remove(adminToDelete);
            context.SaveChanges();
            return Json("Records(Admin) deleted successfully.");
        }

        [HttpDelete]
        public JsonResult DeleteApp(int appId)
        {
            Application appToDelete = context.Applications.Find(appId);
            context.Applications.Remove(appToDelete);
            context.SaveChanges();
            return Json("Records(App) deleted successfully.");
        }

        public PartialViewResult RenderForm()
        {
            return PartialView("_InputForm", new CreateAppViewModel());
        }

        public JsonResult IsApplicationExist(string projectName)
        {
            return Json(!context.Applications.Any(x => x.ProjectName == projectName), JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsAdminExist(string username)
        {
            return Json(!context.Admins.Any(x => x.UserName == username), JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsUrlExist(string url)
        {
            return Json(!context.Applications.Any(x => x.Url == url), JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsEmailExist(string email)
        {
            return Json(!context.Admins.Any(x => x.Email == email), JsonRequestBehavior.AllowGet);
        }
    }
}

