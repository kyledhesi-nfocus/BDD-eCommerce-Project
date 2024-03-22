using BDD_eCommerce_Project.Support;
using BDD_eCommerce_Project.Support.PageObjects;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow.Infrastructure;

namespace BDD_eCommerce_Project.StepDefinitions {
    [Binding]
    internal class LoginDefinitions {
        private readonly ScenarioContext _scenarioContext;
        private IWebDriver _driver;
        private readonly ISpecFlowOutputHelper _specFlowOutputHelper;
        private string _screenshotFilePath;
        public LoginDefinitions(ScenarioContext scenarioContext, ISpecFlowOutputHelper specFlowOutputHelper) {
            _scenarioContext = scenarioContext;
            _specFlowOutputHelper = specFlowOutputHelper;
            this._driver = (IWebDriver)_scenarioContext["myDriver"];
            this._screenshotFilePath = (string)_scenarioContext["screenshotFilePath"];
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
                loginMyAccount.EnterLoginDetails(username, password);       // call 'EnterLoginDetails' to login to account
                MyAccountDashboard myAccountDashboard = new(_driver);
                
                if (myAccountDashboard.LogoutLink.Displayed) {
                    _specFlowOutputHelper.WriteLine("Successfully logged in - Begin Test!");
                }   
            }
            catch (Exception) {
                _specFlowOutputHelper.WriteLine("Configure .runsettings with valid login credentials");
                HelperLibrary.TakeScreenshot(_driver, _screenshotFilePath + "Unsuccessful Login.jpg");
                _specFlowOutputHelper.AddAttachment(_screenshotFilePath + "Unsuccessful Login.jpg");
                Assert.Fail("Login unsuccessful - Test Failed");        // Assert.Fail if login is unsuccessful
            }
        }
    }
}
