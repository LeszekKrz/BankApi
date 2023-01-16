using FluentAssertions;
using Xunit;

namespace BankApiTests;

[Trait("Category", "Unit")]
public sealed class ExampleTests
{
    [Fact]
    public void ShouldPass()
    {
        "hello".Should().NotBeEmpty();
    }
}