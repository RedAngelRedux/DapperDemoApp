CREATE PROCEDURE [dbo].[spUsers_GetByLastName]
	@lastName nvarchar(50)
AS
begin
	select top 10 * 
	from dbo.Users
	where LastName = @lastName;
end