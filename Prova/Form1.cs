using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace Prova
{
    public partial class Form1 : Form
    {

        /*
         * FOTO_CAMPIONARIO:
         * Data una collezione (es. 99JST) ed un modello, nella cartella /mnt/FOTO_CAMPIONARIO/collezione/modello abbiamo diversi file .jpg.
         * Oltre a questi, per alcuni modelli (es. JSI-001 in 99JST), abbiamo altre cartelle (compresse e non) che contengono altri file (jpg o png).
         * Su suggerimento dell'ufficio marketing, questo programma sincronizza solamente i file .jpg contenuti in /mnt/FOTO_CAMPIONARIO/collezione/modello
         * e non altri eventuali file contenuti in sottocartelle.
         * Nel caso in cui i file jpg siano contenuti in /mnt/FOTO_CAMPIONARIO/collezione  [es. /mnt/FOTO_CAMPIONARIO/TAVOLETTE/], il programma funziona correttamente.
         */

        private SshConnection _sshConnection; // Usato per eseguire comandi via SSH
        private CommandLog commandLog;

        private List<string> _lsCollezione; // Variabile di classe perché viene usata anche per l'implementazione del filtro (Tab Foto)
        private List<string> _lsTavolette;  // Variabile di classe perché viene usata anche per l'implementazione del filtro (Tab Tavolette)
        private List<string> _ls3D;         // Variabile di classe perché viene usata anche per l'implementazione del filtro (Tab Modelli3D)

        private List<ElementToSync> _toSync; // Elementi presi dal file di configurazione XML
        private List<string> _lsXml;         // Variabile di classe perché viene usata anche per l'implementazione del filtro (Tab generato dal file di configurazione XML)
        private CheckedListBox listaXml;     // CheckedListBox del Tab generato dal file di configurazione XML

        private const int MaxFoto = 12;      // Ogni script foto.sh può contenere al massimo MaxFoto foto da aggiornare
        private const int MaxTavolette = 20; // Ogni script foto_tavolette_acetato.sh può contenere al massimo MaxTavolette tavolette da aggiornare

        public Form1()
        {
            InitializeComponent();

            lblVersione.Text = "Versione: " + Assembly.GetExecutingAssembly().GetName().Version.ToString();

            ReadXml(); // Leggi file di configurazione XML, genera di conseguenza i Tab necessari, che saranno poi riempiti a runtime.

            ctlTab.SelectedIndexChanged += new EventHandler(ctlTab_SelectedIndexChanged); // Handler evento cambio TAB

            // INIT Connessione SSH. Se fallisce, avvisa e chiudi l'applicazione.
            try
            {
                _sshConnection = new SshConnection("192.168.0.215", "worker", "ThemaADM01"); // Init connessione ssh
            } catch(Exception ex)
            {
                MessageBox.Show("Errore connessione via SSH. Errore:\n" + ex + "\n\tChiusura in corso...");
                Environment.Exit(1);
            }

            // Modifica valore attributo per fare in modo che si possa checkare un elemento con un solo click (non due).
            listaFoto.CheckOnClick = true;
            listaImmagini.CheckOnClick = true;
            listaModelli3D.CheckOnClick = true;

            // Disabilito button di Aggiornamento e chiamo la funzione per la prima inizializzazione (parto con il tab Foto)
            btnAggiornaFoto.Enabled = false;
            PopulateCollection();
        }

        private void ctlTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Inizializzo tutti i box di ricerca per non incorrere in eventuali problemi
            boxRicerca.Clear();
            boxRicercaFoto.Clear();
            boxRicerca3D.Clear();

            if (ctlTab.SelectedTab == ctlTab.TabPages[0]) // TAB FOTO_CAMPIONARIO
            {
                if (_lsCollezione != null)
                    _lsCollezione.Clear();
                PopulateCollection(); // Popolo il Drop Down Menu "Collezioni"
                // Inizializzo il comboBox e disabilito il button "Aggiorna Selezionati"
                boxCollezione.ResetText();
                boxCollezione.SelectedIndex = -1;
                btnAggiornaFoto.Enabled = false;
            }
            else if (ctlTab.SelectedTab == ctlTab.TabPages[1]) // TAB MODELLI 3D
            {
                listaModelli3D.Items.Clear();
                if (_ls3D != null)
                    _ls3D.Clear();
                PopulateLinea3D();
                // Inizializzo il comboBox
                boxLinea3D.ResetText();
                boxLinea3D.SelectedIndex = -1;
                // Disabilito temporaneamente il button "Aggiorna"
                btnAggiorna3D.Enabled = false;
            }
            else if (ctlTab.SelectedTab == ctlTab.TabPages[2]) // TAB TAVOLETTE
            {
                PopulateTavolette();
            } else if (ctlTab.SelectedTab == ctlTab.TabPages[3]) // TAB CONFORMITY DOCUMENTS
            {
                
            } else // TAB GENERATI DAL FILE XML
            {
                TabPage selectedTab = ctlTab.SelectedTab;
                selectedTab.Controls.Clear(); // Quando viene selezionato un Tab generato dal file XML, elimino tutto ciò che c'è dentro
                ManageXmlTab(selectedTab);    // A questo punto posso metterci gli elementi che mi interessano
            }
        }

        private void ManageXmlTab(TabPage selectedTab)
        {
            ElementToSync el = _toSync.Find(x => x.Name == selectedTab.Text); // Get elemento da sincronizzare che corrisponde alla tab selezionata
            if (el != null)
            {
                if (el.CommandLineArguments) // Di default è settato a false.
                {
                    // L'utente deve avere la possibilità di scegliere cosa aggiornare
                    Button selezionaCartella = new Button();
                    selezionaCartella.Text = "Seleziona cartella";
                    selezionaCartella.Width = 153;
                    selezionaCartella.Height = 106;
                    selezionaCartella.Location = new System.Drawing.Point(279, 194);
                    selezionaCartella.UseVisualStyleBackColor = true;
                    selezionaCartella.Click += new EventHandler(SelezionaCartella_Clicked);

                    selectedTab.Controls.Add(selezionaCartella);

                }
                else
                {
                    // L'utente ha a disposizione un solo Button "Aggiorna Tutto" che semplicemente esegue el.Script
                    Button btnAggiornaTuttoXml = new Button();
                    btnAggiornaTuttoXml.Text = "Aggiorna Tutto";
                    btnAggiornaTuttoXml.Width = 153; 
                    btnAggiornaTuttoXml.Height = 106; 
                    btnAggiornaTuttoXml.Location = new System.Drawing.Point(279, 194);
                    btnAggiornaTuttoXml.UseVisualStyleBackColor = true;
                    btnAggiornaTuttoXml.Click += new EventHandler(BtnAggiornaTuttoXml_Clicked);

                    selectedTab.Controls.Add(btnAggiornaTuttoXml);
                }
            }
        }

        private void SelezionaCartella_Clicked(object sender, EventArgs e)
        {
            ElementToSync el = _toSync.Find(x => x.Name == ctlTab.SelectedTab.Text); // Get elemento da sincronizzare che corrisponde alla tab selezionata

            string initialDirectory = @"\\xnfs\thema\" + el.Source.Replace("/mnt/", "").Replace("/", "\\"); // Setto directory iniziale Windows
            string path = AskDirectory(el, initialDirectory); // Genero finestra di scelta cartella e ritorno il path scelto dall'utente
            bool isParent = IsDirectoryParent(initialDirectory, path); // Controllo che il path sia valido (deve essere una subdir di initialDirectory o initialDirectory stessa)

            ctlTab.SelectedTab.Controls.Clear();

            if (!isParent) // Se il path non è valido, mostra il messaggio di errore ed esci
            {
                MessageBox.Show("La cartella selezionata non è valida.\nUsare una sottocartella della cartella " + initialDirectory + " o la cartella stessa.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ManageXmlTab(ctlTab.SelectedTab);
            }
            else
            {
                // Genera a runtime gli elementi che permetteranno all'utente di selezionare i file ed aggiornarli/selezionare un'altra cartella

                // Label descrizione
                Label lblSelezionaFile = new Label();
                lblSelezionaFile.Text = "Seleziona i file che vuoi aggiornare:";
                lblSelezionaFile.Width = 200;
                lblSelezionaFile.Location = new System.Drawing.Point(260, 60);
                ctlTab.SelectedTab.Controls.Add(lblSelezionaFile);

                // Label "Cerca"
                Label lblCercaXml = new Label();
                lblCercaXml.Text = "Cerca:";
                lblCercaXml.Width = 40;
                lblCercaXml.Location = new System.Drawing.Point(237, 85);
                ctlTab.SelectedTab.Controls.Add(lblCercaXml);

                // Textbox di ricerca per l'implementazione del filtro
                TextBox boxRicercaXml = new TextBox();
                boxRicercaXml.Width = 193;
                boxRicercaXml.Location = new System.Drawing.Point(290, 83);
                ctlTab.SelectedTab.Controls.Add(boxRicercaXml);
                boxRicercaXml.TextChanged += new EventHandler(BoxRicercaXml_TextChanged);

                // Lista di tutti gli elementi presenti nella cartella. E' possibile checkarli
                listaXml = new CheckedListBox();
                listaXml.Width = 253;
                listaXml.Height = 244;
                listaXml.Location = new System.Drawing.Point(230, 110);
                listaXml.CheckOnClick = true;
                ctlTab.SelectedTab.Controls.Add(listaXml);

                // Button per aggiornare i file selezionati
                Button btnAggiornaXml = new Button();
                btnAggiornaXml.Text = "Aggiorna Selezionati";
                btnAggiornaXml.Width = 153;
                btnAggiornaXml.Height = 25;
                btnAggiornaXml.Location = new System.Drawing.Point(279, 385);
                btnAggiornaXml.UseVisualStyleBackColor = true;
                btnAggiornaXml.Click += new EventHandler(BtnAggiornaXml_Clicked);
                ctlTab.SelectedTab.Controls.Add(btnAggiornaXml);

                // Button per selezionare un'altra cartella
                Button btnSelezionaAltraCartella = new Button();
                btnSelezionaAltraCartella.Text = "Seleziona Altra Cartella";
                btnSelezionaAltraCartella.Width = 153;
                btnSelezionaAltraCartella.Height = 25;
                btnSelezionaAltraCartella.Location = new System.Drawing.Point(279, 420);
                btnSelezionaAltraCartella.UseVisualStyleBackColor = true;
                btnSelezionaAltraCartella.Click += new EventHandler(BtnSelezionaAltraCartella_Clicked);
                ctlTab.SelectedTab.Controls.Add(btnSelezionaAltraCartella);

                // Codice per riempire la lista dei file presenti nella cartella scelta
                DirectoryInfo d = new DirectoryInfo(path);
                string toSearch = "*";  // Cerco qualsiasi file

                FileInfo[] files = null;
                try
                {
                    if (el.SearchRecursively)
                    {
                        files = d.GetFiles(toSearch, SearchOption.AllDirectories);
                    }
                    else
                    {
                        files = d.GetFiles(toSearch);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Non è stato possibile elencare i file nella cartella selezionata.\nErrore: " + ex, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctlTab.SelectedTab.Controls.Clear();
                    ManageXmlTab(ctlTab.SelectedTab);
                    return;
                }

                if (files != null)
                {
                    _lsXml = new List<string>();
                    var fInOrder = files.OrderBy(x => x.Name); // Ordino i file per nome

                    foreach (FileInfo f in fInOrder)
                    {
                        if (el.NotEndsWith.Count > 0)
                        {
                            if (el.EndsWith.Count() > 0)
                            {
                                if (el.EndsWith.Any(s => f.Name.EndsWith(s)) && !el.NotEndsWith.Any(s => f.Name.EndsWith(s))) // Il filtraggio offerto da Windows non funziona bene in certi casi, me ne occupo io
                                {
                                    listaXml.Items.Add(f.Name);
                                    _lsXml.Add(f.Name);
                                }
                            }
                            else
                            {
                                if (!el.NotEndsWith.Any(s => f.Name.EndsWith(s)))
                                {
                                    listaXml.Items.Add(f.Name);
                                    _lsXml.Add(f.Name);
                                }
                            }
                        }
                        else
                        {
                            if (el.EndsWith.Count() > 0)
                            {
                                if (el.EndsWith.Any(s => f.Name.EndsWith(s))) // Il filtraggio offerto da Windows non funziona bene in certi casi, me ne occupo io
                                {
                                    listaXml.Items.Add(f.Name);
                                    _lsXml.Add(f.Name);
                                }
                            }
                            else
                            {
                                listaXml.Items.Add(f.Name);
                                _lsXml.Add(f.Name);
                            }
                        }
                    }
                }
            }
        }

        // Funzione per leggere il file di configurazione XML.

        /* FILE XML:
         * inizia con <toupdate>
         * All'interno di toupdate è possibile inserire qualsiasi script. es:
         * <Modelli3D>
         * All'interno di Modelli3D devo specificare:
         * - <script> : script usato [STRINGA]
         * - <commandlinearguments> : lo script va eseguito con o senza argomenti? [BOOLEANO]
         * Se lo script va eseguito con argomenti, bisogna aggiungere:
         * - <source> : dove vado a prendere i file che dovranno essere selezionati dall'utente? [STRINGA]
         * E' possibile inserire altri parametri, quali:
         * - <endswith> : come deve finire il nome del file (es. <endswith> <item>C.obj</item> </endswith>) [LISTA DI STRINGHE]
         * - <notendswith> : come non può finire il nome del file (es. <notendswith> <item>a.jpg</item><item>b.jpg</item> </notendswith> [LISTA DI STRINGHE]
         * - <searchrecursively> : i file devono essere ricercati anche nelle sottocartelle? [BOOLEANO]
         * 
         */
        private void ReadXml()
        {
            _toSync = new List<ElementToSync>();

            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                //xmlDoc.Load("sincro.xml");
                
                xmlDoc.Load(Path.GetDirectoryName(Application.ExecutablePath) + "\\sincro.xml");
            } catch (Exception ex)
            {
                MessageBox.Show("Errore apertura file di configurazione XML:\n" + ex);
                Environment.Exit(5);
            }

            foreach (XmlNode i in xmlDoc.ChildNodes)
            {
                if (i.LocalName == "toupdate")
                {
                    foreach (XmlNode item in i.ChildNodes)
                    {
                        string name = item.LocalName, script = "", source = "";
                        bool commandLineArguments = false, searchRecursively = false;
                        List<string> notEndsWith = new List<string>();
                        List<string> endsWith = new List<string>();
                        
                        foreach (XmlNode child in item.ChildNodes)
                        {
                            if (child.LocalName == "script")
                                script = child.FirstChild.Value;
                            else if (child.LocalName == "commandlinearguments")
                                commandLineArguments = Convert.ToBoolean(child.FirstChild.Value);
                            else if (child.LocalName == "source")
                                source = child.FirstChild.Value;
                            else if (child.LocalName == "searchrecursively")
                                searchRecursively = Convert.ToBoolean(child.FirstChild.Value);
                            else if (child.LocalName == "endswith")
                            {
                                foreach (XmlNode endsChild in child.ChildNodes)
                                {
                                    endsWith.Add(endsChild.FirstChild.Value);
                                }
                            }
                            else if (child.LocalName == "notendswith")
                            {
                                foreach (XmlNode notEndsChild in child.ChildNodes)
                                {
                                    notEndsWith.Add(notEndsChild.FirstChild.Value);
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(script))
                        {
                            MessageBox.Show("Configurazione XML di " + name + ": è necessario inserire l'elemento <script>", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        } else {
                            ElementToSync el = new ElementToSync(name, script, commandLineArguments, source, searchRecursively, endsWith, notEndsWith);
                            _toSync.Add(el);
                        }
                    }
                }
            }

            // Creo un nuovo TAB per ogni elemento trovato nel file di configurazione
            foreach (ElementToSync el in _toSync)
            {
                TabPage newTab = new TabPage(el.Name);
                newTab.BackColor = System.Drawing.Color.WhiteSmoke;
                ctlTab.TabPages.Add(newTab);
            }
        }

        // Button Aggiorna Tutto -> Tab creato dal file di configurazione XML
        private void BtnAggiornaTuttoXml_Clicked (object sender, EventArgs e)
        {
            ElementToSync el = _toSync.Find(x => x.Name == ctlTab.SelectedTab.Text); // Get elemento da sincronizzare che corrisponde alla tab selezionata
            commandLog = new CommandLog(); // Creo finestra che mi mostrerà lo stato di avanzamento del comando
            _sshConnection.ExecuteCommand(el.Script, commandLog); // Eseguo comando
            commandLog.Show();
        }

        // Button Aggiorna Selezionati -> Tab creato dal file di configurazione XML
        private void BtnAggiornaXml_Clicked (object sender, EventArgs e)
        {
            ElementToSync el = _toSync.Find(x => x.Name == ctlTab.SelectedTab.Text); // Get elemento da sincronizzare che corrisponde alla tab selezionata
            List<string> selected = listaXml.CheckedItems.Cast<string>().ToList(); // Get lista dei file selezionati

            if (el != null)
            {
                if (selected.Count() > 0)
                {
                    string command = el.Script + " ";
                    foreach (string name in selected)
                    {
                        command += name + " ";
                    }
                    commandLog = new CommandLog();
                    _sshConnection.ExecuteCommand(command, commandLog);
                    commandLog.Show();

                    ctlTab.SelectedTab.Controls.Clear();
                    ManageXmlTab(ctlTab.SelectedTab);
                }
                else
                {
                    MessageBox.Show("Non è stato selezionato alcun elemento", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        // Button Seleziona altra cartella -> Tab generato dal file di configurazione XML
        private void BtnSelezionaAltraCartella_Clicked(object sender, EventArgs e)
        {
            ctlTab.SelectedTab.Controls.Clear(); // Elimino tutto ciò che ho creato nel TAB
            SelezionaCartella_Clicked(null, e); // Simulo click Button "Seleziona Cartella"
        }

        // Implementazione Filtro mediante TextBox -> Tab generato dal file di configurazione XML
        private void BoxRicercaXml_TextChanged(object sender, EventArgs e)
        {
            if (_lsXml != null)
            {
                // Filtro listaImmagini in base alla ricerca fatta dall'utente, mantenendo i valori checked.
                listaXml.BeginUpdate();

                List<string> selected = listaXml.CheckedItems.Cast<string>().ToList(); // Gli item selezionati vanno riaggiunti

                listaXml.Items.Clear();

                foreach (string name in _lsXml)
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        if (selected.Contains(name))
                        {
                            listaXml.Items.Add(name, true);
                        }
                        else
                        {
                            if (name.ToLower().Contains(((TextBox)sender).Text.ToLower()))
                            {
                                listaXml.Items.Add(name);
                            }
                        }
                    }
                }

                listaXml.EndUpdate();
            }

        }


        // Popolo il box "Collezioni" -> TAB Foto
        private void PopulateCollection()
        {
            listaFoto.Items.Clear();
            List<string> lsFotoCampionario = null;
            try
            {
                lsFotoCampionario = _sshConnection.GetLsResult("/mnt/FOTO_CAMPIONARIO");
            } catch (Exception ex)
            {
                // In caso di errore SSH, chiudo l'applicazione: la connessione via SSH è vitale
                MessageBox.Show("Errore durante l'esecuzione del comando SSH:\n" + ex + "\nChiusura in corso...", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(2);
            }

            if (lsFotoCampionario != null)
            {
                foreach (string name in lsFotoCampionario)
                {
                    if (name != "thumbnail" && name != "Thumbs.db" && name != "")
                    {
                        boxCollezione.Items.Add(name);
                    }
                }
            }
        }

        // Popolo la lista delle immagini -> TAB Tavolette
        private void PopulateTavolette()
        {
            listaImmagini.Items.Clear();
            try
            {
                _lsTavolette = _sshConnection.GetLsResult("/mnt/UFFICIO_MARKETING/18_Tavolette/colori_acetato_sito");
            } catch (Exception ex)
            {
                // In caso di errore SSH, chiudo l'applicazione: la connessione via SSH è vitale
                MessageBox.Show("Errore durante l'esecuzione del comando SSH:\n" + ex, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(2);
            }

            if (_lsTavolette != null)
            {
                foreach (string name in _lsTavolette)
                {
                    if (Path.GetExtension(name) == ".jpg" && !(name.EndsWith("_b.jpg"))) // Non mostro i file _b.jpg: lo script si occupa di loro
                    {
                        listaImmagini.Items.Add(name.Substring(0, name.Count() - 4));
                    }
                }
            }
        }

        // Popolo il box "Linee" -> Tab Modelli3D
        private void PopulateLinea3D()
        {
            List<string> lsLinea3D = null;
            boxLinea3D.Items.Clear();
            try
            {
                lsLinea3D = _sshConnection.GetLsResult("/mnt/MODELLI_3D");
            }
            catch (Exception ex)
            {
                // In caso di errore SSH, chiudo l'applicazione: la connessione via SSH è vitale
                MessageBox.Show("Errore durante l'esecuzione del comando SSH:\n" + ex + "\nChiusura in corso...", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(2);
            }

            if (lsLinea3D != null)
            {
                foreach (string name in lsLinea3D)
                {
                    if (name != "")
                    {
                        boxLinea3D.Items.Add(name);
                    }
                }
            }
        }


        // Button Aggiorna Tutto -> TAB Foto
        private void button2_Click(object sender, EventArgs e)
        {
            commandLog = new CommandLog(); // Creo nuova finestra in cui mostrare lo stato di avanzamento del comando.
            _sshConnection.ExecuteCommand("./foto.sh", commandLog); // Eseguo comando
            commandLog.Show();
        }


        // Al cambio della collezione scelta, svuoto la lista precedentemente riempita e la popolo con i nuovi file
        private void boxCollezione_SelectedIndexChanged(object sender, EventArgs e)
        {
            boxRicercaFoto.Clear();
            listaFoto.Items.Clear();

            if (_lsCollezione != null)
            {
                _lsCollezione.Clear();
            }

            string collezioneScelta = (string)boxCollezione.SelectedItem; // Get collezione scelta 
            if (!string.IsNullOrEmpty(collezioneScelta))
            {
                try
                {
                    _lsCollezione = _sshConnection.GetLsResult("/mnt/FOTO_CAMPIONARIO/" + collezioneScelta); // ls collezione scelta
                }
                catch (Exception ex)
                {
                    // In caso di errore SSH, chiudo l'applicazione: la connessione via SSH è vitale
                    MessageBox.Show("Errore durante l'esecuzione del comando SSH:\n" + ex + "\nChiusura in corso...", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(2);
                }

                if (_lsCollezione != null)
                {
                    //Aggiungo i file nella listaFoto
                    foreach (string name in _lsCollezione)
                    {
                        if (name != "Thumbs.db" && name != "")
                            listaFoto.Items.Add(name);
                    }
                    btnAggiornaFoto.Enabled = true;
                }
            }
        }

        // Implementazione Filtro mediante TextBox -> Tab Foto
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (_lsCollezione != null)
            {
                // Filtro listaImmagini in base alla ricerca fatta dall'utente, mantenendo i valori checked.
                listaFoto.BeginUpdate();

                List<string> selected = listaFoto.CheckedItems.Cast<string>().ToList(); // Gli item selezionati vanno riaggiunti

                listaFoto.Items.Clear();

                foreach (string name in _lsCollezione)
                {
                    if (name != "Thumbs.db" && name != "")
                    {
                        if (selected.Contains(name))
                        {
                            listaFoto.Items.Add(name, true);
                        }
                        else
                        {
                            if (name.ToLower().Contains(boxRicercaFoto.Text.ToLower()))
                            {
                                listaFoto.Items.Add(name);
                            }
                        }
                    }
                }

                listaFoto.EndUpdate();
            }
            
        }

        // Button Aggiorna -> Tab Foto
        private void btnAggiorna_Click(object sender, EventArgs e)
        {
            string collezioneScelta = (string)boxCollezione.SelectedItem; // Get collezione scelta 
            List<string> selected = listaFoto.CheckedItems.Cast<string>().ToList(); // Get valori selezionati dall'utente

            List<string> toUpdate = new List<string>(); // Lista che alla fine conterrà tutti i nomi dei file da aggiornare

            foreach (string name in selected)
            {
                // Non sempre abbiamo anche la cartella modello (es. in TAVOLETTE).
                // Quindi se non ha un'estensione, vedo cosa c'è dentro la cartella, altrimenti se è un file .jpg lo aggiungo ai file da sincronizzare.
                if (! Path.HasExtension(name))
                {
                    // Se è una cartella, faccio ls ed inserisco in toUpdate tutti i nomi dei file di tipo .jpg
                    List<string> prodotti = null;
                    try
                    {
                        prodotti = _sshConnection.GetLsResult("/mnt/FOTO_CAMPIONARIO/" + collezioneScelta + "/" + name);
                    }
                    catch (Exception ex)
                    {
                        // In caso di errore SSH, chiudo l'applicazione: la connessione via SSH è vitale
                        MessageBox.Show("Errore durante l'esecuzione del comando SSH:\n" + ex + "\nChiusura in corso...", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Environment.Exit(2);
                    }


                    if (prodotti != null)
                    {
                        foreach (string prod in prodotti)
                        {
                            if (Path.GetExtension(prod) == ".jpg")
                            {
                                toUpdate.Add(prod);
                            }
                        }
                    }
                } else
                {
                    if (Path.GetExtension(name) == ".jpg")
                    {
                        toUpdate.Add(name);
                    }
                }
            }

            int toUpdateLength = toUpdate.Count(); // Numero dei file da aggiornare

            if (toUpdateLength > 0)
            {
                // Codice per fare in modo che venga eseguito un comando ogni MaxFoto foto (divido il compito, altrimenti starebbe troppo)
                int numberOfCycles = (toUpdateLength - 1) / MaxFoto + 1;
                string[] results = new string[numberOfCycles]; // Conterrà tutti gli script che dovranno essere eseguiti
                for (int i = 0; i < numberOfCycles; i++)
                {
                    results[i] = "./foto.sh ";
                    for (int j = 0; j < MaxFoto && j + (MaxFoto * i) < toUpdateLength; j++)
                    {
                        results[i] += toUpdate[j + (MaxFoto * i)] + " ";
                    }
                    commandLog = new CommandLog(); // Creo finestra in cui mostrare lo stato di avanzamento del comando
                    _sshConnection.ExecuteCommand(results[i], commandLog); // Eseguo comando
                    commandLog.Show();
                    
                }
            } else
            {
                if (selected.Count() > 0)
                {
                    MessageBox.Show("I modelli selezionati non hanno elementi", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                } else
                {
                    MessageBox.Show("Non è stato selezionato alcun elemento", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            
            // Riporto il TAB alla situazione di partenza
            boxCollezione.ResetText();
            boxCollezione.SelectedIndex = -1;
            listaFoto.Items.Clear();
            btnAggiornaFoto.Enabled = false;
        }

        // Button Aggiorna tutto -> Tab Tavolette
        private void btnAggiornaTuttoTav_Click(object sender, EventArgs e)
        {
            commandLog = new CommandLog(); // Finestra in cui mostrare lo stato di avanzamento del comando
            _sshConnection.ExecuteCommand("./foto_tavolette_acetato.sh", commandLog); // Eseguo comando
            commandLog.Show();
        }

        // Implementazione filtro mediante TextBox -> Tab Tavolette
        private void boxRicerca_TextChanged(object sender, EventArgs e)
        {
            // Filtro listaImmagini in base alla ricerca fatta dall'utente, mantenendo i valori checked.
            listaImmagini.BeginUpdate();

            List<string> selected = listaImmagini.CheckedItems.Cast<string>().ToList(); // Gli item selezionati vanno riaggiunti

            listaImmagini.Items.Clear();

            foreach (string name in _lsTavolette)
            {
                if (!name.EndsWith("_b.jpg") && Path.GetExtension(name) == ".jpg")
                {
                    string nameWithoutExtension = name.Substring(0, name.Count() - 4);
                    if (selected.Contains(nameWithoutExtension))
                    {
                        listaImmagini.Items.Add(nameWithoutExtension, true);
                    }
                    else
                    {
                        if (nameWithoutExtension.ToLower().Contains(boxRicerca.Text.ToLower()))
                        {
                            listaImmagini.Items.Add(nameWithoutExtension);
                        }
                    }
                }
            }

            listaImmagini.EndUpdate();
        }

        // Button Aggiorna -> Tab Tavolette
        private void btnAggiornaTav_Click(object sender, EventArgs e)
        {
            List<string> selected = listaImmagini.CheckedItems.Cast<string>().ToList(); // Gli item selezionati vanno riaggiunti

            int selectedLength = selected.Count();

            if (selectedLength > 0)
            {
                // Codice per fare in modo che venga eseguito un comando ogni MaxTavolette tavolette (divido il compito, altrimenti starebbe troppo)
                int numberOfCycles = (selectedLength - 1) / MaxTavolette + 1;

                string[] results = new string[numberOfCycles]; // Conterrà tutti gli script che dovranno essere eseguiti

                for (int i = 0; i < numberOfCycles; i++)
                {
                    results[i] = "./foto_tavolette_acetato.sh ";
                    for (int j = 0; j < MaxTavolette && j + (MaxTavolette * i) < selectedLength; j++)
                    {
                        results[i] += selected[j + (MaxTavolette * i)] + ".jpg ";
                    }
                    commandLog = new CommandLog(); // Finestra in cui mostrare lo stato di avanzamento del comando
                    _sshConnection.ExecuteCommand(results[i], commandLog); // Esegui Comando
                    commandLog.Show();
                }
            } else
            {
                MessageBox.Show("Non è stato selezionato alcun elemento", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            // Aggiornamento eseguito, torno alla situazione di partenza
            listaImmagini.Items.Clear();
            boxRicerca.Clear();
            PopulateTavolette();
        }

        private void boxLinea3D_SelectedIndexChanged(object sender, EventArgs e)
        {
            listaModelli3D.Items.Clear();
            if (_ls3D != null)
                _ls3D.Clear();

            boxRicerca3D.Clear();

            if (!btnAggiorna3D.Enabled)
            {
                btnAggiorna3D.Enabled = true;
            }

            string lineaScelta = (string)boxLinea3D.SelectedItem;

            if (!string.IsNullOrEmpty(lineaScelta))
            {
                try
                {
                    _ls3D = _sshConnection.GetFindResult("/mnt/MODELLI_3D/" + lineaScelta + "/", "C.obj");
                }   catch(Exception ex)
                {
                    // In caso di errore SSH, chiudo l'applicazione: la connessione via SSH è vitale
                    MessageBox.Show("Errore durante l'esecuzione del comando SSH:\n" + ex + "\nChiusura in corso...", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(3);
                }

                if (_ls3D != null)
                {
                    _ls3D.Sort();

                    listaModelli3D.BeginUpdate();
                    listaModelli3D.Items.Clear();

                    foreach (string name in _ls3D)
                    {
                        listaModelli3D.Items.Add(name.Substring(0, name.Count() - 4));
                    }

                    listaModelli3D.EndUpdate();

                }
            }
        }   

        private void btnAggiorna3D_Click(object sender, EventArgs e)
        {
            List<string> selected = listaModelli3D.CheckedItems.Cast<string>().ToList();
            string command = "./sync_3d_models.sh ";

            // Oltre al file obj base, inserisco tutti gli altri file obj a cui esso può essere connesso
            foreach (string name in selected)
            {
                command += name + ".obj " + name + "_ADX.obj " + name + "_ASX.obj " + name + "_CDX.obj " + name + "_CSX.obj " + name + "_LDX.obj " + name + "_LSX.obj ";
                command += name + "_AMDX.obj " + name + "_AMPDX.obj " + name + "_AMPSX.obj " + name + "_AMSX.obj " + name + "_CMDX.obj " + name + "_CMSX.obj ";
            }

            //int toUpdateLength = toUpdate.Count();

            if (selected.Count() > 0)
            {
                commandLog = new CommandLog(); // Finestra in cui mostrare lo stato di avanzamento del comando
                _sshConnection.Update3D(command, commandLog); // Eseguo comando
                commandLog.Show();
            }
            else
            {
                MessageBox.Show("Non è stato selezionato alcun elemento", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Aggiornamento eseguito, torno alla situazione di partenza
            boxRicerca3D.Clear();
            boxLinea3D.ResetText();
            boxLinea3D.SelectedIndex = -1;
            listaModelli3D.Items.Clear();
            btnAggiorna3D.Enabled = false;
            PopulateLinea3D();
        }

        // Button Aggiorna Tutto -> Tab Modelli3D
        private void btnAggiornaTutto3D_Click(object sender, EventArgs e)
        {
            commandLog = new CommandLog(); // Finestra in cui mostrare lo stato di avanzamento del comando
            _sshConnection.Update3D("./sync_3d_models.sh", commandLog); // Eseguo comando
            commandLog.Show();
        }

        // Implementazione filtro mediante TextBox -> Tab Modelli3D
        private void boxRicerca3D_TextChanged(object sender, EventArgs e)
        {

            if (_ls3D != null && _ls3D.Count() > 0)
            {
                // Filtro listaModelli3D in base alla ricerca fatta dall'utente, mantenendo i valori checked.
                listaModelli3D.BeginUpdate();

                List<string> selected = listaModelli3D.CheckedItems.Cast<string>().ToList(); // Gli item selezionati vanno riaggiunti

                listaModelli3D.Items.Clear();

                foreach (string name in _ls3D)
                {
                    if (Path.GetExtension(name) == ".obj")
                    {

                        string nameWithoutExtension = name.Substring(0, name.Count() - 4);

                        if (selected.Contains(nameWithoutExtension))
                        {
                            listaModelli3D.Items.Add(nameWithoutExtension, true);
                        }
                        else
                        {
                            if (nameWithoutExtension.ToLower().Contains(boxRicerca3D.Text.ToLower()))
                            {
                                listaModelli3D.Items.Add(nameWithoutExtension);
                            }
                        }
                    }
                }
                listaModelli3D.EndUpdate();
            }
        }

        // Button Aggiorna Conformity Documents
        private void btnAggiornaDoc_Click(object sender, EventArgs e)
        {
            commandLog = new CommandLog(); // Finestra in cui mostrare lo stato di avanzamento del comando
            _sshConnection.ExecuteCommand("./sync_conformity_documents.sh", commandLog); // Eseguo comando
            commandLog.Show();
        }

        private string AskDirectory(ElementToSync el, string initialDirectory)
        {
            string path = string.Empty;

            // Apri Dialog per permettere all'utente di selezionare la cartella
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = initialDirectory;
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                path = dialog.FileName;
            }
            else
            {
                path = "";
            }
            return path;
        }

        // Ritorna true se la directory parent è padre di otherPath o se è la stessa cartella.
        private bool IsDirectoryParent(string parent, string otherPath)
        {
            bool isParent = false;

            // Se uno dei due path è vuoto, ritorno subito false
            if (string.IsNullOrEmpty(parent) || string.IsNullOrEmpty(otherPath))
                return false;

            // Faccio in modo che entrambe non finiscano con un backslash
            if (parent.EndsWith("\\"))
                parent = parent.Remove(parent.Length - 1);
            if (otherPath.EndsWith("\\"))
                otherPath = otherPath.Remove(otherPath.Length - 1);

            DirectoryInfo di1 = new DirectoryInfo(parent);
            DirectoryInfo di2 = new DirectoryInfo(otherPath);

            while (di2.Parent != null)
            {
                if (di2.Parent.FullName == di1.FullName || di1.FullName == di2.FullName)
                {
                    isParent = true;
                    break;
                }
                else di2 = di2.Parent;
            }

            return isParent;
        }

        private void ApriSincronizzazioneXML_Clicked(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("notepad++.exe", "\"" + Path.GetDirectoryName(Application.ExecutablePath) + "\\sincro.xml\"");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Non è stato possibile aprire il file di configurazione XML.\n Errore: " + ex, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApriConfigNlog_Clicked(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("notepad++.exe", "\"" + Path.GetDirectoryName(Application.ExecutablePath) + "\\NLog.config\"");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Non è stato possibile aprire il file di configurazione di NLog.\n Errore: " + ex, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApriFileDiLog_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine(Path.GetDirectoryName(Application.ExecutablePath) + "\\sincronizzazione.log");
            try
            {
                System.Diagnostics.Process.Start("notepad++.exe", "\"" + Path.GetDirectoryName(Application.ExecutablePath) + "\\sincronizzazione.log\"");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Non è stato possibile aprire il file di Log. Probabilmente ancora non esiste perché non è stato eseguito alcun comando.\n Errore: " + ex, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApriCartellaFileDiLog_Clicked(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Path.GetDirectoryName(Application.ExecutablePath));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Non è stato possibile aprire la cartella con i file di Log.\n Errore: " + ex, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
