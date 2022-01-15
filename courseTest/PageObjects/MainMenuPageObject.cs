using System;
using OpenQA.Selenium;

namespace courseTest.PageObjects
{
    public class MainMenuPageObject 
    {
        private IWebDriver _driver;

        Random rnd = new Random();
        private readonly By _products= By.CssSelector(".product");
        private readonly By _cart = By.XPath("*//a[contains(text(),'Checkout')]");

        public MainMenuPageObject(IWebDriver driver)
        {
            _driver = driver;
        }
      
        public int GetProductsCount()
        {
            return _driver.FindElements(_products).Count;
        }
        public ProductMenuPageObject OpenRandomProduct()
        {
            int count = GetProductsCount();
            _driver.FindElements(_products)[rnd.Next(0, count)].Click();
            
            return new ProductMenuPageObject(_driver); 
        }
        public CartMenuPageObject OpenCart()
        {
            _driver.FindElement(_cart).Click();

            return new CartMenuPageObject(_driver);
        }
    }
}
