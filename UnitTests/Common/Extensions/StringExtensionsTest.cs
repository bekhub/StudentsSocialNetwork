using Common.Extensions;
using Xunit;

namespace UnitTests.Common.Extensions
{
    public class StringExtensionsTest
    {
        [Fact]
        public void AsIntOrNullTest()
        {
            Assert.Null("".AsIntOrNull());
            Assert.Null(((string)null).AsIntOrNull());
            Assert.Equal(1, "1 ".AsIntOrNull());
            Assert.Null("1b".AsIntOrNull());
        }
        
        [Fact]
        public void AsIntOrZeroTest()
        {
            Assert.Equal(0, "".AsIntOrZero());
            Assert.Equal(0, ((string)null).AsIntOrZero());
            Assert.Equal(1, "1 ".AsIntOrZero());
            Assert.Equal(0, "1b".AsIntOrZero());
        }
    }
}
