using ContactServiceGrainInterfaces.Contact;

namespace ContactServiceApp.Contacts;

public record Contact(string Id, string Name, EmailAddress[] Emails, Phone[] Phones, string? ProfilePictureUrl = null)
{
    public EmailAddress? PrimaryEmail => Emails.Length > 0 ? Emails[0] : null;
    public Phone? PrimaryPhone => Phones.Length > 0 ? Phones[0] : null;
}