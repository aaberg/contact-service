using ContactServiceGrainInterfaces.Contact;

namespace ContactServiceServer.Grains.Contact;

public record ContactState
{
    public required Guid OwnerTenantId { get; init; }
    public required string Name { get; init; }
    public string? ProfilePictureUrl { get; init; }
    public Organization? Company { get; init; }
    public Phone[] PhoneNumbers { get; init; } = Array.Empty<Phone>();
    public EmailAddress[] Emails { get; init; } = Array.Empty<EmailAddress>();
}