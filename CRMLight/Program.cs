using CRMLight.Data;
using CRMLight.Forms;

namespace CRMLight;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        try
        {
            AuthRepository.EnsureDefaultAdmin();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "La base de données n'est pas prête ou la chaîne de connexion est incorrecte.\n\n" +
                "Exécute d'abord le script SQL fourni dans le dossier Database.\n\n" +
                $"Détail : {ex.Message}",
                "CRM Light",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        Application.Run(new LoginForm());
    }
}
