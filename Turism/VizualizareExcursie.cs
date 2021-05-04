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
    public partial class VizualizareExcursie : Form
    {
        public VizualizareExcursie()
        {
            InitializeComponent();
        }

        private void DateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            //
        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        string connection_path = Form1.connection_path;
        OleDbConnection connection = null;
        string query = "";
        OleDbCommand cmd = null;

        private void VizualizareExcursie_Load(object sender, EventArgs e)
        {
            connection = new OleDbConnection(connection_path);
            connection.Open();
            // incarcam imagini data grid view in tab-ul Planificari(Nume, DataStart, DataStop, Frecventa, Ziua) din Turism.Planificari
            OleDbDataAdapter dA = new OleDbDataAdapter();
            DataSet dS = new DataSet();
            query = "SELECT Nume, DataStart, DataStop, Frecventa, Ziua FROM Localitati, Planificari WHERE Localitati.IDLocalitate=Planificari.IDLocalitate";
            cmd = new OleDbCommand(query, connection);

            dA = new OleDbDataAdapter(query, connection);
            dS = new DataSet();
            dA.Fill(dS, "Planificari");
            BindingSource bs = new BindingSource(dS, "Planificari");
            dataGridView.DataSource = bs;


            cmd.ExecuteNonQuery();
            connection.Close();

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            // genereaza excurise
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            DateTime d1 = dateTimePicker1.Value;
            DateTime d2 = dateTimePicker2.Value;
            query = "SELECT Nume, DataStart, DataStop FROM Planificari, Localitati WHERE Planificari.IDLocalitate=Localitati.IDLocalitate AND Frecventa='ocazional'";
            connection.Open();
            cmd = new OleDbCommand(query, connection);
            IDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                //MessageBox.Show(dr[1].ToString());
                DateTime d3 = Convert.ToDateTime(dr[1].ToString());
                DateTime d4 = Convert.ToDateTime(dr[2].ToString());
                DateTime dMin;
                if (d2.CompareTo(d4) < 0) dMin = d2;
                else dMin = d4;
                DateTime dMax;
                if (d1.CompareTo(d3) > 0) dMax = d1;
                else dMax = d3;
                //MessageBox.Show("dMax = " + dMax + "\n" + "dMin =" + dMin);
                if (dMax.CompareTo(dMin) <= 0)
                {
                    // se intersecteaza
                    dataGridView1.Rows.Add(dr[0], dMax.ToShortDateString(), dMin.ToShortDateString());
                    DateTime x = dMax;
                    while(x.CompareTo(dMin)<=0)
                    {
                        dataGridView2.Rows.Add(dr[0], x.ToShortDateString());
                        x = x.AddDays(1);
                        //MessageBox.Show(x.ToShortDateString());
                    }
                    //MessageBox.Show("dMax = " + dMax + "\n" + "dMin =" + dMin);
                }
                else
                {
                    //MessageBox.Show("nu se intersecteaza!");
                }
            }
            dr.Close();
            connection.Close();
        }
    }
}
