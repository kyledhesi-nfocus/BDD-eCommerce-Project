@Functional
Feature: Shopping cart

As a customer,
	I want to be able to update my shopping cart
	So that I am able to manage the products I want to checkout with
 
Background:
	Given I am on the shop page

Scenario: Applying a coupon
	When I add an item to the cart
	And I view the cart
	When I apply the coupon '<coupon>'
	Then the coupon '<coupon>' should be applied to the subtotal

	Examples:
	| coupon        |
	| edgewords     |
	| nfocus        |
	| DOESNOTEXIST  |



