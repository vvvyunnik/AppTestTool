using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Text;

namespace Testy.Tests
{
    [TestFixture]
    public class SuiteBase
    {
        public static IWebDriver _driver;
        public static StringBuilder _verificationErrors;

        [TestFixtureSetUp]
        public static void TestFixtureSetupTest()
        {
            Console.WriteLine("The test has started");
            _driver = new ChromeDriver();
            if (_verificationErrors == null) _verificationErrors = new StringBuilder();
        }

        [TestFixtureTearDown]
        public static void TestFixtureTeardownTest()
        {
            try
            {
                _driver.Quit();
            }
            catch (Exception) { }
            _verificationErrors = null;
        }
    }
}
