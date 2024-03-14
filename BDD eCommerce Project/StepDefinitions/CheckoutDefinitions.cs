using BDD_eCommerce_Project.Support;
using BDD_eCommerce_Project.Support.PageObjects;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow.Assist;

namespace BDD_eCommerce_Project.StepDefinitions {
    [Binding]
    public class CheckoutDefinitions {

        private readonly ScenarioContext _scenarioContext;
        private IWebDriver driver;
        private string? orderNumber;
        private string screenshotFilePath;

        public CheckoutDefinitions(ScenarioContext scenarioContext) {
            _scenarioContext = scenarioContext;
            this.driver = (IWebDriver)_scenarioContext["myDriver"];
            this.screenshotFilePath = (string)_scenarioContext["screenshotFilePath"];
        }

        [Given(@"I am on the cart page with one product in the cart")]
        public void GivenIAmOnTheCartPageWithOneProduct() {
            Navigation navigation = new(driver);
            navigation.ClickLink(Navigation.Link.Shop);     // navigates to the 'Shop' page
            Console.WriteLine("Successfully entered shop");

            Shop shop = new(driver);
            shop.AddProductToCart("");    // adds product 'HoodieWithLogo' to the cart
            Console.WriteLine($"Successfully added hoodie with logo to the cart");

            navigation.ClickLink(Navigation.Link.Cart);     // navigates to the 'Cart' page
            Console.WriteLine("Successfully entered cart");
        }

        [When(@"I proceed to checkout")]
        public void WhenIProceedToCheckout() {
            Cart cart = new(driver);
            cart.ClickCheckoutButton();    // clicks the 'Proceed to checkout' button
            Console.WriteLine("Successfully entered checkout");
        }

        [When(@"I enter my billing details")]
        public void WhenIEnterMyBillingDetails(Table billingDetailsTable) {
            Checkout checkout = new(driver);
            BillingDetails billingDetails = billingDetailsTable.CreateInstance<BillingDetails>();
            
            Assert.That(checkout.EnterBillingDetails(billingDetails), "Unsuccessfully entered billing details into input fields");   // enters the billing details into fields
            Console.WriteLine("Successfully entered billing details - Attaching screenshot to report");
            HelperLibrary.TakeScreenshot(driver, screenshotFilePath + "Billing Details Filled.png");   // take screenshot of billing details
            TestContext.AddTestAttachment(screenshotFilePath + "Billing Details Filled.png", "Billing details applied");
        }

        [When(@"I select the payment method '(.*)'")]
        public void WhenISelectThePaymentMethod(string paymentMethod) {
            Checkout checkout = new(driver);
            checkout.ClickPaymentMethod(paymentMethod);
        }

        [When(@"I place the order")]
        public void WhenIPlaceTheOrder(){
            Checkout checkout = new(driver);
            checkout.ClickPlaceOrderButton();  // clicks the 'Place order' button
            Console.WriteLine("Successfully clicked Place order button");
        }

        [Then(@"I should see the Order recieved page")]
        public void ThenIShouldSeeTheOrderRecievedPage() {
            OrderReceived orderReceived = new(driver);

            try {
                Assert.That(orderReceived.WaitForOrderRecievedConfirmaion(), "Unsuccessfully placed order");     // check if the order has successfully been placed
                orderNumber = orderReceived.GetOrderNumber();   // assign Order Number displayed to orderNumber
                TestContext.WriteLine($"Successfully placed order with order number:{orderNumber} - Attaching Order Confirmation screenshot to report");
                HelperLibrary.TakeScreenshot(driver, screenshotFilePath + "Order Confirmation.png");    // take screenshot of successful Order confirmation
                TestContext.AddTestAttachment(screenshotFilePath + "Order Confirmation.png", "Order Confirmation screenshot");
            } catch(Exception) {
                TestContext.WriteLine("Attaching screenshot to report");
                HelperLibrary.TakeScreenshot(driver, screenshotFilePath + "Order Confirmation Error.png");  // take screenshot of unsuccessful Order confirmation
                TestContext.AddTestAttachment(screenshotFilePath + "Order Confirmation Error.png", "Order Confirmation Error screenshot"); 
            }
        }

        [Then(@"the order number should appear on the Orders page")]
        public void ThenTheOrderNumberShouldAppearOnTheOrdersPage() {
            Navigation navigation = new(driver);
            navigation.ClickLink(Navigation.Link.MyAccount);    // naviagate to 'My account' page
            
            MyAccountDashboard myAccountDashboard = new(driver);
            myAccountDashboard.ViewOrders();    // click Orders link to view all orders

            MyAccountOrders myAccountOrders = new(driver);
            string OrderID = myAccountOrders.GetOrderID();  // get and assign the most recent Order ID to OrderID

            try {
                Assert.That(orderNumber, Is.EqualTo(OrderID), "Unsuccessfully displayed latest order");    // check if OrderID matches with orderNumber
                TestContext.WriteLine($"Successfully displayed latest order: Order number displayed {OrderID} | Expected order number: {orderNumber} - Attaching Orders screenshot to report");
                HelperLibrary.TakeScreenshot(driver, screenshotFilePath + "All Orders.png");    // take screenshot of all Orders
                TestContext.AddTestAttachment(screenshotFilePath + "All Orders.png", "All Orders screenshot");
                myAccountOrders.Dashboard(); // navigate to 'My account' dashboard
            } catch {
                TestContext.WriteLine($"Order number displayed {OrderID} | Expected order number: {orderNumber}");
            }   
        }
    }
}
