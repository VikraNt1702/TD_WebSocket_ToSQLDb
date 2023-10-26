# TD_WebSocket_ToSQLDb
For Saving Real Time Data To MSSQL-DB

Here in this code a method is added named "StoreDataInDatabase" 
The C# code appears to be a method for storing data in a SQL Server database using ADO.NET. 
It's designed to insert data into a database using a stored procedure named "InsertData" 
Here we can also use inline query For Eg:- "Inert into Table_name (Columnname1,Columnname2,....) Values (@Columnname1,@Columnname2,....)"
But StoreProcedure performs well and are faster than inline query.
Let's Understand how the code works and what it does.

So First of all we call this method from where we get the data and want to save it in the Database.
Calling method is simple just method name and parameteres. for eg: here it is StoreDataInDatabase(Parameters.......)

1). After calling this method the execution flow turns towards this method with the parameters like SymbolId,Timestamp,LTP,Volume and so on

2).Then the code goes to next line where we have decleared the  connectionstring which is used Here,
it is used of MSSQL we change it as per our requirement it can of MSSQL with SA authentication or it can of MySQL server.


3).Then it creates a connection to the database using SqlConnection which uses the connectionstring that we have specified before here we have to put in parameter place so it can be connected to DB server.
we can also use MySqlConnection for that we need to install package of MySqlConnector rest is the same


4).Further it goes to Next line where we use SqlCommand for letting code know that we are using the Stroed Procedure or inline query through which we are going store the data in DataBase alternately for MySql server users it will MySqlCommand. On next in code we have specified the CommandType which is StoredProcedue if it's Inline Query we don't need to specify it

5).Then goes to the part where it to bind the values that we have fetched in the different parameters of this method using "cmd.Parameter.AddwithValue("@SymbolId", SymbolId)" with this line it bind the value that it has fetched in parameter to the "@SymbolId" variable which bind the value to the coloumn present in database. 


6).And then the Last Part Finally, the "cmd.ExecuteNonQuery()" method is called, which executes the stored procedure on the database. This will insert a new row into the database with the provided data.


This code provides a convenient way to store data in a SQL Server database using a stored procedure.
The parameters are passed into the stored procedure, and the data is inserted into the database table accordingly.
