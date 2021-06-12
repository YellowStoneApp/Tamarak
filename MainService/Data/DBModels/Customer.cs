using System.ComponentModel.DataAnnotations;

namespace MainService.Data.DBModels
{
    public class Customer
    {
        // this is the key from our identity provider
        public string IdentityKey { get; set; }

        // this is the key we keep in our system
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        /// <summary>
        /// This flag prevents us from soft updating the customer's information when we get a response back from federated identity signin
        /// </summary>
        public bool ManuallyEdited { get; set; }
        public string Bio { get; set; }
    }
}