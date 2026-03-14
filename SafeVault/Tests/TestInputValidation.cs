using NUnit.Framework;
using SafeVault.Utils;

namespace SafeVault.Tests;

[TestFixture]
public class TestInputValidation
{
    [Test]
    public void TestForSpecialCharacters()
    {
        string input = "some text with !@# characters";
        bool isValid = ValidationHelpers.IsValidInput(input, new List<char>() { '!' });
        if (isValid)
        {
            Assert.Fail();
        }
        else
        {
            Assert.Pass();
        }
    }

    [Test]
    public void TestForSQLInjection()
    {
        string input = "select";
        bool isValid = ValidationHelpers.IsValidSqlInjectionInput(input);
        if (isValid)
        {
            Assert.Fail();
        }
        else
        {
            Assert.Pass();
        }
    }

    [Test]
    public void TestForXSS()
    {
        string input = "<script>alert(1)</script>";
        bool isValid = ValidationHelpers.IsValidXssInput(input);
        if (isValid)
        {
            Assert.Fail();
        }
        else
        {
            Assert.Pass();
        }
    }
}