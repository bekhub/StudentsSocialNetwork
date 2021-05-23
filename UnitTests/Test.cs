using System;
using Xunit;

namespace UnitTests
{
    // Todo Delete
    public class Test
    {
        [Fact]
        public void DateTimeTest()
        {
            var date = DateTime.UtcNow.AddDays(30);
            Assert.Equal(DateTimeKind.Utc, date.Kind);
        }
    }
}
