using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;

namespace Turism
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static string connection_path = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=Turism.accdb;";
        OleDbConnection connection = new OleDbConnection(connection_path);
        string query = "";
        int idLocalitate = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            query = "DELETE * FROM Localitati";
            connection.Open();
            OleDbCommand command = new OleDbCommand(query, connection);
            command.ExecuteNonQuery();
            query = "DELETE * FROM Imagini";
            command = new OleDbCommand(query, connection);
            command.ExecuteNonQuery();
            query = "DELETE * FROM Planificari";
            command = new OleDbCommand(query, connection);
            command.ExecuteNonQuery();
            connection.Close();
            idLocalitate = 0;
        }

        int nrLinii = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            // initializare
            FolderBrowserDialog fereastra = new FolderBrowserDialog();
            MessageBox.Show("alegeti calea catre folder-ul cu imagini");
            fereastra.ShowDialog();
            string caleImagini = fereastra.SelectedPath;
            MessageBox.Show(caleImagini);
            //string caleImagini = "E:\\Eugen\\Turism\\Turism\\bin\\Debug\\Imagini";
            connection.Open();
            char[] separator = {'*'};
            string linie = "";
            string calePlanificariTXT = "planificari.txt";
            StreamReader sr = new StreamReader(calePlanificariTXT);
            string[] itemi;
            while (sr.EndOfStream == false)
            {
                linie = sr.ReadLine();
                nrLinii++;
                itemi = linie.Split(separator);

                idLocalitate++;
                //MessageBox.Show(idLocalitate.ToString());

                query = "INSERT INTO Localitati (Nume, IDLocalitate) VALUES ('" + itemi[0].Trim() + "', " + idLocalitate + ")";
                OleDbCommand cmd = new OleDbCommand(query, connection);
                cmd.ExecuteNonQuery();
                if (itemi[1] == " ocazional ")
                {
                    //MessageBox.Show(itemi[2].Trim() + " " + itemi[3].Trim());
                    //itemi[1] = itemi[1].Replace('.', '/');
                    //itemi[2] = itemi[2].Replace('.', '/');
                    //MessageBox.Show(itemi[1] + " " + itemi[2].Trim());
                    query = "INSERT INTO Planificari (Frecventa, DataStart, DataStop, IDLocalitate) VALUES ('" + itemi[1].Trim() + "', '" + itemi[2].Trim() + "', '" + itemi[3].Trim() + "', " + idLocalitate + ")";
                    OleDbCommand command = new OleDbCommand(query, connection);
                    command.ExecuteNonQuery();
                    for (int i = 4; i < itemi.Length; i++)
                    {
                        string caleFinalaImagine = caleImagini + "\\" + itemi[i].Trim();
                        query = "INSERT INTO Imagini(IDLocalitate, CaleFisier) VALUES(" + idLocalitate + ", '" + caleFinalaImagine + "')";
                        command = new OleDbCommand(query, connection);
                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    // lunar sau anual
                    query = "INSERT INTO Planificari (Frecventa, Ziua, IDLocalitate) VALUES ('" + itemi[1].Trim() + "', " + int.Parse(itemi[2].Trim()) + ", " + idLocalitate + ");";
                    OleDbCommand command = new OleDbCommand(query, connection);
                    command.ExecuteNonQuery();
                    for (int i = 3; i < itemi.Length; i++)
                    {
                        string caleFinalaImagine = caleImagini + "\\" + itemi[i].Trim();
                        query = "INSERT INTO Imagini(IDLocalitate, CaleFisier) VALUES(" + idLocalitate + ", '" + caleFinalaImagine + "')";
                        command = new OleDbCommand(query, connection);
                        command.ExecuteNonQuery();
                    }
                }
            }
            //MessageBox.Show(nrLinii.ToString());
            connection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // genereaza poster
            GenereazaPoster genereazaPoster = new GenereazaPoster();
            genereazaPoster.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // vizualizare excursie
            VizualizareExcursie vizualizareExcursie = new VizualizareExcursie();
            vizualizareExcursie.Show();
        }
    }
}
