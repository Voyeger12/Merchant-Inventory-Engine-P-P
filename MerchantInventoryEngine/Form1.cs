using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
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
        private const int MinGridWidth = 720;
        private const int MinFilterButtonWidth = 110;
        private const int MinNumericWidth = 100;
        private const int MinSearchWidth = 180;
        private const int MinCategoryWidth = 180;

        private MerchantController _controller;
        private List<InventoryItem> _currentInventory;
        private List<InventoryItem> _allInventory;
        private bool _showPriceExplanation;
        private float _lastFontScale = -1f;

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
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            MinimumSize = new Size(1280, 720);

            ApplyMedievalStyle();
            InitializeTooltips();
            await LoadModifiersAsync();
            panelPriceExplanation.Visible = false;
            _showPriceExplanation = false;
            priceErklaerungsmodusAnzeigenToolStripMenuItem.Checked = false;
            Resize += Form1_Resize;
            UpdateResponsiveTypography();
            UpdateResponsiveLayout();
            SetStatus("⚔️ The Merchant stands ready...");
        }

        private void Form1_Resize(object? sender, EventArgs e)
        {
            UpdateResponsiveTypography();
            UpdateResponsiveLayout();
        }

        private void ApplyMedievalStyle()
        {
            var titleFont = GetReadableMedievalFont(11F, FontStyle.Bold);
            var bodyFont = GetReadableMedievalFont(10F, FontStyle.Regular);
            var buttonFont = new Font("Segoe UI Emoji", 11F, FontStyle.Bold);

            Text = "Merchant Inventory Engine - Guild Ledger";
            BackColor = Color.FromArgb(244, 232, 201);
            ForeColor = Color.FromArgb(43, 30, 18);
            Font = bodyFont;

            buttonCalculate.FlatStyle = FlatStyle.Flat;
            buttonCalculate.BackColor = Color.FromArgb(125, 72, 36);
            buttonCalculate.ForeColor = Color.FromArgb(255, 246, 220);
            buttonCalculate.FlatAppearance.BorderColor = Color.FromArgb(82, 49, 20);
            buttonCalculate.FlatAppearance.BorderSize = 2;
            buttonCalculate.FlatAppearance.MouseOverBackColor = Color.FromArgb(145, 86, 45);
            buttonCalculate.FlatAppearance.MouseDownBackColor = Color.FromArgb(94, 56, 27);
            buttonCalculate.Text = "🎲 Roll Prices";
            buttonCalculate.Font = buttonFont;
            buttonCalculate.Size = new Size(200, 42);
            buttonCalculate.TextAlign = ContentAlignment.MiddleCenter;

            buttonExport.FlatStyle = FlatStyle.Flat;
            buttonExport.BackColor = Color.FromArgb(125, 72, 36);
            buttonExport.ForeColor = Color.FromArgb(255, 246, 220);
            buttonExport.FlatAppearance.BorderColor = Color.FromArgb(82, 49, 20);
            buttonExport.FlatAppearance.BorderSize = 2;
            buttonExport.FlatAppearance.MouseOverBackColor = Color.FromArgb(145, 86, 45);
            buttonExport.FlatAppearance.MouseDownBackColor = Color.FromArgb(94, 56, 27);
            buttonExport.Text = "📜 Export Scroll";
            buttonExport.Font = buttonFont;
            buttonExport.Size = new Size(200, 42);
            buttonExport.TextAlign = ContentAlignment.MiddleCenter;

            buttonApplyFilter.FlatStyle = FlatStyle.Flat;
            buttonApplyFilter.BackColor = Color.FromArgb(125, 72, 36);
            buttonApplyFilter.ForeColor = Color.FromArgb(255, 246, 220);
            buttonApplyFilter.FlatAppearance.BorderColor = Color.FromArgb(82, 49, 20);
            buttonApplyFilter.FlatAppearance.BorderSize = 2;
            buttonApplyFilter.FlatAppearance.MouseOverBackColor = Color.FromArgb(145, 86, 45);
            buttonApplyFilter.FlatAppearance.MouseDownBackColor = Color.FromArgb(94, 56, 27);

            buttonResetFilter.FlatStyle = FlatStyle.Flat;
            buttonResetFilter.BackColor = Color.FromArgb(145, 96, 56);
            buttonResetFilter.ForeColor = Color.FromArgb(255, 246, 220);
            buttonResetFilter.FlatAppearance.BorderColor = Color.FromArgb(82, 49, 20);
            buttonResetFilter.FlatAppearance.BorderSize = 2;
            buttonResetFilter.FlatAppearance.MouseOverBackColor = Color.FromArgb(160, 109, 67);
            buttonResetFilter.FlatAppearance.MouseDownBackColor = Color.FromArgb(101, 64, 33);

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
            label1.Text = "🎭 Merchant Trait";
            label2.Text = "📍 Region";
            label3.Text = "⚖️ Realm Politics";
            label8.Text = "👑 Faction";
            label4.Text = "🔍 Search";
            label5.Text = "📦 Category";
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

            menuStrip1.BackColor = Color.FromArgb(99, 61, 29);
            menuStrip1.ForeColor = Color.FromArgb(255, 246, 220);
            menuStrip1.Renderer = new ToolStripProfessionalRenderer(new MedievalMenuColorTable());
            menuStrip1.AutoSize = false;
            menuStrip1.Height = 38;
            menuStrip1.Padding = new Padding(12, 4, 12, 4);
            foreach (ToolStripMenuItem item in menuStrip1.Items)
            {
                item.ForeColor = Color.FromArgb(255, 246, 220);
                item.Font = titleFont;
                item.Padding = new Padding(10, 4, 10, 4);
                item.Margin = new Padding(4, 0, 4, 0);
                foreach (ToolStripItem dropDownItem in item.DropDownItems)
                {
                    dropDownItem.ForeColor = Color.FromArgb(255, 246, 220);
                    dropDownItem.Font = bodyFont;
                }
            }

            panelPriceExplanation.BackColor = Color.FromArgb(245, 233, 205);
            labelPriceExplanationTitle.Font = titleFont;
            labelPriceExplanationTitle.ForeColor = Color.FromArgb(43, 30, 18);
            textBoxPriceExplanation.BackColor = Color.FromArgb(251, 242, 220);
            textBoxPriceExplanation.ForeColor = Color.FromArgb(27, 21, 14);
            textBoxPriceExplanation.Font = bodyFont;

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

        private void UpdateResponsiveLayout()
        {
            if (ClientSize.Width <= 0 || ClientSize.Height <= 0)
            {
                return;
            }

            var margin = 12;
            var topY = menuStrip1.Bottom + 12;
            var statusHeight = statusStrip1.Height;
            var bottomRowY = ClientSize.Height - statusHeight - 38;

            var leftLabelWidth = new[]
            {
                TextRenderer.MeasureText(label1.Text, label1.Font).Width,
                TextRenderer.MeasureText(label2.Text, label2.Font).Width,
                TextRenderer.MeasureText(label3.Text, label3.Font).Width
            }.Max() + 8;

            var comboHeight = comboBoxPersonality.Height;
            var rowHeight = Math.Max(comboHeight, label1.Height) + 8;

            var comboX = margin + leftLabelWidth + 8;
            var y1 = topY;
            var y2 = y1 + rowHeight;
            var y3 = y2 + rowHeight;
            var gridTop = y3 + rowHeight + 10;

            var panelVisible = _showPriceExplanation;
            var desiredPanelWidth = Math.Clamp(ClientSize.Width / 3, 300, 420);
            var minGridWhenPanelVisible = 520;
            var maxPanelWidth = Math.Max(260, ClientSize.Width - (margin * 3) - minGridWhenPanelVisible);
            var panelWidth = panelVisible ? Math.Min(desiredPanelWidth, maxPanelWidth) : 0;
            var panelX = ClientSize.Width - margin - panelWidth;

            var rightLimit = panelVisible ? panelX - margin : ClientSize.Width - margin;
            var minGridWidth = panelVisible ? 420 : MinGridWidth;
            var gridWidth = Math.Max(minGridWidth, rightLimit - margin);
            var gridHeight = Math.Max(260, bottomRowY - gridTop - 14);

            dataGridViewInventory.SetBounds(margin, gridTop, gridWidth, gridHeight);

            panelPriceExplanation.Visible = panelVisible;
            if (panelVisible)
            {
                panelPriceExplanation.SetBounds(panelX, gridTop, panelWidth, gridHeight);
                labelPriceExplanationTitle.Location = new Point(9, 10);
                var explanationTop = labelPriceExplanationTitle.Bottom + 8;
                textBoxPriceExplanation.SetBounds(9, explanationTop, panelPriceExplanation.Width - 20, panelPriceExplanation.Height - explanationTop - 10);
            }

            // Top controls
            var comboW = Math.Max(180, Math.Min(300, (rightLimit - comboX - 280) / 2));

            label1.Location = new Point(margin, y1 + 4);
            label2.Location = new Point(margin, y2 + 4);
            label3.Location = new Point(margin, y3 + 4);

            comboBoxPersonality.SetBounds(comboX, y1, comboW, comboHeight);
            comboBoxLocation.SetBounds(comboX, y2, comboW, comboHeight);
            comboBoxPolitical.SetBounds(comboX, y3, comboW, comboHeight);

            var factionLabelX = comboBoxPersonality.Right + 28;
            label8.Location = new Point(factionLabelX, y1 + 4);
            var factionLabelWidth = TextRenderer.MeasureText(label8.Text, label8.Font).Width + 8;
            comboBoxFaction.SetBounds(factionLabelX + factionLabelWidth, y1, comboW, comboBoxFaction.Height);

            var buttonY = y3;
            var buttonW = 200;
            var buttonH = 42;
            var calcX = comboBoxFaction.Left;
            var exportX = calcX + buttonW + 18;
            if (exportX + buttonW > rightLimit)
            {
                exportX = rightLimit - buttonW;
                calcX = exportX - buttonW - 18;
            }
            buttonCalculate.SetBounds(calcX, buttonY, buttonW, buttonH);
            buttonExport.SetBounds(exportX, buttonY, buttonW, buttonH);

            // Bottom filter row (responsive, measured layout)
            var gap = 10;
            var filterButtonW = MinFilterButtonWidth;
            var filterButtonH = Math.Max(32, buttonApplyFilter.Height);
            var numericW = MinNumericWidth;

            var searchLabelW = TextRenderer.MeasureText(label4.Text, label4.Font).Width;
            var categoryLabelW = TextRenderer.MeasureText(label5.Text, label5.Font).Width;
            var minLabelW = TextRenderer.MeasureText(label6.Text, label6.Font).Width;
            var maxLabelW = TextRenderer.MeasureText(label7.Text, label7.Font).Width;

            var resetX = rightLimit - filterButtonW;
            var applyX = resetX - gap - filterButtonW;
            buttonApplyFilter.SetBounds(applyX, bottomRowY - 2, filterButtonW, filterButtonH);
            buttonResetFilter.SetBounds(resetX, bottomRowY - 2, filterButtonW, filterButtonH);

            var maxInputX = applyX - gap - numericW;
            var maxLabelX = maxInputX - 6 - maxLabelW;
            label7.Location = new Point(maxLabelX, bottomRowY + 4);
            numericUpDownMaxPrice.SetBounds(maxInputX, bottomRowY, numericW, numericUpDownMaxPrice.Height);

            var minInputX = maxLabelX - gap - numericW;
            var minLabelX = minInputX - 6 - minLabelW;
            label6.Location = new Point(minLabelX, bottomRowY + 4);
            numericUpDownMinPrice.SetBounds(minInputX, bottomRowY, numericW, numericUpDownMinPrice.Height);

            var leftStart = margin;
            label4.Location = new Point(leftStart, bottomRowY + 4);
            var searchInputX = leftStart + searchLabelW + 8;

            var categoryLabelXMin = searchInputX + MinSearchWidth + gap;
            var categoryLabelXMax = minLabelX - gap - categoryLabelW - 6 - MinCategoryWidth;
            var categoryLabelX = Math.Max(categoryLabelXMin, categoryLabelXMax);

            var searchW = Math.Max(MinSearchWidth, categoryLabelX - gap - searchInputX);
            textBoxSearch.SetBounds(searchInputX, bottomRowY, searchW, textBoxSearch.Height);

            label5.Location = new Point(categoryLabelX, bottomRowY + 4);
            var categoryInputX = categoryLabelX + categoryLabelW + 6;
            var categoryW = Math.Max(MinCategoryWidth, minLabelX - gap - categoryInputX);
            comboBoxCategoryFilter.SetBounds(categoryInputX, bottomRowY, categoryW, comboBoxCategoryFilter.Height);
        }

        private void UpdateResponsiveTypography()
        {
            var dpiScale = DeviceDpi / 96f;
            var scale = Math.Clamp((ClientSize.Width / 1500f) * dpiScale, 0.9f, 1.2f);
            if (Math.Abs(scale - _lastFontScale) < 0.02f)
            {
                return;
            }

            _lastFontScale = scale;

            var titleFont = GetReadableMedievalFont(11f * scale, FontStyle.Bold);
            var bodyFont = GetReadableMedievalFont(10f * scale, FontStyle.Regular);
            var buttonFont = new Font("Segoe UI Emoji", 11f * scale, FontStyle.Bold);

            label1.Font = titleFont;
            label2.Font = titleFont;
            label3.Font = titleFont;
            label8.Font = titleFont;
            label4.Font = bodyFont;
            label5.Font = bodyFont;
            label6.Font = bodyFont;
            label7.Font = bodyFont;

            comboBoxPersonality.Font = bodyFont;
            comboBoxLocation.Font = bodyFont;
            comboBoxPolitical.Font = bodyFont;
            comboBoxFaction.Font = bodyFont;
            comboBoxCategoryFilter.Font = bodyFont;

            textBoxSearch.Font = bodyFont;
            textBoxPriceExplanation.Font = bodyFont;
            labelPriceExplanationTitle.Font = titleFont;

            menuStrip1.Font = titleFont;
            menuStrip1.Height = Math.Max(34, (int)(38 * scale));
            foreach (ToolStripMenuItem item in menuStrip1.Items)
            {
                item.Font = titleFont;
                foreach (ToolStripItem dropDownItem in item.DropDownItems)
                {
                    dropDownItem.Font = bodyFont;
                }
            }

            buttonCalculate.Font = buttonFont;
            buttonExport.Font = buttonFont;
            buttonCalculate.Size = new Size((int)(200 * scale), (int)(42 * scale));
            buttonExport.Size = new Size((int)(200 * scale), (int)(42 * scale));

            buttonApplyFilter.Font = bodyFont;
            buttonResetFilter.Font = bodyFont;
            buttonApplyFilter.Size = new Size(Math.Max(110, (int)(110 * scale)), Math.Max(32, (int)(34 * scale)));
            buttonResetFilter.Size = new Size(Math.Max(110, (int)(110 * scale)), Math.Max(32, (int)(34 * scale)));
            numericUpDownMinPrice.Font = bodyFont;
            numericUpDownMaxPrice.Font = bodyFont;

            dataGridViewInventory.DefaultCellStyle.Font = bodyFont;
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
                    SetStatus("Rolling the fates...");
                    _allInventory = await _controller.CalculateInventoryAsync(pMult, lMult, polMult, factionMult);
                    _currentInventory = _allInventory.ToList();
                    BindInventory(_currentInventory);
                    PopulateCategoryFilter();
                    SetStatus($"✨ {_currentInventory.Count} wares discovered");
                }
                catch (Exception ex)
                {
                    AppLogger.Error("Calculation failed.", ex);
                    MessageBox.Show(ex.Message, "Calculation error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SetStatus("The fates turned cruel...");
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
                        SetStatus("The scroll has been sealed.");
                    }
                    catch (Exception ex)
                    {
                        AppLogger.Error("Export failed.", ex);
                        MessageBox.Show("Export failed.", "Export error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        SetStatus("The scroll could not be sealed.");
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
            SetStatus($"Sifted through the wares: {_currentInventory.Count} remain.");
        }

        private void buttonResetFilter_Click(object sender, EventArgs e)
        {
            textBoxSearch.Text = string.Empty;
            comboBoxCategoryFilter.SelectedIndex = 0;
            numericUpDownMinPrice.Value = 0;
            numericUpDownMaxPrice.Value = 0;
            _currentInventory = _allInventory.ToList();
            BindInventory(_currentInventory);
            SetStatus("All wares restored to view.");
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

            var explanationColumn = dataGridViewInventory.Columns["PriceExplanation"];
            if (explanationColumn != null)
            {
                explanationColumn.Visible = false;
                explanationColumn.HeaderText = "Price Explanation";
                explanationColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            UpdateExplanationView();
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
            toolTip1.SetToolTip(textBoxPriceExplanation, "Detailed item pricing explanation");
        }

        private void SetStatus(string message)
        {
            toolStripStatusLabel1.Text = message;
        }

        private void dataGridViewInventory_SelectionChanged(object sender, EventArgs e)
        {
            UpdateExplanationView();
        }

        private void UpdateExplanationView()
        {
            if (!_showPriceExplanation)
            {
                textBoxPriceExplanation.Text = string.Empty;
                return;
            }

            if (dataGridViewInventory.CurrentRow?.DataBoundItem is InventoryItem selectedItem)
            {
                textBoxPriceExplanation.Text = selectedItem.PriceExplanation;
            }
            else
            {
                textBoxPriceExplanation.Text = "Select an item to view price details.";
            }
        }

        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void exportierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonExport_Click(sender, e);
        }

        private void logsOffnenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var logDirectory = Path.Combine(AppContext.BaseDirectory, "logs");
                var logPath = Path.Combine(logDirectory, "app.log");
                Directory.CreateDirectory(logDirectory);
                if (!File.Exists(logPath))
                {
                    File.WriteAllText(logPath, "");
                }

                Process.Start(new ProcessStartInfo
                {
                    FileName = logPath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                AppLogger.Error("Failed to open logs.", ex);
                MessageBox.Show("Could not open log file.", "Log error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void priceErklaerungsmodusAnzeigenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _showPriceExplanation = priceErklaerungsmodusAnzeigenToolStripMenuItem.Checked;
            panelPriceExplanation.Visible = _showPriceExplanation;
            UpdateResponsiveLayout();
            BindInventory(_currentInventory);
            SetStatus(_showPriceExplanation ? "The ledger reveals its secrets..." : "The ledger conceals its secrets.");
        }

        private sealed class MedievalMenuColorTable : ProfessionalColorTable
        {
            public override Color MenuStripGradientBegin => Color.FromArgb(99, 61, 29);
            public override Color MenuStripGradientEnd => Color.FromArgb(99, 61, 29);
            public override Color ToolStripDropDownBackground => Color.FromArgb(99, 61, 29);
            public override Color ImageMarginGradientBegin => Color.FromArgb(99, 61, 29);
            public override Color ImageMarginGradientMiddle => Color.FromArgb(99, 61, 29);
            public override Color ImageMarginGradientEnd => Color.FromArgb(99, 61, 29);
            public override Color MenuItemSelected => Color.FromArgb(145, 86, 45);
            public override Color MenuItemSelectedGradientBegin => Color.FromArgb(145, 86, 45);
            public override Color MenuItemSelectedGradientEnd => Color.FromArgb(145, 86, 45);
            public override Color MenuItemBorder => Color.FromArgb(82, 49, 20);
            public override Color MenuItemPressedGradientBegin => Color.FromArgb(125, 72, 36);
            public override Color MenuItemPressedGradientMiddle => Color.FromArgb(125, 72, 36);
            public override Color MenuItemPressedGradientEnd => Color.FromArgb(125, 72, 36);
        }
    }
}
