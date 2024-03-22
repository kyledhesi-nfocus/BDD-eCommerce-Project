using OpenQA.Selenium.Edge;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using BDD_eCommerce_Project.Support.PageObjects;

namespace BDD_eCommerce_Project.Support {

    [Binding]
    public class Hooks {
        private IWebDriver? _driver;
        private readonly ScenarioContext _scenarioContext;
        private string? browser;
        public Hooks (ScenarioContext scenarioContext) {
            _scenarioContext = scenarioContext;
        }

        [Before]
        public void Setup() {
            
            browser = Environment.GetEnvironmentVariable("BROWSER");      // get browser type from mysettings.runsettings
            if (browser == null) {      
                browser = "firefox";
                Console.WriteLine("Browser environment not set: Setting to Firefox...");
            } 

            switch (browser.ToLower()) {      // calls driver depending on browser variable
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
            if (string.IsNullOrEmpty(baseUrl)) {   // baseUrl null check
                throw new WebDriverException("The baseURL is null - configure .runsettings");
            }
            _driver.Url = baseUrl;
            _scenarioContext["myDriver"] = _driver;

            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Screenshots\"));
            string screenshotFilePath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Screenshots\");
            _scenarioContext["screenshotFilePath"] = screenshotFilePath;    // store screenshotFilePath into _scenarioContext
        }

        [After]
        public void TearDown() {
            MyAccountDashboard myAccountDashboard = new(_driver!);
            Cart cart = new(_driver!);
            Navigation navigation = new(_driver!);
            
            Console.WriteLine("Test Complete - Begin Teardown...");
            
            navigation.ClickLink(Navigation.Link.Cart);
            
            Console.WriteLine("Checking if cart is empty...");
            
            try {
                cart.ClearCart();   // clear the cart for next test
            } catch {

            }

            Console.WriteLine("Cart is empty - Logging out...");
            
            navigation.ClickLink(Navigation.Link.MyAccount);
            myAccountDashboard.ClickLogoutLink();        // logout of account
            Console.WriteLine("Successfully logged out - Teardown Complete!");
            
            _driver!.Quit();     // quit the driver
        }
    }
}
