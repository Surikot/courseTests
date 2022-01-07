using FsCheck.Experimental;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

        public void AuthAdmin(string username, string password)
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
            //_driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0.5);


        }

        [Test]
        public void Task6()
        {
            AuthAdmin("admin", "admin");
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
            IList<IWebElement> productDivs = _driver.FindElements(By.XPath("//*[@class='listing-wrapper products']"));
            foreach (IWebElement productDiv in productDivs)
            {
                IList<IWebElement> products = productDiv.FindElements(By.XPath("//*[@class='product column shadow hover-light']"));
                foreach (IWebElement product in products)
                {
                    var stickerCount = product.FindElements(By.XPath("//div[contains(@class,'sticker')]")).Count;
                    Assert.IsTrue(stickerCount > 0);
                }
            }
       
        }

        [Test]
        public void Task8()
        {
            AuthAdmin("admin", "admin");
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
            AuthAdmin("admin", "admin");
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
            IJavaScriptExecutor jse = (IJavaScriptExecutor)_driver;
            Random rnd = new Random();

            string firstName = "Suritest";
            string lastName = "Pikievatest";
            string adress = "Gamidova 18j";
            string postCode = "36700";
            string city = "Citytest";
            string password = "Suriunipub32";
            string email = "surikat" + rnd.Next(1,2022) + "@test.ru";
            var zonesIndex = rnd.Next(0, 65);
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


        [Test]
        public void Task12()
        {
            Random rnd = new Random();
            Actions actions = new Actions(_driver);
            var forProductNum = rnd.Next(0, 199);
            IJavaScriptExecutor jse = (IJavaScriptExecutor)_driver;
            AuthAdmin("admin", "admin");
            _driver.FindElement(By.XPath("//*[@id='app-']//span[contains (text(), 'Catalog')]")).Click();
            _driver.FindElement(By.XPath("//*[@id='content']//a[contains (text(), ' Add New Product')]")).Click();
            //заполнение General
            _driver.FindElement(By.XPath("//*[@id='tab-general']//label[contains(text(),' Enabled')]/input[@type='radio']")).Click();
            _driver.FindElement(By.XPath("//*[@id='tab-general']//input[contains(@name,'name')]")).SendKeys("ProductName" + forProductNum);
            _driver.FindElement(By.XPath("//*[@id='tab-general']//input[@type='checkbox'][@name='product_groups[]'][@value='1-" + rnd.Next(1, 4) + "']")).Click();
         
            _driver.FindElement(By.XPath("//*[@id='tab-general']//input[@name='quantity']")).Clear();
            _driver.FindElement(By.XPath("//*[@id='tab-general']//input[@name='quantity']")).SendKeys("12,00");
            _driver.FindElement(By.XPath("//*[@id='tab-general']//input[@name='new_images[]']")).SendKeys(AppDomain.CurrentDomain.BaseDirectory + @"\img\product_img.jpg");
           
            jse.ExecuteScript("arguments[0].setAttribute('value', '1990-01-01')", _driver.FindElement(By.XPath("//*[@id='tab-general']//input[@name='date_valid_from']")));
            jse.ExecuteScript("arguments[0].setAttribute('value', '2000-12-31')", _driver.FindElement(By.XPath("//*[@id='tab-general']//input[@name='date_valid_to']")));
            //заполнение Information
            _driver.FindElement(By.XPath("//*[@id='content']//a[contains(text(),'Information')]")).Click();
            SelectElement manufacturer = new SelectElement(_driver.FindElement(By.XPath("//select[@name='manufacturer_id']")));
            manufacturer.SelectByValue("1");
            _driver.FindElement(By.XPath("//input[contains(@name,'keywords')]")).SendKeys("something");
            _driver.FindElement(By.XPath("//input[contains(@name,'short_description')]")).SendKeys("Short Description");
            _driver.FindElement(By.XPath("//*[@id='tab-information']//div[@contenteditable='true']")).SendKeys("Description Description Description");
            _driver.FindElement(By.XPath("//input[contains(@name,'head_title')]")).SendKeys("Head Title");
            _driver.FindElement(By.XPath("//input[contains(@name,'meta_description')]")).SendKeys("Meta Description");
            //заполнение Data
            _driver.FindElement(By.XPath("//*[@id='content']//a[contains(text(),'Data')]")).Click();
            _driver.FindElement(By.XPath("//input[contains(@name,'sku')]")).SendKeys("SKU");
            _driver.FindElement(By.XPath("//input[contains(@name,'gtin')]")).SendKeys("GTIN");
            _driver.FindElement(By.XPath("//input[contains(@name,'taric')]")).SendKeys("TARIC");

            _driver.FindElement(By.XPath("//*[@id='tab-data']//input[@name='weight']")).Clear();
            _driver.FindElement(By.XPath("//*[@id='tab-data']//input[@name='weight']")).SendKeys("2,00");

            _driver.FindElement(By.XPath("//*[@id='tab-data']//input[@name='dim_x']")).Clear();
            _driver.FindElement(By.XPath("//*[@id='tab-data']//input[@name='dim_x']")).SendKeys("1,00");

            _driver.FindElement(By.XPath("//*[@id='tab-data']//input[@name='dim_y']")).Clear();
            _driver.FindElement(By.XPath("//*[@id='tab-data']//input[@name='dim_y']")).SendKeys("3,00");

            _driver.FindElement(By.XPath("//*[@id='tab-data']//input[@name='dim_z']")).Clear();
            _driver.FindElement(By.XPath("//*[@id='tab-data']//input[@name='dim_z']")).SendKeys("4,00");
            jse.ExecuteScript("arguments[0].value = 'Attributes'", _driver.FindElement(By.XPath("//textarea[contains(@name,'attributes')]")));
            _driver.FindElement(By.XPath("//*[@id='content']//button[@name='save']")).Click();

            Assert.IsTrue(IsElementPresent(_driver, By.XPath("//*[@id='content']//a[contains(text(),'ProductName" + forProductNum + "')]")));
        }

        [Test]
        public void Task13()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IJavaScriptExecutor jse = (IJavaScriptExecutor)_driver;
            Random rnd = new Random();
            _driver.Url = "http://localhost:8080/litecart/en/";
            var cartCount = Int32.Parse(_driver.FindElement(By.XPath("//*[@id='cart']//span[@class ='quantity']")).Text);

            void AddToCart()
            {
                _driver.FindElement(By.XPath("//*[@class = 'product column shadow hover-light']")).Click();
                if (IsElementPresent(_driver, By.XPath("//*[@id='box-product']//select[contains(@name,'options')]")))
                {
                    jse.ExecuteScript("arguments[0].selectedIndex = '" + rnd.Next(1, 4) + "'", _driver.FindElement(By.XPath("//*[@id='box-product']//select[contains(@name,'options')]")));

                }

                _driver.FindElement(By.XPath("//*[@id='box-product']//button[@name='add_cart_product']")).Click();
                cartCount++;
                wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='cart']//span[text() ='" + cartCount + "']")));
                _driver.FindElement(By.XPath("//*[@id='site-menu']//i[@title='Home']")).Click();
            }

            void ClaerCart(int count)
            {
                for (int i = 0; i < count; i++)
                {
                    if (IsElementPresent(_driver, By.XPath("//*[@id='order_confirmation-wrapper']/table")))
                    {
                        wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='box-checkout-cart']//li[1][@class='item']")));
                        var itemName = _driver.FindElement(By.XPath("//*[@id='box-checkout-cart']//li[1][@class='item']//strong")).Text;
                        var productInTable = _driver.FindElement(By.XPath("//*[@id='order_confirmation-wrapper']/table//td[text()='" + itemName + "']"));

                        _driver.FindElement(By.XPath("//*[@id='box-checkout-cart']//button[text()='Remove']")).Click();
                    
                        if (wait.Until(ExpectedConditions.StalenessOf(productInTable)))
                            continue;
                    }
                }
            }

            AddToCart();

            _driver.FindElement(By.XPath("//*[@id='cart']/a[contains(text(),'Checkout')]")).Click();
            _driver.FindElement(By.XPath("//*[@id='site-menu']//i[@title='Home']")).Click();
            AddToCart();
            AddToCart();
            _driver.FindElement(By.XPath("//*[@id='cart']/a[contains(text(),'Checkout')]")).Click();
            ClaerCart(cartCount);
            //

        }

            [TearDown]
        public void Stop()
        {
             _driver.Quit();
             _driver = null;
        }
    }
}