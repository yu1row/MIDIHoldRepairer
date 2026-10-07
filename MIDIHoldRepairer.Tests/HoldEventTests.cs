namespace MIDIHoldRepairer.Tests;

public class HoldEventTests
{
    [Theory]
    [InlineData(-1, "")]
    [InlineData(0, "0")]
    [InlineData(12, "12")]
    public void TimeDiffDisp_FormatsNonNegativeDiffOnly(int timeDiff, string expected)
    {
        var holdEvent = new HoldEvent { TimeDiff = timeDiff };

        Assert.Equal(expected, holdEvent.TimeDiffDisp);
    }
}
