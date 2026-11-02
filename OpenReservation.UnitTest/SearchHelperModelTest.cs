using OpenReservation.Helpers;
using Xunit;

namespace OpenReservation.UnitTest;

public class SearchHelperModelTest
{
    [Fact]
    public void PageNumber_IsOneBased()
    {
        var search = new SearchHelperModel
        {
            PageNumber = 0
        };

        Assert.Equal(1, search.PageNumber);

        search.PageNumber = 2;

        Assert.Equal(2, search.PageNumber);
    }
}
