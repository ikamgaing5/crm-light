using CRMLight.Data;
using CRMLight.Models;

namespace CRMLight.Forms;

public class ClientsForm : Form
{
    private readonly DataGridView _grid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
    private readonly TextBox _txtCode = new();
    private readonly TextBox _txtNom = new();
    private readonly TextBox _txtEntreprise = new();
    private readonly TextBox _txtEmail = new();
    private readonly TextBox _txtTelephone = new();
    private readonly TextBox _txtAdresse = new();
    private readonly TextBox _txtSource = new();
    private readonly ComboBox _cmbStatut = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private int _selectedId = 0;

    public ClientsForm()
    {
        Text = "CRM Light - Clients";
        Width = 1200;
        Height = 700;
        StartPosition = FormStartPosition.CenterParent;

        _cmbStatut.Items.AddRange(new object[] { "Prospect", "Client", "Inactif" });
        _cmbStatut.SelectedIndex = 0;

        var formPanel = BuildFormPanel();
        var buttons = BuildButtons();

        var left = new Panel { Dock = DockStyle.Left, Width = 420, Padding = new Padding(10), AutoScroll = true };
        left.Controls.Add(buttons);
        left.Controls.Add(formPanel);

        _grid.CellDoubleClick += Grid_DoubleClick;
        Load += (_, _) => RefreshGrid();

        Controls.Add(_grid);
        Controls.Add(left);
    }

    private Control BuildFormPanel()
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            RowCount = 0,
            ColumnCount = 2,
            AutoSize = true,
            Padding = new Padding(10),
            AutoScroll = true
        };
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));

        AddRow(panel, "Code client", _txtCode);
        AddRow(panel, "Nom", _txtNom);
        AddRow(panel, "Entreprise", _txtEntreprise);
        AddRow(panel, "Email", _txtEmail);
        AddRow(panel, "Téléphone", _txtTelephone);
        AddRow(panel, "Adresse", _txtAdresse);
        AddRow(panel, "Source", _txtSource);
        AddRow(panel, "Statut", _cmbStatut);

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

    private Control BuildButtons()
    {
        var panel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 55,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(8)
        };

        var btnNew = new Button { Text = "Nouveau", Width = 90 };
        var btnSave = new Button { Text = "Enregistrer", Width = 90 };
        var btnDelete = new Button { Text = "Supprimer", Width = 90 };
        var btnRefresh = new Button { Text = "Actualiser", Width = 90 };

        btnNew.Click += (_, _) => ClearForm();
        btnSave.Click += (_, _) => SaveClient();
        btnDelete.Click += (_, _) => DeleteClient();
        btnRefresh.Click += (_, _) => RefreshGrid();

        panel.Controls.AddRange(new Control[] { btnNew, btnSave, btnDelete, btnRefresh });
        return panel;
    }

    private void RefreshGrid()
    {
        _grid.DataSource = ClientRepository.GetAll();
        _grid.Columns["Id"].Visible = false;
    }

    private void Grid_DoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var row = _grid.Rows[e.RowIndex];
        _selectedId = Convert.ToInt32(row.Cells["Id"].Value);
        _txtCode.Text = row.Cells["CodeClient"].Value?.ToString();
        _txtNom.Text = row.Cells["Nom"].Value?.ToString();
        _txtEntreprise.Text = row.Cells["Entreprise"].Value?.ToString();
        _txtEmail.Text = row.Cells["Email"].Value?.ToString();
        _txtTelephone.Text = row.Cells["Telephone"].Value?.ToString();
        _txtAdresse.Text = row.Cells["Adresse"].Value?.ToString();
        _txtSource.Text = row.Cells["Source"].Value?.ToString();
        _cmbStatut.SelectedItem = row.Cells["Statut"].Value?.ToString() ?? "Prospect";
    }

    private void SaveClient()
    {
        if (string.IsNullOrWhiteSpace(_txtNom.Text))
        {
            MessageBox.Show("Le nom du client est obligatoire.");
            return;
        }

        var client = new Client
        {
            Id = _selectedId,
            CodeClient = string.IsNullOrWhiteSpace(_txtCode.Text) ? GenerateCode() : _txtCode.Text.Trim(),
            Nom = _txtNom.Text.Trim(),
            Entreprise = _txtEntreprise.Text.Trim(),
            Email = _txtEmail.Text.Trim(),
            Telephone = _txtTelephone.Text.Trim(),
            Adresse = _txtAdresse.Text.Trim(),
            Source = _txtSource.Text.Trim(),
            Statut = _cmbStatut.SelectedItem?.ToString() ?? "Prospect"
        };

        ClientRepository.Save(client);
        RefreshGrid();
        ClearForm();
    }

    private void DeleteClient()
    {
        if (_selectedId == 0) return;
        if (MessageBox.Show("Supprimer ce client ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            ClientRepository.Delete(_selectedId);
            RefreshGrid();
            ClearForm();
        }
    }

    private void ClearForm()
    {
        _selectedId = 0;
        _txtCode.Clear();
        _txtNom.Clear();
        _txtEntreprise.Clear();
        _txtEmail.Clear();
        _txtTelephone.Clear();
        _txtAdresse.Clear();
        _txtSource.Clear();
        _cmbStatut.SelectedIndex = 0;
    }

    private static string GenerateCode() => $"CLI-{DateTime.Now:yyyyMMddHHmmss}";
}
