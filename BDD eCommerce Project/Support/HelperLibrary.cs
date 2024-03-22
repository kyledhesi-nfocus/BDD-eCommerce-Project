using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System.Globalization;
using SeleniumExtras.WaitHelpers;

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
        public static void TakeScreenshot(IWebDriver driver, string screenshotFilePath) {
            ITakesScreenshot? screenshotDriver = driver as ITakesScreenshot;
            if (screenshotDriver != null) {
                Screenshot screenshot = screenshotDriver.GetScreenshot();
                screenshot.SaveAsFile(screenshotFilePath);
            }
        }

    }
}
