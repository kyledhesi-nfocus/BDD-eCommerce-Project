﻿using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System.Globalization;

namespace BDD_eCommerce_Project.Support {
    public static class HelperLibrary {
        public static IWebElement WaitForElement(IWebDriver driver, int timeoutInSeconds, By locator) {
            WebDriverWait myWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            myWait.Until(drv => drv.FindElement(locator).Displayed);
            return driver.FindElement(locator);
        }
        public static bool WaitForElementDisabled(IWebDriver driver, int timeoutInSeconds, By locator) {
            WebDriverWait myWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            try {
                return myWait.Until(drv => {
                    try {
                        var element = drv.FindElement(locator);
                        return !element.Enabled; // Return true if element is disabled
                    } catch (StaleElementReferenceException) {
                        return true;
                    }
                });
            } catch (WebDriverTimeoutException) {
                return false;
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

        public static void ScrollAndTakeScreenshot(IWebDriver driver, string screenshotFile) {       
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("window.scrollBy(0,323)", "");

            ITakesScreenshot? screenshotDriver = driver as ITakesScreenshot;
            if (screenshotDriver != null) {
                Screenshot screenshot = screenshotDriver.GetScreenshot();
                screenshot.SaveAsFile(screenshotFile);
            }
        }

    }
}
