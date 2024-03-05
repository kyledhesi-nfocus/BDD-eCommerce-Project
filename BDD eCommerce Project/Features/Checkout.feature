Feature: Checkout

A short summary of the feature


Background:
	Given I have at least one product in my cart
	And I am on the cart page

Scenario: Placing an order
	When I click the Proceed to checkout button
	And I enter my billing details
	When I click the Place order button
	Then I should see the Order recieved page
	And the order number should appear on the Orders page
