namespace ContactServiceGrainInterfaces.Contact;

public interface IContactGrain : IGrainWithGuidKey
{
    Task RegisterContact(string name);
    Task SetProfilePictureUrl(string url);

    Task<string> GetName();
    Task<string> GetProfilePictureUrl();

    Task SetCompany(Organization company);
    Task<Organization> GetCompany();

    Task AddEmail(EmailAddress email);
    Task<EmailAddress[]> ListEmails();

    Task AddPhone(Phone phone);
    Task<Phone[]> ListPhones();
}