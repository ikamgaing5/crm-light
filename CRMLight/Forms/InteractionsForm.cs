using CRMLight.Data;
using CRMLight.Models;

namespace CRMLight.Forms;

public class InteractionsForm : Form
{
    private readonly AppUser _user;
    private readonly DataGridView _grid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
    private readonly ComboBox _cmbClient = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly ComboBox _cmbType = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly TextBox _txtSujet = new();
    private readonly TextBox _txtNotes = new() { Multiline = true, Height = 80 };
    private readonly DateTimePicker _dtFollow = new() { Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm" };
    private readonly CheckBox _chkFollow = new() { Text = "Prévoir une relance" };

    public InteractionsForm(AppUser user)
    {
        _user = user;

        Text = "CRM Light - Interactions";
        Width = 1200;
        Height = 700;
        StartPosition = FormStartPosition.CenterParent;

        _cmbType.Items.AddRange(new object[] { "Appel", "Email", "Visite", "WhatsApp", "Autre" });
        _cmbType.SelectedIndex = 0;
        _chkFollow.CheckedChanged += (_, _) => _dtFollow.Enabled = _chkFollow.Checked;
        _dtFollow.Enabled = false;

        var left = new Panel { Dock = DockStyle.Left, Width = 380, Padding = new Padding(10) };
        left.Controls.Add(BuildForm());

        var btnSave = new Button { Text = "Enregistrer", Width = 120, Top = 430, Left = 20 };
        var btnRefresh = new Button { Text = "Actualiser", Width = 120, Top = 430, Left = 150 };
        btnSave.Click += (_, _) => SaveInteraction();
        btnRefresh.Click += (_, _) => RefreshGrid();

        left.Controls.Add(btnSave);
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
        AddRow(panel, "Type", _cmbType);
        AddRow(panel, "Sujet", _txtSujet);
        AddRow(panel, "Notes", _txtNotes);
        AddRow(panel, "Relance", _chkFollow);
        AddRow(panel, "Date relance", _dtFollow);

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
        var table = InteractionRepository.GetClients();
        _cmbClient.DataSource = table;
        _cmbClient.DisplayMember = "Label";
        _cmbClient.ValueMember = "Id";
    }

    private void RefreshGrid()
    {
        _grid.DataSource = InteractionRepository.GetAll();
        _grid.Columns["Id"].Visible = false;
    }

    private void SaveInteraction()
    {
        if (_cmbClient.SelectedValue is null) return;
        if (string.IsNullOrWhiteSpace(_txtSujet.Text))
        {
            MessageBox.Show("Le sujet est obligatoire.");
            return;
        }

        var interaction = new Interaction
        {
            ClientId = Convert.ToInt32(_cmbClient.SelectedValue),
            UserId = _user.Id,
            TypeInteraction = _cmbType.SelectedItem?.ToString() ?? "Appel",
            Sujet = _txtSujet.Text.Trim(),
            Notes = _txtNotes.Text.Trim(),
            DateInteraction = DateTime.Now,
            NextFollowUpDate = _chkFollow.Checked ? _dtFollow.Value : null
        };

        InteractionRepository.Save(interaction);

        if (_chkFollow.Checked)
        {
            ReminderRepository.Save(new Reminder
            {
                ClientId = interaction.ClientId,
                Canal = interaction.TypeInteraction,
                Message = $"Relance automatique à partir de l'interaction : {interaction.Sujet}",
                ReminderDate = _dtFollow.Value,
                Statut = "Planifiée",
                IsNotified = false
            });
        }

        RefreshGrid();
        _txtSujet.Clear();
        _txtNotes.Clear();
        _chkFollow.Checked = false;
    }
}
