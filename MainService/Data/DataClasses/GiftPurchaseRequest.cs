namespace MainService.Data.DataClasses
{
    public class GiftPurchaseRequest
    {
        public int GiftId { get;set; }
        public string ReceiverIdentityKey { get; set; }
        public string PurchaserEmail { get; set; }
    }
}