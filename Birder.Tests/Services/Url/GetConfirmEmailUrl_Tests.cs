using Microsoft.Extensions.Options;

namespace Birder.Tests.Services;

public class GetConfirmEmailUrl_Tests
{
    [Fact]
    public void GetConfirmEmailUrl_Returns_Valid_Uri_When_Args_Valid()
    {
        // Arrange
        var testOptions = Options.Create<ConfigOptions>(new ConfigOptions()
        { BaseUrl = "https://www.my.domain", TokenKey = "tOkEn" });

        const string VALID_USERNAME_ARGUMENT = "validUsername";
        const string VALID_CODE_ARGUMENT = "1234";

        var service = new UrlService(testOptions);

        // Act
        var result = service.GetConfirmEmailUrl(VALID_USERNAME_ARGUMENT, VALID_CODE_ARGUMENT);

        // Assert
        var expected = $"https://www.my.domain/api/Account/ConfirmEmail?username={VALID_USERNAME_ARGUMENT}&code={VALID_CODE_ARGUMENT}";

        result.Should().BeOfType<Uri>();
        var parsed = result.ToString();
        parsed.Should().BeEquivalentTo(expected);
    }

    [Theory, MemberData(nameof(Null_Empty_Whitespace_String_Test_Data))]
    public void GetConfirmEmailUrl_Throws_ArgumentException_When_Username_Arg_Null_Empty_Whitspace(string username)
    {
        // Arrange
        var testOptions = Options.Create<ConfigOptions>(new ConfigOptions()
        { BaseUrl = "we", TokenKey = "fgjiorgjivjbrihgnvrHeij45lk45lmf" });

        const string VALID_STRING_ARGUMENT = "not-null-or-empty";

        var service = new UrlService(testOptions);

        // Assert
        var ex = Assert.Throws<ArgumentException>(
            // Act      
            () => service.GetConfirmEmailUrl(username, VALID_STRING_ARGUMENT));

        Assert.Equal("method argument is not valid (Parameter 'username')", ex.Message);
    }

    [Theory, MemberData(nameof(Null_Empty_Whitespace_String_Test_Data))]
    public void GetConfirmEmailUrl_Throws_ArgumentException_When_Code_Arg_Null_Empty_Whitspace(string code)
    {
        // Arrange
        var testOptions = Options.Create<ConfigOptions>(new ConfigOptions()
        { BaseUrl = "we", TokenKey = "fgjiorgjivjbrihgnvrHeij45lk45lmf" });

        const string VALID_STRING_ARGUMENT = "not-null-or-empty";

        var service = new UrlService(testOptions);

        // Assert
        var ex = Assert.Throws<ArgumentException>(
            // Act      
            () => service.GetConfirmEmailUrl(VALID_STRING_ARGUMENT, code));

        Assert.Equal("method argument is not valid (Parameter 'code')", ex.Message);
    }

    public static IEnumerable<object[]> Null_Empty_Whitespace_String_Test_Data
    {
        get
        {
            return new[]
            {
                new object[] { null },
                new object[] { "" },
                new object[] { string.Empty },
                new object[] { "       " },
                new object[] { " " },
                new object[] { "    " }
            };
        }
    }
}