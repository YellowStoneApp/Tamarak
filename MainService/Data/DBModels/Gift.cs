using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MainService.Data.DataClasses;

namespace MainService.Data.DBModels
{
    public class Gift
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Url { get; set; }
        
        [Required]
        public string Title { get; set; }
         
        [Required]
        public string Image { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        public string Vendor { get; set; }
    }
}