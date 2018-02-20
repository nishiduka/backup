using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.IO;

namespace Backup
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            cmbEscolha.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String servidor = mskServer.Text.Replace(" ", "");
            String usuario = txtUser.Text.Trim();
            String senha = mskPassword.Text;
            String banco = txtDataBase.Text;
            String arquivo = txtDirectory.Text;

            try
            {
                string constring = "server=" + servidor + ";user=" + usuario + ";pwd=" + senha + ";database=" + banco + ";";

                constring += "charset=utf8;convertzerodatetime=true;";

                using (MySqlConnection conn = new MySqlConnection(constring))
                {
                    conn.Close();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            conn.Open();

                            if (cmbEscolha.SelectedItem == "Backup")
                                mb.ExportToFile(arquivo);
                            else
                                mb.ImportFromFile(arquivo);
                            conn.Close();
                            MessageBox.Show("Sucess");
                        }
                    }
                }
            }
            catch(Exception erro)
            {
                MessageBox.Show("Erro: \n" + erro);
            }
                
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (cmbEscolha.SelectedItem == "Backup")
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Sql|*.sql";
                saveFileDialog1.Title = "Save backup in";

                string dia = DateTime.Now.Day.ToString();
                string mes = DateTime.Now.Month.ToString();
                string ano = DateTime.Now.Year.ToString();
                string hora = DateTime.Now.ToLongTimeString().Replace(":", "");
                string nomeDoArquivo = ano + "-" + mes + "-" + dia + "_" + hora;
                saveFileDialog1.InitialDirectory = @"C:\Backup\";

                if (saveFileDialog1.CheckPathExists)
                    Directory.CreateDirectory(saveFileDialog1.InitialDirectory);

                saveFileDialog1.FileName = nomeDoArquivo;

                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    txtDirectory.Text = saveFileDialog1.FileName;
            }
            else
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "Sql|*.sql";
                openFileDialog1.Title = "Select a Sql File";

                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    txtDirectory.Text = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(mskServer.Text.Replace(" ",""));
        }
    }
}
