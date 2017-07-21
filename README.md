
-# StoredProcedureGenerator
 -Stored Procedure Generator For SQL Server

## Introduction

Lately, I have been involved in a project where one of the requirements was to write stored procedures from scratch for inserting and updating data in all the available tables in SQL database. Writing large number of `insert` and `update` stored procedure from scratch manually; I find this task very arduous. So I Googled around for some time to find some free tool for generating stored procedures, but I did not find anything free which was close to my requirement. Hence I thought of giving it a try and developing a small utility for the same. Eventually, I was able to come up with a small and very basic utility that generates stored procedure for SQL server tables from scratch.

## Background

The utility is developed using C# WPF (MVVM pattern) in .NET Framework 4.0\. This utility generates basic `insert` and `update` stored procedures, which you can modify according to your requirement. So the only thing this utility does is save your time when you want to write `insert` and `update` stored procedure from scratch and you are not willing to do it manually.

## Using the Utility

Once you run the attached utility, you get a window as shown in the below figure:

![](https://www.codeproject.com/KB/database/771755/Initail.png)

As you can see in the above screen, there are four action buttons (Connect To Server, Generate SP, Settings and Copy Generated SP). I will explain the significance of all them one by one.

### Connect To Server

This first thing you need to do is to connect to SQL server where you have the tables for which you want to generate stored procedure. You need to enter connection string in the given text box and after that, click on “Connect to Server" button. Once the application is successfully connected to the SQL server, it will show all the tables of database as shown in the below figure:

![](ConnectToServer.png)

### Generate SP

For generating `insert` stored procedure for a single table, you need to perform the following steps:

1.  Expand the table node for which you want to generate stored procedure.
2.  Expand the node "Insert Fields" under table node.
3.  Now select the fields which you want to include in your `insert` stored procedure.
4.  Finally click on Generate SP button. Your `insert` stored procedure will be visible in the Text Box as shown in the below figure:

![](InsertSp.png)

### Generate SP

For generating update SP for single table, you need to perform the following steps:

1.  Expand the table node for which you want to generate stored procedure.
2.  Expand the node "Update Fields" under table node.
3.  Select the fields which you want to include in your update stored procedure.
4.  If you want to do conditional update for that expand "Where Clause Fields" which is the last node in update field list and select fields which you want to include in where clause.
5.  Finally click on Generate SP button. Your update stored procedure will be visible in the Text Box as shown in the below figure:

![](UpdateSp.png)

### Settings

Using settings button, you can customize few things given below:

*   **Insert SP Name Prefix**: This setting allows you to customize the prefix of `insert` stored procedure name. The name of `insert` stored procedure is composed of `<insert sp name prefix> + <TableName>`
*   **Update SP Name Prefix**: This setting allows you to customize the prefix of `update` stored procedure name. The name of `update` stored procedure is composed of `<Update SP Name Prefix> + <TableName>`
*   **Input Parameter Prefix**: This setting allows you to customize the prefix of input parameter name of stored procedure. The name of input parameter is composed of `<Input Parameter Prefix> + <FieldName>`
*   **Where Parameter Prefix**: This setting allows you to customize the prefix of "Where Clause" parameter name. The name of `where` clause parameter is composed of `<Where Parameter Prefix> + <FieldName>`
*   **Error Handling**: This setting allows you to enable/disable error handling, if this setting is on then stored procedure will contain `Try`-`Catch` block for handling errors.

![](Settings.png)

### Copy Generated SP

Clicking on "Copy Generated SP" button will copy generated stored procedures in Windows clipboard which you can paste on Notepad or anywhere else using Windows copy (Ctrl +V).

#### Identity/Timestamp Fields

As we know in SQL server we cannot insert/update identity and timestamp/rowversion fields. So if table contains any identity/timestamp field then that filed we will get excluded from insert/update stored procedure by default.

#### SQL Server Key Words

The utility wraps the table name/field name inside square brackets [] if it is SQL server key word (like order, form, etc.). The utility maintains the list of SQL server key words inside a lookup file called “SqlKeyWords.txt". In case you find any keyword that causing issue and not getting wrapped inside square brackets, then you can add the SQL key word in SqlKeyWords.txt file to make it work.
