Feature: Checkout

A short summary of the feature


Background:
	Given I am on the login page
	When I enter my email and password
	And I click the Login button or press ENTER
	Then I should have successfully logged in

@tag1
Scenario: Placing an order
	Given I have an item in my cart
	When I proceed to checkout 
	And Enter my billing details
	When I place order
	Then the order should have gone through
	And be on my account
