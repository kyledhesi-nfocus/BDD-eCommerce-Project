using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using BDD_eCommerce_Project.Support.PageObjects;
using BDD_eCommerce_Project.Support;
using NUnit.Framework.Internal;

namespace BDD_eCommerce_Project.StepDefinitions {
    [Binding]
    public class ShoppingCartDefinitions { 
        private readonly ScenarioContext _scenarioContext;
        private IWebDriver driver;
        private readonly Navigation navigation;
        private readonly Shop shop;
        private readonly Cart cart;
        private string screenshotFilePath;
        public ShoppingCartDefinitions(ScenarioContext scenarioContext) {
            _scenarioContext = scenarioContext;

            this.driver = (IWebDriver)_scenarioContext["myDriver"];
            this.screenshotFilePath = (string)_scenarioContext["screenshotFilePath"];
            this.navigation = new(driver);
            this.shop = new(driver);
            this.cart = new(driver);
        }

        [Given(@"I am on the shop page")]
        public void GivenIAmOnTheShopPage() {
            navigation.ClickLink(Navigation.Link.Shop);     // navigate to the 'Shop' page
            Console.WriteLine("Successfully entered shop");
        }

        [When(@"I add an item to the cart")]
        public void WhenIAddAnItemToTheCart() {
            shop.AddToCart(Shop.Product.Sunglasses);    // // adds a specific product 'Sunglasses' to the cart
            Console.WriteLine($"Successfully added item {Shop.Product.Sunglasses} to the cart");
        }

        [When(@"I view the cart")]
        public void WhenIViewTheCart() {
            navigation.ClickLink(Navigation.Link.Cart);     // navigate to the 'Cart' page
            Console.WriteLine("Successfully entered cart");
        }

        [When(@"I apply the coupon '(.*)'")]
        public void WhenIApplyTheCoupon(string coupon) {
            try {
                cart.EnterCouponCode(coupon);   // enter and submit the coupon code into field
                cart.WaitForAlert();
                HelperLibrary.TakeScreenshot(driver, screenshotFilePath + "Coupon Applied.jpg");    // take screenshot of coupon being applied confirmation
                TestContext.WriteLine($"Attaching screenshot to report");
                TestContext.AddTestAttachment(screenshotFilePath + "Coupon Applied.jpg", "Coupon applied");

                Console.WriteLine($"Successfully applied coupon:{coupon}");
            } catch(Exception) {
                Assert.Fail("Unsuccessfully entered coupon");   // assert fail if the coupon is not entered and submitted
            }
        }

        [Then(@"the coupon '(.*)' should be applied to the subtotal")]
        public void ThenTheCouponShouldBeAppliedToTheSubtotal(string coupon) {

            decimal originalPrice = cart.GetOriginalPrice(); // get orignal price
            decimal reducedAmount = 0; // initialise variable to hold the amount reduced value
            try {
                reducedAmount += cart.GetReducedAmount(coupon);     // get reduced amount and updated reducedAmount
            } catch(Exception) {
                TestContext.WriteLine($"Unsuccessfully applied coupon: {coupon} to subtotal");
                cart.RemoveCouponAndItem();
                navigation.ClickLink(Navigation.Link.MyAccount);
                Assert.Fail($"Invalid coupon - {coupon}");  // assert fail if the coupon has not been applied to the cart
            }
            decimal shippingPrice = cart.GetShippingPrice(); // get shipping price
            decimal totalPrice = cart.GetTotalPrice(); // get total price

            decimal couponDiscount = 0M;    // initalise variable to store discount from a the coupon
            if (coupon == "edgewords") {
                couponDiscount += 0.15M;    // 15% discount if coupon matches 'edgewords'
            } else if (coupon == "nfocus") {
                couponDiscount += 0.25M;    // 25% discount if coupon matches 'nfocus;
            }

            decimal discountAmount = originalPrice * couponDiscount; // value to be compared with reducedAmount
            decimal total = (originalPrice - reducedAmount) + shippingPrice; // value to be compared with totalPrice

            try {
                Assert.That(reducedAmount, Is.EqualTo(discountAmount));     // check if the amount reduced value is correct and matches with the expected coupon discount
                Console.WriteLine($"Successfully reduced {couponDiscount * 100}% from original price");
                TestContext.WriteLine($"Subtotal price displayed: {originalPrice} | Reduced amount displayed: {reducedAmount}");
            } catch {
                TestContext.WriteLine($"Subtotal price displayed: {originalPrice} | Reduced amount displayed: {reducedAmount} | Expected reduced amount: {discountAmount}");
                HelperLibrary.ScrollAndTakeScreenshot(driver, screenshotFilePath + "Subtotal discount error.jpg");
                TestContext.WriteLine($"Attaching screenshot to report");
                TestContext.AddTestAttachment(screenshotFilePath + "Subtotal discount error.jpg", "Discount error");
                cart.RemoveCouponAndItem();
                navigation.ClickLink(Navigation.Link.MyAccount);
                Assert.Fail($"Unsuccessfully reduced {couponDiscount * 100}% from subtotal price");     // assert fail if the amount reduced does not match the expected coupon discount 
            }

            try {
                Assert.That(total, Is.EqualTo(totalPrice));     // check if the cart totals successfully add up to the overall total 
                Console.WriteLine("Successfully calculated total after coupon & shippping");
                HelperLibrary.ScrollAndTakeScreenshot(driver, screenshotFilePath + "Cart overview.jpg");
                TestContext.WriteLine($"Total price displayed {totalPrice} | (original price {originalPrice} - reduced amount {reducedAmount}) + shipping cost {shippingPrice} - Attaching screenshot to report");
                TestContext.AddTestAttachment(screenshotFilePath + "Cart overview.jpg", "Cart overview");
            } catch {
                TestContext.WriteLine($"Total price displayed {totalPrice} | Expected value: {total} - Attaching screenshot to report");
                HelperLibrary.ScrollAndTakeScreenshot(driver, screenshotFilePath + "Total error.jpg");
                TestContext.WriteLine($"Attaching screenshot to report");
                TestContext.AddTestAttachment(screenshotFilePath + "Total error.jpg", "Calculating total error");
                cart.RemoveCouponAndItem();
                navigation.ClickLink(Navigation.Link.MyAccount);
                Assert.Fail("Unsuccessfully calculated total after coupon & shipping");     // assert fail if the total is incorrect
            }

            cart.RemoveCouponAndItem();     // remove the coupon and item
            Console.WriteLine("Successfully removed items");

            navigation.ClickLink(Navigation.Link.MyAccount);    // navigate to 'My account' page
            Console.WriteLine("Successfully entered My account");
        }
    }
}
