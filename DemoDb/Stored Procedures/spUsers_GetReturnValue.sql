CREATE PROCEDURE [dbo].[spUsers_GetReturnValue]
	@lastName nvarchar(50)
AS
BEGIN

	DECLARE @returnInfo int;

	SELECT @returnInfo = count(1)
	FROM [dbo].[Users]
	WHERE LastName = @lastName;

	return @returnInfo;

END
