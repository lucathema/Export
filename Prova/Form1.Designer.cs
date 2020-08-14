namespace Prova
{
    partial class Form1
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.ctlTab = new System.Windows.Forms.TabControl();
            this.tabFoto = new System.Windows.Forms.TabPage();
            this.groupSelezioneManualeFoto = new System.Windows.Forms.GroupBox();
            this.listaFoto = new System.Windows.Forms.CheckedListBox();
            this.boxCollezione = new System.Windows.Forms.ComboBox();
            this.boxRicercaFoto = new System.Windows.Forms.TextBox();
            this.lblCollezione = new System.Windows.Forms.Label();
            this.lblRicercaFoto = new System.Windows.Forms.Label();
            this.btnAggiornaFoto = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAggiornaTuttoFoto = new System.Windows.Forms.Button();
            this.tab3D = new System.Windows.Forms.TabPage();
            this.groupSelezioneManuale3D = new System.Windows.Forms.GroupBox();
            this.listaModelli3D = new System.Windows.Forms.CheckedListBox();
            this.boxLinea3D = new System.Windows.Forms.ComboBox();
            this.lblLinea3D = new System.Windows.Forms.Label();
            this.btnAggiorna3D = new System.Windows.Forms.Button();
            this.boxRicerca3D = new System.Windows.Forms.TextBox();
            this.lblRicerca3D = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAggiornaTutto3D = new System.Windows.Forms.Button();
            this.tabTavolette = new System.Windows.Forms.TabPage();
            this.groupSelezioneManualeTav = new System.Windows.Forms.GroupBox();
            this.listaImmagini = new System.Windows.Forms.CheckedListBox();
            this.boxRicerca = new System.Windows.Forms.TextBox();
            this.lblRicerca = new System.Windows.Forms.Label();
            this.btnAggiornaTav = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAggiornaTuttoTav = new System.Windows.Forms.Button();
            this.tabDocuments = new System.Windows.Forms.TabPage();
            this.btnAggiornaDoc = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.configurazioneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.apriSincronizzazionexmlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.apriFileDiConfigurazioneNLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.apriCartellaConIFileDiLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.apriFileDiLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblVersione = new System.Windows.Forms.Label();
            this.ctlTab.SuspendLayout();
            this.tabFoto.SuspendLayout();
            this.groupSelezioneManualeFoto.SuspendLayout();
            this.tab3D.SuspendLayout();
            this.groupSelezioneManuale3D.SuspendLayout();
            this.tabTavolette.SuspendLayout();
            this.groupSelezioneManualeTav.SuspendLayout();
            this.tabDocuments.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ctlTab
            // 
            this.ctlTab.Controls.Add(this.tabFoto);
            this.ctlTab.Controls.Add(this.tab3D);
            this.ctlTab.Controls.Add(this.tabTavolette);
            this.ctlTab.Controls.Add(this.tabDocuments);
            this.ctlTab.Location = new System.Drawing.Point(-4, 23);
            this.ctlTab.Name = "ctlTab";
            this.ctlTab.SelectedIndex = 0;
            this.ctlTab.Size = new System.Drawing.Size(718, 521);
            this.ctlTab.TabIndex = 0;
            // 
            // tabFoto
            // 
            this.tabFoto.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabFoto.Controls.Add(this.groupSelezioneManualeFoto);
            this.tabFoto.Controls.Add(this.label1);
            this.tabFoto.Controls.Add(this.btnAggiornaTuttoFoto);
            this.tabFoto.Location = new System.Drawing.Point(4, 22);
            this.tabFoto.Name = "tabFoto";
            this.tabFoto.Padding = new System.Windows.Forms.Padding(3);
            this.tabFoto.Size = new System.Drawing.Size(710, 495);
            this.tabFoto.TabIndex = 0;
            this.tabFoto.Text = "Foto Campionario";
            // 
            // groupSelezioneManualeFoto
            // 
            this.groupSelezioneManualeFoto.Controls.Add(this.listaFoto);
            this.groupSelezioneManualeFoto.Controls.Add(this.boxCollezione);
            this.groupSelezioneManualeFoto.Controls.Add(this.boxRicercaFoto);
            this.groupSelezioneManualeFoto.Controls.Add(this.lblCollezione);
            this.groupSelezioneManualeFoto.Controls.Add(this.lblRicercaFoto);
            this.groupSelezioneManualeFoto.Controls.Add(this.btnAggiornaFoto);
            this.groupSelezioneManualeFoto.Location = new System.Drawing.Point(48, 16);
            this.groupSelezioneManualeFoto.Name = "groupSelezioneManualeFoto";
            this.groupSelezioneManualeFoto.Size = new System.Drawing.Size(360, 446);
            this.groupSelezioneManualeFoto.TabIndex = 12;
            this.groupSelezioneManualeFoto.TabStop = false;
            this.groupSelezioneManualeFoto.Text = "Selezione Manuale";
            // 
            // listaFoto
            // 
            this.listaFoto.FormattingEnabled = true;
            this.listaFoto.Location = new System.Drawing.Point(64, 110);
            this.listaFoto.Name = "listaFoto";
            this.listaFoto.Size = new System.Drawing.Size(250, 244);
            this.listaFoto.TabIndex = 11;
            // 
            // boxCollezione
            // 
            this.boxCollezione.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.boxCollezione.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.boxCollezione.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.boxCollezione.ForeColor = System.Drawing.SystemColors.Desktop;
            this.boxCollezione.FormattingEnabled = true;
            this.boxCollezione.Location = new System.Drawing.Point(117, 48);
            this.boxCollezione.Name = "boxCollezione";
            this.boxCollezione.Size = new System.Drawing.Size(197, 21);
            this.boxCollezione.TabIndex = 0;
            this.boxCollezione.SelectedIndexChanged += new System.EventHandler(this.boxCollezione_SelectedIndexChanged);
            // 
            // boxRicercaFoto
            // 
            this.boxRicercaFoto.Location = new System.Drawing.Point(117, 84);
            this.boxRicercaFoto.Name = "boxRicercaFoto";
            this.boxRicercaFoto.Size = new System.Drawing.Size(197, 20);
            this.boxRicercaFoto.TabIndex = 10;
            this.boxRicercaFoto.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // lblCollezione
            // 
            this.lblCollezione.AutoSize = true;
            this.lblCollezione.Location = new System.Drawing.Point(12, 51);
            this.lblCollezione.Name = "lblCollezione";
            this.lblCollezione.Size = new System.Drawing.Size(99, 13);
            this.lblCollezione.TabIndex = 1;
            this.lblCollezione.Text = "Inserisci Collezione:";
            // 
            // lblRicercaFoto
            // 
            this.lblRicercaFoto.AutoSize = true;
            this.lblRicercaFoto.Location = new System.Drawing.Point(73, 87);
            this.lblRicercaFoto.Name = "lblRicercaFoto";
            this.lblRicercaFoto.Size = new System.Drawing.Size(38, 13);
            this.lblRicercaFoto.TabIndex = 9;
            this.lblRicercaFoto.Text = "Cerca:";
            // 
            // btnAggiornaFoto
            // 
            this.btnAggiornaFoto.Location = new System.Drawing.Point(115, 381);
            this.btnAggiornaFoto.Name = "btnAggiornaFoto";
            this.btnAggiornaFoto.Size = new System.Drawing.Size(145, 30);
            this.btnAggiornaFoto.TabIndex = 6;
            this.btnAggiornaFoto.Text = "Aggiorna Selezionati";
            this.btnAggiornaFoto.UseVisualStyleBackColor = true;
            this.btnAggiornaFoto.Click += new System.EventHandler(this.btnAggiorna_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(461, 182);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 43);
            this.label1.TabIndex = 8;
            this.label1.Text = "Utilizzando il pulsante \"Aggiorna Tutto\" verranno aggiornati tutti i file che son" +
    "o stati modificati nelle ultime 24 ore";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAggiornaTuttoFoto
            // 
            this.btnAggiornaTuttoFoto.Location = new System.Drawing.Point(491, 237);
            this.btnAggiornaTuttoFoto.Name = "btnAggiornaTuttoFoto";
            this.btnAggiornaTuttoFoto.Size = new System.Drawing.Size(132, 61);
            this.btnAggiornaTuttoFoto.TabIndex = 7;
            this.btnAggiornaTuttoFoto.Text = "Aggiorna Tutto";
            this.btnAggiornaTuttoFoto.UseVisualStyleBackColor = true;
            this.btnAggiornaTuttoFoto.Click += new System.EventHandler(this.button2_Click);
            // 
            // tab3D
            // 
            this.tab3D.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tab3D.Controls.Add(this.groupSelezioneManuale3D);
            this.tab3D.Controls.Add(this.label3);
            this.tab3D.Controls.Add(this.btnAggiornaTutto3D);
            this.tab3D.Location = new System.Drawing.Point(4, 22);
            this.tab3D.Name = "tab3D";
            this.tab3D.Padding = new System.Windows.Forms.Padding(3);
            this.tab3D.Size = new System.Drawing.Size(710, 495);
            this.tab3D.TabIndex = 2;
            this.tab3D.Text = "Modelli 3D";
            // 
            // groupSelezioneManuale3D
            // 
            this.groupSelezioneManuale3D.Controls.Add(this.listaModelli3D);
            this.groupSelezioneManuale3D.Controls.Add(this.boxLinea3D);
            this.groupSelezioneManuale3D.Controls.Add(this.lblLinea3D);
            this.groupSelezioneManuale3D.Controls.Add(this.btnAggiorna3D);
            this.groupSelezioneManuale3D.Controls.Add(this.boxRicerca3D);
            this.groupSelezioneManuale3D.Controls.Add(this.lblRicerca3D);
            this.groupSelezioneManuale3D.Location = new System.Drawing.Point(48, 16);
            this.groupSelezioneManuale3D.Name = "groupSelezioneManuale3D";
            this.groupSelezioneManuale3D.Size = new System.Drawing.Size(360, 446);
            this.groupSelezioneManuale3D.TabIndex = 12;
            this.groupSelezioneManuale3D.TabStop = false;
            this.groupSelezioneManuale3D.Text = "Selezione Manuale";
            // 
            // listaModelli3D
            // 
            this.listaModelli3D.FormattingEnabled = true;
            this.listaModelli3D.Location = new System.Drawing.Point(64, 110);
            this.listaModelli3D.Name = "listaModelli3D";
            this.listaModelli3D.Size = new System.Drawing.Size(250, 244);
            this.listaModelli3D.TabIndex = 8;
            // 
            // boxLinea3D
            // 
            this.boxLinea3D.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.boxLinea3D.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.boxLinea3D.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.boxLinea3D.FormattingEnabled = true;
            this.boxLinea3D.Location = new System.Drawing.Point(117, 48);
            this.boxLinea3D.Name = "boxLinea3D";
            this.boxLinea3D.Size = new System.Drawing.Size(197, 21);
            this.boxLinea3D.TabIndex = 0;
            this.boxLinea3D.SelectedIndexChanged += new System.EventHandler(this.boxLinea3D_SelectedIndexChanged);
            // 
            // lblLinea3D
            // 
            this.lblLinea3D.AutoSize = true;
            this.lblLinea3D.Location = new System.Drawing.Point(25, 51);
            this.lblLinea3D.Name = "lblLinea3D";
            this.lblLinea3D.Size = new System.Drawing.Size(77, 13);
            this.lblLinea3D.TabIndex = 3;
            this.lblLinea3D.Text = "Inserisci Linea:";
            // 
            // btnAggiorna3D
            // 
            this.btnAggiorna3D.Location = new System.Drawing.Point(115, 381);
            this.btnAggiorna3D.Name = "btnAggiorna3D";
            this.btnAggiorna3D.Size = new System.Drawing.Size(145, 30);
            this.btnAggiorna3D.TabIndex = 9;
            this.btnAggiorna3D.Text = "Aggiorna Selezionati";
            this.btnAggiorna3D.UseVisualStyleBackColor = true;
            this.btnAggiorna3D.Click += new System.EventHandler(this.btnAggiorna3D_Click);
            // 
            // boxRicerca3D
            // 
            this.boxRicerca3D.Location = new System.Drawing.Point(117, 84);
            this.boxRicerca3D.Name = "boxRicerca3D";
            this.boxRicerca3D.Size = new System.Drawing.Size(197, 20);
            this.boxRicerca3D.TabIndex = 6;
            this.boxRicerca3D.TextChanged += new System.EventHandler(this.boxRicerca3D_TextChanged);
            // 
            // lblRicerca3D
            // 
            this.lblRicerca3D.AutoSize = true;
            this.lblRicerca3D.Location = new System.Drawing.Point(73, 87);
            this.lblRicerca3D.Name = "lblRicerca3D";
            this.lblRicerca3D.Size = new System.Drawing.Size(38, 13);
            this.lblRicerca3D.TabIndex = 7;
            this.lblRicerca3D.Text = "Cerca:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(461, 182);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(200, 43);
            this.label3.TabIndex = 11;
            this.label3.Text = "Utilizzando il pulsante \"Aggiorna Tutto\" verranno aggiornati tutti i file che son" +
    "o stati modificati nelle ultime 24 ore";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAggiornaTutto3D
            // 
            this.btnAggiornaTutto3D.Location = new System.Drawing.Point(491, 237);
            this.btnAggiornaTutto3D.Name = "btnAggiornaTutto3D";
            this.btnAggiornaTutto3D.Size = new System.Drawing.Size(132, 61);
            this.btnAggiornaTutto3D.TabIndex = 10;
            this.btnAggiornaTutto3D.Text = "Aggiorna Tutto";
            this.btnAggiornaTutto3D.UseVisualStyleBackColor = true;
            this.btnAggiornaTutto3D.Click += new System.EventHandler(this.btnAggiornaTutto3D_Click);
            // 
            // tabTavolette
            // 
            this.tabTavolette.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabTavolette.Controls.Add(this.groupSelezioneManualeTav);
            this.tabTavolette.Controls.Add(this.label2);
            this.tabTavolette.Controls.Add(this.btnAggiornaTuttoTav);
            this.tabTavolette.Location = new System.Drawing.Point(4, 22);
            this.tabTavolette.Name = "tabTavolette";
            this.tabTavolette.Padding = new System.Windows.Forms.Padding(3);
            this.tabTavolette.Size = new System.Drawing.Size(710, 495);
            this.tabTavolette.TabIndex = 1;
            this.tabTavolette.Text = "Tavolette";
            // 
            // groupSelezioneManualeTav
            // 
            this.groupSelezioneManualeTav.Controls.Add(this.listaImmagini);
            this.groupSelezioneManualeTav.Controls.Add(this.boxRicerca);
            this.groupSelezioneManualeTav.Controls.Add(this.lblRicerca);
            this.groupSelezioneManualeTav.Controls.Add(this.btnAggiornaTav);
            this.groupSelezioneManualeTav.Location = new System.Drawing.Point(48, 16);
            this.groupSelezioneManualeTav.Name = "groupSelezioneManualeTav";
            this.groupSelezioneManualeTav.Size = new System.Drawing.Size(360, 446);
            this.groupSelezioneManualeTav.TabIndex = 6;
            this.groupSelezioneManualeTav.TabStop = false;
            this.groupSelezioneManualeTav.Text = "Selezione Manuale";
            // 
            // listaImmagini
            // 
            this.listaImmagini.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.listaImmagini.FormattingEnabled = true;
            this.listaImmagini.Location = new System.Drawing.Point(64, 110);
            this.listaImmagini.Name = "listaImmagini";
            this.listaImmagini.Size = new System.Drawing.Size(250, 244);
            this.listaImmagini.TabIndex = 0;
            // 
            // boxRicerca
            // 
            this.boxRicerca.Location = new System.Drawing.Point(117, 84);
            this.boxRicerca.Name = "boxRicerca";
            this.boxRicerca.Size = new System.Drawing.Size(197, 20);
            this.boxRicerca.TabIndex = 1;
            this.boxRicerca.TextChanged += new System.EventHandler(this.boxRicerca_TextChanged);
            // 
            // lblRicerca
            // 
            this.lblRicerca.AutoSize = true;
            this.lblRicerca.Location = new System.Drawing.Point(73, 87);
            this.lblRicerca.Name = "lblRicerca";
            this.lblRicerca.Size = new System.Drawing.Size(38, 13);
            this.lblRicerca.TabIndex = 2;
            this.lblRicerca.Text = "Cerca:";
            // 
            // btnAggiornaTav
            // 
            this.btnAggiornaTav.Location = new System.Drawing.Point(115, 381);
            this.btnAggiornaTav.Name = "btnAggiornaTav";
            this.btnAggiornaTav.Size = new System.Drawing.Size(145, 30);
            this.btnAggiornaTav.TabIndex = 3;
            this.btnAggiornaTav.Text = "Aggiorna selezionati";
            this.btnAggiornaTav.UseVisualStyleBackColor = true;
            this.btnAggiornaTav.Click += new System.EventHandler(this.btnAggiornaTav_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(461, 182);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(200, 43);
            this.label2.TabIndex = 5;
            this.label2.Text = "Utilizzando il pulsante \"Aggiorna Tutto\" verranno aggiornati tutti i file che son" +
    "o stati modificati nelle ultime 24 ore";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAggiornaTuttoTav
            // 
            this.btnAggiornaTuttoTav.Location = new System.Drawing.Point(491, 237);
            this.btnAggiornaTuttoTav.Name = "btnAggiornaTuttoTav";
            this.btnAggiornaTuttoTav.Size = new System.Drawing.Size(132, 61);
            this.btnAggiornaTuttoTav.TabIndex = 4;
            this.btnAggiornaTuttoTav.Text = "Aggiorna Tutto";
            this.btnAggiornaTuttoTav.UseVisualStyleBackColor = true;
            this.btnAggiornaTuttoTav.Click += new System.EventHandler(this.btnAggiornaTuttoTav_Click);
            // 
            // tabDocuments
            // 
            this.tabDocuments.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabDocuments.Controls.Add(this.btnAggiornaDoc);
            this.tabDocuments.Location = new System.Drawing.Point(4, 22);
            this.tabDocuments.Name = "tabDocuments";
            this.tabDocuments.Padding = new System.Windows.Forms.Padding(3);
            this.tabDocuments.Size = new System.Drawing.Size(710, 495);
            this.tabDocuments.TabIndex = 3;
            this.tabDocuments.Text = "Conformity Documents";
            // 
            // btnAggiornaDoc
            // 
            this.btnAggiornaDoc.Location = new System.Drawing.Point(279, 194);
            this.btnAggiornaDoc.Name = "btnAggiornaDoc";
            this.btnAggiornaDoc.Size = new System.Drawing.Size(153, 106);
            this.btnAggiornaDoc.TabIndex = 0;
            this.btnAggiornaDoc.Text = "Aggiorna Conformity Documents";
            this.btnAggiornaDoc.UseVisualStyleBackColor = true;
            this.btnAggiornaDoc.Click += new System.EventHandler(this.btnAggiornaDoc_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configurazioneToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(706, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // configurazioneToolStripMenuItem
            // 
            this.configurazioneToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.apriSincronizzazionexmlToolStripMenuItem,
            this.apriFileDiConfigurazioneNLogToolStripMenuItem,
            this.apriCartellaConIFileDiLogToolStripMenuItem,
            this.apriFileDiLogToolStripMenuItem});
            this.configurazioneToolStripMenuItem.Name = "configurazioneToolStripMenuItem";
            this.configurazioneToolStripMenuItem.Size = new System.Drawing.Size(100, 20);
            this.configurazioneToolStripMenuItem.Text = "Configurazione";
            // 
            // apriSincronizzazionexmlToolStripMenuItem
            // 
            this.apriSincronizzazionexmlToolStripMenuItem.Name = "apriSincronizzazionexmlToolStripMenuItem";
            this.apriSincronizzazionexmlToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.apriSincronizzazionexmlToolStripMenuItem.Text = "Apri File Sincronizzazione.xml";
            this.apriSincronizzazionexmlToolStripMenuItem.Click += new System.EventHandler(this.ApriSincronizzazioneXML_Clicked);
            // 
            // apriFileDiConfigurazioneNLogToolStripMenuItem
            // 
            this.apriFileDiConfigurazioneNLogToolStripMenuItem.Name = "apriFileDiConfigurazioneNLogToolStripMenuItem";
            this.apriFileDiConfigurazioneNLogToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.apriFileDiConfigurazioneNLogToolStripMenuItem.Text = "Apri File di configurazione NLog";
            this.apriFileDiConfigurazioneNLogToolStripMenuItem.Click += new System.EventHandler(this.ApriConfigNlog_Clicked);
            // 
            // apriCartellaConIFileDiLogToolStripMenuItem
            // 
            this.apriCartellaConIFileDiLogToolStripMenuItem.Name = "apriCartellaConIFileDiLogToolStripMenuItem";
            this.apriCartellaConIFileDiLogToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.apriCartellaConIFileDiLogToolStripMenuItem.Text = "Apri Cartella con i File di Log";
            this.apriCartellaConIFileDiLogToolStripMenuItem.Click += new System.EventHandler(this.ApriCartellaFileDiLog_Clicked);
            // 
            // apriFileDiLogToolStripMenuItem
            // 
            this.apriFileDiLogToolStripMenuItem.Name = "apriFileDiLogToolStripMenuItem";
            this.apriFileDiLogToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.apriFileDiLogToolStripMenuItem.Text = "Apri File di Log";
            this.apriFileDiLogToolStripMenuItem.Click += new System.EventHandler(this.ApriFileDiLog_Clicked);
            // 
            // lblVersione
            // 
            this.lblVersione.AutoSize = true;
            this.lblVersione.Location = new System.Drawing.Point(500, 6);
            this.lblVersione.Name = "lblVersione";
            this.lblVersione.Size = new System.Drawing.Size(0, 13);
            this.lblVersione.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(706, 536);
            this.Controls.Add(this.lblVersione);
            this.Controls.Add(this.ctlTab);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sincronizzazione";
            this.ctlTab.ResumeLayout(false);
            this.tabFoto.ResumeLayout(false);
            this.groupSelezioneManualeFoto.ResumeLayout(false);
            this.groupSelezioneManualeFoto.PerformLayout();
            this.tab3D.ResumeLayout(false);
            this.groupSelezioneManuale3D.ResumeLayout(false);
            this.groupSelezioneManuale3D.PerformLayout();
            this.tabTavolette.ResumeLayout(false);
            this.groupSelezioneManualeTav.ResumeLayout(false);
            this.groupSelezioneManualeTav.PerformLayout();
            this.tabDocuments.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl ctlTab;
        private System.Windows.Forms.TabPage tabFoto;
        private System.Windows.Forms.Label lblCollezione;
        private System.Windows.Forms.ComboBox boxCollezione;
        private System.Windows.Forms.TabPage tabTavolette;
        private System.Windows.Forms.TabPage tab3D;
        private System.Windows.Forms.TabPage tabDocuments;
        private System.Windows.Forms.Button btnAggiornaTuttoFoto;
        private System.Windows.Forms.Button btnAggiornaFoto;
        private System.Windows.Forms.CheckedListBox listaImmagini;
        private System.Windows.Forms.Label lblRicerca;
        private System.Windows.Forms.TextBox boxRicerca;
        private System.Windows.Forms.Button btnAggiornaTav;
        private System.Windows.Forms.Button btnAggiornaTuttoTav;
        private System.Windows.Forms.Label lblLinea3D;
        private System.Windows.Forms.ComboBox boxLinea3D;
        private System.Windows.Forms.Label lblRicerca3D;
        private System.Windows.Forms.TextBox boxRicerca3D;
        private System.Windows.Forms.Button btnAggiornaTutto3D;
        private System.Windows.Forms.Button btnAggiorna3D;
        private System.Windows.Forms.CheckedListBox listaModelli3D;
        private System.Windows.Forms.Button btnAggiornaDoc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckedListBox listaFoto;
        private System.Windows.Forms.TextBox boxRicercaFoto;
        private System.Windows.Forms.Label lblRicercaFoto;
        private System.Windows.Forms.GroupBox groupSelezioneManualeFoto;
        private System.Windows.Forms.GroupBox groupSelezioneManuale3D;
        private System.Windows.Forms.GroupBox groupSelezioneManualeTav;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem configurazioneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem apriSincronizzazionexmlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem apriFileDiConfigurazioneNLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem apriFileDiLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem apriCartellaConIFileDiLogToolStripMenuItem;
        private System.Windows.Forms.Label lblVersione;
    }
}

