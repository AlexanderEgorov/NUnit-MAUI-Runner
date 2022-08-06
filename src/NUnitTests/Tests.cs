using NUnit.Framework;

namespace NUnitTests;

[TestFixture]
public class Tests {
    [SetUp]
    public void SetUp() { }
    
    [Test]
    public void RunTestSuccess() {
        Assert.True(true);
    }
    
    [Test]
    public void RunTestFailed() {
        Assert.True(false);
    }
    
    [Test]
    public void RunTestSuccessOnUI() {
        Device.InvokeOnMainThreadAsync(() => {
            Assert.True(true);
        }).Wait();
    }
    
    [Test]
    public void RunTestFailedOnUI() {
        Device.InvokeOnMainThreadAsync(() => {
            Assert.True(false);
        }).Wait();
    }
    
    [Test]
    public void CatchExceptionSuccess() {
        Assert.Throws<Exception>(() => throw new Exception("Test"));
    }
    
    [Test]
    public void CatchExceptionFailed() {
        Assert.Throws<Exception>(() => { });
    }
    
    [Test]
    public void CatchExceptionSuccessOnUI() {
        Device.InvokeOnMainThreadAsync(() => {
            Assert.Throws<Exception>(() => throw new Exception("Test"));
        }).Wait();
    }
    
    [Test]
    public void CatchExceptionFailedOnUI() {
        Device.InvokeOnMainThreadAsync(() => {
            Assert.Throws<Exception>(() => { });
        }).Wait();
    }
    
    [TearDown]
    public void TearDown() { }
}