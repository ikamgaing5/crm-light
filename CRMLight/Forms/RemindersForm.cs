using CRMLight.Data;
using CRMLight.Models;

namespace CRMLight.Forms;

public class RemindersForm : Form
{
    private readonly DataGridView _grid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
    private readonly ComboBox _cmbClient = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly TextBox _txtMessage = new() { Multiline = true, Height = 80 };
    private readonly DateTimePicker _dtReminder = new() { Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm" };
    private readonly ComboBox _cmbCanal = new() { DropDownStyle = ComboBoxStyle.DropDownList };

    public RemindersForm()
    {
        Text = "CRM Light - Relances";
        Width = 1200;
        Height = 700;
        StartPosition = FormStartPosition.CenterParent;

        _cmbCanal.Items.AddRange(new object[] { "Email", "Appel", "WhatsApp", "SMS", "Autre" });
        _cmbCanal.SelectedIndex = 0;

        var left = new Panel { Dock = DockStyle.Left, Width = 390, Padding = new Padding(10) };
        left.Controls.Add(BuildForm());

        var btnSave = new Button { Text = "Enregistrer", Width = 120, Top = 320, Left = 20 };
        var btnDone = new Button { Text = "Marquer fait", Width = 120, Top = 320, Left = 150 };
        var btnRefresh = new Button { Text = "Actualiser", Width = 120, Top = 360, Left = 20 };

        btnSave.Click += (_, _) => SaveReminder();
        btnDone.Click += (_, _) => MarkDone();
        btnRefresh.Click += (_, _) => RefreshGrid();

        left.Controls.Add(btnSave);
        left.Controls.Add(btnDone);
        left.Controls.Add(btnRefresh);

        Controls.Add(_grid);
        Controls.Add(left);

        Load += (_, _) =>
        {
            LoadClients();
            RefreshGrid();
        };
    }

    private Control BuildForm()
    {
        var panel = new TableLayoutPanel { Dock = DockStyle.Top, ColumnCount = 2, RowCount = 0, AutoSize = true };
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58));

        AddRow(panel, "Client", _cmbClient);
        AddRow(panel, "Canal", _cmbCanal);
        AddRow(panel, "Message", _txtMessage);
        AddRow(panel, "Date relance", _dtReminder);

        return panel;
    }

    private void AddRow(TableLayoutPanel p, string label, Control ctrl)
    {
        var row = p.RowCount;
        p.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        p.Controls.Add(new Label { Text = label, AutoSize = true, Padding = new Padding(0, 8, 0, 0) }, 0, row);
        ctrl.Width = 220;
        p.Controls.Add(ctrl, 1, row);
        p.RowCount++;
    }

    private void LoadClients()
    {
        var clients = Db.Query("SELECT Id, Nom + ' (' + ISNULL(Entreprise, '') + ')' AS Label FROM Clients ORDER BY Nom");
        _cmbClient.DataSource = clients;
        _cmbClient.DisplayMember = "Label";
        _cmbClient.ValueMember = "Id";
    }

    private void RefreshGrid()
    {
        _grid.DataSource = ReminderRepository.GetAll();
        _grid.Columns["Id"].Visible = false;
    }

    private void SaveReminder()
    {
        if (_cmbClient.SelectedValue is null || string.IsNullOrWhiteSpace(_txtMessage.Text))
        {
            MessageBox.Show("Client et message sont obligatoires.");
            return;
        }

        ReminderRepository.Save(new Reminder
        {
            ClientId = Convert.ToInt32(_cmbClient.SelectedValue),
            Canal = _cmbCanal.SelectedItem?.ToString() ?? "Email",
            Message = _txtMessage.Text.Trim(),
            ReminderDate = _dtReminder.Value,
            Statut = "Planifiée",
            IsNotified = false
        });

        RefreshGrid();
        _txtMessage.Clear();
    }

    private void MarkDone()
    {
        if (_grid.CurrentRow is null) return;
        var id = Convert.ToInt32(_grid.CurrentRow.Cells["Id"].Value);
        Db.Execute("UPDATE Relances SET Statut='Fait' WHERE Id=@Id", new System.Data.SqlClient.SqlParameter("@Id", id));
        RefreshGrid();
    }
}
