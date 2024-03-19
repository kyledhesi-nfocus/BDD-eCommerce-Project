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

        private IWebElement _addToCartBelt => WaitForElement(_driver, 2, By.CssSelector("a.button.ajax_add_to_cart[data-product_id='28']"));
        private IWebElement _addToCartHoodieWithLogo => WaitForElement(_driver, 5, By.CssSelector("a.button.ajax_add_to_cart[data-product_id='31']"));
        private IWebElement _addToCartPolo => WaitForElement(_driver, 5, By.CssSelector("a.button.ajax_add_to_cart[data-product_id='36']"));
        private IWebElement _addToCartSunglasses => WaitForElement(_driver, 5, By.CssSelector("a.button.ajax_add_to_cart[data-product_id='30']"));
        private IWebElement _productsInCartCount => WaitForElement(_driver, 5, By.CssSelector("#site-header-cart > li:nth-child(1) > a > span.count"));

        public bool AddProductToCart(string product) {
            bool addedProduct = false;
            switch (product.ToLower()) {
                case "belt":
                    _addToCartBelt.Click();
                    break;
                case "hoodie":
                    _addToCartHoodieWithLogo.Click();
                    break;
                case "polo":
                    _addToCartPolo.Click();
                    break;
                case "sunglasses":
                    _addToCartSunglasses.Click();
                    break;
                default:
                    _addToCartBelt.Click();
                    break;
            }

            if (_productsInCartCount.Displayed) {
                addedProduct = true;
            }
            return addedProduct;
        }
    }
}
