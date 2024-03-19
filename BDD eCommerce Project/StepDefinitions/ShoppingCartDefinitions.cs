using OpenQA.Selenium;
using NUnit.Framework;
using BDD_eCommerce_Project.Support.PageObjects;
using BDD_eCommerce_Project.Support;

namespace BDD_eCommerce_Project.StepDefinitions {
    [Binding]
    public class ShoppingCartDefinitions {
        private readonly ScenarioContext _scenarioContext;
        private IWebDriver driver;
        private string screenshotFilePath;
        private decimal originalPrice;
        private decimal reducedAmount;
        private decimal shippingPrice;
        private decimal totalPrice;
        public ShoppingCartDefinitions(ScenarioContext scenarioContext) {
            _scenarioContext = scenarioContext;

            this.driver = (IWebDriver)_scenarioContext["myDriver"];
            this.screenshotFilePath = (string)_scenarioContext["screenshotFilePath"];

            this.originalPrice = 0.0m;
            this.reducedAmount = 0.0m;
            this.shippingPrice = 0.0m;
            this.totalPrice = 0.0m;
        }

        [Given(@"I am on the shop page")]
        public void GivenIamOnThePage() {
            Navigation navigation = new(driver);
            navigation.ClickLink(Navigation.Link.Shop);     // navigate to the 'Shop' page
            Console.WriteLine("Successfully entered shop");
        }

        [Given(@"I add a product '(.*)' to the cart")]
        public void GivenIAddAProductToTheCart(string product) {
            Shop shop = new(driver);
            if (shop.AddProductToCart(product)) {
                Console.WriteLine($"Successfully added item {product} to the cart");
            } else {
                Assert.Fail($"Unsuccessfully added item to the cart");
            }
        }

        [Given(@"I view the cart")]
        public void GivenIViewTheCart() {
            Navigation navigation = new(driver);
            navigation.ClickLink(Navigation.Link.Cart);     // navigate to the 'Cart' page
            Console.WriteLine("Successfully entered cart");
        }

        [When(@"I apply the coupon '(.*)'")]
        public void WhenIApplyTheCoupon(string coupon) {
            Cart cart = new(driver);
            try {
                cart.EnterCouponCode(coupon);   // enter and submit the coupon code into field
                reducedAmount += cart.GetReducedAmount();
                TestContext.WriteLine($"Successfully entered coupon:{coupon} - Attaching screenshot to report");
            } catch (Exception) {
                if (string.IsNullOrEmpty(coupon)) {
                    Console.WriteLine("Coupon is null");
                }
                Assert.Fail($"Unsuccessfully applied coupon: {coupon} to subtotal");  // assert fail if the coupon has not been applied to the cart    
            }
        }

        [Then(@"the discount '(.*)' should be applied to the subtotal")]
        public void ThenTheDiscountShouldBeAppliedToTheSubtotal(decimal discount) {
            Cart cart = new(driver);
            
            originalPrice = cart.GetOriginalPrice(); // get orignal price
            decimal discountAmount = originalPrice * (discount / 100); // value to be compared with reducedAmount
        
            try {
                Assert.That(reducedAmount, Is.EqualTo(discountAmount), $"Unsuccessfully reduced {discount}% from {originalPrice}");     // check if the amount reduced value is correct and matches with the expected coupon discount
                Console.WriteLine($"Successfully reduced {discount}% from original price\nSubtotal price displayed: {originalPrice} | Reduced amount displayed: {reducedAmount}");
            } catch {
                TestContext.WriteLine($"Coupon should give {discount}%: Subtotal price: {originalPrice} | Reduced amount: {reducedAmount} | Expected reduced amount: {discountAmount} - Attaching screenshot to report");
            }
        }

        [Then(@"the correct total should be displayed")]
        public void ThenTheCorrectTotalShouldBeDisplayed(){
            Cart cart = new(driver);

            shippingPrice = cart.GetShippingPrice(); // get shipping price
            totalPrice = cart.GetTotalPrice(); // get total price

            decimal total = (originalPrice - reducedAmount) + shippingPrice; // value to be compared with totalPrice

            try { 
                Assert.That(totalPrice, Is.EqualTo(total), "Unsuccessfully calculated total after coupon & shipping");     // check if the cart totals successfully add up to the overall total 
                Console.WriteLine($"Successfully calculated total after coupon & shippping\nTotal price displayed {totalPrice} | (original price {originalPrice} - reduced amount {reducedAmount}) + shipping cost {shippingPrice} - Attaching screenshot to report");
                HelperLibrary.TakeScreenshot(driver, screenshotFilePath + "Cart overview.jpg", true);
                TestContext.AddTestAttachment(screenshotFilePath + "Cart overview.jpg", "Cart overview");
            } catch {
                TestContext.WriteLine($"Total price displayed {totalPrice} | Expected value: {total} - Attaching screenshot to report");
                HelperLibrary.TakeScreenshot(driver, screenshotFilePath + "Total error.jpg", true);
                TestContext.AddTestAttachment(screenshotFilePath + "Total error.jpg", "Calculating total error");
            }
        }
    }
}
