using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Session;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Text;
using System.Text.Json;

namespace AccessingChildcareEntitlementChecker.UnitTests.Services;

public class JourneySessionTests
{
    private ISession _session;
    private IHttpContextAccessor _httpContextAccessor;
    private JourneySession _journeySession;

    public JourneySessionTests()
    {
        _session = Substitute.For<ISession>();
        var httpContext = new DefaultHttpContext();
        httpContext.Features.Set<ISessionFeature>(new SessionFeature { Session = _session });
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        _httpContextAccessor.HttpContext.Returns(httpContext);
        _journeySession = new JourneySession(_httpContextAccessor);
    }

    [Fact]
    public void SetSavesJourneyStateToSession()
    {
        var journeyState = new JourneyState
        {
            CountryOfResidence = CountryOfResidence.England,
            HasPartner = true,
            PartnerAge = AgeRange.EighteenToTwenty
        };
        _journeySession.SetState(journeyState);

        _session.Received(1).Set("JourneyState", Arg.Any<byte[]>());
    }

    [Fact]
    public void SetThrowsExceptionIfHttpContextIsNull()
    {
        _httpContextAccessor.HttpContext.ReturnsNull();
        Assert.Throws<InvalidOperationException>(() => _journeySession.SetState(new JourneyState()));
    }

    [Fact]
    public void GetRetrievesJourneyStateFromSession()
    {
        var journeyState = new JourneyState
        {
            CountryOfResidence = CountryOfResidence.England,
            HasPartner = true,
            PartnerAge = AgeRange.EighteenToTwenty
        };

        var serializedState = JsonSerializer.SerializeToUtf8Bytes(journeyState);
        _session.TryGetValue("JourneyState", out Arg.Any<byte[]>()!).Returns(x =>
        {
            x[1] = serializedState;
            return true;
        });

        var result = _journeySession.GetState();

        Assert.NotNull(result);
        Assert.Equal(CountryOfResidence.England, result.CountryOfResidence);
        Assert.True(result.HasPartner);
        Assert.Equal(AgeRange.EighteenToTwenty, result.PartnerAge);
    }

    [Fact]
    public void GetRetrievesNewJourneyStateIfHttpContextIsNull()
    {
        _httpContextAccessor.HttpContext.ReturnsNull();

        var result = _journeySession.GetState();

        Assert.NotNull(result);
        Assert.Null(result.CountryOfResidence);
        Assert.Null(result.HasPartner);
        Assert.Null(result.PartnerAge);
    }

    [Fact]
    public void GetRetrievesNewJourneyStateIfSessionStringIsNull()
    {
        _session.TryGetValue("JourneyState", out Arg.Any<byte[]>()!).Returns(x =>
        {
            x[1] = null;
            return true;
        });

        var result = _journeySession.GetState();

        Assert.NotNull(result);
        Assert.Null(result.CountryOfResidence);
        Assert.Null(result.HasPartner);
        Assert.Null(result.PartnerAge);
    }

    [Fact]
    public void GetRetrievesNewJourneyStateIfSavedSessionEvaluatesToNull()
    {
        _session.TryGetValue("JourneyState", out Arg.Any<byte[]>()!).Returns(x =>
        {
            x[1] = Encoding.UTF8.GetBytes("null");
            return true;
        });

        var result = _journeySession.GetState();

        Assert.NotNull(result);
        Assert.Null(result.CountryOfResidence);
        Assert.Null(result.HasPartner);
        Assert.Null(result.PartnerAge);
    }

    [Fact]
    public void HasSessionReturnsFalseIfHttpContextIsNull()
    {
        _httpContextAccessor.HttpContext.ReturnsNull();

        Assert.False(_journeySession.HasSession);
    }

    [Fact]
    public void HasSessionReturnsTrueWhenNoSession()
    {
        _session.TryGetValue("JourneyState", out Arg.Any<byte[]>()!).Returns(x =>
        {
            x[1] = Encoding.UTF8.GetBytes("A");
            return true;
        });

        Assert.True(_journeySession.HasSession);
    }

    [Fact]
    public void HasSessionReturnsFalseWhenNoSession()
    {
        _session.TryGetValue("JourneyState", out Arg.Any<byte[]>()!).Returns(_ =>
        {
            return false;
        });

        Assert.False(_journeySession.HasSession);
    }
}
