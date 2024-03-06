using BDD_eCommerce_Project.Support;
using BDD_eCommerce_Project.Support.PageObjects;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_eCommerce_Project.StepDefinitions {
    [Binding]
    public class CheckoutDefinitions {

        private readonly ScenarioContext _scenarioContext;
        private IWebDriver driver;
        private readonly Navigation navigation;
        private readonly Shop shop;
        private readonly Cart cart;
        private readonly BillingDetails billingDetails;
        private readonly Checkout checkout;
        private readonly OrderReceived orderReceived;
        private readonly MyAccountDashboard myAccountDashboard;
        private readonly MyAccountOrders myAccountOrders;
        private string orderNumber;
        private string screenshotFilePath;

        public CheckoutDefinitions(ScenarioContext scenarioContext) {
            _scenarioContext = scenarioContext;

            this.driver = (IWebDriver)_scenarioContext["myDriver"];
            this.screenshotFilePath = (string)_scenarioContext["screenshotFilePath"];
            this.navigation = new(driver);
            this.shop = new(driver);
            this.cart = new(driver);
            this.checkout = new(driver);
            this.billingDetails = new(firstName: "King", lastName: "Charles", streetName: "Buckingham Palace Road", city: "London", postcode: "SW1A 1AA", phoneNumber: "0798347190321", email: "example@email.co.uk");
            this.myAccountDashboard = new(driver);
            this.myAccountOrders = new(driver);
            this.orderReceived = new(driver);  
        }

  
        [Given(@"I have at least one product in my cart")]
        public void GivenIHaveAtLeastOneProductInMyCart() {
            navigation.ClickLink(Navigation.Link.Shop);
            Console.WriteLine("Successfully entered shop");

            shop.AddToCart(Shop.Product.HoodieWithLogo);
            Console.WriteLine($"Successfully added item {Shop.Product.HoodieWithLogo} to the cart");
        }

        [Given(@"I am on the cart page")]
        public void GivenIAmOnTheCartPage() {
            navigation.ClickLink(Navigation.Link.Cart);
            Console.WriteLine("Successfully entered cart");
        }

        [When(@"I click the Proceed to checkout button")]
        public void WhenIClickTheProceedToCheckoutButton() {
            cart.Checkout();
            Console.WriteLine("Successfully clicked Proceed to checkout button");
            Console.WriteLine("Successfully entered checkout");
        }

        [When(@"I enter my billing details")]
        public void WhenIEnterMyBillingDetails() {
            checkout.EnterBillingDetails(billingDetails);
            Console.WriteLine("Successfully entered billing details - Attaching screenshot to report");

            HelperLibrary.TakeScreenshot(driver, screenshotFilePath + "Billing details.jpg");
            TestContext.AddTestAttachment(screenshotFilePath + "Billing details.jpg", "Billing details applied");
        }

        [When(@"I click the Place order button")]
        public void WhenIClickThePlaceOrderButton(){
            checkout.PlaceOrder();
            Console.WriteLine("Successfully clicked Place order button");
        }

        [Then(@"I should see the Order recieved page")]
        public void ThenIShouldSeeTheOrderRecievedPage() {
            try {
                Assert.That(orderReceived.OrderRecieved());
                orderNumber = orderReceived.GetOrderNumber();
                TestContext.WriteLine($"Successfully placed order with order number:{orderNumber} - Attaching Order Confirmation screenshot to report");
                HelperLibrary.TakeScreenshot(driver, screenshotFilePath + "Order Confirmation.jpg");
                TestContext.AddTestAttachment(screenshotFilePath + "Order Confirmation.jpg", "Order Confirmation screenshot");

            } catch(Exception) {
                TestContext.WriteLine("Attaching screenshot to report");
                HelperLibrary.TakeScreenshot(driver, screenshotFilePath + "Order Confirmation Error.jpg");
                TestContext.AddTestAttachment(screenshotFilePath + "Order Confirmation Error.jpg", "Order Confirmation Error screenshot");
                Assert.Fail("Unsuccessfully placed order");
                
            }
        }

        [Then(@"the order number should appear on the Orders page")]
        public void ThenTheOrderNumberShouldAppearOnTheOrdersPage() {
            navigation.ClickLink(Navigation.Link.MyAccount);
            myAccountDashboard.ViewOrders();

            string OrderID = myAccountOrders.GetOrderID();

            try {
                Assert.That(orderNumber, Is.EqualTo(OrderID.TrimStart('#')).IgnoreCase);
                TestContext.WriteLine($"Successfully displayed latest order: Order number displayed {OrderID} | Expected order number: {orderNumber} - Attaching Orders screenshot to report");
                HelperLibrary.TakeScreenshot(driver, screenshotFilePath + "All Orders.jpg");
                TestContext.AddTestAttachment(screenshotFilePath + "All Orders.jpg", "All Orders screenshot");
                myAccountOrders.Dashboard();
            } catch {
                TestContext.WriteLine($"Order number displayed {OrderID} | Expected order number: {orderNumber}");
                Assert.Fail("Unsuccessfully displayed latest order");
            }   
        }
    }
}
