namespace CRMLight.Models;

public class Reminder
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int? InteractionId { get; set; }
    public string Canal { get; set; } = "Email";
    public string Message { get; set; } = "";
    public DateTime ReminderDate { get; set; }
    public string Statut { get; set; } = "Planifiée";
    public bool IsNotified { get; set; }
}
