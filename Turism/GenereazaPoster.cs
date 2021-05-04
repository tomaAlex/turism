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
    public partial class GenereazaPoster : Form
    {
        public GenereazaPoster()
        {
            InitializeComponent();
        }

        string connection_path = Form1.connection_path;
        OleDbConnection connection = null;
        string query = "";

        private void GenereazaPoster_Load(object sender, EventArgs e)
        {
            connection = new OleDbConnection(connection_path);
            connection.Open();
            // trebuie sa fie incarcate numele localitatilor din Turism.Localitati.Nume
            query = "SELECT Nume FROM Localitati";
            OleDbCommand command = new OleDbCommand(query, connection);
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string localitate = reader[0].ToString();
                comboBox1.Items.Add(localitate);
            }
            reader.Close();
            connection.Close();
            // se face vector nul
            for (int i = 0; i < 100; i++) imagineAdaugata[i] = 0;
        }

        int nrImagini = 0;
        int[] imagineAdaugata = new int[100];

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            // a fost aleasa alta localitate deci trebuie sa fie schimbate imaginile
            connection = new OleDbConnection(connection_path);
            connection.Open();
            string localitateSelectata = comboBox1.SelectedItem.ToString();
            // trebuie sa facem rost de IDLocalitate din Turism.Localitati.IDLocalitate
            query = "SELECT IDLocalitate FROM Localitati WHERE Nume='" + localitateSelectata + "'";
            OleDbCommand command = new OleDbCommand(query, connection);
            OleDbDataReader reader = command.ExecuteReader();
            int idLocalitateSelectata = 1;
            if (reader.Read())
            {
                idLocalitateSelectata = int.Parse(reader[0].ToString());
            }
            //MessageBox.Show(idLocalitateSelectata.ToString());
            // cu ajutorul id-ului localitatii, scoatem caile catre imagini din Turism.Imagini.CaleFisier //// Imagini.IDLocalitate
            string[] CaleImagine = new string[11];
            // golire valori vechi din listBox1 si din comboBox2
            nrImagini = 0;
            listBox1.Items.Clear();
            comboBox2.Items.Clear();
            query = "SELECT CaleFisier FROM Imagini WHERE IDLocalitate=" + idLocalitateSelectata;
            command = new OleDbCommand(query, connection);
            OleDbDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                comboBox2.Items.Add(dataReader[0].ToString());
            }
            dataReader.Close();
            connection.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            // se adauga imaginea selectata in comboBox1, in listBox1
            if (nrImagini < 10)
            {
                if (comboBox2.Items.Count > 0 && imagineAdaugata[comboBox2.SelectedIndex]==0)
                {
                    string caleDeAdaugat = comboBox2.SelectedItem.ToString();
                    //MessageBox.Show(caleDeAdaugat);
                    listBox1.Items.Add(caleDeAdaugat);
                    imagineAdaugata[comboBox2.SelectedIndex] = 1;
                    nrImagini++;
                }
                else if (imagineAdaugata[comboBox2.SelectedIndex] == 1)
                {
                    MessageBox.Show("Imagine deja adaugata!");
                }
            }
            else MessageBox.Show("Prea multe poze :(");
        }

        string CalePoster = "";

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalePoster = listBox1.SelectedItem.ToString();
            pictureBox1.Load(CalePoster);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (CalePoster != "")
            {
                // se salveaza poster-ul in format png, avand denumirea luata din titlul textBox1
                string denumireFisier = textBox1.Text;
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = denumireFisier;
                saveFileDialog.FileName = denumireFisier;
                saveFileDialog.Filter = "PNG File| *.png";
                saveFileDialog.AddExtension = true;
                saveFileDialog.SupportMultiDottedExtensions = true;
                saveFileDialog.ShowDialog();
                Image bmpImageToConvert = Image.FromFile(CalePoster);
                Image bmpNewImage = new Bitmap(bmpImageToConvert.Width, bmpImageToConvert.Height);
                Graphics gfxNewImage = Graphics.FromImage(bmpNewImage);
                gfxNewImage.DrawImage(bmpImageToConvert, new Rectangle(0, 0, bmpNewImage.Width, bmpNewImage.Height), 0, 0, bmpImageToConvert.Width, bmpImageToConvert.Height, GraphicsUnit.Pixel);
                gfxNewImage.Dispose();
                bmpImageToConvert.Dispose();
                bmpNewImage.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }
            else MessageBox.Show("selecteaza o imagine!");
        }

       
    }
}
