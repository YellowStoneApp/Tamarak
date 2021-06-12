using System;

namespace MainService.Data.DataClasses
{
    public class GiftResponse : GiftRequest
    {
        public int Id { get; set; }
        public DateTime DateAdded { get; set; }
    }
}