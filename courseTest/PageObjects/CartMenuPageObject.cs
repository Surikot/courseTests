using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace courseTest.PageObjects
{
    public class CartMenuPageObject
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;
        private readonly By _removeBtn = By.Name("remove_cart_item");
    

        public CartMenuPageObject(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

        }
        public void ClearCart(int count)
        {
            var basicSet = new BasicSet(_driver);
            for (int i = 0; i < count; i++)
            {
                if (basicSet.IsElementPresent(_driver, By.XPath("//*[@id='order_confirmation-wrapper']/table")))
                {
                    _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//li[1][@class='item']")));
                    var itemName = _driver.FindElement(By.XPath("//li[1][@class='item']//strong")).Text;
                    var productInTable = _driver.FindElement(By.XPath("//*[@id='order_confirmation-wrapper']/table//td[text()='" + itemName + "']"));
                    _driver.FindElement(_removeBtn).Click();
                    _wait.Until(ExpectedConditions.StalenessOf(productInTable));
                }
            }
        }

    }
}
