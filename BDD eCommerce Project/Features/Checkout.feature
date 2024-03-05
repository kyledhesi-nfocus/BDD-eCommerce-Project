@Functional
Feature: Checkout

As a customer,
	I want to able to input my billing details
	So that I am able to place an order


Background:
	Given I have at least one product in my cart
	And I am on the cart page

Scenario: Placing an order
	When I click the Proceed to checkout button
	And I enter my billing details
	When I click the Place order button
	Then I should see the Order recieved page
	And the order number should appear on the Orders page
