using BDD_eCommerce_Project.Support.PageObjects;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_eCommerce_Project.StepDefinitions {
    [Binding]
    internal class LoginDefinitions {

        private readonly ScenarioContext _scenarioContext;
        private IWebDriver _driver;
        private string screenshotFilePath;

        public LoginDefinitions(ScenarioContext scenarioContext) {
            _scenarioContext = scenarioContext;

            this._driver = (IWebDriver)_scenarioContext["myDriver"];
            this.screenshotFilePath = (string)_scenarioContext["screenshotFilePath"];
        }

        [Given(@"I am logged in as a user")]
        public void GivenIAmLoggedIn() {
            string username = Environment.GetEnvironmentVariable("SECRET_USERNAME");        // get username from mysettings.runsettings
            string password = Environment.GetEnvironmentVariable("SECRET_PASSWORD");        // get password from mysettings.runsettings
            try {
                LoginMyAccount loginMyAccount = new LoginMyAccount(_driver);
                loginMyAccount.Login(username, password);
                Assert.That(_driver.FindElement(By.LinkText("Log out")).Displayed);     // login with username and password
                Console.WriteLine("Successfully logged in - Begin Test!");
            }
            catch (Exception) {
                Console.WriteLine("Configure .runsettings with valid login credentials");
                Assert.Fail("Login unsuccessful - Test Failed");        // assert fail if login is not successful
            }
        }

    }
}
