using OpenQA.Selenium;
using NUnit.Framework;
using BDD_eCommerce_Project.Support.PageObjects;
using BDD_eCommerce_Project.Support;
using TechTalk.SpecFlow.Infrastructure;

namespace BDD_eCommerce_Project.StepDefinitions {
    [Binding]
    public class ShoppingCartDefinitions {
        private readonly ScenarioContext _scenarioContext;
        private readonly ISpecFlowOutputHelper _specFlowOutputHelper;
        private IWebDriver driver;
        private string screenshotFilePath;

        /* Steps definitions for applying a coupon */
        public ShoppingCartDefinitions(ScenarioContext scenarioContext, ISpecFlowOutputHelper specFlowOutputHelper) {
            _scenarioContext = scenarioContext;
            _specFlowOutputHelper = specFlowOutputHelper;
            this.driver = (IWebDriver)_scenarioContext["myDriver"];
            this.screenshotFilePath = (string)_scenarioContext["screenshotFilePath"];
        }

        /* Background Step - Enter the shop page */
        [Given(@"I am on the shop page")]
        public void OnTheShopPage() {
            Navigation navigation = new(driver);
            navigation.ClickLink(Navigation.Link.Shop);     // navigate to the 'Shop' page
            _specFlowOutputHelper.WriteLine("Successfully entered shop");
        }

        /* Adds a product passed from feature file to the cart */
        [Given(@"I add a product '(.*)' to the cart")]
        public void AddAProductToTheCart(string product) {
            Shop shop = new(driver);
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
            Navigation navigation = new(driver);
            navigation.ClickLink(Navigation.Link.Cart);     // navigate to the 'Cart' page
            _specFlowOutputHelper.WriteLine("Successfully entered cart");
        }

        /* Enter and apply the coupon code passed from the feature file */
        [When(@"I apply the coupon '(.*)'")]
        public void ApplyTheCoupon(string coupon) {
            Cart cart = new(driver);
            try {
                cart.EnterCouponCode(coupon);   // call 'EnterCouponCode' to apply the coupon
                _scenarioContext["reducedAmount"] = cart.GetReducedAmount(coupon);      // store 'GetReducedAmount' value into _scenarioContext 
                _specFlowOutputHelper.WriteLine($"Successfully entered coupon:{coupon}");
            } catch (Exception) {
                if (string.IsNullOrEmpty(coupon)) {
                    _specFlowOutputHelper.WriteLine("Coupon is null!");
                }
                HelperLibrary.TakeScreenshot(driver, screenshotFilePath + "Coupon Application Error.jpg");
                _specFlowOutputHelper.AddAttachment(screenshotFilePath + "Coupon Application Error.jpg");

                Assert.Fail($"Unsuccessfully applied coupon: {coupon} to subtotal - Attatching screenshot to report...");  // Assert.Fail if the coupon has not been applied to the cart    
            }
        }

        /* Checks if the correct discount has been applied to the cart */
        [Then(@"the discount '(.*)' should be applied to the subtotal")]
        public void DiscountShouldBeApplied(decimal discount) {
            Cart cart = new(driver);

            decimal originalPrice = cart.GetOriginalPrice();    // get original price - call 'GetOriginalPrice'
            decimal actualReducedAmount = (decimal)_scenarioContext["reducedAmount"];   // get reduced amount - value from scenarioContext
            decimal expectedReducedAmount = cart.GetOriginalPrice() * (discount / 100); // value to be compared with actualReducedAmount
 
            if (actualReducedAmount == expectedReducedAmount) {
                _specFlowOutputHelper.WriteLine($"Successfully reduced {discount}% from original price\nSubtotal price displayed: {originalPrice} | Reduced amount displayed: {actualReducedAmount}");
            } else {
                _specFlowOutputHelper.WriteLine($"Coupon should give {discount}%: Subtotal price: {originalPrice} | Reduced amount: {actualReducedAmount} | Expected reduced amount: {expectedReducedAmount} - Attaching screenshot to report");
                Assert.Fail($"Unsuccessfully reduced {discount}% from {originalPrice}");
            }
        }

        /* Checks if the cart displays the correct total */ 
        [Then(@"the correct total should be displayed")]
        public void CorrectTotalShouldBeDisplayed(){
            Cart cart = new(driver);

            decimal originalPrice = cart.GetOriginalPrice();    // get original price - call 'GetOriginalPrice'
            decimal reducedAmount = (decimal)_scenarioContext["reducedAmount"];     // get reduced amount - value from scenarioContext
            decimal shippingPrice = cart.GetShippingPrice();    // get shipping price - call 'GetShippingPrice'
            decimal actualTotalPrice = cart.GetTotalPrice();  // get total price - call 'GetTotalPrice'
            decimal expectedTotalPrice = (originalPrice - reducedAmount) + shippingPrice; // value to be compared with actualTotalPrice
            
            Assert.That(actualTotalPrice, Is.EqualTo(expectedTotalPrice), $"Unsuccessfully calculated total after coupon & shipping\nTotal price displayed {actualTotalPrice} | Expected value: {expectedTotalPrice} - Attaching screenshot to report...");             
            _specFlowOutputHelper.WriteLine($"Successfully calculated total after coupon & shippping\nTotal price displayed {actualTotalPrice} | (original price {originalPrice} - reduced amount {reducedAmount}) + shipping cost {shippingPrice} - Attaching screenshot to report...");

            HelperLibrary.TakeScreenshot(driver, screenshotFilePath + "Cart Overview.jpg");
            _specFlowOutputHelper.AddAttachment(screenshotFilePath + "Cart Overview.jpg");
        }
    }
}
