﻿using iTextSharp.text.pdf;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Spire.Pdf;
using Spire.Pdf.Annotations;
using Spire.Pdf.Annotations.Appearance;
using Spire.Pdf.Graphics;
using PdfAppearance = Spire.Pdf.Annotations.Appearance.PdfAppearance;
using PdfImage = Spire.Pdf.Graphics.PdfImage;
using PdfTemplate = Spire.Pdf.Graphics.PdfTemplate;
using PdfDocument = Spire.Pdf.PdfDocument;

namespace Documental2
{
    public partial class FrmMenu : Form
    {
        private bool esSoloLectura = false;
        private bool esFirma = false;
        private bool esHipervinculo = false;
        private bool esClave = false;
        private bool esMarcaAgua = false;
        private bool esIcono = false;
        private string textoMarcaAgua = "";
        private string clave = "";
        private string textHipervinculo = "";
        private string urlHipervinculo = "";
        private string fileNameImage = "";
        private Spire.Pdf.PdfDocument pdfDoc;
        public FrmMenu()
        {
            pdfDoc = new Spire.Pdf.PdfDocument();
            InitializeComponent();
        }

        private void LblTitulo_Click(object sender, EventArgs e)
        {

        }

        private void btnArchivos_Click(object sender, EventArgs e)
        {

        }

        private void webEditor_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void FrmMenu_Load(object sender, EventArgs e)
        {

        }

        private void pctClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pctMinimice_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void seleccionarArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void lstFiles_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (lstFiles.SelectedIndices.Count <= 0)
            {
                return;
            }
            int intselectedindex = lstFiles.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                string text = (string)lstFiles.Items[intselectedindex].Tag;
                PdfDocument pdf = new PdfDocument();
               
                //pdf.LoadFromFile(text);
                //pdf.SaveToFile(text.Replace(".pdf",".html"), FileFormat.HTML);

                webEditor.Navigate(text);
                //htmlEditor.OpenFile(text.Replace(".pdf", ".html"));
                this.Tag = (string)lstFiles.Items[intselectedindex].Tag;

                //do something
                //MessageBox.Show(listView1.Items[intselectedindex].Text); 
            }
            //MessageBox.Show("pdf " + folderBrowserDialog1.SelectedPath + "\\" + curItem);
            //webEditor.Url = fbdDirectory.SelectedPath + "\\" + curItem;
            //webEditor.LoadUrl(folderBrowserDialog1.SelectedPath + "\\" + curItem);
        }

        private void seleccionarDirectorioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fbdDirectory.ShowDialog() == DialogResult.OK)
            {
                string[] listFiles = Directory.GetFiles(fbdDirectory.SelectedPath, "*.pdf");
                ListViewItem lstItem;
                foreach (string file in listFiles)
                {
                    lstItem = new ListViewItem();
                    lstItem.Text = Path.GetFileName(file);
                    lstItem.Tag = file;
                    lstFiles.Items.Add(lstItem);

                }
            }
            lstFiles.Columns[0].Width = lstFiles.Width;
        }

        private void seleccionarArchivosToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void agregarIconoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Tag == null || this.Tag.ToString().Equals(""))
            {
                MessageBox.Show("Debe Seleccionar un archivo");
                return;
            }
            TextWriter tw = new StreamWriter("desktop.ini");
            tw.WriteLine("[.ShellClassInfo]");
            tw.WriteLine("IconResource=");
            tw.Close();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ofdFiles.InitialDirectory = "C:\\";
            // filtro de archivos.
            ofdFiles.Filter = "Documentos (*.pdf)|*.pdf";
            ofdFiles.Multiselect = true;    

            // codigo para abrir el cuadro de dialogo
            if (ofdFiles.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string[] listFiles = ofdFiles.FileNames;
                    ListViewItem lstItem;
                    foreach (string file in listFiles)
                    {
                        lstItem = new ListViewItem();
                        lstItem.Text = Path.GetFileName(file);
                        lstItem.Tag = file;
                        lstFiles.Items.Add(lstItem);
                    }
                    lstFiles.Columns[0].Width = lstFiles.Width;

                }
                catch (Exception)
                {

                    throw;
                }
            }

        }

        private void button1_MouseMove(object sender, MouseEventArgs e)
        {
            btnArchivos.BackColor = LblTitulo.ForeColor;
            btnArchivos.ForeColor = LblTitulo.BackColor;
        }

        private void btnArchivos_MouseLeave(object sender, EventArgs e)
        {
            btnArchivos.BackColor = LblTitulo.BackColor;
            btnArchivos.ForeColor = LblTitulo.ForeColor;
        }

       
        private void btnLectura_Click(object sender, EventArgs e)
        {
            if (this.Tag == null || this.Tag.ToString().Equals(""))
            {
                MessageBox.Show("Debe Seleccionar un archivo");
                return;
            }
            esSoloLectura = true;
            MessageBox.Show("Solo Lectura generado con exito");

        }

        private void btnFirma_Click(object sender, EventArgs e)
        {
            if (this.Tag == null || this.Tag.ToString().Equals(""))
            {
                MessageBox.Show("Debe Seleccionar un archivo");
                return;
            }
            ofdFiles.InitialDirectory = "C:\\";
            // filtro de archivos.
            ofdFiles.Filter = "Documentos (*.jpg)|*.jpg";
            ofdFiles.Multiselect = true;

            // codigo para abrir el cuadro de dialogo
            if (ofdFiles.ShowDialog() == DialogResult.OK)
            {
                esFirma = true;
                fileNameImage = ofdFiles.FileName;

                MessageBox.Show("firma agregada de forma correcta");

            }
            else
            {
                MessageBox.Show("Debe seleccionar una imagen de firma");
            }

        }

        private void btnClave_Click(object sender, EventArgs e)
        {
            if (this.Tag == null || this.Tag.ToString().Equals(""))
            {
                MessageBox.Show("Debe Seleccionar un archivo");
                return;
            }
            FrmClaveDocumento f = new FrmClaveDocumento();
            if (f.ShowDialog() == DialogResult.OK)
            {
                clave = f.Clave;

                esClave = true;
                MessageBox.Show("Se agrego contraseña al documento");

            }
            else
            {
                MessageBox.Show("Contraseña no puede estar en blanco");
            }

        }

        private void webEditor_DocumentCompleted_1(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

       

        private void btnLectura_MouseLeave(object sender, EventArgs e)
        {
            btnLectura.BackColor = LblTitulo.BackColor;
            btnLectura.ForeColor = LblTitulo.ForeColor;
        }

        private void btnLectura_MouseMove(object sender, MouseEventArgs e)
        {
            btnLectura.BackColor = LblTitulo.ForeColor;
            btnLectura.ForeColor = LblTitulo.BackColor;
        }

        private void btnMarca_MouseLeave(object sender, EventArgs e)
        {
            btnMarca.BackColor = LblTitulo.BackColor;
            btnMarca.ForeColor = LblTitulo.ForeColor;
        }

        private void btnMarca_MouseMove(object sender, MouseEventArgs e)
        {
            btnMarca.BackColor = LblTitulo.ForeColor;
            btnMarca.ForeColor = LblTitulo.BackColor;
        }

        private void btnClave_MouseLeave(object sender, EventArgs e)
        {
            btnClave.BackColor = LblTitulo.BackColor;
            btnClave.ForeColor = LblTitulo.ForeColor;
        }

        private void btnClave_MouseMove(object sender, MouseEventArgs e)
        {
            btnClave.BackColor = LblTitulo.ForeColor;
            btnClave.ForeColor = LblTitulo.BackColor;
        }

        private void btnFirma_MouseLeave(object sender, EventArgs e)
        {
            btnFirma.BackColor = LblTitulo.BackColor;
            btnFirma.ForeColor = LblTitulo.ForeColor;
        }

        private void btnFirma_MouseMove(object sender, MouseEventArgs e)
        {
            btnFirma.BackColor = LblTitulo.ForeColor;
            btnFirma.ForeColor = LblTitulo.BackColor;
        }

       

        private void btnEnviarCorreo_Click(object sender, EventArgs e)
        {
            
            if (this.Tag == null || this.Tag.ToString().Equals(""))
            {
                MessageBox.Show("Debe Seleccionar un archivo");
                return;
            }
            FrmEnvioMail f = new FrmEnvioMail(this.Tag.ToString());
            //f.fileName = this.Tag.ToString();
            if (f.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Correo enviado con exito");
            }            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string filaNameOuput = this.Tag.ToString().Replace(".pdf", "Final.pdf");
            webEditor.Navigate("about:blank");
            pdfDoc = new Spire.Pdf.PdfDocument();
            pdfDoc.LoadFromFile(this.Tag.ToString()); 

            if (esFirma)
            {
                pdfDoc = Libreria.putSign(pdfDoc, fileNameImage);
            }
            
            if (esMarcaAgua)
            {

                pdfDoc = Libreria.putMarkWater(pdfDoc, textoMarcaAgua);
            }

            if (esSoloLectura)
            {
                pdfDoc = Libreria.putReadOnly(pdfDoc);
            }
            pdfDoc.SaveToFile(filaNameOuput);
            if (esClave)
            {
                Libreria.putClave(filaNameOuput, clave);
            }
            
           
            webEditor.Navigate(filaNameOuput);
            this.Tag = filaNameOuput;
            ListViewItem lstItem = new ListViewItem();
            lstItem.Text = Path.GetFileName(filaNameOuput);
            lstItem.Tag = filaNameOuput;
            lstFiles.Items.Add(lstItem);
        }

        private void btnMarca_Click(object sender, EventArgs e)
        {
            if (this.Tag == null || this.Tag.ToString().Equals(""))
            {
                MessageBox.Show("Debe Seleccionar un archivo");
                return;
            }
            FrmMarcaAgua f = new FrmMarcaAgua();
            if (f.ShowDialog() == DialogResult.OK)
            {
                textoMarcaAgua = f.Texto;
                esMarcaAgua = true;
                MessageBox.Show("Se agrego marca de agua al documento");
            }
            
        }

        private void btnConfiguracion_Click(object sender, EventArgs e)
        {
            FrmConfiguracion f = new FrmConfiguracion();
            if (f.ShowDialog() == DialogResult.OK)
            {
            }
        }
    }
}
