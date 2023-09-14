namespace Birder.Tests.HelpersTests;

public class UsernameValidatorTests
{
    [Theory, MemberData(nameof(Null_Empty_Whitespace_String_Test_Data))]
    public void IsValidUsername_Returns_False_When_Null_Empty_Whitspace(string username)
    {
        //Act
        var result = RegexHelpers.IsValidUsername(username);

        // Assert
        Assert.False(result);
    }

    [Theory, MemberData(nameof(Not_Valid_String_Lengths_Data))]
    public void IsValidUsername_Returns_False_When_Char_Count_Too_Short_Long(string username)
    {
        //Act
        var result = RegexHelpers.IsValidUsername(username);

        // Assert
        Assert.False(result);
    }

    [Theory, MemberData(nameof(Not_Valid_Alpha_Numeric_Username_Data))]
    public void IsValidUsername_Returns_False_If_String_Contains_Non_Aplha_Numeric_Chars(string username)
    {
        //Act
        var result = RegexHelpers.IsValidUsername(username);

        // Assert
        Assert.False(result);
    }

    [Theory, MemberData(nameof(Valid_Alpha_Numeric_Username_Data))]
    public void IsValidUsername_Returns_True_When_Valid(string username)
    {
        //Act
        var result = RegexHelpers.IsValidUsername(username);

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


    public static IEnumerable<object[]> Not_Valid_String_Lengths_Data
    {
        get
        {
            return new[]
            {
                // too short < 5 Char
                new object[] { "1" },
                new object[] { "er2" },
                new object[] { "rety" },
                new object[] { "1234" },
                // too short > 20 Char
                new object[] { "123456789123456789123" },
                new object[] { "rnt7fkjjtnrgntksd5rmpZSD" },
                new object[] { "yop[snebrtvkfndhckgmj" },
                new object[] { "URJNGKKSGYFMFKFJF,FUF" },
                new object[] { "%&()r$£()*(&^(&^%$£&!" }
            };
        }
    }

    public static IEnumerable<object[]> Not_Valid_Alpha_Numeric_Username_Data
    {
        get
        {
            return new[]
            {
                new object[] { "andrew2%" },
                new object[] { "[jimmy]" },
                new object[] { "&*!%$" },
                new object[] { "angh^h4tuy" },
                new object[] { "12345" }
            };
        }
    }

    public static IEnumerable<object[]> Valid_Alpha_Numeric_Username_Data
    {
        get
        {
            return new[]
            {
                new object[] { "andrew2" },
                new object[] { "jimmy" },
                new object[] { "1a23456789" },
                new object[] { "1danny5" }
            };
        }
    }
}