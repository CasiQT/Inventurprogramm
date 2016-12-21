using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

/* Wichtig: 
 * move und copy der MDE-TXT wieder aktivieren. Wurde zum debuggen der Druckfunktion auskommentiert!
*/

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        //Filename setzen
        string filePfadLog = @"..\verarbeitet.txt";
        
        public Form1()
        {
            InitializeComponent();
            textBox3.Text = System.Environment.CurrentDirectory;
            //form2.Show();

            //Standardprinter anzeigen
            PrinterSettings printSettings = new PrinterSettings();
            textBoxStdPrinter.Text = printSettings.PrinterName.ToString();

            //Logdatei vorbereiten            
            //File falls nicht vorhanden erstellen
            StreamWriter sw = new StreamWriter(filePfadLog, true, Encoding.UTF8);
            //Introzeilen generieren
            //Zeile1
            sw.Write("\r\nProgramm wurde gestartet: " + DateTime.Now.ToString("dd.MM.yyyy - HH:mm") + ";");
            sw.Write(";");
            sw.Write(";");
            sw.Write(";");
            sw.Write(";");
            sw.Write(";");
            sw.Write(";");
            sw.Write("\r\n");
            //Zeile2
            sw.Write("Dateiname;");
            sw.Write("Anzahl Positionen;");
            sw.Write("Zaehlposition;");
            sw.Write("Bereich;");
            sw.Write("Zaehler1;");
            sw.Write("Zaehler2;");
            sw.Write("Uhrzeit;");
            sw.Write("\r\n");
            //StreamWriter schließen
            sw.Close();
        }

        //deklarieren wegen Scope
        string[,] invArtSepariert; //invArt Array separiert
        string[,] mdeSepariert; // MDE Array separiert

        //globales fürs Drucken
        public string fileNmForPrint { get; set; }
        public string posForPrint { get; set; }
        public string amtForPrint { get; set; }
        public string usr1ForPrint { get; set; }
        public string usr2ForPrint { get; set; }
        public string areaForPrint { get; set; }
        //Filename mit Pfad

        //Printstrings deklarieren
        string printStringTopLeft = "";
        string printStringTopRight = "";
        string printStringTrenner = "";
        string[] printStringContentArr;
        string printStringContent = "";
        //string printStringFusszeile = "";
        int printZeile = 0;
        int printZeilenGesamt;
        bool headerGedruckt = false;
        string[] printStringLines;
        bool firstPageRdy;

        //Artikelstamm auswählen
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result1 = openFileDialog1.ShowDialog();
            
            if(result1 == DialogResult.OK)
            {
                // Dateipfad und name in Feld ablegen zur Anzeige
                textBox1.Text = openFileDialog1.FileName;

                // Status für Statusbox
                statusBox.AppendText("Inv-Art ausgewählt: " + textBox1.Text);
                statusBox.AppendText(System.Environment.NewLine);

                //execution timer
                var watch1 = System.Diagnostics.Stopwatch.StartNew();

                try
                {
                    //InvArt in Arbeitsspeicher geladen.
                    string[] lines = File.ReadAllLines(openFileDialog1.FileName, Encoding.UTF8);

                    invArtSepariert = new string[lines.Length, 10];

                    //aufsplitten in 2D Array
                    for (int i = 0; i < lines.Length; i++)
                    {
                        //Splitten
                        string[] a = lines[i].Split(';');

                        for (int j = 0; j < a.Length; j++)
                        {
                            //Array "a" in "invArtSepariert"-Array einpflegen
                            invArtSepariert[i, j] = a[j];
                        }
                    }

                    //Verarbeitungszeit wird gestoppt
                    watch1.Stop();
                    var elapsedMs1 = watch1.ElapsedMilliseconds;

                    statusBox.AppendText("Inv-Art erfolgreich eingelesen und verarbeitet in " + elapsedMs1 + "ms.\r\n");

                    //nächster Button aktiv wenn alles OK ist.
                    button2.Enabled = true;
                    button2_2.Enabled = true;
                }
                catch (Exception ex)
                {
                    statusBox.AppendText("Es gab ein Problem beim Verarbeiten der Inv-Art: " + ex.Message + "\r\n");
                }
            } else
            {
                statusBox.AppendText("Bitte gültige Inv-Art Datei auswählen.\r\n");
            }            
        }

        //MDE-TXT auswählen
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result2 = openFileDialog2.ShowDialog();

            if (result2 == DialogResult.OK)
            {
                // Dateipfad und name in Feld ablegen zur Anzeige
                textBox2.Text = openFileDialog2.FileName;

                // Status für Statusbox
                statusBox.AppendText("MDE-TXT ausgewählt: " + Path.GetFileNameWithoutExtension(textBox2.Text) + "\r\n");

                //nächster Button aktiv wenn alles OK ist.
                button3.Enabled = true;

                progressBar1.Value = 0;
            } else
            {
                statusBox.AppendText("Bitte gültige MDE-TXT-Datei auswählen.\r\n");
            }
        }

        //neuste MDE automatisch auswählen
        private void button2_2_Click(object sender, EventArgs e)
        {
            var directory = new DirectoryInfo(System.IO.Path.GetDirectoryName(Application.ExecutablePath));
            //debug
            statusBox.AppendText("Aktueller Pfad: " + directory.ToString() + "\r\n");

            try
            {
                var myFile = (from f in directory.GetFiles("*.txt")
                              orderby f.LastWriteTime descending
                              select f).FirstOrDefault();

                if (myFile != null)
                {
                    // Dateipfad und name in Feld ablegen zur Anzeige
                    textBox2.Text = directory.ToString() + "\\" + myFile.ToString();

                    // Status für Statusbox
                    statusBox.AppendText("MDE-TXT ausgewählt: " + Path.GetFileNameWithoutExtension(textBox2.Text) + "\r\n");

                    //nächster Button aktiv wenn alles OK ist.
                    button3.Enabled = true;

                    progressBar1.Value = 0;

                    //pfad in Variable zum weiterverarbeiten ablegen
                    openFileDialog2.FileName = (directory.ToString() + "\\" + myFile.ToString());
                }
                else
                {
                    statusBox.AppendText("Keine (gültige) MDE-TXT-Datei gefunden.\r\n");
                }
            }
            catch (Exception ex)
            {
                statusBox.AppendText("Es ist ein Problem aufgetreten: " + ex.Message + "\r\n");
            }
        }

        //MDE TXT verarbeiten
        private void button3_Click(object sender, EventArgs e)
        {
            // ProgressbarValue
            int progressValue = 0;

            //execution timer
            var watch2 = System.Diagnostics.Stopwatch.StartNew();

            //Deklaration wegen Scope
            string[] lines;

            //MDE-TXT parsen
            try
            {
                //MDE-TXT in Arbeitsspeicher laden.
                lines = File.ReadAllLines(openFileDialog2.FileName, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                statusBox.AppendText("Beim lesen der MDE-TXT gab es ein Problem: " + ex.Message + "\r\n");
                return;
            }

            mdeSepariert = new string[lines.Length, 10];

            //Druckzeilen gesamt
            printZeilenGesamt = lines.Length;

            //aufsplitten in 2D Array
            for (int i = 0; i < lines.Length; i++)
            {
                //Splitten
                string[] a = lines[i].Split(';');

                for (int j = 0; j < a.Length; j++)
                {
                    //Array "a" in "MDESepariert"-Array einpflegen
                    mdeSepariert[i, j] = a[j];
                }
            }

            // New Array zur Ausgabe
            String[,] verarbeitet = new string[lines.Length, 8];
            
            // 1D Stringarray mit größe MDE-TXT Lines.Length
            printStringLines = new string[lines.Length];

            // Datenverarbeitung 
            // Array einmal MDE Positionenzahl durchlaufen
            for (int i = 0; i < lines.Length; i++)
            {
                // index in neues array eintragen, jeweils "i"
                verarbeitet[i, 0] = String.Format("{0, 5}", (i+1).ToString());

                int j = 0;

                // While Schleife maximal 1x invArt-Anzahl durchlaufen
                while (j < invArtSepariert.GetLength(0))
                {
                    // Falls MDE Pos 2 (Artikelnummer) der Inv-Art Pos 1 entspricht (auch Artikelnummer) ..
                    // .. dann Daten in "verarbeitet"-Array eintragen. Außerdem j inkrementieren und schleife verlassen.
                    if (mdeSepariert[i, 1] == invArtSepariert[j, 0])
                    {
                        verarbeitet[i, 1] = mdeSepariert[i, 1];
                        verarbeitet[i, 2] = invArtSepariert[j, 1];
                        
                        printStringLines[i] += verarbeitet[(i), 0]
                            + "   "
                            + String.Format("{0, 8}", mdeSepariert[i, 1].ToString())
                            + "   "
                            + String.Format("{0, 10}", "  _______ ")
                            + "   "
                            + String.Format("{0, 10}", mdeSepariert[i, 2].ToString().TrimStart('0'))
                            + "    "
                            + String.Format("{0, -40}", invArtSepariert[j, 1].ToString())
                            + "\r\n";

                        break;
                    }
                    j++;
                }

                //Progressbar
                if ((progressValue+10) < (i / ((float)lines.Length / 100)) && progressValue < 90)
                {
                    //ProgressValue um 10 erhöhen
                    progressValue += 10;

                    //Progressbar zuweisen
                    progressBar1.Value = progressValue;

                    //debug
                    //form2.statusBox2.AppendText("Progressbar wurde erhöht auf: " + progressValue + "\r\n");
                }
            }

            progressBar1.Value = 100;

            //Verarbeitungszeit wird gestoppt
            watch2.Stop();
            var elapsedMs2 = watch2.ElapsedMilliseconds;

            statusBox.AppendText("Daten erfolgreich verarbeitet in: " + elapsedMs2.ToString() + "ms.\r\n");

            //Eintrag ins Datagridview
            dataGridView1.ColumnCount = 7;

            //Spaltenname im GridView festlegen
            dataGridView1.Columns[0].Name = "Dateiname";
            dataGridView1.Columns[1].Name = "#";
            dataGridView1.Columns[2].Name = "Zaehlposition";
            dataGridView1.Columns[3].Name = "Bereich";
            dataGridView1.Columns[4].Name = "Zaehler1";
            dataGridView1.Columns[5].Name = "Zaehler2";
            dataGridView1.Columns[6].Name = "Uhrzeit";

            //Reihe für GridView füllen
            string[] row1 = new string[]
            {
                Path.GetFileNameWithoutExtension(openFileDialog2.FileName),
                lines.Length.ToString(),
                mdeSepariert[0,8],
                mdeSepariert[0,7],
                mdeSepariert[0,5],
                mdeSepariert[0,6],
                DateTime.Now.ToString("dd.MM.yyyy - HH:mm")
            };

            //Reihe ins GridView einfügen
            dataGridView1.Rows.Add(row1);

            //Reihe in ..\verarbeitet.txt einfügen
            StreamWriter sw = new StreamWriter(filePfadLog, true, Encoding.UTF8);

            sw.Write(Path.GetFileNameWithoutExtension(textBox2.Text) + ".txt;");
            sw.Write(lines.Length.ToString() + ";");
            sw.Write(mdeSepariert[0, 8] + ";");
            sw.Write(mdeSepariert[0, 7] + ";");
            sw.Write(mdeSepariert[0, 5] + ";");
            sw.Write(mdeSepariert[0, 6] + ";");
            sw.Write(DateTime.Now.ToString("dd.MM.yyyy - HH:mm") + ";");
            sw.Write("\r\n");
            //StreamWriter schließen
            sw.Close();

            //Globale Variablen fürs Drucken setzen
            //globales fürs Drucken
            fileNmForPrint = row1[0];
            amtForPrint = row1[1];
            posForPrint = row1[2];
            areaForPrint = row1[3];
            usr1ForPrint = row1[4];
            usr2ForPrint = row1[5];

            ////Statusbox Info
            //form2.setTextForm2("Daten verarbeitet in: " + elapsedMs2.ToString() + "ms\r\n");

            //button "Verarbeiten" ausblenden (damit neue Datei ausgewählt wird)
            button3.Enabled = false;

            //button "Drucken" eingeblendet
            btnPrint.Enabled = true;

            //Daten zu Drucken in String Array legen
            printStringContentArr = File.ReadAllLines(textBox2.Text);

            //Datei wegkopieren (zu debugzwecken ausgeblendet)
            //checken obs Ordner schon gibt
            if (!Directory.Exists(@"..\Erledigt"))
            {
                Directory.CreateDirectory(@"..\Erledigt");
            }

            if (!Directory.Exists(@"..\Originale"))
            {
                Directory.CreateDirectory(@"..\Originale");
            }

            //Files wegschieben 
            try
            {
                File.Copy(textBox2.Text, @"..\Originale\" + Path.GetFileName(textBox2.Text));
                File.Move(textBox2.Text, @"..\Erledigt\" + Path.GetFileName(textBox2.Text));
                statusBox.AppendText("Datei verschoben nach ..\\Erledigt und \\Original \r\n");
            }
            catch (Exception ex)
            {
                statusBox.AppendText("Es ist ein Problem aufgetreten beim Verschieben der Datei: " + ex.Message + "\r\n");
            }

            // direkt drucken falls Haken gesetzt ist
            if (checkBox1.Checked == true)
            {
                btnPrint_Click(this, e);
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            // Nur für Statusbox
            statusBox.AppendText("Button4 clicked\r\n");
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        //Datagridview automatisch speichern
                
        
        //private void btnSave_Click(object sender, EventArgs e)
        //{
        //    //wenn das datagridview nicht leer ist
        //    if (dataGridView1.RowCount > 0)
        //    {
        //        //Filename mit Pfad
        //        string filePfad = @"..\verarbeitet.txt";

        //        // neues 2D Array 7 Spalten (eine für Nummerierung und eine für Uhrzeit)
        //        string[,] dgv1StrArr = new string[dataGridView1.RowCount + 1, 8];

        //        //Kopfzeile füllen
        //        dgv1StrArr[0, 0] = "Zeile";
        //        dgv1StrArr[0, 1] = "Dateiname";
        //        dgv1StrArr[0, 2] = "Anzahl Positionen";
        //        dgv1StrArr[0, 3] = "Zaehlposition";
        //        dgv1StrArr[0, 4] = "Bereich";
        //        dgv1StrArr[0, 5] = "Zaehler1";
        //        dgv1StrArr[0, 6] = "Zaehler2";
        //        dgv1StrArr[0, 7] = "Uhrzeit";

        //        // stringArray füllen
        //        for (int i = 1; i <= dataGridView1.RowCount; i++)
        //        {
        //            dgv1StrArr[i, 0] = i.ToString();
        //            dgv1StrArr[i, 1] = dataGridView1.Rows[(i - 1)].Cells[0].Value.ToString();
        //            dgv1StrArr[i, 2] = dataGridView1.Rows[(i - 1)].Cells[1].Value.ToString();
        //            dgv1StrArr[i, 3] = dataGridView1.Rows[(i - 1)].Cells[2].Value.ToString();
        //            dgv1StrArr[i, 4] = dataGridView1.Rows[(i - 1)].Cells[3].Value.ToString();
        //            dgv1StrArr[i, 5] = dataGridView1.Rows[(i - 1)].Cells[4].Value.ToString();
        //            dgv1StrArr[i, 6] = dataGridView1.Rows[(i - 1)].Cells[5].Value.ToString();
        //            dgv1StrArr[i, 7] = dataGridView1.Rows[(i - 1)].Cells[6].Value.ToString();
        //        }

        //        //Datei beschreiben
        //        try
        //        {
        //            //Streamwriter initialisieren
        //            StreamWriter sw = new StreamWriter(filePfad, true, Encoding.UTF8);

        //            for (int i = 0; i < dgv1StrArr.GetLength(0); i++)
        //            {
        //                sw.Write(dgv1StrArr[i, 0] + ";");
        //                sw.Write(dgv1StrArr[i, 1] + ";");
        //                sw.Write(dgv1StrArr[i, 2] + ";");
        //                sw.Write(dgv1StrArr[i, 3] + ";");
        //                sw.Write(dgv1StrArr[i, 4] + ";");
        //                sw.Write(dgv1StrArr[i, 5] + ";");
        //                sw.Write(dgv1StrArr[i, 6] + ";");
        //                sw.Write(dgv1StrArr[i, 7] + ";");
        //                sw.Write("\r\n");
        //            }
        //            //StreamWriter schließen
        //            sw.Close();
        //            statusBox.AppendText("Datei erfolgreich gespeichert: ..\\verarbeitet.txt\r\n");
        //        }
        //        catch (Exception ex)
        //        {
        //            statusBox.AppendText("Es gab ein Problem: " + ex.Message + "\r\n");
        //        }
        //    }
        //    else
        //    {
        //        statusBox.AppendText("Keine Infos zum Speichern vorhanden\r\n");
        //    }
        //}

        //Suchknopf

        private void srchBtn_Click(object sender, EventArgs e)
        {
            //bei neuer Suche erstmal alles abmarkieren
            dataGridView1.ClearSelection();

            int result = 0;

            //falls das Gridview nicht leer ist dann...
            if (dataGridView1.RowCount > 0)
            {
                //Grid wird immer komplett durchlaufen
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    //Regex um Leerzeichen zu entfernen. Da nur zahlen, kein Casecheck.
                    if (Regex.Replace(dataGridView1.Rows[i].Cells[2].Value.ToString(), @"\s", "") == Regex.Replace(textBox5.Text.ToString(), @"\s", ""))
                    {
                        //Resultatscounter wird erhöht
                        result++;
                        //Zeile mit den Funden wird markiert
                        dataGridView1.Rows[i].Selected = true;
                    }
                }
            }
            //Resultate ausgeben.
            if (result==0)
            {
                statusBox.AppendText("Keine Ergebnisse.\r\n");
            }
            else
            {
                statusBox.AppendText("Suche ergab: " + result.ToString() + " Treffer\r\n");
            }
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                srchBtn_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        //PrinterCode
        //Standard Printer bereits oben ausgegeben

        private void btnPrint_Click(object sender, EventArgs e)
        {
            // Parameter resetten
            printStringTrenner = "";
            printStringTopLeft = "";
            printStringTopRight = "";
            printStringTrenner = "";
            printStringContent = "";
            headerGedruckt = false;
            firstPageRdy = false;
            printZeile = 0;
            statusBox.AppendText("Wird gedruckt ...\r\n");
            printDocument1.Print();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            firstPageRdy = false;
            ////debug
            //form2.statusBox2.AppendText("PrintPage event ausgelöst\r\n");

            //Druckausgabe
            //Rechteck definieren und position darin
            var formatRechts = new StringFormat() { Alignment = StringAlignment.Far };

            var rect = new RectangleF(50, 100, 700, 250);
            if (headerGedruckt == false) //falls Header noch nicht gedruckt
            {
                ////debug
                //form2.statusBox2.AppendText("Headerdruck.\r\n");

                //Header drucken
                //Header links
                printStringTopLeft += "Informationen aus der Datei: ";
                printStringTopLeft += "\r\n";
                printStringTopLeft += "\r\n";
                printStringTopLeft += "Datei: " + fileNmForPrint + ".txt";
                printStringTopLeft += "\r\n";
                printStringTopLeft += "Zaehlung: " + posForPrint;
                printStringTopLeft += "\r\n";
                printStringTopLeft += "Positionen: " + amtForPrint;
                printStringTopLeft += "\r\n";
                printStringTopLeft += "Zaehler 1: " + usr1ForPrint;
                printStringTopLeft += "\r\n";
                printStringTopLeft += "Zaehler 2: " + usr2ForPrint;
                printStringTopLeft += "\r\n";
                printStringTopLeft += "Bereich: " + areaForPrint;
                printStringTopLeft += "\r\n";

                //Header rechts
                printStringTopRight += "Ausgelesen: ";
                printStringTopRight += "\r\n";
                printStringTopRight += "\r\n";
                printStringTopRight += System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                printStringTopRight += "\r\n";
                printStringTopRight += DateTime.Now.ToString("dd.MM.yyyy - HH:mm");
                printStringTopRight += "\r\n";
                printStringTopRight += System.Environment.MachineName.ToString();
                printStringTopRight += "\r\n";
                printStringTopRight += "\r\n";
                printStringTopRight += "\r\n";
                printStringTopRight += "____________________          ____________________\r\n";
                printStringTopRight += "Unterschrift Ausleser                         Unterschrift Zaehler";

                //Trennlinien
                printStringTrenner += trennlinienForPrint(85);

                e.Graphics.DrawString(printStringTrenner, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 50, 50);
                e.Graphics.DrawString(printStringTopLeft, new Font("Arial", 10), Brushes.Black, rect);
                e.Graphics.DrawString(printStringTopRight, new Font("Arial", 10), Brushes.Black, rect, formatRechts);
                e.Graphics.DrawString(printStringTrenner, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 50, 260);

                //Header damit gedruckt
                headerGedruckt = true;
            }

            // Falls 50 Positionen oder weniger
            if (printZeilenGesamt < 50)
            {
                //Tabellenhead
                printStringContent += tabellenHeadString();

                ////debug
                //form2.statusBox2.AppendText("Weniger als 50 Zeilen\r\n");

                for (int i = 0; i < printZeilenGesamt; i++)
                {
                    ////debug
                    //form2.statusBox2.AppendText("Unter 50 Zeilen - for-Schleife, Pos. (i): " + i + "\r\n");

                    printStringContent += printStringLines[i];
                }
                e.Graphics.DrawString(printStringContent, new Font(FontFamily.GenericMonospace, 9), Brushes.Black, 50, 310);
                e.HasMorePages = false;
            } else //mehr als 50
            {
                ////debug
                //form2.statusBox2.AppendText("Mehr als 50 Zeilen\r\n");

                //Tabellenhead
                printStringContent += tabellenHeadString();

                // erste Seite fertigdrucken mit max 50 Zeilen
                if (printZeile < 49)
                {
                    //// debug
                    //form2.statusBox2.AppendText("Erste Seite fertigdrucken mit 50 Datensätzen \r\n");

                    // erste Seite fertiggedruckt?
                    firstPageRdy = true;

                    // Die ersten 50 printZeilenGesamt
                    for (int i = 0; i < 50; i++)
                    {
                        printStringContent += printStringLines[i];
                        printZeile++; // wird 50x
                    }
                    
                    //// debug
                    //form2.statusBox2.AppendText("printZeile sollte hier auf 49 sein. Ist: " + printZeile + "\r\n");
                    
                    //1. Seite fertigdrucken und content resetten.
                    e.Graphics.DrawString(printStringContent, new Font(FontFamily.GenericMonospace, 9), Brushes.Black, 50, 310);
                    printStringContent = "";
                    e.HasMorePages = true;
                }
                //weitere Durchläufe
                if (printZeile >= 49 && firstPageRdy == false)
                {
                    ////debug
                    //form2.statusBox2.AppendText("Weiterer Durchgang printZeile > 50\r\n");

                    //Seitencontent resetten
                    printStringContent = "";

                    //wenn noch mehr als 75 Datensätze kommen dann volle Seite drucken, ansonsten den Rest
                    //wenn gesamtdatensätze abzüglich der bereits verarbeiteten größer 75 dann volle Seite also 75
                    if ((printZeilenGesamt-printZeile)>75)
                    {
                        printStringContent += tabellenHeadString();
                        for (int i = 0; i < 75; i++)
                        {
                            printStringContent += printStringLines[printZeile];
                            printZeile++;
                        }
                        e.Graphics.DrawString(printStringContent, new Font(FontFamily.GenericMonospace, 9), Brushes.Black, 50, 30);
                        e.HasMorePages = true;
                    }
                    //wenn noch weniger oder gleich als 75 Datensätze kommen dann wird der rest hier ausgegeben auf letzter Seite
                    else
                    {
                        ////debug
                        //form2.statusBox2.AppendText("letzte Seite, Rest kleiner 75 und zwar: " + (printZeilenGesamt - printZeile).ToString() + "\r\n");

                        printStringContent += tabellenHeadString();
                        int restMenge = (printZeilenGesamt - printZeile);
                        for (int i = 0; i < restMenge; i++)
                        {
                            ////debug
                            //form2.statusBox2.AppendText("letzte Seite for-Schleife durchlaufen. i: " + i + "\r\n");

                            printStringContent += printStringLines[printZeile];
                            printZeile++;
                        }
                        e.Graphics.DrawString(printStringContent, new Font(FontFamily.GenericMonospace, 9), Brushes.Black, 50, 30);
                        e.HasMorePages = false;
                    }
                }
            }
        }

        //Standardprinter aktualisieren
        private void stdPrntBtnNeu_Click(object sender, EventArgs e)
        {
            PrinterSettings printSettings = new PrinterSettings();
            textBoxStdPrinter.Text = printSettings.PrinterName.ToString();
        }

        private string trennlinienForPrint(int x)
        {
            string rueckgabe = "";
            if (x>0)
            {
                for (int i = 0; i < x; i++)
                {
                    rueckgabe += "=";
                }
                rueckgabe += "\r\n";
            }
            return rueckgabe;
        }

        private string tabellenHeadString()
        {
            //Kopfzeile
            //string tabHeadString = " Pos.  | Artikel  |   Anzahl   |            Artikelname\r\n \r\n";
            string tabHeadString = " Pos.  | Artikel  | Korrektur |   Anzahl   |            Artikelname\r\n \r\n";
            return tabHeadString;
        }

        // Help window (form3)
        private void btnHelp_Click(object sender, EventArgs e)
        {
            // schauen ob ne Form3 form offen ist. Falls nein dann eine aufmachen. Verhindert multiple windows.
            if (Application.OpenForms.OfType<Form3>().Count() < 1)
            {
                Form3 form3 = new Form3();
                form3.Show();
            }
        }
    }
}
