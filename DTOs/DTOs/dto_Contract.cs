using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_DTOs.DTOs
{
    public class Contract
    {
        public int ContractId { get; set; }
        public int ListingId { get; set; }
        public int SellerId { get; set; }
        public int BuyerId { get; set; }

        public string ContractType { get; set; }
        public decimal Price { get; set; }
        public string PriceUnit { get; set; }

        public int DurationMonths { get; set; }
        public decimal DepositAmount { get; set; }
        public decimal DownPayment { get; set; }

        public string Status { get; set; }
        public string PdfDocumentUrl { get; set; }

        public DateTime SigningDate { get; set; }
        public DateTime EffectiveDate { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
