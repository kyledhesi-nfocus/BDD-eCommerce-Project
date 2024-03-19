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
        public IWebElement CheckPaymentsLink => WaitForElement(_driver, 5, By.CssSelector("#payment li.wc_payment_method.payment_method_cheque label"));
        public IWebElement CashOnDeliveryLink => WaitForElement(_driver, 5, By.CssSelector("#payment li.wc_payment_method.payment_method_cod label"));
        public IWebElement PlaceOrderButton => WaitForElement(_driver, 2, By.Id("place_order"));

        public bool EnterBillingDetails(BillingDetails billingDetails) {
            // Check each field and return false immediately if any field is not properly filled
            if (!ClearAndEnterKeys(FirstNameInput, billingDetails.FirstName!) ||
                !ClearAndEnterKeys(LastNameInput, billingDetails.LastName!) ||
                !ClearAndEnterKeys(StreetNameInput, billingDetails.StreetName!) ||
                !ClearAndEnterKeys(CityInput, billingDetails.City!) ||
                !ClearAndEnterKeys(PostcodeInput, billingDetails.Postcode!) ||
                !ClearAndEnterKeys(EmailInput, billingDetails.Email!)) {
                return false; // Return false if any field check fails
            }
            return true; // Return true if all field checks pass
        }

        public bool ClearAndEnterKeys(IWebElement inputField, string billingDetailsInput) {
            inputField.Clear();
            if (string.IsNullOrEmpty(billingDetailsInput)) {
                return false;
            } else {
                inputField.SendKeys(billingDetailsInput);
                return true;
            }
        }

        public void ClickPaymentMethod(string paymentMethod) {
            switch (paymentMethod) {
                case "Check payments":
                    WaitForElementToBeClickable(_driver, 5, By.CssSelector("#payment li.wc_payment_method.payment_method_cheque label"));
                    CheckPaymentsLink.Click();
                    break;
                case "Cash on delivery":
                    CashOnDeliveryLink.Click();
                    break;
                default:
                    CheckPaymentsLink.Click();
                    break;
            }
        }
        public void ClickPlaceOrderButton(){
            WaitForElementToBeClickable(_driver, 5, By.Id("place_order"));
            PlaceOrderButton.Click();
            
        }
    }
}
