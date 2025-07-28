CREATE PROCEDURE [dbo].[spUsers_GetDataWithOutput]
	@id int,
	@lastName nvarchar(50),
	@fullName nvarchar(100) OUTPUT
AS
BEGIN

	DECLARE @returnCount int;

	SELECT @fullName = CONCAT(FirstName,' ',LastName)
	FROM Users
	WHERE Id = @id;

	IF @fullName IS NULL
	BEGIN
		SET @fullName = '';
		SET @returnCount = 0;
	END
	ELSE
	BEGIN
		SET @returnCount = 1;
	END

	SELECT * 
	FROM dbo.Users
	WHERE LastName = @lastName;

END
	
RETURN @returnCount;
