namespace MainService.Data.DataClasses
{
    /// <summary>
    /// Data in this class gets returned to the front end and will be public to anyone visiting website.
    /// DO NOT PUT PRIVATE CUSTOMER DATA IN THIS CLASS!!!
    /// </summary>
    public class CustomerPublic
    {
        public string IdentityKey { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Bio { get; set; }
    }
}