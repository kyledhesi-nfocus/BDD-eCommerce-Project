using OpenQA.Selenium;
using static BDD_eCommerce_Project.Support.HelperLibrary;

namespace BDD_eCommerce_Project.Support.PageObjects {
    
    class LoginMyAccount {

        private IWebDriver _driver;
        public LoginMyAccount(IWebDriver driver) {
            this._driver = driver;
        }
        private IWebElement _usernameField => WaitForElement(_driver, 3, By.Id("username"));
        private IWebElement _passwordField => WaitForElement(_driver, 3, By.Id("password"));
        private IWebElement _loginButton => WaitForElement(_driver, 3, By.CssSelector("#customer_login button"));
        private IWebElement _dismiss => WaitForElement(_driver, 3, By.ClassName("woocommerce-store-notice__dismiss-link"));

        public void EnterLoginDetails(string username, string password) {
            _usernameField.Clear();
            _usernameField.Click();
            _usernameField.SendKeys(username);

            _passwordField.Clear();
            _passwordField.Click();
            _passwordField.SendKeys(password);

            _loginButton.Click();
        }

        public void DismissBanner() {
            try {
                _dismiss.Click();
            } catch {

            }
        }
    }
}
