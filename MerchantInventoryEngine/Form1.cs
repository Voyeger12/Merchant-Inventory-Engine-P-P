using System;
using System.Windows.Forms;
using System.Collections.Generic;
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

        public Form1()
        {
            InitializeComponent();
            var db = new DatabaseHelper();
            var calc = new PriceCalculator();
            _controller = new MerchantController(db, calc);
            _currentInventory = new List<InventoryItem>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var db = new DatabaseHelper();
            var calc = new PriceCalculator();
            _controller = new MerchantController(db, calc);

            LoadModifiers();
        }

        private void LoadModifiers()
        {
            comboBoxPersonality.DataSource = _controller.GetModifiers("Personality");
            comboBoxPersonality.DisplayMember = "Name";
            comboBoxPersonality.ValueMember = "Multiplier";

            comboBoxLocation.DataSource = _controller.GetModifiers("Location");
            comboBoxLocation.DisplayMember = "Name";
            comboBoxLocation.ValueMember = "Multiplier";

            comboBoxPolitical.DataSource = _controller.GetModifiers("Political");
            comboBoxPolitical.DisplayMember = "Name";
            comboBoxPolitical.ValueMember = "Multiplier";
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            if (comboBoxPersonality.SelectedValue is decimal pMult &&
                comboBoxLocation.SelectedValue is decimal lMult &&
                comboBoxPolitical.SelectedValue is decimal polMult)
            {
                _currentInventory = _controller.CalculateInventory(pMult, lMult, polMult);
                dataGridViewInventory.DataSource = _currentInventory;

                if (dataGridViewInventory.Columns["FinalPrice"] != null)
                {
                    dataGridViewInventory.Columns["FinalPrice"].DefaultCellStyle.Format = "N2";
                }
            }
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            if (_currentInventory == null || _currentInventory.Count == 0)
            {
                MessageBox.Show("No inventory to export. Please calculate first.");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "CSV file|*.csv", Title = "Export Inventory" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    _controller.ExportToCsv(sfd.FileName, _currentInventory);
                    MessageBox.Show("Export successful!");
                }
            }
        }
    }
}
