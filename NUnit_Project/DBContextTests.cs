using System.Data;
using System.Dynamic;
using Microsoft.Extensions.Configuration;
using Moq;
using MySqlConnector;
using SOEN6441_Project;
using SOEN6441_Project.Entities.Output;
using SOEN6441_Project.Interfaces;

namespace NUnit_Project;

[TestFixture]
public class DBContextTests
{
    private const string conn = "Server=localhost;Port=3306;Database=appProject;Uid=root;Pwd=rootadmin;";
    private Dictionary<string, string> myConfiguration = new Dictionary<string, string>
    {
        {"ConnectionStrings:myconn", conn}
    };
    private DBContext _context;

    private IConfigurationRoot configuration;

    [SetUp]
    public void Setup()
    {
        configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(myConfiguration)
            .Build();
        _context = DBContext.getInstance(configuration);
    }

    [Test, Order(1)]
    public void Test_InsertCollection_Results_OK()
    {
        //Arrange
        
        FlightRecords flr = new FlightRecords() { flight_date = "2022-11-09", flight_status = "Unit Test" };

        //Act
        bool result = _context.InsertCollection<FlightRecords>(flr);

        //Assert
        Assert.AreEqual(result, true);
    }


    [Test, Order(2)]
    public void Test_UpdateCollection_Results_OK()
    {
        //Arrange
        
        FlightRecords flr = new FlightRecords() { flight_date = "2022-11-09", flight_status = "Unit Test" };

        var dt = _context.SelectCollection<FlightRecords>(flr, new List<string> { "flight_status" }).CopyToDataTable();
        flr.Id = Convert.ToInt32(dt.Rows[0]["Id"].ToString());
        flr.flight_date = "2022-11-10";

        //Act
        bool result = _context.UpdateCollection<FlightRecords>(flr);

        //Assert
        Assert.AreEqual(result, true);
    }


    [Test, Order(3)]
    public void Test_SelectCollection_Results_OK()
    {
        //Arrange
        
        FlightRecords flr = new FlightRecords() { flight_date = "2022-11-09", flight_status = "Unit Test" };

        //Act
        var dt = _context.SelectCollection<FlightRecords>(flr, new List<string> { "flight_status" }).CopyToDataTable();

        //Assert
        Assert.AreEqual(dt.Rows.Count, 1);
    }


    [Test, Order(4)]
    public void Test_SelectAllCollection_Results_OK()
    {
        //Arrange
        
        FlightRecords flr = new FlightRecords() { flight_date = "2022-11-09", flight_status = "Unit Test" };

        //Act
        var dt = _context.SelectAllCollection<FlightRecords>(flr).CopyToDataTable();

        //Assert
        Assert.AreEqual(dt.Rows.Count > 0, true);
    }


    [Test, Order(5)]
    public void Test_DeleteCollection_Results_OK()
    {
        //Arrange
        
        FlightRecords flr = new FlightRecords() { flight_date = "2022-11-09", flight_status = "Unit Test" };

        var dt = _context.SelectCollection<FlightRecords>(flr, new List<string> { "flight_status" }).CopyToDataTable();
        flr.Id = Convert.ToInt32(dt.Rows[0]["Id"].ToString());

        //Act
        bool result = _context.DeleteCollection<FlightRecords>(flr);

        //Assert
        Assert.AreEqual(result, true);
    }

    [Test]
    public void Test_InsertCollection_Results_Fail()
    {
        //Arrange
        
        dynamic param = new ExpandoObject();

        //Act
        var ex = Assert.Throws<Microsoft.CSharp.RuntimeBinder.RuntimeBinderException>(() => _context.InsertCollection<FlightRecords>(param));

        //Assert
        Assert.That(ex.Message.Contains("has some invalid arguments"), Is.EqualTo(true));
    }


    [Test]
    public void Test_UpdateCollection_Results_Fail()
    {
        //Arrange
        
        dynamic param = new ExpandoObject();

        //Act
        var ex = Assert.Throws<Microsoft.CSharp.RuntimeBinder.RuntimeBinderException>(() => _context.UpdateCollection<FlightRecords>(param));

        //Assert
        Assert.That(ex.Message.Contains("has some invalid arguments"), Is.EqualTo(true));
    }


    [Test]
    public void Test_SelectCollection_Results_Fail()
    {
        //Arrange
        
        dynamic param = new ExpandoObject();

        //Act
        var ex = Assert.Throws<Microsoft.CSharp.RuntimeBinder.RuntimeBinderException>(() => _context.SelectCollection<FlightRecords>(param, new List<string> { "flight_status" }).CopyToDataTable());

        //Assert
        Assert.That(ex.Message.Contains("has some invalid arguments"), Is.EqualTo(true));
    }


    [Test]
    public void Test_SelectAllCollection_Results_Fail()
    {
        //Arrange
        
        dynamic param = new ExpandoObject();

        //Act
        var ex = Assert.Throws<Microsoft.CSharp.RuntimeBinder.RuntimeBinderException>(() => _context.SelectAllCollection<FlightRecords>(param).CopyToDataTable());

        //Assert
        Assert.That(ex.Message.Contains("has some invalid arguments"), Is.EqualTo(true));
    }


    [Test]
    public void Test_DeleteCollection_Results_Fail()
    {
        //Arrange
        
        dynamic param = new ExpandoObject();

        //Act
        var ex = Assert.Throws<Microsoft.CSharp.RuntimeBinder.RuntimeBinderException>(() => _context.DeleteCollection<FlightRecords>(param));

        //Assert
        Assert.That(ex.Message.Contains("has some invalid arguments"), Is.EqualTo(true));
    }

}
