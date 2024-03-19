using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow.Infrastructure;

namespace BDD_eCommerce_Project.Support {
    /*
    [Binding]
    internal class TestLoggingHooks {

        private readonly ISpecFlowOutputHelper _specFlowOutputHelper;
        private readonly ScenarioContext _scenarioContext;
        private string screenshotFilePath;

        public TestLoggingHooks(ScenarioContext scenarioContext, ISpecFlowOutputHelper specFlowOutputHelper) {
            _scenarioContext = scenarioContext;
            _specFlowOutputHelper = specFlowOutputHelper;
            this.screenshotFilePath = (string)_scenarioContext["screenshotFilePath"];
        }

        [AfterStep]
        public void TakeScreenshot(){

            if(_scenarioContext.ScenarioExecutionStatus != ScenarioExecutionStatus.OK) {
                _specFlowOutputHelper.WriteLine("Step Incomplete");
                specFlowOutputHelper.AddAttachment(screenshotFilePath);
            }
            else {
                ("Step complete");
            }
        }
    }
    */
}
