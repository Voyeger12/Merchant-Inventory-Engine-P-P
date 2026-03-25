using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Threading.Tasks;
using MerchantInventoryEngine.Controllers;
using MerchantInventoryEngine.Data;
using MerchantInventoryEngine.Models;
using MerchantInventoryEngine.Services;

namespace MerchantInventoryEngine
{
    public partial class Form1 : Form
    {
        private MerchantController _controller;
        private List<InventoryItem> _currentInventory;
        private List<InventoryItem> _allInventory;

        public Form1()
        {
            InitializeComponent();
            var db = new DatabaseHelper();
            if (!db.IsDatabaseHealthy())
            {
                MessageBox.Show("Database health check failed. See logs/app.log for details.", "Database warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            var calc = new PriceCalculator();
            _controller = new MerchantController(db, calc);
            _currentInventory = new List<InventoryItem>();
            _allInventory = new List<InventoryItem>();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            ApplyMedievalStyle();
            InitializeTooltips();
            await LoadModifiersAsync();
            SetStatus("Ready");
        }

        private void ApplyMedievalStyle()
        {
            var titleFont = GetReadableMedievalFont(11F, FontStyle.Bold);
            var bodyFont = GetReadableMedievalFont(10F, FontStyle.Regular);

            Text = "Merchant Inventory Engine - Guild Ledger";
            BackColor = Color.FromArgb(244, 232, 201);
            ForeColor = Color.FromArgb(43, 30, 18);
            Font = bodyFont;

            buttonCalculate.FlatStyle = FlatStyle.Flat;
            buttonCalculate.BackColor = Color.FromArgb(125, 72, 36);
            buttonCalculate.ForeColor = Color.FromArgb(255, 246, 220);
            buttonCalculate.FlatAppearance.BorderColor = Color.FromArgb(82, 49, 20);
            buttonCalculate.FlatAppearance.BorderSize = 2;
            buttonCalculate.Text = "Roll Prices";
            buttonCalculate.Font = titleFont;
            buttonCalculate.Size = new Size(130, 32);

            buttonExport.FlatStyle = FlatStyle.Flat;
            buttonExport.BackColor = Color.FromArgb(125, 72, 36);
            buttonExport.ForeColor = Color.FromArgb(255, 246, 220);
            buttonExport.FlatAppearance.BorderColor = Color.FromArgb(82, 49, 20);
            buttonExport.FlatAppearance.BorderSize = 2;
            buttonExport.Text = "Export Scroll";
            buttonExport.Font = titleFont;
            buttonExport.Size = new Size(130, 32);

            buttonApplyFilter.FlatStyle = FlatStyle.Flat;
            buttonApplyFilter.BackColor = Color.FromArgb(125, 72, 36);
            buttonApplyFilter.ForeColor = Color.FromArgb(255, 246, 220);
            buttonApplyFilter.FlatAppearance.BorderColor = Color.FromArgb(82, 49, 20);
            buttonApplyFilter.FlatAppearance.BorderSize = 2;

            buttonResetFilter.FlatStyle = FlatStyle.Flat;
            buttonResetFilter.BackColor = Color.FromArgb(145, 96, 56);
            buttonResetFilter.ForeColor = Color.FromArgb(255, 246, 220);
            buttonResetFilter.FlatAppearance.BorderColor = Color.FromArgb(82, 49, 20);
            buttonResetFilter.FlatAppearance.BorderSize = 2;

            StyleComboBox(comboBoxPersonality);
            StyleComboBox(comboBoxLocation);
            StyleComboBox(comboBoxPolitical);
            StyleComboBox(comboBoxFaction);

            label1.Font = titleFont;
            label2.Font = titleFont;
            label3.Font = titleFont;
            label8.Font = titleFont;
            label1.ForeColor = Color.FromArgb(43, 30, 18);
            label2.ForeColor = Color.FromArgb(43, 30, 18);
            label3.ForeColor = Color.FromArgb(43, 30, 18);
            label8.ForeColor = Color.FromArgb(43, 30, 18);
            label1.Text = "Merchant Trait";
            label2.Text = "Region";
            label3.Text = "Realm Politics";
            label8.Text = "Faction";
            label4.Text = "Search";
            label5.Text = "Category";
            label6.Text = "Min";
            label7.Text = "Max";

            textBoxSearch.BackColor = Color.FromArgb(251, 242, 220);
            textBoxSearch.ForeColor = Color.FromArgb(43, 30, 18);
            textBoxSearch.Font = bodyFont;

            comboBoxCategoryFilter.BackColor = Color.FromArgb(251, 242, 220);
            comboBoxCategoryFilter.ForeColor = Color.FromArgb(43, 30, 18);
            comboBoxCategoryFilter.FlatStyle = FlatStyle.Flat;
            comboBoxCategoryFilter.Font = bodyFont;

            numericUpDownMinPrice.BackColor = Color.FromArgb(251, 242, 220);
            numericUpDownMaxPrice.BackColor = Color.FromArgb(251, 242, 220);
            numericUpDownMinPrice.ForeColor = Color.FromArgb(43, 30, 18);
            numericUpDownMaxPrice.ForeColor = Color.FromArgb(43, 30, 18);

            statusStrip1.BackColor = Color.FromArgb(99, 61, 29);
            toolStripStatusLabel1.ForeColor = Color.FromArgb(255, 246, 220);

            dataGridViewInventory.BackgroundColor = Color.FromArgb(236, 221, 180);
            dataGridViewInventory.GridColor = Color.FromArgb(108, 67, 33);
            dataGridViewInventory.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewInventory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewInventory.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewInventory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewInventory.MultiSelect = false;
            dataGridViewInventory.ReadOnly = true;

            dataGridViewInventory.DefaultCellStyle.BackColor = Color.FromArgb(251, 242, 220);
            dataGridViewInventory.DefaultCellStyle.ForeColor = Color.FromArgb(27, 21, 14);
            dataGridViewInventory.DefaultCellStyle.SelectionBackColor = Color.FromArgb(93, 58, 28);
            dataGridViewInventory.DefaultCellStyle.SelectionForeColor = Color.FromArgb(255, 247, 232);
            dataGridViewInventory.DefaultCellStyle.Font = bodyFont;

            dataGridViewInventory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 233, 205);

            dataGridViewInventory.EnableHeadersVisualStyles = false;
            dataGridViewInventory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(99, 61, 29);
            dataGridViewInventory.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(255, 246, 220);
            dataGridViewInventory.ColumnHeadersDefaultCellStyle.Font = titleFont;
        }

        private static Font GetReadableMedievalFont(float size, FontStyle style)
        {
            var candidates = new[]
            {
                "Palatino Linotype",
                "Book Antiqua",
                "Constantia",
                "Georgia"
            };

            using var installedFonts = new InstalledFontCollection();
            foreach (var candidate in candidates)
            {
                foreach (var family in installedFonts.Families)
                {
                    if (family.Name.Equals(candidate, StringComparison.OrdinalIgnoreCase))
                    {
                        return new Font(candidate, size, style);
                    }
                }
            }

            return new Font("Segoe UI", size, style);
        }

        private void StyleComboBox(ComboBox comboBox)
        {
            comboBox.BackColor = Color.FromArgb(251, 242, 220);
            comboBox.ForeColor = Color.FromArgb(43, 30, 18);
            comboBox.FlatStyle = FlatStyle.Flat;
            comboBox.Font = GetReadableMedievalFont(10F, FontStyle.Regular);
        }

        private async Task LoadModifiersAsync()
        {
            comboBoxPersonality.DataSource = await _controller.GetModifiersAsync("Personality");
            comboBoxPersonality.DisplayMember = "Name";
            comboBoxPersonality.ValueMember = "Multiplier";

            comboBoxLocation.DataSource = await _controller.GetModifiersAsync("Location");
            comboBoxLocation.DisplayMember = "Name";
            comboBoxLocation.ValueMember = "Multiplier";

            comboBoxPolitical.DataSource = await _controller.GetModifiersAsync("Political");
            comboBoxPolitical.DisplayMember = "Name";
            comboBoxPolitical.ValueMember = "Multiplier";

            comboBoxFaction.DataSource = await _controller.GetModifiersAsync("Faction");
            comboBoxFaction.DisplayMember = "Name";
            comboBoxFaction.ValueMember = "Multiplier";

            comboBoxCategoryFilter.DataSource = new List<string> { "All" };
        }

        private async void buttonCalculate_Click(object sender, EventArgs e)
        {
            if (TryGetSelectedMultiplier(comboBoxPersonality, out var pMult) &&
                TryGetSelectedMultiplier(comboBoxLocation, out var lMult) &&
                TryGetSelectedMultiplier(comboBoxPolitical, out var polMult) &&
                TryGetSelectedMultiplier(comboBoxFaction, out var factionMult))
            {
                try
                {
                    SetStatus("Calculating...");
                    _allInventory = await _controller.CalculateInventoryAsync(pMult, lMult, polMult, factionMult);
                    _currentInventory = _allInventory.ToList();
                    BindInventory(_currentInventory);
                    PopulateCategoryFilter();
                    SetStatus($"Calculated {_currentInventory.Count} items.");
                }
                catch (Exception ex)
                {
                    AppLogger.Error("Calculation failed.", ex);
                    MessageBox.Show(ex.Message, "Calculation error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SetStatus("Calculation failed.");
                }
            }
            else
            {
                MessageBox.Show("Please select all multipliers.", "Invalid selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool TryGetSelectedMultiplier(ComboBox comboBox, out decimal multiplier)
        {
            multiplier = 1.0m;

            if (comboBox.SelectedValue == null)
            {
                return false;
            }

            try
            {
                multiplier = Convert.ToDecimal(comboBox.SelectedValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async void buttonExport_Click(object sender, EventArgs e)
        {
            if (_currentInventory == null || _currentInventory.Count == 0)
            {
                MessageBox.Show("No inventory to export. Please calculate first.");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "CSV file|*.csv|Text file|*.txt", Title = "Export Inventory" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        SetStatus("Exporting...");
                        await _controller.ExportToCsvAsync(sfd.FileName, _currentInventory, includeBom: true);
                        MessageBox.Show("Export successful!");
                        SetStatus("Export successful.");
                    }
                    catch (Exception ex)
                    {
                        AppLogger.Error("Export failed.", ex);
                        MessageBox.Show("Export failed.", "Export error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        SetStatus("Export failed.");
                    }
                }
            }
        }

        private void buttonApplyFilter_Click(object sender, EventArgs e)
        {
            var name = string.IsNullOrWhiteSpace(textBoxSearch.Text) ? null : textBoxSearch.Text.Trim();
            var category = comboBoxCategoryFilter.SelectedItem as string;
            if (string.Equals(category, "All", StringComparison.OrdinalIgnoreCase))
            {
                category = null;
            }

            decimal? min = numericUpDownMinPrice.Value > 0 ? numericUpDownMinPrice.Value : null;
            decimal? max = numericUpDownMaxPrice.Value > 0 ? numericUpDownMaxPrice.Value : null;

            if (min.HasValue && max.HasValue && min.Value > max.Value)
            {
                MessageBox.Show("Min value cannot be greater than max value.", "Filter validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _currentInventory = _controller.FilterInventory(_allInventory, name, category, min, max);
            BindInventory(_currentInventory);
            SetStatus($"Filter applied: {_currentInventory.Count} items.");
        }

        private void buttonResetFilter_Click(object sender, EventArgs e)
        {
            textBoxSearch.Text = string.Empty;
            comboBoxCategoryFilter.SelectedIndex = 0;
            numericUpDownMinPrice.Value = 0;
            numericUpDownMaxPrice.Value = 0;
            _currentInventory = _allInventory.ToList();
            BindInventory(_currentInventory);
            SetStatus("Filter reset.");
        }

        private void BindInventory(List<InventoryItem> items)
        {
            dataGridViewInventory.DataSource = null;
            dataGridViewInventory.DataSource = items;

            var finalPriceColumn = dataGridViewInventory.Columns["FinalPrice"];
            if (finalPriceColumn != null)
            {
                finalPriceColumn.DefaultCellStyle.Format = "N2";
            }
        }

        private void PopulateCategoryFilter()
        {
            var categories = _allInventory
                .Select(i => i.CategoryName)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(c => c)
                .ToList();

            categories.Insert(0, "All");
            comboBoxCategoryFilter.DataSource = categories;
            comboBoxCategoryFilter.SelectedIndex = 0;
        }

        private void InitializeTooltips()
        {
            toolTip1.SetToolTip(comboBoxPersonality, "Merchant personality multiplier");
            toolTip1.SetToolTip(comboBoxLocation, "Location multiplier");
            toolTip1.SetToolTip(comboBoxPolitical, "Political situation multiplier");
            toolTip1.SetToolTip(comboBoxFaction, "Faction/reputation multiplier");
            toolTip1.SetToolTip(buttonCalculate, "Calculate final prices");
            toolTip1.SetToolTip(buttonExport, "Export current view to CSV/TXT");
            toolTip1.SetToolTip(buttonApplyFilter, "Apply search and price filters");
            toolTip1.SetToolTip(buttonResetFilter, "Clear all filters");
        }

        private void SetStatus(string message)
        {
            toolStripStatusLabel1.Text = message;
        }
    }
}
