using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Core;
using NUnit.Core.Filters;
using System.Collections;
using MVCTestTools.Entities;

namespace MVCTestTools.Tests
{
    /// <summary>
    /// Runs selectrd tests, get results
    /// </summary>
    public class TestRunner
    {
        public List<String> TestName { get; }

        private String pathToTestLibrary { get; }

        public TestRunner(String pathToTestLibrary)
        {
            this.pathToTestLibrary = pathToTestLibrary;
        }

        /// <summary>
        /// Run tests with specified filter
        /// </summary>
        /// <param name="filter"> Tests filter </param>
        /// <returns> Tests result </returns>
        private TestResult RunTests(TestFilter filter)
        {
            CoreExtensions.Host.InitializeService();
            SimpleTestRunner runner = new SimpleTestRunner();
            TestResult result = null;
            if (runner.Load(new TestPackage(pathToTestLibrary)))
            {
                result = runner.Run(new NullListener(), filter, false, LoggingThreshold.Off);
            }
            if (result != null) return result;
            return null;
        }

        /// <summary>
        /// Set a filter for a single chosen test and run it 
        /// </summary>
        /// <param name="testName">
        ///     Specifies the name of executed test
        /// </param>
        /// <returns> Result of a single test </returns>
        public TestResult[] RunSingleTest(String testName)
        {
            SimpleNameFilter filter = new SimpleNameFilter("Testy.STest." + testName);
            TestResult result = RunTests(filter);
            return new TestResult[] { ((ReachInnerTestResult(result, testName)).Results[0] as TestResult) };
        }

        /// <summary>
        /// Run all test of a single app
        /// </summary>
        /// <returns> Test results </returns>
        public TestResult[] RunProjectTests()
        {
            TestResult result = RunTests(TestFilter.Empty);
            TestResult[] requiredTestResult = new TestResult[Enum.GetNames(typeof(TestNames)).Length];
            for(int i = 0; i < Enum.GetNames(typeof(TestNames)).Length; i++)
            {
                requiredTestResult[i] = (ReachInnerTestResult(result, Enum.GetNames(typeof(TestNames))[i])).Results[0] as TestResult;
            }
            return requiredTestResult;
        }

        /// <summary>
        ///  Run all tests of all apps
        /// </summary>
        /// <returns> Test results </returns>
        public TestResult[] RunAllTests()
        {
            TestResult result = RunTests(TestFilter.Empty);
            List<TestResult> requiredTestResult = new List<TestResult>();
            for (int i = 0; i < Enum.GetNames(typeof(TestNames)).Length; i++)
            {
                ArrayList res = ReachInnerTestResult(result, Enum.GetNames(typeof(TestNames))[i]).Results as ArrayList;
                requiredTestResult.AddRange(res.Cast<TestResult>().ToList());
            }
            return requiredTestResult.ToArray();
        }

        /// <summary>
        /// Select required test result 
        /// </summary>
        /// <param name="test"> Test pack result </param>
        /// <param name="name"> Required test name </param>
        /// <returns> Required test result </returns>
        private TestResult ReachInnerTestResult(TestResult test, string name)
        {
            if (test.Results == null) return null;
            foreach(TestResult item in test.Results)
            {
                if (item.Name == name)
                    return item;
                else
                {
                    TestResult currtest = ReachInnerTestResult(item, name);
                    if (currtest != null)
                        return currtest;
                }
            }
            return null;
        }



    }
}