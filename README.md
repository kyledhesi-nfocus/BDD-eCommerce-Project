# eCommerce Website BDD Testing with SpecFlow in C#

This project is a Behavior-Driven Development (BDD) testing suite for an eCommerce website, using SpecFlow with C#. The purpose of the testing suite is to validate the functionality of features within the website. SpecFlow's natural language approach to test case definition, makes the tests understandable to stakeholders and maintaining a clear documentation of system behavior.

https://www.edgewordstraining.co.uk/demo-site/

### Prerequisites

- Visual Studio 2019 or later with the .NET desktop development workload installed
- .NET Core SDK 3.1 or later
- SpecFlow for Visual Studio 2019 extension
- SpecFlow+ Living Doc and Allure Report Docs for reporting
- Valid login credentials for the eCommerce website

### Installation

1. Clone the repository to your local machine.
2. Open the solution file (.sln) in Visual Studio.
3. Build the solution (Build > Build Solution).

The tests are reliant on an XML .runsettings file, in order to configure this:

- Add a new XML file to the project, using the `template.runsettings.txt` file.
- Replace placeholder values with actual login details.
- In Visual Studio, navigate to `Test > Configure Run Settings > Select Solution Wide runsettings File` and select your `.runsettings` file.

## Running the tests

The test suite includes the following test cases designed to validate the core functionalities of the eCommerce platform:

**Test Case 1: Applying a Coupon**

Validates the functionality of applying a coupon code during the checkout process. This test ensures that the coupon code correctly applies a discount to the total price, updates the price displayed to the user, and handles invalid or expired coupons gracefully.

**Test Case 2: Checking Out**

Ensures the checkout process works correctly from start to finish. This includes validating that the user can add items to their cart, enter shipping information, choose a payment method, and receive confirmation of their order.

To run the tests, navigate to Test Explorer in Visual Studio and click "Run All" or select individual tests to run.

To run the tests via the Command Prompt navigate to Solution Explorer > Open Folder in File Explorer - ensure you can see the '.csproj' file > Open a Command Prompt in this folder and paste either one of the following commands:

`dotnet test --results-directory "Results" --logger:trx --settings mysettings.runsettings`

`dotnet test --settings mysettings.runsettings --logger "nunit;LogFilePath=reports\test-result.xml`

## Reporting

**SpecFlow+ LivingDoc**

Run the tests via the Command Prompt - Naviagate to Solution Explorer and 'Open Folder in File Explorer' > Select the 'bin'...'Debug'...'net6.0' folders > Open a Command Prompt > Copy and paste the following:

`livingdoc test-assembly BDD" "eCommerce" "Project.dll -t TestExecution.json`

A `LivingDoc.html` should appear in the 'net6.0' folder.

**Allure Report Docs**

Run the tests via Test Explorer - Naviagate to Solution Explorer and 'Open Folder in File Explorer' > Select the 'bin'...'Debug'...'net6.0' folders - ensure you can see `allure-results` folder > Open a Command Prompt > Copy and paste the following:

`allure serve allure-results`

If a popup appears, select 'Allow', then the Allure report should appear.

## Resources
- [SpecFlow+ LivingDoc Installation](https://docs.specflow.org/projects/specflow-livingdoc/en/latest/LivingDocGenerator/Installing-the-command-line-tool.html)
- [Scoop Installation (for Allure Report)](https://scoop.sh/)
- [Allure Report Setup](https://allurereport.org/docs/gettingstarted-installation/)
- [Java Installation (required for Allure Report)](https://www.oracle.com/uk/java/technologies/downloads/#jdk22-windows) | [Installation Guide Video](https://www.youtube.com/watch?v=SQykK40fFds)