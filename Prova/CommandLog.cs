using System;
using System.IO;
using System.Windows.Forms;

namespace Prova
{
    public partial class CommandLog : Form
    {
        
        public CommandLog()
        {
            InitializeComponent();
            pictureBox1.Size = pictureBox1.Image.Size;
            lblIstruzione.Text = "";
        }

        delegate void SetTextCallback(string text);

        // Aggiungi log al textBox -> Usato dal thread in SshConnection.cs
        public void addLog(string msg)
        {
            if (lblIstruzione.InvokeRequired) // Se chi ha chiamato la funzione non può eseguirla perché è in un thread diverso, allora uso il metodo Invoke
            {
                SetTextCallback d = new SetTextCallback(addLog);
                Invoke(d, new object[] { msg });
            }
            else
            {
                lblIstruzione.Text = msg;
            }
        }

        delegate void SetLoadingDoneCallback(); 

        // Setta nuova Image Ok ed abilita i Button "Ok" e "Mostra Log" -> Usato dal thread in SshConnection.cs
        public void LoadingDone()
        {
            if (btnOk.InvokeRequired || btnMostraLog.InvokeRequired || pictureBox1.InvokeRequired) // Se chi ha chiamato la funzione non può eseguirla perché è in un thread diverso, allora uso il metodo Invoke
            {
                SetLoadingDoneCallback d = new SetLoadingDoneCallback(LoadingDone);
                Invoke(d);
            } else
            {
                pictureBox1.Image = Properties.Resources.ok;
                pictureBox1.Size = pictureBox1.Image.Size;
                btnOk.Enabled = true;
                btnMostraLog.Enabled = true;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMostraLog_Click(object sender, EventArgs e)
        {
            // Apri log
            try
            {
                System.Diagnostics.Process.Start("notepad++.exe", "\"" + Path.GetDirectoryName(Application.ExecutablePath) + "\\sincronizzazione.log\"");
            } catch (Exception ex)
            {
                MessageBox.Show("Non è stato possibile aprire il file di log.\n Errore: " + ex, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
