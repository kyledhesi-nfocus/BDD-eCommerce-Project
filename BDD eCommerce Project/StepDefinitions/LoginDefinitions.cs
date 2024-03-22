using BDD_eCommerce_Project.Support.PageObjects;
using NUnit.Framework;
using OpenQA.Selenium;

namespace BDD_eCommerce_Project.StepDefinitions {
    [Binding]
    internal class LoginDefinitions {
        private readonly ScenarioContext _scenarioContext;
        private IWebDriver _driver;
        public LoginDefinitions(ScenarioContext scenarioContext) {
            _scenarioContext = scenarioContext;
            this._driver = (IWebDriver)_scenarioContext["myDriver"];
        }

        /* Background Step - Login Process */
        [Given(@"I am logged in as a user")]
        public void GivenIAmLoggedIn() {
            string? username = Environment.GetEnvironmentVariable("SECRET_USERNAME");        // get username from mysettings.runsettings
            string? password = Environment.GetEnvironmentVariable("SECRET_PASSWORD");        // get password from mysettings.runsettings

            if (string.IsNullOrEmpty(username)) {
                throw new Exception("Username is null - configure .runsettings file");
            } else if (string.IsNullOrEmpty(password)) {
                throw new Exception("Password is null - .configure .runsettings file");
            }

            try {
                LoginMyAccount loginMyAccount = new LoginMyAccount(_driver);
                loginMyAccount.DismissBanner();
                loginMyAccount.EnterLoginDetails(username, password);
                MyAccountDashboard myAccountDashboard = new(_driver);
                
                if (myAccountDashboard.LogoutLink.Displayed) {
                    Console.WriteLine("Successfully logged in - Begin Test!");
                }   
            }
            catch (Exception) {
                Console.WriteLine("Configure .runsettings with valid login credentials");
                Assert.Fail("Login unsuccessful - Test Failed");        // Assert.Fail if login is unsuccessful
            }
        }
    }
}
