using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.api
{
    public class ApiProductPostObject
    {
        public string barcode { get; set; }
        public string title { get; set; }
        public string productMainId { get; set; }
        public int brandId { get; set; }
        public int categoryId { get; set; }
        public int quantity { get; set; }
        public string stockCode { get; set; }
        public decimal dimensionalWeight { get; set; }
        public string description { get; set; }
        public string currencyType { get; set; }
        public decimal listPrice { get; set; }
        public decimal salePrice { get; set; }
        public int cargoCompanyId { get; set; }
        public int vatRate { get; set; }
        public DeliveryOption deliveryOption { get; set; }
        public List<Image> images { get; set; }
        public List<Attribute> attributes { get; set; }
    }

    public class Image
    {
        public string url { get; set; }
    }

    public class Attribute
    {
        public int attributeId { get; set; }
        private int? _attributeValueId;
        public int? attributeValueId
        {
            get
            {
                return _attributeValueId;
            }
            set
            {
                _attributeValueId = value == 0 ? null : value;
            }
        }
        public string customAttributeValue { get; set; }
    }

    public class DeliveryOption
    {
        public int? deliveryDuration { get; set; }
        public string fastDeliveryType { get; set; }
    }

    public class PostRequest
    {
        public List<ApiProductPostObject> items { get; set; }
    }
}
