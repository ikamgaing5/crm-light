using CRMLight.Data;
using CRMLight.Models;

namespace CRMLight.Forms;

public class OpportunitiesForm : Form
{
    private readonly DataGridView _grid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
    private readonly ComboBox _cmbClient = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly TextBox _txtTitre = new();
    private readonly NumericUpDown _numMontant = new() { Maximum = 100000000, DecimalPlaces = 2, Width = 220 };
    private readonly ComboBox _cmbEtape = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly NumericUpDown _numProb = new() { Minimum = 0, Maximum = 100, Width = 220 };
    private readonly DateTimePicker _dtCloture = new() { Format = DateTimePickerFormat.Short };
    private int _selectedId;

    public OpportunitiesForm()
    {
        Text = "CRM Light - Opportunités";
        Width = 1200;
        Height = 700;
        StartPosition = FormStartPosition.CenterParent;

        _cmbEtape.Items.AddRange(new object[] { "Prospection", "Qualification", "Proposition", "Négociation", "Gagnée", "Perdue" });
        _cmbEtape.SelectedIndex = 0;
        _numProb.Value = 10;

        var left = new Panel { Dock = DockStyle.Left, Width = 380, Padding = new Padding(10) };
        left.Controls.Add(BuildForm());

        var buttons = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 50 };
        var btnSave = new Button { Text = "Enregistrer", Width = 120 };
        var btnRefresh = new Button { Text = "Actualiser", Width = 120 };
        var btnNew = new Button { Text = "Nouveau", Width = 120 };

        btnSave.Click += (_, _) => SaveOpportunity();
        btnRefresh.Click += (_, _) => RefreshGrid();
        btnNew.Click += (_, _) => ClearForm();

        buttons.Controls.AddRange(new Control[] { btnSave, btnRefresh, btnNew });
        left.Controls.Add(buttons);

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
        AddRow(panel, "Titre", _txtTitre);
        AddRow(panel, "Montant", _numMontant);
        AddRow(panel, "Étape", _cmbEtape);
        AddRow(panel, "Probabilité (%)", _numProb);
        AddRow(panel, "Clôture prévue", _dtCloture);

        return panel;
    }

    private void AddRow(TableLayoutPanel p, string label, Control ctrl)
    {
        var row = p.RowCount;
        p.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        p.Controls.Add(new Label { Text = label, AutoSize = true, Padding = new Padding(0, 8, 0, 0) }, 0, row);
        p.Controls.Add(ctrl, 1, row);
        p.RowCount++;
    }

    private void LoadClients()
    {
        var table = OpportunityRepository.GetClients();
        _cmbClient.DataSource = table;
        _cmbClient.DisplayMember = "Label";
        _cmbClient.ValueMember = "Id";
    }

    private void RefreshGrid()
    {
        _grid.DataSource = OpportunityRepository.GetAll();
        _grid.Columns["Id"].Visible = false;
    }

    private void SaveOpportunity()
    {
        if (_cmbClient.SelectedValue is null || string.IsNullOrWhiteSpace(_txtTitre.Text))
        {
            MessageBox.Show("Client et titre sont obligatoires.");
            return;
        }

        OpportunityRepository.Save(new Opportunity
        {
            Id = _selectedId,
            ClientId = Convert.ToInt32(_cmbClient.SelectedValue),
            Titre = _txtTitre.Text.Trim(),
            Montant = _numMontant.Value,
            Etape = _cmbEtape.SelectedItem?.ToString() ?? "Prospection",
            Probabilite = (int)_numProb.Value,
            DateCreation = DateTime.Now,
            DateCloturePrevue = _dtCloture.Value
        });

        RefreshGrid();
        ClearForm();
    }

    private void ClearForm()
    {
        _selectedId = 0;
        _txtTitre.Clear();
        _numMontant.Value = 0;
        _cmbEtape.SelectedIndex = 0;
        _numProb.Value = 10;
    }
}
