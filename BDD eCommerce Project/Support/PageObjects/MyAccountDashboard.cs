using OpenQA.Selenium;
using static BDD_eCommerce_Project.Support.HelperLibrary;

namespace BDD_eCommerce_Project.Support.PageObjects {
    internal class MyAccountDashboard {

        private IWebDriver _driver;

        public MyAccountDashboard(IWebDriver driver) {
            this._driver = driver;
        }

        public IWebElement LogoutLink => WaitForElement(_driver, 3, By.LinkText("Logout"));
        public IWebElement OrdersLink => WaitForElement(_driver, 3, By.LinkText("Orders"));

        public void ClickLogoutLink() {
            LogoutLink.Click();
        }

        public void ClickViewOrdersLink() {
            OrdersLink.Click();
        }
    }
}
