using OpenQA.Selenium.Edge;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using BDD_eCommerce_Project.Support.PageObjects;
using System.Reflection;

namespace BDD_eCommerce_Project.Support {

    [Binding]
    public class Hooks {
        private IWebDriver? _driver;
        private readonly ScenarioContext _scenarioContext;
        private string? browser;
        private string? screenshotFilePath;
        public Hooks (ScenarioContext scenarioContext) {
            _scenarioContext = scenarioContext;
        }

        [Before]
        public void Setup() {
            
            browser = Environment.GetEnvironmentVariable("BROWSER")!.ToLower();      // get browser variable from mysettings.runsettings
            
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

            Console.WriteLine($"Browser set to: {browser}");

            _driver.Manage().Window.Maximize();
            string? baseUrl = TestContext.Parameters["WebAppURL"];       // get website URL from mysettings.runsettings
            
            if (string.IsNullOrEmpty(baseUrl)) {   // Check if baseUrl is null
                throw new WebDriverException("The baseURL is null - configure .runsettings");
            }
            _driver.Url = baseUrl;
            _scenarioContext["myDriver"] = _driver;

            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Screenshots\"));
            string screenshotFilePath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Screenshots\");
            _scenarioContext["screenshotFilePath"] = screenshotFilePath;
        }

        [After]
        public void TearDown() {
            try {
                MyAccountDashboard myAccountDashboard = new(_driver!);
                myAccountDashboard.Logout();        // logout at the end of the test
                Console.WriteLine("Successfully logged out - Test complete!");
            } catch {
                Console.WriteLine("Test Complete!");
            }
            _driver!.Quit();     // quit the driver
        }
    }
}
