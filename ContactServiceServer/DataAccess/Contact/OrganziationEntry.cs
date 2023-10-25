namespace ContactServiceServer.DataAccess.Contact;

public record OrganziationEntry
{
    public string? Company { get; init; }
    public string? JobTitle { get; init; }
}