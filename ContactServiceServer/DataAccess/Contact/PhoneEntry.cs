namespace ContactServiceServer.DataAccess.Contact;

public record PhoneEntry(string PhoneNumber)
{
    public string? Label { get; init; } 
}