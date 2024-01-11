namespace cslox.tests;

public class ScannerTests
{
    private readonly string _input1;

    public ScannerTests()
    {
        _input1 = """
// this is a comment
(( )){} // grouping stuff
!*+-/=<> <= == // operators
""";

    }
    [Fact]
    public void Test1()
    {
        // Arrange
        Lox.Run(_input1);

        // Act

        // Assert
    }

    [Fact]
    public void ShouldParseSimpleAdditionExpression()
    {
        Lox.Run("150 + 270");
    }
}