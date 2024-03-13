@Functional
Feature: Checkout

As a customer,
	I want to able to input my billing details
	So that I am able to place an order


Background:
	Given I am logged in as a user
	And I am on the cart page with one product in the cart
	

Scenario: Placing an order
	When I proceed to checkout 
	And I enter my billing details
	| field        | value                     |
	| FirstName    | Queen                     |
	| LastName     | Charles                   |
	| StreetName   | Buckingham Palace Road    |
	| City         | London                    |
	| Postcode     | SW1A 1AA                  |
	| PhoneNumber  | 0798347190321             |
	| Email        | example@email.co.uk       |
	When I place the order
	Then I should see the Order recieved page
	And the order number should appear on the Orders page


