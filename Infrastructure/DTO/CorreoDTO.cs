using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Core.DTO;
using System.IO;
using System.Web;
using Infrastructure.Utils;

namespace Infrastructure.DTO
{
    public class CorreoDTO
    {
        public string asunto { get; set; }
        public string cuerpo { get; set; }
        public List<string> correos { get; set; }
        public List<string> correosCC { get; set; }
        public List<Attachment> archivos { get; set; }
        #region Configuracion
        protected MailMessage contenedor;
        protected SmtpClient smptConfig;
        protected const string IPHOST = "184.95.60.10";
        protected const string CORREOHOST = "Mail.construplan.com.mx";
        protected const string DE = "alertas.sigoplan@construplan.com.mx";
        protected const string PASS = "feFA$YUc38";
        protected const int puerto = 587;
        protected const string iniCuerpo =
        @"<html><head>
            <style>
                table {
                    font-family: arial, sans-serif;
                    border-collapse: collapse;
                    width: 100%;
                }
                td, th {
                    border: 1px solid #dddddd;
                    text-align: left;
                    padding: 8px;
                }
                tr:nth-child(even) {
                    background-color: #dddddd;
                }
            </style>
        </head>
        <body lang=ES-MX link='#0563C1' vlink='#954F72'><div class=WordSection1>";
        protected const string finCuerpo =
            @"<p class=MsoNormal><o:p>&nbsp;</o:p></p><p class=MsoNormal><o:p>&nbsp;</o:p></p>
            <p class=MsoNormal>
                Se informa que esta es un correo autogenerado por el sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx</a>). No es necesario dar una respuesta. Gracias.
            </p>
            </div></body></html>";
        #endregion
        public CorreoDTO()
        {
            asunto = string.Empty;
            cuerpo = string.Empty;
            correos = new List<string>();
            correosCC = new List<string>();
            archivos = new List<Attachment>();
            #region Smtp
            smptConfig = new SmtpClient();
            #endregion
            #region Mensaje
            contenedor = new MailMessage();
            contenedor.IsBodyHtml = true;
            contenedor.From = new MailAddress(DE);
            #endregion
            
        }
        public bool Enviar()
        {
            if(correos == null || correos.Count == 0 || string.IsNullOrEmpty(asunto) || string.IsNullOrEmpty(cuerpo))
            {
                return false;
            }
            correos = getFinalMailList(correos);
            correos.ForEach(correo => contenedor.To.Add(new MailAddress(correo)));

            foreach (var item in correosCC)
            {
                contenedor.CC.Add(new MailAddress(item));
            }

            archivos.ForEach(archivo => contenedor.Attachments.Add(archivo));
            contenedor.Subject = asunto;
            contenedor.Body = iniCuerpo + cuerpo + finCuerpo;
            smptConfig.Send(contenedor);
            smptConfig.Dispose();
            return true;
        }
        private List<string> getFinalMailList(List<string> listaActual)
        {
            List<string> listaFinal = new List<string>();
            var _context = vSesiones.sesionUsuarioDTO.correosVinculados;
            var usuarioFiltrada = from actual in listaActual
                                  where !string.IsNullOrEmpty(actual)
                                  select actual;
            var vinculados = from vinculado in _context
                             where usuarioFiltrada.Contains(vinculado.principalCorreo)
                             select vinculado.vinculadoCorreo;
            listaActual.AddRange(vinculados);
            listaFinal.AddRange(usuarioFiltrada);
            return listaFinal.GroupBy(g => g).Select(s => s.Key).ToList();
        }
    }
}
