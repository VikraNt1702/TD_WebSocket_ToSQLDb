# TD_WebSocket_ToSQLDb
For Saving Real Time Data To MSSQL-DB

Here in this code a method is added named "StoreDataInDatabase" 
The C# code appears to be a method for storing data in a SQL Server database using ADO.NET. 
It's designed to insert data into a database using a stored procedure named "InsertData" 
Here we can also use inline query For Eg:- "Inert into Table_name (Columnname1,Columnname2,....) Values (@Columnname1,@Columnname2,....)"
But StoreProcedure perfomes well and are faster than inline query,


1).As we can fisrt line of this method the method Signature(name) and it's declearation along with many number of parameter in this case they are 8.
The method StoreDataInDatabase takes 8 parameters, including the data you want to store in the database. These parameters include SymbolId, Timestamp, LTP, Volume, Open, High, Low, and PrevClose.


2).Then the connectionstring is used Here it is used of MSSQL we change it as per our requirement it can of MSSQL with SA authentication or it can of MySQL server.


3).Then we need to create a connection to the database using SqlConnection which uses the connectionstring that we have specified before here we have to put in parameter place so it can be connected to DB server.
we can also use MySqlConnection for that we need to install package of MySqlConnector rest is the same


4).Next we need to SqlCommand to which can be a store procedure or a inline query alternately for MySql server users it will MySqlCommand.

5).Then comes the part where we need to bind the values that we have fetched in the different parameters of this method using "cmd.Parameter.AddwithValue("@SymbolId", SymbolId)"


6).And then the Last Part Finally, the "cmd.ExecuteNonQuery()" method is called, which executes the stored procedure on the database. This will insert a new row into the database with the provided data.


This code provides a convenient way to store data in a SQL Server database using a stored procedure.
The parameters are passed into the stored procedure, and the data is inserted into the database table accordingly.
