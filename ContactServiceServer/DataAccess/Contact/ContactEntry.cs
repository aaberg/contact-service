namespace ContactServiceServer.DataAccess.Contact;

public record ContactEntry
{
    public required Guid Id { get; init; }
    public required Guid OwnerTenantId { get; init; }
    public required string Name { get; init; }
    public string? ProfilePictureUrl { get; set; }
    public OrganziationEntry? Company { get; init; }
    public PhoneEntry[] PhoneNumbers { get; init; } = Array.Empty<PhoneEntry>();
    public EmailAddressEntry[] Emails { get; init; } = Array.Empty<EmailAddressEntry>();
}