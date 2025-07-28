CREATE PROCEDURE [dbo].[spAddresses_GetTop10]
AS
begin
	select top 10 *
	from dbo.Addresses;
end