using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Testy.WebPagesDescription
{
    class PageBase
    {
        protected IWebDriver WebDriver;
        private readonly string _url;
        public WebDriverWait _wait;

        public PageBase(IWebDriver driver, string url)
        {
            WebDriver = driver;
            _url = url;
            WebDriver.Url = _url;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public virtual bool IsRightPage()
        {
            return WebDriver.Url.Contains("https://websession-wcf.office.int/");
        }

        private IWebElement processingPanel
        {
            get { return WebDriver.FindElement(By.Id("ProcessingPopupExtender_backgroundElement")); }
        }

        public virtual void WaitingLoading()
        {
            _wait.Until((b) => CheckAllElements());
        }

        public virtual bool CheckAllElements()
        {
            try
            {
                var style = processingPanel.GetAttribute("style");
                return !(processingPanel.Enabled && processingPanel.Displayed && style.Contains("position: fixed"));
            }
            catch (Exception)
            {
                return true;
            }
        }


        public virtual bool IsIdExist(string id)
        {
            try
            {
                var button = WebDriver.FindElement(By.Id(id));
            }
            catch (NoSuchElementException e)
            {
                return false;
            }
            return true;
        }
    }
}
