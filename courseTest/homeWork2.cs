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
        public void Test2()
        {
            driver.Url = "http://localhost:8080/litecart/admin";
            driver.FindElement(By.Name("username")).SendKeys("Surikat");
            driver.FindElement(By.Name("password")).SendKeys("Suriunipub32");
            driver.FindElement(By.Name("remember_me")).Click();
            driver.FindElement(By.Name("login")).Click();
        }

        [TearDown]
        public void Stop()
        {
             driver.Quit();
             driver = null;
        }
    }
}