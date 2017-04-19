using System;
using System.Collections.Generic;
using System.Linq;
using MVCTestTools.Contex;
using NUnit.Core;
using MVCTestTools.Mailers;
using System.Text.RegularExpressions;

namespace MVCTestTools.Helper
{
    public class Result
    {
        MVCTestToolsContext db = new MVCTestToolsContext();

        public void SaveChanges(TestResult[] result)
        {
            for (int i = 0; i < result.Length; i++)
            {
                var id = DivideTestResultName(result[i])[1];
                Entities.Test test = db.Tests.Find(Int32.Parse(id));
                if (test == null)
                    continue;
                test.IsSuccessful = result[i].IsSuccess;
                test.Runtime = result[i].Time;
                test.TestDate = DateTime.Now;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Remake test result name stirng to an array (name and input params)
        /// </summary>
        /// <param name="test"> Test result </param>
        /// <returns> Param array </returns>
        public static String[] DivideTestResultName(TestResult test)
        {
            Regex reg = new Regex(@"[\(\),]");
            String name = test.Name;
            name = reg.Replace(name, "");
            String[] matchparams = name.Split(new string[] { "\"" }, StringSplitOptions.None)
                .Where(x => !string.IsNullOrEmpty(x)).ToArray();
            return matchparams;
        }

        public void Mailer(TestResult[] result)
        {
            var failedTests = IfAllSuccess(result);
            if (failedTests != null)
            {
                Mailer mailObj = new Mailer();
                mailObj.SendEmails(failedTests);
            }
        }

        /// <summary>
        /// Check if all test end successfully
        /// </summary>
        /// <param name="result"> Test results array </param>
        /// <returns> Failed tests or null </returns>
        private TestResult[] IfAllSuccess(TestResult[] result)
        {
            List<TestResult> failedTests = new List<TestResult>();
            foreach (TestResult res in result)
            {
                if (!res.IsSuccess)
                {
                    failedTests.Add(res);
                }
            }
            if (failedTests.Count != 0)
                return failedTests.ToArray();
            else return null;
        }
    }
}