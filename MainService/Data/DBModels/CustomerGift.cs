using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MainService.Data.DataClasses;

namespace MainService.Data.DBModels
{
    public class CustomerGift
    {
        [Key]
        public int Id { get; set; }
        
        [ForeignKey("CustomerId")]
        [Required]
        public Customer Customer { get; set; }
        
        [ForeignKey("GiftId")]
        [Required]
        public Gift Gift { get; set; }
        
        
        [Required]
        public DateTime DateAdded { get; set; }
        
        public string CustomDescription { get; set; }

        public int Quantity { get; set; } 
        
        public GiftState GiftState { get; set; }
    }
}