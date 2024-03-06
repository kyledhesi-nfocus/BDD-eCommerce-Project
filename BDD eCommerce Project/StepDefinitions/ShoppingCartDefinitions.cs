using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using BDD_eCommerce_Project.Support.PageObjects;
using BDD_eCommerce_Project.Support;

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
            navigation.ClickLink(Navigation.Link.Shop);
            Console.WriteLine("Successfully entered shop");
        }

        [When(@"I add an item to the cart")]
        public void WhenIAddAnItemToTheCart() {
            shop.AddToCart(Shop.Product.Sunglasses);
            Console.WriteLine($"Successfully added item {Shop.Product.HoodieWithLogo} to the cart");
        }

        [When(@"I view the cart")]
        public void WhenIViewTheCart() {
            navigation.ClickLink(Navigation.Link.Cart);
            Console.WriteLine("Successfully entered cart");
        }

        [When(@"I apply the coupon '(.*)'")]
        public void WhenIApplyTheCoupon(string coupon) {
            cart.EnterCouponCode(coupon);
            Console.WriteLine($"Successfully entered coupon:{coupon}");

            cart.WaitForAlert();
            HelperLibrary.TakeScreenshot(driver, screenshotFilePath + "Coupon Applied.jpg");
            TestContext.WriteLine($"Attaching screenshot to report");
            TestContext.AddTestAttachment(screenshotFilePath + "Coupon Applied.jpg", "Coupon applied");

            if (cart.CouponAlert.Contains("does not exist!")) {
                HelperLibrary.TakeScreenshot(driver, screenshotFilePath + "Coupon Error.jpg");
                TestContext.WriteLine($"Attaching screenshot to report");
                TestContext.AddTestAttachment(screenshotFilePath + "Coupon Error.jpg", "Coupon not applied");

                cart.RemoveCouponAndItem();
                navigation.ClickLink(Navigation.Link.MyAccount);
                Assert.Fail($"Unsuccessfully applied the coupon: {coupon}");   
            }

            Console.WriteLine($"Successfully applied coupon:{coupon}");
        }

        [Then(@"the coupon '(.*)' should be applied to the subtotal")]
        public void ThenTheCouponShouldBeAppliedToTheSubtotal(string coupon) {  
            
            decimal originalPrice = cart.GetOriginalPrice(); // Get orignal price
            decimal reducedAmount = cart.GetReducedAmount(coupon); // Get reduced amount
            decimal shippingPrice = cart.GetShippingPrice(); // Get shipping price
            decimal totalPrice = cart.GetTotalPrice(); // Get total price

            decimal couponDiscount = 0M;
            if(coupon == "edgewords") {
                couponDiscount += 0.10M;
            } else if (coupon == "nfocus") {
                couponDiscount += 0.25M;
            }

            decimal discountAmount = originalPrice * couponDiscount; // Value to be compared with reducedAmount
            decimal total = (originalPrice - reducedAmount) + shippingPrice; // Value to be compared with totalPrice

            try {
                Assert.That(discountAmount, Is.EqualTo(reducedAmount));
                Console.WriteLine($"Successfully reduced {couponDiscount*100}% from original price");
                TestContext.WriteLine($"Subtotal price displayed: {originalPrice} | Reduced amount displayed: {reducedAmount}");
            } catch {
                TestContext.WriteLine($"Subtotal price displayed: {originalPrice} | Reduced amount displayed: {reducedAmount} | Expected reduced amount: {discountAmount}");
                cart.RemoveCouponAndItem();
                Assert.Fail($"Unsuccessfully reduced {couponDiscount*100}% from subtotal price");
            }

            try {
                Assert.That(total, Is.EqualTo(totalPrice));
                Console.WriteLine("Successfully calculated total after coupon & shippping");
                TestContext.WriteLine($"Total price displayed {totalPrice} | (original price {originalPrice} - reduced amount {reducedAmount}) + shipping cost {shippingPrice}");
            } catch {
                TestContext.WriteLine($"Total price displayed {totalPrice} | Expected value: {total} - Attaching screenshot to report");
                cart.RemoveCouponAndItem();
                Assert.Fail("Unsuccessfully calculated total after coupon & shipping");
            }

            cart.RemoveCouponAndItem();
            Console.WriteLine("Successfully removed items");

            navigation.ClickLink(Navigation.Link.MyAccount);
            Console.WriteLine("Successfully entered My account");
        }
    }
}
