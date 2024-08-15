using Core.DAO.Maquinaria.Inventario;
using Core.DTO;
using Core.DTO.Maquinaria.Inventario;
using Core.DTO.Maquinaria.Reporte.ActivoFijo;
using Core.DTO.Maquinaria.StandBy;
using Core.DTO.Utils.Data;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using Core.Entity.Maquinaria.StandBy;
using Core.Entity.Principal.Alertas;
using Core.Enum.Maquinaria.StandBy;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Alertas;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Maquinaria.Reporte.ActivoFijo;
using Infrastructure.DTO;
using Infrastructure.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Inventario
{
    public class StandByNuevoDAO : GenericDAO<tblM_STB_CapturaStandBy>, IStandByNuevoDAO
    {
        #region INIT
        private const int _SISTEMA = (int)SistemasEnum.MAQUINARIA;
        private const string _NOMBRE_CONTROLADOR = "StandByNuevoController";
        private Dictionary<string, object> resultado = new Dictionary<string, object>();
        private const int _JOSE_GAYTAN = 3314;
        private const int _GERARDO_REINA = 1164;
        private const int _OMAR_NUNEZ = 7939;
        private const int _ADMIN = 13;
        #endregion

        public bool GuardarCaptura(List<tblM_STB_CapturaStandBy> lst)
        {
            ActivoFijoFactoryServices affs = new ActivoFijoFactoryServices();

            var result = true;

            foreach (var item in lst)
            {
                var depEconomico = affs.getActivoFijoServices().DepreciacionNumEconomico(item.Economico, DateTime.Now);
                if (depEconomico.Success)
                {
                    var detDep = depEconomico.Value as DepreciacionMaquinaConOverhaulDTO;

                    item.moiEquipo = detDep.MoiEquipo;
                    item.valorEnLibroEquipo = detDep.MoiEquipo - detDep.DepreciacionEquipo;
                    item.depreciacionMensualEquipo = detDep.DepreciacionMensualEquipo;

                    item.valorEnLibroOverhaul = detDep.MoiOverhaul - detDep.DepreciacionOverhaul;
                    item.depreciacionMensualOverhaul = detDep.DepreciacionMensualOH;
                }
                else
                {
                    result = false;
                    throw new Exception(depEconomico.Message);
                }
            }

            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.tblM_STB_CapturaStandBy.AddRange(lst);
                    _context.SaveChanges();

                    #region CORREO
                    var correos = new List<string>();
                    var correosCC = new List<string>();
                    correos.Add("oscar.roman@construplan.com.mx");
                    correosCC.Add(vSesiones.sesionUsuarioDTO.correo);
#if DEBUG
                    correos = new List<string> { "martin.zayas@construplan.com.mx" };
                    correosCC = new List<string> { "martin.zayas@construplan.com.mx" };
#endif
                    string listaEconomicos = "<p><ul>";
                    foreach (var item in lst)
                    {
                        listaEconomicos += "<li><strong>" + item.Economico + "</strong></li>";
                    }
                    listaEconomicos += "</ul></p>";

                    string asunto = "Se solicita su visto bueno de equipos en StandBy";
                    string cuerpo = string.Format(@"
                        <p>Buen día.</p>
                        <p>Se solicita su visto bueno para colocar los siguientes equipos en StandBy.</p>
                        {0}", listaEconomicos);

                    var envioCorrecto = EnviarCorreo(new CorreoDTO()
                    {
                        asunto = asunto,
                        cuerpo = cuerpo,
                        correos = correos,
                        correosCC = correosCC
                    });

                    if (!envioCorrecto)
                    {
                        throw new Exception("Error al enviar el correo de captura de StandBy");
                    }
                    #endregion

                    // SE VERIFICA SI YA EXISTE ALERTA, SI NO, PARA CREARLA
                    if (!VerificarAlertaVoBoAutorizacion(_JOSE_GAYTAN))
                        CrearAlerta(_JOSE_GAYTAN, AlertasEnum.REDIRECCION, "/StandByNuevo/Validacion", -1, "StandBy: Se requiere VoBo.");

                    transaccion.Commit();
                }
                catch (Exception e)
                {
                    transaccion.Rollback();
                    result = false;
                }
            }
            return result;
        }
        public bool GuardarValidacion(List<StandByNuevoDTO> lst)
        {
            var result = true;
            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    var detalles = new List<StandByNuevoDTO>();
                    int esAutorizacion = 0;

                    #region STANDBY
                    foreach (var standByGp in lst.GroupBy(x => x.ccActual))
                    {
                        foreach (var standBy in standByGp)
                        {
                            var data = _context.tblM_STB_CapturaStandBy.FirstOrDefault(x => x.id == standBy.id);
                            data.estatus = standBy.estatus;
                            data.usuarioAutorizaID = vSesiones.sesionUsuarioDTO.id;
                            data.fechaAutoriza = DateTime.Now;
                            data.comentarioValidacion = standBy.comentario;
                            _context.SaveChanges();

                            switch (data.estatus)
                            {
                                case 2:
                                    {
                                        var maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == data.noEconomicoID);
                                        if (maquina != null)
                                        {
                                            maquina.estatus = 2;//StandBy
                                            _context.SaveChanges();
                                        }
                                    }
                                    break;
                                case 3:
                                    {
                                        esAutorizacion = data.estatus;
                                    }
                                    break;
                            }

                            var detalle = new StandByNuevoDTO();
                            detalle.Economico = data.Economico;
                            detalle.modelo = standBy.modelo;
                            detalle.ccActual = data.ccActual.Trim();
                            detalle.ccDescripcion = standBy.ccActual.Trim();
                            detalle.fechaCaptura = data.fechaCaptura.ToShortDateString();
                            detalle.justificacion = data.comentarioJustificacion ?? "";
                            detalle.moiEquipo = data.moiEquipo;
                            detalle.valorEnLibroEquipo = data.valorEnLibroEquipo;
                            detalle.depreciacionMensualEquipo = data.depreciacionMensualEquipo;
                            detalle.valorEnLibroOverhaul = data.valorEnLibroOverhaul;
                            detalle.depreciacionMensualOverhaul = data.depreciacionMensualOverhaul;
                            detalle.comentario = data.comentarioValidacion ?? "";
                            detalle.usuarioCapturaID = data.usuarioCapturaID;
                            detalles.Add(detalle);
                        }
                    }
                    #endregion

                    #region CORREO
                    foreach (var standByGp in detalles.GroupBy(x => x.ccActual))
                    {
                        var adminsGerentes = _context.Select<AutorizanteDTO>(new DapperDTO
                        {
                            baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                            consulta = @"SELECT
                                            u.id,
                                            u.nombre,
                                            u.apellidoPaterno,
                                            u.apellidoMaterno,
                                            u.correo,
                                            c.cc as ac,
                                            a.perfilAutorizaID
                                        FROM
                                            tblP_Autoriza AS a
                                        INNER JOIN
                                            tblP_Usuario AS u ON u.id = a.usuarioID
                                        INNER JOIN
                                            tblP_CC_Usuario AS c ON c.id = a.cc_usuario_ID
                                        WHERE
                                            u.estatus = 1 AND
                                            a.perfilAutorizaID in (5, 1) AND /*5 == Admin, 1 == Gerente*/
                                            c.cc = @paramCC",
                            parametros = new { paramCC = standByGp.Key }
                        });

                        var correos = new List<string>();
                        var correosCC = new List<string>();

                        correos.AddRange(adminsGerentes.Select(x => x.correo).Distinct().ToList());
                        
                        correosCC.Add("g.reina@construplan.com.mx");
                        correosCC.Add("oscar.roman@construplan.com.mx");
                        correosCC.Add("martin.valle@construplan.com.mx");

                        string asunto = esAutorizacion == 3 ? "Se ha rechazado la colocación de equipos en StandBy" : "Se ha autorizado la colocación de equipos en StandBy";

                        var listaEconomicos = "<p><ul>";
                        foreach (var standBy in standByGp)
                        {
                            listaEconomicos += "<li><strong>" + standBy.Economico + "</strong></li>";

                            string correoCaptura = _context.tblP_Usuario.Where(w => w.id == standBy.usuarioCapturaID && w.estatus).Select(s => s.correo).FirstOrDefault();
                            if (!string.IsNullOrEmpty(correoCaptura))
                                correos.Add(correoCaptura);
                        }
                        listaEconomicos += "</ul></p>";

#if DEBUG
                        correos = new List<string> { "martin.zayas@construplan.com.mx" };
                        correosCC = new List<string> { "martin.zayas@construplan.com.mx" };
#endif
                        string contenido = "";

                        if (esAutorizacion == 3)
                        {
                            contenido = string.Format(@"
                            <p>Buen día.</p>
                            <p>Los siguientes equipos han rechazados para ser colocados en StandBy.</p>
                            {0}", listaEconomicos);
                        }
                        else
                        {
                            contenido = string.Format(@"
                            <p>Buen día.</p>
                            <p>Los siguientes equipos han sido colocados en StandBy, favor de verificar que la información de los equipos se encuentre regularizada en sistema.</p>
                            {0}", listaEconomicos);
                        }

                        using (var excel = new ExcelPackage())
                        {
                            var excelDetalles = excel.Workbook.Worksheets.Add("StandBy");

                            var header = new List<string> { "Equipo", "Modelo", "Obra", "Fecha solicitud", "Justificación", "MOI Equipo", "Falta Dep Equipo", "Dep Mensual Equipo", "Falta Dep OH", "Dep Mensual OH", "Observación" };

                            for (int o = 1; o < header.Count; o++)
                            {
                                excelDetalles.Cells[1, o].Value = header[o - 1];
                            }

                            var cellData = new List<object[]>();
                            foreach (var standBy in standByGp)
                            {
                                cellData.Add(new object[]{
                                    standBy.Economico,
                                    standBy.modelo,
                                    standBy.ccDescripcion,
                                    standBy.fechaCaptura,
                                    standBy.justificacion,
                                    standBy.moiEquipo,
                                    standBy.valorEnLibroEquipo,
                                    standBy.depreciacionMensualEquipo,
                                    standBy.valorEnLibroOverhaul,
                                    standBy.depreciacionMensualOverhaul,
                                    standBy.comentario
                                });
                            }

                            excelDetalles.Cells[2, 1].LoadFromArrays(cellData);

                            ExcelRange range = excelDetalles.Cells[1, 1, excelDetalles.Dimension.End.Row, excelDetalles.Dimension.End.Column];

                            ExcelTable tab = excelDetalles.Tables.Add(range, "Tabla");

                            excelDetalles.Cells[1, 6, excelDetalles.Dimension.End.Row, 10].Style.Numberformat.Format = "$#,##0.00";

                            tab.TableStyle = TableStyles.Medium17;

                            excelDetalles.Cells[excelDetalles.Dimension.Address].AutoFitColumns();

                            excel.Compression = CompressionLevel.BestSpeed;

                            var adjuntos = new List<Attachment>();
                            using (var exportData = new MemoryStream())
                            {
                                excel.SaveAs(exportData);

                                var file = exportData.ToArray();
                                exportData.Close();

                                adjuntos.Add(new Attachment(new MemoryStream(file), "StandBy - " + standByGp.Key + " - " + DateTime.Now.ToString("dd-MM-yyyy") +  ".xlsx"));
                            }

                            var envioCorrecto = EnviarCorreo(new CorreoDTO()
                            {
                                asunto = asunto,
                                cuerpo = contenido,
                                correos = correos,
                                correosCC = correosCC,
                                archivos = adjuntos
                            });

                            if (!envioCorrecto)
                            {
                                throw new Exception("Error al enviar el correo de autorización de StandBy");
                            }
                        }
                    }
                    #endregion

                    // SE VERIFICA SI HAY REGISTROS PENDIENTES DE AUTORIZAR, SI NO, PARA ELIMINAR ALERTA
                    if (!_context.tblM_STB_CapturaStandBy.Any(w => w.usuarioAutoriza == null))
                        EliminarAlerta();

                    transaccion.Commit();
                }
                catch (Exception e)
                {
                    transaccion.Rollback();

                    result = false;
                }
            }

            return result;
        }

        private bool EnviarCorreo(CorreoDTO correo)
        {
            if (correo.correos == null || correo.correos.Count == 0 || string.IsNullOrEmpty(correo.asunto) || string.IsNullOrEmpty(correo.cuerpo))
            {
                return false;
            }

            MailMessage mailMessage = new MailMessage();

            correo.correos.ForEach(c => mailMessage.To.Add(new MailAddress(c)));
            correo.correosCC.ForEach(c => mailMessage.CC.Add(new MailAddress(c)));
            correo.archivos.ForEach(archivo => mailMessage.Attachments.Add(archivo));

            mailMessage.IsBodyHtml = true;
            mailMessage.From = new MailAddress("alertas.sigoplan@construplan.com.mx");
            mailMessage.Subject = correo.asunto;
            mailMessage.Body = string.Format(@"
                {0} 
                <p><o:p>&nbsp;</o:p></p>
                <p><o:p>&nbsp;</o:p></p>
                <p>Se informa que este es un correo autogenerado por el sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx</a>). No es necesario dar una respuesta. Gracias.</p>
            ", correo.cuerpo);

            SmtpClient smptConfig = new SmtpClient();
            smptConfig.Send(mailMessage);
            smptConfig.Dispose();

            return true;
        }

        public bool GuardarLibracion(List<StandByNuevoDTO> lst)
        {
            var result = true;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var detalles = new List<StandByNuevoDTO>();
                    foreach (var i in lst)
                    {
                        var data = _context.tblM_STB_CapturaStandBy.FirstOrDefault(x => x.id == i.id);
                        data.usuarioLiberaID = vSesiones.sesionUsuarioDTO.id;
                        data.fechaLibera = DateTime.Now;
                        data.estatus = i.estatus;
                        data.comentarioLiberacion = i.comentario;
                        _context.SaveChanges();
                        var maq = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == data.noEconomicoID);
                        maq.estatus = 1;
                        _context.SaveChanges();

                        var detalle = new StandByNuevoDTO();
                        detalle.Economico = data.Economico;
                        detalle.modelo = i.modelo;
                        detalle.ccActual = data.ccActual.Trim();
                        detalle.ccDescripcion = i.ccActual.Trim();
                        detalle.fechaCaptura = data.fechaCaptura.ToShortDateString();
                        detalle.justificacion = data.comentarioJustificacion ?? "";
                        detalle.moiEquipo = data.moiEquipo;
                        detalle.valorEnLibroEquipo = data.valorEnLibroEquipo;
                        detalle.depreciacionMensualEquipo = data.depreciacionMensualEquipo;
                        detalle.valorEnLibroOverhaul = data.valorEnLibroOverhaul;
                        detalle.depreciacionMensualOverhaul = data.depreciacionMensualOverhaul;
                        detalle.comentario = data.comentarioValidacion ?? "";
                        detalle.usuarioCapturaID = data.usuarioCapturaID;
                        detalles.Add(detalle);
                    }

                    #region CORREO
                    foreach (var standByGp in detalles.GroupBy(x => x.ccActual))
                    {
                        var adminsGerentes = _context.Select<AutorizanteDTO>(new DapperDTO
                        {
                            baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                            consulta = @"SELECT
                                                u.id,
                                                u.nombre,
                                                u.apellidoPaterno,
                                                u.apellidoMaterno,
                                                u.correo,
                                                c.cc as ac,
                                                a.perfilAutorizaID
                                            FROM
                                                tblP_Autoriza AS a
                                            INNER JOIN
                                                tblP_Usuario AS u ON u.id = a.usuarioID
                                            INNER JOIN
                                                tblP_CC_Usuario AS c ON c.id = a.cc_usuario_ID
                                            WHERE
                                                u.estatus = 1 AND
                                                a.perfilAutorizaID in (5, 1) AND /*5 == Admin, 1 == Gerente*/
                                                c.cc = @paramCC",
                            parametros = new { paramCC = standByGp.Key }
                        });

                        var correos = new List<string>();
                        var correosCC = new List<string>();

                        correos.AddRange(adminsGerentes.Select(x => x.correo).Distinct().ToList());

                        correosCC.Add("g.reina@construplan.com.mx");
                        correosCC.Add("oscar.roman@construplan.com.mx");
                        correosCC.Add("martin.valle@construplan.com.mx");

                        string asunto = "Se han liberado los siguientes equipos de StandBy";

                        var listaEconomicos = "<p><ul>";
                        foreach (var standBy in standByGp)
                        {
                            listaEconomicos += "<li><strong>" + standBy.Economico + "</strong></li>";

                            string correoCaptura = _context.tblP_Usuario.Where(w => w.id == standBy.usuarioCapturaID && w.estatus).Select(s => s.correo).FirstOrDefault();
                            if (!string.IsNullOrEmpty(correoCaptura))
                                correosCC.Add(correoCaptura);
                        }
                        listaEconomicos += "</ul></p>";

    #if DEBUG
                        correos = new List<string> { "martin.zayas@construplan.com.mx" };
                        correosCC = new List<string> { "martin.zayas@construplan.com.mx" };
    #endif
                        string contenido = string.Format(@"
                                <p>Buen día.</p>
                                <p>Los siguientes equipos han sido liberados de StandBy, favor de verificar que la información de los equipos se encuentre regularizada en sistema.</p>
                                {0}", listaEconomicos);

                        using (var excel = new ExcelPackage())
                        {
                            var excelDetalles = excel.Workbook.Worksheets.Add("StandBy");

                            var header = new List<string> { "Equipo", "Modelo", "Obra", "Fecha solicitud", "Justificación", "MOI Equipo", "Falta Dep Equipo", "Dep Mensual Equipo", "Falta Dep OH", "Dep Mensual OH", "Observación" };

                            for (int o = 1; o < header.Count; o++)
                            {
                                excelDetalles.Cells[1, o].Value = header[o - 1];
                            }

                            var cellData = new List<object[]>();
                            foreach (var standBy in standByGp)
                            {
                                cellData.Add(new object[]{
                                    standBy.Economico,
                                    standBy.modelo,
                                    standBy.ccDescripcion,
                                    standBy.fechaCaptura,
                                    standBy.justificacion,
                                    standBy.moiEquipo,
                                    standBy.valorEnLibroEquipo,
                                    standBy.depreciacionMensualEquipo,
                                    standBy.valorEnLibroOverhaul,
                                    standBy.depreciacionMensualOverhaul,
                                    standBy.comentario
                                    });
                            }

                            excelDetalles.Cells[2, 1].LoadFromArrays(cellData);

                            ExcelRange range = excelDetalles.Cells[1, 1, excelDetalles.Dimension.End.Row, excelDetalles.Dimension.End.Column];

                            ExcelTable tab = excelDetalles.Tables.Add(range, "Tabla");

                            excelDetalles.Cells[1, 6, excelDetalles.Dimension.End.Row, 10].Style.Numberformat.Format = "$#,##0.00";

                            tab.TableStyle = TableStyles.Medium17;

                            excelDetalles.Cells[excelDetalles.Dimension.Address].AutoFitColumns();

                            excel.Compression = CompressionLevel.BestSpeed;

                            var adjuntos = new List<Attachment>();
                            using (var exportData = new MemoryStream())
                            {
                                excel.SaveAs(exportData);

                                var file = exportData.ToArray();
                                exportData.Close();

                                adjuntos.Add(new Attachment(new MemoryStream(file), "StandBy - " + standByGp.Key + " - " + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx"));
                            }

                            var envioCorrecto = EnviarCorreo(new CorreoDTO()
                            {
                                asunto = asunto,
                                cuerpo = contenido,
                                correos = correos,
                                correosCC = correosCC,
                                archivos = adjuntos
                            });

                            if (!envioCorrecto)
                            {
                                throw new Exception("Error al enviar el correo de autorización de StandBy");
                            }
                        }
                    }
                    #endregion

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    result = false;
                }
            }
            
            return result;
        }
        public List<tblM_CatMaquina> getListaDisponible(string cc)
        {
            var result = new List<tblM_CatMaquina>();
            var lst = _context.tblM_STB_CapturaStandBy.Where(x=>x.estatus<3).Select(x=>x.noEconomicoID).ToList();
            result = _context.tblM_CatMaquina.Where(x => x.estatus == 1 && x.centro_costos.Equals(cc) && !lst.Contains(x.id)).OrderBy(x=>x.noEconomico).ToList();
            return result;
        }
        public List<tblM_STB_CapturaStandBy> getListaByEstatus(int estatus, string noAC, string noEconomico)
        {
            var result = new List<tblM_STB_CapturaStandBy>();
            result = _context.tblM_STB_CapturaStandBy.Where(x => (estatus==0?true:x.estatus == estatus) && ((string.IsNullOrEmpty(noAC)?true:x.ccActual.Equals(noAC)) && (string.IsNullOrEmpty(noEconomico)?true:x.Economico.Equals(noEconomico)))).OrderBy(x => x.Economico).ThenBy(x => x.fechaCaptura).ToList();

            //if (vSesiones.sesionUsuarioDTO.id == _JOSE_GAYTAN)
            //    result = result.Where(w => !w.esVoBo).ToList();

            #region ACTUALIZAR DEP
            //ActivoFijoFactoryServices affs = new ActivoFijoFactoryServices();
            //foreach (var item in result)
            //{
            //    var depEconomico = affs.getActivoFijoServices().DepreciacionNumEconomico(item.Economico, new DateTime(2023, 7, 31));
            //    if (depEconomico.Success)
            //    {
            //        var detDep = depEconomico.Value as DepreciacionMaquinaConOverhaulDTO;

            //        item.moiEquipo = detDep.MoiEquipo;
            //        item.valorEnLibroEquipo = detDep.MoiEquipo - detDep.DepreciacionEquipo;
            //        item.depreciacionMensualEquipo = detDep.DepreciacionMensualEquipo;

            //        item.valorEnLibroOverhaul = detDep.MoiOverhaul - detDep.DepreciacionOverhaul;
            //        item.depreciacionMensualOverhaul = detDep.DepreciacionMensualOH;

            //        _context.SaveChanges();
            //    }
            //    else
            //    {
            //        throw new Exception(depEconomico.Message);
            //    }
            //}
            #endregion

            return result;
        }
        public List<tblM_STB_CapturaStandBy> getListaByEstatusConDepreciacion(int estatus, string noAC, string noEconomico, DateTime fechaInicio, DateTime fechaFin, int tipo)
        {
            var result = new List<tblM_STB_CapturaStandBy>();
            result = _context.tblM_STB_CapturaStandBy.Where(x => (estatus == 0 ? true : x.estatus == estatus) && ((string.IsNullOrEmpty(noAC) ? true : x.ccActual.Equals(noAC)) && (string.IsNullOrEmpty(noEconomico) ? true : x.Economico.Equals(noEconomico))) && ((estatus == 0 || estatus==1) ? (x.fechaCaptura >= fechaInicio && x.fechaCaptura <= fechaFin) : (estatus == 2 || estatus==3) ? (x.fechaAutoriza >= fechaInicio && x.fechaAutoriza <= fechaFin) : (x.fechaLibera >= fechaInicio && x.fechaLibera <= fechaFin))).OrderBy(x => x.Economico).ThenBy(x => (estatus == 0 || estatus == 1) ? x.fechaCaptura : (estatus==2 || estatus==3)?x.fechaAutoriza:x.fechaLibera).ToList();
            return result;
        }
        public List<DepreciacionLugarDTO> getDepreciacionPorStandBy(string ac, string economico ,DateTime fechaInicio, DateTime fechaFin, bool corteSemanal)
        {
            var data = new List<DepreciacionLugarDTO>();
            var diasMartes = getDiasMartes(fechaInicio, fechaFin);

            if (corteSemanal)
            {
                var maquinas = _context.tblM_CatMaquina.Where(x => x.estatus == 2).ToList();
                foreach (var item in maquinas)
                {
                    var o = new DepreciacionLugarDTO();
                    var stb = new tblM_STB_CapturaStandBy();
                    stb.ccActual = item.centro_costos;
                    o.standBy = stb;
                    o.equipo = item;
                    o.fechaDepreciacion = fechaFin;
                    data.Add(o);
                }
            }
            else
            {
                var recepciones = _context.tblM_STB_CapturaStandBy.Where(
                    x =>
                        (string.IsNullOrEmpty(ac) ? true : x.ccActual.Equals(ac)) &&
                        (string.IsNullOrEmpty(economico) ? true : x.Economico.Equals(economico)) &&
                        (x.estatus == 2 || x.estatus == 4) &&
                        (x.fechaAutoriza >= fechaInicio &&
                        (x.fechaAutoriza != null && x.fechaAutoriza.Value <= fechaFin))
                    ).ToList();
                foreach (var i in recepciones)
                {
                    foreach (var j in diasMartes)
                    {
                        var semanaHastaElMartes = j.Date.AddDays(-6);
                        if (((DateTime)i.fechaAutoriza).Date >= semanaHastaElMartes && ((DateTime)i.fechaAutoriza).Date <= j.Date)
                        {
                            var o = new DepreciacionLugarDTO();
                            o.standBy = i;
                            o.equipo = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == i.noEconomicoID);
                            o.fechaDepreciacion = j;
                            data.Add(o);
                        }
                    }
                }
            }

            return data;
        }
        public List<DepreciacionLugarDTO> getDepreciacionPorNoasignado(string economico,DateTime fechaInicio, DateTime fechaFin, bool corteSemanal)
        {
            var data = new List<DepreciacionLugarDTO>();
            var diasMartes = getDiasMartes(fechaInicio, fechaFin);

            if (corteSemanal)
            {
                var maquinas = _context.tblM_CatMaquina.Where(x => x.estatus == 1 && (x.centro_costos == "1010" || x.centro_costos == "1015" || x.centro_costos == "1018")).ToList();
                foreach (var item in maquinas)
                {
                    var o = new DepreciacionLugarDTO();
                    var stb = new tblM_STB_CapturaStandBy();
                    stb.ccActual = item.centro_costos;
                    o.standBy = stb;
                    o.equipo = item;
                    o.fechaDepreciacion = fechaFin;
                    data.Add(o);
                }
            }
            else
            {
                var maq = _context.tblM_CatMaquina.FirstOrDefault(x => x.noEconomico.Equals(economico));
                var recepciones = new List<tblM_ControlEnvioMaquinaria>();
                if (string.IsNullOrEmpty(economico))
                {
                    recepciones = _context.tblM_ControlEnvioMaquinaria.Where(x => /*&& x.tipoControl == 4*/ (x.fechaRecepcionEmbarque >= fechaInicio && x.fechaRecepcionEmbarque <= fechaFin)).ToList();
                }
                else
                {
                    recepciones = _context.tblM_ControlEnvioMaquinaria.Where(x => x.noEconomico == maq.id /*&& x.tipoControl == 4*/ && (x.fechaRecepcionEmbarque >= fechaInicio && x.fechaRecepcionEmbarque <= fechaFin)).ToList();
                }
                foreach (var i in recepciones.Where(x => x.tipoControl == 4))
                {
                    foreach (var j in diasMartes)
                    {
                        var semanaHastaElMartes = j.Date.AddDays(-6);
                        if (i.fechaRecepcionEmbarque.Date >= semanaHastaElMartes && i.fechaRecepcionEmbarque <= j.Date)
                        {
                            if (!recepciones.Any(x => x.noEconomico == i.noEconomico && x.tipoControl != 4 && x.fechaRecepcionEmbarque.Date > i.fechaRecepcionEmbarque && x.fechaRecepcionEmbarque <= j.Date))
                            {
                                var o = new DepreciacionLugarDTO();
                                o.control = i;
                                o.equipo = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == i.noEconomico);
                                o.fechaDepreciacion = j;
                                data.Add(o);
                            }
                        }
                    }
                }
            }

            return data;
        }

        public List<DateTime> getDiasMartes(DateTime inicio, DateTime fin)
        {
            List<DateTime> lst = new List<DateTime>();

            while (inicio <= fin)
            {
                if (inicio.DayOfWeek == DayOfWeek.Tuesday)
                {
                    lst.Add(inicio);
                }
                inicio = inicio.AddDays(1);
            }
            return lst;
        }

        public bool ActivarEconomicoPorAccionRealizada(string numeroEconomico, int? idEconomico, AccionActivacionEconomicoEnum accion, object objeto, bool buscarEnEnkontrol = false)
        {
            if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora)
            {
                return false;
            }

            tblM_CatMaquina maquina = null;

            if (buscarEnEnkontrol)
            {
                if (!string.IsNullOrEmpty(numeroEconomico))
                {
                    var queryEk = new OdbcConsultaDTO();
                    queryEk.consulta = "SELECT * FROM cc WHERE cc = ?";
                    queryEk.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "cc",
                        tipo = OdbcType.NVarChar,
                        valor = numeroEconomico
                    });
                    var ccDescripcion = _contextEnkontrol.Select<dynamic>(vSesiones.sesionAmbienteEnkontrolAdm, queryEk).FirstOrDefault();

                    if (ccDescripcion != null)
                    {
                        numeroEconomico = (string)ccDescripcion.descripcion;
                    }
                }
                else
                {
                    throw new Exception("Se tiene que indicar un CC");
                }
            }

            if (!string.IsNullOrEmpty(numeroEconomico))
            {
                maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.noEconomico == numeroEconomico && x.estatus == 2); //x.estatus == 2 significa que esta en StandBy
            }
            else if (idEconomico.HasValue)
            {
                maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == idEconomico.Value && x.estatus == 2); //x.estatus == 2 significa que esta en StandBy
            }

            if (maquina != null)
            {
                if (_context.tblM_STB_EconomicoBloqueado.Any(x => x.noEconomico == maquina.noEconomico && x.registroActivo))
                {
                    throw new Exception("No es posible realizar la acción puesto que el equipo referenciado se encuentra bloqueado por estatus StandBy");
                }

                var standBy = _context.tblM_STB_CapturaStandBy
                    .FirstOrDefault(x =>
                        x.noEconomicoID == maquina.id &&
                        x.estatus == 2 //Autorizado
                    );

                if (standBy != null)
                {
                    string motivoLiberacion = "";
                    maquina.estatus = 1;
                    standBy.estatus = 4; //Liberado
                    standBy.usuarioLiberaID = vSesiones.sesionUsuarioDTO.id;
                    standBy.fechaLibera = DateTime.Now;
                    standBy.comentarioLiberacion = "Se liberó por sistema - ";
                    switch (accion)
                    {
                        case AccionActivacionEconomicoEnum.ELABORACION_REQUISICION:
                            standBy.comentarioLiberacion += "Se realizó una requisición";
                            motivoLiberacion = "elaboración de requisición";
                            break;
                        case AccionActivacionEconomicoEnum.ELABORACION_ORDEN_COMPRA:
                            standBy.comentarioLiberacion += "Se realizó una orden de compra";
                            motivoLiberacion = "elaboración de orden de compra";
                            break;
                        case AccionActivacionEconomicoEnum.CAPTURA_HOROMETROS:
                            standBy.comentarioLiberacion += "Se capturó horómetros";
                            motivoLiberacion = "captura de horómetros";
                            break;
                        case AccionActivacionEconomicoEnum.CAPTURA_COMBUSTIBLE:
                            standBy.comentarioLiberacion += "Se capturó combustible";
                            motivoLiberacion = "captura de combustible";
                            break;
                        case AccionActivacionEconomicoEnum.CAPTURA_ACEITE:
                            standBy.comentarioLiberacion += "Se capturó aceite";
                            motivoLiberacion = "captura de aceite";
                            break;
                        case AccionActivacionEconomicoEnum.RECEPCION_FACTURA:
                            standBy.comentarioLiberacion += "Por recepción de factura";
                            motivoLiberacion = "recepción de factura";
                            break;
                        case AccionActivacionEconomicoEnum.SALIDA_ALMACEN:
                            standBy.comentarioLiberacion += "Por salida de almacén";
                            motivoLiberacion = "salida de almacén";
                            break;
                    }

                    var bitacora = new tblM_STB_BitacoraActivacionEconomico();
                    bitacora.economicoId = maquina.id;
                    bitacora.fechaAccion = DateTime.Now;
                    bitacora.motivoActivacionId = (int)accion;
                    bitacora.usuarioAccionId = vSesiones.sesionUsuarioDTO.id;
                    bitacora.objeto = JsonUtils.convertNetObjectToJson(objeto);
                    _context.tblM_STB_BitacoraActivacionEconomico.Add(bitacora);
                    _context.SaveChanges();

                    var correos = new List<string>();
                    var correosCC = new List<string>();

                    var adminsGerentes = _context.Select<AutorizanteDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT
                                                u.id,
                                                u.nombre,
                                                u.apellidoPaterno,
                                                u.apellidoMaterno,
                                                u.correo,
                                                c.cc as ac,
                                                a.perfilAutorizaID
                                            FROM
                                                tblP_Autoriza AS a
                                            INNER JOIN
                                                tblP_Usuario AS u ON u.id = a.usuarioID
                                            INNER JOIN
                                                tblP_CC_Usuario AS c ON c.id = a.cc_usuario_ID
                                            WHERE
                                                u.estatus = 1 AND
                                                a.perfilAutorizaID in (5, 1) AND /*5 == Admin, 1 == Gerente*/
                                                c.cc = @paramCC",
                        parametros = new { paramCC = standBy.ccActual }
                    });

                    correosCC.AddRange(adminsGerentes.Select(x => x.correo).Distinct().ToList());

                    correos.Add("oscar.roman@construplan.com.mx");
                    correosCC.Add("g.reina@construplan.com.mx");
                    correosCC.Add("e.encinas@construplan.com.mx");
                    correosCC.Add("luis.fortino@construplan.com.mx");
                    correosCC.Add("martin.valle@construplan.com.mx");
                    correosCC.Add("alan.palomera@construplan.com.mx");
                    correosCC.Add("diego.gonzalez@construplan.com.mx");
#if DEBUG
                    correos = new List<string> { "martin.zayas@construplan.com.mx" };
                    correosCC = new List<string> { "martin.zayas@construplan.com.mx" };
#endif
                    var cc = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == maquina.centro_costos);
                    var ccDescripcion = cc != null ? cc.descripcion.Trim() : maquina.centro_costos;
                    string asunto = "El equipo " + maquina.noEconomico + " ha sido liberado de StandBy por " + motivoLiberacion;
                    string contenido = string.Format(@"
                        <p>Buen día.</p>
                        <p>El equipo <strong>{0}</strong> ha sido liberado de StandBy por {1} </p>
                        <p>El equipo se encuentra en <strong>{2}.</strong>", maquina.noEconomico, motivoLiberacion, ccDescripcion);

                    var envioCorrecto = EnviarCorreo(new CorreoDTO
                    {
                        asunto = asunto,
                        cuerpo = contenido,
                        correos = correos,
                        correosCC = correosCC
                    });

                    if (!envioCorrecto)
                    {
                        throw new Exception("Error al enviar correo de liberación de StandBy");
                    }

                    return true;
                }
                else
                {
                    throw new Exception("El económico esta en StandBy pero no se encuentra su registro autorizado");
                }
            }

            return false;
        }

        public Dictionary<string, object> GetUsuarioTipoAutorizacion()
        {
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    List<tblM_STB_UsuarioTipoAutorizacion> lstUsuarioTipoAutorizacion = _ctx.tblM_STB_UsuarioTipoAutorizacion.Where(w => w.FK_Usuario == vSesiones.sesionUsuarioDTO.id && w.registroActivo).ToList();
                    if (lstUsuarioTipoAutorizacion.Count() <= 0)
                        throw new Exception("No cuenta con los permisos necesarios, favor de notificar a TI.");

                    resultado.Add(SUCCESS, true);
                    resultado.Add("vobo", lstUsuarioTipoAutorizacion.Any(s => s.tipoAutorizacion == (int)TipoAutorizacionEnum.VOBO));
                    resultado.Add("autorizar", lstUsuarioTipoAutorizacion.Any(s => s.tipoAutorizacion == (int)TipoAutorizacionEnum.AUTORIZACION));
                }
                catch (Exception e)
                {
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
                return resultado;
            }
        }

        public Dictionary<string, object> GuardarVoBo(List<StandByNuevoDTO> lstStandByDTO)
        {
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                using (var dbContextTransaction = _ctx.Database.BeginTransaction())
                {
                    resultado = new Dictionary<string, object>();
                    try
                    {
                        #region SE GUARDA VOBO
                        if (lstStandByDTO == null || lstStandByDTO.Count() <= 0) { throw new Exception("Es necesario seleccionar al menos un registro."); }

                        List<int> lstStandByID = lstStandByDTO.Select(s => s.id).ToList();
                        List<tblM_STB_CapturaStandBy> lstStandBy = _ctx.tblM_STB_CapturaStandBy.Where(w => lstStandByID.Contains(w.id)).ToList();
                        foreach (var item in lstStandBy)
                        {
                            item.esVoBo = true;
                            item.FK_UsuarioVoBo = vSesiones.sesionUsuarioDTO.id;
                            item.fechaVoBo = DateTime.Now;
                        }
                        _ctx.SaveChanges();
                        #endregion

                        #region CORREO
                        List<StandByNuevoDTO> lstDetalles = new List<StandByNuevoDTO>();

                        #region STANDBY
                        foreach (var standByGp in lstStandByDTO.GroupBy(x => x.ccActual))
                        {
                            foreach (var standBy in standByGp)
                            {
                                var data = _ctx.tblM_STB_CapturaStandBy.FirstOrDefault(x => x.id == standBy.id);
                                var detalle = new StandByNuevoDTO();
                                detalle.Economico = data.Economico;
                                detalle.modelo = standBy.modelo;
                                detalle.ccActual = data.ccActual.Trim();
                                detalle.ccDescripcion = standBy.ccActual.Trim();
                                detalle.fechaCaptura = data.fechaCaptura.ToShortDateString();
                                detalle.justificacion = data.comentarioJustificacion ?? "";
                                detalle.moiEquipo = data.moiEquipo;
                                detalle.valorEnLibroEquipo = data.valorEnLibroEquipo;
                                detalle.depreciacionMensualEquipo = data.depreciacionMensualEquipo;
                                detalle.valorEnLibroOverhaul = data.valorEnLibroOverhaul;
                                detalle.depreciacionMensualOverhaul = data.depreciacionMensualOverhaul;
                                detalle.comentario = data.comentarioValidacion ?? "";
                                detalle.usuarioCapturaID = data.usuarioCapturaID;
                                lstDetalles.Add(detalle);
                            }
                        }
                        #endregion

                        #region CORREO
                        foreach (var standByGp in lstDetalles.GroupBy(x => x.ccActual))
                        {
                            var adminsGerentes = _ctx.Select<AutorizanteDTO>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT
                                        u.id,
                                        u.nombre,
                                        u.apellidoPaterno,
                                        u.apellidoMaterno,
                                        u.correo,
                                        c.cc as ac,
                                        a.perfilAutorizaID
                                    FROM
                                        tblP_Autoriza AS a
                                    INNER JOIN
                                        tblP_Usuario AS u ON u.id = a.usuarioID
                                    INNER JOIN
                                        tblP_CC_Usuario AS c ON c.id = a.cc_usuario_ID
                                    WHERE
                                        u.estatus = 1 AND
                                        a.perfilAutorizaID in (5, 1) AND /*5 == Admin, 1 == Gerente*/
                                        c.cc = @paramCC",
                                parametros = new { paramCC = standByGp.Key }
                            });

                            List<string> correos = new List<string>();
                            List<string> correosCC = new List<string>();
                            correosCC.AddRange(adminsGerentes.Select(x => x.correo).Distinct().ToList());
                            correos.Add("g.reina@construplan.com.mx");
                            correosCC.Add("oscar.roman@construplan.com.mx");
                            correosCC.Add("martin.valle@construplan.com.mx");

                            string asunto = "Se requiere autorización.";

                            var listaEconomicos = "<p><ul>";
                            foreach (var standBy in standByGp)
                            {
                                listaEconomicos += "<li><strong>" + standBy.Economico + "</strong></li>";

                                string correoCaptura = _ctx.tblP_Usuario.Where(w => w.id == standBy.usuarioCapturaID && w.estatus).Select(s => s.correo).FirstOrDefault();
                                if (!string.IsNullOrEmpty(correoCaptura))
                                    correosCC.Add(correoCaptura);
                            }
                            listaEconomicos += "</ul></p>";

#if DEBUG
                            correos = new List<string> { "martin.zayas@construplan.com.mx" };
                            correosCC = new List<string> { "martin.zayas@construplan.com.mx" };
#endif

                            string contenido = string.Format(@"
                        <p>Buen día.</p>
                        <p>Los siguientes equipos requieren de su autorización para ser colocados en StandBy.</p>
                        {0}", listaEconomicos);

                            using (var excel = new ExcelPackage())
                            {
                                var excelDetalles = excel.Workbook.Worksheets.Add("StandBy");

                                var header = new List<string> { "Equipo", "Modelo", "Obra", "Fecha solicitud", "Justificación", "MOI Equipo", "Falta Dep Equipo", "Dep Mensual Equipo", "Falta Dep OH", "Dep Mensual OH", "Observación" };

                                for (int o = 1; o < header.Count; o++)
                                {
                                    excelDetalles.Cells[1, o].Value = header[o - 1];
                                }

                                var cellData = new List<object[]>();
                                foreach (var standBy in standByGp)
                                {
                                    cellData.Add(new object[]{
                                       standBy.Economico,
                                        standBy.modelo,
                                        standBy.ccDescripcion,
                                        standBy.fechaCaptura,
                                        standBy.justificacion,
                                        standBy.moiEquipo,
                                        standBy.valorEnLibroEquipo,
                                        standBy.depreciacionMensualEquipo,
                                        standBy.valorEnLibroOverhaul,
                                        standBy.depreciacionMensualOverhaul,
                                        standBy.comentario
                                 });
                                }

                                excelDetalles.Cells[2, 1].LoadFromArrays(cellData);

                                ExcelRange range = excelDetalles.Cells[1, 1, excelDetalles.Dimension.End.Row, excelDetalles.Dimension.End.Column];

                                ExcelTable tab = excelDetalles.Tables.Add(range, "Tabla");

                                excelDetalles.Cells[1, 6, excelDetalles.Dimension.End.Row, 10].Style.Numberformat.Format = "$#,##0.00";

                                tab.TableStyle = TableStyles.Medium17;

                                excelDetalles.Cells[excelDetalles.Dimension.Address].AutoFitColumns();

                                excel.Compression = CompressionLevel.BestSpeed;

                                var adjuntos = new List<Attachment>();
                                using (var exportData = new MemoryStream())
                                {
                                    excel.SaveAs(exportData);

                                    var file = exportData.ToArray();
                                    exportData.Close();

                                    adjuntos.Add(new Attachment(new MemoryStream(file), "StandBy - " + standByGp.Key + " - " + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx"));
                                }

                                var envioCorrecto = EnviarCorreo(new CorreoDTO()
                                {
                                    asunto = asunto,
                                    cuerpo = contenido,
                                    correos = correos,
                                    correosCC = correosCC,
                                    archivos = adjuntos
                                });

                                if (!envioCorrecto)
                                {
                                    throw new Exception("Error al enviar el correo de autorización de StandBy");
                                }
                            }
                        }
                        #endregion
                        #endregion

                        resultado.Add(SUCCESS, true);
                        resultado.Add(MESSAGE, string.Format("Se ha indicado {0} con éxito", lstStandBy.Count() == 1 ? "el vobo" : "los vobos"));
                        dbContextTransaction.Commit();

                        // SE VERIFICA SI HAY REGISTROS PENDIENTES DE VOBO.
                        if (!_ctx.tblM_STB_CapturaStandBy.Any(w => !w.esVoBo))
                            EliminarAlerta();

                        // SE VERIFICA SI YA EXISTE ALERTA, SI NO, PARA CREARLA
                        if (!VerificarAlertaVoBoAutorizacion(_GERARDO_REINA))
                            CrearAlerta(_GERARDO_REINA, AlertasEnum.REDIRECCION, "/StandByNuevo/Validacion", -1, "StandBy: Se requiere autorización.");

                        SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, 0, JsonUtils.convertNetObjectToJson(lstStandByDTO));
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                        LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ACTUALIZAR, 0, lstStandByDTO);
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, e.Message);
                    }
                    return resultado;
                }
            }
        }

        private bool EnviarCorreoAutorizante(List<StandByNuevoDTO> lstStandByDTO)
        {
            bool exitoCorreo = true;
            try
            {
                
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                exitoCorreo = false;
            }
            return exitoCorreo;
        }

        #region GENERALES
        private bool CrearAlerta(int userRecibeID, AlertasEnum tipoAlerta, string url, int FK_Registro, string msj)
        {
            bool resultado = true;
            try
            {
                tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                objNuevaAlerta.userEnviaID = vSesiones.sesionUsuarioDTO.id;
                objNuevaAlerta.userRecibeID = userRecibeID;
#if DEBUG
                objNuevaAlerta.userRecibeID = _ADMIN;
#endif
                objNuevaAlerta.tipoAlerta = (int)tipoAlerta;
                objNuevaAlerta.sistemaID = _SISTEMA;
                objNuevaAlerta.visto = false;
                objNuevaAlerta.url = url;
                objNuevaAlerta.objID = FK_Registro;
                objNuevaAlerta.obj = null;
                objNuevaAlerta.msj = msj;
                objNuevaAlerta.documentoID = null;
                objNuevaAlerta.moduloID = _SISTEMA;
                _context.tblP_Alerta.Add(objNuevaAlerta);
                _context.SaveChanges();

                SaveBitacora(0, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(new { userRecibeID = userRecibeID, tipoAlerta = tipoAlerta, url = url, FK_Registro = FK_Registro, msj = msj }));
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                resultado = false;
            }
            return resultado;
        }

        private bool EliminarAlerta()
        {
            bool alertaEliminada = true;
            try
            {
                tblP_Alerta objEliminarAlerta = _context.tblP_Alerta.Where(w => w.userRecibeID == vSesiones.sesionUsuarioDTO.id && w.sistemaID == _SISTEMA && !w.visto && w.objID == -1 && w.moduloID == _SISTEMA).FirstOrDefault();
                if (objEliminarAlerta != null)
                {
                    objEliminarAlerta.visto = true;
                    _context.SaveChanges();
                }

                SaveBitacora(0, (int)AccionEnum.ELIMINAR, 0, JsonUtils.convertNetObjectToJson(objEliminarAlerta));
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ELIMINAR, 0, null);
                alertaEliminada = false;
            }
            return alertaEliminada;
        }

        public bool VerificarAlertaVoBoAutorizacion(int userRecibeID)
        {
            bool existeAlerta = false;
            try
            {
                tblP_Alerta objAlerta = _context.tblP_Alerta.Where(w => !w.visto && w.objID == -1 && w.userRecibeID == userRecibeID && w.moduloID == (int)SistemasEnum.MAQUINARIA).FirstOrDefault();
                if (objAlerta != null)
                    existeAlerta = true;
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                existeAlerta = true;
            }
            return existeAlerta;
        }
        #endregion
    }
}
