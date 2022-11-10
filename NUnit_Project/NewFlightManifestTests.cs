using System.Dynamic;
using Microsoft.Extensions.Configuration;
using Moq;
using SOEN6441_Project;
using SOEN6441_Project.Entities.Output;
using SOEN6441_Project.Interfaces;

namespace NUnit_Project;

[TestFixture]
public class NewFlightManifestTests
{
    private Dictionary<string, string> myConfiguration = new Dictionary<string, string>
    {
        {"InternalAPI:URI", "http://api.aviationstack.com/v1/flights?access_key=YOUR_ACCESS_KEY"},
        {"InternalAPI:YOUR_ACCESS_KEY","026c431faeec8b6e603c264eadcb2907"}
    };

    private IConfigurationRoot configuration;
    private Mock<IDatabaseSubscriber> mockObserver;
    private NewFlightsManifest newFlightsManifest;

    [SetUp]
    public void Setup()
    {
        configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(myConfiguration)
            .Build();
        mockObserver = new Mock<IDatabaseSubscriber>();
        newFlightsManifest = new NewFlightsManifest(configuration);
    }

    [Test]
    public void Test_AddObserver_Result_OK()
    {
        //Arrange

        //Act
        newFlightsManifest.addObserver(mockObserver.Object);

        //Assert
        Assert.IsTrue(true);

    }

    [Test]
    public void Test_AddObserver_Result_Fail()
    {
        //Arrange

        dynamic param = new ExpandoObject();

        //Act
        var ex = Assert.Throws<Microsoft.CSharp.RuntimeBinder.RuntimeBinderException>(() => newFlightsManifest.addObserver(param));

        //Assert
        Assert.That(ex.Message.Contains("has some invalid arguments"), Is.EqualTo(true));

    }

    [Test]
    public void Test_RemoveObserver_Result_OK()
    {
        //Arrange


        //Act
        newFlightsManifest.removeObserver(mockObserver.Object);

        //Assert
        Assert.IsTrue(true);

    }

    [Test]
    public void Test_RemoveObserver_Result_Fail()
    {
        //Arrange

        dynamic param = new ExpandoObject();

        //Act
        var ex = Assert.Throws<Microsoft.CSharp.RuntimeBinder.RuntimeBinderException>(() => newFlightsManifest.removeObserver(param));

        //Assert
        Assert.That(ex.Message.Contains("has some invalid arguments"), Is.EqualTo(true));

    }

    [Test]
    public void Test_NotifyObservers_Result_OK()
    {
        //Arrange


        //Act
        newFlightsManifest.notifyObservers();

        //Assert
        Assert.IsTrue(true);

    }

    [Test]
    public void Test_GetNewFlightsManifest_Result_OK()
    {
        //Arrange


        //Act
        newFlightsManifest.GetNewFlightsManifest();

        //Assert
        Assert.AreEqual(newFlightsManifest.GetManifestResponse().data.Count > 0, true);

    }

    [Test]
    public void Test_GetNewFlightsManifest_Result_Fail()
    {
        //Arrange
        myConfiguration["InternalAPI:YOUR_ACCESS_KEY"] = "";
        configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(myConfiguration)
            .Build();
        newFlightsManifest = new NewFlightsManifest(configuration);

        //Act
        newFlightsManifest.GetNewFlightsManifest();
        var result = newFlightsManifest.GetManifestResponse().data;

        //Assert
        Assert.AreEqual(result, null);

    }
}
