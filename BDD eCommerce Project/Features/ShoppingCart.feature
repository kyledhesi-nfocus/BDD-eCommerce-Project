Feature: Shopping cart

As a customer,
	I want to be able to add and remove products from my shopping cart
	So that I am able to manage the products I want to purchase
 
Background:
	Given I am on the login page
	When I enter my email and password
	And I click the Login button or press ENTER
	Then I should have successfully logged in

@SmokeTests
Scenario: Applying a coupon
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



