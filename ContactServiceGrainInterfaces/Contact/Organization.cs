namespace ContactServiceGrainInterfaces.Contact;

public record Organization
{
    public string? Company { get; init; }
    public string? JobTitle { get; init; }
}