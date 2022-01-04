using FsCheck.Experimental;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
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


        [SetUp]
        public void Setup()
        {
           
            _driver = new ChromeDriver();
            Actions actions = new Actions(_driver);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);


        }

        [Test]
        public void Task6()
        {
            authAdmin("admin", "admin");
            var menuTabs = _driver.FindElements(By.CssSelector("ul#box-apps-menu > li"));
            for (int i = 1; i < menuTabs.Count + 1; i++)
            {
                _driver.FindElement(By.XPath("//ul[@id='box-apps-menu']/li[" + i + "]")).Click();
                Assert.IsTrue(IsElementPresent(_driver, By.XPath("//td[@id='content']/h1")));
                if (!IsElementPresent(_driver, By.XPath("//ul[@id='box-apps-menu']/li[" + i + "]/ul/li")))
                    continue;

                var menuSubTabs = _driver.FindElements(By.XPath("//ul[@id='box-apps-menu']/li[" + i + "]/ul/li"));
                for (int j = 1; j < menuSubTabs.Count + 1; j++)
                {
                    _driver.FindElement(By.XPath("//ul[@id='box-apps-menu']/li[" + i + "]/ul/li[" + j + "]")).Click();
                    Assert.IsTrue(IsElementPresent(_driver, By.XPath("//td[@id='content']/h1")));
                }

            }
        }

        [Test]
        public void Task7()
        {
            _driver.Url = "http://localhost:8080/litecart/";
            Thread.Sleep(100);
            var items = _driver.FindElements(By.XPath("//ul[@class='listing-wrapper products']/li")).Count;
            for (int i = 1; i < items+1; i++)
            {
                Console.WriteLine(i);
                Assert.IsTrue(IsElementPresent(_driver, By.XPath("//div[@class='box']/div[@class='content']/ul//a["+i+"]//div[contains(@class,'sticker')]")));
            }
            //.ForEach(Console.WriteLine);
            /*items.Add(_driver.FindElements(By.XPath("//ul[@class='listing-wrapper products']/li")));
            var item = _driver.FindElements(By.XPath("//ul[@class='listing-wrapper products']/li")).Count;
            for (int i = 0; i < items; i++)
                Assert.IsTrue(IsElementPresent(_driver, By.XPath("//ul[@class='listing-wrapper products']/li[" + i + "]//div[contains(@class,'sticker')]")));
                var stickers = _driver.FindElements(By.XPath("//ul[@class='listing-wrapper products']/li//div[contains(@class,'sticker')]"));
            List <IWebElement> items = new List<IWebElement> (_driver.FindElements(By.XPath("//ul[@class='listing-wrapper products']/li")));
            items.ForEach(x)
            Assert.AreEqual(items.Count, stickers.Count);*/
        }

        [Test]
        public void Task8()
        {
            authAdmin("admin", "admin");
            _driver.Url = "http://localhost:8080/litecart/admin/?app=countries&doc=countries";
            //получаем кол-во стран
            var countriesCnt = _driver.FindElements(By.XPath("//*[@id='content']/form/table/tbody/tr/td[6]")).Count;
            var countriesName = new List<string>();
            //для кодов стран, имеющих зоны > 0
            List<string> hasZones = new List<string>();
            //заполняем массив стран и записываем коды стран, имеющих зоны > 0
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

            //для каждой страны, имеющих кол-во зон > 0 открываем страницу этой страны и сравниваем также зоны
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

        [Test]
        public void Task10()
        {
            _driver.Url = "http://localhost:8080/litecart/en/";
            var productName1 = _driver.FindElement(By.XPath("//*[@id='box-campaigns']//div[@class='name']")).Text;
            var regularPrice1 = _driver.FindElement(By.XPath("//*[@id='box-campaigns']//s[@class='regular-price']"));
            var campaignPrice1 = _driver.FindElement(By.XPath("//*[@id='box-campaigns']//strong[@class='campaign-price']"));
            var regularPriceTxt1 = regularPrice1.Text;
            var campaignPriceTxt1 = campaignPrice1.Text;

            static string[] GetColorArray(string cssColValue)
            {
                string colStr = cssColValue.Replace("rgba", "").Replace("(", "").Replace(")", "").Replace(",", "").Replace("rgb", "");
                string[] colStrArray = colStr.Split(' ');
                return colStrArray;
            }

            string[] isGreyColor1 = GetColorArray(regularPrice1.GetCssValue("color"));
            Assert.IsTrue((isGreyColor1[0] == isGreyColor1[1]) & (isGreyColor1[1] == isGreyColor1[2]));
            
            string[] isRedColor1 = GetColorArray(campaignPrice1.GetCssValue("color"));
            Assert.IsTrue((isRedColor1[1] == "0") & (isRedColor1[2] == "0"));

            Assert.IsTrue(regularPrice1.GetCssValue("text-decoration").Contains("line-through")
                & ((campaignPrice1.GetCssValue("text-decoration").Contains("solid")) || (campaignPrice1.GetCssValue("text-decoration").Contains("none")) || (campaignPrice1.GetCssValue("text-decoration-style").Contains("solid")) )
                & (campaignPrice1.Size.Height > regularPrice1.Size.Height));
            
            _driver.FindElement(By.XPath("//*[@id='box-campaigns']//a[1]")).Click();

            var productName2 = _driver.FindElement(By.XPath("//*[@id='box-product']//h1[@itemprop='name']")).Text;
            var regularPrice2 = _driver.FindElement(By.XPath("//*[@id='box-product']//s[@class='regular-price']"));
            var campaignPrice2 = _driver.FindElement(By.XPath("//*[@id='box-product']//strong[@class='campaign-price']"));
            var regularPriceTxt2 = regularPrice2.Text;
            var campaignPriceTxt2 = campaignPrice2.Text;
            
            string[] isGreyColor2 = GetColorArray(regularPrice2.GetCssValue("color"));
            Assert.IsTrue((isGreyColor2[0] == isGreyColor2[1]) & (isGreyColor2[1] == isGreyColor2[2]));
            string[] isRedColor2 = GetColorArray(campaignPrice2.GetCssValue("color"));
            Assert.IsTrue((isRedColor2[1] == "0") & (isRedColor2[2] == "0"));

            Assert.IsTrue(regularPrice2.GetCssValue("text-decoration").Contains("line-through")
                & ((campaignPrice2.GetCssValue("text-decoration").Contains("solid")) || (campaignPrice2.GetCssValue("text-decoration").Contains("none")) || (campaignPrice2.GetCssValue("text-decoration-style").Contains("solid")) )
                & (campaignPrice2.Size.Height > regularPrice2.Size.Height)
                & (productName1 == productName2)
                & (regularPriceTxt1 == regularPriceTxt2)
                & (campaignPriceTxt1 == campaignPriceTxt2));
        }

        [Test]
        public void Task11()
        {
            _driver.Url = "http://localhost:8080/litecart/en/create_account";
            IJavaScriptExecutor jse = (IJavaScriptExecutor) _driver;
            Random rnd = new Random();

            string firstName = "Suritest";
            string lastName = "Pikievatest";
            string adress = "Gamidova 18j";
            string postCode = "36700";
            string city = "Citytest";
            string password = "Suriunipub32";
            string email = "surikat" + rnd.Next(1,2022) + "@test.ru";
            var zonesIndex = rnd.Next(-1, 65);
            //заполнение обязательных полей
            _driver.FindElement(By.XPath("//*[@id='create-account']//tr[2]/td[1]/input")).SendKeys(firstName);
            _driver.FindElement(By.XPath("//*[@id='create-account']//tr[2]/td[2]/input")).SendKeys(lastName);
            _driver.FindElement(By.XPath("//*[@id='create-account']//tr[3]/td[1]/input")).SendKeys(adress);
            _driver.FindElement(By.XPath("//*[@id='create-account']//tr[4]/td[1]/input")).SendKeys(postCode);
            _driver.FindElement(By.XPath("//*[@id='create-account']//tr[4]/td[2]/input")).SendKeys(city);
            _driver.FindElement(By.XPath("//*[@id='create-account']//tr[6]/td[1]/input")).SendKeys(email);
            _driver.FindElement(By.XPath("//*[@id='create-account']//tr[6]/td[2]/input")).SendKeys("+123456");
            _driver.FindElement(By.XPath("//*[@id='create-account']//tr[8]/td[1]/input")).SendKeys(password);
            _driver.FindElement(By.XPath("//*[@id='create-account']//tr[8]/td[2]/input")).SendKeys(password);
            //выбор US и рандомной зоны
            jse.ExecuteScript("arguments[0].selectedIndex = 224; arguments[0].dispatchEvent(new Event('change'))", _driver.FindElement(By.XPath("//*[@id='create-account']//tr[5]/td[1]/select")));
            jse.ExecuteScript("arguments[0].selectedIndex = " + zonesIndex + "; arguments[0].dispatchEvent(new Event('change'))", _driver.FindElement(By.XPath("//*[@id='create-account']//tr[5]/td[2]/select")));
            //содание аккаунта
            _driver.FindElement(By.XPath("//*[@id='create-account']//button[@name='create_account']")).Click();
            //разлогин
            _driver.FindElement(By.XPath("//*[contains (text(), 'Logout')]")).Click();
            //смена страны и зоны для успешной авторизации
            _driver.FindElement(By.XPath("//*[@id='region']//a")).Click();
            jse.ExecuteScript("arguments[0].selectedIndex = 224; arguments[0].dispatchEvent(new Event('change'))", _driver.FindElement(By.XPath("//*[@id='box-regional-settings']//select[@name='country_code']")));
            jse.ExecuteScript("arguments[0].selectedIndex = " + zonesIndex + "; arguments[0].dispatchEvent(new Event('change'))", _driver.FindElement(By.XPath("//*[@id='box-regional-settings']//select[@name='zone_code']")));
            _driver.FindElement(By.XPath("//*[@id='box-regional-settings']//button[@name='save']")).Click();
            //вход в ранее созданную учетную запись
            _driver.FindElement(By.XPath("//*[@id='box-account-login']//input[@name='email']")).SendKeys(email);
            _driver.FindElement(By.XPath("//*[@id='box-account-login']//input[@name='password']")).SendKeys(password);
            _driver.FindElement(By.XPath("//*[@id='box-account-login']//button[@name='login']")).Click();
            //разлогин
            _driver.FindElement(By.XPath("//*[contains (text(), 'Logout')]")).Click();



        }
        [TearDown]
        public void Stop()
        {
             _driver.Quit();
             _driver = null;
        }
    }
}