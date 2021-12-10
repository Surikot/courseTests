using FsCheck.Experimental;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace courseTest
{
    public class Tests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {


            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void Test1()
        {
            driver.Url = "https://harabadealer.ru/";
            driver.FindElement(By.Id("btn_showLogin")).Click();
        }

        [TearDown]
        public void Stop()
        {
             driver.Quit();
             driver = null;
        }
    }
}