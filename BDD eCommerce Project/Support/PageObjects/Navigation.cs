using OpenQA.Selenium;
using static BDD_eCommerce_Project.Support.HelperLibrary;

namespace BDD_eCommerce_Project.Support.PageObjects {
    internal class Navigation {

        private IWebDriver _driver;

        public Navigation(IWebDriver driver) {
            this._driver = driver;
        }
        public enum Link {
            Home,
            Shop,
            Cart,
            Checkout,
            MyAccount,
            Blog
        }
        public IWebElement HomeLink => WaitForElement(_driver, 3, By.LinkText("Home"));
        public IWebElement ShopLink => WaitForElement(_driver, 3, By.LinkText("Shop"));
        public IWebElement CartLink => WaitForElement(_driver, 3, By.LinkText("Cart"));
        public IWebElement CheckoutLink => WaitForElement(_driver, 3, By.LinkText("Checkout"));
        public IWebElement MyAccountLink => WaitForElement(_driver, 3, By.LinkText("My account"));
        public IWebElement BlogLink => WaitForElement(_driver, 3, By.LinkText("Blog"));

        // Method to click on a link based on the Enum
        public void ClickLink(Link link) {
            Thread.Sleep(1000);     // wait for page to load
            switch (link) {
                case Link.Home:
                    HomeLink.Click();
                    break;
                case Link.Shop:
                    ShopLink.Click();
                    break;
                case Link.Cart:
                    CartLink.Click();
                    break;
                case Link.Checkout:
                    CheckoutLink.Click();
                    break;
                case Link.MyAccount:
                    MyAccountLink.Click();
                    break;
                case Link.Blog:
                    BlogLink.Click();
                    break;
                default:
                    throw new ArgumentException($"Unsupported link: {link}");
            }
        }
    }
}

