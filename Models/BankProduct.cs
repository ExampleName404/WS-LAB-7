using System.ComponentModel.DataAnnotations;

namespace BankProductsAPI.Models;

public class BankProduct
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Range(typeof(decimal), "0", "100")]
    public decimal InterestRate { get; set; }

    public bool IsActive { get; set; }
}
