namespace CityInfo.API.Models;

public class AuthenticationUserDto(int userId, string username, string firstName, string lastName, string city)
{
    public int UserId { get; set; } = userId;
    public string Username { get; set; } = username;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string City { get; set; } = city;
}