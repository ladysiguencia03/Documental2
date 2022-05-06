using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Documental2
{
    public partial class FrmConfiguracion : Form
    {
        public FrmConfiguracion()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FrmConfiguracion_Load(object sender, EventArgs e)
        {
            txtRemitente.Text = ConfigurationManager.AppSettings["remitente"];
            txtClave.Text = ConfigurationManager.AppSettings["clave"];
            txtNombre.Text = ConfigurationManager.AppSettings["nombreMostrar"];
            txtSmtp.Text = ConfigurationManager.AppSettings["cliente"];
            txtPuerto.Value = int.Parse(ConfigurationManager.AppSettings["puerto"]);
            txtPiePagina.Text = ConfigurationManager.AppSettings["piePagina"];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["remitente"].Value = txtRemitente.Text;
            config.AppSettings.Settings["clave"].Value = txtClave.Text;
            config.AppSettings.Settings["nombreMostrar"].Value = txtNombre.Text;
            config.AppSettings.Settings["cliente"].Value = txtSmtp.Text;
            config.AppSettings.Settings["puerto"].Value = txtPuerto.Value.ToString();
            config.AppSettings.Settings["piePagina"].Value = txtPiePagina.Text;
            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("appSettings");
            MessageBox.Show("Configuraciones guardadas con exito!");
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
