@Functional
Feature: Shopping cart

As a customer,
	I want to be able to update my shopping cart
	So that I am able to manage the products I want to checkout with
 
Background:
	Given I am logged in as a user
	And I am on the shop page

Scenario Outline: Applying a coupon
    Given I add a product '<product>' to the cart
	And I view the cart
	When I apply the coupon '<coupon>'
	Then the discount '<discount>' should be applied to the subtotal
	And the correct total should be displayed

	Examples:
	| product      | coupon        | discount |
	| belt         | edgewords     | 15       |
	| sunglasses   | edgewords     | 10       |
	| hoodie       | nfocus        | 25       |
	| polo         | DOESNOTEXIST  | 10       |



