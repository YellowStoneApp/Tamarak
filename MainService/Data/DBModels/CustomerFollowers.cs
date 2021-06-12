using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainService.Data.DBModels
{
    public class CustomerFollowers 
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        [ForeignKey("FollowedCustomerId")]
        public virtual Customer FollowedCustomer { get; set; }
    }
}