using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.api
{
    public class RootObject
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public int TotalElements { get; set; }
        public int TotalPages { get; set; }
        public List<ContentObject> Content { get; set; }
    }

    public class ContentObject
    {
        public bool Approved { get; set; }
        public bool Archived { get; set; }
        public List<AttributeObject> Attributes { get; set; }
        public string Barcode { get; set; }
        public string BatchRequestId { get; set; }
        public string Brand { get; set; }
        public int BrandId { get; set; }
        public string CategoryName { get; set; }
        public long CreateDateTime { get; set; }
        public string Description { get; set; }
        public double DimensionalWeight { get; set; }
        public bool HasActiveCampaign { get; set; }
        public string Id { get; set; }
        public List<ImageObject> Images { get; set; }
        public string LastUpdateDate { get; set; }
        public string LastPriceChangeDate { get; set; }
        public string LastStockChangeDate { get; set; }
        public decimal ListPrice { get; set; }
        public bool Locked { get; set; }
        public bool OnSale { get; set; }
        public int PimCategoryId { get; set; }
        public string PlatformListingId { get; set; }
        public string ProductMainId { get; set; }
        public int Quantity { get; set; }
        public decimal SalePrice { get; set; }
        public string StockCode { get; set; }
        public string StockUnitType { get; set; }
        public int SupplierId { get; set; }
        public string Title { get; set; }
        public int VatRate { get; set; }
        public int Version { get; set; }
        public bool Rejected { get; set; }
        public List<RejectReasonDetailObject> RejectReasonDetails { get; set; }
        public string ProductUrl { get; set; }
        public DeliveryOptions DeliveryOptions { get; set; }
        public bool Blacklisted { get; set; }

        public DateTime ConvertToDate(string dateString)
        {
            if (DateTime.TryParse(dateString, out DateTime date))
            {
                return date;
            }
            else
            {
                // Tarih dönüşümü başarısız olursa, bir hata fırlatabilir veya varsayılan bir tarih döndürebilirsiniz.
                throw new Exception("Invalid date format");
            }
        }
    }

    public class AttributeObject
    {
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        public int AttributeValueId { get; set; }
    }

    public class DeliveryOptions
    {
        public int DeliveryDuration { get; set; }
        public string FastDeliveryType { get; set; }
    }

    public class ImageObject
    {
        public string Url { get; set; }
    }

    public class RejectReasonDetailObject
    {
        public string RejectReason { get; set; }
        public string RejectReasonDetail { get; set; }
    }


}
