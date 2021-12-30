using FsCheck.Experimental;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        public void authAdmin(string username, string password)
        {
            _driver.Url = "http://localhost:8080/litecart/admin";
            _driver.FindElement(By.Name("username")).SendKeys(username);
            _driver.FindElement(By.Name("password")).SendKeys(password);
            _driver.FindElement(By.Name("login")).Click();
        }
        // private WebDriverWait _wait;

        [SetUp]
        public void Setup()
        {

            _driver = new ChromeDriver();
           // _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void Task6()
        {
            authAdmin("admin", "admin");
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
        public void Task7()
        {
            _driver.Url = "http://localhost:8080/litecart/";
            Thread.Sleep(100);

            var items = _driver.FindElements(By.XPath("//ul[@class='listing-wrapper products']/li"));
            var stickers = _driver.FindElements(By.XPath("//ul[@class='listing-wrapper products']/li//div[contains(@class,'sticker')]"));

            Assert.AreEqual(items.Count, stickers.Count);
        }

        [Test]
        public void Task8()
        {
            authAdmin("admin", "admin");
            _driver.Url = "http://localhost:8080/litecart/admin/?app=countries&doc=countries";
            //получаем кол-во стран
            var countriesCnt = _driver.FindElements(By.XPath("//*[@id='content']/form/table/tbody/tr/td[6]")).Count;
            // массивы для сравнения
            var countriesName = new List<string>();
            //для кодов стран, имеющих зоны > 0
            List<string> hasZones = new List<string>();
            //заполняем массивы стран для сравнения и записываем коды стран, имеющих зоны > 0
            for (int i = 2; i < countriesCnt + 2; i++)
            {
                countriesName.Add(_driver.FindElement(By.XPath("//*[@id='content']/form/table/tbody/tr[" + i + "]/td[5]/a")).Text);
                if (_driver.FindElement(By.XPath("//*[@id='content']/form/table/tbody/tr[" + i + "]/td[6]")).Text != "0")
                {
                    hasZones.Add(_driver.FindElement(By.XPath("//*[@id='content']/form/table/tbody/tr[" + i + "]/td[4]")).Text);
                }
            }

            // сортируем массив стран
            var sortCountriesName = countriesName.OrderBy(x => x).ToList();
            //сравниваем массивы стран
            for (int i = 0; i < countriesCnt; i++)
                Assert.IsTrue(sortCountriesName[i] == countriesName[i]);
            
            //для каждой страны, имеющих кол-во зон > 0 открываем страницу этой страны и сравниваем также
            foreach (string s in hasZones)
            {
                _driver.Url = "http://localhost:8080/litecart/admin/?app=countries&doc=edit_country&country_code=" + s;
                var zonesCnt = _driver.FindElements(By.XPath("//*[@id='remove-zone']")).Count;
                var zonesName = new List<string>();

                for (int i = 2; i < zonesCnt + 2; i++)
                {
                    zonesName.Add(_driver.FindElement(By.XPath("//*[@id='table-zones']/tbody/tr[" + i + "]/td[3]")).Text);
                }

                var sortZonesName = zonesName.OrderBy(x => x).ToList();

                for (int i = 0; i < zonesCnt; i++)
                    Assert.IsTrue(sortZonesName[i] == zonesName[i]);
            }
        }
        [Test]
        public void Task9()
        {
            authAdmin("admin", "admin");
            _driver.Url = "http://localhost:8080/litecart/admin/?app=geo_zones&doc=geo_zones";
            var countriesCnt = _driver.FindElements(By.XPath("//*[@id='content']/form/table/tbody/tr/td[2]")).Count;
            List<string> zonesID = new List<string>();
            for (int i = 2; i < countriesCnt + 2; i++)
            {
                zonesID.Add(_driver.FindElement(By.XPath("//*[@id='content']/form/table/tbody/tr[" + i + "]/td[2]")).Text);
            }
            foreach (string s in zonesID)
            {
                _driver.Url = "http://localhost:8080/litecart/admin/?app=geo_zones&doc=edit_geo_zone&page=1&geo_zone_id=" + s;
                var zonesCnt = _driver.FindElements(By.XPath("//*[@id='table-zones']/tbody/tr/td[3]")).Count;
                var zonesName = new List<string>();
               
                for (int i = 2; i < zonesCnt + 2; i++)
                    zonesName.Add(_driver.FindElement(By.XPath("//*[@id='table-zones']/tbody/tr[" + i + "]/td[3]/select/option[@selected='selected']")).Text);
                
                var sortZonesName = zonesName.OrderBy(x => x).ToList();

                for (int i = 0; i < zonesCnt; i++)
                    Assert.IsTrue(sortZonesName[i] == zonesName[i]);
            }
        }
                
        [TearDown]
        public void Stop()
        {
             _driver.Quit();
             _driver = null;
        }
    }
}