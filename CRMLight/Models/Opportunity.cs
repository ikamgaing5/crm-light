namespace CRMLight.Models;

public class Opportunity
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string Titre { get; set; } = "";
    public decimal Montant { get; set; }
    public string Etape { get; set; } = "Prospection";
    public int Probabilite { get; set; } = 10;
    public DateTime DateCreation { get; set; } = DateTime.Now;
    public DateTime? DateCloturePrevue { get; set; }
}
