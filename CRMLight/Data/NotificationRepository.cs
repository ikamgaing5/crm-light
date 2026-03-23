using System.Data;
using System.Data.SqlClient;
using CRMLight.Models;

namespace CRMLight.Data;

public static class NotificationRepository
{
    public static DataTable GetAll()
    {
        return Db.Query(@"
            SELECT Id, ISNULL(UserId, 0) AS UserId, Titre, Message, IsRead, CreatedAt
            FROM Notifications
            ORDER BY CreatedAt DESC");
    }

    public static int Save(NotificationItem notification)
    {
        return Db.Execute(@"
            INSERT INTO Notifications(UserId, Titre, Message, IsRead, CreatedAt)
            VALUES(@UserId, @Titre, @Message, @IsRead, @CreatedAt)",
            new SqlParameter("@UserId", (object?)notification.UserId ?? DBNull.Value),
            new SqlParameter("@Titre", notification.Titre),
            new SqlParameter("@Message", notification.Message),
            new SqlParameter("@IsRead", notification.IsRead),
            new SqlParameter("@CreatedAt", notification.CreatedAt));
    }

    public static int MarkRead(int id)
    {
        return Db.Execute("UPDATE Notifications SET IsRead = 1 WHERE Id=@Id", new SqlParameter("@Id", id));
    }

    public static void GenerateFromDueReminders()
    {
        var due = ReminderRepository.GetDueForNotification();
        foreach (DataRow row in due.Rows)
        {
            var id = Convert.ToInt32(row["Id"]);
            var clientNom = row["ClientNom"].ToString() ?? "";
            var message = row["Message"].ToString() ?? "";

            Save(new NotificationItem
            {
                Titre = $"Relance à traiter - {clientNom}",
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.Now
            });

            ReminderRepository.MarkNotified(id);
        }
    }
}
