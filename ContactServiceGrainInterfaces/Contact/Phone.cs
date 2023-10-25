namespace ContactServiceGrainInterfaces.Contact;

public record Phone(string PhoneNumber)
{
    public string? Label { get; init; }
}