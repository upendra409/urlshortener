CREATE PROCEDURE sp_GetLongUrl
                @ShortUrl nvarchar(70)

AS
--DECLARE @CreatedOn DateTime
SELECT LongUrl FROM dbo.Url WHERE ShortUrl = @ShortUrl