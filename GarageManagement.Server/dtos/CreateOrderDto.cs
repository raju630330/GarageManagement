using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.dtos
{
    // ── Request DTOs ─────────────────────────────────────────────────────────

    public class CreateOrderDto
    {
        [Required]
        public long SupplierId { get; set; }

        [Required]
        public string PaymentType { get; set; } = string.Empty;  // Cash | Credit

        public string StockType { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public long? JobCardId { get; set; }
        public string RegNo { get; set; } = "STOCK";
        public string JobCardNo { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;

        [Required, MinLength(1)]
        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class OrderItemDto
    {
        [Required]
        public long PartId { get; set; }

        [Required, Range(0.0001, double.MaxValue)]
        public decimal Qty { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Discount { get; set; }

        public string ServiceType { get; set; } = "Part";  // Part | Labour
        public string Remarks { get; set; } = string.Empty;
        public string SellerInfo { get; set; } = string.Empty;
    }

    public class UpdateStatusDto
    {
        [Required]
        public string Status { get; set; } = string.Empty;
    }

    public class CreateSupplierDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string GSTIN { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }

    public class OrderFilterDto
    {
        public string? Status { get; set; }
        public string? RegNo { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    // ── Response DTOs ────────────────────────────────────────────────────────

    public class CreateOrderResponse
    {
        public bool IsSuccess { get; set; }
        public string OrderNo { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class SupplierDto
    {
        public long SupplierId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

    public class OrderListDto
    {
        public long Id { get; set; }
        public string OrderNo { get; set; } = string.Empty;
        public string OrderDate { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
        public string RegNo { get; set; } = string.Empty;
        public string JobCardNo { get; set; } = string.Empty;
        public string PaymentType { get; set; } = string.Empty;
        public string StockType { get; set; } = string.Empty;
        public decimal OrderValue { get; set; }
        public int OrderedParts { get; set; }
        public int InwardedParts { get; set; }
        public int PendingParts { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}