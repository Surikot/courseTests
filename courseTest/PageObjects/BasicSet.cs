using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
namespace courseTest.PageObjects
{
    public class BasicSet
    {
        protected IWebDriver _driver;
        protected WebDriverWait _wait;
        public BasicSet(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        public bool IsElementPresent(IWebDriver driver, By locator)
        {
            try
            {
                driver.FindElement(locator);
                return true;
            }
            catch (NoSuchElementException ex)
            {
                return false;
            }
        }

    }
}
