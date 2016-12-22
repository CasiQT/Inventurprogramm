using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            this.richTextBox1.Rtf = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1031{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil\fcharset0 MS Sans Serif;}}
                \viewkind4\uc1\pard\fs40 Guide und Infos zum Inventurprogramm
                \par 
                \par \b\fs20 Quickstart:
                \par - Tool ins Verzeichnis in dem die MDE-Textfiles landen
                \par - Standarddrucker w\'e4hlen
                \par - MDE-TXT ausw\'e4hlen
                \par - MDE-TXT verarbeiten
                \par - MDE-TXT drucken
                \par 
                \par ==============================
                \par 
                \par Guide:
                \par 
                \par 1.)\b0  Das Tool selbst (Inventurtool.exe) soll in dem Ordner liegen, in den sp\'e4ter die MDE-Z\'e4hldateien (Bsp. 97017.txt) vom MDE-Ger\'e4t gespeichert werden. 
                \par Der aktuelle Pfad wird oben im Programm angezeigt. Sollte dieser nicht korrekt sein, bitte das Programm schlie\'dfen, verschieben und erneut starten damit der Pfad korrekt ist.
                \par 
                \par \b 2.)\b0  Den richtigen Standarddrucker ausw\'e4hlen. Auf diesem wird sp\'e4ter gedruckt. Falls der Standarddrucker ge\'e4ndert wird w\'e4hrend das Programm l\'e4uft, 
                \par muss der Drucker per Klick auf 'aktualisieren' neu gesetzt werden.
                \par 
                \par \b 3.) \b0 Es muss eine Inv-Art ausgew\'e4hlt werden. Der Speicherort spielt keine Rolle. Die Datei kann also auch Zentral abgelegt werden. 
                \par 
                \par \b 4.)\b0  Die zu verarbeitende MDE-TXT Datei ausw\'e4hlen: entweder per Klick auf 'MDE-TXT ausw\'e4hlen' oder 'letzte MDE-TXT'. 
                \par Beim ersteren muss eine Datei vom Benutzer ausgew\'e4hlt werden. Bei der zweiten M\'f6glichkeit wird automatisch die letzte gespeicherte MDE-Textdatei ausgew\'e4hlt. 
                \par 
                \par \b 5.)\b0  Mit Klick auf 'Verarbeiten' wird die Datei intern verarbeitet, in jeweils den Ordner '..\\Erledigt' und '..\\Original' verschoben und die Aktion wird in der Datei '..\\verarbeitet.txt' Protokolliert. 
                \par Falls der Haken 'automatisch drucken' gesetzt ist, wird die Kontrollliste direkt gedruckt. Ansonsten kann jederzeit die zuletzt verarbeitete Datei gedruckt werden ('Drucken'-Button)\fs24 
                \par 
                \par \b\fs20 ==============================\b0\fs24 
                \par \b\fs16 
                \par R\'fcckfragen an: Patrick Stickel - OMEGA SORG GmbH - patrick.stickel@omega-sorg.de\b0\f1\fs17 
                \par }";
        }
    }
}


