CREATE PROCEDURE [dbo].[spUsers_UpdateName]
	@userId int,
	@firstName nvarchar(50),
	@lastName nvarchar(50)
AS
BEGIN
	UPDATE [dbo].[Users]
	SET FirstName = @firstName,
		LastName = @lastName
	WHERE Id = @userId; -- Assuming we are updating the user with UserId = 1
END
