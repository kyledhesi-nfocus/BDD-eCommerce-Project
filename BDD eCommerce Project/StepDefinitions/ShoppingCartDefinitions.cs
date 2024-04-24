using OpenQA.Selenium;
using NUnit.Framework;
using BDD_eCommerce_Project.Support.PageObjects;
using BDD_eCommerce_Project.Support;
using TechTalk.SpecFlow.Infrastructure;
using Allure.Net.Commons;
using System.IO;

namespace BDD_eCommerce_Project.StepDefinitions {
    [Binding]
    public class ShoppingCartDefinitions {
        private readonly ScenarioContext _scenarioContext;
        private readonly ISpecFlowOutputHelper _specFlowOutputHelper;
        private IWebDriver _driver;
        private string _screenshotFilePath;

        /* Steps definitions for applying a coupon */
        public ShoppingCartDefinitions(ScenarioContext scenarioContext, ISpecFlowOutputHelper specFlowOutputHelper) {
            _scenarioContext = scenarioContext;
            _specFlowOutputHelper = specFlowOutputHelper;
            this._driver = (IWebDriver)_scenarioContext["myDriver"];
            this._screenshotFilePath = (string)_scenarioContext["screenshotFilePath"];
        }

        /* Background Step - Enter the shop page */
        [Given(@"I am on the shop page")]
        public void OnTheShopPage() {
            Navigation navigation = new(_driver);
            navigation.ClickLink(Navigation.Link.Shop);     // navigate to the 'Shop' page
        }

        /* Adds a product passed from feature file to the cart */
        [Given(@"I add a product '(.*)' to the cart")]
        public void AddAProductToTheCart(string product) {
            Shop shop = new(_driver);
            try {
                shop.AddProductToCart(product);     // call 'AddProductToCart' to add a product to the cart
                _specFlowOutputHelper.WriteLine($"Successfully added item {product} to the cart");
            } catch {
                Assert.Fail($"Unsuccessfully added item to the cart");      // Assert.Fail if an exception occurs during the attempt to add a product
            }
        }

        /* Navigates and view the product in the cart */
        [Given(@"I view the cart")]
        public void ViewTheCart() {
            Navigation navigation = new(_driver);
            navigation.ClickLink(Navigation.Link.Cart);     // navigate to the 'Cart' page
        }

        /* Enter and apply the coupon code passed from the feature file */
        [When(@"I apply the coupon '(.*)'")]
        public void ApplyTheCoupon(string coupon) {
            Cart cart = new(_driver);
            try {
                cart.EnterCouponCode(coupon);   // call 'EnterCouponCode' to apply the coupon
                _scenarioContext["reducedAmount"] = cart.GetReducedAmount(coupon);      // store 'GetReducedAmount' value into _scenarioContext 
                _specFlowOutputHelper.WriteLine($"Successfully entered coupon: {coupon}");
            } catch (Exception) {
                if (string.IsNullOrEmpty(coupon)) {
                    _specFlowOutputHelper.WriteLine("Coupon is null!");
                }
                HelperLibrary.TakeScreenshot(_driver, _screenshotFilePath + "Coupon Application Error.jpg");
                _specFlowOutputHelper.AddAttachment(_screenshotFilePath + "Coupon Application Error.jpg");
                AllureApi.AddAttachment(_screenshotFilePath + "Coupon Application Error.jpg");
                Assert.Fail($"Unsuccessfully applied coupon: {coupon} to subtotal");  // Assert.Fail if the coupon has not been applied to the cart    
            }
        }

        /* Checks if the correct discount has been applied to the cart */
        [Then(@"the discount '(.*)' should be applied to the subtotal")]
        public void DiscountShouldBeApplied(decimal discount) {
            Cart cart = new(_driver);
            decimal originalPrice = cart.GetOriginalPrice();    // get original price - call 'GetOriginalPrice'
            decimal actualReducedAmount = (decimal)_scenarioContext["reducedAmount"];   // get reduced amount - value from scenarioContext
            decimal expectedReducedAmount = cart.GetOriginalPrice() * (discount/100); // value to be compared with actualReducedAmount
            try {
                Assert.That(actualReducedAmount, Is.EqualTo(expectedReducedAmount), $"Unsuccessfully reduced {discount}% from {originalPrice}");
                _specFlowOutputHelper.WriteLine($"Successfully reduced {discount}% from original price");
            } catch (Exception) {
                HelperLibrary.TakeScreenshot(_driver, _screenshotFilePath + "Discount Exception.jpg");
                _specFlowOutputHelper.AddAttachment(_screenshotFilePath + "Discount Exception.jpg");
                AllureApi.AddAttachment(_screenshotFilePath + "Discount Exception.jpg");
                throw;
            }
        }

        /* Checks if the cart displays the correct total */
        [Then(@"the correct total should be displayed")]
        public void CorrectTotalShouldBeDisplayed(){
            Cart cart = new(_driver);
            decimal originalPrice = cart.GetOriginalPrice();    // get original price - call 'GetOriginalPrice'
            decimal reducedAmount = (decimal)_scenarioContext["reducedAmount"];     // get reduced amount - value from scenarioContext
            decimal shippingPrice = cart.GetShippingPrice();    // get shipping price - call 'GetShippingPrice'
            decimal actualTotalPrice = cart.GetTotalPrice();  // get total price - call 'GetTotalPrice'
            decimal expectedTotalPrice = (originalPrice - reducedAmount) + shippingPrice; // value to be compared with actualTotalPrice
            try {
                Assert.That(actualTotalPrice, Is.EqualTo(expectedTotalPrice), $"Incorrect total was displayed");
                _specFlowOutputHelper.WriteLine($"Correct total was displayed");
            } finally {
                HelperLibrary.TakeScreenshot(_driver, _screenshotFilePath + "Cart Overview.jpg");
                _specFlowOutputHelper.AddAttachment(_screenshotFilePath + "Cart Overview.jpg");
                AllureApi.AddAttachment(_screenshotFilePath + "Cart Overview.jpg");
            }
        }
    }
}
