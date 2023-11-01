namespace ContactServiceGrainInterfaces.Contact;

[GenerateSerializer]
public record Phone(string PhoneNumber)
{
    [Id(0)] public string? Label { get; init; }
}