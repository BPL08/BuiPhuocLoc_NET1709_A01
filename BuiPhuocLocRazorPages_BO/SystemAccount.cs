using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BuiPhuocLocRazorPages_BO;

public partial class SystemAccount
{
    [Required]
    public short AccountId { get; set; }

    [Required]
    public string? AccountName { get; set; }

    [Required]
    public string? AccountEmail { get; set; }

    [Required]
    public int? AccountRole { get; set; }

    [Required]
    public string? AccountPassword { get; set; }

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}
