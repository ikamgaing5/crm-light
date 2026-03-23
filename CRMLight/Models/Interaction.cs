namespace CRMLight.Models;

public class Interaction
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int UserId { get; set; }
    public string TypeInteraction { get; set; } = "Appel";
    public string Sujet { get; set; } = "";
    public string Notes { get; set; } = "";
    public DateTime DateInteraction { get; set; } = DateTime.Now;
    public DateTime? NextFollowUpDate { get; set; }
}
