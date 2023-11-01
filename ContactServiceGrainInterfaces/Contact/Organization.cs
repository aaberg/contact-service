namespace ContactServiceGrainInterfaces.Contact;

[GenerateSerializer]
public record Organization
{
    [Id(0)] public string? Company { get; init; }
    
    [Id(1)] public string? JobTitle { get; init; }
}