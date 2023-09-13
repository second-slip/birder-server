namespace Birder.Tests.HelpersTests;

public class EmailValidatorTests
{

    [Theory, MemberData(nameof(Null_Empty_Whitespace_String_Test_Data))]
    public void IsEmailValid_Returns_False_When_Null_Empty_Whitspace(string email)
    {
        //Act
        var result = RegexUtilities.IsValidEmail(email);

        // Assert
        Assert.False(result);
    }

    [Theory, MemberData(nameof(InValid_Email_String_Test_Data))]
    public void IsEmailValid_Returns_False_When_Not_Valid_Email_Pattern(string email)
    {
        //Act
        var result = RegexUtilities.IsValidEmail(email);

        // Assert
        Assert.False(result);
    }

    [Theory, MemberData(nameof(Valid_Email_String_Test_Data))]
    public void IsEmailValid_Returns_True_When_Valid_Email_Pattern(string email)
    {
        //Act
        var result = RegexUtilities.IsValidEmail(email);

        // Assert
        Assert.True(result);
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

    public static IEnumerable<object[]> InValid_Email_String_Test_Data
    {
        get
        {
            return new[]
            {
                new object[] { "a@" },
                new object[] { "@b" },
                new object[] {  "@" } ,
                new object[] { "sdefd@b. " },
                new object[] { "sdefd@b." },
                new object[] { "sdefd@b " },
                new object[] { "sdefdb " },
                new object[] { "@b.com" },
                new object[] { " @.co.uk" },
                new object[] { "  a@b.com" }, // leading whitepace
                new object[] { "a@b.com  " }, // trailing whitespace
            };
        }
    }

    public static IEnumerable<object[]> Valid_Email_String_Test_Data
    {
        get
        {
            return new[]
            {
                new object[] { "a@b.com" },
                new object[] { "fggre@b.co" },
                new object[] { "A@AND.CO.UK" },
                new object[] { "zsdffv@j.org.uk" },
                new object[] { "AESTDNdmrjcm@b.com" }
            };
        }
    }
}