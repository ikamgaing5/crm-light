using System.Data.SqlClient;
using CRMLight.Models;

namespace CRMLight.Data;

public static class AuthRepository
{
    public static void EnsureDefaultAdmin()
    {
        try
        {
            var countObj = Db.Scalar("SELECT COUNT(*) FROM Users");
            var count = Convert.ToInt32(countObj);
            if (count > 0) return;
        }
        catch
        {
            // Si la table n'existe pas, on laisse l'exception remonter lors de l'authentification
            throw;
        }

        var hash = PasswordHasher.Hash("Admin123!");
        Db.Execute(@"
            INSERT INTO Users(FullName, Username, PasswordHash, Role, IsActive)
            VALUES(@FullName, @Username, @PasswordHash, @Role, 1)",
            new SqlParameter("@FullName", "Administrateur"),
            new SqlParameter("@Username", "admin"),
            new SqlParameter("@PasswordHash", hash),
            new SqlParameter("@Role", "Admin"));
    }

    public static AppUser? Login(string username, string password)
    {
        var table = Db.Query(@"
            SELECT TOP 1 Id, FullName, Username, PasswordHash, Role, IsActive
            FROM Users
            WHERE Username = @Username",
            new SqlParameter("@Username", username));

        if (table.Rows.Count == 0) return null;

        var row = table.Rows[0];
        var hash = row["PasswordHash"].ToString() ?? "";
        if (!PasswordHasher.Verify(password, hash)) return null;
        if (!Convert.ToBoolean(row["IsActive"])) return null;

        return new AppUser
        {
            Id = Convert.ToInt32(row["Id"]),
            FullName = row["FullName"].ToString() ?? "",
            Username = row["Username"].ToString() ?? "",
            PasswordHash = hash,
            Role = row["Role"].ToString() ?? "User",
            IsActive = Convert.ToBoolean(row["IsActive"])
        };
    }
}
