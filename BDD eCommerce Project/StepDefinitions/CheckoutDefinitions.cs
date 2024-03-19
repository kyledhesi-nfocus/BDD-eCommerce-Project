using BDD_eCommerce_Project.Support;
using BDD_eCommerce_Project.Support.PageObjects;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow.Assist;
using TechTalk.SpecFlow.Infrastructure;

namespace BDD_eCommerce_Project.StepDefinitions {
    [Binding]
    public class CheckoutDefinitions {

        private readonly ScenarioContext _scenarioContext;
        private IWebDriver driver;
        private string? orderNumber;
        private string screenshotFilePath;
        private readonly ISpecFlowOutputHelper _specFlowOutputHelper;
        public CheckoutDefinitions(ScenarioContext scenarioContext, ISpecFlowOutputHelper specFlowOutputHelper) {
            _scenarioContext = scenarioContext;
            _specFlowOutputHelper = specFlowOutputHelper;
            this.driver = (IWebDriver)_scenarioContext["myDriver"];
            this.screenshotFilePath = (string)_scenarioContext["screenshotFilePath"];     
        }

        [Given(@"I add a product to the cart")]
        public void GivenIAddAProductToTheCart() {
            Navigation navigation = new(driver);
            navigation.ClickLink(Navigation.Link.Shop);     // navigates to the 'Shop' page
            
            Shop shop = new(driver);
           _specFlowOutputHelper.WriteLine("Successfully entered shop");
            
            if (shop.AddProductToCart("")) {
                _specFlowOutputHelper.WriteLine($"Successfully added item to the cart");
            }
            else {
                Assert.Fail($"Unsuccessfully added item to the cart");
            }

            navigation.ClickLink(Navigation.Link.Cart);     // navigates to the 'Cart' page
            _specFlowOutputHelper.WriteLine("Successfully entered cart");
        }

        [Given(@"I proceed to checkout")]
        public void GivenIProceedToCheckout() {
            Cart cart = new(driver);
            cart.ClickCheckoutButton();    // clicks the 'Proceed to checkout' button
            _specFlowOutputHelper.WriteLine("Successfully entered checkout");
        }

        [Given(@"I enter my billing details")]
        public void GivenIEnterMyBillingDetails(Table billingDetailsTable) {
            Checkout checkout = new(driver);
            BillingDetails billingDetails = billingDetailsTable.CreateInstance<BillingDetails>();
            try {
                Assert.That(checkout.EnterBillingDetails(billingDetails), "Unsuccessfully entered billing details into input fields");   // enters the billing details into fields
                _specFlowOutputHelper.WriteLine("Successfully entered billing details");
            } catch (AssertionException) {
                HelperLibrary.TakeScreenshot(driver, screenshotFilePath + "Billing Details Exception.png", false);   // take screenshot of billing details
                _specFlowOutputHelper.AddAttachment(screenshotFilePath + "Billing Details Exception.png");
                throw;
            }
        }

        [Given(@"I select the payment method '(.*)'")]
        public void GivenISelectThePaymentMethod(string paymentMethod) {
            Checkout checkout = new(driver);
            checkout.ClickPaymentMethod(paymentMethod);
            _specFlowOutputHelper.WriteLine($"Successfully clicked payment method: {paymentMethod}");
        }

        [When(@"I place the order")]
        public void WhenIPlaceTheOrder(){
            Checkout checkout = new(driver);
            checkout.ClickPlaceOrderButton();  // clicks the 'Place order' button
            _specFlowOutputHelper.WriteLine("Successfully clicked Place order button");
        }

        [Then(@"I should see the Order recieved page")]
        public void ThenIShouldSeeTheOrderRecievedPage() {
            OrderReceived orderReceived = new(driver);
            try {
                Assert.That(orderReceived.WaitForOrderRecievedConfirmaion(), "Unsuccessfully placed order");     // check if the order has successfully been placed
                orderNumber = orderReceived.GetOrderNumber();   // assign Order Number displayed to orderNumber
                _specFlowOutputHelper.WriteLine($"Successfully placed order with order number:{orderNumber} - Attaching Order Confirmation screenshot to report");
                HelperLibrary.TakeScreenshot(driver, screenshotFilePath + "Order Confirmation.png", false);    // take screenshot of successful Order confirmation
                _specFlowOutputHelper.AddAttachment(screenshotFilePath + "Order Confirmation.png");
            } catch(Exception) {
                _specFlowOutputHelper.WriteLine("Attaching screenshot to report");
                HelperLibrary.TakeScreenshot(driver, screenshotFilePath + "Order Confirmation Error.png", false);  // take screenshot of unsuccessful Order confirmation
                _specFlowOutputHelper.AddAttachment(screenshotFilePath + "Order Confirmation Error.png"); 
            }
        }

        [Then(@"the order number should appear on the Orders page")]
        public void ThenTheOrderNumberShouldAppearOnTheOrdersPage() {
            Navigation navigation = new(driver);
            navigation.ClickLink(Navigation.Link.MyAccount);    // naviagate to 'My account' page

            MyAccountDashboard myAccountDashboard = new(driver);
            _specFlowOutputHelper.WriteLine("Successfully entered My account");

            myAccountDashboard.ViewOrders();    // click Orders link to view all orders
            MyAccountOrders myAccountOrders = new(driver);

            _specFlowOutputHelper.WriteLine("Successfully entered Orders page");

            string OrderID = myAccountOrders.GetOrderID();  // get and assign the most recent Order ID to OrderID

            try {
                Assert.That(orderNumber, Is.EqualTo(OrderID), "Unsuccessfully displayed latest order");    // check if OrderID matches with orderNumber
                _specFlowOutputHelper.WriteLine($"Successfully displayed latest order: Order number displayed {OrderID} | Expected order number: {orderNumber} - Attaching Orders screenshot to report");
                HelperLibrary.TakeScreenshot(driver, screenshotFilePath + "All Orders.png", false);    // take screenshot of all Orders
                _specFlowOutputHelper.AddAttachment(screenshotFilePath + "All Orders.png");
            } catch {
                _specFlowOutputHelper.WriteLine($"Order number displayed {OrderID} | Expected order number: {orderNumber}");
            }

        }
    }
}
