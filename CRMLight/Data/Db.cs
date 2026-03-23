using System.Data;
using System.Data.SqlClient;

namespace CRMLight.Data;

public static class Db
{
    // À adapter selon ton environnement SQL Server
    public const string ConnectionString =
        @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CRMLightDb;Integrated Security=True;TrustServerCertificate=True";

    public static DataTable Query(string sql, params SqlParameter[] parameters)
    {
        using var conn = new SqlConnection(ConnectionString);
        using var cmd = new SqlCommand(sql, conn);
        if (parameters.Length > 0) cmd.Parameters.AddRange(parameters);

        using var adapter = new SqlDataAdapter(cmd);
        var table = new DataTable();
        adapter.Fill(table);
        return table;
    }

    public static int Execute(string sql, params SqlParameter[] parameters)
    {
        using var conn = new SqlConnection(ConnectionString);
        using var cmd = new SqlCommand(sql, conn);
        if (parameters.Length > 0) cmd.Parameters.AddRange(parameters);

        conn.Open();
        return cmd.ExecuteNonQuery();
    }

    public static object? Scalar(string sql, params SqlParameter[] parameters)
    {
        using var conn = new SqlConnection(ConnectionString);
        using var cmd = new SqlCommand(sql, conn);
        if (parameters.Length > 0) cmd.Parameters.AddRange(parameters);

        conn.Open();
        return cmd.ExecuteScalar();
    }
}
