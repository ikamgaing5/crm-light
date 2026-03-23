using System.Data;
using System.Data.SqlClient;
using CRMLight.Models;

namespace CRMLight.Data;

public static class ReminderRepository
{
    public static DataTable GetAll()
    {
        return Db.Query(@"
            SELECT r.Id, c.Nom AS Client, r.Canal, r.Message, r.ReminderDate, r.Statut, r.IsNotified
            FROM Relances r
            INNER JOIN Clients c ON c.Id = r.ClientId
            ORDER BY r.ReminderDate ASC");
    }

    public static DataTable GetDueForNotification()
    {
        return Db.Query(@"
            SELECT r.Id, r.ClientId, c.Nom AS ClientNom, r.Message, r.ReminderDate
            FROM Relances r
            INNER JOIN Clients c ON c.Id = r.ClientId
            WHERE r.ReminderDate <= GETDATE()
              AND r.IsNotified = 0
              AND r.Statut <> 'Fait'");
    }

    public static int MarkNotified(int id)
    {
        return Db.Execute("UPDATE Relances SET IsNotified = 1 WHERE Id=@Id", new SqlParameter("@Id", id));
    }

    public static int Save(Reminder reminder)
    {
        if (reminder.Id == 0)
        {
            return Db.Execute(@"
                INSERT INTO Relances(ClientId, InteractionId, Canal, Message, ReminderDate, Statut, IsNotified)
                VALUES(@ClientId, @InteractionId, @Canal, @Message, @ReminderDate, @Statut, @IsNotified)",
                new SqlParameter("@ClientId", reminder.ClientId),
                new SqlParameter("@InteractionId", (object?)reminder.InteractionId ?? DBNull.Value),
                new SqlParameter("@Canal", reminder.Canal),
                new SqlParameter("@Message", reminder.Message),
                new SqlParameter("@ReminderDate", reminder.ReminderDate),
                new SqlParameter("@Statut", reminder.Statut),
                new SqlParameter("@IsNotified", reminder.IsNotified));
        }

        return Db.Execute(@"
            UPDATE Relances
            SET ClientId=@ClientId, InteractionId=@InteractionId, Canal=@Canal, Message=@Message,
                ReminderDate=@ReminderDate, Statut=@Statut, IsNotified=@IsNotified
            WHERE Id=@Id",
            new SqlParameter("@ClientId", reminder.ClientId),
            new SqlParameter("@InteractionId", (object?)reminder.InteractionId ?? DBNull.Value),
            new SqlParameter("@Canal", reminder.Canal),
            new SqlParameter("@Message", reminder.Message),
            new SqlParameter("@ReminderDate", reminder.ReminderDate),
            new SqlParameter("@Statut", reminder.Statut),
            new SqlParameter("@IsNotified", reminder.IsNotified),
            new SqlParameter("@Id", reminder.Id));
    }
}
