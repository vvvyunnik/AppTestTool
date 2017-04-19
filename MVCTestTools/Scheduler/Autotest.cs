using System;
using System.Collections.Generic;
using Quartz;
using MVCTestTools.Contex;
using Testy;
using System.Collections;
using Testy.Helper;
using NUnit.Core;
using MVCTestTools.Helper;
using MVCTestTools.Entities;

namespace MVCTestTools.Scheduler
{
    public class Autotest : IJob
    {
        MVCTestToolsContext db = new MVCTestToolsContext();

        public void Execute(IJobExecutionContext context)
        {
            STest.testParameters = new ArrayList();
            for (int i = 0; i < Enum.GetNames(typeof(TestNames)).Length; i++)
            {
                STest.testParameters.Add(new List<String[]>());
            }

            foreach (Entities.Test test in db.Tests)
            {
                if (test.Name == TestNames.Redirect_To_LogIn_Page_Test)
                {
                    (STest.testParameters[0] as List<String[]>).Add(new String[] { test.TestID.ToString(), test.Application.Url });
                }
                else if (test.Name == TestNames.Find_Element_By_Id_Test)
                {
                    (STest.testParameters[1] as List<String[]>).Add(new String[] { test.TestID.ToString(), test.SearchParameter, test.Application.Url });
                }
            }

            Tests.TestRunner runner = new Tests.TestRunner(GetPath.testDllPath);
            TestResult[] result = runner.RunAllTests();

            Result res = new Result();
            res.SaveChanges(result);
            res.Mailer(result);

        }
    }
}