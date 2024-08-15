using Core.DTO;
using Core.DTO.Contabilidad.ControlPresupuestal;
using Core.DTO.Maquinaria.Catalogos;
using Core.DTO.Principal.Usuarios;
using Core.DTO.Utils;
using Core.DTO.Utils.Firmas;
using Core.Entity.Principal.Usuarios;
using Core.Enum.Contabilidad.ControlPresupuestal;
using Core.Enum.Principal;
using Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace Infrastructure.Utils
{
    public class GlobalUtils
    {
        public static dynamic LoopPropertiesObject(object object1, object object2)
        {
            foreach (PropertyInfo pro in ((Type)object1.GetType()).GetProperties())
            {
                if ((pro.GetGetMethod()).ReturnType == object2.GetType())
                    return pro;
            }
            return null;
        }


        public static List<FechasDTO> GetFechas(DateTime fecha)
        {

            List<FechasDTO> ListaFechas = new List<FechasDTO>();
            DateTime FechaInicio = new DateTime();
            DateTime FechaFin = new DateTime();

            for (int i = 1; i <= 52; i++)
            {
                if (i == 1)
                {
                    var diaSemana = (int)fecha.DayOfWeek;
                    FechaInicio = fecha.AddDays(-(int)fecha.DayOfWeek - 4);
                    int diasViernes = ((int)DayOfWeek.Tuesday - (int)fecha.DayOfWeek + 7) % 7;
                    FechaFin = fecha.AddDays(diasViernes);


                    if (FechaInicio >= DateTime.Now.Date && DateTime.Now <= FechaFin)
                    {
                        ListaFechas.Add(new FechasDTO
                        {
                            Value = i,
                            Text = FechaInicio.ToShortDateString() + " - " + FechaFin.ToShortDateString()

                        });
                    }


                }
                else
                {
                    var TempFecha = FechaFin.AddDays(1);

                    FechaInicio = TempFecha;
                    FechaFin = TempFecha.AddDays(6);

                    var fechaActual = DateTime.Now.Date.AddDays(-7).Date;
                    if (FechaInicio.Date >= fechaActual)
                    {
                        ListaFechas.Add(new FechasDTO
                        {
                            Value = i,
                            Text = FechaInicio.ToShortDateString() + " - " + FechaFin.ToShortDateString()

                        });
                    }
                }

            }
            return ListaFechas;

        }
        /// <summary>
        /// Enlista las quincenas del año de martes a miercoles
        /// </summary>
        /// <returns>Combobox</returns>
        public static List<FechasDTO> GetQuincenas(int anio)
        {
            var ListaFechas = new List<FechasDTO>();
            var FechaFin = new DateTime();
            var FechaInicio = new DateTime(anio, 01, 01);
            var anioActual = FechaInicio.Year;
            var diasSem = 7;
            var i = 0;
            FechaInicio = FechaInicio.AddDays(diasSem);
            while (FechaInicio.Year == anioActual)
            {
                int diasMiercoles = ((int)DayOfWeek.Wednesday - (int)FechaInicio.DayOfWeek + 7) % 7;
                FechaInicio = FechaInicio.AddDays(diasMiercoles);
                FechaFin = FechaInicio.AddDays(diasSem);
                int diasMartes = ((int)DayOfWeek.Tuesday - (int)FechaInicio.DayOfWeek + 7) % 7;
                FechaFin = FechaFin.AddDays(diasMartes);
                ListaFechas.Add(new FechasDTO()
                {
                    Value = ++i,
                    Text = FechaInicio.ToShortDateString() + " - " + FechaFin.ToShortDateString()
                });
                FechaInicio = FechaFin.AddDays(1);
            }
            return ListaFechas;
        }

        public static List<FechasDTO> GetQuincenasPorDia(int anio, int dia)
        {
            var ListaFechas = new List<FechasDTO>();
            var FechaFin = DateTime.Today;
            var FechaInicio = new DateTime(anio - 1, 12, dia);
            var anioActual = anio;
            var i = 0;
            while (FechaFin.Year == anioActual)
            {
                if (FechaInicio.Day == dia) FechaFin = (new DateTime(FechaInicio.Year, FechaInicio.Month, dia - 15)).AddMonths(1);
                else FechaFin = new DateTime(FechaInicio.Year, FechaInicio.Month, dia - 1);
                if (FechaFin.Year == anio) {
                    ListaFechas.Add(new FechasDTO()
                    {
                        Value = ++i,
                        Text = FechaInicio.ToShortDateString() + " - " + FechaFin.ToShortDateString()
                    });
                    if (FechaInicio.Day == dia) FechaInicio = (new DateTime(FechaInicio.Year, FechaInicio.Month, dia - 14)).AddMonths(1);
                    else FechaInicio = new DateTime(FechaInicio.Year, FechaInicio.Month, dia);
                }
            }
            return ListaFechas;
        }

        public static List<FechasDTO> GetQuincenasNormales(int anioBase)
        {
            var ListaFechas = new List<FechasDTO>();
            var listaAnios = new List<int>();
            listaAnios.Add(anioBase - 1);
            listaAnios.Add(anioBase);
            foreach (var anio in listaAnios)
            {


                //var FechaInicioTemp = new DateTime(anio, 11, 01);
                //var FechaInicio = new DateTime(anio, 01, 01);
                //var FechaFin = new DateTime(anio, 01, 01);
                var FechaInicioTemp = new DateTime(anio, 12, 01);
                var FechaInicio = new DateTime(anio, 01, 01);
                var FechaFin = new DateTime(anio, 01, 01);
                var anioActual = FechaInicio.Year;
                var diasSem = 14;
                var i = 0;
                if (FechaInicioTemp.Year == 2019)
                {
                    FechaInicio = FechaInicioTemp;
                }
                while (FechaInicio.Year == anioActual)
                {


                    var ss = new DateTime(FechaInicio.Year, FechaInicio.Month, 1);
                    var ultimoDia = ss.AddMonths(1).AddDays(-1);
                    var esPrimero = FechaInicio.Day;
                    if (esPrimero == 1)
                    {
                        FechaFin = FechaInicio.AddDays(diasSem);
                    }
                    else
                    {
                        FechaFin = ultimoDia;
                    }
                    ListaFechas.Add(new FechasDTO()
                    {
                        Value = ++i,
                        Text = FechaInicio.ToShortDateString() + " - " + FechaFin.ToShortDateString()
                    });
                    FechaInicio = FechaFin.AddDays(1);
                }
            }
            return ListaFechas;
        }
        public static IList<ComboDTO> ParseEnumToCombo<T>() where T : IConvertible
        {
            IList<ComboDTO> comboList = new List<ComboDTO>();
            Int32 valueItem = 0;
            string labelItem = null;
            if (typeof(T).IsEnum)
            {
                foreach (Enum item in typeof(T).GetEnumValues())
                {
                    valueItem = Convert.ToInt32(item);
                    labelItem = item.GetDescription();
                    comboList.Add(new ComboDTO() { Value = valueItem, Text = labelItem });
                }
                return comboList;
            }
            else
            {
                throw new Exception("La clase seleccionada no es del tipo enumerador.");
            }
        }
        public static List<Economico_MesDTO> ParseEnumToCtrlPresupuestal<T>() where T : IConvertible
        {
            List<Economico_MesDTO> comboList = new List<Economico_MesDTO>();
            Int32 valueItem = 0;
            string labelItem = null;
            if (typeof(T).IsEnum)
            {
                foreach (Enum item in typeof(T).GetEnumValues())
                {
                    valueItem = Convert.ToInt32(item);
                    labelItem = item.GetDescription();
                    comboList.Add(new Economico_MesDTO() { conceptoID = valueItem, concepto = labelItem });
                }
                return comboList;
            }
            else
            {
                throw new Exception("La clase seleccionada no es del tipo enumerador.");
            }
        }
        public static IList<ComboDTO> getFecha(int cantidad)
        {
            IList<ComboDTO> comboList = new List<ComboDTO>();

            int anio = DateTime.Now.Year + 1;


            for (int i = anio; i >= anio - cantidad; i--)
            {
                comboList.Add(new ComboDTO() { Value = i, Text = i.ToString() });
            }

            return comboList;
        }

        public static IList<ToValidDTO> getRelatedObjects(object objectToFind, object m)
        {
            IList<ToValidDTO> listResult = new List<ToValidDTO>();
            StringBuilder sb = new StringBuilder();

            foreach (var obj in m.GetType().GetProperties())
            {
                foreach (var property in obj.PropertyType.GetProperties())
                {
                    if (property.Name.Equals(objectToFind.GetType().Name))
                    {
                        sb.Append("Objeto: " + obj.PropertyType.Name + ", Propiedad: " + property.Name + ". \n");
                        listResult.Add(new ToValidDTO()
                        {
                            Object = obj,
                            Property = obj.PropertyType.Name
                        });
                    }
                }
            }
            return listResult;
        }

        public Expression<Func<T, bool>> CreateExpression<T>(string propertyName, object propertyValue)
        {
            var parameter = Expression.Parameter(typeof(T));
            return Expression.Lambda<Func<T, bool>>(Expression.Equal(Expression.Property(parameter, propertyName), Expression.Constant(propertyValue)));
        }

        public static bool Deployed()
        {
            return Convert.ToBoolean(ConfigurationSettings.AppSettings["deployed"].ToString());
        }

        public static string virtualFolder()
        {
            return ConfigurationSettings.AppSettings["virtualFolder"].ToString();
        }

        public static string getMensaje(int Actualizacion)
        {
            switch (Actualizacion)
            {
                case 1:
                    return "Registro guardado exitosamente";
                case 2:
                    return "Registro modificado exitosamente";
                case 3:
                    return "Registro fue deshabilidado exitosamente";
                case 4:
                    return "Registro fue habilidado nuevamete";
                default:
                    return "";
            }
        }

        public static dynamic getResolution()
        {
            var screen = Screen.PrimaryScreen.Bounds;
            return screen;
        }
        public static byte[] ConvertFileToByte(Stream file)
        {
            byte[] result;
            using (var streamReader = new MemoryStream())
            {
                file.CopyTo(streamReader);
                result = streamReader.ToArray();
            }
            return result; //return the byte data
        }

        public static adjuntoCorreoDTO setAdjunto(Byte[] archivo, string extArchivo, string nombreArchivo)
        {

            return new adjuntoCorreoDTO
            {
                archivo = archivo,
                extArchivo = extArchivo,
                nombreArchivo = nombreArchivo
            };

        }
        private static string getTypeArchivo(string p)
        {

            switch (p)
            {
                case ".xlsx":
                    return "application/vnd.ms-excel";
                case ".pdf":
                    return "application/pdf";

                default:
                    return "";
            }
        }
        public static string getExtencionArchivo(string p)
        {

            switch (p)
            {
                case "application/vnd.ms-excel":
                    return ".xlsx";
                case "application/pdf":
                    return ".pdf";
                case "image/jpeg":
                    return ".jpeg";
                case "image/jpg":
                    return ".jpg";
                case "image/png":
                    return ".png";
                default:
                    return "";
            }
        }

        public static bool SaveArchivo(HttpPostedFileBase archivo, string ruta)
        {
            var maxFileSize = 1024 * 20000;
            if (archivo.ContentLength <= 0)
                throw new Exception(string.Format("{0} está vacio", archivo.FileName));
            //if (archivo.ContentLength > maxFileSize)
            //    throw new Exception(string.Format("{0} es muy grande", archivo.FileName));
            using (Stream inputStream = archivo.InputStream)
            {
                using (var memoryStream = new MemoryStream())
                {
                    inputStream.CopyTo(memoryStream);
#if DEBUG
                    ruta = ruta.Replace("\\REPOSITORIO\\", "C:\\Proyectos\\");
#else
                    ruta = ruta.Replace("C:\\", "\\REPOSITORIO\\");
#endif
                    File.WriteAllBytes(ruta, memoryStream.ToArray());
                    return File.Exists(ruta);
                }
            }
        }
        public static bool SaveArchivo(HttpPostedFileBase archivo, string ruta, string nombre)
        {
            var maxFileSize = 1024 * 20000;
            if (archivo.ContentLength <= 0)
                throw new Exception(string.Format("{0} está vacio", archivo.FileName));
            //if (archivo.ContentLength > maxFileSize)
            //    throw new Exception(string.Format("{0} es muy grande", archivo.FileName));
            using (Stream inputStream = archivo.InputStream)
            {
                using (var memoryStream = new MemoryStream())
                {
                    inputStream.CopyTo(memoryStream);
#if DEBUG
                    ruta = ruta.Replace("\\REPOSITORIO\\", "C:\\Proyectos\\");
#else
                    ruta = ruta.Replace("C:\\", "\\REPOSITORIO\\");
#endif
                    var esRuta = VerificarExisteCarpeta(ruta, true);
                    if (esRuta)
                    {
                        File.WriteAllBytes(ruta + "\\" + nombre, memoryStream.ToArray());
                    }
                    return File.Exists(ruta);
                }
            }
        }
        /// <summary>
        /// Crea un archivo zip de alguna carpeta existente.
        /// </summary>
        /// <param name="rutaCarpeta">Ruta física del folder y su nombre.</param>
        /// <param name="rutaZip">Ruta física en donde se creará el zip y su nombre.</param>
        /// <returns>En caso de éxito, retorna la ruta física del zip creado.
        /// Si no, retorna una cadena vacía.</returns>
        public static bool ComprimirCarpeta(string rutaCarpeta, string rutaZip)
        {
            //rutaCarpeta: Ruta del folder y su nombre.                |||   Ejemplo: C:\Users\Usuario\Documents\NuevaCarpeta
            //////rutaZip: Ruta en donde se creará el zip y su nombre. |||   Ejemplo: C:\Users\Usuario\MyFolder\Zips\NuevaCarpeta.zip
            ZipFile.CreateFromDirectory(rutaCarpeta, rutaZip, CompressionLevel.Optimal, false);
            return File.Exists(rutaZip);
        }

        /// <summary>
        /// Crea una cadena que se identificará como firma digital válida en base a un conjunto de parámetros.
        /// </summary>
        /// <param name="objetoPrincipalID">Identificador de la entidad principal.</param>
        /// <param name="tipoDocumento">Identificador del documento para el cuál se creará la firma digital.</param>
        /// <param name="usuarioAutorizanteID">Identificador del usuario que está firmando.</param>
        /// <param name="tipoFirma">Identificador del tipo de firma.
        /// Si no se envía este parámetro se tomará como autorización por default.</param>
        /// <returns></returns>
        public static string CrearFirmaDigital(int objetoPrincipalID, DocumentosEnum tipoDocumento, int usuarioAutorizanteID, TipoFirmaEnum tipoFirma = TipoFirmaEnum.Autorizacion)
        {
            // Ejemplo: --126|23112018|1652|4|6031|A--
            try
            {
                var firma = new StringBuilder().Append(string.Format("--{0}|{1}|{2}|{3}|",
                    objetoPrincipalID,
                    DateTime.Now.ToString("ddMMyyyy|HHmm"),
                    (int)tipoDocumento,
                    usuarioAutorizanteID));
                switch (tipoFirma)
                {
                    case TipoFirmaEnum.Autorizacion:
                        firma.Append("A--");
                        break;
                    case TipoFirmaEnum.Rechazo:
                        firma.Append("R--");
                        break;
                    default:
                        firma.Append("A--");
                        break;
                }
                return firma.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string CrearFirmaDigitalConFecha(int objetoPrincipalID, DocumentosEnum tipoDocumento, int usuarioAutorizanteID, DateTime fecha, TipoFirmaEnum tipoFirma = TipoFirmaEnum.Autorizacion)
        {
            // Ejemplo: --126|23112018|1652|4|6031|A--
            try
            {
                var firma = new StringBuilder().Append(string.Format("--{0}|{1}|{2}|{3}|",
                    objetoPrincipalID,
                    fecha.ToString("ddMMyyyy|HHmm"),
                    (int)tipoDocumento,
                    usuarioAutorizanteID));
                switch (tipoFirma)
                {
                    case TipoFirmaEnum.Autorizacion:
                        firma.Append("A--");
                        break;
                    case TipoFirmaEnum.Rechazo:
                        firma.Append("R--");
                        break;
                    default:
                        firma.Append("A--");
                        break;
                }
                return firma.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// Crea el archivo dentro de un folder temporal y luego lo comprime.
        /// </summary>
        /// <param name="file">Archivo a comprimir.</param>
        /// <param name="fullPath">Ruta física en donde será guardado el archivo como .zip.</param>
        /// <returns>Returna true si el archivo zip fue creado correctamente.</returns>
        public static bool SaveCompressedFile(HttpPostedFileBase file, string fullPath)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fullPath);
            string filePath = Path.GetDirectoryName(fullPath);
            string tempFolderPath = Path.Combine(filePath, "TEMP");
            try
            {
                // Crea el folder temporal.
                Directory.CreateDirectory(tempFolderPath);

                string fileInTempFolderPath = Path.Combine(tempFolderPath, Path.GetFileName(fullPath));

                // Crea el archivo dentro del folder temporal.
                using (Stream inputStream = file.InputStream)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        inputStream.CopyTo(memoryStream);
                        File.WriteAllBytes(fileInTempFolderPath, memoryStream.ToArray());
                    }
                }

                string zipFilePath = Path.Combine(filePath, fileNameWithoutExtension + ".zip");

                // Crea el zip del folder temporal.
                ZipFile.CreateFromDirectory(tempFolderPath, zipFilePath, CompressionLevel.Optimal, false);

                // Elimina el folder temporal.
                Directory.Delete(tempFolderPath, true);

                return File.Exists(zipFilePath);
            }
            catch (Exception e)
            {
                // Si tira error, intenta eliminar el folder temporal.
                //List<string> lstCorreos = new List<string>();
                //lstCorreos.Add("omar.nunez@construplan.com.mx");
                string body = string.Empty;
                try
                {
                    if (Directory.Exists(tempFolderPath))
                        Directory.Delete(tempFolderPath, true);
                }
                catch (Exception) 
                {
                    body = string.Format("UsuarioID: {0}, File: {1}, Exception: {2}", vSesiones.sesionUsuarioDTO.id, file, e.Message);
                    //GlobalUtils.sendEmail("FileManager", body, lstCorreos);
                }
                //GlobalUtils.sendEmail("FileManager", body, lstCorreos);
                return false;
            }
        }

        public static bool SaveCompressedByteArray(byte[] file, string fullPath)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fullPath);
            string filePath = Path.GetDirectoryName(fullPath);
            string tempFolderPath = Path.Combine(filePath, "TEMP");
            try
            {
                // Crea el folder temporal.
                Directory.CreateDirectory(tempFolderPath);

                string fileInTempFolderPath = Path.Combine(tempFolderPath, Path.GetFileName(fullPath));

                // Crea el archivo dentro del folder temporal.
                //using (Stream inputStream = file.InputStream)
                //{
                //    using (var memoryStream = new MemoryStream())
                //    {
                //        inputStream.CopyTo(memoryStream);
                //        File.WriteAllBytes(fileInTempFolderPath, memoryStream.ToArray());
                //    }
                //}

                File.WriteAllBytes(fileInTempFolderPath, file);

                string zipFilePath = Path.Combine(filePath, fileNameWithoutExtension + ".zip");

                // Crea el zip del folder temporal.
                ZipFile.CreateFromDirectory(tempFolderPath, zipFilePath, CompressionLevel.Optimal, false);

                // Elimina el folder temporal.
                Directory.Delete(tempFolderPath, true);

                return File.Exists(zipFilePath);
            }
            catch (Exception)
            {
                // Si tira error, intenta eliminar el folder temporal.
                try
                {
                    if (Directory.Exists(tempFolderPath))
                    {
                        Directory.Delete(tempFolderPath, true);
                    }
                }
                catch (Exception) { }
                return false;
            }
        }

        /// <summary>
        /// Retorna el archivo seleccionado que esté dentro del ZIP.
        /// </summary>
        /// <param name="zipPath">Ruta física del archivo zip.</param>
        /// <param name="fileName">Nombre del archivo(con extensión) que está dentro del zip.</param>
        /// <returns>Retorna un stream del archivo seleccionado</returns>
        public static Stream GetFileFromZipAsStream(string zipPath, string fileName)
        {
            using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
            {
                using (var fileStream = archive.GetEntry(fileName).Open())
                {
                    var memoryStream = new MemoryStream();
                    fileStream.CopyTo(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    return memoryStream;
                }
            }
        }

        /// <summary>
        /// Crea el archivo seleccionado del zip proporcionado, en la ruta deseada.
        /// </summary>
        /// <param name="zipPath">Ruta del archivo zip (que contiene al archivo dentro).</param>
        /// <param name="fileName">Nombre del archivo dentro del zip.</param>
        /// <param name="newFilePath">La ruta en donde se creará el archivo extraído del zip.</param>
        /// <returns></returns>
        public static bool SaveFileFromZip(string zipPath, string fileName, string newFilePath)
        {
            using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
            {
                var fileInZip = archive.GetEntry(fileName);
                fileInZip.ExtractToFile(newFilePath);
                return File.Exists(newFilePath);
            }
        }

        public static Image byteArrayToImage(byte[] bytesArr)
        {
            using (MemoryStream memstr = new MemoryStream(bytesArr))
            {
                Image img = Image.FromStream(memstr);
                return img;
            }
        }

        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(imageIn, typeof(byte[]));
        }

        public static byte[] FixedSize(byte[] img, int Width, int Height)
        {
            Image imgPhoto = byteArrayToImage(img);
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height,
                              PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                             imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.White);
            grPhoto.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return ImageToByteArray(bmPhoto);
        }

        public static Stream GetFileAsStream(string pathFile)
        {

            using (var fileStream = new System.IO.FileStream(pathFile, FileMode.Open))
            {
                var memoryStream = new MemoryStream();
                fileStream.CopyTo(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return memoryStream;
            }

        }

        public static bool SaveArchivoByteArray(byte[] archivo, string ruta)
        {
            File.WriteAllBytes(ruta, archivo);
            return File.Exists(ruta);
        }

        #region Obtener nombre completo de usuario
        /// <summary>
        /// Retorna el nombre completo de un usuario (nombre +  apPaterno  + apMaterno) en base a una entidad tipo tblP_Usuario
        /// </summary>
        /// <param name="usuario">Entidad usuario del cual se quiere obtener el nombre completo.</param>
        /// <returns></returns>
        public static string ObtenerNombreCompletoUsuario(tblP_Usuario usuario)
        {
            string nombreCompleto = "";
            try
            {
                if (usuario == null)
                {
                    nombreCompleto = "INDEFINIDO";
                }

                var nombre = usuario.nombre != null ? usuario.nombre.Trim() : "";
                var apellidoPaterno = usuario.apellidoPaterno != null ? usuario.apellidoPaterno.Trim() : "";
                var apellidoMaterno = usuario.apellidoMaterno != null ? usuario.apellidoMaterno.Trim() : "";
                nombreCompleto = String.Format("{0} {1} {2}", nombre, apellidoPaterno, apellidoMaterno);
            }
            catch (Exception)
            {
                return "INDEFINIDO";
            }
            return nombreCompleto.Length > 0 ? nombreCompleto : "INDEFINIDO";
        }

        /// <summary>
        /// Retorna el nombre completo de un usuario (nombre +  apPaterno  + apMaterno) en base a una entidad tipo UsuarioDTO
        /// </summary>
        /// <param name="usuario">Entidad usuario del cual se quiere obtener el nombre completo.</param>
        /// <returns></returns>
        public static string ObtenerNombreCompletoUsuario(UsuarioDTO usuario)
        {
            string nombreCompleto = "";
            try
            {
                if (usuario == null)
                {
                    nombreCompleto = "INDEFINIDO";
                }

                var nombre = usuario.nombre != null ? usuario.nombre.Trim() : "";
                var apellidoPaterno = usuario.apellidoPaterno != null ? usuario.apellidoPaterno.Trim() : "";
                var apellidoMaterno = usuario.apellidoMaterno != null ? usuario.apellidoMaterno.Trim() : "";
                nombreCompleto = String.Format("{0} {1} {2}", nombre, apellidoPaterno, apellidoMaterno);
            }
            catch (Exception)
            {
                return "INDEFINIDO";
            }
            return nombreCompleto.Length > 0 ? nombreCompleto : "INDEFINIDO";
        }

        /// <summary>
        /// Retorna el nombre completo del usuario logueado actualmente (nombre +  apPaterno  + apMaterno)
        /// </summary>
        /// <returns></returns>
        public static string ObtenerNombreCompletoUsuarioActual()
        {
            string nombreCompleto = "";
            try
            {
                UsuarioDTO usuario = vSesiones.sesionUsuarioDTO;

                if (usuario == null)
                {
                    nombreCompleto = "INDEFINIDO";
                }

                var nombre = usuario.nombre != null ? usuario.nombre.Trim() : "";
                var apellidoPaterno = usuario.apellidoPaterno != null ? usuario.apellidoPaterno.Trim() : "";
                var apellidoMaterno = usuario.apellidoMaterno != null ? usuario.apellidoMaterno.Trim() : "";
                nombreCompleto = String.Format("{0} {1} {2}", nombre, apellidoPaterno, apellidoMaterno);
            }
            catch (Exception)
            {
                return "INDEFINIDO";
            }
            return nombreCompleto.Length > 0 ? nombreCompleto : "INDEFINIDO";
        }
        #endregion

        #region Correos
        public static bool sendEmail(string subject, string msg, List<string> emails)
        {
            bool result = true;
            try
            {
                var message = new MailMessage();

                var finalList = getFinalMailList(emails);
                foreach (var i in finalList)
                {
                    message.To.Add(new MailAddress(i));
                }



                string DE = "alertas.sigoplan@construplan.com.mx";
                string PASS = "feFA$YUc38";
                message.Subject = subject;
                message.Body = msg;
                message.IsBodyHtml = true;
                message.From = new MailAddress(DE);
                System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient("184.95.60.10");
                smtpMail.EnableSsl = false;
                smtpMail.UseDefaultCredentials = false;
                smtpMail.Host = "Mail.construplan.com.mx";
                smtpMail.Port = 587;
                smtpMail.Credentials = new System.Net.NetworkCredential(DE, PASS);

                smtpMail.Send(message);
                smtpMail.Dispose();
            }
            catch (Exception e)
            {
                var exs = e.Message;
                result = false;
            }
            return result;
        }
        public static bool sendEmailAdjuntoInMemory(string subject, string msg, List<string> emails, List<Byte[]> ListaArchivos)
        {
            bool result = true;
            
            var message = new MailMessage();
            var finalList = getFinalMailList(emails);
            foreach (var i in finalList)
            {
                message.To.Add(new MailAddress(i));
            }

            if (ListaArchivos != null)
            {
                message.Attachments.Add(new Attachment(new MemoryStream(ListaArchivos[0]), "Minuta.pdf", "application/pdf"));
                message.Attachments.Add(new Attachment(new MemoryStream(ListaArchivos[1]), "ListaAsistencia.pdf", "application/pdf"));
            }

            string DE = "alertas.sigoplan@construplan.com.mx";
            string PASS = "feFA$YUc38";
            message.Subject = subject;
            message.Body = msg;
            message.IsBodyHtml = true;
            message.From = new MailAddress(DE);
            System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient("184.95.60.10");
            smtpMail.EnableSsl = false;
            smtpMail.UseDefaultCredentials = false;
            smtpMail.Host = "Mail.construplan.com.mx";
            smtpMail.Port = 587;
            smtpMail.Credentials = new System.Net.NetworkCredential(DE, PASS);

            smtpMail.Send(message);
            smtpMail.Dispose();

            return result;
        }
        public static bool sendEmailAdjuntoInMemory2(string subject, string msg, List<string> emails, List<Byte[]> ListaArchivos)
        {
            bool result = true;
            try
            {
                var message = new MailMessage();
                var finalList = getFinalMailList(emails);
                foreach (var i in finalList)
                {
                    message.To.Add(new MailAddress(i, "", Encoding.UTF8));
                }

                if (ListaArchivos != null)
                {
                    var cantidadArchivo = ListaArchivos.Count();
                    switch (cantidadArchivo)
                    {
                        case 1:
                            message.Attachments.Add(new Attachment(new MemoryStream(ListaArchivos[0]), subject + ".xlsx", "application/vnd.ms-excel"));
                            break;
                        case 2:
                            message.Attachments.Add(new Attachment(new MemoryStream(ListaArchivos[0]), subject + ".xlsx", "application/vnd.ms-excel"));
                            message.Attachments.Add(new Attachment(new MemoryStream(ListaArchivos[1]), subject + ".pdf", "application/pdf"));
                            break;
                        default:
                            break;
                    }
                }

                string DE = "alertas.sigoplan@construplan.com.mx";
                string PASS = "feFA$YUc38";
                message.Subject = subject;
                message.Body = msg;
                message.IsBodyHtml = true;
                message.From = new MailAddress(DE);
                System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient("184.95.60.10");
                smtpMail.EnableSsl = false;
                smtpMail.UseDefaultCredentials = false;
                smtpMail.Host = "Mail.construplan.com.mx";
                smtpMail.Port = 587;
                smtpMail.Credentials = new System.Net.NetworkCredential(DE, PASS);

                smtpMail.Send(message);
                smtpMail.Dispose();

            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }
        public static bool sendMailWithFiles(string subject, string msg, List<string> emails, List<adjuntoCorreoDTO> ListaArchivos)
        {
            bool result = true;
            try
            {
                var message = new MailMessage();

                var finalList = getFinalMailList(emails);
                foreach (var i in finalList)
                {
                    message.To.Add(new MailAddress(i, "", Encoding.UTF8));
                }

                if (ListaArchivos != null)
                {

                    for (int i = 0; i < ListaArchivos.Count; i++)
                    {
                        message.Attachments.Add(new Attachment(new MemoryStream(ListaArchivos[i].archivo), ListaArchivos[i].nombreArchivo + ListaArchivos[i].extArchivo, getTypeArchivo(ListaArchivos[i].extArchivo)));
                    }
                }

                string DE = "alertas.sigoplan@construplan.com.mx";
                string PASS = "feFA$YUc38";
                message.Subject = subject;
                message.Body = msg;
                message.IsBodyHtml = true;
                message.From = new MailAddress(DE);
                System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient("184.95.60.10");
                smtpMail.EnableSsl = false;
                smtpMail.UseDefaultCredentials = false;
                smtpMail.Host = "Mail.construplan.com.mx";
                smtpMail.Port = 587;
                smtpMail.Credentials = new System.Net.NetworkCredential(DE, PASS);

                smtpMail.Send(message);
                smtpMail.Dispose();

            }
            catch (Exception e)
            {
                result = false;
                sendEmail("Error al intentar enviar el correo: " + subject, "Mensaje: " + msg + "<br><br><br>" + "Correos: " + JsonUtils.convertNetObjectToJson(emails) + "<br><br><br>" + "Exception: " + e.ToString(), new List<string> { "alertas.sigoplan@construplan.com.mx" });
            }

            return result;
        }
        public static bool sendEmailAdjuntoInMemory2(string subject, string msg, List<string> emails, List<Byte[]> ListaArchivos, string tipoFormato)
        {
            bool result = true;
            try
            {
                var message = new MailMessage();
                var finalList = getFinalMailList(emails);
                foreach (var i in finalList)
                {
                    if (i != null)
                    {
                        message.To.Add(new MailAddress(i));
                    }
                }

                if (ListaArchivos != null && ListaArchivos.Count() > 0)
                {
                    //agregado de archivo
                    //foreach (Byte[] archivo in ListaArchivos)
                    //{
                    //    //comprobamos si existe el archivo y lo agregamos a los adjuntos
                    //    message.Attachments.Add(new Attachment(new MemoryStream(archivo), "Minuta.pdf", "application/pdf"));
                    //}


                    message.Attachments.Add(new Attachment(new MemoryStream(ListaArchivos[0]), tipoFormato, "application/pdf"));
                    //message.Attachments.Add(new Attachment(new MemoryStream(ListaArchivos[0]), "FormatoCambios.pdf", "application/pdf"));
                }

                string DE = "alertas.sigoplan@construplan.com.mx";
                string PASS = "feFA$YUc38";
                message.Subject = subject;
                message.Body = msg;
                message.IsBodyHtml = true;
                message.From = new MailAddress(DE);
                System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient("184.95.60.10");
                smtpMail.EnableSsl = false;
                smtpMail.UseDefaultCredentials = false;
                smtpMail.Host = "Mail.construplan.com.mx";
                smtpMail.Port = 587;
                smtpMail.Credentials = new System.Net.NetworkCredential(DE, PASS);

                smtpMail.Send(message);
                smtpMail.Dispose();

            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }
        public static bool sendEmailAdjuntoInMemorySolicitudes(string subject, string msg, List<string> emails, List<Byte[]> ListaArchivos)
        {
            bool result = true;
            try
            {
                var message = new MailMessage();
                var finalList = getFinalMailList(emails);
                foreach (var i in finalList)
                {
                    message.To.Add(new MailAddress(i));
                }

                if (ListaArchivos != null)
                {
                    //agregado de archivo
                    //foreach (Byte[] archivo in ListaArchivos)
                    //{
                    //    //comprobamos si existe el archivo y lo agregamos a los adjuntos
                    //    message.Attachments.Add(new Attachment(new MemoryStream(archivo), "Minuta.pdf", "application/pdf"));
                    //}
                    int count = 1;
                    foreach (Byte[] archivo in ListaArchivos)
                    {
                        if ((ListaArchivos.IndexOf(archivo) == 0))
                        {
                            message.Attachments.Add(new Attachment(new MemoryStream(archivo), "SolicitudMaquinariayEquipo.pdf", "application/pdf"));
                        }
                        else
                        {
                            message.Attachments.Add(new Attachment(new MemoryStream(archivo), "doc" + count + ".pdf", "application/pdf"));
                            count++;
                        }

                    }

                }

                string DE = "alertas.sigoplan@construplan.com.mx";
                string PASS = "feFA$YUc38";
                message.Subject = subject;
                message.Body = msg;
                message.IsBodyHtml = true;
                message.From = new MailAddress(DE);
                System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient("184.95.60.10");
                smtpMail.EnableSsl = false;
                smtpMail.UseDefaultCredentials = false;
                smtpMail.Host = "Mail.construplan.com.mx";
                smtpMail.Port = 587;
                smtpMail.Credentials = new System.Net.NetworkCredential(DE, PASS);

                smtpMail.Send(message);
                smtpMail.Dispose();

            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }
        public static bool sendEmailAdjunto(string subject, string msg, List<string> emails, List<string> ListaArchivos)
        {
            bool result = true;
            try
            {
                var message = new MailMessage();
                var finalList = getFinalMailList(emails);
                foreach (var i in finalList)
                {
                    message.To.Add(new MailAddress(i));
                }

                if (ListaArchivos != null)
                {
                    //agregado de archivo
                    foreach (string archivo in ListaArchivos)
                    {
                        //comprobamos si existe el archivo y lo agregamos a los adjuntos
                        if (System.IO.File.Exists(@archivo))
                            message.Attachments.Add(new Attachment(@archivo));

                    }
                }

                string DE = "alertas.sigoplan@construplan.com.mx";
                string PASS = "feFA$YUc38";
                message.Subject = subject;
                message.Body = msg;
                message.IsBodyHtml = true;
                message.From = new MailAddress(DE);
                System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient("184.95.60.10");
                smtpMail.EnableSsl = false;
                smtpMail.UseDefaultCredentials = false;
                smtpMail.Host = "Mail.construplan.com.mx";
                smtpMail.Port = 587;
                smtpMail.Credentials = new System.Net.NetworkCredential(DE, PASS);

                smtpMail.Send(message);
                smtpMail.Dispose();
            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }
        public static bool sendEmailAdjuntoInMemorySend(string subject, string msg, List<string> emails, List<Byte[]> ListaArchivos, string nombreArchivo)
        {
            bool result = true;
            try
            {
                var message = new MailMessage();
                var finalList = getFinalMailList(emails);
                foreach (var i in finalList)
                {
                    message.To.Add(new MailAddress(i));
                }

                if (ListaArchivos != null)
                {
                    //agregado de archivo
                    //foreach (Byte[] archivo in ListaArchivos)
                    //{
                    //    //comprobamos si existe el archivo y lo agregamos a los adjuntos
                    //    message.Attachments.Add(new Attachment(new MemoryStream(archivo), "Minuta.pdf", "application/pdf"));
                    //}
                    message.Attachments.Add(new Attachment(new MemoryStream(ListaArchivos[0]), nombreArchivo + ".pdf", "application/pdf"));
                }

                string DE = "alertas.sigoplan@construplan.com.mx";
                string PASS = "feFA$YUc38";
                message.Subject = subject;
                message.Body = msg;
                message.IsBodyHtml = true;
                message.From = new MailAddress(DE);
                System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient("184.95.60.10");
                smtpMail.EnableSsl = false;
                smtpMail.UseDefaultCredentials = false;
                smtpMail.Host = "Mail.construplan.com.mx";
                smtpMail.Port = 587;
                smtpMail.Credentials = new System.Net.NetworkCredential(DE, PASS);

                smtpMail.Send(message);
                smtpMail.Dispose();

            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }
        public static bool sendEmailAdjuntoInMemorySendExcel(string subject, string msg, List<string> emails, List<Byte[]> ListaArchivos, string nombreArchivo)
        {
            bool result = true;
            try
            {
                var message = new MailMessage();
                var finalList = getFinalMailList(emails);
                foreach (var i in finalList)
                {
                    message.To.Add(new MailAddress(i));
                }

                if (ListaArchivos != null)
                {
                    //agregado de archivo
                    //foreach (Byte[] archivo in ListaArchivos)
                    //{
                    //    //comprobamos si existe el archivo y lo agregamos a los adjuntos
                    //    message.Attachments.Add(new Attachment(new MemoryStream(archivo), "Minuta.pdf", "application/pdf"));
                    //}
                    message.Attachments.Add(new Attachment(new MemoryStream(ListaArchivos[0]), nombreArchivo));
                }

                string DE = "alertas.sigoplan@construplan.com.mx";
                string PASS = "feFA$YUc38";
                message.Subject = subject;
                message.Body = msg;
                message.IsBodyHtml = true;
                message.From = new MailAddress(DE);
                System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient("184.95.60.10");
                smtpMail.EnableSsl = false;
                smtpMail.UseDefaultCredentials = false;
                smtpMail.Host = "Mail.construplan.com.mx";
                smtpMail.Port = 587;
                smtpMail.Credentials = new System.Net.NetworkCredential(DE, PASS);

                smtpMail.Send(message);
                smtpMail.Dispose();

            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }
        public static bool sendEmailAdjuntoLiberacionOH(string subject, string msg, List<string> emails, List<Byte[]> DiagramaGantt, List<Byte[]> ReporteEjecutivo, List<string> ListaArchivos, List<string> ListaNombres)
        {
            bool result = true;
            try
            {
                var message = new MailMessage();
                var finalList = getFinalMailList(emails);
                foreach (var i in finalList) { message.To.Add(new MailAddress(i)); }

                if (ReporteEjecutivo != null)
                {
                    //agregado de archivo
                    var i = 0;
                    foreach (Byte[] archivo in ReporteEjecutivo)
                    {
                        //comprobamos si existe el archivo y lo agregamos a los adjuntos
                        i++;
                        message.Attachments.Add(new Attachment(new MemoryStream(archivo), "REPORTE EJECUTIVO.pdf", "application/pdf"));
                    }
                }

                if (DiagramaGantt != null)
                {
                    //agregado de archivo
                    var i = 0;
                    foreach (Byte[] archivo in DiagramaGantt)
                    {
                        //comprobamos si existe el archivo y lo agregamos a los adjuntos
                        i++;
                        message.Attachments.Add(new Attachment(new MemoryStream(archivo), "DIAGRAMA DE GANTT.pdf", "application/pdf"));
                    }
                }

                if (ListaArchivos != null)
                {
                    for (int i = 0; i < ListaArchivos.Count; i++)
                    {
                        if (System.IO.File.Exists(@ListaArchivos[i]))
                        {
                            Attachment data = new Attachment(@ListaArchivos[i]);
                            data.Name = ListaNombres[i];
                            message.Attachments.Add(data);
                        }
                    }
                }

                string DE = "alertas.sigoplan@construplan.com.mx";
                string PASS = "feFA$YUc38";
                message.Subject = subject;
                message.Body = msg;
                message.IsBodyHtml = true;
                message.From = new MailAddress(DE);
                System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient("184.95.60.10");
                smtpMail.EnableSsl = false;
                smtpMail.UseDefaultCredentials = false;
                smtpMail.Host = "Mail.construplan.com.mx";
                smtpMail.Port = 587;
                smtpMail.Credentials = new System.Net.NetworkCredential(DE, PASS);

                smtpMail.Send(message);
                smtpMail.Dispose();

            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }
        public static bool sendEmailAdjuntoNombre(string subject, string msg, List<string> emails, List<string> ListaArchivos, List<string> ListaNombres)
        {
            bool result = true;
            try
            {
                var message = new MailMessage();
                var finalList = getFinalMailList(emails);
                foreach (var i in finalList)
                {
                    message.To.Add(new MailAddress(i));
                }
                if (ListaArchivos != null)
                {
                    for (int i = 0; i < ListaArchivos.Count; i++)
                    {
                        if (System.IO.File.Exists(@ListaArchivos[i]))
                        {
                            Attachment data = new Attachment(@ListaArchivos[i]);
                            data.Name = ListaNombres[i];
                            message.Attachments.Add(data);
                        }
                    }
                }
                string DE = "alertas.sigoplan@construplan.com.mx";
                string PASS = "feFA$YUc38";
                message.Subject = subject;
                message.Body = msg;
                message.IsBodyHtml = true;

                message.From = new MailAddress(DE);
                System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient("184.95.60.10");
                smtpMail.EnableSsl = false;
                smtpMail.UseDefaultCredentials = false;
                smtpMail.Host = "Mail.construplan.com.mx";
                smtpMail.Port = 587;
                smtpMail.Credentials = new System.Net.NetworkCredential(DE, PASS);

                smtpMail.Send(message);
                message.Attachments.Dispose();
                smtpMail.Dispose();
            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }
        public static bool sendEmailOCProv(string subject, string msg, List<string> emails, List<Byte[]> ListaArchivos, string nombreArchivo, string nombreArchivo2)
        {
            bool result = true;
            try
            {
                var message = new MailMessage();
                var finalList = getFinalMailList(emails);
                foreach (var i in finalList)
                {
                    message.To.Add(new MailAddress(i));
                }

                if (ListaArchivos != null)
                {
                    if (ListaArchivos.Count() == 2)
                    {
                        message.Attachments.Add(new Attachment(new MemoryStream(ListaArchivos[0]), nombreArchivo));
                        message.Attachments.Add(new Attachment(new MemoryStream(ListaArchivos[1]), nombreArchivo2 + ".pdf", "application/pdf"));
                    }
                    else
                    {
                        message.Attachments.Add(new Attachment(new MemoryStream(ListaArchivos[0]), nombreArchivo2 + ".pdf", "application/pdf"));
                    }
                }

                string DE = "compras.construplan@construplan.com.mx";
                string PASS = "F84$Rt45";
                message.Subject = subject;
                message.Body = msg;
                message.IsBodyHtml = true;
                message.From = new MailAddress(DE);
                System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient("184.95.60.10");
                smtpMail.EnableSsl = false;
                smtpMail.UseDefaultCredentials = false;
                smtpMail.Host = "Mail.construplan.com.mx";
                smtpMail.Port = 587;
                smtpMail.Credentials = new System.Net.NetworkCredential(DE, PASS);

                smtpMail.Send(message);
                smtpMail.Dispose();

            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }
        public static bool sendEmailLogo(string subject, string msg, List<string> emails)
        {
            bool result = true;

            try
            {
                var message = new MailMessage();
                var finalList = getFinalMailList(emails);

                foreach (var i in finalList)
                {
                    message.To.Add(new MailAddress(i));
                }

                message.Subject = subject;
                message.IsBodyHtml = true;
                message.From = new MailAddress("alertas.sigoplan@construplan.com.mx");

                #region Agregar Logo
                string htmlBody = "<html><body><img src=\"cid:filename\"></body></html>";
                AlternateView avHtml = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);

                LinkedResource inline = new LinkedResource(HttpContext.Current.Server.MapPath("\\Content\\img\\logo\\logoRESPALDO.jpg"), MediaTypeNames.Image.Jpeg);
                inline.ContentId = Guid.NewGuid().ToString();
                avHtml.LinkedResources.Add(inline);

                message.AlternateViews.Add(avHtml);

                Attachment att = new Attachment(HttpContext.Current.Server.MapPath("\\Content\\img\\logo\\logoRESPALDO.jpg"));
                att.ContentDisposition.Inline = true;
                #endregion

                message.Body = string.Format(@"<img src=""cid:{0}"" />", att.ContentId) + msg;

                System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient("184.95.60.10");
                smtpMail.EnableSsl = false;
                smtpMail.UseDefaultCredentials = false;
                smtpMail.Host = "Mail.construplan.com.mx";
                smtpMail.Port = 587;
                smtpMail.Credentials = new System.Net.NetworkCredential("alertas.sigoplan@construplan.com.mx", "feFA$YUc38");

                smtpMail.Send(message);
                smtpMail.Dispose();
            }
            catch (Exception e)
            {
                result = false;
            }

            return result;
        }

        private AlternateView GetEmbeddedImage(String filePath)
        {
            LinkedResource res = new LinkedResource(filePath);
            res.ContentId = Guid.NewGuid().ToString();
            string htmlBody = @"<img src='cid:" + res.ContentId + @"'/>";
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(res);
            return alternateView;
        }

        public static List<string> getFinalMailList(List<string> listaActual)
        {
            List<string> listaFinal = new List<string>();
            var _context = vSesiones.sesionUsuarioDTO.correosVinculados;
            var usuarioFiltrada = listaActual.Where(x => x != null && !x.Equals(""));
            var vinculados = _context.Where(x => usuarioFiltrada.Contains(x.principalCorreo)).Select(x => x.vinculadoCorreo).ToList();
            listaActual.AddRange(vinculados);


            listaFinal.AddRange(listaActual.Where(x => x != null && !x.Equals("")).Distinct());
            return listaFinal;
        }
        public static bool enviarCorreo(string subject, string msg, List<string> emails, List<string> ListaArchivos)
        {
            bool result = true;
            try
            {
                var message = new MailMessage();
                var finalList = getFinalMailList(emails);
                foreach (var i in finalList)
                {
                    message.To.Add(new MailAddress(i));
                }

                if (ListaArchivos != null)
                {
                    //agregado de archivo
                    foreach (string archivo in ListaArchivos)
                    {
                        //comprobamos si existe el archivo y lo agregamos a los adjuntos
                        if (System.IO.File.Exists(@archivo))
                            message.Attachments.Add(new Attachment(@archivo));

                    }
                }

                string DE = "alertas.sigoplan@construplan.com.mx";
                string PASS = "feFA$YUc38";
                message.Subject = subject;
                message.Body = msg;
                message.IsBodyHtml = true;
                message.From = new MailAddress(DE);
                System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient("184.95.60.10");
                smtpMail.EnableSsl = false;
                smtpMail.UseDefaultCredentials = false;
                smtpMail.Host = "Mail.construplan.com.mx";
                smtpMail.Port = 587;
                smtpMail.Credentials = new System.Net.NetworkCredential(DE, PASS);

                smtpMail.Send(message);
                smtpMail.Dispose();
            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }
        #endregion

        #region Visor documentos Doconut
        public static List<string> ObtenerExtensionesValidasVisor()
        {
            return new List<string>{
                ".DWG", ".DXF", ".DOC", ".DOCX", ".TXT", 
                ".EML", ".MSG", ".XLS", ".XLSX", ".ODS",
                ".PDF", ".PNG", ".BMP", ".JPG", ".JPEG",
                ".PSD", ".GIF"
                };
        }
        #endregion

        public static byte[] GetZipVariosDocumentos(List<Tuple<byte[], string>> byteArrayList)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    foreach (var file in byteArrayList)
                    {
                        var entry = archive.CreateEntry(file.Item2, CompressionLevel.Fastest);
                        using (var zipStream = entry.Open())
                        {
                            zipStream.Write(file.Item1, 0, file.Item1.Length);
                        }
                    }
                }

                return ms.ToArray();
            }
        }

        /// <summary>
        /// Verifica si una carpeta existe físicamente en el servidor. Si la condicipon de crear se manda true y la carpeta no existe, la creará.
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="crear"></param>
        /// <returns></returns>
        public static bool VerificarExisteCarpeta(string ruta, bool crear = false)
        {
            bool existe = false;
            try
            {
                existe = Directory.Exists(ruta);
                if (!existe && crear)
                {
                    Directory.CreateDirectory(ruta);
                    existe = true;
                }
            }
            catch (Exception e)
            {
                existe = false;
            }

            return existe;
        }

        /// <summary>
        /// Guarda un archivo tipo HttpPostedFileBase en la ruta especificada.
        /// </summary>
        /// <param name="archivo"></param>
        /// <param name="ruta"></param>
        /// <returns></returns>
        public static bool SaveHTTPPostedFile(HttpPostedFileBase archivo, string ruta)
        {
            try
            {
                byte[] data;

                using (Stream inputStream = archivo.InputStream)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }
                    data = memoryStream.ToArray();
                }

                File.WriteAllBytes(ruta, data);
            }
            catch (Exception)
            {
                return false;
            }

            return File.Exists(ruta);
        }
        public static Tuple<bool, Exception> SaveHTTPPostedFileValidacion(HttpPostedFileBase archivo, string ruta)
        {
            try
            {
                byte[] data;

                using (Stream inputStream = archivo.InputStream)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }
                    data = memoryStream.ToArray();
                }

                File.WriteAllBytes(ruta, data);
            }
            catch (Exception e)
            {
                return Tuple.Create(false, e);
            }

            return Tuple.Create(File.Exists(ruta), new Exception());
        }
        public static string getBase64FromNombreRuta(string ruta, string nombreArchivo)
        {
            var archivo = System.IO.File.ReadAllBytes(ruta);
            var base64 = Convert.ToBase64String(archivo);
            return "data:" + MimeMapping.GetMimeMapping(nombreArchivo) + ";base64," + base64;
        }
        public static string getMonthName(DateTime fecha)
        {
            string r = "";
            switch (fecha.Month)
            {
                case 1:
                    {
                        r = "Enero";
                    }
                    break;
                case 2:
                    {
                        r = "Febrero";
                    }
                    break;
                case 3:
                    {
                        r = "Marzo";
                    }
                    break;
                case 4:
                    {
                        r = "Abril";
                    }
                    break;
                case 5:
                    {
                        r = "Mayo";
                    }
                    break;
                case 6:
                    {
                        r = "Junio";
                    }
                    break;
                case 7:
                    {
                        r = "Julio";
                    }
                    break;
                case 8:
                    {
                        r = "Agosto";
                    }
                    break;
                case 9:
                    {
                        r = "Septiembre";
                    }
                    break;
                case 10:
                    {
                        r = "Octubre";
                    }
                    break;
                case 11:
                    {
                        r = "Nomviembre";
                    }
                    break;
                case 12:
                    {
                        r = "Diciembre";
                    }
                    break;
                default:
                    break;
            }
            return r;
        }
        public static PeriodoDTO GetPeriodoEntreFechas(System.DateTime fechaInicio, System.DateTime fechaFin)
        {
            PeriodoDTO obj = new PeriodoDTO();
            int anos = 0;
            int meses = 0;
            int dias = 0;
            int m = 0;

            anos = fechaFin.Year - fechaInicio.Year;
            if (fechaInicio.Month > fechaFin.Month)
            {
                anos = anos - 1;
            }
            if (fechaFin.Month < fechaInicio.Month)
            {
                meses = 12 - fechaInicio.Month + fechaFin.Month;
            }
            else
            {
                meses = fechaFin.Month - fechaInicio.Month;
            }
            if (fechaFin.Day < fechaInicio.Day)
            {
                meses = meses - 1;
                if (fechaFin.Month == fechaInicio.Month)
                {
                    anos = anos - 1;
                    meses = 11;
                }
            }
            dias = fechaFin.Day - fechaInicio.Day;
            if (dias < 0)
            {
                m = Convert.ToInt32(fechaFin.Month) - 1;
                if (m == 0)
                {
                    m = 12;
                }
                switch (m)
                {
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 8:
                    case 10:
                    case 12:
                        dias = 31 + dias;
                        break;
                    case 4:
                    case 6:
                    case 9:
                    case 11:
                        dias = 30 + dias;
                        break;
                    case 2:
                        if ((fechaFin.Year % 4 == 0 & fechaFin.Year % 100 != 0) | fechaFin.Year % 400 == 0)
                        {
                            dias = 29 + dias;
                        }
                        else
                        {
                            dias = 28 + dias;
                        }
                        break;
                }
            }
            obj.anio = anos;
            obj.mes = meses;
            obj.dia = dias;
            return obj;
        }

        public static Image LoadBase64(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }
            return image;
        }

        public static string toText(double value)
        {
            string Num2Text = "";
            if (value < 0)
            {
                value = value * -1;
            }
            value = Math.Truncate(value);
            if (value == 0) Num2Text = "CERO";
            else if (value == 1) Num2Text = "UNO";
            else if (value == 2) Num2Text = "DOS";
            else if (value == 3) Num2Text = "TRES";
            else if (value == 4) Num2Text = "CUATRO";
            else if (value == 5) Num2Text = "CINCO";
            else if (value == 6) Num2Text = "SEIS";
            else if (value == 7) Num2Text = "SIETE";
            else if (value == 8) Num2Text = "OCHO";
            else if (value == 9) Num2Text = "NUEVE";
            else if (value == 10) Num2Text = "DIEZ";
            else if (value == 11) Num2Text = "ONCE";
            else if (value == 12) Num2Text = "DOCE";
            else if (value == 13) Num2Text = "TRECE";
            else if (value == 14) Num2Text = "CATORCE";
            else if (value == 15) Num2Text = "QUINCE";
            else if (value < 20) Num2Text = "DIECI" + toText(value - 10);
            else if (value == 20) Num2Text = "VEINTE";
            else if (value < 30) Num2Text = "VEINTI" + toText(value - 20);
            else if (value == 30) Num2Text = "TREINTA";
            else if (value == 40) Num2Text = "CUARENTA";
            else if (value == 50) Num2Text = "CINCUENTA";
            else if (value == 60) Num2Text = "SESENTA";
            else if (value == 70) Num2Text = "SETENTA";
            else if (value == 80) Num2Text = "OCHENTA";
            else if (value == 90) Num2Text = "NOVENTA";
            else if (value < 100) Num2Text = toText(Math.Truncate(value / 10) * 10) + " Y " + toText(value % 10);
            else if (value == 100) Num2Text = "CIEN";
            else if (value < 200) Num2Text = "CIENTO " + toText(value - 100);
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = toText(Math.Truncate(value / 100)) + "CIENTOS";
            else if (value == 500) Num2Text = "QUINIENTOS";
            else if (value == 700) Num2Text = "SETECIENTOS";
            else if (value == 900) Num2Text = "NOVECIENTOS";
            else if (value < 1000) Num2Text = toText(Math.Truncate(value / 100) * 100) + " " + toText(value % 100);
            else if (value == 1000) Num2Text = "MIL";
            else if (value < 2000) Num2Text = "MIL " + toText(value % 1000);
            else if (value < 1000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000)) + " MIL";
                if ((value % 1000) > 0) Num2Text = Num2Text + " " + toText(value % 1000);
            }
            else if (value == 1000000) Num2Text = "UN MILLON";
            else if (value < 2000000) Num2Text = "UN MILLON " + toText(value % 1000000);
            else if (value < 1000000000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000000)) + " MILLONES ";
                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000) * 1000000);
            }
            else if (value == 1000000000000) Num2Text = "UN BILLON";
            else if (value < 2000000000000) Num2Text = "UN BILLON " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            else
            {
                Num2Text = toText(Math.Truncate(value / 1000000000000)) + " BILLONES";
                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            }
            return Num2Text;
        }

        public static bool senEmailAdjuntos(string subject, string msg, List<string> emails, Byte[] objArchivos, string nombreArchivo)
        {
            bool result = true;
            try
            {
                var message = new MailMessage();
                //var finalList = getFinalMailList(emails);
                foreach (var i in emails)
                {
                    message.To.Add(new MailAddress(i));
                }

                if (objArchivos != null)
                {
                    //agregado de archivo
                    //foreach (Byte[] archivo in ListaArchivos)
                    //{
                    //    //comprobamos si existe el archivo y lo agregamos a los adjuntos
                    //    message.Attachments.Add(new Attachment(new MemoryStream(archivo), "Minuta.pdf", "application/pdf"));
                    //}
                    message.Attachments.Add(new Attachment(new MemoryStream(objArchivos), nombreArchivo + ".pdf", "application/pdf"));
                }

                string DE = "expediente@construplan.com.mx";
                string PASS = "8tKt234e";
                message.Subject = subject;
                message.Body = msg;
                message.IsBodyHtml = true;
                message.From = new MailAddress(DE);
                System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient("184.95.60.10");
                smtpMail.EnableSsl = false;
                smtpMail.UseDefaultCredentials = false;
                smtpMail.Host = "Mail.construplan.com.mx";
                smtpMail.Port = 587;
                smtpMail.Credentials = new System.Net.NetworkCredential(DE, PASS);

                smtpMail.Send(message);
                smtpMail.Dispose();

            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }


        public static string cuerpoDeCorreoFormato(string titulo, string modulo, string asunto, string mensaje, string texto, string link, List<FirmantesDTO> lstFirmantes)
        {
            #region CONTENIDO HTML
            string html = @"<!doctype html>
<html xmlns='http://www.w3.org/1999/xhtml' xmlns:v='urn:schemas-microsoft-com:vml'
    xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:o='urn:schemas-microsoft-com:office:office'
    style='width:100%;font-family:Arial, sans-serif;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;padding:0;Margin:0'>

<head>
    <title> </title>
    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1'>
    <style type='text/css'>
        #outlook a {
            padding: 0;
        }
        table{
            border-color: rgb(230, 230, 230);
        }

        body {
            margin: 0;
            padding: 0;
            -webkit-text-size-adjust: 100%;
            -ms-text-size-adjust: 100%;
        }

        table,
        td {
            border-collapse: collapse;
            mso-table-lspace: 0pt;
            mso-table-rspace: 0pt;
        }

        img {
            border: 0;
            height: auto;
            line-height: 100%;
            outline: none;
            text-decoration: none;
            -ms-interpolation-mode: bicubic;
        }

        p {
            display: block;
            margin: 13px 0;
        }
    </style>
    <style type='text/css'>
        @media only screen and (min-width:480px) {
            .mj-column-per-100 {
                width: 100% !important;
                max-width: 100%;
            }

            .mj-column-per-50 {
                width: 50% !important;
                max-width: 50%;
            }
        }
    </style>
    <style type='text/css'>
        @media only screen and (max-width:480px) {
            table.mj-full-width-mobile {
                width: 100% !important;
            }

            td.mj-full-width-mobile {
                width: auto !important;
            }
        }
    </style>
</head>

<body style='background-color:#f4f4f4;'>
    <div style='background-color:#f4f4f4;'>
        <div style='background:#fff;background-color:#fff;margin:0px auto;max-width:900px;'>
            <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' 
                style='background:#fff;background-color:#fff;width:900px;'>
                <tbody>
                    <tr>
                        <td
                            style='direction:ltr;font-size:0px;padding:10px 0;padding-bottom:0px;padding-top:5px;text-align:center;'>
                            <div class='mj-column-per-100 mj-outlook-group-fix'
                                style='font-size:0px;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%;'>
                                <table border='0' cellpadding='0' cellspacing='0' role='presentation'
                                    style='vertical-align:top;' width='100%'>
                                    <tr>
                                        <td align='center'
                                            style='font-size:0px;padding:10px 25px;word-break:break-word;'>
                                            <table border='0' cellpadding='0' cellspacing='0' role='presentation'
                                                style='border-collapse:collapse;border-spacing:0px;'>
                                                <tbody>
                                                    <tr>
                                                        <img src='data:image/png;base64,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'/> </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>

                                </table>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <div style='background:#ffffff;background-color:#ffffff;margin:0px auto;max-width:900px;'>
            <table align='center' border='1' cellpadding='0' cellspacing='0' role='presentation'
                style='background:#ffffff;background-color:#ffffff;width:900px; border-color:  #e4e1e1;'>
                <tbody>
                    <tr>
                        <td style='direction:ltr;font-size:0px;padding:0px 0;padding-bottom:0px;;text-align:center;'>
                            <div class='mj-column-per-100 mj-outlook-group-fix'
                                style='font-size:0px;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%;'>
                               
                                <table border='0' cellpadding='0' cellspacing='0' role='presentation'
                                    style='vertical-align:top;background-color: #ffffff; background: #ffffff; width: 100%;'>
                                   
                                    <tr>
                                        <td align='center'
                                            style='font-size:0px;padding:10px 10px;padding-top:0px;padding-right:0px;padding-bottom:0px;padding-left:0px;word-break:break-word;'>
                                            <table border='0' cellpadding='0' cellspacing='0' role='presentation'
                                                style='border-collapse:collapse;border-spacing:0px;'>
                                                <tbody>
                                                    <tr>
                                                        <td style='width:900px;'>  </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='left' style='font-size:0px;padding:10px 25px;padding-top:10px;padding-bottom:10px;word-break:break-word;'>
                                            <div style='font-family:Arial, sans-serif;font-size:35px;line-height:22px;text-align:left;color:#2e3192;'>
                                              <p style='line-height: 25px; margin: 10px 0; text-align: center; color:#000; font-size:20px; font-family:Montserrat, sans-serif; font-weight: 700;'> " + modulo + @"</p>
                                              <p style='line-height: 25px; margin: 10px 0; text-align: center; color:#000; font-size:15px; font-family:Montserrat, sans-serif; font-weight: 500;'> </p>
                                            </div>
                                          </td>
                                    </tr>
                                    <tr>
                                        <td align='left'
                                            style='font-size:0px;padding:10px 25px;padding-bottom:10px;word-break:break-word;'>
                                            <div
                                                style='font-family:Arial, sans-serif;font-size:30px;line-height:22px;text-align:center;color:#2e3192;'>
                                                
                                               
                                                <p
                                                    style='line-height: 25px; margin: 10px 0; text-align: center; color:#424242; font-size:15px; font-family:Poppins, -apple-system, BlinkMacSystemFont, Segoe UI, Roboto, Helvetica Neue, Arial, sans-serif; font-weight: 600;'>
                                                    " + asunto + @"</p>



                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='left'
                                            style='font-size:0px;padding:0px 25px 0px 25px;word-break:break-word;'>
                                            <div
                                                style='font-family:Arial, sans-serif;font-size:14px;line-height:25px;text-align:center;color:#000; border-radius: 50px;'>
                                               
                                              
                                                
                                               
                                                <a href='" + link + @"'
                                                class='btn-btn-primary'
                                                style='font-family:Arial, sans-serif; border-radius: 5px; text-decoration: none;font-size:16px;color:#FFFFFF;border-style:solid;border-color:#548bdc;border-width:10px 30px 10px 30px;display:inline-block;background:#548bdc;border-radius:0px;font-weight:normal;font-style:normal;line-height:19px;width:auto;text-align:center; margin: botton 10px;'>Haz clic aquí</a>
                                            
                                               

                                                <br>
                                            </div>
                                        </td>

                                    </tr>
                                  
                                    <tr>
                                        <td style='direction:ltr;font-size:0px;padding:0px 0;padding-bottom:0px;;text-align:center;'>
                                            <div class='mj-column-per-100 mj-outlook-group-fix'
                                                style='font-size:0px;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%;'>
                                                <table border='0' cellpadding='0' cellspacing='0' role='presentation'
                                                    style='vertical-align:top;background-color:  #ffffff; background:  #ffffff; width: 100%;'>
                
                                                    <tr>
                                                        <td align='center'
                                                            style='font-size:0px;padding:10px 25px;padding-bottom:10px;word-break:break-word;'>
                                                            <div style='font-family:Arial, sans-serif;text-align:center;color:#424242;'>
                                                              
                
                                                                <p
                                                                    style='color: #424242; font-size:15px;font-family:Arial, sans-serif;'>
                                                                    " + titulo + @"
                                                                </p>
                                                                <p
                                                                    style='font-family:Arial, sans-serif;font-size:10px;line-height:10px;text-align:justify;color:#424242;'>
                                                                   " + mensaje + @"
                
                                                                </p>
     
                                                            </div>
                                                        </td>
                
                                                    </tr>
                                             
                
                
                                                </table>
                                            </div>
                
                                        </td>
                                    </tr>
                

                                </table>
                            </div>

                        </td>
                    </tr>

                </tbody>
            </table>
        </div>
       


    </div>

" + texto +

    @"<div
        style='background:#FF8000;background-color:#FF8000;margin:0px auto;max-width:900px;'>
        <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation'
            style='background:#FF8000;background-color:#FF8000;width:900px;'>
            <tbody>
                <tr>
                    <td style='direction:ltr;font-size:0px;padding:20px 0;text-align:center;'>
                        <div class='mj-column-per-100 mj-outlook-group-fix'
                            style='font-size:0px;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%;'>
                            <table border='0' cellpadding='0' cellspacing='0' role='presentation'
                                style='vertical-align:top; width: 100%;'>
                                <tr>
                                    <td align='center' style='font-size:0px;padding:10px 25px;word-break:break-word;'>
                                        <div
                                            style='font-family:Arial, sans-serif;font-size:13px;line-height:22px;text-align:center;color:#ffffff;'>
                                            Enviado automaticamente
                                            &nbsp;por&nbsp;<a style='color:#ffffff'
                                                href='" + link + @"'><b>
                                                   " + link + @"</b></a></div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>



</body>

</html>";
            #endregion
            return html;
        }
        public static string tablaFirmantes(List<FirmantesDTO> lstFirmantes)
        {
            string html = "";
            html += "<style>h3 {text-align: center;}table.dataTable tbody tr td, table thead tr th, table.dataTable, .dataTables_scrollBody {";
            html += "border: 0px solid #81bd72 !important;}table.dataTable thead {font-size: 15px;background-color: #81bd72;color: white;}";
            html += ".select2-container {width: 100% !important;}.seccion {padding: 15px 25px 15px 25px;margin: 10px 5px;background-color: white;";
            html += "border-radius: 4px 4px;box-shadow: 0 0 2px 0 rgba(0,0,0,0.14), 0 2px 2px 0 rgba(0,0,0,0.12), 0 1px 3px 0 rgba(0,0,0,0.2);}";
            html += ".my-card {position: absolute;left: 40%;top: -20px;border-radius: 50%;}#txtFechaInicio {background-color: #fff;}";
            html += "</style><br><table id='tblM_AutorizanteAdquisicion' class='table-bordered hover stripe order-column dataTable no-footer' role='grid'";
            html += "aria-describedby='tblM_AutorizanteAdquisicion_info'>";
            html += "<thead>";
            html += "<tr role='row'>";
            html += "<th class='sorting_disabled' rowspan='1' colspan='1'>Puesto</th>";
            html += "<th class='sorting_disabled' rowspan='1' colspan='1'>Nombre Completo</th>";
            html += "<th class='sorting_disabled' rowspan='1' colspan='1'>Fecha Autorizacion</th>";
            html += "<th class='sorting_disabled' rowspan='1' colspan='1>Firma</th>";
            html += "<th class='sorting_disabled' rowspan='1' colspan='1'>Estado de la firma</th>";
            html += "</tr>";
            html += "</thead>";
            html += "<tbody>";

            foreach (var item in lstFirmantes)
            {
                var a = item.estado == true ? "AUTORIZADO" : "PENDIENTE";
                html += "<tr>";
                html += "<td>" + item.puesto + "</td>";
                html += "<td>" + item.nombreCompleto + "</td>";
                html += "<td>" + item.Fecha + "</td>";
                html += "<td>" + item.Firma + "</td>";
                html += "<td>" + a + "</td>";
                html += "</tr>";
            }
            html += "</tbody>";
            html += "</table>";
            html += "</div>";
            return html;
        }



        //public static string tablaFirmantes(List<FirmantesDTO> lstFirmantes)
        //{
        //    string html = "";
        //    html += "<style>h3 {text-align: center;}table.dataTable tbody tr td, table thead tr th, table.dataTable, .dataTables_scrollBody {";
        //    html += "border: 0px solid #81bd72 !important;}table.dataTable thead {font-size: 15px;background-color: #81bd72;color: white;}";
        //    html += ".select2-container {width: 100% !important;}.seccion {padding: 15px 25px 15px 25px;margin: 10px 5px;background-color: white;";
        //    html += "border-radius: 4px 4px;box-shadow: 0 0 2px 0 rgba(0,0,0,0.14), 0 2px 2px 0 rgba(0,0,0,0.12), 0 1px 3px 0 rgba(0,0,0,0.2);}";
        //    html += ".my-card {position: absolute;left: 40%;top: -20px;border-radius: 50%;}#txtFechaInicio {background-color: #fff;}";
        //    html += "</style><br><table id='tblM_AutorizanteAdquisicion' class='table-bordered hover stripe order-column dataTable no-footer' role='grid'";
        //    html += "aria-describedby='tblM_AutorizanteAdquisicion_info' style='width: 0px;'>";
        //    html += "<thead>";
        //    html += "<tr role='row'>";
        //    html += "<th class='sorting_disabled' rowspan='1' colspan='1' style='width: 0px;'>Puesto</th>";
        //    html += "<th class='sorting_disabled' rowspan='1' colspan='1' style='width: 0px;'>Nombre Completo</th>";
        //    html += "<th class='sorting_disabled' rowspan='1' colspan='1' style='width: 0px;'>Fecha Autorizacion</th>";
        //    html += "<th class='sorting_disabled' rowspan='1' colspan='1' style='width: 0px;'>Firma</th>";
        //    html += "<th class='sorting_disabled' rowspan='1' colspan='1' style='width: 0px;'>Estado de la firma</th>";
        //    html += "</tr>";
        //    html += "</thead>";
        //    html += "<tbody>";

        //    foreach (var item in lstFirmantes)
        //    {
        //        var a = item.estado == true ? "AUTORIZADO" : "PENDIENTE";
        //        html += "<tr>";
        //        html += "<td>" + item.puesto + "</td>";
        //        html += "<td>" + item.nombreCompleto + "</td>";
        //        html += "<td>" + item.Fecha + "</td>";
        //        html += "<td>" + item.Firma + "</td>";
        //        html += "<td>" + a + "</td>";
        //        html += "</tr>";
        //    }
        //    html += "</tbody>";
        //    html += "</table>";
        //    html += "</div>";
        //    return html;
        //}

        public static string tablaFirmantesCtrlPptal(List<FirmantesDTO> lstFirmantes)
        {
            string html = "";
            //html += "<div><table id='tblM_AutorizanteAdquisicion' class='table-bordered hover stripe order-column dataTable no-footer' role='grid'";
            html += "<style>h3 {text-align: center;} table.dataTable tbody tr td, table thead tr th, table.dataTable, .dataTables_scrollBody {";
            html += "border: 2px solid #81bd72 !important;} td, th { border: 1px solid #000; !important } table.dataTable thead {font-size: 15px;background-color: #81bd72;color: white;} .styTD { background-color: #ff7979 !important; }";
            html += ".select2-container {width: 100% !important;} .styTDAutorizado { background-color: #7eff79 !important; } .seccion {padding: 15px 25px 15px 25px;margin: 10px 5px;background-color: white;";
            html += "border-radius: 4px 4px;box-shadow: 0 0 2px 0 rgba(0,0,0,0.14), 0 2px 2px 0 rgba(0,0,0,0.12), 0 1px 3px 0 rgba(0,0,0,0.2);}";
            html += ".my-card {position: absolute;left: 40%;top: -20px;border-radius: 50%;}#txtFechaInicio {background-color: #fff;}";
            html += "</style><br><table id='tblM_AutorizanteAdquisicion' class='table-bordered hover stripe order-column dataTable no-footer' role='grid'";
            html += "aria-describedby='tblM_AutorizanteAdquisicion_info' style='width: 0px;'>";
            html += "<thead>";
            html += "<tr role='row'>";
            html += "<th class='' rowspan='1' colspan='1' style='width: 0px;'>Nombre Completo</th>";
            html += "<th class='' rowspan='1' colspan='1' style='width: 0px;'>Fecha Autorización</th>";
            html += "<th class='' rowspan='1' colspan='1' style='width: 0px;'>Estado de la firma</th>";
            html += "</tr>";
            html += "</thead>";
            html += "<tbody>";

            foreach (var item in lstFirmantes)
            {
                html += "<tr>";
                html += "<td>" + item.nombreCompleto + "</td>";
                html += "<td>" + item.Fecha + "</td>";
                if (item.estatus == "RECHAZADO")
                    html += "<td class='styTD'>" + item.estatus + "</td>";
                else if (item.estatus == "AUTORIZADO")
                    html += "<td class='styTDAutorizado'>" + item.estatus + "</td>";
                else
                    html += "<td>" + item.estatus + "</td>";

                html += "</tr>";
            }
            html += "</tbody>";
            html += "</table>";
            html += "</div>";
            return html;
        }

        public static string sendEmailFormato(string titulo, string modulo, string asunto, string mensaje, string texto, string link, List<FirmantesDTO> lstFirmantes)
        {
            #region CONTENIDO HTML
            string html = @"<!doctype html>
            <html xmlns='http://www.w3.org/1999/xhtml' xmlns:v='urn:schemas-microsoft-com:vml' xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:o='urn:schemas-microsoft-com:office:office' style='width:100%;font-family:Arial, sans-serif;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;padding:0;Margin:0'>
            <head>
                <title> </title>
                <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                <meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1'>
                <style type='text/css'>
                    #outlook a {
                        padding: 0;
                    }

                    table{
                        border-color: rgb(230, 230, 230);
                    }

                    body {
                        margin: 0;
                        padding: 0;
                        -webkit-text-size-adjust: 100%;
                        -ms-text-size-adjust: 100%;
                    }

                    table, td {
                        border-collapse: collapse;
                        mso-table-lspace: 0pt;
                        mso-table-rspace: 0pt;
                    }

                    img {
                        border: 0;
                        height: auto;
                        line-height: 100%;
                        outline: none;
                        text-decoration: none;
                        -ms-interpolation-mode: bicubic;
                    }

                    p {
                        display: block;
                        margin: 13px 0;
                    }
                </style>
                <style type='text/css'>
                    @media only screen and (min-width:480px) {
                        .mj-column-per-100 {
                            width: 100% !important;
                            max-width: 100%;
                        }

                        .mj-column-per-50 {
                            width: 50% !important;
                            max-width: 50%;
                        }
                    }
                </style>
                <style type='text/css'>
                    @media only screen and (max-width:480px) {
                        table.mj-full-width-mobile {
                            width: 100% !important;
                        }

                        td.mj-full-width-mobile {
                            width: auto !important;
                        }   
                    }
                </style>
            </head>

            <body style='background-color:#f4f4f4;'>
                <div style='background-color:#f4f4f4;'>
                    <div style='background:#fff;background-color:#fff;margin:0px auto;max-width:600px;'>
                        <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' 
                            style='background:#fff;background-color:#fff;width:600px;'>
                            <tbody>
                                <tr>
                                    <td style='direction:ltr;font-size:0px;padding:10px 0;padding-bottom:0px;padding-top:5px;text-align:center;'>
                                        <div class='mj-column-per-100 mj-outlook-group-fix' style='font-size:0px;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%;'>
                                        <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top;' width='100%'>
                                <tr>
                                    <td align='center' style='font-size:0px;padding:10px 25px;word-break:break-word;'>
                                        <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='border-collapse:collapse;border-spacing:0px;'>
                                        <tbody>
                                <tr>
                                    <!--<img src='data:image/png;base64,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'/>-->
                                    </td>
                                </tr>
                                        </tbody>
                                        </table>
                                    </td>
                                </tr>

                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                    <div style='background:#ffffff;background-color:#ffffff;margin:0px auto;max-width:600px;'>
                        <table align='center' border='1' cellpadding='0' cellspacing='0' role='presentation'
                            style='background:#ffffff;background-color:#ffffff;width:600px; border-color:  #e4e1e1;'>
                            <tbody>
                                <tr>
                                    <td style='direction:ltr;font-size:0px;padding:0px 0;padding-bottom:0px;;text-align:center;'>
                                        <div class='mj-column-per-100 mj-outlook-group-fix'
                                            style='font-size:0px;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%;'>
                               
                                            <table border='0' cellpadding='0' cellspacing='0' role='presentation'
                                                style='vertical-align:top;background-color: #ffffff; background: #ffffff; width: 100%;'>
                                   
                                                <tr>
                                                    <td align='center'
                                                        style='font-size:0px;padding:10px 10px;padding-top:0px;padding-right:0px;padding-bottom:0px;padding-left:0px;word-break:break-word;'>
                                                        <table border='0' cellpadding='0' cellspacing='0' role='presentation'
                                                            style='border-collapse:collapse;border-spacing:0px;'>
                                                            <tbody>
                                                                <tr>
                                                                    <td style='width:600px;'>  </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align='left' style='font-size:0px;padding:10px 25px;padding-top:10px;padding-bottom:10px;word-break:break-word;'>
                                                        <div style='font-family:Arial, sans-serif;font-size:35px;line-height:22px;text-align:left;color:#2e3192;'>
                                                            <p style='line-height: 25px; margin: 10px 0; text-align: center; color:#000; font-size:35px; font-family:Montserrat, sans-serif; font-weight: 700;'> " + modulo + @"</p>
                                                            <p style='line-height: 25px; margin: 10px 0; text-align: center; color:#000; font-size:15px; font-family:Montserrat, sans-serif; font-weight: 500;'> </p>
                                                        </div>
                                                        </td>
                                                </tr>
                                                <tr>
                                                    <td align='left'
                                                        style='font-size:0px;padding:10px 25px;padding-bottom:10px;word-break:break-word;'>
                                                        <div
                                                            style='font-family:Arial, sans-serif;font-size:30px;line-height:22px;text-align:center;color:#2e3192;'>
                                                
                                               
                                                            <p
                                                                style='line-height: 25px; margin: 10px 0; text-align: center; color:#424242; font-size:15px; font-family:Poppins, -apple-system, BlinkMacSystemFont, Segoe UI, Roboto, Helvetica Neue, Arial, sans-serif; font-weight: 600;'>
                                                                " + asunto + @"</p>



                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align='left'
                                                        style='font-size:0px;padding:0px 25px 0px 25px;word-break:break-word;'>
                                                        <div
                                                            style='font-family:Arial, sans-serif;font-size:14px;line-height:25px;text-align:center;color:#000; border-radius: 50px;'>
                                               
                                              
                                                
                                               
                                                            <a href='" + link + @"'
                                                            class='btn-btn-primary'
                                                            style='font-family:Arial, sans-serif; border-radius: 5px; text-decoration: none;font-size:16px;color:#FFFFFF;border-style:solid;border-color:#548bdc;border-width:10px 30px 10px 30px;display:inline-block;background:#548bdc;border-radius:0px;font-weight:normal;font-style:normal;line-height:19px;width:auto;text-align:center; margin: botton 10px;'>Haz clic aquí</a>
                                            
                                               

                                                            <br>
                                                        </div>
                                                    </td>

                                                </tr>
                                  
                                                <tr>
                                                    <td style='direction:ltr;font-size:0px;padding:0px 0;padding-bottom:0px;;text-align:center;'>
                                                        <div class='mj-column-per-100 mj-outlook-group-fix'
                                                            style='font-size:0px;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%;'>
                                                            <table border='0' cellpadding='0' cellspacing='0' role='presentation'
                                                                style='vertical-align:top;background-color:  #ffffff; background:  #ffffff; width: 100%;'>
                
                                                                <tr>
                                                                    <td align='center'
                                                                        style='font-size:0px;padding:10px 25px;padding-bottom:10px;word-break:break-word;'>
                                                                        <div style='font-family:Arial, sans-serif;text-align:center;color:#424242;'>
                                                              
                
                                                                            <p
                                                                                style='color: #424242; font-size:15px;font-family:Arial, sans-serif;'>
                                                                                " + titulo + @"
                                                                            </p>
                                                                            <p
                                                                                style='font-family:Arial, sans-serif;font-size:10px;line-height:10px;text-align:justify;color:#424242;'>
                                                                                " + mensaje + @"
                
                                                                            </p>
                                                                ";


            if (lstFirmantes.Count() != 0)
            {
                html += tablaFirmantes(lstFirmantes);
            }
            html += @"
                                                                        </div>
                                                                    </td>
                
                                                                </tr>
                                             
                
                
                                                            </table>
                                                        </div>
                
                                                    </td>
                                                </tr>
                

                                            </table>
                                        </div>

                                    </td>
                                </tr>

                            </tbody>
                        </table>
                    </div>
       


                </div>

                <div
                    style='background:#FF8000;background-color:#FF8000;margin:0px auto;max-width:600px;'>
                    <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation'
                        style='background:#FF8000;background-color:#FF8000;width:600px;'>
                        <tbody>
                            <tr>
                                <td style='direction:ltr;font-size:0px;padding:20px 0;text-align:center;'>
                                    <div class='mj-column-per-100 mj-outlook-group-fix'
                                        style='font-size:0px;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%;'>
                                        <table border='0' cellpadding='0' cellspacing='0' role='presentation'
                                            style='vertical-align:top; width: 100%;'>
                                            <tr>
                                                <td align='center' style='font-size:0px;padding:10px 25px;word-break:break-word;'>
                                                    <div
                                                        style='font-family:Arial, sans-serif;font-size:13px;line-height:22px;text-align:center;color:#ffffff;'>
                                                        Enviado automaticamente
                                                        &nbsp;por&nbsp;<a style='color:#ffffff'
                                                            href='" + link + @"'><b>
                                                                " + link + @"</b></a></div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>



            </body>

            </html>";
            #endregion
            return html;
        }


        private static string getTypeArchivoReclutamientos(string p)
        {

            switch (p)
            {
                case ".xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".pdf":
                    return "application/pdf";

                default:
                    string mimeType = MimeMapping.GetMimeMapping(p);

                    return mimeType;
            }
        }


        public static bool sendMailWithFilesReclutamientos(string subject, string msg, List<string> emails, List<adjuntoCorreoDTO> ListaArchivos)
        {
            bool result = true;
            try
            {
                var message = new MailMessage();

                var finalList = getFinalMailList(emails);
                foreach (var i in finalList)
                {
                    message.To.Add(new MailAddress(i, "", Encoding.UTF8));
                }

                if (ListaArchivos != null)
                {

                    for (int i = 0; i < ListaArchivos.Count; i++)
                    {
                        message.Attachments.Add(new Attachment(new MemoryStream(ListaArchivos[i].archivo), ListaArchivos[i].nombreArchivo + ListaArchivos[i].extArchivo, getTypeArchivoReclutamientos(ListaArchivos[i].extArchivo)));

                    }
                }

                string DE = "alertas.sigoplan@construplan.com.mx";
                string PASS = "feFA$YUc38";
                message.Subject = subject;
                message.Body = msg;
                message.IsBodyHtml = true;
                message.From = new MailAddress(DE);
                System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient("184.95.60.10");
                smtpMail.EnableSsl = false;
                smtpMail.UseDefaultCredentials = false;
                smtpMail.Host = "Mail.construplan.com.mx";
                smtpMail.Port = 587;
                smtpMail.Credentials = new System.Net.NetworkCredential(DE, PASS);

                smtpMail.Send(message);
                smtpMail.Dispose();

            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }

        #region correosSeguridad
        public static bool sendMailWithFilesSeguridad(string subject, string msg, List<string> emails, List<adjuntoCorreoDTO> ListaArchivos)
        {
            bool result = true;
            try
            {
                var message = new MailMessage();

                var finalList = getFinalMailList(emails);
                foreach (var i in finalList)
                {
                    message.To.Add(new MailAddress(i, "", Encoding.UTF8));
                }

                if (ListaArchivos != null)
                {

                    for (int i = 0; i < ListaArchivos.Count; i++)
                    {
                        message.Attachments.Add(new Attachment(new MemoryStream(ListaArchivos[i].archivo), ListaArchivos[i].nombreArchivo + ListaArchivos[i].extArchivo, getTypeArchivo(ListaArchivos[i].extArchivo)));
                    }
                }

                string DE = "reportes.incidentes@construplan.com.mx";
                string PASS = "$6Gf4#d45";
                message.Subject = subject;
                message.Body = msg;
                message.IsBodyHtml = true;
                message.From = new MailAddress(DE);
                System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient("184.95.60.10");
                smtpMail.EnableSsl = false;
                smtpMail.UseDefaultCredentials = false;
                smtpMail.Host = "Mail.construplan.com.mx";
                smtpMail.Port = 587;
                smtpMail.Credentials = new System.Net.NetworkCredential(DE, PASS);

                smtpMail.Send(message);
                smtpMail.Dispose();

            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }
        public static bool sendEmailSeguridad(string subject, string msg, List<string> emails)
        {
            bool result = true;
            try
            {
                var message = new MailMessage();

                var finalList = getFinalMailList(emails);
                foreach (var i in finalList)
                {
                    message.To.Add(new MailAddress(i));
                }



                string DE = "reportes.incidentes@construplan.com.mx";
                string PASS = "$6Gf4#d45";
                message.Subject = subject;
                message.Body = msg;
                message.IsBodyHtml = true;
                message.From = new MailAddress(DE);
                System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient("184.95.60.10");
                smtpMail.EnableSsl = false;
                smtpMail.UseDefaultCredentials = false;
                smtpMail.Host = "Mail.construplan.com.mx";
                smtpMail.Port = 587;
                smtpMail.Credentials = new System.Net.NetworkCredential(DE, PASS);

                smtpMail.Send(message);
                smtpMail.Dispose();
            }
            catch (Exception e)
            {
                var exs = e.Message;
                result = false;
            }
            return result;
        }

        public static bool sendEmailAdjuntoInMemory2Seguridad(string subject, string msg, List<string> emails, List<Byte[]> ListaArchivos, string tipoFormato)
        {
            bool result = true;
            try
            {
                var message = new MailMessage();
                var finalList = getFinalMailList(emails);
                foreach (var i in finalList)
                {
                    if (i != null)
                    {
                        message.To.Add(new MailAddress(i));
                    }
                }

                if (ListaArchivos != null && ListaArchivos.Count() > 0)
                {
                    //agregado de archivo
                    //foreach (Byte[] archivo in ListaArchivos)
                    //{
                    //    //comprobamos si existe el archivo y lo agregamos a los adjuntos
                    //    message.Attachments.Add(new Attachment(new MemoryStream(archivo), "Minuta.pdf", "application/pdf"));
                    //}


                    message.Attachments.Add(new Attachment(new MemoryStream(ListaArchivos[0]), tipoFormato, "application/pdf"));
                    //message.Attachments.Add(new Attachment(new MemoryStream(ListaArchivos[0]), "FormatoCambios.pdf", "application/pdf"));
                }

                string DE = "reportes.incidentes@construplan.com.mx";
                string PASS = "$6Gf4#d45";
                message.Subject = subject;
                message.Body = msg;
                message.IsBodyHtml = true;
                message.From = new MailAddress(DE);
                System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient("184.95.60.10");
                smtpMail.EnableSsl = false;
                smtpMail.UseDefaultCredentials = false;
                smtpMail.Host = "Mail.construplan.com.mx";
                smtpMail.Port = 587;
                smtpMail.Credentials = new System.Net.NetworkCredential(DE, PASS);

                smtpMail.Send(message);
                smtpMail.Dispose();

            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }
        #endregion

    }
}
