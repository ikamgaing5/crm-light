using CRMLight.Data;

namespace CRMLight.Forms;

public class NotificationsForm : Form
{
    private readonly DataGridView _grid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

    public NotificationsForm()
    {
        Text = "CRM Light - Notifications";
        Width = 1000;
        Height = 650;
        StartPosition = FormStartPosition.CenterParent;

        var panel = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 50, Padding = new Padding(10) };
        var btnRefresh = new Button { Text = "Actualiser", Width = 110 };
        var btnRead = new Button { Text = "Marquer lu", Width = 110 };

        btnRefresh.Click += (_, _) => RefreshGrid();
        btnRead.Click += (_, _) => MarkRead();

        panel.Controls.AddRange(new Control[] { btnRefresh, btnRead });

        Controls.Add(_grid);
        Controls.Add(panel);

        Load += (_, _) => RefreshGrid();
    }

    private void RefreshGrid()
    {
        _grid.DataSource = NotificationRepository.GetAll();
        _grid.Columns["Id"].Visible = false;
    }

    private void MarkRead()
    {
        if (_grid.CurrentRow is null) return;
        var id = Convert.ToInt32(_grid.CurrentRow.Cells["Id"].Value);
        NotificationRepository.MarkRead(id);
        RefreshGrid();
    }
}
