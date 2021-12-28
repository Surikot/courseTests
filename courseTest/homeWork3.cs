using FsCheck.Experimental;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace courseTest
{
    public class Tests
    {
        private ChromeDriver _driver;

        bool IsElementPresent(WebDriver driver, By locator)
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
        // private WebDriverWait _wait;

        [SetUp]
        public void Setup()
        {

            _driver = new ChromeDriver();
           // _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void Test3()
        {
            _driver.Url = "http://localhost:8080/litecart/admin";
            _driver.FindElement(By.Name("username")).SendKeys("admin");
            _driver.FindElement(By.Name("password")).SendKeys("admin");
            _driver.FindElement(By.Name("remember_me")).Click();
            _driver.FindElement(By.Name("login")).Click();
            Thread.Sleep(100);

            var menuTabs = _driver.FindElements(By.CssSelector("ul#box-apps-menu > li"));
            for (int i = 1; i < menuTabs.Count + 1; i++)
            {
                _driver.FindElement(By.XPath("//ul[@id='box-apps-menu']/li["+i+"]")).Click();
                Assert.IsTrue(IsElementPresent(_driver, By.XPath("//td[@id='content']/h1")));
                if (!IsElementPresent(_driver, By.XPath("//ul[@id='box-apps-menu']/li[" + i + "]/ul/li"))) 
                    continue;
                
                var menuSubTabs = _driver.FindElements(By.XPath("//ul[@id='box-apps-menu']/li[" + i + "]/ul/li"));
                for (int j = 1; j < menuSubTabs.Count + 1; j++)
                {
                    _driver.FindElement(By.XPath("//ul[@id='box-apps-menu']/li[" + i + "]/ul/li[" + j + "]")).Click();
                    Assert.IsTrue(IsElementPresent(_driver, By.XPath("//td[@id='content']/h1")));
                }
                // Thread.Sleep(800);
            }
        }

        [Test]
        public void Test4()
        {
            _driver.Url = "http://localhost:8080/litecart/";
            Thread.Sleep(100);

            var items = _driver.FindElements(By.XPath("//ul[@class='listing-wrapper products']/li"));
            var stickers = _driver.FindElements(By.XPath("//ul[@class='listing-wrapper products']/li//div[contains(@class,'sticker')]"));

            Assert.AreEqual(items.Count, stickers.Count);
        }

        [TearDown]
        public void Stop()
        {
             _driver.Quit();
             _driver = null;
        }
    }
}