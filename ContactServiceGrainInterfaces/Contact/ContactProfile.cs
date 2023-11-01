namespace ContactServiceGrainInterfaces.Contact;

[GenerateSerializer]
public record ContactProfile(string Name)
{
    [Id(0)] public string? ProfileImageUrl { get; init; }
}


