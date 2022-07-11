// See https://aka.ms/new-console-template for more information
using System.Data.SqlClient;
using System.Transactions;
using TransactScope_IEnlistmentNotification_2PC;


using (var transaction = new TransactionScope())
{
	//transaction sample 
	//using (var context = new UserContext())
	//{
	//	context.Users.Add(new User() { });
	//	context.SaveChanges();
	//}

    //You can use FileSystem operations, MessageQueue and other data source operations in TransactionScope. 
    //this is not limited only database operations. 

	var fileCreator = new FileResourceManager(Path.Combine(Path.GetTempPath(), "test.txt"));

    using (var conn = new SqlConnection("YourConnectionString"))
    {
        using (SqlCommand cmd = conn.CreateCommand())
        {
            cmd.CommandText = "INSERT INTO User(Name) values ('Hakan');";
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
        }
    }

    transaction.Complete();
	


}