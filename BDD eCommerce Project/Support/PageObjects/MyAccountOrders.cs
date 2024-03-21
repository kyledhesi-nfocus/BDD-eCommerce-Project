using OpenQA.Selenium;
using static BDD_eCommerce_Project.Support.HelperLibrary;

namespace BDD_eCommerce_Project.Support.PageObjects {
    internal class MyAccountOrders {

        private IWebDriver _driver;

        public MyAccountOrders(IWebDriver driver) {
            this._driver = driver;
        }

        public IWebElement DashboardLink => WaitForElement(_driver, 3, By.LinkText("Dashboard"));
        public string OrderID => WaitForElement(_driver, 3, By.CssSelector("#post-7 tbody tr:nth-child(1) td.woocommerce-orders-table__cell.woocommerce-orders-table__cell-order-number a")).Text;
        public string GetMostRecentOrderID() {
            return OrderID.TrimStart('#');
        }
        public void ClickDashboardLink() {
            DashboardLink.Click();
        }
    }
}
