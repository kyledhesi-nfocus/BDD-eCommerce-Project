using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BDD_eCommerce_Project.Support.HelperLibrary;

namespace BDD_eCommerce_Project.Support.PageObjects {
    internal class MyAccountOrders {

        private IWebDriver _driver;

        public MyAccountOrders(IWebDriver driver) {
            this._driver = driver;
        }

        public string OrderID => WaitForElement(_driver, 2, By.CssSelector("#post-7 tbody > tr:nth-child(1) td.woocommerce-orders-table__cell.woocommerce-orders-table__cell-order-number a")).Text;

        public string GetOrderID() {
            return OrderID;
        }
    }
}
