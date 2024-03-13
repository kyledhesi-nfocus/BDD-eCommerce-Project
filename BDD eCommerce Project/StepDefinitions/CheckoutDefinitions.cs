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
using TechTalk.SpecFlow.Assist;

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
            this.myAccountDashboard = new(driver);
            this.myAccountOrders = new(driver);
            this.orderReceived = new(driver);
            this.billingDetails = new BillingDetails();
        }

        [Given(@"I am on the cart page with one product in the cart")]
        public void GivenIAmOnTheCartPageWithOneProduct() {
            navigation.ClickLink(Navigation.Link.Shop);     // navigates to the 'Shop' page
            Console.WriteLine("Successfully entered shop");

            shop.AddToCart("belt");    // adds a specific product 'HoodieWithLogo' to the cart
            Console.WriteLine($"Successfully added belt to the cart");

            navigation.ClickLink(Navigation.Link.Cart);     // navigates to the 'Cart' page
            Console.WriteLine("Successfully entered cart");
        }

        [When(@"I proceed to checkout")]
        public void WhenIProceedToCheckout() {
            cart.Checkout();    // clicks the 'Proceed to checkout' button
            Console.WriteLine("Successfully entered checkout");
        }

        [When(@"I enter my billing details")]
        public void WhenIEnterMyBillingDetails(Table billingDetailsTable) {
            var billingDetails = billingDetailsTable.CreateInstance<BillingDetails>();
            try {    
                checkout.EnterBillingDetails(billingDetails);   // enters the billing details into fields
                Console.WriteLine("Successfully entered billing details - Attaching screenshot to report");
                HelperLibrary.TakeScreenshot(driver, screenshotFilePath + "Billing details.jpg");   // take screenshot of billing details
                TestContext.AddTestAttachment(screenshotFilePath + "Billing details.jpg", "Billing details applied");
            } catch (Exception){
                Assert.Fail("Unsuccessfully entered billing details into input fields");  // assert fail if billing details are not entered successfully
            }
        }

        [When(@"I place the order")]
        public void WhenIPlaceTheOrder(){
            checkout.PlaceOrder();  // clicks the 'Place order' button
            Console.WriteLine("Successfully clicked Place order button");
        }

        [Then(@"I should see the Order recieved page")]
        public void ThenIShouldSeeTheOrderRecievedPage() {
            try {
                Assert.That(orderReceived.OrderRecieved(), "Unsuccessfully placed order");     // check if the order has successfully been placed
                orderNumber += orderReceived.GetOrderNumber();   // assign Order Number displayed to orderNumber
                TestContext.WriteLine($"Successfully placed order with order number:{orderNumber} - Attaching Order Confirmation screenshot to report");
                HelperLibrary.TakeScreenshot(driver, screenshotFilePath + "Order Confirmation.jpg");    // take screenshot of successful Order confirmation
                TestContext.AddTestAttachment(screenshotFilePath + "Order Confirmation.jpg", "Order Confirmation screenshot");

            } catch(Exception) {
                TestContext.WriteLine("Attaching screenshot to report");
                HelperLibrary.TakeScreenshot(driver, screenshotFilePath + "Order Confirmation Error.jpg");  // take screenshot of unsuccessful Order confirmation
                TestContext.AddTestAttachment(screenshotFilePath + "Order Confirmation Error.jpg", "Order Confirmation Error screenshot"); 
            }
        }

        [Then(@"the order number should appear on the Orders page")]
        public void ThenTheOrderNumberShouldAppearOnTheOrdersPage() {
            navigation.ClickLink(Navigation.Link.MyAccount);    // naviagate to 'My account' page
            myAccountDashboard.ViewOrders();    // click Orders link to view all orders

            string OrderID = myAccountOrders.GetOrderID();  // get and assign the most recent Order ID to OrderID

            try {
                Assert.That(orderNumber, Is.EqualTo(OrderID), "Unsuccessfully displayed latest order");    // check if OrderID matches with orderNumber
                TestContext.WriteLine($"Successfully displayed latest order: Order number displayed {OrderID} | Expected order number: {orderNumber} - Attaching Orders screenshot to report");
                HelperLibrary.TakeScreenshot(driver, screenshotFilePath + "All Orders.jpg");    // take screenshot of all Orders
                TestContext.AddTestAttachment(screenshotFilePath + "All Orders.jpg", "All Orders screenshot");
                myAccountOrders.Dashboard(); // navigate to 'My account' dashboard
            } catch {
                TestContext.WriteLine($"Order number displayed {OrderID} | Expected order number: {orderNumber}");
            }   
        }
    }
}
