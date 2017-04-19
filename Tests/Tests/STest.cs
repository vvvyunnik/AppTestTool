using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Testy.Tests;
using Testy.WebPagesDescription;

namespace Testy
{
    public partial class STest : SuiteBase
    {
        [Test, TestCaseSource("testParameters_param1")]      
        public void Redirect_To_LogIn_Page_Test(String testId, String startPath)
        {
            var page = new PageBase(_driver, startPath);
            page.WaitingLoading();
            Assume.That(true == page.IsRightPage());
        }

        [Test, TestCaseSource("testParameters_param2")]
        public void Find_Element_By_Id_Test(String testId, String ID, String startPath)
        {
            var page = new PageBase(_driver, startPath);
            page.WaitingLoading();
            Assume.That(true == page.IsIdExist(ID));
        }
    }

    public partial class STest
    {
        public static ArrayList testParameters;

        public IEnumerable<String[]> testParameters_param1()
        {
            if (testParameters[0] is List<String[]>)
                for (int i = 0; i < (testParameters[0] as List<String[]>).Count; i++)
                {
                    yield return (testParameters[0] as List<String[]>)[i];
                }
            else yield break;
        }

        public IEnumerable<String[]> testParameters_param2()
        {
            if (testParameters[0] is List<String[]>)
                for (int i = 0; i < (testParameters[1] as List<String[]>).Count; i++)
                {
                    yield return (testParameters[1] as List<String[]>)[i];
                }
            else yield break;
        }
    }
}


