using System.Linq;
using System.Web.Mvc;
using MVCTestTools.Contex;
using NUnit.Core;
using Testy.Helper;
using MVCTestTools.Helper;
using MVCTestTools.ViewModels;
using MVCTestTools.Tests;

namespace MVCTestTools.Controllers
{
    public class HomeController : Controller
    {
        MVCTestToolsContext db = new MVCTestToolsContext();

        public ActionResult Index()
        {
            return View(db.Applications.ToList());
        }

        public ActionResult PackTestRun(int? id)
        {
            Entities.Application app = db.Applications.Find(id);
            if (app == null)
            {
                return HttpNotFound();
            }

            TestInput testCase = new TestInput();
            testCase.SetInput(app);

            Tests.TestRunner runner = new Tests.TestRunner(GetPath.testDllPath);
            TestResult[] result = runner.RunProjectTests();

            Save_Mail(result);

            return RedirectToAction("Index");
        }

        public ActionResult AllTestRun()
        {
            TestInput testCase = new TestInput();
            testCase.SetInput();

            Tests.TestRunner runner = new Tests.TestRunner(GetPath.testDllPath);
            TestResult[] result = runner.RunAllTests();

            Save_Mail(result);

            return RedirectToAction("Index");
        }

        private void Save_Mail(TestResult[] result)
        {
            Result res = new Result();
            res.SaveChanges(result);
            res.Mailer(result);
        }

    }
}