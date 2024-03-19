using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System.Globalization;
using SeleniumExtras.WaitHelpers;
using System.Xml.Linq;

namespace BDD_eCommerce_Project.Support {
    public static class HelperLibrary {
        public static IWebElement WaitForElement(IWebDriver driver, int timeoutInSeconds, By locator) {
            WebDriverWait myWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            myWait.Until(drv => drv.FindElement(locator).Displayed);
            return driver.FindElement(locator);
        }
        
        public static void WaitForElementToBeClickable(IWebDriver driver, int timeoutInSeconds, By locator) {
            WebDriverWait myWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            try {
                myWait.Until(ExpectedConditions.ElementToBeClickable(locator));
            } catch(ElementClickInterceptedException) {
                myWait.Until(drv => !drv.FindElement(By.CssSelector("blockUI.blockOverlay")).Enabled);
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
                jsExecutor.ExecuteScript("window.scrollBy(0,323)", "");
            }
         
            ITakesScreenshot? screenshotDriver = driver as ITakesScreenshot;
            if (screenshotDriver != null) {
                Screenshot screenshot = screenshotDriver.GetScreenshot();
                screenshot.SaveAsFile(screenshotFilePath);
            }
        }

    }
}
