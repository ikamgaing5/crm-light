using System.Data;
using System.Data.SqlClient;
using CRMLight.Models;

namespace CRMLight.Data;

public static class OpportunityRepository
{
    public static DataTable GetAll()
    {
        return Db.Query(@"
            SELECT o.Id, c.Nom AS Client, o.Titre, o.Montant, o.Etape, o.Probabilite, o.DateCreation, o.DateCloturePrevue
            FROM Opportunites o
            INNER JOIN Clients c ON c.Id = o.ClientId
            ORDER BY o.DateCreation DESC");
    }

    public static DataTable GetClients()
    {
        return Db.Query("SELECT Id, Nom + ' (' + ISNULL(Entreprise, '') + ')' AS Label FROM Clients ORDER BY Nom");
    }

    public static int Save(Opportunity opportunity)
    {
        if (opportunity.Id == 0)
        {
            return Db.Execute(@"
                INSERT INTO Opportunites(ClientId, Titre, Montant, Etape, Probabilite, DateCreation, DateCloturePrevue)
                VALUES(@ClientId, @Titre, @Montant, @Etape, @Probabilite, @DateCreation, @DateCloturePrevue)",
                new SqlParameter("@ClientId", opportunity.ClientId),
                new SqlParameter("@Titre", opportunity.Titre),
                new SqlParameter("@Montant", opportunity.Montant),
                new SqlParameter("@Etape", opportunity.Etape),
                new SqlParameter("@Probabilite", opportunity.Probabilite),
                new SqlParameter("@DateCreation", opportunity.DateCreation),
                new SqlParameter("@DateCloturePrevue", (object?)opportunity.DateCloturePrevue ?? DBNull.Value));
        }

        return Db.Execute(@"
            UPDATE Opportunites
            SET ClientId=@ClientId, Titre=@Titre, Montant=@Montant, Etape=@Etape, Probabilite=@Probabilite,
                DateCloturePrevue=@DateCloturePrevue
            WHERE Id=@Id",
            new SqlParameter("@ClientId", opportunity.ClientId),
            new SqlParameter("@Titre", opportunity.Titre),
            new SqlParameter("@Montant", opportunity.Montant),
            new SqlParameter("@Etape", opportunity.Etape),
            new SqlParameter("@Probabilite", opportunity.Probabilite),
            new SqlParameter("@DateCloturePrevue", (object?)opportunity.DateCloturePrevue ?? DBNull.Value),
            new SqlParameter("@Id", opportunity.Id));
    }
}
