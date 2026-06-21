namespace FUNewsManagement.RazorPages.ViewModels;

public class AccountViewModel
{
    public short AccountID { get; set; }
    public string? AccountName { get; set; }
    public string? AccountEmail { get; set; }
    public int? AccountRole { get; set; }
    public string? AccountPassword { get; set; }
    public string? RoleName => AccountRole switch
    {
        0 => "Admin",
        1 => "Staff",
        2 => "Lecturer",
        _ => "Unknown"
    };
}
