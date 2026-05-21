using Microsoft.Playwright;

namespace AccessingChildcareEntitlementChecker.Tests.E2e.Steps.DeferredBackground
{
    public class DeferredActionQueue
    {
        private readonly List<IDeferredAction> _deferredActions;

        public DeferredActionQueue()
        {
            _deferredActions = new List<IDeferredAction>();
        }

        public void Answers(IEnumerable<(string title, string answer)> answers)
        {
            foreach (var answer in answers)
            {
                Answer(answer.title, answer.answer);
            }
        }

        public void ScopedAnswers(string scope, IEnumerable<(string title, string answer)> answers)
        {
            foreach (var answer in answers)
            {
                Answer(new DeferredAnswer(answer.title, answer.answer, scope));
            }
        }

        public void Answer(string title, string answer)
        {
            Answer(new DeferredAnswer(title, answer));
        }

        private void Answer(DeferredAnswer answer)
        {
            var matches = _deferredActions
                .Select((action, index) => new { action, index })
                .Where(match =>
                {
                    if (match.action is not DeferredAnswer deferredAnswer)
                    {
                        return false;
                    }

                    return deferredAnswer.Title == answer.Title && deferredAnswer.Scope == answer.Scope;
                })
                .ToList();

            if (matches.Count > 1)
            {
                if (answer.Scope == string.Empty)
                {
                    throw new InvalidOperationException($"Multiple matching answers found for title '{answer.Title}' with no scope. Consider providing a scope to disambiguate.");
                }

                throw new InvalidOperationException($"Multiple matching answers found for title '{answer.Title}' and scope '{answer.Scope}'.");
            }

            if (matches.Count == 1)
            {
                // Roll back `Given` to the match
                var match = matches[0];
                _deferredActions.RemoveRange(match.index, _deferredActions.Count - match.index);
            }

            _deferredActions.Add(answer);
        }

        public void ClickButton(string title, string label)
        {
            var action = new DeferredButtonClick(title, label);
            _deferredActions.Add(action);
        }

        public void ClickLink(string title, string label)
        {
            var action = new DeferredLinkClick(title, label);
            _deferredActions.Add(action);
        }

        public async Task FlushPendingActions(IPage page)
        {
            foreach (var action in _deferredActions)
            {
                await action.Execute(page);
            }

            _deferredActions.Clear();
        }
    }
}
