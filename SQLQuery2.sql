--drop procedure sp_InsertUrl
CREATE PROCEDURE sp_InsertUrl
                 @LongUrl nvarchar(500),
                 @ShortUrl nvarchar(50),
		        @Identifier nvarchar(50),
                 @CreatedOn datetime,
		         @CreatedBy nvarchar(50)
AS
DECLARE @Count int = 0
DECLARE @ReturnCode int = 0
SELECT @Count = count(1) FROM dbo.Url WHERE ShortUrl = @ShortUrl
print 'asdads'
print @Count
print 'xxxxx'
IF (@Count = 0)
BEGIN
   -- we can't divide by zero, so assume time is 1 hour
       INSERT INTO [dbo].[Url]
           ([LongUrl]
           ,[ShortUrl]
           ,[Identifier]
           ,[CreatedOn]
           ,[CreatedBy])
     VALUES
           (@LongUrl
           ,@ShortUrl
           ,@Identifier
           ,@CreatedOn
           ,@CreatedBy
           )
     SELECT @ReturnCode = 0;
END
ELSE
BEGIN
    SELECT @ReturnCode = -1;
END
RETURN @ReturnCode

--drop sp_insertUrl
CREATE TABLE [dbo].[Url] (
    [ID]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [LongUrl]    NVARCHAR (500) NOT NULL,
    [ShortUrl]   NVARCHAR (50)  NOT NULL,
    [Identifier] NVARCHAR (50)  NULL,
    [CreatedOn]  DATETIME       NULL,
    [CreatedBy]  NVARCHAR (50)  NULL,
    CONSTRAINT [PK_Url] PRIMARY KEY CLUSTERED ([ID] ASC)
);

