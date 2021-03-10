/*
	Use Master
	Go
	Drop DataBase ATMovies3
*/

CREATE DATABASE ATMovies3
   ON 
     ( NAME = 'ATMovies3_Data', 
       FILENAME = 'D:\full-stack\FullStack-yearB-SemB\nir\lesson3\ATMovies3_Data.MDF', 
       SIZE = 10, 
       FILEGROWTH = 10% ) 
   LOG ON 
     ( NAME = 'ATMovies3_Log', 
       FILENAME = 'D:\full-stack\FullStack-yearB-SemB\nir\lesson3\ATMovies3_Log.LDF' 
	 )
 COLLATE Hebrew_CI_AS
Go

Use ATMovies3
GO

CREATE TABLE  Movies  
(
	 id nvarchar(150) not null primary key ,
	 title nvarchar(150),
	 category nvarchar(150),
	 release_date nvarchar(150)
)
GO

Create Proc SelectMoviesTable
As
Select * from [dbo].[Movies]
GO