CREATE PROCEDURE [dbo].[spUsers_GetTop10]
	
AS
begin
	select top 10 *
	from dbo.Users;
end