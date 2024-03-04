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

namespace BDD_eCommerce_Project.Support {

    [Binding]
    public class Hooks {

        private IWebDriver _driver;
        private readonly ScenarioContext _scenarioContext;

        public Hooks (ScenarioContext scenarioContext) {
            _scenarioContext = scenarioContext;
        }

        [Before]
        public void Setup() {

            string browser = Environment.GetEnvironmentVariable("BROWSER").ToLower();
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

            _scenarioContext["myDriver"] = _driver;
            _scenarioContext["baseURL"] = TestContext.Parameters["WebAppURL"];

            _scenarioContext["username"] = Environment.GetEnvironmentVariable("SECRET_USERNAME");
            _scenarioContext["password"] = Environment.GetEnvironmentVariable("SECRET_PASSWORD");

            
        }

        [After]
        public void TearDown() {
            Thread.Sleep(2000);
            _driver.Quit();
        }
    }
}
