using Dfe.Acec.Web.Services;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;

namespace Dfe.Acec.Web.Tests.Unit.Services;

public class IncompleteChildHandlerTests
{
    private const string ChildId = "child-1";

    private readonly JourneyState _journeyState = new();
    private readonly IJourneySession _journeySession = Substitute.For<IJourneySession>();

    [Fact]
    public void CheckForIncompleteChildrenReturnsEmptyWhenChildrenAreValid()
    {
        var validator = Substitute.For<IValidator<JourneyState>>();

        validator
            .Validate(Arg.Any<ValidationContext<JourneyState>>())
            .Returns(new ValidationResult());

        var handler = new IncompleteChildHandler(
            _journeyState,
            _journeySession,
            validator);

        var result = handler.CheckForIncompleteChildren();

        Assert.Empty(result);
        _journeySession.DidNotReceive().SetState(_journeyState);
    }

    [Fact]
    public void CheckForIncompleteChildrenRemovesInvalidChild()
    {
        _journeyState.Children[ChildId] = new Child(ChildId, "Sara");

        var validator = Substitute.For<IValidator<JourneyState>>();

        validator
            .Validate(Arg.Any<ValidationContext<JourneyState>>())
            .Returns(new ValidationResult(
            [
                new ValidationFailure("Children", "Invalid child")
                {
                    CustomState = ChildId
                }
            ]));

        var handler = new IncompleteChildHandler(
            _journeyState,
            _journeySession,
            validator);

        var result = handler.CheckForIncompleteChildren();

        Assert.Equal(["Sara"], result);
        Assert.False(_journeyState.Children.ContainsKey(ChildId));

        _journeySession.Received(1).SetState(_journeyState);
    }
}
