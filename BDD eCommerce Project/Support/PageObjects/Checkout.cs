using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BDD_eCommerce_Project.Support.HelperLibrary;

namespace BDD_eCommerce_Project.Support.PageObjects {
    internal class Checkout {
        private IWebDriver _driver;
        public Checkout(IWebDriver driver) {
            this._driver = driver;
        }

        public IWebElement FirstNameInput => WaitForElement(_driver, 2, By.Id("billing_first_name"));
        public IWebElement LastNameInput => WaitForElement(_driver, 2, By.Id("billing_last_name"));
        public IWebElement StreetNameInput => WaitForElement(_driver, 2, By.Id("billing_address_1"));
        public IWebElement CityInput => WaitForElement(_driver, 2, By.Id("billing_city"));
        public IWebElement PostcodeInput => WaitForElement(_driver, 2, By.Id("billing_postcode"));
        public IWebElement PhoneNumberInput => WaitForElement(_driver,2, By.Id("billing_phone"));
        public IWebElement EmailInput => WaitForElement(_driver,2,By.Id("billing_email"));
        public IWebElement CheckPaymentsLink => WaitForElement(_driver, 2, By.CssSelector("#payment li.wc_payment_method.payment_method_cheque label"));
        public IWebElement PlaceOrderButton => WaitForElement(_driver, 2, By.Id("place_order"));


        public void EnterBillingDetails(BillingDetails billingDetails) {
            FirstNameInput.Clear();
            FirstNameInput.SendKeys(billingDetails.FirstName);
            LastNameInput.Clear();
            LastNameInput.SendKeys(billingDetails.LastName);
            StreetNameInput.Clear();
            StreetNameInput.SendKeys(billingDetails.StreetName);
            CityInput.Clear();
            CityInput.SendKeys(billingDetails.City);
            PostcodeInput.Clear();
            PostcodeInput.SendKeys(billingDetails.Postcode);
            PhoneNumberInput.Clear();
            PhoneNumberInput.SendKeys(billingDetails.PhoneNumber);
            
            WaitForElementDisabled(_driver, 2, By.CssSelector("#payment li.wc_payment_method.payment_method_cheque label"));
            CheckPaymentsLink.Click();
        }

        public void PlaceOrder(){
            WaitForElementDisabled(_driver, 2, By.CssSelector("blockUI.blockOverlay"));
            PlaceOrderButton.Click();
        }
    }
}
