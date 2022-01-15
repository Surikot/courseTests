using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace courseTest.PageObjects
{
    public class ProductMenuPageObject
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        Random rnd = new Random();
        private readonly By _productOptions = By.CssSelector(".buy_now  select");
        private readonly By _cartCount = By.CssSelector("#cart span.quantity");
        private readonly By _addToCatdButton = By.CssSelector("#box-product button[name='add_cart_product']");
        private readonly By _homeButton = By.CssSelector("i[title='Home']");

        public ProductMenuPageObject(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

        }
        public void ClickRandomProductOptious()
        {
           _driver.FindElement(_productOptions).FindElements(By.TagName("option"))[rnd.Next(1, 4)].Click();
        }
        public int GetCurrentCartCount() 
        {
            return Int32.Parse(_driver.FindElement(_cartCount).Text);
        }

        public ProductMenuPageObject AddToCart()
        {
            var basicSet = new BasicSet(_driver);

            if (basicSet.IsElementPresent(_driver, _productOptions))
            {
                ClickRandomProductOptious();
            }
            _driver.FindElement(_addToCatdButton).Click();
            int cartCntAfterAdd = GetCurrentCartCount() + 1;
            _wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='cart']//span[text() ='" + cartCntAfterAdd + "']")));
            return this;
        }
         
        public MainMenuPageObject GoHome()
        {
            _driver.FindElement(_homeButton).Click();
            return new MainMenuPageObject(_driver);
        }

    }
}
