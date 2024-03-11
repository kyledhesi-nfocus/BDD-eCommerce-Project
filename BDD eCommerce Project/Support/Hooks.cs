﻿using OpenQA.Selenium.Edge;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using BDD_eCommerce_Project.Support.PageObjects;
using NUnit.Framework.Internal;

namespace BDD_eCommerce_Project.Support {

    [Binding]
    public class Hooks {
        private IWebDriver _driver;
        private readonly ScenarioContext _scenarioContext;
        private string browser;
        private string screenshotFilePath;
        public Hooks (ScenarioContext scenarioContext) {
            _scenarioContext = scenarioContext;
        }

        [Before]
        public void Setup() {
            browser = Environment.GetEnvironmentVariable("BROWSER").ToLower();      // get browser variable from mysettings.runsettings
            Console.WriteLine("Browser set to: " + browser);
            
            if (browser == null) {      
                browser = "firefox";
                Console.WriteLine("Browser environment not set: Setting to Firefox");
            }
            switch (browser) {      // calls driver depending on browser variable
                case "edge":
                    _driver = new EdgeDriver();
                    break;
                case "firefox":
                    _driver = new FirefoxDriver();
                    break;
                case "chrome":
                    _driver = new ChromeDriver();
                    break;
                default:
                    Console.WriteLine("Unsupported browser: Using Firefox as default");
                    _driver = new FirefoxDriver();
                    break;
            }

            _driver.Manage().Window.Maximize();
            string baseUrl = TestContext.Parameters["WebAppURL"];       // get website URL from mysettings.runsettings
            _driver.Url = baseUrl;
            
            _scenarioContext["myDriver"] = _driver;

            screenshotFilePath = Path.Combine(TestContext.CurrentContext.WorkDirectory);
            _scenarioContext["screenshotFilePath"] = screenshotFilePath;        

            string username = Environment.GetEnvironmentVariable("SECRET_USERNAME");        // get username from mysettings.runsettings
            string password = Environment.GetEnvironmentVariable("SECRET_PASSWORD");        // get password from mysettings.runsettings
            
            try {       
                LoginMyAccount loginMyAccount = new LoginMyAccount(_driver);
                loginMyAccount.Login(username, password);
                Assert.That(_driver.FindElement(By.LinkText("Log out")).Displayed);     // login with username and password
                Console.WriteLine("Successfully logged in - Begin Test!");
            } catch(Exception) {        
                Console.WriteLine("Configure .runsettings with valid login credentials");
                Assert.Fail("Login unsuccessful - Test Failed");        // assert fail if login is not successful
            }
        }

        [After]
        public void TearDown() {
            MyAccountDashboard myAccountDashboard = new(_driver);
            myAccountDashboard.Logout();        // logout at the end of the test
            
            Thread.Sleep(2000);
            _driver.Quit();     // quit the driver
            Console.WriteLine("Successfully logged out - Test complete!");
        }
    }
}
