using OpenQA.Selenium;
using static BDD_eCommerce_Project.Support.HelperLibrary;

namespace BDD_eCommerce_Project.Support.PageObjects {
    internal class OrderReceived {

        private IWebDriver _driver;

        public OrderReceived(IWebDriver driver) {
            this._driver = driver;
        }

        public string OrderNumber => WaitForElement(_driver, 3, By.CssSelector("#post-6  li.woocommerce-order-overview__order.order strong")).Text;
       
        public string GetOrderNumber() {
            WaitForElement(_driver, 3, By.CssSelector("#post-6 > header > h1"));
            return OrderNumber;
        }
    }
}
