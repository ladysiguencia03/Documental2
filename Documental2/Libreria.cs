using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using Spire.Pdf;
using Spire.Pdf.Annotations;
using Spire.Pdf.Annotations.Appearance;
using Spire.Pdf.Graphics;
using PdfAppearance = Spire.Pdf.Annotations.Appearance.PdfAppearance;
using PdfImage = Spire.Pdf.Graphics.PdfImage;
using PdfTemplate = Spire.Pdf.Graphics.PdfTemplate;
using System.Drawing;
using System.IO;
using iTextSharp.text.pdf;
using System.Net.Mime;
using PdfFont = Spire.Pdf.Graphics.PdfFont;

namespace Documental2
{
    public static class Libreria
    {
        public static Spire.Pdf.PdfDocument putReadOnly(Spire.Pdf.PdfDocument pdfdocument)
        {
            //Spire.Pdf.PdfDocument pdfdocument = new Spire.Pdf.PdfDocument(filePath);
            for (int i = 0; i < pdfdocument.Pages.Count; i++)
            {
                System.Drawing.Image image = pdfdocument.SaveAsImage(i);
                image.Save(string.Format("ImagePage{0}.png", i), System.Drawing.Imaging.ImageFormat.Png);
            }

            Spire.Pdf.PdfDocument pdfDoc = new Spire.Pdf.PdfDocument();
            

            for (int i = 0; i < pdfdocument.Pages.Count; i++)
            {
                PdfMargins margin = new PdfMargins();
                margin.Top = 1;// unitCvtr.ConvertUnits(2.54f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
                margin.Bottom = 1;// margin.Top;
                margin.Left = 1;// unitCvtr.ConvertUnits(3.17f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
                margin.Right = 1;// margin.Left;


                //pdfimage.PhysicalDimension.Width
                System.Drawing.Image image = Image.FromFile(string.Format("ImagePage{0}.png", i));
                PdfImage pdfImg = PdfImage.FromImage(image);
                //PdfPageBase page = pdfDoc.Pages.Add();
                
                PdfPageBase page = pdfDoc.Pages.Add(PdfPageSize.A4, margin);

                float width = pdfImg.PhysicalDimension.Width; //pdfImg.Width * 0.3f;
                float height = pdfImg.PhysicalDimension.Height; //pdfImg.Height * 0.3f;
                                                                //float x = (page.Canvas.ClientSize.Width - width);


                PdfFont font = new PdfFont(PdfFontFamily.Helvetica, 8f);
                
                PdfSolidBrush brush1 = new PdfSolidBrush(Color.White);
        
            PdfStringFormat format = new PdfStringFormat(PdfTextAlignment.Center);
            page.Canvas.DrawString("Solo Lectura", font, brush1, 0, 0, format);

                page.Canvas.DrawImage(pdfImg, 200, 0, 0, width, height);
                //page.Canvas.DrawImage(pdfImg, 0, 0);
                image.Dispose();
            }
            //string PdfFilename = "prueba.pdf";
            //pdfDoc.SaveToFile(PdfFilename);
            for (int i = 0; i < pdfdocument.Pages.Count; i++)
            {

                //File.Delete(string.Format("ImagePage{0}.png", i));

            }
            return pdfDoc;
            //MessageBox.Show("Archivo convertido en solo lectura");
            

        }
        
        public static Boolean SendMail(string asunto, string cuerpo, string destinatario, string fileName)
        {
            try
            {
                var remitente = ConfigurationManager.AppSettings["remitente"];
                var clave = ConfigurationManager.AppSettings["clave"];
                var displayName = ConfigurationManager.AppSettings["nombreMostrar"];
                var cliente = ConfigurationManager.AppSettings["cliente"];
                var puerto = ConfigurationManager.AppSettings["puerto"];
                var piePagina = ConfigurationManager.AppSettings["piePagina"];
                var imagen = ConfigurationManager.AppSettings["imagen"];
                imagen = imagen == "" ? "logo.jpg": imagen;
                piePagina = piePagina.Replace("\r\n", "<br />").Replace("\n", "<br />").Replace("\r", "<br />");

                string htmlBody = string.Format("<html><body>{0}<br />{1}<br /> <img src=\"cid:Footer\" width=\"200\" height=\"300\" /></body></html>", cuerpo, piePagina);
                AlternateView avHtml = AlternateView.CreateAlternateViewFromString(htmlBody, Encoding.Default, MediaTypeNames.Text.Html);

                // Create a LinkedResource object for each embedded image
                //LinkedResource header = new LinkedResource("Header.jpg", MediaTypeNames.Image.Jpeg);
                //header.ContentId = "Header";

                LinkedResource footer = new LinkedResource(imagen, MediaTypeNames.Image.Jpeg);
                footer.ContentId = "Footer";

                //avHtml.LinkedResources.Add(header);
                avHtml.LinkedResources.Add(footer);
                //Configuración del Mensaje
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(cliente); //"smtp.gmail.com"
                //Especificamos el correo desde el que se enviará el Email y el nombre de la persona que lo envía
                mail.From = new MailAddress(remitente, displayName, Encoding.UTF8);
                //Aquí ponemos el asunto del correo
                mail.Subject = asunto; //"Prueba de Envío de Correo"
                //Aquí ponemos el mensaje que incluirá el correo
                //mail.Body = string.Format("<p>{0}</p><br/><br/><img src='{1}'>", cuerpo, global::Documental2.Properties.Resources.logo_copia.ToString()); 
                mail.AlternateViews.Add(avHtml);
                //Especificamos a quien enviaremos el Email, no es necesario que sea Gmail, puede ser cualquier otro proveedor
                mail.To.Add(destinatario);
                //Si queremos enviar archivos adjuntos tenemos que especificar la ruta en donde se encuentran
                mail.Attachments.Add(new Attachment(fileName));
                   
                //Configuracion del SMTP
                SmtpServer.Port = int.Parse(puerto); //Puerto que utiliza Gmail para sus servicios
                //Especificamos las credenciales con las que enviaremos el mail
                SmtpServer.EnableSsl = false;
                SmtpServer.UseDefaultCredentials = false;

                SmtpServer.Credentials = new System.Net.NetworkCredential(remitente, clave);
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static Boolean putClave(string fileName, string clave)
        {
            Spire.Pdf.PdfDocument pdfDoc = new Spire.Pdf.PdfDocument();
            using (var input = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var output = new FileStream(fileName.Replace(".pdf", "2.pdf"), FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var reader = new PdfReader(input);
                PdfEncryptor.Encrypt(reader, output, true, clave, clave, PdfWriter.ALLOW_PRINTING);



            }
            pdfDoc.LoadFromFile(fileName.Replace(".pdf", "2.pdf"), clave);
            pdfDoc.SaveToFile(fileName);
            pdfDoc.Close();
            pdfDoc.Dispose();
            //File.Delete(fileName.Replace(".pdf", "2.pdf"));
            return true;
        }

        public static Spire.Pdf.PdfDocument putMarkWater(Spire.Pdf.PdfDocument pdfDoc, string texto)
        {
            for (int i = 0; i < pdfDoc.Pages.Count; i++)
            {
                PdfPageBase page = pdfDoc.Pages[i];

                PdfTilingBrush brush = new PdfTilingBrush(new SizeF(page.Canvas.ClientSize.Width / 2, page.Canvas.ClientSize.Height / 3));
                brush.Graphics.SetTransparency(0.3f);
                brush.Graphics.Save();
                brush.Graphics.TranslateTransform(brush.Size.Width / 2, brush.Size.Height / 2);
                brush.Graphics.RotateTransform(-45);
                brush.Graphics.DrawString(texto, new Spire.Pdf.Graphics.PdfFont(PdfFontFamily.Helvetica, 24), PdfBrushes.Blue, 0, 0, new PdfStringFormat(PdfTextAlignment.Center));
                brush.Graphics.Restore();
                brush.Graphics.SetTransparency(1);
                page.Canvas.DrawRectangle(brush, new RectangleF(new PointF(0, 0), page.Canvas.ClientSize));
            }
            return pdfDoc;

        }
        public static Spire.Pdf.PdfDocument putSign(Spire.Pdf.PdfDocument pdfDoc, string fileImage)
        {
            PdfPageBase page = pdfDoc.Pages[0];

            PdfRubberStampAnnotation loStamp = new PdfRubberStampAnnotation(new RectangleF(new PointF(-5, -5), new SizeF(60, 60)));
            PdfAppearance loApprearance = new PdfAppearance(loStamp);
            PdfImage image = PdfImage.FromFile(fileImage);


            PdfTemplate template = new PdfTemplate(160, 160);
            template.Graphics.DrawImage(image, 0, 0);
            loApprearance.Normal = template;

            loStamp.Appearance = loApprearance;

            page.AnnotationsWidget.Add(loStamp);

            return pdfDoc;


        }

      

    }


}
