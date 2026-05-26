using Reqnroll;

namespace AccessingChildcareEntitlementChecker.Tests.E2e
{
    [Binding]
    public class DeferredActionHooks(ScenarioContext scenarioContext, Context context)
    {
        private readonly ScenarioContext _scenarioContext = scenarioContext;
        private readonly Context _context = context;

        /// <summary>
        /// Flush pending actions on the first "When" or "Then" block encountered.
        /// </summary>
        /// <returns>Task representing the asynchronous operation.</returns>
        [BeforeStep]
        public async Task FlushPendingActions()
        {
            if (_scenarioContext.CurrentScenarioBlock is ScenarioBlock.When or ScenarioBlock.Then)
            {
                await _context.FlushPendingActions();
            }
        }
    }
}
