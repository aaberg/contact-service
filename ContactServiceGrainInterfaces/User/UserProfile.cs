namespace ContactServiceGrainInterfaces.User;

[GenerateSerializer]
public record UserProfile(string Name, string Email);