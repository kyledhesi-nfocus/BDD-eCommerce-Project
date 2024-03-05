using OpenQA.Selenium.Edge;
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

        public Hooks (ScenarioContext scenarioContext) {
            _scenarioContext = scenarioContext;
        }

        [Before]
        public void Setup() {
            browser = Environment.GetEnvironmentVariable("BROWSER").ToLower();
            Console.WriteLine("Browser set to: " + browser);
            
            if (browser == null) {
                browser = "firefox";
                Console.WriteLine("Browser environment not set: Setting to Firefox");
            }
            switch (browser) {
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
            string baseUrl = TestContext.Parameters["WebAppURL"];
            _driver.Url = baseUrl;
            
            _scenarioContext["myDriver"] = _driver;

            string username = Environment.GetEnvironmentVariable("SECRET_USERNAME");
            string password = Environment.GetEnvironmentVariable("SECRET_PASSWORD");

            try {
                LoginMyAccount loginMyAccount = new LoginMyAccount(_driver);
                loginMyAccount.Login(username, password);
                Assert.That(_driver.FindElement(By.LinkText("Log out")).Displayed);
                Console.WriteLine("Successfully logged in - Begin Test!");
            } catch(Exception) {
                Assert.Fail("Unsuccessfully logged in - Test Failed");
            }
        }

        [After]
        public void TearDown() {
            MyAccountDashboard myAccountDashboard = new(_driver);
            myAccountDashboard.Logout();
            
            Thread.Sleep(2000);
            _driver.Quit();
            Console.WriteLine("Successfully logged out - Test complete!");
        }
    }
}
