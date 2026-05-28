using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
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
    public void Set_SavesJourneyStateToSession()
    {
        var journeyState = new JourneyState
        {
            User = new UserState
            {
                CountryOfResidence = CountryOfResidence.England,
                HasPartner = HasPartnerOption.HasPartner,
            },
            Partner = new PartnerState
            {
                PartnerAge = AgeRange.EighteenToTwenty,
            }
        };
        _journeySession.Set(journeyState);

        _session.Received(1).Set("JourneyState", Arg.Any<byte[]>());
    }

    [Fact]
    public void Set_ThrowsExceptionIfHttpContextIsNull()
    {
        _httpContextAccessor.HttpContext.ReturnsNull();
        Assert.Throws<InvalidOperationException>(() => _journeySession.Set(new JourneyState()));
    }

    [Fact]
    public void Get_RetrievesJourneyStateFromSession()
    {
        var journeyState = new JourneyState
        {
            User = new UserState
            {
                CountryOfResidence = CountryOfResidence.England,
                HasPartner = HasPartnerOption.HasPartner,
            },
            Partner = new PartnerState
            {
                PartnerAge = AgeRange.EighteenToTwenty
            }
        };

        var serializedState = JsonSerializer.SerializeToUtf8Bytes(journeyState);
        _session.TryGetValue("JourneyState", out Arg.Any<byte[]>()!).Returns(x =>
        {
            x[1] = serializedState;
            return true;
        });

        var result = _journeySession.Get();

        Assert.NotNull(result);
        Assert.Equal(CountryOfResidence.England, result.User.CountryOfResidence);
        Assert.Equal(HasPartnerOption.HasPartner, result.User.HasPartner);
        Assert.Equal(AgeRange.EighteenToTwenty, result.Partner.PartnerAge);
    }

    [Fact]
    public void Get_RetrievesNewJourneyStateIfHttpContextIsNull()
    {
        _httpContextAccessor.HttpContext.ReturnsNull();

        var result = _journeySession.Get();

        Assert.NotNull(result);
        Assert.Null(result.User.CountryOfResidence);
        Assert.Null(result.User.HasPartner);
        Assert.Null(result.Partner.PartnerAge);
    }

    [Fact]
    public void Get_RetrievesNewJourneyStateIfSessionStringIsNull()
    {
        _session.TryGetValue("JourneyState", out Arg.Any<byte[]>()!).Returns(x =>
        {
            x[1] = null;
            return true;
        });

        var result = _journeySession.Get();

        Assert.NotNull(result);
        Assert.Null(result.User.CountryOfResidence);
        Assert.Null(result.User.HasPartner);
        Assert.Null(result.Partner.PartnerAge);
    }

    [Fact]
    public void Get_RetrievesNewJourneyStateIfSavedSessionEvaluatesToNull()
    {
        _session.TryGetValue("JourneyState", out Arg.Any<byte[]>()!).Returns(x =>
        {
            x[1] = Encoding.UTF8.GetBytes("null");
            return true;
        });

        var result = _journeySession.Get();

        Assert.NotNull(result);
        Assert.Null(result.User.CountryOfResidence);
        Assert.Null(result.User.HasPartner);
        Assert.Null(result.Partner.PartnerAge);
    }
}
