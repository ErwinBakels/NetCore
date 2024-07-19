using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DSharp.NetCore;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        Guard.AreEqual("Erwin", "Erwin");

        Log.Name = "CoreTest";
        Log.Level = Level.Trace;

        Log.Error("Error");
        Log.Warning("Warning");
        Log.Information("Information");
        Log.Process("Process");
        Log.Debug("Debug");
        Log.Verbose("Verbose");
        Log.Trace("Trace");

        Log.Exception(new Exception("Exception"));



        Guard.IsNotEmpty(new Uri("http://www.dsharp.nl"));


    }

    [TestMethod]
    public void TestMethod2()
    {
        Guard.ThrowsException<GuardFailedException>(() => Guard.AreEqual("Erwin", "Erwin1"));


    }
}