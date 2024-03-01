Feature: Discount Application

In order to achieve more sales, the business owner wants to allow customer's 
to apply a coupon which reduces 15% off the basket subtotal.
 
Background:
	Given I am on the login page
	When I enter my email and password
	And I click the Login button or press ENTER
	Then I should have successfully logged in

@SmokeTests
Scenario: Successful coupon application
	Given I am on the shop page	
	When I add an item to the cart
	And I view the cart
	When I apply the coupon '<coupon>'
	Then the coupon '<coupon>' should be applied to the subtotal

	Examples:
	| coupon        |
	| edgewords     |
	| nfocus        |
	| INVALIDCOUPON |



