using CRMLight.Data;
using CRMLight.Models;

namespace CRMLight.Forms;

public class MainForm : Form
{
    private readonly AppUser _user;

    public MainForm(AppUser user)
    {
        _user = user;

        Text = $"CRM Light - {_user.FullName} ({_user.Role})";
        StartPosition = FormStartPosition.CenterScreen;
        Width = 900;
        Height = 600;

        var header = new Label
        {
            Text = "Plateforme de gestion simplifiée de la relation client",
            Dock = DockStyle.Top,
            Height = 60,
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter
        };

        var panel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 80,
            Padding = new Padding(20),
            FlowDirection = FlowDirection.LeftToRight
        };

        panel.Controls.AddRange(new Control[]
        {
            MakeButton("Clients", (_, _) => new ClientsForm().ShowDialog()),
            MakeButton("Interactions", (_, _) => new InteractionsForm(_user).ShowDialog()),
            MakeButton("Opportunités", (_, _) => new OpportunitiesForm().ShowDialog()),
            MakeButton("Relances", (_, _) => new RemindersForm().ShowDialog()),
            MakeButton("Notifications", (_, _) => new NotificationsForm().ShowDialog()),
        });

        var info = new Label
        {
            Dock = DockStyle.Fill,
            Text = "CRM Light centralise les clients, les interactions, les opportunités, les relances et les notifications.",
            Font = new Font("Segoe UI", 11),
            TextAlign = ContentAlignment.MiddleCenter
        };

        Controls.Add(info);
        Controls.Add(panel);
        Controls.Add(header);

        Load += (_, _) =>
        {
            try
            {
                NotificationRepository.GenerateFromDueReminders();
            }
            catch
            {
                // On ne bloque pas l'ouverture de l'application
            }
        };
    }

    private Button MakeButton(string text, EventHandler onClick)
    {
        var btn = new Button
        {
            Text = text,
            Width = 150,
            Height = 40,
            Margin = new Padding(8)
        };
        btn.Click += onClick;
        return btn;
    }
}
