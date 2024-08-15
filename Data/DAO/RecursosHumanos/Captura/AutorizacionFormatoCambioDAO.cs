using Core.DAO.Maquinaria.Reporte;
using Core.DAO.RecursosHumanos.Captura;
using Core.DTO;
using Core.DTO.RecursosHumanos.FormatoCambio;
using Core.DTO.Utils.Data;
using Core.Entity.RecursosHumanos.Captura;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Maquinaria.Reporte;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace Data.DAO.RecursosHumanos.Captura
{
    public class AutorizacionFormatoCambioDAO : GenericDAO<tblRH_AutorizacionFormatoCambio>, IAutorizacionFormatoCambio
    {
        IEncabezadoDAO encabezadoFactoryServices = new EncabezadoFactoryServices().getEncabezadoServices();
        string RutaServidor = "";

        public AutorizacionFormatoCambioDAO()
        {
#if DEBUG
            RutaServidor = @"C:\Proyectos\SIGOPLANv2\ENCABEZADOS";
#else
            RutaServidor = @"\\10.1.0.112\Proyecto\SIGOPLAN\ENCABEZADOS";
#endif
        }

        public tblRH_AutorizacionFormatoCambio SaveChangesAutorizacionCambios(tblRH_AutorizacionFormatoCambio objAutorizacion)
        {
            try
            {
                if (objAutorizacion.id == 0)
                {
                    SaveEntity(objAutorizacion, (int)BitacoraEnum.FORMATOCAMBIORH);
                }
                else
                {
                    Update(objAutorizacion, objAutorizacion.id, (int)BitacoraEnum.FORMATOCAMBIORH);
                }

            }
            catch (Exception e)
            {
                return new tblRH_AutorizacionFormatoCambio();
            }
            return objAutorizacion;
        }

        public void EliminarAutorizador(tblRH_AutorizacionFormatoCambio objAutorizacion)
        {

            try
            {

                tblRH_AutorizacionFormatoCambio entidad = _context.tblRH_AutorizacionFormatoCambio.FirstOrDefault(x => x.id == objAutorizacion.id);

                Delete(entidad, (int)BitacoraEnum.FORMATOCAMBIORH);


            }
            catch (Exception)
            {
                throw new Exception("Error al eliminar el registro");
            }

        }
        public void EliminarAutorizadores(int id)
        {
            var firmas = _context.tblRH_AutorizacionFormatoCambio.Where(x => x.Id_FormatoCambio == id).ToList();
            foreach (var i in firmas)
            {
                Delete(i, (int)BitacoraEnum.FORMATOCAMBIORH);
            }
            var alertas = _context.tblP_Alerta.Where(x => x.sistemaID == (int)SistemasEnum.RH && x.objID == id).ToList();
            alertas.ForEach(x => x.visto = true);
            _context.SaveChanges();

        }
        public List<tblRH_AutorizacionFormatoCambio> getAutorizacion(int idFormato)
        {
            try
            {
                return _context.tblRH_AutorizacionFormatoCambio.Where(x => x.Id_FormatoCambio == idFormato).OrderBy(x => x.Orden).ToList();
            }
            catch (Exception e)
            {
                return new List<tblRH_AutorizacionFormatoCambio>();
            }
        }
        //public List<tblRH_AutorizacionFormatoCambio> getAutorizacion(int idFormato)
        //{

        //    var dto = new List<tblRH_AutorizacionFormatoCambio>();

        //    var dto2 = new List<tblRH_AutorizacionFormatoCambio>();

        //    for (int i = 1; i <= 8; i++)
        //    {
        //        var d = new tblRH_AutorizacionFormatoCambio();
        //        d.Orden = i;

        //        switch (i)
        //        {
        //            case 1:
        //                d.Responsable = "Solicitante";
        //                d.PuestoAprobador = "Responsable del Centro de Costos";
        //                d.Orden = 1;
        //                break;
        //            case 2:
        //                d.Responsable = "VoBo";
        //                d.PuestoAprobador = "Recursos Humanos";
        //                break;
        //            case 3:
        //                d.Responsable = "Autoriza 1";
        //                d.PuestoAprobador = "Director de Área / Gerente de Área";
        //                d.Orden = 1;
        //                break;
        //            case 4:
        //                d.Responsable = "Autoriza 1";
        //                d.PuestoAprobador = "Director de Área / Gerente de Área";
        //                d.Orden = 1;
        //                break;
        //            case 5:
        //                d.Responsable = "Autoriza 2";
        //                d.PuestoAprobador = "Director de División";
        //                break;
        //            case 7:
        //                d.Responsable = "Autoriza 2";
        //                d.PuestoAprobador = "Director de División";
        //                break;
        //            case 8:
        //                d.Responsable = "Autoriza 3";
        //                d.PuestoAprobador = "Alta Dirección";
        //                break;
        //            case 9:
        //                d.Responsable = "Autoriza 3";
        //                d.PuestoAprobador = "Alta Dirección";
        //                break;
        //            default:
        //                break;
        //        }
        //        dto.Add(d);
        //    }


        //    try
        //    {
        //        var result = _context.tblRH_AutorizacionFormatoCambio.Where(x => x.Id_FormatoCambio == idFormato).OrderBy(x => x.Orden).ToList();

        //        bool bandera = false;

        //        for (int i = 0; i < dto.ToArray().Length; i++)
        //        {
        //            bandera = false;
        //            foreach (var item in result)
        //            {
        //                if (dto[i].Orden == item.Orden && bandera == false)
        //                {
        //                    bandera = true;


        //                    dto2.Add(item);
        //                    item.Orden = 0;
        //                }
        //            }

        //            if (!bandera)
        //            {
        //                dto2.Add(dto[i]);
        //            }

        //        }




        //    }
        //    catch (Exception e)
        //    {
        //        return new List<tblRH_AutorizacionFormatoCambio>();
        //    }

        //    return dto2;
        //}
        public int getFormatoIDByAutorizacion(int id)
        {
            var formatoID = 0;
            var data = _context.tblRH_AutorizacionFormatoCambio.FirstOrDefault(x => x.id == id);
            formatoID = data.Id_FormatoCambio;
            return formatoID;
        }
        public bool EsUsuarioMismoCC(int usuarioCapturoID)
        {
            var usuarioActualID = vSesiones.sesionUsuarioDTO.id;

            if (usuarioCapturoID == usuarioActualID)
            {
                return true;
            }

            //var ccsUsuarioCapturo = _context.tblP_CC_Usuario
            //    .Where(x => x.usuarioID == usuarioCapturoID)
            //    .Select(x => x.cc)
            //    .ToList();

            var ccsUsuarioLogueado = _context.tblP_CC_Usuario
                .Where(x => x.usuarioID == usuarioActualID)
                .Select(x => x.cc)
                .ToList();

            if (/*ccsUsuarioCapturo.Count == 0 ||*/ ccsUsuarioLogueado.Count == 0)
            {
                return false;
            }

            //return ccsUsuarioCapturo.Intersect(ccsUsuarioLogueado).Count() > 0;

            return true;
        }

        public Dictionary<string, object> CancelarFormatoCambioPorTiempo()
        {
            var resultado = new Dictionary<string, object>();

            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    var fechaLimite = DateTime.Now.AddDays(-30);

                    var cambiosPorCancelar = _context.Select<CambiosPorCancelarDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT DISTINCT
	                                f.id,
	                                (
		                                SELECT TOP 1
			                                safc.Firma
		                                FROM
			                                tblRH_AutorizacionFormatoCambio AS safc
		                                WHERE
			                                safc.Id_FormatoCambio = f.id AND
			                                safc.Firma != 'S/F'
		                                ORDER BY
			                                safc.Orden
	                                ) AS firma,
                                    (
                                        SELECT TOP 1
                                            safc.id
                                        FROM
                                            tblRH_AutorizacionFormatoCambio AS safc
                                        WHERE
                                            safc.Id_FormatoCambio = f.id AND
                                            safc.Firma = 'S/F'
                                        ORDER BY
                                            safc.Orden
                                    ) AS idFirmaPendiente,
                                    f.fechaCaptura,
									f.CcID
                                FROM
	                                tblRH_FormatoCambio AS f
                                WHERE
	                                f.Aprobado = 0 AND
	                                f.Rechazado = 0"
                    });

                    //cambiosPorCancelar = cambiosPorCancelar.Where(x => x.firma != null).ToList();

                    var formatosCambioPorCancelar = new List<CambiosPorCancelarDTO>();

                    foreach (var item in cambiosPorCancelar)
                    {
                        //var dia = Convert.ToInt32(item.firma.Substring(9, 2));
                        //var mes = Convert.ToInt32(item.firma.Substring(11, 2));
                        //var anio = Convert.ToInt32(item.firma.Substring(13, 4));

                        //if (new DateTime(anio, mes, dia).Date < fechaLimite.Date)
                        //{
                        //    formatosCambioPorCancelar.Add(item);
                        //}

                        if (item.fechaCaptura.Value.Date < fechaLimite.Date)
                        {
                            formatosCambioPorCancelar.Add(item);
                        }
                    }

                    var idsPorCancelar = formatosCambioPorCancelar.Select(x => x.id);
                    var idsPorComentar = formatosCambioPorCancelar.Select(x => x.idFirmaPendiente);

                    var formatosCambio = _context.tblRH_FormatoCambio.Where(x => idsPorCancelar.Contains(x.id)).ToList();
                    foreach (var item in formatosCambio)
                    {
                        item.Rechazado = true;
                    }
                    _context.SaveChanges();

                    var firmasCambios = _context.tblRH_AutorizacionFormatoCambio.Where(x => idsPorComentar.Contains(x.id)).ToList();
                    foreach (var item in firmasCambios)
                    {
                        item.Rechazado = true;
                        item.Autorizando = false;
                        item.comentario = "RECHAZADO AUTOMATICAMENTE POR SISTEMA";
                    }
                    _context.SaveChanges();

                    var alertas = _context.tblP_Alerta.Where(x => !x.visto && x.tipoAlerta == 2 && x.sistemaID == 16 && x.msj.Contains("Firma-Formato de Cambio") && x.moduloID == 22 && idsPorComentar.Contains(x.objID)).ToList();
                    foreach (var item in alertas)
                    {
                        item.visto = true;
                    }
                    _context.SaveChanges();

                    transaccion.Commit();

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, formatosCambioPorCancelar);
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();

                    LogError(16, 0, "FormatoCambio", "CancelarFormatoCambioPorTiempo", ex, AccionEnum.ACTUALIZAR, 0, null);

                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public DataTable getInfoEnca(string nombreReporte, string area)
        {
            DataTable tableEncabezado = new DataTable();

            tableEncabezado.Columns.Add("logo", System.Type.GetType("System.Byte[]"));
            tableEncabezado.Columns.Add("nombreEmpresa", System.Type.GetType("System.String"));
            tableEncabezado.Columns.Add("nombreReporte", System.Type.GetType("System.String"));
            tableEncabezado.Columns.Add("area", System.Type.GetType("System.String"));

            var data = encabezadoFactoryServices.getEncabezadoDatos();
            string path = data.logo;
            //string path = Path.Combine(RutaServidor, Path.GetFileName(data.logo));
            byte[] imgdata = System.IO.File.ReadAllBytes(HostingEnvironment.MapPath(path)); //File.ReadAllBytes(path);
            string empresa = data.nombreEmpresa;

            tableEncabezado.Rows.Add(imgdata, empresa, nombreReporte, area);

            return tableEncabezado;
        }
    }
}
