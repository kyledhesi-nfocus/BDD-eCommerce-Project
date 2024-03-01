using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BDD_eCommerce_Project.Support.HelperLibrary;
using static BDD_eCommerce_Project.Support.PageObjects.Navigation;

namespace BDD_eCommerce_Project.Support.PageObjects {
    internal class Shop {

        private IWebDriver _driver;

        public Shop(IWebDriver driver) {
            this._driver = driver;
        }
        public enum Product {
            Belt,
            HoodieWithLogo,
            Polo,
            Sunglasses
        }
        private IWebElement addToCartBelt => _driver.FindElement(By.CssSelector("a.button.ajax_add_to_cart[data-product_id='28']"));
        private IWebElement addToCartHoodieWithLogo => _driver.FindElement(By.CssSelector("a.button.ajax_add_to_cart[data-product_id='31']"));
        private IWebElement addToCartPolo => _driver.FindElement(By.CssSelector("a.button.ajax_add_to_cart[data-product_id='36']"));
        private IWebElement addToCartSunglasses => _driver.FindElement(By.CssSelector("a.button.ajax_add_to_cart[data-product_id='30']"));

        public void AddToCart(Product product) {
            switch (product) {
                case Product.Belt:
                    WaitForElement(_driver, 2, By.CssSelector("a.button.ajax_add_to_cart[data-product_id='28']"));
                    addToCartBelt.Click();
                    break;
                case Product.HoodieWithLogo:
                    WaitForElement(_driver, 5, By.CssSelector("a.button.ajax_add_to_cart[data-product_id='31']"));
                    addToCartHoodieWithLogo.Click();
                    break;
                case Product.Polo:
                    WaitForElement(_driver, 5, By.CssSelector("a.button.ajax_add_to_cart[data-product_id='36']"));
                    addToCartPolo.Click();
                    break;
                case Product.Sunglasses:
                    WaitForElement(_driver, 5, By.CssSelector("a.button.ajax_add_to_cart[data-product_id='30']"));
                    addToCartSunglasses.Click();
                    break;
                default:
                    throw new ArgumentException($"Unsupported product: {product}");
            }
        }
    }
}
