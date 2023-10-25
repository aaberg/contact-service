using ContactServiceGrainInterfaces.Contact;
using ContactServiceServer.Grains.Contact;

namespace ContactServiceServer.DataAccess.Contact;

public static class ContactMapperExtension
{
    internal static ContactEntry Map(this ContactState c, Guid contactId)
    {
        return new ContactEntry
        {
            Id = contactId,
            Name = c.Name,
            ProfilePictureUrl = c.ProfilePictureUrl,
            Company = c.Company == null
                ? null
                : new OrganziationEntry
                {
                    Company = c.Company?.Company,
                    JobTitle = c.Company?.JobTitle,
                },
            Emails = c.Emails.Select(e => new EmailAddressEntry(e.Email)).ToArray(),
            PhoneNumbers = c.PhoneNumbers.Select(p => new PhoneEntry(p.PhoneNumber) { Label = p.Label }).ToArray(),
        };
    }
    
    internal static ContactState Map(this ContactEntry c)
    {
        return new ContactState
        {
            Name = c.Name,
            ProfilePictureUrl = c.ProfilePictureUrl,
            Company = c.Company == null
                ? null
                : new Organization
                {
                    Company = c.Company?.Company,
                    JobTitle = c.Company?.JobTitle,
                },
            Emails = c.Emails.Select(e => new EmailAddress(e.Email)).ToArray(),
            PhoneNumbers = c.PhoneNumbers.Select(p => new Phone(p.PhoneNumber) { Label = p.Label }).ToArray(),
        };
    }
}