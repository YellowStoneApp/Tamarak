using System.ComponentModel.DataAnnotations;

namespace MainService.Data.DataClasses
{
    public class GiftRequest
    {
        public string Url { get; set; }
        
        public string Title { get; set; }
         
        public string Image { get; set; }
        
        public string Description { get; set; }
        
        public string CustomDescription { get; set; }
        
        public int Quantity { get; set; }
        
        public string Vendor { get; set; }
    }
}