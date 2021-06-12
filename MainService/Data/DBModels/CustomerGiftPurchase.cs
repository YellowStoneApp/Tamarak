using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainService.Data.DBModels
{
    public class CustomerGiftPurchase
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("CustomerGiftId")]
        public virtual CustomerGift CustomerGift { get; set; }
        
        public string PurchaserEmail { get; set; }
        
        public bool ConfirmedPurchase { get; set; }
        
        public DateTime? ConfirmationRequestTimestamp { get; set; }
        
        public DateTime GiftPurchaseRequestTimestamp { get; set; }
        
    }
}