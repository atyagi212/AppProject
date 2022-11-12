using Microsoft.Extensions.Configuration;
using Moq;
using SOEN6441_Project;
using SOEN6441_Project.Entities.Output;
using SOEN6441_Project.Interfaces;
using System.Dynamic;

namespace NUnit_Project;

[TestFixture]
public class PublishFlightTests
{
    private const string conn = "Server=localhost;Port=3306;Database=appProject;Uid=root;Pwd=rootadmin;";
    private Dictionary<string, string> myConfiguration = new Dictionary<string, string>
    {
        {"ConnectionStrings:myconn", conn}
    };
    private DBContext _context;

    private IConfigurationRoot configuration;
    private Mock<IDatabaseSubscriber> mockObserver;
    private PublishFlights publishFlights;

    [SetUp]
    public void Setup()
    {
        configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(myConfiguration)
            .Build();
        mockObserver = new Mock<IDatabaseSubscriber>();
        publishFlights = new PublishFlights(new NewFlightsManifest(configuration), configuration);
        _context = DBContext.getInstance(configuration);
    }

    [Test]
    public void Test_Update_Result_OK()
    {
        //Arrange

        //Act
        publishFlights.update();

        //Assert
        Assert.IsTrue(true);

    }

    [Test]
    public void Test_GetLatestFlightId_Result_Ok()
    {
        //Arrange

        //Act
        int Result= publishFlights.GetLatestFlightId();

        //Assert
        Assert.AreNotEqual(Result, 0);

    }

    [Test]
    public void Test_InsertComplexTables_Result_OK()
    {
        //Arrange
        FlightRecords records = new FlightRecords();

        //Act
        publishFlights.InsertComplexTables(records,_context);

        //Assert
        Assert.IsTrue(true);

    }

    [Test]
    public void Test_ParseFlightData_Result_OK()
    {
        //Arrange
        FlightRecords records = new FlightRecords();

        //Act
        publishFlights.ParseFlightData(records);

        //Assert
        Assert.IsTrue(true);

    }

    [Test]
    public void Test_ParseFlightData_Result_Fail()
    {
        //Arrange
        dynamic param = new ExpandoObject();

        //Act
        var ex = Assert.Throws<Microsoft.CSharp.RuntimeBinder.RuntimeBinderException>(() => publishFlights.ParseFlightData(param));

        //Assert
        Assert.That(ex.Message.Contains("has some invalid arguments"), Is.EqualTo(true));

    }

}
