using System.Drawing;
using System.Windows.Forms;

namespace MerchantInventoryEngine;

public class SplashForm : Form
{
    private readonly Label _statusLabel;

    public SplashForm()
    {
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterScreen;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        TopMost = true;
        Width = 520;
        Height = 180;
        BackColor = Color.FromArgb(244, 232, 201);
        Text = "Merchant Inventory Engine";

        var titleLabel = new Label
        {
            AutoSize = false,
            Left = 20,
            Top = 16,
            Width = 470,
            Height = 32,
            Text = "Merchant Inventory Engine",
            Font = new Font("Palatino Linotype", 14f, FontStyle.Bold),
            ForeColor = Color.FromArgb(43, 30, 18)
        };

        _statusLabel = new Label
        {
            AutoSize = false,
            Left = 20,
            Top = 64,
            Width = 470,
            Height = 26,
            Text = "Starting...",
            Font = new Font("Segoe UI", 10f, FontStyle.Regular),
            ForeColor = Color.FromArgb(43, 30, 18)
        };

        var progress = new ProgressBar
        {
            Left = 20,
            Top = 100,
            Width = 470,
            Height = 20,
            Style = ProgressBarStyle.Marquee,
            MarqueeAnimationSpeed = 30
        };

        Controls.Add(titleLabel);
        Controls.Add(_statusLabel);
        Controls.Add(progress);
    }

    public void SetStatus(string message)
    {
        _statusLabel.Text = message;
        _statusLabel.Refresh();
        Refresh();
        Application.DoEvents();
    }
}
