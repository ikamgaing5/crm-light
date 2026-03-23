using CRMLight.Data;

namespace CRMLight.Forms;

public class LoginForm : Form
{
    private readonly TextBox _txtUsername = new() { Width = 260 };
    private readonly TextBox _txtPassword = new() { Width = 260, UseSystemPasswordChar = true };

    public LoginForm()
    {
        Text = "CRM Light - Connexion";
        StartPosition = FormStartPosition.CenterScreen;
        Width = 420;
        Height = 260;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;

        var lblTitle = new Label
        {
            Text = "CRM Light",
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            AutoSize = true,
            Left = 120,
            Top = 15
        };

        var lblUser = new Label { Text = "Nom d'utilisateur", Left = 40, Top = 65, AutoSize = true };
        var lblPass = new Label { Text = "Mot de passe", Left = 40, Top = 105, AutoSize = true };

        _txtUsername.Left = 160; _txtUsername.Top = 60;
        _txtPassword.Left = 160; _txtPassword.Top = 100;

        var btnLogin = new Button { Text = "Se connecter", Left = 160, Top = 140, Width = 130 };
        btnLogin.Click += LoginClicked;

        Controls.AddRange(new Control[] { lblTitle, lblUser, lblPass, _txtUsername, _txtPassword, btnLogin });

        AcceptButton = btnLogin;
    }

    private void LoginClicked(object? sender, EventArgs e)
    {
        try
        {
            var user = AuthRepository.Login(_txtUsername.Text.Trim(), _txtPassword.Text);
            if (user is null)
            {
                MessageBox.Show("Identifiants invalides.", "CRM Light", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Hide();
            using var main = new MainForm(user);
            main.ShowDialog();
            Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "CRM Light", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
