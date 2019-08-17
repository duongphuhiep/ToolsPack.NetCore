# ToolsPack.Sql

Avoid redundancy of pure ADO.NET code

- http://www.blackbeltcoder.com/Articles/ado/an-ado-net-sql-helper-class
- I've made an improvement so that we can declare the length of VarChar parameters. [It is recommended to always declare length of the VarChar parameters](http://blogs.msdn.com/b/psssql/archive/2010/10/05/query-performance-and-plan-cache-issues-when-parameter-length-not-specified-correctly.aspx)
- http://stackoverflow.com/a/18551053/347051

```CSharp
string qry = "SELECT.. FROM.. WHERE ArtApproved = @Approved AND ArtUpdated > @Updated AND name <> @Foo";
using (AdoHelper db = new AdoHelper(connectionString))
{
	using (SqlDataReader rdr = db.ExecDataReader(qry,
	    "@Approved", true,
	    "@Foo", "Bazz", 50 //50 is the parameter length to optimize query cache in some case
	    "@Fuu", "Beuh", //also a varchar parameter, but I do not declare the length (not recommended)
	    "@Holly", null, //will be replaced by DBNull value
	    "@Updated", new DateTime(2011, 3, 1)))
	{
	    while (rdr.Read())
	    {
	        rdr.GetValue<int?>("views"); //no need to check null value anymore
	        rdr.GetValue<DateTime?>("lastModified");
	    }
	}
}
```
