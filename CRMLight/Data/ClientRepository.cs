using System.Data;
using System.Data.SqlClient;
using CRMLight.Models;

namespace CRMLight.Data;

public static class ClientRepository
{
    public static DataTable GetAll()
    {
        return Db.Query(@"
            SELECT Id, CodeClient, Nom, Entreprise, Email, Telephone, Adresse, Source, Statut, CreatedAt
            FROM Clients
            ORDER BY Id DESC");
    }

    public static int Save(Client client)
    {
        if (client.Id == 0)
        {
            return Db.Execute(@"
                INSERT INTO Clients(CodeClient, Nom, Entreprise, Email, Telephone, Adresse, Source, Statut, CreatedAt)
                VALUES(@CodeClient, @Nom, @Entreprise, @Email, @Telephone, @Adresse, @Source, @Statut, GETDATE())",
                new SqlParameter("@CodeClient", client.CodeClient),
                new SqlParameter("@Nom", client.Nom),
                new SqlParameter("@Entreprise", client.Entreprise),
                new SqlParameter("@Email", client.Email),
                new SqlParameter("@Telephone", client.Telephone),
                new SqlParameter("@Adresse", client.Adresse),
                new SqlParameter("@Source", client.Source),
                new SqlParameter("@Statut", client.Statut));
        }

        return Db.Execute(@"
            UPDATE Clients
            SET CodeClient=@CodeClient, Nom=@Nom, Entreprise=@Entreprise, Email=@Email,
                Telephone=@Telephone, Adresse=@Adresse, Source=@Source, Statut=@Statut
            WHERE Id=@Id",
            new SqlParameter("@CodeClient", client.CodeClient),
            new SqlParameter("@Nom", client.Nom),
            new SqlParameter("@Entreprise", client.Entreprise),
            new SqlParameter("@Email", client.Email),
            new SqlParameter("@Telephone", client.Telephone),
            new SqlParameter("@Adresse", client.Adresse),
            new SqlParameter("@Source", client.Source),
            new SqlParameter("@Statut", client.Statut),
            new SqlParameter("@Id", client.Id));
    }

    public static int Delete(int id)
    {
        return Db.Execute("DELETE FROM Clients WHERE Id=@Id", new SqlParameter("@Id", id));
    }
}
