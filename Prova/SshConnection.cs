using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace Prova
{
    class SshConnection
    {

        // Per utilizzare NLog: è possibile scrivere sul file di log sincronizzazione.log con: Logger.Info("msg");
        // sincronizzazione.log è in bin/Debug/sincronizzazione.log  ( o bin/Release/sincronizzazione.log se usiamo la Release)
        // Il file di configurazione di NLog è in bin/Debug/NLog.config ( o bin/Release/Nlog.config se usiamo la Release)
        // Al momento ho impostato un limite di 100MB per file di log, e vengono tenuti al massimo 7 file (uno al giorno se nessuno eccede i 100MB).
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly object lockForSharedMethod = new object(); // Lock per i Modelli3D

        private string _address; // Indirizzo
        private string _name;    // Username
        private string _pwd;     // Password
        public SshClient Ssh { get; private set; }

        // Costruttore, prende in input indirizzo, username e password e crea la connessione.
        public SshConnection(string address, string name, string pwd)
        {
            _address = address;
            _name = name;
            _pwd = pwd;

            try
            {
                Ssh = new SshClient(_address, _name, _pwd);
                Ssh.Connect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        // Gli script che riguardano i modelli3D usano una cartella di supporto che creano quando eseguiti e distruggono prima di terminare.
        // Se facessi partire tali script contemporaneamente, andrebbero in conflitto tra loro.
        // Dunque ho creato delle funzioni apposite che usano un lock per decidere se far eseguire un comando ad un thread oppure no.
        public void Update3D(string command, CommandLog log)
        {
            object args = new object[2] { command, log };
            // Uso un thread per poter continuare ad usare l'applicazione principale mentre il comando viene eseguito
            Thread thr = new Thread(new ParameterizedThreadStart(Execute3D));
            thr.Start(args);
        }

        private void Execute3D(object obj)
        {
            Array argArray = new object[2];
            argArray = (Array)obj;
            string command = (string)argArray.GetValue(0);
            CommandLog form = (CommandLog)argArray.GetValue(1);
            try
            {
                form.addLog("In attesa che il comando lanciato precedentemente termini.");
            } catch (Exception ex)
            {
                MessageBox.Show("Errore nell'acquisizione dei log per il comando:\n" + command + "\nErrore:\n" + ex, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lock (lockForSharedMethod)
            {
                Execute(obj);
            }
        }

        // Esegui il comando command in un nuovo thread. Stampa l'output in una nuova finestra (CommandLog)
        public void ExecuteCommand(string command, CommandLog log)
        {
            object args = new object[2] {command, log };
            // Uso un thread per poter continuare ad usare l'applicazione principale mentre il comando viene eseguito
            Thread thr = new Thread(new ParameterizedThreadStart(Execute));
            thr.Start(args);
        }

        private void Execute(object obj)
        {
            // Get degli argomenti da obj
            Array argArray = new object[2];
            argArray = (Array)obj;
            string command = (string)argArray.GetValue(0);
            CommandLog form = (CommandLog)argArray.GetValue(1);

            SshCommand cmd;
            IAsyncResult asynch;
            try
            {
                // Se avviene un errore in questo try-catch, il comando non è stato nemmeno inviato via SSH.
                cmd = Ssh.CreateCommand(command);
                // Chiamata asincrona: nel frattempo leggo l'output e lo stampo a video
                asynch = cmd.BeginExecute();
            } catch (Exception ex)
            {
                MessageBox.Show("Errore SSH nell'esecuzione del comando:\n" + command + "\nIl comando non è stato eseguito. Errore:\n" + ex, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            StreamReader reader = new StreamReader(cmd.OutputStream); // Per la lettura on-the-fly dei log

            string output;
            string lastCommand = string.Empty;
            string completeOutput = string.Empty;

            try
            {
                // Se avviene un errore qui, il comando verrà comunque eseguito (a meno che la macchina a cui eravamo connessi non si sia spenta, in quel caso
                // avremo i log nella macchina stessa per capire fino a che punto era arrivata con l'esecuzione del comando)
                // Se la connessione è stata persa nel momento in cui anche la macchina a cui eravamo connessi ha smesso di eseguire il comando, allora 
                // i log in "sincronizzazione.log" saranno aggiornati (riusciremo a capire fino a che punto la macchina ha eseguito il comando).
                while (!asynch.IsCompleted)
                {
                    output = reader.ReadToEnd(); // Leggo fino alla fine del flusso
                    completeOutput += output;

                    // Prendo l'ultima stringa non vuota da output e la inserisco nel CommandLog
                    bool found = false;
                    var commands = completeOutput.Split('\n');
                    for (int i = commands.Count()-1; i >= 0 && !found; i--)
                    {
                        if (!string.IsNullOrEmpty(commands[i]))
                        {
                            found = true;
                            if (commands[i] != lastCommand)
                            {
                                form.addLog(commands[i]);
                            }
                            lastCommand = commands[i];
                        }
                    }
                }

                output = reader.ReadToEnd(); // Leggo fino alla fine del flusso
                completeOutput += output;
                form.addLog("Completato");

            } catch(Exception ex)
            {
                MessageBox.Show("Errore nell'acquisizione dei log per il comando:\n" + command + "\nErrore:\n" + ex, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            form.LoadingDone();
            Logger.Info(completeOutput); // Aggiungo ai log l'output completo
        }

        // Esegue una ls di directory, ritorna la lista dei file/cartelle presenti.
        public List<string> GetLsResult(string directory)
        {
            List<string> resList = new List<string>(); // Risultato che ritornerò
            try
            {
                string res = string.Empty; // Effettiva directory su cui chiamerò ls
                if (directory.Contains(' ')) // Se ci sono spazi, aggiungo un backslash prima di essi (perché ls funzioni senza errori)
                {
                    string[] words = directory.Split(' ');

                    foreach (string word in words)
                    {
                        res += "\\ " + word;
                    }
                    res = res.Substring(2);
                }
                else // Se non ci sono spazi, semplicemente uso la stringa directory
                {
                    res = directory;
                }
                var cmdLs = Ssh.CreateCommand("ls " + res);
                cmdLs.Execute();
                var result = cmdLs.Result;
                foreach (string word in result.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(word))
                    {
                        resList.Add(word);
                    }
                }
            } catch(Exception ex)
            {
                throw ex; // L'eccezione deve essere gestita quando la funzione viene richiamata.
            }
            return resList;
        }

        // Esegue una find, listando tutti i file che finiscono in "fileEnding" nella directory "directory" (ricorsivamente). Ritorna la lista di tali file
        public List<string> GetFindResult(string directory, string fileEnding)
        {
            List<string> resList = new List<string>(); // Risultato che ritornerò
            try
            {
                string res = string.Empty; // Effettiva directory su cui chiamerò find
                if (directory.Contains(' '))
                {
                    string[] words = directory.Split(' ');

                    foreach (string word in words)
                    {
                        res += "\\ " + word;
                    }
                    res = res.Substring(2);
                }
                else // Se non ci sono spazi, semplicemente uso la stringa directory
                {
                    res = directory;
                }
                var cmdFind = Ssh.CreateCommand("SAVEIFS=$IFS; IFS=$'\n'; basename -a $(find " + directory + " -iname '*" + fileEnding + "'); IFS=$SAVEIFS");
                cmdFind.Execute();
                var result = cmdFind.Result;
                foreach (string word in result.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(word))
                    {
                        resList.Add(word);
                    }
                }
            } catch(Exception ex)
            {
                throw ex; // L'eccezione deve essere gestita quando la funzione viene richiamata.
            }
            return resList;
        }
    }
}
