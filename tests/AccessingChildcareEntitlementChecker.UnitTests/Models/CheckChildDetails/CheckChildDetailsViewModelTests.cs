using AccessingChildcareEntitlementChecker.Web.Models.CheckChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.UnitTests.Models.CheckChildDetails
{
    public class CheckChildDetailsViewModelTests
    {

        [Fact]
        public void ResolveLastEditedChild_ReturnsNull_WhenNoChildren()
        {
            var journeyState = new JourneyState();
            var viewModel = new CheckChildDetailsViewModel(journeyState, null);
            Assert.Null(viewModel.LastEditedChild);
        }

        [Fact]
        public void ResolveLastEditedChild_ReturnsChild_WhenFromChildIdMatches()
        {
            var journeyState = new JourneyState();
            var child = new Child("child-1", "Child 1");
            journeyState.Children[child.ChildId] = child;
            var viewModel = new CheckChildDetailsViewModel(journeyState, "child-1");
            Assert.Equal(child, viewModel.LastEditedChild);
        }
    }
}
