using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static BDD_eCommerce_Project.Support.HelperLibrary;

namespace BDD_eCommerce_Project.Support.PageObjects {
    internal class OrderReceived {

        private IWebDriver _driver;

        public OrderReceived(IWebDriver driver) {
            this._driver = driver;
        }

        public string OrderNumber => WaitForElement(_driver, 2, By.CssSelector("#post-6  li.woocommerce-order-overview__order.order strong")).Text;
        public bool OrderRecieved(){
            try {
                WaitForElement(_driver, 5, By.CssSelector("#post-6 > header > h1"));
                return true;
            } catch(Exception){
                return false;
            }
        }
        public string GetOrderNumber(){
            return OrderNumber;
        }
    }
}
