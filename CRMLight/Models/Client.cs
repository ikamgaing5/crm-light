namespace CRMLight.Models;

public class Client
{
    public int Id { get; set; }
    public string CodeClient { get; set; } = "";
    public string Nom { get; set; } = "";
    public string Entreprise { get; set; } = "";
    public string Email { get; set; } = "";
    public string Telephone { get; set; } = "";
    public string Adresse { get; set; } = "";
    public string Source { get; set; } = "";
    public string Statut { get; set; } = "Prospect";
    public DateTime CreatedAt { get; set; }
}
