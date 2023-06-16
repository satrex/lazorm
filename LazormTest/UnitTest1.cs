using System.Transactions;

namespace LazormTest;

public class UnitTest1 : IDisposable
{
    public UnitTest1()
    {
        var db = test.GetDatabase();
        db.ExecuteNonQuery("BEGIN TRANSACTION t1;");
    }

    public void Dispose()
    {
        var db = test.GetDatabase();
        db.ExecuteNonQuery("ROLLBACK TRANSACTION t1;");
    }

    [Fact]
    public void CanInsertRecord()
    {
        test t = new test();
        t.id = 1;
        t.message = "test message";
        t.Store();

        t.message = string.Empty;
        Assert.Empty(t.message);
        t.Fill();
        Assert.NotEmpty(t.message);
    }

    [Fact]
    public void CanStore()
    { 
        test t = new test();
        t.id = 1;
        t.message = "test message";
        t.Store();
        Assert.NotEmpty(t.message);
    }
}
