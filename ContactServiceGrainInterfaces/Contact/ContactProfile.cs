namespace ContactServiceGrainInterfaces.Contact;

public record ContactProfile(string Name)
{
    public string? ProfileImageUrl { get; init; }
}


