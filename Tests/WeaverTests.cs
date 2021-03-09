using Fody;
using System;
using Xunit;

public class WeaverTests
{
    static TestResult testResult;

    static WeaverTests()
    {
        var weavingTask = new ModuleWeaver();
        testResult = weavingTask.ExecuteTestRun(
            "AssemblyToProcess.dll",
            runPeVerify: false);
    }

    [Fact]
    public void ValidateRecordCanBeSetAfterConstruction()
    {
        var type = testResult.Assembly.GetType("Example");
        var instance = (dynamic)Activator.CreateInstance(type, 1, "foo");
        instance.Number = 42;
        instance.String = "Foobar";

        Assert.Equal("Example { Number = 42, String = Foobar }", instance.ToString());
    }
}
