using Core.DTO;
using Core.DTO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Inventario.Controles;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Principal.Usuarios;
using Data.Factory.Maquinaria.Captura;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Maquinaria.Inventario;
using Data.Factory.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Inventario
{
    public class ControlesMovimientoEquipoController : BaseController
    {
        private MaquinaFactoryServices maquinaFactoryServices = new MaquinaFactoryServices();
        private CentroCostosFactoryServices centroCostosFactoryServices = new CentroCostosFactoryServices();
        private AsignacionEquiposFactoryServices asignacionEquiposFactoryServices = new AsignacionEquiposFactoryServices();
        private ControlEnvioyRecepcionFactoryServices controlEnvioyRecepcionFactoryService = new ControlEnvioyRecepcionFactoryServices();
        private CapturaHorometroFactoryServices capturaHorometroFactoryServices = new CapturaHorometroFactoryServices();
        private UsuarioFactoryServices usuarioFactoryServices = new UsuarioFactoryServices();
        private GrupoMaquinariaFactoryServices grupoMaquinariaFactoryServices = new GrupoMaquinariaFactoryServices();


        // GET: ControlesMovimientoEquipo
        public ActionResult MovimientosMaquinaria()
        {
            return View();
        }

        public ActionResult LoadTablas(int filtros, string CentroCostos)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var ListaTabla = asignacionEquiposFactoryServices.getAsignacionEquiposFactoryServices().GetAsignacionControles(filtros, CentroCostos);

                var TblData = ListaTabla.Select(x => new
                {
                    Folio = x.folio,
                    CC = x.SolicitudEquipo.CC,
                    CCName = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(x.SolicitudEquipo.CC),
                    Economico = x.noEconomicoID != 0 ? maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(x.noEconomicoID).FirstOrDefault().noEconomico : "",
                    Fecha = x.FechaPromesa.ToShortDateString(),
                    SolicitudDetalleID = x.SolicitudDetalleId,
                    EconomicoID = x.noEconomicoID,
                    AsignacionID = x.id
                });

                result.Add("TblData", TblData);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDataFromEconomico(int AsignacionID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                tblM_CatMaquina objMaquina = new tblM_CatMaquina();
                DetalleControlDTO objDetalleControlDTO = new DetalleControlDTO();

                var Asignacion = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().getInfoAsignacion(AsignacionID);
                string CCEnvia = "";
                string CCRecibe = "";
                if (Asignacion != null)
                {
                    if (Asignacion.noEconomicoID != 0)
                    {
                        objMaquina = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().GetInfoMaquinaria(Asignacion.noEconomicoID);
                        var CCEnviaObj = Asignacion.CCOrigen; //usuarioFactoryServices.getUsuarioService().getPerfilesUsuario(1,Asignacion.CCOrigen);
                        var CCRecibeObj = Asignacion.cc;//  usuarioFactoryServices.getUsuarioService().getPerfilesUsuario(2,Asignacion.cc);
                        CCEnvia = Asignacion.CCOrigen;
                        CCRecibe = Asignacion.cc;

                        if (objMaquina != null)
                        {
                            var control = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().getInfoControl(Asignacion.id, 1, Asignacion.solicitudEquipoID);
                            objDetalleControlDTO.TipoCaptura = objMaquina.TipoCaptura;
                            objDetalleControlDTO.UsuarioEnviaNombre = control == null ? "" : control.nombreResponsableEnvio ;
                            objDetalleControlDTO.UsuarioRecibeNombre = control == null ? "" : control.nombreResponsableRecepcion ;

                            var UltimoHorometro = capturaHorometroFactoryServices.getCapturaHorometroServices().GetHorometroFinal(objMaquina.noEconomico);

                            var archivoSOS = objMaquina.grupoMaquinaria.sos;
                            var archivoBitacora = objMaquina.grupoMaquinaria.bitacora;
                            var archivoDN = objMaquina.grupoMaquinaria.dn;
                            var archivoSetFotografico = objMaquina.grupoMaquinaria.setFotografico;
                            var archivoRehabilitacion = objMaquina.grupoMaquinaria.rehabilitacion;


                            result.Add("UltimoHorometro", UltimoHorometro);

                            result.Add("archivoSOS", archivoSOS);
                            result.Add("archivoBitacora", archivoBitacora);
                            result.Add("archivoDN", archivoDN);
                            result.Add("archivoSetFotografico", archivoSetFotografico);
                            result.Add("archivoRehabilitacion", archivoRehabilitacion);

                        }

                        result.Add("Compania", "CONSTRUPLAN");
                    }
                    else
                    {
                        result.Add("Compania", "PROVEEDOR");
                    }
                }
                result.Add("objDetalleControlDTO", objDetalleControlDTO);
                result.Add("CCusuarioEnvia", Asignacion.CCOrigen);
                result.Add("CCusuarioRecibe", Asignacion.cc);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboResponsableControles(string cc, int tipo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<cboDTO> cboDTOList = new List<cboDTO>();
                List<tblP_Autoriza> cboResponsables = new List<tblP_Autoriza>();
                if (tipo == 1)
                {
                    cc = "1010";
                    cboResponsables = usuarioFactoryServices.getUsuarioService().getPerfilesUsuario(1, cc);


                    cboDTOList = cboResponsables.Select(x => new cboDTO { Value = x.usuarioID.ToString(), Text = x.usuario.nombre + " " + x.usuario.apellidoPaterno }).ToList();

                    cboDTOList.Add(new cboDTO
                    {
                        Value = vSesiones.sesionUsuarioDTO.id.ToString(),
                        Text = Infrastructure.Utils.PersonalUtilities.NombreCompleto(vSesiones.sesionUsuarioDTO.nombre, vSesiones.sesionUsuarioDTO.apellidoPaterno, vSesiones.sesionUsuarioDTO.apellidoMaterno)
                    });

                    cboDTOList.Add(new cboDTO { Value = "0", Text = "Proveedor" });

                    cc = "1015";
                    cboResponsables = usuarioFactoryServices.getUsuarioService().getPerfilesUsuario(1, cc);

                    var cboDTOList2 = cboResponsables.Select(x => new cboDTO { Value = x.usuarioID.ToString(), Text = x.usuario.nombre + " " + x.usuario.apellidoPaterno }).ToList();

                    cboDTOList.AddRange(cboDTOList2);
                }
                else
                {
                    cboResponsables = usuarioFactoryServices.getUsuarioService().getPerfilesUsuario(1, cc);
                    cboDTOList = cboResponsables.Select(x => new cboDTO { Value = x.usuarioID.ToString(), Text = x.usuario.nombre + " " + x.usuario.apellidoPaterno }).ToList();

                    cboDTOList.Add(new cboDTO
                    {
                        Value = vSesiones.sesionUsuarioDTO.id.ToString(),
                        Text = Infrastructure.Utils.PersonalUtilities.NombreCompleto(vSesiones.sesionUsuarioDTO.nombre, vSesiones.sesionUsuarioDTO.apellidoPaterno, vSesiones.sesionUsuarioDTO.apellidoMaterno)
                    });

                    cboDTOList.Add(new cboDTO { Value = "0", Text = "Proveedor" });
                }
                var distinctKust = cboDTOList.GroupBy(c => c.Value, (key, c) => c.FirstOrDefault()).ToList();
                result.Add(ITEMS, distinctKust);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult fillCboTransporte()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cboListaEconomicos = maquinaFactoryServices.getMaquinaServices().FillCboEconomicos(158); //158

                List<cboDTO> cboDTOList = cboListaEconomicos.Select(x => new cboDTO { Value = x.id.ToString(), Text = x.noEconomico }).ToList();

                cboDTOList.Add(new cboDTO
                {
                    Value = "0",
                    Text = "PROVEEDOR"
                });

                cboDTOList.Add(new cboDTO
                {
                    Value = "0",
                    Text = "IMPULSO PROPIO"
                });

                result.Add(ITEMS, cboDTOList);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}