using System.Data;
using System.Data.SqlClient;
using CRMLight.Models;

namespace CRMLight.Data;

public static class InteractionRepository
{
    public static DataTable GetAll()
    {
        return Db.Query(@"
            SELECT i.Id, c.Nom AS Client, i.TypeInteraction, i.Sujet, i.Notes, i.DateInteraction, i.NextFollowUpDate
            FROM Interactions i
            INNER JOIN Clients c ON c.Id = i.ClientId
            ORDER BY i.DateInteraction DESC");
    }

    public static DataTable GetClients()
    {
        return Db.Query("SELECT Id, Nom + ' (' + ISNULL(Entreprise, '') + ')' AS Label FROM Clients ORDER BY Nom");
    }

    public static int Save(Interaction interaction)
    {
        if (interaction.Id == 0)
        {
            return Db.Execute(@"
                INSERT INTO Interactions(ClientId, UserId, TypeInteraction, Sujet, Notes, DateInteraction, NextFollowUpDate)
                VALUES(@ClientId, @UserId, @TypeInteraction, @Sujet, @Notes, @DateInteraction, @NextFollowUpDate)",
                new SqlParameter("@ClientId", interaction.ClientId),
                new SqlParameter("@UserId", interaction.UserId),
                new SqlParameter("@TypeInteraction", interaction.TypeInteraction),
                new SqlParameter("@Sujet", interaction.Sujet),
                new SqlParameter("@Notes", interaction.Notes),
                new SqlParameter("@DateInteraction", interaction.DateInteraction),
                new SqlParameter("@NextFollowUpDate", (object?)interaction.NextFollowUpDate ?? DBNull.Value));
        }

        return Db.Execute(@"
            UPDATE Interactions
            SET ClientId=@ClientId, UserId=@UserId, TypeInteraction=@TypeInteraction, Sujet=@Sujet,
                Notes=@Notes, DateInteraction=@DateInteraction, NextFollowUpDate=@NextFollowUpDate
            WHERE Id=@Id",
            new SqlParameter("@ClientId", interaction.ClientId),
            new SqlParameter("@UserId", interaction.UserId),
            new SqlParameter("@TypeInteraction", interaction.TypeInteraction),
            new SqlParameter("@Sujet", interaction.Sujet),
            new SqlParameter("@Notes", interaction.Notes),
            new SqlParameter("@DateInteraction", interaction.DateInteraction),
            new SqlParameter("@NextFollowUpDate", (object?)interaction.NextFollowUpDate ?? DBNull.Value),
            new SqlParameter("@Id", interaction.Id));
    }
}
