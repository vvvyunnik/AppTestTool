using System;
using System.Collections.Generic;
using MVCTestTools.Contex;
using MVCTestTools.Entities;
using Testy;
using System.Collections;

namespace MVCTestTools.Tests
{
    public class TestInput
    {
        MVCTestToolsContext db = new MVCTestToolsContext();

        public void SetInput(Application app)
        {
            STest.testParameters = new ArrayList();
            for (int i = 0; i < app.Tests.Count; i++)
            {
                STest.testParameters.Add(new List<String[]>());
            }

            foreach (Entities.Test test in app.Tests)
            {
                if (test.Name == TestNames.Redirect_To_LogIn_Page_Test)
                {
                    (STest.testParameters[0] as List<String[]>).Add(new String[] { test.TestID.ToString(), app.Url });
                }
                else if (test.Name == TestNames.Find_Element_By_Id_Test)
                {
                    (STest.testParameters[1] as List<String[]>).Add(new String[] { test.TestID.ToString(), test.SearchParameter, app.Url });
                }
            }
        }

        public void SetInput()
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
                    (STest.testParameters[1] as List<String[]>).Add(new String[] { test.TestID.ToString(), "gi", test.Application.Url });
                }
            }
        }
    }
}