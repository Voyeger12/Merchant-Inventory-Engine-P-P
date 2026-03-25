namespace MerchantInventoryEngine
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.comboBoxPersonality = new System.Windows.Forms.ComboBox();
            this.comboBoxLocation = new System.Windows.Forms.ComboBox();
            this.comboBoxPolitical = new System.Windows.Forms.ComboBox();
            this.comboBoxFaction = new System.Windows.Forms.ComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dateiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.beendenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ansichtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.priceErklaerungsmodusAnzeigenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logsOffnenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.diagnoseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewInventory = new System.Windows.Forms.DataGridView();
            this.buttonCalculate = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.comboBoxCategoryFilter = new System.Windows.Forms.ComboBox();
            this.numericUpDownMinPrice = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMaxPrice = new System.Windows.Forms.NumericUpDown();
            this.buttonApplyFilter = new System.Windows.Forms.Button();
            this.buttonResetFilter = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panelPriceExplanation = new System.Windows.Forms.Panel();
            this.labelPriceExplanationTitle = new System.Windows.Forms.Label();
            this.textBoxPriceExplanation = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInventory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxPrice)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.panelPriceExplanation.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxPersonality
            // 
            this.comboBoxPersonality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPersonality.FormattingEnabled = true;
            this.comboBoxPersonality.Location = new System.Drawing.Point(160, 38);
            this.comboBoxPersonality.Name = "comboBoxPersonality";
            this.comboBoxPersonality.Size = new System.Drawing.Size(200, 23);
            this.comboBoxPersonality.TabIndex = 0;
            // 
            // comboBoxLocation
            // 
            this.comboBoxLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLocation.FormattingEnabled = true;
            this.comboBoxLocation.Location = new System.Drawing.Point(160, 67);
            this.comboBoxLocation.Name = "comboBoxLocation";
            this.comboBoxLocation.Size = new System.Drawing.Size(200, 23);
            this.comboBoxLocation.TabIndex = 1;
            // 
            // comboBoxPolitical
            // 
            this.comboBoxPolitical.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPolitical.FormattingEnabled = true;
            this.comboBoxPolitical.Location = new System.Drawing.Point(160, 96);
            this.comboBoxPolitical.Name = "comboBoxPolitical";
            this.comboBoxPolitical.Size = new System.Drawing.Size(200, 23);
            this.comboBoxPolitical.TabIndex = 2;
            // 
            // comboBoxFaction
            // 
            this.comboBoxFaction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFaction.FormattingEnabled = true;
            this.comboBoxFaction.Location = new System.Drawing.Point(700, 38);
            this.comboBoxFaction.Name = "comboBoxFaction";
            this.comboBoxFaction.Size = new System.Drawing.Size(200, 23);
            this.comboBoxFaction.TabIndex = 3;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateiToolStripMenuItem,
            this.ansichtToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1500, 24);
            this.menuStrip1.TabIndex = 30;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dateiToolStripMenuItem
            // 
            this.dateiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportierenToolStripMenuItem,
            this.beendenToolStripMenuItem});
            this.dateiToolStripMenuItem.Name = "dateiToolStripMenuItem";
            this.dateiToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.dateiToolStripMenuItem.Text = "Datei";
            // 
            // exportierenToolStripMenuItem
            // 
            this.exportierenToolStripMenuItem.Name = "exportierenToolStripMenuItem";
            this.exportierenToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.exportierenToolStripMenuItem.Text = "Exportieren";
            this.exportierenToolStripMenuItem.Click += new System.EventHandler(this.exportierenToolStripMenuItem_Click);
            // 
            // beendenToolStripMenuItem
            // 
            this.beendenToolStripMenuItem.Name = "beendenToolStripMenuItem";
            this.beendenToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.beendenToolStripMenuItem.Text = "Beenden";
            this.beendenToolStripMenuItem.Click += new System.EventHandler(this.beendenToolStripMenuItem_Click);
            // 
            // ansichtToolStripMenuItem
            // 
            this.ansichtToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.priceErklaerungsmodusAnzeigenToolStripMenuItem});
            this.ansichtToolStripMenuItem.Name = "ansichtToolStripMenuItem";
            this.ansichtToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.ansichtToolStripMenuItem.Text = "Ansicht";
            // 
            // priceErklaerungsmodusAnzeigenToolStripMenuItem
            // 
            this.priceErklaerungsmodusAnzeigenToolStripMenuItem.CheckOnClick = true;
            this.priceErklaerungsmodusAnzeigenToolStripMenuItem.Name = "priceErklaerungsmodusAnzeigenToolStripMenuItem";
            this.priceErklaerungsmodusAnzeigenToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.priceErklaerungsmodusAnzeigenToolStripMenuItem.Text = "Preis-Erklärmodus anzeigen";
            this.priceErklaerungsmodusAnzeigenToolStripMenuItem.Click += new System.EventHandler(this.priceErklaerungsmodusAnzeigenToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logsOffnenToolStripMenuItem,
            this.diagnoseToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // logsOffnenToolStripMenuItem
            // 
            this.logsOffnenToolStripMenuItem.Name = "logsOffnenToolStripMenuItem";
            this.logsOffnenToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.logsOffnenToolStripMenuItem.Text = "Logs öffnen";
            this.logsOffnenToolStripMenuItem.Click += new System.EventHandler(this.logsOffnenToolStripMenuItem_Click);
            // 
            // diagnoseToolStripMenuItem
            // 
            this.diagnoseToolStripMenuItem.Name = "diagnoseToolStripMenuItem";
            this.diagnoseToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.diagnoseToolStripMenuItem.Text = "Diagnose";
            this.diagnoseToolStripMenuItem.Click += new System.EventHandler(this.diagnoseToolStripMenuItem_Click);
            // 
            // dataGridViewInventory
            // 
            this.dataGridViewInventory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewInventory.Location = new System.Drawing.Point(12, 145);
            this.dataGridViewInventory.Name = "dataGridViewInventory";
            this.dataGridViewInventory.RowTemplate.Height = 25;
            this.dataGridViewInventory.Size = new System.Drawing.Size(1045, 350);
            this.dataGridViewInventory.TabIndex = 3;
            this.dataGridViewInventory.SelectionChanged += new System.EventHandler(this.dataGridViewInventory_SelectionChanged);
            // 
            // buttonCalculate
            // 
            this.buttonCalculate.Location = new System.Drawing.Point(400, 96);
            this.buttonCalculate.Name = "buttonCalculate";
            this.buttonCalculate.Size = new System.Drawing.Size(180, 32);
            this.buttonCalculate.TabIndex = 4;
            this.buttonCalculate.Text = "Calculate";
            this.buttonCalculate.UseVisualStyleBackColor = true;
            this.buttonCalculate.Click += new System.EventHandler(this.buttonCalculate_Click);
            // 
            // buttonExport
            // 
            this.buttonExport.Location = new System.Drawing.Point(620, 96);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(180, 32);
            this.buttonExport.TabIndex = 5;
            this.buttonExport.Text = "Export CSV";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Personality";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "Location";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Political";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(560, 41);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 15);
            this.label8.TabIndex = 20;
            this.label8.Text = "Faction";
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Location = new System.Drawing.Point(85, 520);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.PlaceholderText = "item name...";
            this.textBoxSearch.Size = new System.Drawing.Size(250, 23);
            this.textBoxSearch.TabIndex = 9;
            // 
            // comboBoxCategoryFilter
            // 
            this.comboBoxCategoryFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCategoryFilter.FormattingEnabled = true;
            this.comboBoxCategoryFilter.Location = new System.Drawing.Point(470, 520);
            this.comboBoxCategoryFilter.Name = "comboBoxCategoryFilter";
            this.comboBoxCategoryFilter.Size = new System.Drawing.Size(230, 23);
            this.comboBoxCategoryFilter.TabIndex = 10;
            // 
            // numericUpDownMinPrice
            // 
            this.numericUpDownMinPrice.DecimalPlaces = 2;
            this.numericUpDownMinPrice.Location = new System.Drawing.Point(780, 520);
            this.numericUpDownMinPrice.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDownMinPrice.Name = "numericUpDownMinPrice";
            this.numericUpDownMinPrice.Size = new System.Drawing.Size(90, 23);
            this.numericUpDownMinPrice.TabIndex = 11;
            // 
            // numericUpDownMaxPrice
            // 
            this.numericUpDownMaxPrice.DecimalPlaces = 2;
            this.numericUpDownMaxPrice.Location = new System.Drawing.Point(930, 520);
            this.numericUpDownMaxPrice.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDownMaxPrice.Name = "numericUpDownMaxPrice";
            this.numericUpDownMaxPrice.Size = new System.Drawing.Size(90, 23);
            this.numericUpDownMaxPrice.TabIndex = 12;
            // 
            // buttonApplyFilter
            // 
            this.buttonApplyFilter.Location = new System.Drawing.Point(1085, 516);
            this.buttonApplyFilter.Name = "buttonApplyFilter";
            this.buttonApplyFilter.Size = new System.Drawing.Size(90, 30);
            this.buttonApplyFilter.TabIndex = 13;
            this.buttonApplyFilter.Text = "Apply";
            this.buttonApplyFilter.UseVisualStyleBackColor = true;
            this.buttonApplyFilter.Click += new System.EventHandler(this.buttonApplyFilter_Click);
            // 
            // buttonResetFilter
            // 
            this.buttonResetFilter.Location = new System.Drawing.Point(1183, 516);
            this.buttonResetFilter.Name = "buttonResetFilter";
            this.buttonResetFilter.Size = new System.Drawing.Size(90, 30);
            this.buttonResetFilter.TabIndex = 14;
            this.buttonResetFilter.Text = "Reset";
            this.buttonResetFilter.UseVisualStyleBackColor = true;
            this.buttonResetFilter.Click += new System.EventHandler(this.buttonResetFilter_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 524);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 15);
            this.label4.TabIndex = 15;
            this.label4.Text = "Search Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(370, 524);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 15);
            this.label5.TabIndex = 16;
            this.label5.Text = "Category";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(740, 524);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 15);
            this.label6.TabIndex = 17;
            this.label6.Text = "Min Price";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(895, 524);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 15);
            this.label7.TabIndex = 18;
            this.label7.Text = "Max";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 678);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1500, 22);
            this.statusStrip1.TabIndex = 19;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // panelPriceExplanation
            // 
            this.panelPriceExplanation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelPriceExplanation.Controls.Add(this.textBoxPriceExplanation);
            this.panelPriceExplanation.Controls.Add(this.labelPriceExplanationTitle);
            this.panelPriceExplanation.Location = new System.Drawing.Point(1065, 145);
            this.panelPriceExplanation.Name = "panelPriceExplanation";
            this.panelPriceExplanation.Size = new System.Drawing.Size(420, 350);
            this.panelPriceExplanation.TabIndex = 31;
            // 
            // labelPriceExplanationTitle
            // 
            this.labelPriceExplanationTitle.AutoSize = true;
            this.labelPriceExplanationTitle.Location = new System.Drawing.Point(9, 10);
            this.labelPriceExplanationTitle.Name = "labelPriceExplanationTitle";
            this.labelPriceExplanationTitle.Size = new System.Drawing.Size(102, 15);
            this.labelPriceExplanationTitle.TabIndex = 0;
            this.labelPriceExplanationTitle.Text = "Price Explanation";
            // 
            // textBoxPriceExplanation
            // 
            this.textBoxPriceExplanation.Location = new System.Drawing.Point(9, 33);
            this.textBoxPriceExplanation.Multiline = true;
            this.textBoxPriceExplanation.Name = "textBoxPriceExplanation";
            this.textBoxPriceExplanation.ReadOnly = true;
            this.textBoxPriceExplanation.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxPriceExplanation.Size = new System.Drawing.Size(400, 305);
            this.textBoxPriceExplanation.TabIndex = 1;
            this.textBoxPriceExplanation.WordWrap = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1500, 700);
            this.Controls.Add(this.panelPriceExplanation);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonResetFilter);
            this.Controls.Add(this.buttonApplyFilter);
            this.Controls.Add(this.numericUpDownMaxPrice);
            this.Controls.Add(this.numericUpDownMinPrice);
            this.Controls.Add(this.comboBoxCategoryFilter);
            this.Controls.Add(this.textBoxSearch);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.buttonCalculate);
            this.Controls.Add(this.dataGridViewInventory);
            this.Controls.Add(this.comboBoxFaction);
            this.Controls.Add(this.comboBoxPolitical);
            this.Controls.Add(this.comboBoxLocation);
            this.Controls.Add(this.comboBoxPersonality);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Merchant Inventory Engine";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInventory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxPrice)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panelPriceExplanation.ResumeLayout(false);
            this.panelPriceExplanation.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ComboBox comboBoxPersonality;
        private System.Windows.Forms.ComboBox comboBoxLocation;
        private System.Windows.Forms.ComboBox comboBoxPolitical;
        private System.Windows.Forms.ComboBox comboBoxFaction;
        private System.Windows.Forms.DataGridView dataGridViewInventory;
        private System.Windows.Forms.Button buttonCalculate;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.ComboBox comboBoxCategoryFilter;
        private System.Windows.Forms.NumericUpDown numericUpDownMinPrice;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxPrice;
        private System.Windows.Forms.Button buttonApplyFilter;
        private System.Windows.Forms.Button buttonResetFilter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dateiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportierenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem beendenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ansichtToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem priceErklaerungsmodusAnzeigenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logsOffnenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem diagnoseToolStripMenuItem;
        private System.Windows.Forms.Panel panelPriceExplanation;
        private System.Windows.Forms.Label labelPriceExplanationTitle;
        private System.Windows.Forms.TextBox textBoxPriceExplanation;
    }
}
