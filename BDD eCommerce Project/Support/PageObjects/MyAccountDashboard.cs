using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BDD_eCommerce_Project.Support.HelperLibrary;

namespace BDD_eCommerce_Project.Support.PageObjects {
    internal class MyAccountDashboard {

        private IWebDriver _driver;

        public MyAccountDashboard(IWebDriver driver) {
            this._driver = driver;
        }

        public IWebElement LogoutLink => WaitForElement(_driver, 2, By.LinkText("Logout"));
        public IWebElement OrdersLink => WaitForElement(_driver, 2, By.LinkText("Orders"));

        public void Logout() {
            LogoutLink.Click();
        }

        public void ViewOrders() {
            OrdersLink.Click();
        }
    }
}
