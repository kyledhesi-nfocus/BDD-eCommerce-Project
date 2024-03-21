using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System.Globalization;
using SeleniumExtras.WaitHelpers;
using System.Xml.Linq;
using System.Diagnostics;

namespace BDD_eCommerce_Project.Support {
    public static class HelperLibrary {
        public static IWebElement WaitForElement(IWebDriver driver, int timeoutInSeconds, By locator) {
            WebDriverWait myWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            myWait.Until(drv => drv.FindElement(locator).Displayed);
            return driver.FindElement(locator);
        }
        
        public static void WaitForElementToBeClickable(IWebDriver driver, int timeoutInSeconds, By locator) {
            WebDriverWait myWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            myWait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        public static void WaitForElementToBeDisabled(IWebDriver driver, int timeoutInSeconds, By locator) {
            WebDriverWait myWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            try {
                myWait.Until(drv => !drv.FindElement(locator).Enabled);
            } catch (Exception) {
               
            }
        }

        public static decimal ToDecimal(string str) {
            var style = NumberStyles.Currency | NumberStyles.AllowCurrencySymbol;
            var provider = new CultureInfo("en-GB");
            return decimal.Parse(str, style, provider);
        }
        public static void TakeScreenshot(IWebDriver driver, string screenshotFilePath, bool scroll) {
            if (scroll == true) {
                IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
                jsExecutor.ExecuteScript("window.scrollBy(0,85)", "");
            }
         
            ITakesScreenshot? screenshotDriver = driver as ITakesScreenshot;
            if (screenshotDriver != null) {
                Screenshot screenshot = screenshotDriver.GetScreenshot();
                screenshot.SaveAsFile(screenshotFilePath);
            }
        }

    }
}
