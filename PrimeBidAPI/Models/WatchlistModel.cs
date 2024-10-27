using System.ComponentModel.DataAnnotations.Schema;

namespace PrimeBidAPI.Models
{
    public class WatchlistModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? ItemName { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentPrice { get; set; }
        public DateTime EndDate { get; set; }
        public string? ItemImage { get; set; }
    }
}
