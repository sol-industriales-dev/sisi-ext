using Core.DAO.Administracion.Seguridad.Capacitacion;
using Core.DTO;
using Core.DTO.Administracion.Seguridad.Capacitacion.CincoS;
using Core.DTO.Enkontrol.Tablas.CC;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Seguridad.Capacitacion;
using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using Core.Entity.Administrativo.Seguridad.Capacitacion.CincoS;
using Core.Entity.Principal.Multiempresa;
using Core.Entity.Principal.Usuarios;
using Core.Enum.Administracion.Seguridad.Capacitacion.CincoS;
using Data.EntityFramework.Context;
using Data.Factory.Enkontrol.General.CC;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Core.DTO.Reportes;
using System.Net.Mail;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;

namespace Data.DAO.Administracion.Seguridad.Capacitacion
{
    public class CincoSDAO : GenericDAO<tblP_Usuario>, ICincoSDAO
    {
        private readonly string RutaBase = @"\\REPOSITORIO\Proyecto\SIGOPLAN\CAPACITACION_5S";
        private const string RutaLocal = @"C:\Proyecto\SIGOPLAN\CAPACITACION_5S";

        Dictionary<string, object> resultado = new Dictionary<string, object>();

        const string SUCCESS = "success";
        const string MESSAGE = "message";
        const string ITEMS = "items";
        const string NombreControlador = "CincoSDAO";

        #region CHECKLIST
        public Dictionary<string, object> GetCCs(ConsultaCCsEnum consulta, int? checkListId)
        {
            var ccFS = new CCFactoryService().getCCServiceSP();

            using (var ctx = new MainContext())
            {
                try
                {
                    var ccs = new List<ComboDTO>();

                    switch (consulta)
                    {
                        case ConsultaCCsEnum.Todos:
                            {
                                ccs = ccFS.GetCCsNomina(null).Select(x => new ComboDTO
                                {
                                    Value = x.cc,
                                    Text = x.descripcion
                                }).ToList();
                            }
                            break;
                        case ConsultaCCsEnum.TodosLosActivos:
                            {
                                ccs = ccFS.GetCCsNomina(true).Select(x => new ComboDTO
                                {
                                    Value = x.cc,
                                    Text = x.descripcion
                                }).ToList();
                            }
                            break;
                        case ConsultaCCsEnum.TodosConCheckListCreado:
                            {
                                var ccsCheckLists = ctx.tbl5s_CC
                                    .Where(x =>
                                        x.registroActivo &&
                                        x.checkList.registroActivo)
                                    .Select(x => x.cc).Distinct().ToList();
                                ccs = ccFS.GetCCsNominaFiltrados(ccsCheckLists).Select(x => new ComboDTO
                                {
                                    Value = x.cc,
                                    Text = x.descripcion
                                }).ToList();
                            }
                            break;
                        case ConsultaCCsEnum.LosDelCheckList:
                            {
                                var ccsCheckList = ctx.tbl5s_CC
                                    .Where(x =>
                                        x.registroActivo &&
                                        x.checkListId == checkListId.Value &&
                                        x.checkList.registroActivo)
                                    .Select(x => x.cc).Distinct().ToList();
                                ccs = ccFS.GetCCsNominaFiltrados(ccsCheckList).Select(x => new ComboDTO
                                {
                                    Value = x.cc,
                                    Text = x.descripcion
                                }).ToList();
                            }
                            break;
                        case ConsultaCCsEnum.TodosConAuditorCreado:
                            {
                                var ccsAuditores = ctx.tbl5s_CC_Usuario
                                    .Where(x =>
                                        x.registroActivo &&
                                        x.usuario.registroActivo)
                                    .Select(x => x.cc).Distinct().ToList();
                                ccs = ccFS.GetCCsNominaFiltrados(ccsAuditores).Select(x => new ComboDTO
                                {
                                    Value = x.cc,
                                    Text = x.descripcion
                                }).ToList();
                            }
                            break;
                        case ConsultaCCsEnum.TieneAuditoria:
                            {
                                var inspecciones = ctx.tbl5s_AuditoriasDet.Select(x => x.inspeccionId).ToList();
                                var ccInspecciones = ctx.tbl5s_Inspeccion.Where(x => inspecciones.Contains(x.id)).Select(x => x.checkList).SelectMany(x => x.ccs).Select(x => x.cc).ToList();
                                ccs = ccFS.GetCCsNominaFiltrados(ccInspecciones).Select(x => new ComboDTO
                                {
                                    Value = x.cc,
                                    Text = x.descripcion
                                }).ToList();
                                break;
                            }
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, ccs);
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }


        public Dictionary<string, object> GetCheckLists(List<string> ccs)
        {
            var ccFS = new CCFactoryService().getCCServiceSP();

            using (var ctx = new MainContext())
            {
                try
                {
                    var checkLists = ctx.tbl5s_CheckList
                        .Where(x => x.registroActivo).ToList().Where(x => 
                            x.ccs.Any(y => ccs.Contains(y.cc) && x.registroActivo)
                            )
                        .Select(x => new TablaCheckListCreadoDTO
                        {
                            checkListId = x.id,
                            ccSinCalendario = x.ccs.Where(y => y.calendario == null || y.calendario.Count() == 0).Select(y => y.cc).ToList(),
                            ccConCalendario = x.ccs.Where(y => y.calendario.Count() > 0 && y.calendario.Where(e => e.registroActivo).Count() > 0).Select(y => y.cc).ToList(),
                            nombreAuditoria = x.nombre,
                            area = x.area.nombre,
                            lideres = x.lideres
                                .Where(y =>
                                    y.registroActivo &&
                                    y.usuario.registroActivo)
                                .Select(m =>
                                    PersonalUtilities.NombreCompletoMayusculas
                                    (m.usuario.usuario.nombre, m.usuario.usuario.apellidoPaterno, m.usuario.usuario.apellidoMaterno)
                                ).ToList()
                        }).ToList();

                    foreach (var cl in checkLists)
                    {
                        var ccsCheckList = ccFS.GetCCsNominaFiltrados(cl.ccConCalendario);
                        ccsCheckList.AddRange(ccFS.GetCCsNominaFiltrados(cl.ccSinCalendario));

                        for (int i = 0; i < cl.ccSinCalendario.Count; i++)
                        {
                            var ccItem = ccsCheckList.FirstOrDefault(x => x.cc == cl.ccSinCalendario[i]);
                            if (ccItem != null)
                            {
                                cl.ccSinCalendario[i] += " - " + ccItem.descripcion;
                            }
                        }

                        for (int i = 0; i < cl.ccConCalendario.Count; i++)
                        {
                            var ccItem = ccsCheckList.FirstOrDefault(x => x.cc == cl.ccConCalendario[i]);
                            if (ccItem != null)
                            {
                                cl.ccConCalendario[i] += " - " + ccItem.descripcion;
                            }
                        }
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, checkLists);
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetCheckList(int checkListId)
        {
            using (var ctx = new MainContext())
            {
                try
                {
                    var checkList = ctx.tbl5s_CheckList.FirstOrDefault(x => x.id == checkListId && x.registroActivo);
                    if (checkList != null)
                    {
                        var datos = new CheckListInfoDTO();
                        datos.id = checkListId;
                        datos.nombre = checkList.nombre;
                        datos.ccs = checkList.ccs.Where(x => x.registroActivo).Select(x => x.cc).ToList();
                        datos.area = checkList.areaId;
                        datos.lideres = checkList.lideres.Where(x => x.registroActivo).Select(x => x.usuario5sId).ToList();

                        var inspeccionesInfo = new List<InspeccionesDTO>();
                        foreach (var item in checkList.inspecciones.Where(x => x.registroActivo))
                        {
                            var inspeccion = new InspeccionesDTO();
                            inspeccion.id = item.id;
                            inspeccion.inspeccion = item.inspeccion;
                            inspeccion.area = checkList.area.id;
                            inspeccion.areaDesc = checkList.area.nombre;
                            inspeccion.subAreaId = item.subAreaId;
                            inspeccion.subAreaDesc = item.subArea.descripcion;
                            inspeccion.subAreaDescripcion = item.subArea.descripcion;
                            inspeccion.cincoS = item.detalles.Where(x => x.registroActivo).Select(x => x.cincoS.id).ToList();
                            inspeccionesInfo.Add(inspeccion);

                            datos.cincoS_clasificar += item.detalles.Where(x => x.registroActivo && x.cincoId == (int)CincoSEnum.Clasificar).Count();
                            datos.cincoS_orden += item.detalles.Where(x => x.registroActivo && x.cincoId == (int)CincoSEnum.Orden).Count();
                            datos.cincoS_limpieza += item.detalles.Where(x => x.registroActivo && x.cincoId == (int)CincoSEnum.Limpieza).Count();
                            datos.cincoS_estandarizacion += item.detalles.Where(x => x.registroActivo && x.cincoId == (int)CincoSEnum.Estandarizar).Count();
                            datos.cincoS_disciplina += item.detalles.Where(x => x.registroActivo && x.cincoId == (int)CincoSEnum.Disciplina).Count();
                        }

                        datos.inspecciones = inspeccionesInfo;

                        datos.cincoS_total = datos.cincoS_clasificar + datos.cincoS_orden + datos.cincoS_limpieza + datos.cincoS_estandarizacion + datos.cincoS_disciplina;

                        resultado.Add(SUCCESS, true);
                        resultado.Add(ITEMS, datos);
                    }
                    else
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se encontró información del check list");
                    }
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarCheckList(CheckListGuardarDTO checkList)
        {
            using (var ctx = new MainContext())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        var checkListEntity = new tbl5s_CheckList();
                        checkListEntity.nombre = checkList.nombre;
                        checkListEntity.areaId = checkList.areaId;
                        checkListEntity.fechaCreacion = checkList.id == 0 ? DateTime.Now : checkList.fechaCreacion;
                        checkListEntity.fechaModificacion = checkList.id == 0 ? (DateTime?)null : checkList.fechaModificacion;
                        checkListEntity.usuarioCreacionId = checkList.id == 0 ? vSesiones.sesionUsuarioDTO.id : checkList.usuarioCreacionId;
                        checkListEntity.usuarioModificacionId = checkList.id == 0 ? (int?)null : checkList.usuarioModificacionId;
                        checkListEntity.registroActivo = true;
                        ctx.tbl5s_CheckList.Add(checkListEntity);
                        ctx.SaveChanges();

                        var ccsEntity = new List<tbl5s_CC>();
                        foreach (var item in checkList.ccs)
                        {
                            var ccEntity = new tbl5s_CC();
                            ccEntity.cc = item;
                            ccEntity.checkListId = checkListEntity.id;
                            ccEntity.fechaCreacion = checkListEntity.fechaCreacion;
                            ccEntity.fechaModificacion = checkListEntity.fechaModificacion;
                            ccEntity.usuarioCreacionId = checkListEntity.usuarioCreacionId;
                            ccEntity.usuarioModificacionId = checkListEntity.usuarioModificacionId;
                            ccEntity.registroActivo = true;
                            ccsEntity.Add(ccEntity);
                        }
                        ctx.tbl5s_CC.AddRange(ccsEntity);
                        ctx.SaveChanges();

                        var lideresEntity = new List<tbl5s_LiderArea>();
                        foreach (var item in checkList.lideresId)
                        {
                            var liderEntity = new tbl5s_LiderArea();
                            liderEntity.checkListId = checkListEntity.id;
                            liderEntity.usuario5sId = item;
                            liderEntity.fechaCreacion = checkListEntity.fechaCreacion;
                            liderEntity.fechaModificacion = checkListEntity.fechaModificacion;
                            liderEntity.usuarioCreacionId = checkListEntity.usuarioCreacionId;
                            liderEntity.usuarioModificacionId = checkListEntity.usuarioModificacionId;
                            liderEntity.registroActivo = true;
                            lideresEntity.Add(liderEntity);
                        }
                        ctx.tbl5s_LiderArea.AddRange(lideresEntity);
                        ctx.SaveChanges();

                        if (checkList.inspecciones != null)
                        {
                            foreach (var item in checkList.inspecciones)
                            {
                                var inspeccionEntity = new tbl5s_Inspeccion();
                                inspeccionEntity.checkListId = checkListEntity.id;
                                inspeccionEntity.inspeccion = item.inspeccion;
                                inspeccionEntity.subAreaId = item.subAreaId;
                                inspeccionEntity.fechaCreacion = checkListEntity.fechaCreacion;
                                inspeccionEntity.fechaModificacion = checkListEntity.fechaModificacion;
                                inspeccionEntity.usuarioCreacionId = checkListEntity.usuarioCreacionId;
                                inspeccionEntity.usuarioModificacionId = checkListEntity.usuarioModificacionId;
                                inspeccionEntity.registroActivo = true;
                                ctx.tbl5s_Inspeccion.Add(inspeccionEntity);
                                ctx.SaveChanges();

                                foreach (var cincoS in item.cincoS)
                                {
                                    var cincoSEntity = new tbl5s_InspeccionDet();
                                    cincoSEntity.inspeccionId = inspeccionEntity.id;
                                    cincoSEntity.cincoId = cincoS;
                                    cincoSEntity.fechaCreacion = checkListEntity.fechaCreacion;
                                    cincoSEntity.fechaModificacion = checkListEntity.fechaModificacion;
                                    cincoSEntity.usuarioCreacionId = checkListEntity.usuarioCreacionId;
                                    cincoSEntity.usuarioModificacionId = checkListEntity.usuarioModificacionId;
                                    cincoSEntity.registroActivo = true;
                                    ctx.tbl5s_InspeccionDet.Add(cincoSEntity);
                                    ctx.SaveChanges();
                                }
                            }   
                        }

                        transaction.Commit();

                        resultado.Add(SUCCESS, true);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, ex.Message);
                    }
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EliminarCheckList(int checkListId)
        {
            using (var ctx = new MainContext())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        var checkList = ctx.tbl5s_CheckList.FirstOrDefault(x => x.id == checkListId);
                        if (checkList != null)
                        {
                            checkList.registroActivo = false;
                            checkList.fechaModificacion = DateTime.Now;
                            checkList.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                            ctx.SaveChanges();

                            foreach (var item in checkList.ccs.Where(x => x.registroActivo))
                            {
                                item.registroActivo = false;
                                item.fechaModificacion = checkList.fechaModificacion;
                                item.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                                ctx.SaveChanges();

                                foreach (var itemCal in item.calendario.Where(e => e.registroActivo))
                                {
                                    itemCal.registroActivo = false;
                                    itemCal.fechaModificacion = checkList.fechaModificacion;
                                    itemCal.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                                    ctx.SaveChanges();
                                }
                                
                            }

                            foreach (var item in checkList.lideres.Where(x => x.registroActivo))
                            {
                                item.registroActivo = false;
                                item.fechaModificacion = checkList.fechaModificacion.Value;
                                item.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                                ctx.SaveChanges();
                            }

                            foreach (var item in checkList.inspecciones.Where(x => x.registroActivo))
                            {
                                item.registroActivo = false;
                                item.fechaModificacion = checkList.fechaModificacion;
                                item.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                                ctx.SaveChanges();

                                foreach (var cincoS in item.detalles.Where(x => x.registroActivo))
                                {
                                    cincoS.registroActivo = false;
                                    cincoS.fechaModificacion = checkList.fechaModificacion;
                                    cincoS.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                                    ctx.SaveChanges();
                                }
                            }

                            transaction.Commit();

                            resultado.Add(SUCCESS, true);
                        }
                        else
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "No se encuentra información del check list");
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, ex.Message);
                    }
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EditarCheckList(CheckListGuardarDTO checkList)
        {
            using (var ctx = new MainContext())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        var cL = ctx.tbl5s_CheckList.FirstOrDefault(x => x.id == checkList.id);
                        if (cL != null)
                        {
                            var resultadoEliminar = EliminarCheckList(checkList.id);
                            if ((bool)resultadoEliminar[SUCCESS])
                            {
                                resultado.Clear();
                                checkList.fechaCreacion = cL.fechaCreacion;
                                checkList.fechaModificacion = DateTime.Now;
                                checkList.usuarioCreacionId = cL.usuarioCreacionId;
                                checkList.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                                resultado = GuardarCheckList(checkList);
                            }
                        }
                        else
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "No se encontró el check list a editar");
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, ex.Message);
                    }
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetAreas()
        {
            using (var ctx = new MainContext())
            {
                try
                {
                    var areas = ctx.tbl5s_Area.Where(x => x.registroActivo).Select(x => new ComboDTO
                    {
                        Value = x.id.ToString(),
                        Text = x.nombre
                    }).ToList();

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, areas);
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetLideres()
        {
            using (var ctx = new MainContext())
            {
                try
                {
                    var lideres = ctx.tbl5s_Usuario
                        .Where(x =>
                            x.registroActivo &&
                            x.privilegioId.HasValue &&
                            x.privilegioId.Value == (int)PrivilegioEnum.Lider).ToList()
                        .Select(x => new ComboDTO
                        {
                            Value = x.id.ToString(),
                            Text = PersonalUtilities.NombreCompletoMayusculas
                                (x.usuario.nombre, x.usuario.apellidoPaterno, x.usuario.apellidoMaterno)
                        }).ToList();

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, lideres);
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetSubAreas()
        {
            using (var ctx = new MainContext())
            {
                try
                {
                    var subAreas = ctx.tbl5s_SubArea.Where(x => x.registroActivo).Select(x => new ComboDTO
                    {
                        Value = x.id.ToString(),
                        Text = x.descripcion
                    }).ToList();

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, subAreas);
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetCalendarioCheckList(int checkListId)
        {
            var ccFS = new CCFactoryService().getCCServiceSP();

            using (var ctx = new MainContext())
            {
                try
                {
                    var calendarioEntity = ctx.tbl5s_Calendario
                        .Where(x =>
                            x.registroActivo &&
                            x.cc.checkListId == checkListId &&
                            x.cc.registroActivo &&
                            x.cc.checkList.registroActivo).ToList();

                    var checkL = calendarioEntity.Count > 0 ? calendarioEntity.First().cc.checkList : ctx.tbl5s_CheckList.First(x => x.registroActivo && x.id == checkListId);

                    var calendario = new CalendarioCheckListDTO();
                    calendario.id = checkListId;
                    calendario.nombre = checkL.nombre;

                    var ccs = checkL.ccs.Where(x => x.registroActivo).Select(x => new
                    {
                        x.id,
                        x.cc
                    }).ToList();

                    var ccsInfo = ccFS.GetCCsNominaFiltrados(ccs.Select(x => x.cc).ToList());
                    var datosCC = new List<ComboDTO>();

                    for (int i = 0; i < ccs.Count; i++)
                    {
                        var cc = ccsInfo.FirstOrDefault(x => x.cc == ccs[i].cc);
                        if (cc != null)
                        {
                            var combo = new ComboDTO();
                            combo.Value = ccs[i].id.ToString();
                            combo.Text = cc.cc + " - " + cc.descripcion;
                            combo.Prefijo = calendarioEntity.Any(x => x.cc5sId == ccs[i].id) ? "seleccionado" : "";
                            datosCC.Add(combo);
                        }
                    }
                    calendario.ccs = datosCC;

                    var fechas = new List<ComboDTO>();
                    foreach (var item in calendarioEntity.GroupBy(x => x.fecha))
                    {
                        var combo = new ComboDTO();
                        combo.Value = item.Key.ToShortDateString();
                        combo.Text = item.Key.ToShortDateString();
                        fechas.Add(combo);
                    }
                    calendario.fechas = fechas;

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, calendario);
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarCalendarioCheckList(CalendarioCheckListDTO calendario)
        {
            using (var ctx = new MainContext())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        var calendarioActual = ctx.tbl5s_Calendario
                            .Where(x =>
                                x.registroActivo &&
                                x.cc.registroActivo &&
                                x.cc.checkList.registroActivo &&
                                x.cc.checkListId == calendario.checkListId).ToList();

                        var fechaActual = DateTime.Now;

                        //Elimina cc del calendario
                        foreach (var item in calendarioActual)
                        {
                            if (!calendario.ccs.Any(x => Convert.ToInt32(x.Value) == item.cc5sId))
                            {
                                item.registroActivo = false;
                                item.fechaModificacion = fechaActual;
                                item.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                                ctx.SaveChanges();
                            }
                        }

                        //Elimina fechas del calendario
                        foreach (var item in calendario.fechas)
                        {
                            calendario.fechasDateTime.Add(Convert.ToDateTime(item.Value));
                        }
                        foreach (var item in calendario.fechasDateTime)
                        {
                            calendario.fechasString.Add(item.ToShortDateString());
                        }
                        foreach (var item in calendarioActual)
                        {
                            var fechaReg = item.fecha.ToShortDateString();
                            if (!calendario.fechasString.Any(x => x == fechaReg))
                            {
                                item.registroActivo = false;
                                item.fechaModificacion = fechaActual;
                                item.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                                ctx.SaveChanges();
                            }
                        }

                        calendarioActual = ctx.tbl5s_Calendario
                            .Where(x =>
                                x.registroActivo &&
                                x.cc.registroActivo &&
                                x.cc.checkList.registroActivo &&
                                x.cc.checkListId == calendario.checkListId).ToList();

                        //Agregar cc y fechas al calendario
                        foreach (var ccNuevo in calendario.ccs.Where(x => x.Prefijo == "seleccionado"))
                        {
                            var idCcNuevo = Convert.ToInt32(ccNuevo.Value);
                            var objCC = ctx.tbl5s_CC.FirstOrDefault(e => e.checkListId == calendario.checkListId && idCcNuevo == e.id);

                            if (objCC != null)
                            {
                                foreach (var fecha in calendario.fechasString)
                                {
                                    if (!calendarioActual.Any(x => x.cc5sId == objCC.id && x.fecha == Convert.ToDateTime(fecha)))
                                    {
                                        var calendarioEntity = new tbl5s_Calendario();
                                        calendarioEntity.cc5sId = objCC.id;
                                        calendarioEntity.fecha = Convert.ToDateTime(fecha);
                                        calendarioEntity.fechaCreacion = fechaActual;
                                        calendarioEntity.usuarioCreacionId = vSesiones.sesionUsuarioDTO.id;
                                        calendarioEntity.registroActivo = true;
                                        ctx.tbl5s_Calendario.Add(calendarioEntity);
                                        ctx.SaveChanges();
                                    }
                                }    
                            }
                        }

                        transaction.Commit();

                        resultado.Add(SUCCESS, true);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, ex.Message);
                    }
                }
            }

            return resultado;
        }


        #endregion

        #region CALENDARIO
        public Dictionary<string, object> GetCalendarios(List<string> ccsFiltro, int añoFiltro)
        {
            resultado.Clear();

            using (var ctx = new MainContext())
            {
                try
                {
                    var calendariosEntity = ctx.tbl5s_Calendario
                    .Where(x =>
                        x.registroActivo &&
                        ccsFiltro.Contains(x.cc.cc) &&
                        x.cc.registroActivo &&
                        x.cc.checkList.registroActivo &&
                        x.fecha.Year == añoFiltro).ToList();

                    var calendarios = new List<CalendarioCheckListDTO>();


                    var calendariosGrpCheckList = calendariosEntity.GroupBy(e => e.cc.checkListId).ToList();

                    foreach (var item in calendariosGrpCheckList)
                    {
                        var objCanlendarioDTO = new CalendarioCheckListDTO();
                        var objCheckList = ctx.tbl5s_CheckList.FirstOrDefault(e => e.id == item.Key);

                        if (objCheckList != null)
                        {
                            objCanlendarioDTO.nombre = objCheckList.nombre;

                        }
                        else
                        {
                            objCanlendarioDTO.nombre = "S/N";

                        }

                        var fechas = new List<ComboDTO>();
                        foreach (var itemCalendario in item)
                        {
                            var combo = new ComboDTO();
                            combo.Value = itemCalendario.fecha.ToShortDateString();
                            combo.Text = itemCalendario.fecha.ToShortDateString();
                            fechas.Add(combo);

                        }
                        objCanlendarioDTO.fechas = fechas;
                        calendarios.Add(objCanlendarioDTO);

                    }
                    resultado.Add(ITEMS, calendarios);
                    resultado.Add(SUCCESS, true);

                }
                catch (Exception e)
                {
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);                        
                }
            }

            return resultado;
        }

        #endregion

        #region AUDITORIAS 5'S
        /// <summary>
        /// SE REGISTRA LA AUDITORIA COMO REGISTRO PRINCIPAL Y SU DETALLE. SE REGISTRA EN UNA TERCERA TABLA LOS ARCHIVOS DE DICHA AUDITORIA.
        /// </summary>
        public Dictionary<string, object> GetAuditorias(AuditoriaDTO objParamDTO)
        {
            using (var ctx = new MainContext())
            {
                try
                {
                    #region SE OBTIENE LISTADO DE AUDITORIAS
                    List<tbl5s_CheckList> lstCheckList = ctx.tbl5s_CheckList.Where(w => w.registroActivo).ToList();
                    List<tbl5s_Auditorias> lstAuditorias = ctx.tbl5s_Auditorias.Where(w => w.registroActivo).ToList();

                    #region FILTROS
                    if (!string.IsNullOrEmpty(objParamDTO.cc))
                        lstAuditorias = lstAuditorias.Where(w => objParamDTO.cc.Contains(w.cc)).ToList();

                    if (objParamDTO.fechaInicio != null)
                        lstAuditorias = lstAuditorias.Where(w => w.fecha >= objParamDTO.fechaInicio).ToList();

                    if (objParamDTO.fechaFinal != null)
                        lstAuditorias = lstAuditorias.Where(w => w.fecha <= objParamDTO.fechaFinal).ToList();
                    #endregion

                    List<tbl5s_Area> lstAreas = ctx.tbl5s_Area.Where(w => w.registroActivo).ToList();
                    List<tbl5s_Usuario> lstUsuarios5s = ctx.tbl5s_Usuario.Where(w => w.registroActivo).ToList();
                    List<tblP_Usuario> lstUsuarios = ctx.tblP_Usuario.ToList();
                    List<tblP_CC> lstCC = ctx.tblP_CC.ToList();

                    List<AuditoriaDTO> lstAuditoriasDTO = new List<AuditoriaDTO>();
                    AuditoriaDTO objAuditoriaDTO = new AuditoriaDTO();
                    foreach (var item in lstAuditorias)
                    {
                        tbl5s_CheckList objCheckList = lstCheckList.Where(w => w.id == item.checkListId && w.registroActivo).FirstOrDefault();
                        tbl5s_Usuario objUsuario5s = lstUsuarios5s.Where(w => w.id == item.usuario5sId && w.registroActivo).FirstOrDefault();
                        if (objCheckList != null)
                        {
                            objAuditoriaDTO = new AuditoriaDTO();
                            objAuditoriaDTO.id = item.id;
                            objAuditoriaDTO.nombreAuditoria = objCheckList.nombre.Trim();
                            objAuditoriaDTO.auditor = GetNombreAuditor(item.usuario5sId, lstUsuarios5s, lstUsuarios);
                            objAuditoriaDTO.cc = GetCC_Descripcion(item.cc, lstCC);
                            objAuditoriaDTO.area = objCheckList.area.nombre.Trim();
                            objAuditoriaDTO.porcCumplimiento = GetPorcCumplimientoAuditoria(item.id);
                            objAuditoriaDTO.fecha = item.fecha;
                            objAuditoriaDTO.checkListId = item.checkListId;
                            objAuditoriaDTO.auditoriaCompleta = item.auditoriaCompleta;
                            lstAuditoriasDTO.Add(objAuditoriaDTO);
                        }
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add("lstAuditorias", lstAuditoriasDTO);
                    #endregion
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }
            return resultado;
        }

        /// <summary>
        /// SE REGISTRA LA AUDITORIA COMO REGISTRO PRINCIPAL Y SU DETALLE. SE REGISTRA EN UNA TERCERA TABLA LOS ARCHIVOS DE DICHA AUDITORIA.
        /// </summary>
        public Dictionary<string, object> CrearEditarAuditoria(AuditoriaDTO objParamDTO, List<HttpPostedFileBase> lstDetecciones, List<HttpPostedFileBase> lstMedidas, List<int> lstIndice_Detecciones, List<int> lstIndice_Medidas)
        {
            using (var ctx = new MainContext())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        if (objParamDTO.auditoriaCompleta)
                        {
                            #region VALIDACIONES
                            // AUDITORIAS
                            if (objParamDTO.checkListId <= 0) { throw new Exception("Es necesario seleccionar una auditoria."); }
                            if (string.IsNullOrEmpty(objParamDTO.cc)) { throw new Exception("Es necesario seleccionar un CC."); }
                            if (objParamDTO.fecha == null) { throw new Exception("Es necesario indicar la fecha."); }
                            if (objParamDTO.usuarioAuditorId <= 0) { throw new Exception("Es necesario seleccionar al auditor."); }

                            // AUDITORIAS DET
                            if (objParamDTO.lstAuditoriaDet != null)
                            {
                                foreach (var item in objParamDTO.lstAuditoriaDet)
                                {
                                    if (item.inspeccionId <= 0) { throw new Exception("Ocurrió un error durante el registro."); }
                                    if (string.IsNullOrEmpty(item.descripcion)) { throw new Exception("Es necesario indicar la descripción."); }
                                    switch ((int)item.respuesta)
                                    {
                                        case (int)RespuestaEnum.PENDIENTE:
                                            throw new Exception("Es necesario seleccionar una respuesta.");
                                        case (int)RespuestaEnum.OK:
                                            if (item.usuario5sId <= 0) { throw new Exception("Es necesario seleccionar al líder."); }
                                            break;
                                        case (int)RespuestaEnum.NO_OK:
                                            if (item.usuario5sId <= 0) { throw new Exception("Es necesario seleccionar al líder."); }
                                            if (item.fecha == null) { throw new Exception("Es necesario indicar la fecha de la columna: Fecha / Líder."); }
                                            break;
                                    }
                                    //if (string.IsNullOrEmpty(item.accion)) { throw new Exception("Es necesario indicar la acción."); }
                                }
                            }
                            #endregion
                        }

                        if (objParamDTO.id <= 0)
                        {
                            #region SE REGISTRA LA AUDITORIA
                            // REGISTRO PRINCIPAL
                            tbl5s_Auditorias objAuditoria = new tbl5s_Auditorias();
                            objAuditoria.checkListId = objParamDTO.checkListId;
                            objAuditoria.cc = objParamDTO.cc.Trim();
                            objAuditoria.fecha = objParamDTO.fecha;
                            objAuditoria.usuario5sId = objParamDTO.usuarioAuditorId;
                            objAuditoria.auditoriaCompleta = objParamDTO.auditoriaCompleta;
                            objAuditoria.fechaCreacion = DateTime.Now;
                            objAuditoria.usuarioCreacionId = (int)vSesiones.sesionUsuarioDTO.id;
                            objAuditoria.registroActivo = true;
                            ctx.tbl5s_Auditorias.Add(objAuditoria);
                            ctx.SaveChanges();

                            // SE OBTIENE ID DEL REGISTRO PRINCIPAL
                            int idAuditoria = ctx.tbl5s_Auditorias.Where(w => w.registroActivo).OrderByDescending(o => o.id).Select(s => s.id).FirstOrDefault();
                            if (idAuditoria <= 0)
                                throw new Exception("Ocurrió un error durante el registro.");

                            var auditor = ctx.tbl5s_Usuario.FirstOrDefault(x => x.id == objAuditoria.usuario5sId);

                            resultado.Add(ITEMS, new { ccs = objAuditoria.cc, auditor = auditor != null ? PersonalUtilities.NombreCompletoMayusculas(auditor.usuario.nombre, auditor.usuario.apellidoPaterno, auditor.usuario.apellidoMaterno) : "", checkListId = objAuditoria.checkListId, idAudi = objAuditoria.id });

                            // SE REGISTRA EL DETALLE DE LA AUDITORIA
                            int contador = 0;
                            tbl5s_AuditoriasDet objAuditoriaDet = new tbl5s_AuditoriasDet();
                            foreach (var item in objParamDTO.lstAuditoriaDet)
                            {
                                objAuditoriaDet = new tbl5s_AuditoriasDet();
                                objAuditoriaDet.auditoriaId = idAuditoria;
                                objAuditoriaDet.inspeccionId = item.inspeccionId;
                                objAuditoriaDet.descripcion = item.descripcion.Trim();
                                objAuditoriaDet.respuesta = item.respuesta;
                                objAuditoriaDet.accion = item.accion.Trim();
                                objAuditoriaDet.usuario5sId = item.usuario5sId;
                                objAuditoriaDet.fecha = item.fecha;
                                objAuditoriaDet.fechaCreacion = DateTime.Now;
                                objAuditoriaDet.usuarioCreacionId = (int)vSesiones.sesionUsuarioDTO.id;
                                objAuditoriaDet.registroActivo = true;
                                ctx.tbl5s_AuditoriasDet.Add(objAuditoriaDet);
                                ctx.SaveChanges();

                                // SE OBTIENE EL ID DETALLE DE LA AUDITORIA
                                int auditoriaDetId = ctx.tbl5s_AuditoriasDet.Where(w => w.registroActivo).OrderByDescending(o => o.id).Select(s => s.id).FirstOrDefault();

                                int indiceDeteccion = lstIndice_Detecciones.Where(w => w == contador).FirstOrDefault();
                                if (indiceDeteccion == contador && lstDetecciones != null)
                                {
                                    #region SE REGISTRA ARCHIVO DETECCIÓN
                                    var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();
#if DEBUG
                                    var CarpetaNueva = Path.Combine(RutaLocal, idAuditoria.ToString());
#else
                                    var CarpetaNueva = Path.Combine(RutaBase, idAuditoria.ToString());
#endif
                                    VerificarExisteCarpeta(CarpetaNueva, true);
                                    string nombreArchivo = SetNombreArchivo("Deteccion_" + contador, lstDetecciones[contador].FileName);
                                    string rutaArchivo = Path.Combine(CarpetaNueva, nombreArchivo);
                                    listaRutaArchivos.Add(Tuple.Create(lstDetecciones[contador], rutaArchivo));

                                    // SE REGISTRA LA INFORMACIÓN DEL ARCHIVO DETECCIÓN
                                    tbl5s_AuditoriasArchivos objArchivo = new tbl5s_AuditoriasArchivos();
                                    objArchivo.auditoriaDetId = auditoriaDetId;
                                    objArchivo.nombreArchivo = nombreArchivo;
                                    objArchivo.rutaArchivo = rutaArchivo;
                                    objArchivo.tipoArchivo = (int)TipoArchivoEnum.DETECCION;
                                    objArchivo.aprobado = (int)EstatusArchivoSeguimientoEnum.NA;
                                    objArchivo.fechaCreacion = DateTime.Now;
                                    objArchivo.usuarioCreacionId = (int)vSesiones.sesionUsuarioDTO.id;
                                    objArchivo.registroActivo = true;
                                    ctx.tbl5s_AuditoriasArchivos.Add(objArchivo);
                                    ctx.SaveChanges();

                                    foreach (var objArchivoAdd in listaRutaArchivos)
                                    {
                                        if (GlobalUtils.SaveHTTPPostedFile(objArchivoAdd.Item1, objArchivoAdd.Item2) == false)
                                        {
                                            transaction.Rollback();
                                            resultado = new Dictionary<string, object>();
                                            resultado.Add(SUCCESS, false);
                                            resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                                            return resultado;
                                        }
                                    }
                                    #endregion
                                }

                                int indiceMedida = lstIndice_Medidas.Where(w => w == contador).FirstOrDefault();
                                if (indiceMedida == contador && lstMedidas != null)
                                {
                                    #region SE REGISTRA ARCHIVO MEDIDA
                                    var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();
#if DEBUG
                                    var CarpetaNueva = Path.Combine(RutaLocal, idAuditoria.ToString());
#else
                                    var CarpetaNueva = Path.Combine(RutaBase, idAuditoria.ToString());
#endif
                                    VerificarExisteCarpeta(CarpetaNueva, true);
                                    string nombreArchivo = SetNombreArchivo("Medida_" + contador, lstMedidas[contador].FileName);
                                    string rutaArchivo = Path.Combine(CarpetaNueva, nombreArchivo);
                                    listaRutaArchivos.Add(Tuple.Create(lstMedidas[contador], rutaArchivo));

                                    // SE REGISTRA LA INFORMACIÓN DEL ARCHIVO MEDIDA
                                    tbl5s_AuditoriasArchivos objArchivo = new tbl5s_AuditoriasArchivos();
                                    objArchivo.auditoriaDetId = auditoriaDetId;
                                    objArchivo.nombreArchivo = nombreArchivo;
                                    objArchivo.rutaArchivo = rutaArchivo;
                                    objArchivo.tipoArchivo = (int)TipoArchivoEnum.MEDIDA;
                                    objArchivo.aprobado = (int)EstatusArchivoSeguimientoEnum.NA;
                                    objArchivo.fechaCreacion = DateTime.Now;
                                    objArchivo.usuarioCreacionId = (int)vSesiones.sesionUsuarioDTO.id;
                                    objArchivo.registroActivo = true;
                                    ctx.tbl5s_AuditoriasArchivos.Add(objArchivo);
                                    ctx.SaveChanges();

                                    foreach (var objArchivoAdd in listaRutaArchivos)
                                    {
                                        if (GlobalUtils.SaveHTTPPostedFile(objArchivoAdd.Item1, objArchivoAdd.Item2) == false)
                                        {
                                            transaction.Rollback();
                                            resultado = new Dictionary<string, object>();
                                            resultado.Add(SUCCESS, false);
                                            resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                                            return resultado;
                                        }
                                    }
                                    #endregion
                                }
                                contador++;
                            }
                            #endregion
                        }
                        else if (objParamDTO.id > 0)
                        {
                            #region SE ACTUALIZA LA AUDITORIA
                            // SE ACTUALIZA EL REGISTRO PRINCIPAL
                            tbl5s_Auditorias objAuditoria = ctx.tbl5s_Auditorias.Where(w => w.id == objParamDTO.id && w.registroActivo).FirstOrDefault();
                            if (objAuditoria == null)
                                throw new Exception("Ocurrió un error al actualizar la información de la auditoría.");

                            objAuditoria.cc = !string.IsNullOrEmpty(objParamDTO.cc) ? objParamDTO.cc.Trim() : null;
                            objAuditoria.fecha = objParamDTO.fecha;
                            objAuditoria.usuario5sId = objParamDTO.usuarioAuditorId;
                            objAuditoria.auditoriaCompleta = objParamDTO.auditoriaCompleta;
                            objAuditoria.fechaModificacion = DateTime.Now;
                            objAuditoria.usuarioModificacionId = (int)vSesiones.sesionUsuarioDTO.id;
                            ctx.SaveChanges();

                            var auditor = ctx.tbl5s_Usuario.FirstOrDefault(x => x.id == objAuditoria.usuario5sId);
                            resultado.Add(ITEMS, new { ccs = objAuditoria.cc, auditor = auditor != null ? PersonalUtilities.NombreCompletoMayusculas(auditor.usuario.nombre, auditor.usuario.apellidoPaterno, auditor.usuario.apellidoMaterno) : "", checkListId = objAuditoria.checkListId, idAudi = objAuditoria.id });

                            // SE ACTUALIZA EL DETALLE DEL REGISTRO PRINCIPAL
                            int rowDetalle = 0;
                            int rowDeteccion = 0;
                            int rowMedida = 0;
                            foreach (var item in objParamDTO.lstAuditoriaDet)
                            {
                                // REGISTRO DETALLE
                                tbl5s_AuditoriasDet objAuditoriaDet = ctx.tbl5s_AuditoriasDet.Where(w => w.id == item.auditoriaDetId && w.registroActivo).FirstOrDefault();
                                if (objAuditoriaDet == null)
                                    throw new Exception("Ocurrió un error al actualizar la información de la auditoría.");

                                objAuditoriaDet.descripcion = !string.IsNullOrEmpty(item.descripcion) ? item.descripcion.Trim() : null;
                                objAuditoriaDet.respuesta = item.respuesta;
                                objAuditoriaDet.accion = !string.IsNullOrEmpty(item.accion) ? item.accion.Trim() : null;
                                objAuditoriaDet.usuario5sId = item.usuario5sId > 0 ? item.usuario5sId : 0;
                                objAuditoriaDet.fecha = item.fecha;
                                objAuditoriaDet.fechaModificacion = DateTime.Now;
                                objAuditoriaDet.usuarioModificacionId = (int)vSesiones.sesionUsuarioDTO.id;
                                ctx.SaveChanges();

                                // ARCHIVO DETECCIÓN
                                if (item.idArchivoDeteccion <= 0)
                                {
                                    int detalleConImagen = lstIndice_Detecciones.Where(w => w == rowDetalle).Count();
                                    if (detalleConImagen > 0)
                                    {
                                        #region SE REGISTRA ARCHIVO DETECCIÓN
                                        var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();
#if DEBUG
                                        var CarpetaNueva = Path.Combine(RutaLocal, objParamDTO.id.ToString());
#else
                                        var CarpetaNueva = Path.Combine(RutaBase, objParamDTO.id.ToString());
#endif
                                        VerificarExisteCarpeta(CarpetaNueva, true);
                                        string nombreArchivo = SetNombreArchivo("Deteccion_", lstDetecciones[rowDeteccion].FileName);
                                        string rutaArchivo = Path.Combine(CarpetaNueva, nombreArchivo);
                                        listaRutaArchivos.Add(Tuple.Create(lstDetecciones[rowDeteccion], rutaArchivo));

                                        // SE REGISTRA LA INFORMACIÓN DEL ARCHIVO DETECCIÓN
                                        tbl5s_AuditoriasArchivos objArchivo = new tbl5s_AuditoriasArchivos();
                                        objArchivo.auditoriaDetId = item.auditoriaDetId;
                                        objArchivo.nombreArchivo = nombreArchivo;
                                        objArchivo.rutaArchivo = rutaArchivo;
                                        objArchivo.tipoArchivo = (int)TipoArchivoEnum.DETECCION;
                                        objArchivo.aprobado = (int)EstatusArchivoSeguimientoEnum.NA;
                                        objArchivo.fechaCreacion = DateTime.Now;
                                        objArchivo.usuarioCreacionId = (int)vSesiones.sesionUsuarioDTO.id;
                                        objArchivo.registroActivo = true;
                                        ctx.tbl5s_AuditoriasArchivos.Add(objArchivo);
                                        ctx.SaveChanges();

                                        foreach (var objArchivoAdd in listaRutaArchivos)
                                        {
                                            if (GlobalUtils.SaveHTTPPostedFile(objArchivoAdd.Item1, objArchivoAdd.Item2) == false)
                                            {
                                                transaction.Rollback();
                                                resultado = new Dictionary<string, object>();
                                                resultado.Add(SUCCESS, false);
                                                resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                                                return resultado;
                                            }
                                        }
                                        rowDeteccion = rowDetalle;
                                        #endregion
                                    }
                                }

                                // ARCHIVO MEDIDA
                                if (item.idArchivoMedida <= 0)
                                {
                                    int detalleConImagen = lstIndice_Medidas.Where(w => w == rowDetalle).Count();
                                    if (detalleConImagen > 0)
                                    {
                                        #region SE REGISTRA ARCHIVO MEDIDA
                                        var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();
#if DEBUG
                                        var CarpetaNueva = Path.Combine(RutaLocal, objParamDTO.id.ToString());
#else
                                        var CarpetaNueva = Path.Combine(RutaBase, objParamDTO.id.ToString());
#endif
                                        VerificarExisteCarpeta(CarpetaNueva, true);
                                        string nombreArchivo = SetNombreArchivo("Medida_", lstMedidas[rowMedida].FileName);
                                        string rutaArchivo = Path.Combine(CarpetaNueva, nombreArchivo);
                                        listaRutaArchivos.Add(Tuple.Create(lstMedidas[rowMedida], rutaArchivo));

                                        // SE REGISTRA LA INFORMACIÓN DEL ARCHIVO MEDIDA
                                        tbl5s_AuditoriasArchivos objArchivo = new tbl5s_AuditoriasArchivos();
                                        objArchivo.auditoriaDetId = item.auditoriaDetId;
                                        objArchivo.nombreArchivo = nombreArchivo;
                                        objArchivo.rutaArchivo = rutaArchivo;
                                        objArchivo.tipoArchivo = (int)TipoArchivoEnum.MEDIDA;
                                        objArchivo.aprobado = (int)EstatusArchivoSeguimientoEnum.NA;
                                        objArchivo.fechaCreacion = DateTime.Now;
                                        objArchivo.usuarioCreacionId = (int)vSesiones.sesionUsuarioDTO.id;
                                        objArchivo.registroActivo = true;
                                        ctx.tbl5s_AuditoriasArchivos.Add(objArchivo);
                                        ctx.SaveChanges();

                                        foreach (var objArchivoAdd in listaRutaArchivos)
                                        {
                                            if (GlobalUtils.SaveHTTPPostedFile(objArchivoAdd.Item1, objArchivoAdd.Item2) == false)
                                            {
                                                transaction.Rollback();
                                                resultado = new Dictionary<string, object>();
                                                resultado.Add(SUCCESS, false);
                                                resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                                                return resultado;
                                            }
                                        }
                                        rowMedida = rowDetalle;
                                        #endregion
                                    }
                                }
                                rowDetalle++;
                            }
                            #endregion
                        }

                        if (objParamDTO.auditoriaCompleta)
                        {
                            #region SE INDICA QUE LA AUDITORIA YA SE ENCUENTRA COMPLETA
                            if (objParamDTO.id <= 0)
                            {
                                tbl5s_Auditorias objAuditoria = ctx.tbl5s_Auditorias.Where(w => w.registroActivo).OrderByDescending(o => o.id).FirstOrDefault();
                                if (objAuditoria != null)
                                    objParamDTO.id = objAuditoria.id;
                            }

                            tbl5s_Auditorias objAuditoriaCompleta = ctx.tbl5s_Auditorias.Where(w => w.id == objParamDTO.id && w.registroActivo).FirstOrDefault();
                            if (objAuditoriaCompleta == null)
                                throw new Exception("Ocurrió un error al indicar que la auditoría ya se encuentra completa.");

                            objAuditoriaCompleta.auditoriaCompleta = true;
                            objAuditoriaCompleta.usuarioModificacionId = (int)vSesiones.sesionUsuarioDTO.id;
                            objAuditoriaCompleta.fechaModificacion = DateTime.Now;
                            ctx.SaveChanges();
                            #endregion
                        }

                        transaction.Commit();
                        resultado.Add(SUCCESS, true);
                        if (objParamDTO.auditoriaCompleta)
                            resultado.Add(MESSAGE, "Se ha registrado y finalizado con éxito.");
                        else
                            resultado.Add(MESSAGE, objParamDTO.id <= 0 ? "Se ha registrado con éxito." : "Se ha actualizado con éxito.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, ex.Message);
                    }
                }
            }
            return resultado;
        }

        /// <summary>
        /// SE OBTIENE LAS INSPECCIONES RELACIONADAS AL CHECKLIST SELECCIONADO Y SU ÁREA.
        /// </summary>
        public Dictionary<string, object> GetInspeccionesRelCheckList(AuditoriaDTO objParamDTO)
        {
            using (var ctx = new MainContext())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        #region VALIDACIONES
                        if (objParamDTO.checkListId <= 0) { throw new Exception("Ocurrió un error al obtener las inspecciones."); }
                        #endregion

                        #region SE OBTIENE LISTADO DE INSPECCIONES Y EL ÁREA CORRESPONDIENTE

                        // SE OBTIENE AREA
                        string area = ctx.tbl5s_CheckList.Where(w => w.id == objParamDTO.checkListId).Select(s => s.area.nombre).FirstOrDefault();
                        if (string.IsNullOrEmpty(area))
                            throw new Exception("El CheckList seleccionado no cuenta con un área.");

                        // SE OBTIENE LAS INSPECCIONES
                        List<tbl5s_Inspeccion> lstInspecciones = ctx.tbl5s_Inspeccion.Where(w => w.checkListId == objParamDTO.checkListId && w.registroActivo).ToList();
                        if (lstInspecciones.Count() <= 0)
                            throw new Exception("El CheckList seleccionado no cuenta con inspecciones registradas.");

                        List<InspeccionesDTO> lstInspeccionesDTO = new List<InspeccionesDTO>();
                        InspeccionesDTO objInspeccionDTO = new InspeccionesDTO();
                        foreach (var item in lstInspecciones)
                        {
                            objInspeccionDTO = new InspeccionesDTO();
                            objInspeccionDTO.id = item.id;
                            objInspeccionDTO.inspeccion = !string.IsNullOrEmpty(item.inspeccion) ? item.inspeccion.Trim() : string.Empty;
                            objInspeccionDTO.cantPuntos = GetCantPuntosRelInspeccion(item.id);
                            objInspeccionDTO.descripcion = string.Empty;
                            objInspeccionDTO.accion = string.Empty;
                            lstInspeccionesDTO.Add(objInspeccionDTO);
                        }

                        // SE OBTIENE LISTADO DE LIDERES EN BASE AL CC SELECCIONADO
                        List<int> lstUsuarios5sId = ctx.tbl5s_LiderArea.Where(w => w.checkListId == objParamDTO.checkListId && w.registroActivo).Select(s => s.usuario5sId).ToList();
                        List<int> lstUsuarioId = ctx.tbl5s_Usuario.Where(w => lstUsuarios5sId.Contains(w.id) && w.registroActivo).Select(s => s.usuarioId).ToList();
                        List<ComboDTO> lstLideresDTO = new List<ComboDTO>();
                        ComboDTO objComboDTO = new ComboDTO();
                        string nombreCompleto = string.Empty;
                        foreach (var item in lstUsuarioId)
                        {
                            if (item > 0)
                            {
                                tblP_Usuario objUsuario = ctx.tblP_Usuario.Where(w => w.id == item && w.estatus).FirstOrDefault();
                                if (objUsuario != null)
                                {
                                    nombreCompleto = string.Empty;
                                    if (!string.IsNullOrEmpty(objUsuario.nombre))
                                        nombreCompleto = objUsuario.nombre.Trim();
                                    if (!string.IsNullOrEmpty(objUsuario.apellidoPaterno))
                                        nombreCompleto += string.Format(" {0}", objUsuario.apellidoPaterno.Trim());
                                    if (!string.IsNullOrEmpty(objUsuario.apellidoMaterno))
                                        nombreCompleto += string.Format(" {0}", objUsuario.apellidoMaterno.Trim());

                                    objComboDTO = new ComboDTO();
                                    objComboDTO.Value = item.ToString();
                                    objComboDTO.Text = nombreCompleto;
                                    lstLideresDTO.Add(objComboDTO);
                                }
                            }
                        }

                        resultado.Add(SUCCESS, true);
                        resultado.Add("area", area);
                        resultado.Add("lstInspecciones", lstInspeccionesDTO);
                        resultado.Add(ITEMS, lstLideresDTO);
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, ex.Message);
                    }
                }
            }
            return resultado;
        }

        /// <summary>
        /// SE OBTIENE LISTADO DE AUDITORES EN BASE AL PROYECTO SELECCIONADO
        /// </summary>
        public Dictionary<string, object> FillCboAuditores(AuditoriaDTO objParamDTO)
        {
            using (var ctx = new MainContext())
            {
                try
                {
                    #region VALIDACIONES
                    if (string.IsNullOrEmpty(objParamDTO.cc)) { throw new Exception("Es necesario seleccionar un proyecto."); }
                    #endregion

                    #region SE OBTIENE LISTADO DE USUARIOS CON PRIVILEGIO DE AUDITOR
                    List<int> lstUsuarios5sId = ctx.tbl5s_CC_Usuario.Where(w => objParamDTO.cc.Contains(w.cc) && w.registroActivo).Select(s => s.usuario5sId).ToList();
                    List<tbl5s_Usuario> lstAuditores = ctx.tbl5s_Usuario.Where(w => w.privilegioId == (int)PrivilegioEnum.Auditor && lstUsuarios5sId.Contains(w.id) && w.registroActivo).ToList();

                    List<ComboDTO> lstAuditoresDTO = new List<ComboDTO>();
                    ComboDTO objComboDTO = new ComboDTO();
                    string nombreCompleto = string.Empty;
                    foreach (var item in lstAuditores)
                    {
                        if (item.usuarioId > 0)
                        {
                            tblP_Usuario objUsuario = ctx.tblP_Usuario.Where(w => w.id == item.usuarioId).FirstOrDefault();
                            if (objUsuario != null)
                            {
                                nombreCompleto = string.Empty;
                                if (!string.IsNullOrEmpty(objUsuario.nombre))
                                    nombreCompleto = objUsuario.nombre.Trim();
                                if (!string.IsNullOrEmpty(objUsuario.apellidoPaterno))
                                    nombreCompleto += string.Format(" {0}", objUsuario.apellidoPaterno.Trim());
                                if (!string.IsNullOrEmpty(objUsuario.apellidoMaterno))
                                    nombreCompleto += string.Format(" {0}", objUsuario.apellidoMaterno.Trim());

                                objComboDTO = new ComboDTO();
                                objComboDTO.Value = item.id.ToString();
                                objComboDTO.Text = nombreCompleto;
                                lstAuditoresDTO.Add(objComboDTO);
                            }
                        }
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, lstAuditoresDTO);
                    #endregion
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }
            return resultado;
        }

        /// <summary>
        /// SE OBTIENE LA CANTIDAD DE PUNTOS QUE CONTIENE CADA INSPECCIÓN DE UNA AUDITORIA
        /// </summary>
        private int GetCantPuntosRelInspeccion(int inspeccionId)
        {
            int cantPuntos = 0;
            using (var ctx = new MainContext())
            {
                try
                {
                    #region SE OBTIENE LA CANTIDAD DE PUNTOS QUE CONTIENE LA INSPECCIÓN
                    cantPuntos = ctx.tbl5s_InspeccionDet.Where(w => w.inspeccionId == inspeccionId && w.registroActivo).Count();
                    #endregion
                }
                catch (Exception)
                {
                    return cantPuntos;
                }
            }
            return cantPuntos;
        }

        /// <summary>
        /// SE OBTIENE LA INFORMACIÓN DE LA AUDITORIA, YA QUE FUE UN GUARDADO PARCIAL.
        /// </summary>
        public Dictionary<string, object> GetDatosActualizarAuditoria(AuditoriaDTO objParamDTO)
        {
            using (var ctx = new MainContext())
            {
                try
                {
                    #region VALIDACIONES
                    string mensajeError = "Ocurrió un error al obtener la información de la auditoría.";
                    if (objParamDTO.id <= 0) { throw new Exception(mensajeError); }
                    #endregion

                    #region SE OBTIENE LA INFORMACIÓN DE LA AUDITORIA SELECCIONADA
                    // REGISTRO PRINCIPAL
                    tbl5s_Auditorias objAuditoria = ctx.tbl5s_Auditorias.Where(w => w.id == objParamDTO.id && w.registroActivo).FirstOrDefault();
                    if (objAuditoria == null)
                        throw new Exception(mensajeError);

                    // REGISTRO DETALLE
                    List<tbl5s_AuditoriasDet> lstAuditoriasDet = ctx.tbl5s_AuditoriasDet.Where(w => w.auditoriaId == objParamDTO.id && w.registroActivo).ToList();
                    if (lstAuditoriasDet.Count() <= 0)
                        throw new Exception(mensajeError);

                    List<int> lstAuditoriasDet_ID = lstAuditoriasDet.Select(s => s.id).ToList();

                    // ARCHIVO DETECCIÓN Y ARCHIVO MEDIDA
                    List<tbl5s_AuditoriasArchivos> lstArchivos = ctx.tbl5s_AuditoriasArchivos.Where(w => lstAuditoriasDet_ID.Contains(w.auditoriaDetId) && w.registroActivo).ToList();

                    // SE CONSTRUYE DTO PARA MOSTRARLO EN MODAL
                    AuditoriaDTO objAuditoriaDTO = new AuditoriaDTO();
                    objAuditoriaDTO.id = objAuditoria.id > 0 ? objAuditoria.id : 0;
                    objAuditoriaDTO.checkListId = objAuditoria.checkListId > 0 ? objAuditoria.checkListId : 0;
                    objAuditoriaDTO.cc = !string.IsNullOrEmpty(objAuditoria.cc) ? objAuditoria.cc : string.Empty;
                    objAuditoriaDTO.usuarioAuditorId = objAuditoria.usuario5sId > 0 ? objAuditoria.usuario5sId : 0;
                    if (objAuditoria.fecha != null)
                        objAuditoriaDTO.fechaStr = objAuditoria.fecha.ToShortDateString();
                    else
                        objAuditoriaDTO.fechaStr = string.Empty;

                    tbl5s_CheckList objCheckList = ctx.tbl5s_CheckList.Where(w => w.id == objAuditoria.checkListId && w.registroActivo).FirstOrDefault();
                    if (objCheckList == null)
                        throw new Exception("Ocurrió un error al obtener al área.");

                    objAuditoriaDTO.area = !string.IsNullOrEmpty(objCheckList.area.nombre) ? objCheckList.area.nombre : string.Empty;

                    objAuditoriaDTO.lstAuditoriaDet = new List<AuditoriaDetDTO>();
                    AuditoriaDetDTO objAuditoriaDetDTO = new AuditoriaDetDTO();
                    foreach (var item in lstAuditoriasDet)
                    {
                        #region SE OBTIENE CADA DETALLE DE LA AUDITORIA
                        objAuditoriaDetDTO = new AuditoriaDetDTO();
                        objAuditoriaDetDTO.id = item.id > 0 ? item.id : 0;
                        objAuditoriaDetDTO.auditoriaDetId = item.id > 0 ? item.id : 0;
                        objAuditoriaDetDTO.auditoriaId = item.auditoriaId > 0 ? item.auditoriaId : 0;
                        objAuditoriaDetDTO.inspeccionId = item.inspeccionId > 0 ? item.inspeccionId : 0;

                        tbl5s_Inspeccion objInspeccion = ctx.tbl5s_Inspeccion.Where(w => w.id == item.inspeccionId && w.registroActivo).FirstOrDefault();
                        if (objInspeccion == null)
                            throw new Exception("Ocurrió un error al obtener el nombre de la inspección.");

                        objAuditoriaDetDTO.inspeccion = !string.IsNullOrEmpty(objInspeccion.inspeccion) ? objInspeccion.inspeccion.Trim() : null;

                        objAuditoriaDetDTO.respuesta = item.respuesta == (int)RespuestaEnum.OK ? (int)RespuestaEnum.OK : item.respuesta == (int)RespuestaEnum.NO_OK ? (int)RespuestaEnum.NO_OK : 0;
                        objAuditoriaDetDTO.accion = !string.IsNullOrEmpty(item.accion) ? item.accion.Trim() : string.Empty;
                        objAuditoriaDetDTO.usuario5sId = item.usuario5sId > 0 ? item.usuario5sId : 0;
                        if (item.fecha != null)
                            objAuditoriaDetDTO.fechaStr = item.fecha.Value.ToShortDateString();
                        else
                            objAuditoriaDetDTO.fechaStr = string.Empty;

                        #region SE OBTIENE LA INFORMACIÓN DEL ARCHIVO DETECCIÓN
                        tbl5s_AuditoriasArchivos objArchivo = lstArchivos.Where(w => w.auditoriaDetId == item.id && w.tipoArchivo == (int)TipoArchivoEnum.DETECCION).FirstOrDefault();
                        if (objArchivo != null)
                            objAuditoriaDetDTO.idArchivoDeteccion = objArchivo.id > 0 ? objArchivo.id : 0;
                        else
                            objAuditoriaDetDTO.idArchivoDeteccion = 0;

                        objAuditoriaDetDTO.descripcion = !string.IsNullOrEmpty(item.descripcion) ? item.descripcion : string.Empty;

                        #endregion

                        #region SE OBTIENE LA INFORMACIÓN DEL ARCHIVO MEDIDA
                        objArchivo = lstArchivos.Where(w => w.auditoriaDetId == item.id && w.tipoArchivo == (int)TipoArchivoEnum.MEDIDA).FirstOrDefault();
                        if (objArchivo != null)
                            objAuditoriaDetDTO.idArchivoMedida = objArchivo.id > 0 ? objArchivo.id : 0;
                        else
                            objAuditoriaDetDTO.idArchivoMedida = 0;
                        #endregion

                        objAuditoriaDTO.lstAuditoriaDet.Add(objAuditoriaDetDTO);
                        #endregion
                    }

                    #region FILL COMBO LIDERES
                    List<int> lstUsuarios5sId = ctx.tbl5s_LiderArea.Where(w => w.checkListId == objAuditoriaDTO.checkListId && w.registroActivo).Select(s => s.usuario5sId).ToList();
                    List<int> lstUsuarioId = ctx.tbl5s_Usuario.Where(w => lstUsuarios5sId.Contains(w.id) && w.registroActivo).Select(s => s.usuarioId).ToList();
                    List<ComboDTO> lstLideresDTO = new List<ComboDTO>();
                    ComboDTO objComboDTO = new ComboDTO();
                    string nombreCompleto = string.Empty;
                    foreach (var item in lstUsuarioId)
                    {
                        if (item > 0)
                        {
                            tblP_Usuario objUsuario = ctx.tblP_Usuario.Where(w => w.id == item && w.estatus).FirstOrDefault();
                            if (objUsuario != null)
                            {
                                nombreCompleto = string.Empty;
                                if (!string.IsNullOrEmpty(objUsuario.nombre))
                                    nombreCompleto = objUsuario.nombre.Trim();
                                if (!string.IsNullOrEmpty(objUsuario.apellidoPaterno))
                                    nombreCompleto += string.Format(" {0}", objUsuario.apellidoPaterno.Trim());
                                if (!string.IsNullOrEmpty(objUsuario.apellidoMaterno))
                                    nombreCompleto += string.Format(" {0}", objUsuario.apellidoMaterno.Trim());

                                objComboDTO = new ComboDTO();
                                objComboDTO.Value = item.ToString();
                                objComboDTO.Text = nombreCompleto;
                                lstLideresDTO.Add(objComboDTO);
                            }
                        }
                    }
                    #endregion

                    resultado.Add(SUCCESS, true);
                    resultado.Add("objAuditoria", objAuditoriaDTO);
                    resultado.Add(ITEMS, lstLideresDTO);
                    #endregion
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> NotificarAuditoria(int idAuditoria)
        {
            using (var ctx = new MainContext())
            {
                try
                {
                    var reportes = new List<MultiplesDTO>();
                    if (HttpContext.Current.Session["rptAuditoriaCincoS"] != null)
                    {
                        reportes = HttpContext.Current.Session["rptAuditoriaCincoS"] as List<MultiplesDTO>;
                    }

                    var auditoria = ctx.tbl5s_Auditorias.FirstOrDefault(x => x.id == idAuditoria);
                    if (auditoria != null)
                    {
                        var auditor = ctx.tbl5s_Usuario.FirstOrDefault(x => x.id == auditoria.usuario5sId);
                        var lideres = auditoria.checkList.lideres.Where(x => x.registroActivo).ToList();
                        var cc = ctx.tblC_Nom_CatalogoCC.FirstOrDefault(x => x.cc == auditoria.cc);

                        var fechaActual = DateTime.Now;
                        string diaTardeNoche = fechaActual.Hour >= 5 && fechaActual.Hour < 12 ? "Buen día" : fechaActual.Hour >= 12 && fechaActual.Hour < 20 ? "Buenas tardes" : "Buenas noches";

                        var correo = new Infrastructure.DTO.CorreoDTO();
                        correo.asunto = "Seguimiento Auditoria " + auditoria.cc + " " + auditoria.checkList.nombre;
                        correo.cuerpo += "<p>" + diaTardeNoche + ".</p>";
                        correo.cuerpo += "<p>Se informa que ha sido creada una auditoria del CC: " + (cc != null ? cc.ccDescripcion : auditoria.cc) + ".</p>";
                        correo.cuerpo += "<p>Se envia archivo adjunto con la informacion de la auditoria.</p>";

                        if (reportes != null)
                        {
                            foreach (var item in reportes)
                            {
                                var reporte = new MemoryStream(item.reporte);
                                correo.archivos.Add(new Attachment(reporte, item.nombre + ".pdf"));
                            }
                        }

                        correo.correos = lideres.Select(x => x.usuario.usuario.correo).ToList();
                        correo.correosCC = new List<string> { auditor.usuario.correo };
#if DEBUG
                        correo.correos = new List<string> { "martin.zayas@construplan.com.mx" };
                        correo.correosCC = new List<string>();
#endif
                        HttpContext.Current.Session.Remove("rptAuditoriaCincoS");

                        correo.Enviar();

                        resultado.Add(SUCCESS, true);
                    }
                    else
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se encontro informacion de la auditoria");
                    }
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }
        #endregion

        #region REPORTE
        /// <summary>
        /// SE OBTIENE LAS INSPECCIONES RELACIONADAS AL CHECKLIST SELECCIONADO Y SU ÁREA.
        /// </summary>
        public Dictionary<string, object> GetInspeccionesRelCheckListReporte(AuditoriaDTO objParamDTO)
        {
            using (var ctx = new MainContext())
            {
                try
                {
                    #region VALIDACIONES
                    if (objParamDTO.checkListId <= 0) { throw new Exception("Ocurrió un error al obtener las inspecciones."); }
                    #endregion

                    #region SE OBTIENE LISTADO DE INSPECCIONES Y EL ÁREA CORRESPONDIENTE

                    // SE OBTIENE AREA
                    string area = ctx.tbl5s_CheckList.Where(w => w.id == objParamDTO.checkListId && w.registroActivo).Select(s => s.area.nombre).FirstOrDefault();
                    if (string.IsNullOrEmpty(area))
                        throw new Exception("El CheckList seleccionado no cuenta con un área.");

                    // SE OBTIENE LAS INSPECCIONES
                    List<tbl5s_Inspeccion> lstInspecciones = ctx.tbl5s_Inspeccion.Where(w => w.checkListId == objParamDTO.checkListId && w.registroActivo && w.registroActivo).ToList();
                    if (lstInspecciones.Count() <= 0)
                        throw new Exception("El CheckList seleccionado no cuenta con inspecciones registradas.");

                    List<AuditoriaDetDTO> lstInspeccionesReporte = new List<AuditoriaDetDTO>();
                    InspeccionesDTO objInspeccionDTO = new InspeccionesDTO();

                    var lstDetAudit = ctx.tbl5s_AuditoriasDet.Where(e => e.auditoriaId == objParamDTO.id && e.registroActivo).ToList();
                    var lstIdDetAudit = lstDetAudit.Select(e => e.id).ToList();
                    var lstArchivos = ctx.tbl5s_AuditoriasArchivos.Where(e => e.registroActivo && lstIdDetAudit.Contains(e.auditoriaDetId)).ToList();

                    foreach (var item in lstDetAudit)
                    {
                        var objArchDeteccion = lstArchivos.FirstOrDefault(e => e.auditoriaDetId == item.id && e.tipoArchivo == (int)TipoArchivoEnum.DETECCION);
                        var objArchMedida = lstArchivos.FirstOrDefault(e => e.auditoriaDetId == item.id && e.tipoArchivo == (int)TipoArchivoEnum.MEDIDA);
                        //var objUsuario5s = ctx.tbl5s_Usuario.FirstOrDefault(e => e.id == item.usuario5sId);
                        var objUsuario = ctx.tblP_Usuario.FirstOrDefault(e => e.id == item.usuario5sId);


                        lstInspeccionesReporte.Add(new AuditoriaDetDTO()
                        {
                            id = item.id,

                            descInspeccion = GetDescripcionInspeccion(item.inspeccionId, lstInspecciones),
                            accion = !string.IsNullOrEmpty(item.accion) ? item.accion.Trim() : null,
                            descripcion = item.descripcion,
                            respuesta = item.respuesta,
                            descDeteccion = !string.IsNullOrEmpty(item.descripcion) ? item.descripcion.Trim() : null,
                            fecha = item.fecha,
                            nombreUsuario5s = objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno + " " + objUsuario.nombre,
                            rutaDeteccion = objArchDeteccion != null ? objArchDeteccion.rutaArchivo : "",
                            rutaMedida = objArchMedida != null ? objArchMedida.rutaArchivo : "",
                        });
                    }

                    decimal porcAudit = 0;
                    string labelEstatusAuditoria = "";
                    string labelAccionesAuditoria = "";

                    int totalPuntos = 0;
                    int totalPuntosOk = 0;
                    var lstInspectDetAudit = lstDetAudit.Select(e => e.inspeccionId).ToList();

                    foreach (var item in lstInspecciones)
                    {
                        int cantPuntos = GetCantPuntosRelInspeccion(item.id);
                        totalPuntos += cantPuntos;

                        if (lstInspectDetAudit.Contains(item.id))
                        {
                            var objAutidDet = lstDetAudit.FirstOrDefault(e => e.inspeccionId == item.id);

                            if (objAutidDet.respuesta == 1)
                            {
                                totalPuntosOk += cantPuntos;
                            }
                        }

                    }

                    porcAudit = (totalPuntosOk * 100) / (totalPuntos == 0 ? 1 : totalPuntos);

                    //#region Estatus Auditoría
                    if (porcAudit >= 85)
                    {
                        labelEstatusAuditoria = "AUDITORÍA APROBADA";
                    }
                    else
                    {
                        labelEstatusAuditoria = "AUDITORÍA RECHAZADA";
                    }
                    //#endregion

                    //#region Estatus 5's
                    if (porcAudit >= 100)
                    {
                        labelAccionesAuditoria = "IMPLEMENTACIÓN DE 5'S NIVEL DE EXCELENCIA";
                    }
                    else if (porcAudit >= 85 && porcAudit < 100)
                    {
                        labelAccionesAuditoria = "5'S REQUIERE REFORZAMIENTO CONTINUO";
                    }
                    else if (porcAudit >= 0 && porcAudit < 85)
                    {
                        labelAccionesAuditoria = "5'S NIVEL CRÍTICO REQUIERE ACCIONES INMEDIATAS";
                    }
                    //#endregion

                    resultado.Add(SUCCESS, true);
                    resultado.Add("area", area);
                    resultado.Add("porcHead", porcAudit);
                    resultado.Add("labelEstatusAuditoria", labelEstatusAuditoria);
                    resultado.Add("labelAccionesAuditoria", labelAccionesAuditoria);
                    resultado.Add("lstInspeccionesReporte", lstInspeccionesReporte);
                    //resultado.Add(ITEMS, lstLideresDTO);
                    #endregion
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public void NOtificarAuditoriaReporte()
        {

        }
        
        #endregion

        #region SEGUIMIENTOS
        public Dictionary<string, object> GetSeguimientos(AuditoriaDTO objParamDTO)
        {
            using (var ctx = new MainContext())
            {
                try
                {
                    #region SE OBTIENE SEGUIMIENTO (DETECCIONES, MEDIDAS IMPLEMENTADAS Y ACCIONES REALIZADAS) | SOLAMENTE LOS QUE ESTAN EN "NO OKAY"
                    List<tbl5s_AuditoriasDet> lstAuditoriasDet = ctx.tbl5s_AuditoriasDet.Where(w => w.registroActivo && w.auditoria.cc == objParamDTO.cc && w.auditoria.auditoriaCompleta && w.auditoria.fecha >= objParamDTO.fechaInicio.Value && w.auditoria.fecha <= objParamDTO.fechaFinal.Value).ToList();
                    List<int> lstInspeccionesID = lstAuditoriasDet.Select(s => s.inspeccionId).ToList();
                    List<tbl5s_Inspeccion> lstInspecciones = ctx.tbl5s_Inspeccion.Where(w => lstInspeccionesID.Contains(w.id) && w.registroActivo).ToList();
                    List<tbl5s_AuditoriasArchivos> lstArchivos = ctx.tbl5s_AuditoriasArchivos.Where(w => w.registroActivo).ToList();

                    List<AuditoriaDetDTO> lstAuditoriasDetDTO = new List<AuditoriaDetDTO>();
                    AuditoriaDetDTO objAuditoriaDetDTO = new AuditoriaDetDTO();
                    foreach (var item in lstAuditoriasDet)
                    {
                        objAuditoriaDetDTO = new AuditoriaDetDTO();
                        objAuditoriaDetDTO.id = item.id;
                        objAuditoriaDetDTO.descInspeccion = GetDescripcionInspeccion(item.inspeccionId, lstInspecciones) + " - [" + item.auditoria.checkList.nombre + "]";
                        objAuditoriaDetDTO.accion = !string.IsNullOrEmpty(item.accion) ? item.accion.Trim() : null;
                        objAuditoriaDetDTO.idArchivoDeteccion = GetArchivoID(item.id, lstArchivos, (int)TipoArchivoEnum.DETECCION);
                        objAuditoriaDetDTO.descDeteccion = !string.IsNullOrEmpty(item.descripcion) ? item.descripcion.Trim() : null;
                        objAuditoriaDetDTO.estatusAuditoriaDet = GetEstatusAuditoriaDet(item.id, lstArchivos);
                        objAuditoriaDetDTO.idArchivoSeguimiento = GetArchivoID(item.id, lstArchivos, (int)TipoArchivoEnum.SEGUIMIENTO);
                        objAuditoriaDetDTO.comentarioRechazo = GetComentarioRechazo(objAuditoriaDetDTO.idArchivoSeguimiento, lstArchivos);
                        objAuditoriaDetDTO.aprobado = GetEstatusArchivoSeguimiento(objAuditoriaDetDTO.idArchivoSeguimiento, lstArchivos);
                        objAuditoriaDetDTO.comentarioLider = item.comentarioLider;

                        if ((item.respuesta == (int)RespuestaEnum.NO_OK) ||
                            (item.respuesta == (int)RespuestaEnum.OK && objAuditoriaDetDTO.aprobado == (int)EstatusArchivoSeguimientoEnum.AUTORIZADO) ||
                            (item.respuesta == (int)RespuestaEnum.OK && objAuditoriaDetDTO.aprobado == (int)EstatusArchivoSeguimientoEnum.RECHAZADO))
                        {
                            lstAuditoriasDetDTO.Add(objAuditoriaDetDTO);
                        }
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add("lstAuditoriasDetDTO", lstAuditoriasDetDTO);
                    #endregion
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> RegistrarArchivoSeguimiento(AuditoriaDTO objParamDTO, HttpPostedFileBase objArchivoSeguimiento)
        {
            using (var ctx = new MainContext())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        #region VALIDACIONES
                        if (objParamDTO.id <= 0) { throw new Exception("Ocurrió un error al registrar el archivo seguimiento."); }
                        if (objArchivoSeguimiento == null) { throw new Exception("Ocurrió un error al registrar el archivo seguimiento."); }
                        #endregion

                        #region SE REGISTRA ARCHIVO SEGUIMIENTO
                        var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();
#if DEBUG
                        var CarpetaNueva = Path.Combine(RutaLocal, objParamDTO.id.ToString());
#else
                        var CarpetaNueva = Path.Combine(RutaBase, objParamDTO.id.ToString());
#endif
                        VerificarExisteCarpeta(CarpetaNueva, true);
                        string nombreArchivo = SetNombreArchivo("Seguimiento_", objArchivoSeguimiento.FileName);
                        string rutaArchivo = Path.Combine(CarpetaNueva, nombreArchivo);
                        listaRutaArchivos.Add(Tuple.Create(objArchivoSeguimiento, rutaArchivo));

                        // SE REGISTRA LA INFORMACIÓN DEL ARCHIVO SEGUIMIENTO
                        tbl5s_AuditoriasArchivos objArchivo = new tbl5s_AuditoriasArchivos();
                        objArchivo.auditoriaDetId = objParamDTO.id;
                        objArchivo.nombreArchivo = nombreArchivo;
                        objArchivo.rutaArchivo = rutaArchivo;
                        objArchivo.tipoArchivo = (int)TipoArchivoEnum.SEGUIMIENTO;
                        objArchivo.aprobado = (int)EstatusArchivoSeguimientoEnum.PENDIENTE;
                        objArchivo.fechaCreacion = DateTime.Now;
                        objArchivo.usuarioCreacionId = (int)vSesiones.sesionUsuarioDTO.id;
                        objArchivo.registroActivo = true;
                        ctx.tbl5s_AuditoriasArchivos.Add(objArchivo);
                        ctx.SaveChanges();

                        foreach (var objArchivoAdd in listaRutaArchivos)
                        {
                            if (GlobalUtils.SaveHTTPPostedFile(objArchivoAdd.Item1, objArchivoAdd.Item2) == false)
                            {
                                transaction.Rollback();
                                resultado = new Dictionary<string, object>();
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                                return resultado;
                            }
                        }

                        transaction.Commit();
                        resultado.Add(SUCCESS, true);
                        resultado.Add(MESSAGE, "Se ha registrado con éxito el archivo.");
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, ex.Message);
                    }
                }
            }
            return resultado;
        }

        public Dictionary<string, object> AutorizarRechazarArchivoSeguimiento(AuditoriaDetDTO objParamDTO)
        {
            using (var ctx = new MainContext())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        #region VALIDACIONES
                        string mensajeError = "Ocurrió un error al aprobar/rechazar el seguimiento.";
                        if (objParamDTO.idArchivoSeguimiento <= 0) { throw new Exception(mensajeError); }
                        if (objParamDTO.aprobado <= 0) { throw new Exception(mensajeError); }
                        if (objParamDTO.aprobado == (int)EstatusArchivoSeguimientoEnum.RECHAZADO)
                        {
                            if (string.IsNullOrEmpty(objParamDTO.comentarioRechazo))
                                throw new Exception("Es necesario ingresar un comentario del motivo de rechazo.");
                        }
                        #endregion

                        #region SE AUTORIZA / RECHAZA EL ARCHIVO SEGUIMIENTO
                        tbl5s_AuditoriasArchivos objArchivo = ctx.tbl5s_AuditoriasArchivos.Where(w => w.id == objParamDTO.idArchivoSeguimiento && w.registroActivo).FirstOrDefault();
                        if (objParamDTO.aprobado == (int)EstatusArchivoSeguimientoEnum.RECHAZADO)
                            objArchivo.descripcion = objParamDTO.comentarioRechazo.Trim();

                        objArchivo.aprobado = objParamDTO.aprobado;
                        objArchivo.usuarioModificacionId = (int)vSesiones.sesionUsuarioDTO.id;
                        objArchivo.fechaModificacion = DateTime.Now;
                        ctx.SaveChanges();

                        #region SE ACTUALIZA LA RESPUESTA DE LA AUDITORIA DETALLE
                        if (objParamDTO.aprobado == (int)EstatusArchivoSeguimientoEnum.AUTORIZADO)
                        {
                            tbl5s_AuditoriasDet objAuditoriaDet = ctx.tbl5s_AuditoriasDet.Where(w => w.id == objArchivo.auditoriaDetId && w.registroActivo).FirstOrDefault();
                            if (objAuditoriaDet == null)
                                throw new Exception("Ocurrió un error actualizar de NO OK a OK el detalle de la auditoria.");

                            objAuditoriaDet.respuesta = (int)RespuestaEnum.OK;
                            objAuditoriaDet.usuarioModificacionId = (int)vSesiones.sesionUsuarioDTO.id;
                            objAuditoriaDet.fechaModificacion = DateTime.Now;
                            ctx.SaveChanges();
                        }
                        #endregion

                        transaction.Commit();
                        resultado.Add(SUCCESS, true);
                        resultado.Add(MESSAGE, objParamDTO.aprobado == (int)EstatusArchivoSeguimientoEnum.AUTORIZADO ? "Se ha autorizado con éxito" : "Se ha rechazado con éxito.");
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, ex.Message);
                    }
                }
            }
            return resultado;
        }

        public Tuple<Stream, string> DescargarArchivo(AuditoriaDetDTO objParamDTO)
        {
            using (var ctx = new MainContext())
            {
                try
                {
                    #region VALIDACIONES
                    if (objParamDTO.idArchivo <= 0) { throw new Exception("Ocurrió un error al descargar el archivo."); }
                    #endregion

                    #region SE OBTIENE LA INFORMACIÓN DEL ARCHIVO A DESCARGAR
                    tbl5s_AuditoriasArchivos objArchivo = ctx.tbl5s_AuditoriasArchivos.Where(w => w.id == objParamDTO.idArchivo && w.registroActivo).FirstOrDefault();
                    if (objArchivo == null)
                        throw new Exception("No se encuentra el archivo a descargar.");

                    var fileStream = GlobalUtils.GetFileAsStream(objArchivo.rutaArchivo);
                    string name = Path.GetFileName(objArchivo.rutaArchivo);

                    return Tuple.Create(fileStream, name);
                    #endregion
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public Dictionary<string, object> VisualizarArchivo(AuditoriaDetDTO objParamDTO)
        {
             using (var ctx = new MainContext())
            {
                try
                {
                    #region VALIDACIONES
                    string mensajeError = "Ocurrió un error al visualizar el archivo.";
                    if (objParamDTO.idArchivo <= 0) { throw new Exception(mensajeError); }
                    #endregion

                    #region SE OBTIENE LA INFORMACIÓN DEL ARCHIVO PARA VISUALIZAR
                    tbl5s_AuditoriasArchivos objArchivo = ctx.tbl5s_AuditoriasArchivos.Where(w => w.id == objParamDTO.idArchivo && w.registroActivo).FirstOrDefault();
                    if (objArchivo == null)
                        throw new Exception(mensajeError);

                    Stream fileStream = GlobalUtils.GetFileAsStream(objArchivo.rutaArchivo);
                    var byteArray = GlobalUtils.ConvertFileToByte(fileStream);

                    resultado.Add(SUCCESS, true);
                    resultado.Add("archivo", byteArray);
                    resultado.Add("extension", Path.GetExtension(objArchivo.rutaArchivo).ToUpper());
                    #endregion
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }
             return resultado;
        }
        
        public Dictionary<string, object> GuardarComentarioLider(int idAuditDet, string comentario)
        {
            resultado.Clear();

            using (var ctx = new MainContext())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        var objAuditoriaDet = ctx.tbl5s_AuditoriasDet.FirstOrDefault(e => e.id == idAuditDet);

                        objAuditoriaDet.comentarioLider = comentario;
                        ctx.SaveChanges();

                        transaction.Commit();

                        resultado.Add(SUCCESS, true);
                    }
                    catch (Exception e)
                    {
                        resultado.Add(MESSAGE, e.Message);
                        resultado.Add(SUCCESS, false);
                        
                    }
                }
            }

            return resultado;
        }
        #endregion

        #region GENERALES
        /// <summary>
        /// SE OBTIENE LISTADO DE CHECKLIST (EN EL FRONT LE LLAMAN: NOMBRE DE AUDITORIA)
        /// </summary>
        public Dictionary<string, object> FillCboCheckList()
        {
            using (var ctx = new MainContext())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        #region SE OBTIENE LISTADO DE CHECKLIST
                        List<tbl5s_CheckList> lstCheckList = ctx.tbl5s_CheckList.Where(w => w.registroActivo).OrderBy(o => o.nombre).ToList();
                        if (lstCheckList.Count() <= 0)
                            throw new Exception("No se encuentra ningún CheckList cargado, es necesario registrar al menos uno.");

                        List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                        ComboDTO objComboDTO = new ComboDTO();
                        foreach (var item in lstCheckList)
                        {
                            if (item.id > 0 && !string.IsNullOrEmpty(item.nombre))
                            {
                                objComboDTO = new ComboDTO();
                                objComboDTO.Value = item.id.ToString();
                                objComboDTO.Text = item.nombre.Trim();
                                lstComboDTO.Add(objComboDTO);
                            }
                        }

                        resultado.Add(SUCCESS, true);
                        resultado.Add(ITEMS, lstComboDTO);
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, ex.Message);
                    }
                }
            }
            return resultado;
        }

        /// <summary>
        /// SE OBTIENE LISTADO DE CC RELACIONADOS AL CHECKLIST SELECCIONADO
        /// </summary>
        public Dictionary<string, object> FillCboProyectos(AuditoriaDTO objParamDTO)
        {
            using (var ctx = new MainContext())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        #region VALIDACIONES
                        if (objParamDTO.checkListId <= 0) { throw new Exception("Ocurrió un error al obtener el listado de proyectos"); }
                        #endregion

                        #region SE OBTIENE LISTADO DE PROYECTOS EN BASE AL CHECKLIST SELECCIONADO
                        List<tbl5s_CC> lstCC_RelCheckList = ctx.tbl5s_CC.Where(w => w.checkListId == objParamDTO.checkListId && w.registroActivo).ToList();
                        if (lstCC_RelCheckList.Count <= 0)
                            throw new Exception("No cuenta con CC relacionados el CheckList.");

                        List<tblP_CC> lstCC = ctx.tblP_CC.Where(w => w.estatus).ToList();
                        List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                        ComboDTO objComboDTO = new ComboDTO();
                        foreach (var item in lstCC_RelCheckList)
                        {
                            if (item.id > 0 && !string.IsNullOrEmpty(item.cc))
                            {
                                objComboDTO = new ComboDTO();
                                objComboDTO.Value = item.cc.ToString();
                                objComboDTO.Text = GetCC_Descripcion(item.cc, lstCC);
                                lstComboDTO.Add(objComboDTO);
                            }
                        }

                        resultado.Add(SUCCESS, true);
                        resultado.Add(ITEMS, lstComboDTO);
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, ex.Message);
                    }
                }
            }
            return resultado;
        }

        /// <summary>
        /// SE OBTIENE EL NOMBRE COMPLETO DEL CC
        /// </summary>
        private string GetCC_Descripcion(string cc, List<tblP_CC> lstCC)
        {
            string cc_Descripcion = string.Empty;
            try
            {
                #region SE OBTIENE EL NOMBRE DEL CC
                tblP_CC objCC = lstCC.Where(w => w.cc == cc && w.estatus).FirstOrDefault();
                if (objCC != null)
                    cc_Descripcion = string.Format("[{0}] {1}", !string.IsNullOrEmpty(objCC.cc) ? objCC.cc.Trim() : string.Empty, !string.IsNullOrEmpty(objCC.descripcion) ? objCC.descripcion.Trim() : string.Empty);
                #endregion
            }
            catch (Exception)
            {
                return cc_Descripcion;
            }
            return cc_Descripcion;
        }

        /// <summary>
        /// SE VERIFICA SI EL NOMBRE DE LA CARPETA SE ENCUENTRA DISPONIBLE
        /// </summary>
        private static bool VerificarExisteCarpeta(string path, bool crear = false)
        {
            bool existe = false;
            try
            {
                existe = Directory.Exists(path);
                if (!existe && crear)
                {
                    Directory.CreateDirectory(path);
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
        /// SE SETEA EL NOMBRE DEL ARCHIVO A GUARDAR
        /// </summary>
        private string SetNombreArchivo(string nombreBase, string fileName)
        {
            return String.Format("{0}{1}{2}", nombreBase, fileName.Split('.')[0], Path.GetExtension(fileName));
        }

        /// <summary>
        /// SE OBTIENE EL NOMBRE COMPLETO DEL AUDITOR
        /// </summary>
        private string GetNombreAuditor(int usuario5sId, List<tbl5s_Usuario> lstUsuarios5s, List<tblP_Usuario> lstUsuarios)
        {
            string nombreCompleto = string.Empty;
            try
            {
                #region SE OBTIENE EL NOMBRE DEL AUDITOR
                int usuarioId = lstUsuarios5s.Where(w => w.id == usuario5sId && w.registroActivo).Select(s => s.usuarioId).FirstOrDefault();
                if (usuarioId > 0)
                {
                    tblP_Usuario objUsuario = lstUsuarios.Where(w => w.id == usuarioId && w.estatus).FirstOrDefault();
                    if (objUsuario != null)
                    {
                        nombreCompleto = string.Empty;
                        if (!string.IsNullOrEmpty(objUsuario.nombre))
                            nombreCompleto = objUsuario.nombre.Trim();
                        if (!string.IsNullOrEmpty(objUsuario.apellidoPaterno))
                            nombreCompleto += string.Format(" {0}", objUsuario.apellidoPaterno.Trim());
                        if (!string.IsNullOrEmpty(objUsuario.apellidoMaterno))
                            nombreCompleto += string.Format(" {0}", objUsuario.apellidoMaterno.Trim());
                    }
                }
                #endregion
            }
            catch (Exception)
            {
                return nombreCompleto;
            }
            return nombreCompleto;
        }

        /// <summary>
        /// SE OBTIENE EL PORCENTAJE DE CUMPLIMIENTO DE LA AUDITORIA EN BASE A LAS INSPECCIONES QUE CONTIENE REGISTRADAS,
        /// TENIENDO EN CUENTA SI LA INSPECCIÓN SE ENCUENTRA COMO "OK" O COMO "NO OKAY"
        /// </summary>
        private decimal GetPorcCumplimientoAuditoria(int auditoriaId)
        {
            decimal porcCumplimiento = 0;
            using (var ctx = new MainContext())
            {
                try
                {
                    #region SE OBTIENE EL PORCENTAJE DE CUMPLIMIENTO DE LA AUDITORIA
                    int cantPuntosAuditoria = 0;
                    int cantOkay = 0;
                    List<tbl5s_AuditoriasDet> lstAuditoriasDet = ctx.tbl5s_AuditoriasDet.Where(w => w.auditoriaId == auditoriaId && w.registroActivo).ToList();
                    foreach (var item in lstAuditoriasDet)
                    {
                        // SE OBTIENE LA CANTIDAD DE PUNTOS TOTALES QUE CONTIENE LA AUDITORIA
                        List<tbl5s_InspeccionDet> lstInspecciones = ctx.tbl5s_InspeccionDet.Where(w => w.inspeccionId == item.inspeccionId && w.registroActivo).ToList();
                        cantPuntosAuditoria += lstInspecciones.Count();

                        if (item.respuesta == (int)RespuestaEnum.OK)
                            cantOkay += lstInspecciones.Count();
                    }

                    porcCumplimiento = (((decimal)cantOkay / (decimal)cantPuntosAuditoria) * 100);
                    #endregion
                }
                catch (Exception)
                {
                    return (decimal)porcCumplimiento;
                }
            }
            return (decimal)porcCumplimiento;
        }

        /// <summary>
        /// SE OBTIENE LA DESCRIPCIÓN DE LA INSPECCIÓN
        /// </summary>
        private string GetDescripcionInspeccion(int inspeccionID, List<tbl5s_Inspeccion> lstInspecciones)
        {
            string descInspeccion = string.Empty;
            using (var ctx = new MainContext())
            {
                try
                {
                    #region VALIDACIONES
                    if (inspeccionID <= 0) { throw new Exception("Ocurrió un error al obtener la descripción de la inspección."); }
                    if (lstInspecciones.Count() <= 0) { throw new Exception("Ocurrió un error al obtener la descripción de la inspección."); }
                    #endregion

                    #region SE OBTIENE LA DESCRIPCIÓN DE LA INSPECCION
                    tbl5s_Inspeccion objInspeccion = lstInspecciones.Where(w => w.id == inspeccionID).FirstOrDefault();
                    if (objInspeccion == null)
                        throw new Exception("No se encuentra la descripción de la inspección");

                    descInspeccion = objInspeccion.inspeccion.Trim();
                    #endregion
                }
                catch (Exception)
                {
                    return descInspeccion;
                }
            }
            return descInspeccion;
        }

        private int GetArchivoID(int auditoriaDetID, List<tbl5s_AuditoriasArchivos> lstArchivos, int tipoArchivo)
        {
            int idArchivo = 0;
            using (var ctx = new MainContext())
            {
                try
                {
                    #region VALIDACIONES
                    if (auditoriaDetID <= 0) { throw new Exception("Ocurrió un error al obtener el ID del archivo"); }
                    #endregion

                    #region SE OBTIENE EL ID DEL ARCHIVO
                    if (tipoArchivo == (int)TipoArchivoEnum.SEGUIMIENTO)
                    {
                        tbl5s_AuditoriasArchivos objArchivo = lstArchivos.Where(w => w.auditoriaDetId == auditoriaDetID && w.tipoArchivo == tipoArchivo && w.registroActivo).OrderByDescending(o => o.id).FirstOrDefault();
                        if (objArchivo == null)
                            throw new Exception("Ocurrió un error al obtener el ID del archivo.");

                        idArchivo = objArchivo.id;
                    }
                    else
                    {
                        tbl5s_AuditoriasArchivos objArchivo = lstArchivos.Where(w => w.auditoriaDetId == auditoriaDetID && w.tipoArchivo == tipoArchivo && w.registroActivo).FirstOrDefault();
                        if (objArchivo == null)
                            throw new Exception("Ocurrió un error al obtener el ID del archivo.");

                        idArchivo = objArchivo.id;
                    }
                    #endregion
                }
                catch (Exception)
                {
                    return idArchivo;
                }
            }
            return idArchivo;
        }

        private int GetEstatusAuditoriaDet(int auditoriaDetID, List<tbl5s_AuditoriasArchivos> lstArchivos)
        {
            int estatusAuditoriaDet = 0;
            using (var ctx = new MainContext())
            {
                try
                {
                    #region VALIDACIONES
                    if (auditoriaDetID <= 0) { throw new Exception("Ocurrió un error al obtener el estatus de la auditoria detalle."); }
                    #endregion

                    #region SE OBTIENE EL ESTATUS DE LA AUDITORIA DETALLE.
                    tbl5s_AuditoriasArchivos objArchivo = lstArchivos.Where(w => w.auditoriaDetId == auditoriaDetID && w.tipoArchivo == (int)TipoArchivoEnum.SEGUIMIENTO && w.registroActivo).OrderByDescending(o => o.id).FirstOrDefault();
                    if (objArchivo == null)
                        estatusAuditoriaDet = (int)EstatusAuditoriaDet.DETECCION;
                    else
                    {
                        switch (objArchivo.aprobado)
                        {
                            case (int)EstatusArchivoSeguimientoEnum.PENDIENTE:
                                estatusAuditoriaDet = (int)EstatusAuditoriaDet.MEDIDAS_IMPLEMENTADAS;
                                break;
                            case (int)EstatusArchivoSeguimientoEnum.AUTORIZADO:
                                estatusAuditoriaDet = (int)EstatusAuditoriaDet.ACCIONES_REALIZADAS;
                                break;
                            case (int)EstatusArchivoSeguimientoEnum.RECHAZADO:
                                estatusAuditoriaDet = (int)EstatusAuditoriaDet.MEDIDAS_IMPLEMENTADAS;
                                break;
                        }
                    }
                    #endregion
                }
                catch (Exception)
                {
                    return estatusAuditoriaDet;
                }
            }
            return estatusAuditoriaDet;
        }

        private string GetComentarioRechazo(int idArchivo, List<tbl5s_AuditoriasArchivos> lstArchivos)
        {
            string comentarioRechazo = string.Empty;
            using (var ctx = new MainContext())
            {
                try
                {
                    #region SE OBTIENE EL COMENTARIO DE RECHAZO
                    tbl5s_AuditoriasArchivos objArchivo = lstArchivos
                            .Where(w => w.id == idArchivo && w.aprobado == (int)EstatusArchivoSeguimientoEnum.RECHAZADO && w.registroActivo).OrderByDescending(o => o.id).FirstOrDefault();
                    if (objArchivo != null)
                        comentarioRechazo = !string.IsNullOrEmpty(objArchivo.descripcion) ? objArchivo.descripcion.Trim() : string.Empty;
                    #endregion
                }
                catch (Exception)
                {
                    return comentarioRechazo;
                }
            }
            return comentarioRechazo;
        }

        private int GetEstatusArchivoSeguimiento(int idArchivoSeguimiento, List<tbl5s_AuditoriasArchivos> lstArchivos)
        {
            int archivoAprobado = 0;
            using (var ctx = new MainContext())
            {
                try
                {
                    #region SE OBTIENE SI EL ARCHIVO FUE APROBADO O RECHAZADO
                    tbl5s_AuditoriasArchivos objArchivo = lstArchivos
                        .Where(w => w.id == idArchivoSeguimiento && w.tipoArchivo == (int)TipoArchivoEnum.SEGUIMIENTO && w.registroActivo).OrderByDescending(o => o.id).FirstOrDefault();
                    if (objArchivo != null)
                    {
                        switch (objArchivo.aprobado)
                        {
                            case (int)EstatusArchivoSeguimientoEnum.PENDIENTE:
                                archivoAprobado = (int)EstatusArchivoSeguimientoEnum.PENDIENTE;
                                break;
                            case (int)EstatusArchivoSeguimientoEnum.AUTORIZADO:
                                archivoAprobado = (int)EstatusArchivoSeguimientoEnum.AUTORIZADO;
                                break;
                            case (int)EstatusArchivoSeguimientoEnum.RECHAZADO:
                                archivoAprobado = (int)EstatusArchivoSeguimientoEnum.RECHAZADO;
                                break;
                            default:
                                break;
                        }
                    }
                    #endregion
                }
                catch (Exception)
                {
                    return archivoAprobado;
                }
            }
            return archivoAprobado;
        }
        #endregion

        #region FACULTAMIENTOS
        public List<AutoCompleteDTO> GetUsuario(string term)
        {
            var datos = new List<AutoCompleteDTO>();

            using (var ctx = new MainContext())
            {
                try
                {
                    datos = ctx.tblP_Usuario
                        .Where(x =>
                            x.estatus &&
                            (
                                (!string.IsNullOrEmpty(x.nombre) ? x.nombre + " " : "") +
                                (!string.IsNullOrEmpty(x.apellidoPaterno) ? x.apellidoPaterno + " " : "") +
                                (!string.IsNullOrEmpty(x.apellidoMaterno) ? x.apellidoMaterno : "")
                            ).Contains(term))
                        .Take(10).ToList()
                        .Select(x => new AutoCompleteDTO
                        {
                            id = x.id.ToString(),
                            label = PersonalUtilities.NombreCompletoMayusculas(x.nombre, x.apellidoPaterno, x.apellidoMaterno)
                        }).ToList();
                }
                catch (Exception ex)
                {
                    datos.Add(new AutoCompleteDTO
                    {
                        id = 0.ToString(),
                        label = "ERROR EN LA BUSQUEDA"
                    });
                }
            }

            return datos;
        }

        public Dictionary<string, object> GetInfoUsuario(int idUsuario)
        {
            var ccFS = new CCFactoryService().getCCServiceSP();

            using (var ctx = new MainContext())
            {
                try
                {
                    var usuario = ctx.tblP_Usuario.FirstOrDefault(x => x.id == idUsuario && x.estatus);
                    if (usuario != null)
                    {
                        var cveEmpleado = Convert.ToInt32(usuario.cveEmpleado);
                        var empleado = ctx.tblRH_EK_Empleados.FirstOrDefault(x => x.clave_empleado == cveEmpleado);
                        var puesto = empleado != null ? ctx.tblRH_EK_Puestos.FirstOrDefault(x => x.puesto == empleado.puesto) : (tblRH_EK_Puestos)null;
                        var cc = empleado != null ? ccFS.GetCCNomina(empleado.cc_contable) : (ccDTO)null;

                        var datos = new
                        {
                            puesto = puesto != null ? puesto.descripcion : "",
                            proyecto = cc != null ? cc.cc + " - " + cc.descripcion : ""
                        };

                        resultado.Add(SUCCESS, true);
                        resultado.Add(ITEMS, datos);
                    }
                    else
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se encontró información del usuario");
                    }
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetAuditores(List<string> ccs)
        {
            var ccFS = new CCFactoryService().getCCServiceSP();

            using (var ctx = new MainContext())
            {
                try
                {
                    if (ccs == null)
                    {
                        ccs = new List<string>();
                    }

                    var auditores = ctx.tbl5s_Usuario
                        .Where(x =>
                            x.registroActivo &&
                            x.privilegioId.HasValue && x.privilegioId.Value == (int)PrivilegioEnum.Auditor &&
                            (ccs.Count > 0 ? x.ccs.Any(y => ccs.Contains(y.cc)) : true)).ToList()
                        .Select(x => new TablaAuditorDTO
                        {
                            id = x.id,
                            auditor = PersonalUtilities.NombreCompletoMayusculas(x.usuario.nombre, x.usuario.apellidoPaterno, x.usuario.apellidoMaterno),
                            ccs = x.ccs.Where(y => y.registroActivo).Select(y => y.cc).ToList()
                        }).ToList();

                    var ccsEncontrados = new List<string>();
                    foreach (var item in auditores)
                    {
                        ccsEncontrados.AddRange(item.ccs);
                    }

                    var ccsInfo = ccFS.GetCCsNominaFiltrados(ccsEncontrados);

                    foreach (var auditor in auditores)
                    {
                        var ccsArmados = ccsInfo.Where(x => auditor.ccs.Contains(x.cc)).Select(x => x.cc + " - " + x.descripcion).ToList();
                        auditor.ccs = ccsArmados;
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, auditores);

                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetFacultamientos(List<string> ccs, int? privilegioId)
        {
            #region VERSIÓN ANTERIOR
            //var ccFS = new CCFactoryService().getCCServiceSP();

            //using (var ctx = new MainContext())
            //{
            //    try
            //    {
            //        if (ccs == null)
            //        {
            //            ccs = new List<string>();
            //        }

            //        var auditores = ctx.tbl5s_Usuario
            //            .Where(x =>
            //                x.registroActivo &&
            //                x.privilegioId == privilegioId &&
            //                (ccs.Count > 0 ? x.ccs.Any(y => ccs.Contains(y.cc)) : true)).ToList()
            //            .Select(x => new TablaFacultamientoDTO
            //            {
            //                id = x.id,
            //                tipoUsuario = x.privilegioId.HasValue ? Infrastructure.Utils.EnumExtensions.GetDescription((PrivilegioEnum)x.privilegioId) : "",
            //                nombreCompleto = PersonalUtilities.NombreCompletoMayusculas(x.usuario.nombre, x.usuario.apellidoPaterno, x.usuario.apellidoMaterno),
            //                ccs = x.ccs.Where(y => y.registroActivo).Select(y => y.cc).ToList()
            //            }).ToList();

            //        var ccsEncontrados = new List<string>();
            //        foreach (var item in auditores)
            //        {
            //            ccsEncontrados.AddRange(item.ccs);
            //        }

            //        var ccsInfo = ccFS.GetCCsNominaFiltrados(ccsEncontrados);

            //        foreach (var auditor in auditores)
            //        {
            //            var ccsArmados = ccsInfo.Where(x => auditor.ccs.Contains(x.cc)).Select(x => x.cc + " - " + x.descripcion).ToList();
            //            auditor.ccs = ccsArmados;
            //        }

            //        resultado.Add(SUCCESS, true);
            //        resultado.Add(ITEMS, auditores);

            //    }
            //    catch (Exception ex)
            //    {
            //        resultado.Add(SUCCESS, false);
            //        resultado.Add(MESSAGE, ex.Message);
            //    }
            //}

            //return resultado;
            #endregion

            var ccFS = new CCFactoryService().getCCServiceSP();
            using (var _ctx = new MainContext())
            {
                try
                {
                    #region CATALOGOS
                    List<tbl5s_Usuario> lstUsuarios5s = _ctx.tbl5s_Usuario.Where(w => w.registroActivo).ToList();
                    List<tblP_Usuario> lstUsuarios = _ctx.tblP_Usuario.Where(w => w.estatus).ToList();
                    List<tblRH_EK_Empleados> lstEmpleados = _ctx.tblRH_EK_Empleados.ToList();
                    List<tbl5s_CC_Usuario> lstUsuario5sRelCC = _ctx.tbl5s_CC_Usuario.Where(w => w.registroActivo).ToList();
                    #endregion

                    List<tbl5s_UsuarioPrivilegios> lstUsuarios5sPrivilegios = _ctx.tbl5s_UsuarioPrivilegios.Where(w => w.privilegioId == privilegioId && w.registroActivo).ToList();
                    if (lstUsuarios5sPrivilegios.Count() <= 0)
                    {
                        #region SE OBTIENE LA INFORMACIÓN DE LOS USUARIOS SIN PRIVILEGIOS
                        lstUsuarios5sPrivilegios = _ctx.tbl5s_UsuarioPrivilegios.Where(w => w.registroActivo).ToList();

                        List<tbl5s_UsuarioPrivilegios> lstUsuarios5sSinPrivilegios = new List<tbl5s_UsuarioPrivilegios>();
                        foreach (var objUsuario5s in lstUsuarios5s)
                        {
                            if (!lstUsuarios5sPrivilegios.Any(w => w.usuario5sId == objUsuario5s.id && w.registroActivo))
                            {
                                tbl5s_UsuarioPrivilegios objUsuario5sSinPrivilegio = new tbl5s_UsuarioPrivilegios();
                                objUsuario5sSinPrivilegio.usuario5sId = objUsuario5s.id;
                                lstUsuarios5sSinPrivilegios.Add(objUsuario5sSinPrivilegio);
                            }
                        }
                        lstUsuarios5sPrivilegios = new List<tbl5s_UsuarioPrivilegios>();
                        lstUsuarios5sPrivilegios = lstUsuarios5sSinPrivilegios.ToList();
                        #endregion
                    }

                    List<TablaFacultamientoDTO> lstUsuariosFacultamientos = new List<TablaFacultamientoDTO>();
                    TablaFacultamientoDTO objUsuarioFacultamiento = new TablaFacultamientoDTO();
                    foreach (var item in lstUsuarios5sPrivilegios)
                    {
                        objUsuarioFacultamiento = new TablaFacultamientoDTO();
                        objUsuarioFacultamiento.id = item.usuario5sId;
                        objUsuarioFacultamiento.tipoUsuario = item.privilegioId > 0 ? Infrastructure.Utils.EnumExtensions.GetDescription((PrivilegioEnum)item.privilegioId) : string.Empty;

                        #region SE OBTIENE EL NOMBRE COMPLETO DEL EMPLEADO
                        int usuarioFK = lstUsuarios5s.Where(w => w.id == item.usuario5sId).Select(s => s.usuarioId).FirstOrDefault();
                        if (usuarioFK <= 0) { throw new Exception("Ocurrió un error al obtener el nombre completo del usuario."); }
                        string claveEmpleado = lstUsuarios.Where(w => w.id == usuarioFK).Select(s => s.cveEmpleado).FirstOrDefault();
                        tblRH_EK_Empleados objEmpleado = lstEmpleados.Where(w => w.clave_empleado == Convert.ToInt32(claveEmpleado) && w.esActivo).FirstOrDefault();

                        if (objEmpleado != null)
                            objUsuarioFacultamiento.nombreCompleto = PersonalUtilities.NombreCompletoMayusculas(objEmpleado.nombre ?? string.Empty, objEmpleado.ape_paterno ?? string.Empty, objEmpleado.ape_materno ?? string.Empty);
                        #endregion

                        #region SE OBTIENE LISTADO DE CC RELACIONADOS AL USUARIO
                        ccs = new List<string>();
                        ccs = lstUsuario5sRelCC.Where(w => w.usuario5sId == item.usuario5sId).Select(s => s.cc).ToList();
                        objUsuarioFacultamiento.ccs = ccs;
                        #endregion

                        lstUsuariosFacultamientos.Add(objUsuarioFacultamiento);
                    }

                    List<string> ccsEncontrados = new List<string>();
                    foreach (var item in lstUsuariosFacultamientos)
                    {
                        ccsEncontrados.AddRange(item.ccs);
                    }

                    var ccsInfo = ccFS.GetCCsNominaFiltrados(ccsEncontrados);
                    foreach (var auditor in lstUsuariosFacultamientos)
                    {
                        var ccsArmados = ccsInfo.Where(w => auditor.ccs.Contains(w.cc)).Select(s => s.cc + " - " + s.descripcion).ToList();
                        auditor.ccs = ccsArmados;
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, lstUsuariosFacultamientos);
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                    LogError(0, 10, NombreControlador, "GetFacultamientos", ex, AccionEnum.CONSULTA, 0, new { ccs = ccs, privilegioId = privilegioId });
                }
            }
            return resultado;
        }

        public Dictionary<string, object> GetAuditor(int idAuditor)
        {
            var ccFS = new CCFactoryService().getCCServiceSP();

            using (var ctx = new MainContext())
            {
                try
                {
                    var auditor = ctx.tbl5s_Usuario.FirstOrDefault(x => x.id == idAuditor && x.registroActivo);
                    if (auditor != null)
                    {
                        var cveEmpleado = Convert.ToInt32(auditor.usuario.cveEmpleado);
                        var empleado = ctx.tblRH_EK_Empleados.FirstOrDefault(x => x.clave_empleado == cveEmpleado);
                        var puesto = ctx.tblRH_EK_Puestos.FirstOrDefault(x => x.puesto == empleado.puesto);
                        var cc = ccFS.GetCCNomina(empleado.cc_contable);

                        var datos = new AuditorInfoDTO();
                        datos.id = idAuditor;
                        datos.auditor = PersonalUtilities.NombreCompletoMayusculas(auditor.usuario.nombre, auditor.usuario.apellidoPaterno, auditor.usuario.apellidoMaterno);
                        datos.puesto = puesto != null ? puesto.descripcion : "";
                        datos.proyecto = cc != null ? cc.descripcion : "";
                        datos.css = auditor.ccs.Where(x => x.registroActivo).Select(x => x.cc).ToList();

                        resultado.Add(SUCCESS, true);
                        resultado.Add(ITEMS, datos);
                    }
                    else
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se encontró información del auditor");
                    }
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EliminarAuditor(int idAuditor)
        {
            using (var ctx = new MainContext())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        var auditor = ctx.tbl5s_Usuario.FirstOrDefault(x => x.id == idAuditor);
                        if (auditor != null)
                        {
                            auditor.registroActivo = false;
                            auditor.fechaModificacion = DateTime.Now;
                            auditor.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                            ctx.SaveChanges();

                            foreach (var item in auditor.lideresArea.Where(x => x.registroActivo))
                            {
                                item.registroActivo = false;
                                item.fechaModificacion = auditor.fechaModificacion;
                                item.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                                ctx.SaveChanges();
                            }

                            foreach (var item in auditor.ccs.Where(x => x.registroActivo))
                            {
                                item.registroActivo = false;
                                item.fechaModificacion = auditor.fechaModificacion;
                                item.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                                ctx.SaveChanges();
                            }

                            transaction.Commit();

                            resultado.Add(SUCCESS, true);
                        }
                        else
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "No se encontró el auditor a eliminar");
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, ex.Message);
                    }
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EditarAuditor(AuditorInfoDTO info)
        {
            using (var ctx = new MainContext())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        var auditor = ctx.tbl5s_Usuario.FirstOrDefault(x => x.registroActivo && x.id == info.id);
                        if (auditor != null)
                        {
                            auditor.fechaModificacion = DateTime.Now;
                            auditor.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;

                            foreach (var item in auditor.ccs.Where(x => x.registroActivo))
                            {
                                if (!info.css.Contains(item.cc))
                                {
                                    item.registroActivo = false;
                                    item.fechaModificacion = auditor.fechaModificacion;
                                    item.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                                    ctx.SaveChanges();
                                }
                            }

                            foreach (var item in info.css)
                            {
                                if (!auditor.ccs.Any(x => x.registroActivo && x.cc == item))
                                {
                                    var ccNuevo = new tbl5s_CC_Usuario();
                                    ccNuevo.cc = item;
                                    ccNuevo.usuario5sId = auditor.id;
                                    ccNuevo.fechaCreacion = auditor.fechaModificacion.Value;
                                    ccNuevo.usuarioCreacionId = vSesiones.sesionUsuarioDTO.id;
                                    ccNuevo.registroActivo = true;
                                    ctx.tbl5s_CC_Usuario.Add(ccNuevo);
                                    ctx.SaveChanges();
                                }
                            }

                            foreach (var item in auditor.lideresArea.Where(x => x.registroActivo))
                            {
                                if (!item.checkList.ccs.Where(x => x.registroActivo).Any(x => x.registroActivo && info.css.Contains(x.cc)))
                                {
                                    item.fechaModificacion = auditor.fechaModificacion;
                                    item.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                                    item.registroActivo = false;
                                    ctx.SaveChanges();
                                }
                            }

                            ctx.SaveChanges();

                            transaction.Commit();

                            resultado.Add(SUCCESS, true);
                        }
                        else
                        {
                            transaction.Rollback();

                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "No se encontró información del auditor");
                        }
                    }
                    catch (Exception ex)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, ex.Message);
                    }
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarAuditor(GuardarUsuarioDTO usuario)
        {
            using (var ctx = new MainContext())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        if (usuario.ccs != null && usuario.ccs.Count > 0)
                        {
                            if (!ctx.tbl5s_Usuario.Any(x => x.registroActivo && usuario.usuarioId == x.usuarioId))
                            {
                                var nuevoUsuario = new tbl5s_Usuario();

                                nuevoUsuario.usuarioId = usuario.usuarioId;
                                nuevoUsuario.fechaCreacion = DateTime.Now;
                                nuevoUsuario.usuarioCreacionId = vSesiones.sesionUsuarioDTO.id;
                                nuevoUsuario.registroActivo = true;
                                ctx.tbl5s_Usuario.Add(nuevoUsuario);
                                ctx.SaveChanges();

                                var ccUsuarios = new List<tbl5s_CC_Usuario>();
                                foreach (var item in usuario.ccs)
                                {
                                    var ccUsuario = new tbl5s_CC_Usuario();
                                    ccUsuario.cc = item;
                                    ccUsuario.usuario5sId = nuevoUsuario.id;
                                    ccUsuario.fechaCreacion = nuevoUsuario.fechaCreacion;
                                    ccUsuario.usuarioCreacionId = vSesiones.sesionUsuarioDTO.id;
                                    ccUsuario.registroActivo = true;
                                    ccUsuarios.Add(ccUsuario);
                                }
                                ctx.tbl5s_CC_Usuario.AddRange(ccUsuarios);
                                ctx.SaveChanges();

                                transaction.Commit();

                                resultado.Add(SUCCESS, true);
                            }
                            else
                            {
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "El usuario ya existe");
                            }
                        }
                        else
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "Tiene que agregar al menos un CC");
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        resultado.Add(SUCCESS, true);
                        resultado.Add(MESSAGE, ex.Message);
                    }
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetAuditorPrivilegio(int idAuditor)
        {
            using (var ctx = new MainContext())
            {
                try
                {
                    var auditor = ctx.tbl5s_Usuario.FirstOrDefault(x => x.registroActivo && x.id == idAuditor);
                    if (auditor != null)
                    {
                        var cveEmpleado = Convert.ToInt32(auditor.usuario.cveEmpleado);
                        var empleado = ctx.tblRH_EK_Empleados.FirstOrDefault(x => x.clave_empleado == cveEmpleado);
                        if (empleado != null)
                        {
                            var puesto = ctx.tblRH_EK_Puestos.FirstOrDefault(x => x.puesto == empleado.puesto);
                            var datos = new AuditorPrivilegioDTO();
                            datos.id = auditor.id;
                            datos.auditor = PersonalUtilities.NombreCompletoMayusculas(auditor.usuario.nombre, auditor.usuario.apellidoPaterno, auditor.usuario.apellidoMaterno);
                            datos.puesto = puesto != null ? puesto.descripcion : "";
                            //datos.privilegioId = auditor.privilegioId; // TO DO

                            resultado.Add(SUCCESS, true);
                            resultado.Add(ITEMS, datos);
                        }
                        else
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "No se encontró información del empleado");
                        }                        
                    }
                    else
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se encontró información del auditor");
                    }
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarAuditorPrivilegio(AuditorPrivilegioDTO privilegio)
        {
            #region VERSIÓN ANTERIOR
            //using (var ctx = new MainContext())
            //{
            //    using (var transaction = ctx.Database.BeginTransaction())
            //    {
            //        try
            //        {
            //            var fechaActual = DateTime.Now;

            //            var usuario = ctx.tbl5s_Usuario.FirstOrDefault(x => x.registroActivo && x.id == privilegio.id);
            //            if (usuario != null)
            //            {
            //                if (usuario.privilegioId.HasValue && usuario.privilegioId.Value == (int)PrivilegioEnum.Lider && ((privilegio.privilegioId.HasValue && privilegio.privilegioId != (int)PrivilegioEnum.Lider) || (!privilegio.privilegioId.HasValue)))
            //                {
            //                    foreach (var item in usuario.lideresArea.Where(x => x.registroActivo))
            //                    {
            //                        item.fechaModificacion = fechaActual;
            //                        item.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
            //                        item.registroActivo = false;
            //                        ctx.SaveChanges();
            //                    }
            //                }

            //                usuario.privilegioId = privilegio.privilegioId;
            //                usuario.fechaModificacion = DateTime.Now;
            //                usuario.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
            //                ctx.SaveChanges();

            //                transaction.Commit();

            //                resultado.Add(SUCCESS, true);
            //            }
            //            else
            //            {
            //                resultado.Add(SUCCESS, false);
            //                resultado.Add(MESSAGE, "No se encontró información del usuario");
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            transaction.Rollback();

            //            resultado.Add(SUCCESS, false);
            //            resultado.Add(MESSAGE, ex.Message);
            //        }
            //    }
            //}

            //return resultado;
            #endregion
            
            using (var _ctx = new MainContext())
            {
                using (var transaction = _ctx.Database.BeginTransaction())
                {
                    try
                    {
                        DateTime fechaActual = DateTime.Now;

                        tbl5s_Usuario objUsuario = _ctx.tbl5s_Usuario.Where(w => w.id == privilegio.id && w.registroActivo).FirstOrDefault();
                        if (objUsuario != null)
                        {
                            // SE VERIFICA SI EL USUARIO CONSULTADO ES LIDER Y SE VERIFICA SI DEJARA DE SER LIDER PARA ELIMINARLO COMO LIDER AREA
                            bool tienePrivilegioLider = _ctx.tbl5s_UsuarioPrivilegios.Any(w => w.privilegioId == (int)PrivilegioEnum.Lider && w.registroActivo);
                            bool continuaConProvilegioLider = privilegio.lstPrivilegiosID.Any(w => w == (int)PrivilegioEnum.Lider);
                            if (tienePrivilegioLider && !continuaConProvilegioLider)
                            {
                                foreach (var item in objUsuario.lideresArea.Where(w => w.registroActivo))
                                {
                                    item.fechaModificacion = fechaActual;
                                    item.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                                    item.registroActivo = false;
                                    _ctx.SaveChanges();
                                }
                            }
                            objUsuario.fechaModificacion = DateTime.Now;
                            objUsuario.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                            _ctx.SaveChanges();

                            #region SE REGISTRA LOS PRIVILEGIOS AL USUARIO
                            // SE ELIMINA LOS PRIVILEGIOS ANTERIORES
                            List<tbl5s_UsuarioPrivilegios> lstUsuarioPrivilegiosAnteriores = _ctx.tbl5s_UsuarioPrivilegios.Where(w => w.usuario5sId == objUsuario.id && w.registroActivo).ToList();
                            foreach (var item in lstUsuarioPrivilegiosAnteriores)
                            {
                                item.fechaModificacion = fechaActual;
                                item.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                                item.registroActivo = false;
                            }
                            _ctx.SaveChanges();

                            // SE REGISTRA LOS NUEVOS PRIVILEGIOS AL USUARIO
                            List<tbl5s_UsuarioPrivilegios> lstNuevosPrivilegios = new List<tbl5s_UsuarioPrivilegios>();
                            tbl5s_UsuarioPrivilegios objNuevoPrivilegio = new tbl5s_UsuarioPrivilegios();
                            foreach (var item in privilegio.lstPrivilegiosID)
                            {
                                objNuevoPrivilegio = new tbl5s_UsuarioPrivilegios();
                                objNuevoPrivilegio.usuario5sId = objUsuario.id;
                                objNuevoPrivilegio.privilegioId = item;
                                objNuevoPrivilegio.fechaCreacion = fechaActual;
                                objNuevoPrivilegio.registroActivo = true;
                                lstNuevosPrivilegios.Add(objNuevoPrivilegio);
                            }
                            _ctx.tbl5s_UsuarioPrivilegios.AddRange(lstNuevosPrivilegios);
                            _ctx.SaveChanges();
                            #endregion

                            resultado.Add(SUCCESS, true);
                            transaction.Commit();

                            // SE REGISTRA BITACORA
                            SaveBitacora(10, (int)AccionEnum.ACTUALIZAR, privilegio.id, JsonUtils.convertNetObjectToJson(privilegio));
                        }
                        else
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "No se encontró información del usuario");
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogError(0, 10, "CincoSController", "GuardarAuditorPrivilegio", ex, AccionEnum.ACTUALIZAR, privilegio.id, privilegio);
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, ex.Message);
                    }
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EliminarAuditorPrivilegio(int idAuditor)
        {
            using (var ctx = new MainContext())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        var fechaActual = DateTime.Now;

                        var auditor = ctx.tbl5s_Usuario.FirstOrDefault(x => x.id == idAuditor);
                        if (auditor != null)
                        {
                            if (auditor.privilegioId.HasValue && auditor.privilegioId.Value == (int)PrivilegioEnum.Lider)
                            {
                                foreach (var item in auditor.lideresArea)
                                {
                                    item.fechaModificacion = fechaActual;
                                    item.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                                    item.registroActivo = false;
                                    ctx.SaveChanges();
                                }
                            }

                            auditor.privilegioId = null;
                            auditor.fechaModificacion = fechaActual;
                            auditor.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                            ctx.SaveChanges();

                            transaction.Commit();

                            resultado.Add(SUCCESS, true);
                        }
                        else
                        {
                            transaction.Rollback();

                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "No se encontró al auditor");
                        }
                    }
                    catch (Exception ex)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, ex.Message);
                    }
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetTablaLideres(List<string> ccs)
        {
            #region VERSION ANTERIOR
            //using (var ctx = new MainContext())
            //{
            //    try
            //    {
            //        if (ccs == null)
            //        {
            //            ccs = new List<string>();
            //        }

            //        var lideres = ctx.tbl5s_Usuario
            //            .Where(x =>
            //                (ccs.Count > 0 ? x.ccs.Any(a => ccs.Contains(a.cc)) : true) &&
            //                x.registroActivo &&
            //                x.privilegioId.HasValue &&
            //                x.privilegioId.Value == (int)PrivilegioEnum.Lider)
            //            .Select(x => new
            //            {
            //                id = x.id,
            //                nombre = x.usuario.nombre,
            //                apePaterno = x.usuario.apellidoPaterno,
            //                apeMaterno = x.usuario.apellidoMaterno,
            //                cveEmpleado = x.usuario.cveEmpleado,
            //                grupo = x.areaOperativaId,
            //                grupoDesc = x.areaOperativaId.HasValue ? x.areaOperativa.descripcion : "",
            //                ccs = x.ccs.Where(y => y.registroActivo).Select(y => y.cc).ToList()
            //            }).ToList();

            //        var cveEmpleados = lideres.Select(x => Convert.ToInt32(x.cveEmpleado)).ToList();

            //        var empleados = ctx.tblRH_EK_Empleados.Where(x => cveEmpleados.Contains(x.clave_empleado)).ToList();
            //        var puestosEmpleados = empleados.Select(x => x.puesto).ToList();
            //        var puestos = ctx.tblRH_EK_Puestos.Where(x => puestosEmpleados.Contains(x.puesto)).ToList();

            //        var datos = new List<TablaLideresDTO>();

            //        foreach (var item in lideres)
            //        {
            //            var empleado = empleados.FirstOrDefault(x => x.clave_empleado == Convert.ToInt32(item.cveEmpleado));
            //            var puesto = empleado != null ? puestos.FirstOrDefault(x => x.puesto == empleado.puesto) : (tblRH_EK_Puestos)null;

            //            var lider = new TablaLideresDTO();
            //            lider.id = item.id;
            //            lider.nombre = PersonalUtilities.NombreCompletoMayusculas(item.nombre, item.apePaterno, item.apeMaterno);
            //            lider.puesto = puesto != null ? puesto.descripcion : "";
            //            lider.ccs = item.ccs;
            //            lider.grupo = item.grupoDesc.ToUpper();
            //            datos.Add(lider);
            //        }

            //        resultado.Add(SUCCESS, true);
            //        resultado.Add(ITEMS, datos);
            //    }
            //    catch (Exception ex)
            //    {
            //        resultado.Add(SUCCESS, false);
            //        resultado.Add(MESSAGE, ex.Message);
            //    }
            //}

            //return resultado;
            #endregion

            using (var ctx = new MainContext())
            {
                try
                {
                    if (ccs == null)
                        ccs = new List<string>();

                    List<int> lstUsuariosLideresFK = _context.tbl5s_UsuarioPrivilegios.Where(w => w.privilegioId == (int)PrivilegioEnum.Lider && w.registroActivo).Select(s => s.usuario5sId).ToList();
                    List<tbl5s_UsuarioAreaOperativa> lstUsuarioAreaOperativa = ctx.tbl5s_UsuarioAreaOperativa.Where(w => w.registroActivo).ToList();

                    var lideres = ctx.tbl5s_Usuario.Where(x => (ccs.Count > 0 ? x.ccs.Any(a => ccs.Contains(a.cc)) : true) && x.registroActivo && lstUsuariosLideresFK.Contains(x.id))
                        .Select(x => new
                        {
                            id = x.id,
                            nombre = x.usuario.nombre,
                            apePaterno = x.usuario.apellidoPaterno,
                            apeMaterno = x.usuario.apellidoMaterno,
                            cveEmpleado = x.usuario.cveEmpleado,
                            grupoDesc = string.Empty,
                            ccs = x.ccs.Where(y => y.registroActivo).Select(y => y.cc).ToList()
                        }).ToList();

                    var cveEmpleados = lideres.Select(x => Convert.ToInt32(x.cveEmpleado)).ToList();
                    var empleados = ctx.tblRH_EK_Empleados.Where(x => cveEmpleados.Contains(x.clave_empleado)).ToList();
                    var puestosEmpleados = empleados.Select(x => x.puesto).ToList();
                    var puestos = ctx.tblRH_EK_Puestos.Where(x => puestosEmpleados.Contains(x.puesto)).ToList();

                    var datos = new List<TablaLideresDTO>();

                    foreach (var item in lideres)
                    {
                        var empleado = empleados.FirstOrDefault(x => x.clave_empleado == Convert.ToInt32(item.cveEmpleado));
                        var puesto = empleado != null ? puestos.FirstOrDefault(x => x.puesto == empleado.puesto) : (tblRH_EK_Puestos)null;

                        var lider = new TablaLideresDTO();
                        lider.id = item.id;
                        lider.nombre = PersonalUtilities.NombreCompletoMayusculas(item.nombre, item.apePaterno, item.apeMaterno);
                        lider.puesto = puesto != null ? puesto.descripcion : "";
                        lider.ccs = item.ccs;
                        lider.grupo = GetAreasOperativasRelUsuario(item.id, lstUsuarioAreaOperativa);
                        datos.Add(lider);
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, datos);
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                    LogError(0, 0, NombreControlador, "GetTablaLideres", ex, AccionEnum.CONSULTA, 0, 0);
                }
            }

            return resultado;
        }

        private string GetAreasOperativasRelUsuario(int usuario5sId, List<tbl5s_UsuarioAreaOperativa> lstUsuarioAreaOperativa)
        {
            using (var ctx = new MainContext())
            {
                string grupoDesc = string.Empty;
                List<int> lstAreaOperativaFK = lstUsuarioAreaOperativa.Where(w => w.usuario5sId == usuario5sId).Select(s => s.areaOperativaId).ToList();
                List<tbl5s_AreaOperativa> lstAreas = ctx.tbl5s_AreaOperativa.Where(w => lstAreaOperativaFK.Contains(w.id) && w.registroActivo).ToList();

                foreach (var item in lstAreas)
                {
                    grupoDesc += string.Format("&nbsp;<span class='label label-info'>{0}</span>", item.descripcion);
                }
                return grupoDesc;
            }
        }

        public Dictionary<string, object> GetLider(int idLider)
        {
            using (var ctx = new MainContext())
            {
                try
                {
                    var usuario = ctx.tbl5s_Usuario.FirstOrDefault(x => x.id == idLider);
                    if (usuario != null)
                    {
                        var lider = new LiderInfoDTO();
                        lider.id = usuario.id;
                        lider.nombre = PersonalUtilities.NombreCompletoMayusculas(usuario.usuario.nombre, usuario.usuario.apellidoPaterno, usuario.usuario.apellidoMaterno);
                        lider.ccs = usuario.ccs.Where(x => x.registroActivo).Select(x => x.cc).ToList();
                        lider.grupoId = usuario.areaOperativaId;

                        List<int> lstGruposUsuario = ctx.tbl5s_UsuarioAreaOperativa.Where(w => w.usuario5sId == usuario.id && w.registroActivo).Select(s => s.areaOperativaId).ToList();
                        lider.lstGruposID = new List<int>();
                        lider.lstGruposID.AddRange(lstGruposUsuario);

                        resultado.Add(SUCCESS, true);
                        resultado.Add(ITEMS, lider);
                    }
                    else
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se encontró información del líder");
                    }
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarLider(LiderInfoDTO info)
        {
            #region VERSIÓN ANTERIOR
            //using (var ctx = new MainContext())
            //{
            //    using (var transaction = ctx.Database.BeginTransaction())
            //    {
            //        try
            //        {
            //            var fechaActual = DateTime.Now;

            //            var lider = ctx.tbl5s_Usuario.FirstOrDefault(x => x.id == info.id);
            //            if (lider != null)
            //            {
            //                foreach (var item in lider.lideresArea.Where(x => x.registroActivo))
            //                {
            //                    if (!item.checkList.ccs.Where(x => x.registroActivo).Any(x => x.registroActivo && info.ccs.Contains(x.cc)))
            //                    {
            //                        item.fechaModificacion = fechaActual;
            //                        item.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
            //                        item.registroActivo = false;
            //                        ctx.SaveChanges();
            //                    }
            //                }

            //                foreach (var item in lider.ccs.Where(x => x.registroActivo))
            //                {
            //                    if (!info.ccs.Contains(item.cc))
            //                    {
            //                        item.fechaModificacion = fechaActual;
            //                        item.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
            //                        item.registroActivo = false;
            //                        ctx.SaveChanges();
            //                    }
            //                }

            //                foreach (var item in info.ccs)
            //                {
            //                    if (!lider.ccs.Where(x => x.registroActivo).Any(x => x.cc == item))
            //                    {
            //                        var nuevoCc = new tbl5s_CC_Usuario();
            //                        nuevoCc.cc = item;
            //                        nuevoCc.usuario5sId = lider.id;
            //                        nuevoCc.fechaCreacion = fechaActual;
            //                        nuevoCc.usuarioCreacionId = vSesiones.sesionUsuarioDTO.id;
            //                        nuevoCc.registroActivo = true;
            //                        ctx.tbl5s_CC_Usuario.Add(nuevoCc);
            //                        ctx.SaveChanges();
            //                    }
            //                }

            //                lider.areaOperativaId = info.grupoId;
            //                lider.fechaModificacion = fechaActual;
            //                lider.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
            //                lider.registroActivo = true;
            //                ctx.SaveChanges();

            //                transaction.Commit();

            //                resultado.Add(SUCCESS, true);
            //            }
            //            else
            //            {

            //                resultado.Add(SUCCESS, false);
            //                resultado.Add(MESSAGE, "No se encontró información del líder para editar");
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            transaction.Rollback();

            //            resultado.Add(SUCCESS, false);
            //            resultado.Add(MESSAGE, ex.Message);
            //        }
            //    }
            //}

            //return resultado;
            #endregion

            using (var _ctx = new MainContext())
            {
                using (var transaction = _ctx.Database.BeginTransaction())
                {
                    try
                    {
                        var fechaActual = DateTime.Now;

                        tbl5s_Usuario lider = _ctx.tbl5s_Usuario.Where(w => w.id == info.id).FirstOrDefault();
                        if (lider != null)
                        {
                            foreach (var item in lider.lideresArea.Where(x => x.registroActivo))
                            {
                                if (item.checkList != null)
                                {
                                    if (!item.checkList.ccs.Where(x => x.registroActivo).Any(x => x.registroActivo && info.ccs.Contains(x.cc)))
                                    {
                                        item.fechaModificacion = fechaActual;
                                        item.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                                        item.registroActivo = false;
                                        _ctx.SaveChanges();
                                    }
                                }
                            }

                            foreach (var item in lider.ccs.Where(x => x.registroActivo))
                            {
                                if (!info.ccs.Contains(item.cc))
                                {
                                    item.fechaModificacion = fechaActual;
                                    item.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                                    item.registroActivo = false;
                                    _ctx.SaveChanges();
                                }
                            }

                            foreach (var item in info.ccs)
                            {
                                if (!lider.ccs.Where(x => x.registroActivo).Any(x => x.cc == item))
                                {
                                    var nuevoCc = new tbl5s_CC_Usuario();
                                    nuevoCc.cc = item;
                                    nuevoCc.usuario5sId = lider.id;
                                    nuevoCc.fechaCreacion = fechaActual;
                                    nuevoCc.usuarioCreacionId = vSesiones.sesionUsuarioDTO.id;
                                    nuevoCc.registroActivo = true;
                                    _ctx.tbl5s_CC_Usuario.Add(nuevoCc);
                                    _ctx.SaveChanges();
                                }
                            }

                            lider.fechaModificacion = fechaActual;
                            lider.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                            lider.registroActivo = true;
                            _ctx.SaveChanges();

                            #region SE REGISTRA LAS NUEVAS AREAS/OPERATIVAS AL USUARIO
                            // SE ELIMINA LAS AREAS OPERATIVAS QUE TIENE EL USUARIO
                            List<tbl5s_UsuarioAreaOperativa> lstUsuarioRelAreasOperativas = _ctx.tbl5s_UsuarioAreaOperativa.Where(w => w.usuario5sId == info.id && w.registroActivo).ToList();
                            foreach (var item in lstUsuarioRelAreasOperativas)
                            {
                                item.fechaModificacion = fechaActual;
                                item.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                                item.registroActivo = false;
                                _ctx.SaveChanges();
                            }

                            // SE REGISTRA EL NUEVO AREA/OPERATIVA AL USUARIO
                            List<tbl5s_UsuarioAreaOperativa> lstAreasNuevas = new List<tbl5s_UsuarioAreaOperativa>();
                            tbl5s_UsuarioAreaOperativa objAreaNueva = new tbl5s_UsuarioAreaOperativa();
                            foreach (var item in info.lstGruposID)
                            {
                                objAreaNueva = new tbl5s_UsuarioAreaOperativa();
                                objAreaNueva.usuario5sId = info.id;
                                objAreaNueva.areaOperativaId = item;
                                objAreaNueva.usuarioCreacionId = vSesiones.sesionUsuarioDTO.id;
                                objAreaNueva.fechaCreacion = DateTime.Now;
                                objAreaNueva.registroActivo = true;
                                lstAreasNuevas.Add(objAreaNueva);
                            }
                            _ctx.tbl5s_UsuarioAreaOperativa.AddRange(lstAreasNuevas);
                            _ctx.SaveChanges();
                            #endregion

                            transaction.Commit();
                            resultado.Add(SUCCESS, true);

                            // SE REGISTRA BITACORA
                            SaveBitacora(10, info.id <= 0 ? (int)AccionEnum.AGREGAR : (int)AccionEnum.ACTUALIZAR, info.id, JsonUtils.convertNetObjectToJson(info));
                        }
                        else
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "No se encontró información del líder para editar");
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, ex.Message);
                        LogError(0, 0, NombreControlador, "GuardarLider", ex, info.id <= 0 ? AccionEnum.AGREGAR : AccionEnum.ACTUALIZAR, 0, 0);
                    }
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EliminarLider(int idLider)
        {
            using (var ctx = new MainContext())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        var lider = ctx.tbl5s_Usuario.FirstOrDefault(x => x.registroActivo && x.id == idLider);
                        if (lider != null)
                        {
                            lider.privilegioId = null;
                            lider.fechaModificacion = DateTime.Now;
                            lider.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                            ctx.SaveChanges();

                            foreach (var item in lider.lideresArea.Where(x => x.registroActivo))
                            {
                                item.fechaModificacion = lider.fechaModificacion;
                                item.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                                item.registroActivo = false;
                                ctx.SaveChanges();
                            }

                            transaction.Commit();

                            resultado.Add(SUCCESS, true);
                        }
                        else
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "No se encontró información del líder a eliminar");
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, ex.Message);
                    }
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetAreaOperativaLider()
        {
            using (var ctx = new MainContext())
            {
                try
                {
                    var areasOperativas = ctx.tbl5s_AreaOperativa.Where(x => x.registroActivo)
                        .Select(x => new ComboDTO
                        {
                            Value = x.id.ToString(),
                            Text = x.descripcion
                        }).ToList();

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, areasOperativas);
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetPrivilegios()
        {
            using (var ctx = new MainContext())
            {
                try
                {
                    var privilegios = ctx.tbl5s_Privilegio.Where(x => x.registroActivo)
                        .Select(x => new ComboDTO
                        {
                            Value = x.id.ToString(),
                            Text = x.descripcion
                        }).ToList();

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, privilegios);
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetTablaSubAreas()
        {
            using (var ctx = new MainContext())
            {
                try
                {
                    var subAreas = ctx.tbl5s_SubArea.Where(x => x.registroActivo)
                        .Select(x => new
                        {
                            id = x.id,
                            nombre = x.descripcion,
                        }).ToList();

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, subAreas);
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetSubArea(int id)
        {
            using (var ctx = new MainContext())
            {
                try
                {
                    var subArea = ctx.tbl5s_SubArea.FirstOrDefault(x => x.registroActivo && x.id == id);
                    if (subArea != null)
                    {
                        resultado.Add(SUCCESS, true);
                        resultado.Add(ITEMS, subArea);
                    }
                    else
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se encontro informacion del subarea");
                    }
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EditarSubArea(int id, string nombre)
        {
            using (var ctx = new MainContext())
            {
                try
                {
                    var subArea = ctx.tbl5s_SubArea.FirstOrDefault(x => x.registroActivo && x.id == id);
                    if (subArea != null)
                    {
                        subArea.descripcion = nombre;
                        ctx.SaveChanges();

                        resultado.Add(SUCCESS, true);
                    }
                    else
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se encontro informacion del subarea");
                    }
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EliminarSubArea(int id)
        {
            using (var ctx = new MainContext())
            {
                try
                {
                    var subArea = ctx.tbl5s_SubArea.FirstOrDefault(x => x.registroActivo && x.id == id);
                    if (subArea != null)
                    {
                        if (!ctx.tbl5s_Inspeccion.Any(x => x.registroActivo && x.subAreaId == subArea.id))
                        {
                            subArea.registroActivo = false;
                            subArea.fechaModificacion = DateTime.Now;
                            subArea.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                            ctx.SaveChanges();

                            resultado.Add(SUCCESS, true);
                        }
                        else
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "No se puede eliminar el sub area porque hay inspecciones con el subarea registradas");
                        }
                    }
                    else
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se encontro informacion del subarea");
                    }
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarSubArea(string nombre)
        {
            using (var ctx = new MainContext())
            {
                try
                {
                    var subArea = new tbl5s_SubArea();
                    subArea.descripcion = nombre;
                    subArea.fechaCreacion = DateTime.Now;
                    subArea.registroActivo = true;
                    subArea.usuarioCreacionId = vSesiones.sesionUsuarioDTO.id;
                    ctx.tbl5s_SubArea.Add(subArea);
                    ctx.SaveChanges();

                    resultado.Add(SUCCESS, true);
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public bool AccesoPermitido(PrivilegioEnum privilegio)
        {
            var acceso = false;
            using (var ctx = new MainContext())
            {
                try
                {
                    var usuario = ctx.tbl5s_Usuario.FirstOrDefault(x => x.usuarioId == vSesiones.sesionUsuarioDTO.id && x.registroActivo);
                    if (usuario != null && usuario.privilegioId.HasValue && usuario.privilegioId.Value == (int)privilegio)
                    {
                        acceso = true;
                    }
                }
                catch (Exception ex)
                {
                    acceso = false;
                }
            }

            return acceso;
        }
        #endregion

        #region Seguimiento PlanAccion

        public Dictionary<string, object> llenarTablaPlanAccion(AuditoriaDTO objParamDTO,DateTime fechaInicio,DateTime fechaFinal)
        {
            var resultado = new Dictionary<string, object>();
            using (var ctx = new MainContext())
            {
                try
                {
                    var fechaActual = DateTime.Now;

                    var auditoriasDet = ctx.tbl5s_AuditoriasDet
                        .Where(x =>
                            x.registroActivo &&
                            x.auditoria.cc == objParamDTO.cc &&
                            x.respuesta == (int)RespuestaEnum.NO_OK &&
                            x.auditoria.registroActivo &&
                            x.auditoria.checkList.registroActivo &&
                            x.auditoria.checkList.areaId == objParamDTO.areaId &&
                            x.auditoria.auditoriaCompleta).ToList();

                    var usuariosId = auditoriasDet.Select(x => x.usuario5sId).ToList();
                    var lideres = ctx.tbl5s_Usuario.Where(x => x.registroActivo && usuariosId.Contains(x.usuarioId)).ToList();

                    var datos = new List<SeguimientoPlanAccionDTO>();
                    var contador = 1;
                    foreach (var item in auditoriasDet)
                    {
                        var lider = lideres.FirstOrDefault(x => x.usuarioId == item.usuario5sId);
                        var inspeccion = item.auditoria.checkList.inspecciones.FirstOrDefault(x => x.id == item.inspeccionId);

                        var info = new SeguimientoPlanAccionDTO();
                        info.id = contador;
                        info.deteccion = inspeccion.inspeccion;
                        info.descripcion = item.descripcion;
                        info.medida = item.accion;
                        info.fechaCompromiso = item.fecha.Value;
                        info.tiempoTranscurrido = (fechaActual - item.fecha.Value).Days;
                        info.lider = lider != null ? PersonalUtilities.NombreCompletoMayusculas(lider.usuario.nombre, lider.usuario.apellidoPaterno, lider.usuario.apellidoMaterno) : "";
                        datos.Add(info);
                        contador++;
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, datos);
                }
                catch (Exception ex)
                {
                    resultado.Add(MESSAGE, ex.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }
		#endregion
        
		#region REPORTES

        #region ESTADISTICAS

        public Dictionary<string, object> GetEstadisticasTendencias(List<string> CCs, List<int> areas, DateTime fechaInicio, DateTime fechaFin)
        {
            using (var ctx = new MainContext())
            {
                try
                {
                    List<TablaTendenciasEstadisticasDTO> tablaTendencias = new List<TablaTendenciasEstadisticasDTO>();
                    TablaTendenciasEstadisticasDTO auxTablaTotales = new TablaTendenciasEstadisticasDTO();
                    decimal porcentajeClasificar = 0;
                    decimal porcentajeLimpieza = 0;
                    decimal porcentajeOrden = 0;
                    decimal porcentajeEstandarizar = 0;
                    decimal porcentajeDisciplina = 0;

                    List<TablaAccionesSeguimiento> tablaProduccion = new List<TablaAccionesSeguimiento>(12);
                    tablaProduccion.Add(new TablaAccionesSeguimiento { mes = "Enero", pendientes = 0, concluidas = 0, total = 0 });
                    tablaProduccion.Add(new TablaAccionesSeguimiento { mes = "Febrero", pendientes = 0, concluidas = 0, total = 0 });
                    tablaProduccion.Add(new TablaAccionesSeguimiento { mes = "Marzo", pendientes = 0, concluidas = 0, total = 0 });
                    tablaProduccion.Add(new TablaAccionesSeguimiento { mes = "Abril", pendientes = 0, concluidas = 0, total = 0 });
                    tablaProduccion.Add(new TablaAccionesSeguimiento { mes = "Mayo", pendientes = 0, concluidas = 0, total = 0 });
                    tablaProduccion.Add(new TablaAccionesSeguimiento { mes = "Junio", pendientes = 0, concluidas = 0, total = 0 });
                    tablaProduccion.Add(new TablaAccionesSeguimiento { mes = "Julio", pendientes = 0, concluidas = 0, total = 0 });
                    tablaProduccion.Add(new TablaAccionesSeguimiento { mes = "Agosto", pendientes = 0, concluidas = 0, total = 0 });
                    tablaProduccion.Add(new TablaAccionesSeguimiento { mes = "Septiembre", pendientes = 0, concluidas = 0, total = 0 });
                    tablaProduccion.Add(new TablaAccionesSeguimiento { mes = "Octubre", pendientes = 0, concluidas = 0, total = 0 });
                    tablaProduccion.Add(new TablaAccionesSeguimiento { mes = "Noviembre", pendientes = 0, concluidas = 0, total = 0 });
                    tablaProduccion.Add(new TablaAccionesSeguimiento { mes = "Diciembre", pendientes = 0, concluidas = 0, total = 0 });
                    List<TablaAccionesSeguimiento> tablaMantenimiento = new List<TablaAccionesSeguimiento>(12);
                    tablaMantenimiento.Add(new TablaAccionesSeguimiento { mes = "Enero", pendientes = 0, concluidas = 0, total = 0 });
                    tablaMantenimiento.Add(new TablaAccionesSeguimiento { mes = "Febrero", pendientes = 0, concluidas = 0, total = 0 });
                    tablaMantenimiento.Add(new TablaAccionesSeguimiento { mes = "Marzo", pendientes = 0, concluidas = 0, total = 0 });
                    tablaMantenimiento.Add(new TablaAccionesSeguimiento { mes = "Abril", pendientes = 0, concluidas = 0, total = 0 });
                    tablaMantenimiento.Add(new TablaAccionesSeguimiento { mes = "Mayo", pendientes = 0, concluidas = 0, total = 0 });
                    tablaMantenimiento.Add(new TablaAccionesSeguimiento { mes = "Junio", pendientes = 0, concluidas = 0, total = 0 });
                    tablaMantenimiento.Add(new TablaAccionesSeguimiento { mes = "Julio", pendientes = 0, concluidas = 0, total = 0 });
                    tablaMantenimiento.Add(new TablaAccionesSeguimiento { mes = "Agosto", pendientes = 0, concluidas = 0, total = 0 });
                    tablaMantenimiento.Add(new TablaAccionesSeguimiento { mes = "Septiembre", pendientes = 0, concluidas = 0, total = 0 });
                    tablaMantenimiento.Add(new TablaAccionesSeguimiento { mes = "Octubre", pendientes = 0, concluidas = 0, total = 0 });
                    tablaMantenimiento.Add(new TablaAccionesSeguimiento { mes = "Noviembre", pendientes = 0, concluidas = 0, total = 0 });
                    tablaMantenimiento.Add(new TablaAccionesSeguimiento { mes = "Diciembre", pendientes = 0, concluidas = 0, total = 0 });
                    List<TablaAccionesSeguimiento> tablaOverhaul = new List<TablaAccionesSeguimiento>(12);
                    tablaOverhaul.Add(new TablaAccionesSeguimiento { mes = "Enero", pendientes = 0, concluidas = 0, total = 0 });
                    tablaOverhaul.Add(new TablaAccionesSeguimiento { mes = "Febrero", pendientes = 0, concluidas = 0, total = 0 });
                    tablaOverhaul.Add(new TablaAccionesSeguimiento { mes = "Marzo", pendientes = 0, concluidas = 0, total = 0 });
                    tablaOverhaul.Add(new TablaAccionesSeguimiento { mes = "Abril", pendientes = 0, concluidas = 0, total = 0 });
                    tablaOverhaul.Add(new TablaAccionesSeguimiento { mes = "Mayo", pendientes = 0, concluidas = 0, total = 0 });
                    tablaOverhaul.Add(new TablaAccionesSeguimiento { mes = "Junio", pendientes = 0, concluidas = 0, total = 0 });
                    tablaOverhaul.Add(new TablaAccionesSeguimiento { mes = "Julio", pendientes = 0, concluidas = 0, total = 0 });
                    tablaOverhaul.Add(new TablaAccionesSeguimiento { mes = "Agosto", pendientes = 0, concluidas = 0, total = 0 });
                    tablaOverhaul.Add(new TablaAccionesSeguimiento { mes = "Septiembre", pendientes = 0, concluidas = 0, total = 0 });
                    tablaOverhaul.Add(new TablaAccionesSeguimiento { mes = "Octubre", pendientes = 0, concluidas = 0, total = 0 });
                    tablaOverhaul.Add(new TablaAccionesSeguimiento { mes = "Noviembre", pendientes = 0, concluidas = 0, total = 0 });
                    tablaOverhaul.Add(new TablaAccionesSeguimiento { mes = "Diciembre", pendientes = 0, concluidas = 0, total = 0 });
                    List<TablaAccionesSeguimiento> tablaAlmacen = new List<TablaAccionesSeguimiento>(12);
                    tablaAlmacen.Add(new TablaAccionesSeguimiento { mes = "Enero", pendientes = 0, concluidas = 0, total = 0 });
                    tablaAlmacen.Add(new TablaAccionesSeguimiento { mes = "Febrero", pendientes = 0, concluidas = 0, total = 0 });
                    tablaAlmacen.Add(new TablaAccionesSeguimiento { mes = "Marzo", pendientes = 0, concluidas = 0, total = 0 });
                    tablaAlmacen.Add(new TablaAccionesSeguimiento { mes = "Abril", pendientes = 0, concluidas = 0, total = 0 });
                    tablaAlmacen.Add(new TablaAccionesSeguimiento { mes = "Mayo", pendientes = 0, concluidas = 0, total = 0 });
                    tablaAlmacen.Add(new TablaAccionesSeguimiento { mes = "Junio", pendientes = 0, concluidas = 0, total = 0 });
                    tablaAlmacen.Add(new TablaAccionesSeguimiento { mes = "Julio", pendientes = 0, concluidas = 0, total = 0 });
                    tablaAlmacen.Add(new TablaAccionesSeguimiento { mes = "Agosto", pendientes = 0, concluidas = 0, total = 0 });
                    tablaAlmacen.Add(new TablaAccionesSeguimiento { mes = "Septiembre", pendientes = 0, concluidas = 0, total = 0 });
                    tablaAlmacen.Add(new TablaAccionesSeguimiento { mes = "Octubre", pendientes = 0, concluidas = 0, total = 0 });
                    tablaAlmacen.Add(new TablaAccionesSeguimiento { mes = "Noviembre", pendientes = 0, concluidas = 0, total = 0 });
                    tablaAlmacen.Add(new TablaAccionesSeguimiento { mes = "Diciembre", pendientes = 0, concluidas = 0, total = 0 });
                    List<TablaAccionesSeguimiento> tablaAdministrativas = new List<TablaAccionesSeguimiento>(12);
                    tablaAdministrativas.Add(new TablaAccionesSeguimiento { mes = "Enero", pendientes = 0, concluidas = 0, total = 0 });
                    tablaAdministrativas.Add(new TablaAccionesSeguimiento { mes = "Febrero", pendientes = 0, concluidas = 0, total = 0 });
                    tablaAdministrativas.Add(new TablaAccionesSeguimiento { mes = "Marzo", pendientes = 0, concluidas = 0, total = 0 });
                    tablaAdministrativas.Add(new TablaAccionesSeguimiento { mes = "Abril", pendientes = 0, concluidas = 0, total = 0 });
                    tablaAdministrativas.Add(new TablaAccionesSeguimiento { mes = "Mayo", pendientes = 0, concluidas = 0, total = 0 });
                    tablaAdministrativas.Add(new TablaAccionesSeguimiento { mes = "Junio", pendientes = 0, concluidas = 0, total = 0 });
                    tablaAdministrativas.Add(new TablaAccionesSeguimiento { mes = "Julio", pendientes = 0, concluidas = 0, total = 0 });
                    tablaAdministrativas.Add(new TablaAccionesSeguimiento { mes = "Agosto", pendientes = 0, concluidas = 0, total = 0 });
                    tablaAdministrativas.Add(new TablaAccionesSeguimiento { mes = "Septiembre", pendientes = 0, concluidas = 0, total = 0 });
                    tablaAdministrativas.Add(new TablaAccionesSeguimiento { mes = "Octubre", pendientes = 0, concluidas = 0, total = 0 });
                    tablaAdministrativas.Add(new TablaAccionesSeguimiento { mes = "Noviembre", pendientes = 0, concluidas = 0, total = 0 });
                    tablaAdministrativas.Add(new TablaAccionesSeguimiento { mes = "Diciembre", pendientes = 0, concluidas = 0, total = 0 });


                    auxTablaTotales.cincoSDescripcion = "Puntos totales mes";

                    List<decimal> totalObservaciones = new List<decimal>(5) { 0, 0, 0, 0, 0 };
                    decimal totalEnRevision = 0;
                    decimal totalEnProceso = 0;
                    decimal totalConcluidas = 0;

                    var cincoS = ctx.tbl5s_5s.Where(x => x.registroActivo).ToList();
                    
                    var checkListsIdCC = ctx.tbl5s_CC.Where(x => (CCs.Count() > 0 ? CCs.Contains(x.cc) : true) && x.registroActivo).Select(x => x.checkList).Where(x => (areas.Count() > 0 ? areas.Contains(x.areaId) : true) && x.registroActivo).Select(x => x.id).ToList();
                    var auditorias = ctx.tbl5s_Auditorias.Where(x => checkListsIdCC.Contains(x.checkListId) && x.registroActivo && x.fecha >= fechaInicio && x.fecha <= fechaFin && x.auditoriaCompleta).ToList();
                    var auditoriasIDs = auditorias.Select(x => x.id).ToList();

                    //--> Auditorias Cerradas con archivo
                    List<AuditoriaDetDTO> auditoriaDet = ctx.tbl5s_AuditoriasDet.Where(x => auditoriasIDs.Contains(x.auditoriaId) && x.registroActivo && x.respuesta == 1).Select(x => new AuditoriaDetDTO()
                        {
                            id = x.id,
                            auditoriaId = x.auditoriaId,
                            inspeccionId = x.inspeccionId,
                            descripcion = x.descripcion,
                            respuesta = x.respuesta,
                            accion = x.accion,
                            usuario5sId = x.usuario5sId,
                            fecha = x.fecha,
                            fechaCreacion = x.fechaCreacion,
                            fechaModificacion = x.fechaModificacion ?? new DateTime(),
                            usuarioCreacionId = x.usuarioCreacionId,
                            usuarioModificacionId = x.usuarioModificacionId,
                            registroActivo = x.registroActivo,
                        
                    }).ToList();

                    var auditoriasDetIDs = auditoriaDet.Select(x => x.id).ToList();
                    var archivosAuditoria = ctx.tbl5s_AuditoriasArchivos.Where(x => auditoriasDetIDs.Contains(x.auditoriaDetId) && x.tipoArchivo == 2 && x.registroActivo).ToList();

                    foreach (var itemAuditoria in auditoriaDet) 
                    {
                        var archivo = archivosAuditoria.FirstOrDefault(x => x.auditoriaDetId == itemAuditoria.id);
                        if (archivo != null) 
                        {
                            itemAuditoria.aprobado = archivo.aprobado;
                            itemAuditoria.idArchivo = archivo.id;
                        }
                    }

                    //auditoriaDet = auditoriaDet.Where(x => x.aprobado == 1).ToList();
                    //<--
 
                    //--> Auditorias con respuesta 2
                    List<AuditoriaDetDTO> auditoriaProcesoDet = ctx.tbl5s_AuditoriasDet.Where(x => auditoriasIDs.Contains(x.auditoriaId) && x.registroActivo && x.respuesta == 2).Select(x => new AuditoriaDetDTO()
                    {
                        id = x.id,
                        auditoriaId = x.auditoriaId,
                        inspeccionId = x.inspeccionId,
                        descripcion = x.descripcion,
                        respuesta = x.respuesta,
                        accion = x.accion,
                        usuario5sId = x.usuario5sId,
                        fecha = x.fecha,
                        fechaCreacion = x.fechaCreacion,
                        fechaModificacion = x.fechaModificacion ?? new DateTime(),
                        usuarioCreacionId = x.usuarioCreacionId,
                        usuarioModificacionId = x.usuarioModificacionId,
                        registroActivo = x.registroActivo,                        
                    }).ToList();

                    var auditoriasProcesoDetIDs = auditoriaProcesoDet.Select(x => x.id).ToList();
                    var archivosAuditoriaProceso = ctx.tbl5s_AuditoriasArchivos.Where(x => auditoriasDetIDs.Contains(x.auditoriaDetId) && x.tipoArchivo == 2 && x.registroActivo).ToList();

                    foreach (var itemAuditoria in auditoriaProcesoDet)
                    {
                        var archivo = archivosAuditoriaProceso.FirstOrDefault(x => x.auditoriaDetId == itemAuditoria.id);
                        if (archivo != null)
                        {
                            itemAuditoria.aprobado = archivo.aprobado;
                            itemAuditoria.idArchivo = archivo.id;
                        }
                    }
                    auditoriaDet.AddRange(auditoriaProcesoDet);
                    //<--


                    var inspeccionesAuditoriaIDs = auditoriaDet.Select(x => x.inspeccionId).ToList();
                    var inspecciones = ctx.tbl5s_Inspeccion.Where(x => inspeccionesAuditoriaIDs.Contains(x.id)).SelectMany(x => x.detalles).ToList();

                    foreach(var itemCincoS in cincoS)
                    {
                        TablaTendenciasEstadisticasDTO auxTabla = new TablaTendenciasEstadisticasDTO();
                        auxTabla.cincoSDescripcion = itemCincoS.descripcion;
                        var _inspecciones = inspecciones.Where(x => x.cincoId == itemCincoS.id).ToList();

                        foreach(var item_inspecciones in _inspecciones)
                        {
                            var auditoriaInspeccion = auditoriaDet.FirstOrDefault(x => x.inspeccionId == item_inspecciones.inspeccionId);
                            var auditoriaPrincipal = auditorias.FirstOrDefault(x => x.id == auditoriaInspeccion.auditoriaId);
                            var mes = auditoriaPrincipal == null ? 0 : auditoriaPrincipal.fecha.Month;

                            if (auditoriaInspeccion.respuesta == 2 || auditoriaInspeccion.aprobado == 1)
                            {
                                switch (item_inspecciones.cincoId)
                                {
                                    case 1: 
                                        porcentajeClasificar++;
                                        totalObservaciones[0]++;
                                        break;
                                    case 2: 
                                        porcentajeLimpieza++;
                                        totalObservaciones[1]++;
                                        break;
                                    case 3: 
                                        porcentajeOrden++;
                                        totalObservaciones[2]++;
                                        break;
                                    case 4: 
                                        porcentajeEstandarizar++;
                                        totalObservaciones[3]++;
                                        break;
                                    case 5: 
                                        porcentajeDisciplina++;
                                        totalObservaciones[4]++;
                                        break;
                                }
                            }

                            switch(mes)
                            {
                                case 1:
                                    //if (auditoriaInspeccion.respuesta == 2 || auditoriaInspeccion.aprobado == 1)
                                    //{
                                        switch (item_inspecciones.inspeccion.checkList.areaId)
                                        {
                                            case 1:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaProduccion[0].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaProduccion[0].concluidas++; }
                                                tablaProduccion[0].total++;
                                                break;
                                            case 2:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaMantenimiento[0].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaMantenimiento[0].concluidas++; }
                                                tablaMantenimiento[0].total++;
                                                break;
                                            case 3:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaOverhaul[0].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaOverhaul[0].concluidas++; }
                                                tablaOverhaul[0].total++;
                                                break;
                                            case 4:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaAlmacen[0].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaAlmacen[0].concluidas++; }
                                                tablaAlmacen[0].total++;
                                                break;
                                            case 5:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaAdministrativas[0].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaAdministrativas[0].concluidas++; }
                                                tablaAdministrativas[0].total++;
                                                break;
                                        }
                                        if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { totalEnProceso++; }
                                        if (auditoriaInspeccion.respuesta == 1) { totalConcluidas++; }
                                        if (auditoriaInspeccion.respuesta == 2 && auditoriaInspeccion.idArchivo >= 1) { totalEnProceso++; totalEnRevision++; }
                                        //totalObservaciones++;
                                    //}
                                    if (auditoriaInspeccion.respuesta == 1) { auxTabla.enero++; }
                                    auxTablaTotales.enero++;
                                    auxTablaTotales.enero++;                                    
                                    break;
                                case 2:
                                    //if (auditoriaInspeccion.respuesta == 2 || auditoriaInspeccion.aprobado == 1)
                                    //{
                                        switch (item_inspecciones.inspeccion.checkList.areaId)
                                        {
                                            case 1:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaProduccion[1].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaProduccion[1].concluidas++; }
                                                tablaProduccion[1].total++;
                                                break;
                                            case 2:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaMantenimiento[1].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaMantenimiento[1].concluidas++; }
                                                tablaMantenimiento[1].total++;
                                                break;
                                            case 3:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaOverhaul[1].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaOverhaul[1].concluidas++; }
                                                tablaOverhaul[1].total++;
                                                break;
                                            case 4:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaAlmacen[1].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaAlmacen[1].concluidas++; }
                                                tablaAlmacen[1].total++;
                                                break;
                                            case 5:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaAdministrativas[1].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaAdministrativas[1].concluidas++; }
                                                tablaAdministrativas[1].total++;
                                                break;
                                        }
                                        if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { totalEnProceso++; }
                                        if (auditoriaInspeccion.respuesta == 1) { totalConcluidas++; }
                                        if (auditoriaInspeccion.respuesta == 2 && auditoriaInspeccion.idArchivo >= 1) { totalEnProceso++; totalEnRevision++; }
                                        //totalObservaciones++;
                                    //}
                                    if (auditoriaInspeccion.respuesta == 1) { auxTabla.febrero++; }
                                    auxTablaTotales.febrero++;
                                    break;
                                case 3:
                                    //if (auditoriaInspeccion.respuesta == 2 || auditoriaInspeccion.aprobado == 1)
                                    //{
                                        switch (item_inspecciones.inspeccion.checkList.areaId)
                                        {
                                            case 1:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaProduccion[2].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaProduccion[2].concluidas++; }
                                                tablaProduccion[2].total++;
                                                break;
                                            case 2:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaMantenimiento[2].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaMantenimiento[2].concluidas++; }
                                                tablaMantenimiento[2].total++;
                                                break;
                                            case 3:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaOverhaul[2].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaOverhaul[2].concluidas++; }
                                                tablaOverhaul[2].total++;
                                                break;
                                            case 4:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaAlmacen[2].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaAlmacen[2].concluidas++; }
                                                tablaAlmacen[2].total++;
                                                break;
                                            case 5:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaAdministrativas[2].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaAdministrativas[2].concluidas++; }
                                                tablaAdministrativas[2].total++;
                                                break;
                                        }
                                        if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { totalEnProceso++; }
                                        if (auditoriaInspeccion.respuesta == 1) { totalConcluidas++; }
                                        if (auditoriaInspeccion.respuesta == 2 && auditoriaInspeccion.idArchivo >= 1) { totalEnProceso++; totalEnRevision++; }
                                        //totalObservaciones++;
                                    //}
                                    if (auditoriaInspeccion.respuesta == 1) { auxTabla.marzo++; }
                                    auxTablaTotales.marzo++;
                                    break;
                                case 4:
                                    //if (auditoriaInspeccion.respuesta == 2 || auditoriaInspeccion.aprobado == 1)
                                    //{
                                        switch (item_inspecciones.inspeccion.checkList.areaId)
                                        {
                                            case 1:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaProduccion[3].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaProduccion[3].concluidas++; }
                                                tablaProduccion[3].total++;
                                                break;
                                            case 2:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaMantenimiento[3].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaMantenimiento[3].concluidas++; }
                                                tablaMantenimiento[3].total++;
                                                break;
                                            case 3:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaOverhaul[3].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaOverhaul[3].concluidas++; }
                                                tablaOverhaul[3].total++;
                                                break;
                                            case 4:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaAlmacen[3].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaAlmacen[3].concluidas++; }
                                                tablaAlmacen[3].total++;
                                                break;
                                            case 5:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaAdministrativas[3].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaAdministrativas[3].concluidas++; }
                                                tablaAdministrativas[3].total++;
                                                break;
                                        }
                                        if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { totalEnProceso++; }
                                        if (auditoriaInspeccion.respuesta == 1) { totalConcluidas++; }
                                        if (auditoriaInspeccion.respuesta == 2 && auditoriaInspeccion.idArchivo >= 1) { totalEnProceso++; totalEnRevision++; }
                                        //totalObservaciones++;
                                    //}
                                    if (auditoriaInspeccion.respuesta == 1) { auxTabla.abril++; }
                                    auxTablaTotales.abril++;
                                    break;
                                case 5:
                                    //if (auditoriaInspeccion.respuesta == 2 || auditoriaInspeccion.aprobado == 1)
                                    //{
                                        switch (item_inspecciones.inspeccion.checkList.areaId)
                                        {
                                            case 1:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaProduccion[4].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaProduccion[4].concluidas++; }
                                                tablaProduccion[4].total++;
                                                break;
                                            case 2:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaMantenimiento[4].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaMantenimiento[4].concluidas++; }
                                                tablaMantenimiento[4].total++;
                                                break;
                                            case 3:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaOverhaul[4].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaOverhaul[4].concluidas++; }
                                                tablaOverhaul[4].total++;
                                                break;
                                            case 4:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaAlmacen[4].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaAlmacen[4].concluidas++; }
                                                tablaAlmacen[4].total++;
                                                break;
                                            case 5:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaAdministrativas[4].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaAdministrativas[4].concluidas++; }
                                                tablaAdministrativas[4].total++;
                                                break;
                                        }
                                        if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { totalEnProceso++; }
                                        if (auditoriaInspeccion.respuesta == 1) { totalConcluidas++; }
                                        if (auditoriaInspeccion.respuesta == 2 && auditoriaInspeccion.idArchivo >= 1) { totalEnProceso++; totalEnRevision++; }
                                        //totalObservaciones++;
                                    //}
                                    if (auditoriaInspeccion.respuesta == 1) { auxTabla.mayo++; }
                                    auxTablaTotales.mayo++;
                                    break;
                                case 6:
                                    //if (auditoriaInspeccion.respuesta == 2 || auditoriaInspeccion.aprobado == 1)
                                    //{
                                        switch (item_inspecciones.inspeccion.checkList.areaId)
                                        {
                                            case 1:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaProduccion[5].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaProduccion[5].concluidas++; }
                                                tablaProduccion[5].total++;
                                                break;
                                            case 2:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaMantenimiento[5].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaMantenimiento[5].concluidas++; }
                                                tablaMantenimiento[5].total++;
                                                break;
                                            case 3:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaOverhaul[5].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaOverhaul[5].concluidas++; }
                                                tablaOverhaul[5].total++;
                                                break;
                                            case 4:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaAlmacen[5].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaAlmacen[5].concluidas++; }
                                                tablaAlmacen[5].total++;
                                                break;
                                            case 5:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaAdministrativas[5].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaAdministrativas[5].concluidas++; }
                                                tablaAdministrativas[5].total++;
                                                break;
                                        }
                                        if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { totalEnProceso++; }
                                        if (auditoriaInspeccion.respuesta == 1) { totalConcluidas++; }
                                        if (auditoriaInspeccion.respuesta == 2 && auditoriaInspeccion.idArchivo >= 1) { totalEnProceso++; totalEnRevision++; }
                                        //totalObservaciones++;
                                    //}
                                    if (auditoriaInspeccion.respuesta == 1) { auxTabla.junio++; }
                                    auxTablaTotales.junio++;
                                    break;
                                case 7:
                                    //if (auditoriaInspeccion.respuesta == 2 || auditoriaInspeccion.aprobado == 1)
                                   // {
                                        switch (item_inspecciones.inspeccion.checkList.areaId)
                                        {
                                            case 1:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaProduccion[6].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaProduccion[6].concluidas++; }
                                                tablaProduccion[6].total++;
                                                break;
                                            case 2:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaMantenimiento[6].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaMantenimiento[6].concluidas++; }
                                                tablaMantenimiento[6].total++;
                                                break;
                                            case 3:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaOverhaul[6].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaOverhaul[6].concluidas++; }
                                                tablaOverhaul[6].total++;
                                                break;
                                            case 4:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaAlmacen[6].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaAlmacen[6].concluidas++; }
                                                tablaAlmacen[6].total++;
                                                break;
                                            case 5:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaAdministrativas[6].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaAdministrativas[6].concluidas++; }
                                                tablaAdministrativas[6].total++;
                                                break;
                                        }
                                        if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { totalEnProceso++; }
                                        if (auditoriaInspeccion.respuesta == 1) { totalConcluidas++; }
                                        if (auditoriaInspeccion.respuesta == 2 && auditoriaInspeccion.idArchivo >= 1) { totalEnProceso++; totalEnRevision++; }
                                        //totalObservaciones++;
                                    //}
                                    if (auditoriaInspeccion.respuesta == 1) { auxTabla.julio++; }
                                    auxTablaTotales.julio++;
                                    break;
                                case 8:
                                    //if (auditoriaInspeccion.respuesta == 2 || auditoriaInspeccion.aprobado == 1)
                                    //{
                                        switch (item_inspecciones.inspeccion.checkList.areaId)
                                        {
                                            case 1:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaProduccion[7].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaProduccion[7].concluidas++; }
                                                tablaProduccion[7].total++;
                                                break;
                                            case 2:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaMantenimiento[7].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaMantenimiento[7].concluidas++; }
                                                tablaMantenimiento[7].total++;
                                                break;
                                            case 3:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaOverhaul[7].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaOverhaul[7].concluidas++; }
                                                tablaOverhaul[7].total++;
                                                break;
                                            case 4:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaAlmacen[7].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaAlmacen[7].concluidas++; }
                                                tablaAlmacen[7].total++;
                                                break;
                                            case 5:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaAdministrativas[7].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaAdministrativas[7].concluidas++; }
                                                tablaAdministrativas[7].total++;
                                                break;
                                        }
                                        if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { totalEnProceso++; }
                                        if (auditoriaInspeccion.respuesta == 1) { totalConcluidas++; }
                                        if (auditoriaInspeccion.respuesta == 2 && auditoriaInspeccion.idArchivo >= 1) { totalEnProceso++; totalEnRevision++; }
                                        //totalObservaciones++;
                                    //}
                                    if (auditoriaInspeccion.respuesta == 1) { auxTabla.agosto++; }
                                    auxTablaTotales.agosto++;
                                    break;
                                case 9:
                                    //if (auditoriaInspeccion.respuesta == 2 || auditoriaInspeccion.aprobado == 1)
                                    //{
                                        switch (item_inspecciones.inspeccion.checkList.areaId)
                                        {
                                            case 1:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaProduccion[8].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaProduccion[8].concluidas++; }
                                                tablaProduccion[8].total++;
                                                break;
                                            case 2:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaMantenimiento[8].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaMantenimiento[8].concluidas++; }
                                                tablaMantenimiento[8].total++;
                                                break;
                                            case 3:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaOverhaul[8].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaOverhaul[8].concluidas++; }
                                                tablaOverhaul[8].total++;
                                                break;
                                            case 4:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaAlmacen[8].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaAlmacen[8].concluidas++; }
                                                tablaAlmacen[8].total++;
                                                break;
                                            case 5:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaAdministrativas[8].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaAdministrativas[8].concluidas++; }
                                                tablaAdministrativas[8].total++;
                                                break;
                                        }
                                        if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { totalEnProceso++; }
                                        if (auditoriaInspeccion.respuesta == 1) { totalConcluidas++; }
                                        if (auditoriaInspeccion.respuesta == 2 && auditoriaInspeccion.idArchivo >= 1) { totalEnProceso++; totalEnRevision++; }
                                        //totalObservaciones++;
                                    //}
                                    if (auditoriaInspeccion.respuesta == 1) { auxTabla.septiembre++; }
                                    auxTablaTotales.septiembre++;
                                    break;
                                case 10:
                                    //if (auditoriaInspeccion.respuesta == 2 || auditoriaInspeccion.aprobado == 1)
                                    //{
                                        switch (item_inspecciones.inspeccion.checkList.areaId)
                                        {
                                            case 1:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaProduccion[9].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaProduccion[9].concluidas++; }
                                                tablaProduccion[9].total++;
                                                break;
                                            case 2:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaMantenimiento[9].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaMantenimiento[9].concluidas++; }
                                                tablaMantenimiento[9].total++;
                                                break;
                                            case 3:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaOverhaul[9].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaOverhaul[9].concluidas++; }
                                                tablaOverhaul[9].total++;
                                                break;
                                            case 4:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaAlmacen[9].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaAlmacen[9].concluidas++; }
                                                tablaAlmacen[9].total++;
                                                break;
                                            case 5:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaAdministrativas[9].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaAdministrativas[9].concluidas++; }
                                                tablaAdministrativas[9].total++;
                                                break;
                                        }
                                        if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { totalEnProceso++; }
                                        if (auditoriaInspeccion.respuesta == 1) { totalConcluidas++; }
                                        if (auditoriaInspeccion.respuesta == 2 && auditoriaInspeccion.idArchivo >= 1) { totalEnProceso++; totalEnRevision++; }
                                        //totalObservaciones++;
                                    //}
                                    if (auditoriaInspeccion.respuesta == 1) { auxTabla.octubre++; }
                                    auxTablaTotales.octubre++;
                                    break;
                                case 11:
                                    //if (auditoriaInspeccion.respuesta == 2 || auditoriaInspeccion.aprobado == 1)
                                    //{
                                        switch (item_inspecciones.inspeccion.checkList.areaId)
                                        {
                                            case 1:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaProduccion[10].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaProduccion[10].concluidas++; }
                                                tablaProduccion[10].total++;
                                                break;
                                            case 2:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaMantenimiento[10].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaMantenimiento[10].concluidas++; }
                                                tablaMantenimiento[10].total++;
                                                break;
                                            case 3:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaOverhaul[10].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaOverhaul[10].concluidas++; }
                                                tablaOverhaul[10].total++;
                                                break;
                                            case 4:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaAlmacen[10].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaAlmacen[10].concluidas++; }
                                                tablaAlmacen[10].total++;
                                                break;
                                            case 5:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaAdministrativas[10].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaAdministrativas[10].concluidas++; }
                                                tablaAdministrativas[10].total++;
                                                break;
                                        }
                                        if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { totalEnProceso++; }
                                        if (auditoriaInspeccion.respuesta == 1) { totalConcluidas++; }
                                        if (auditoriaInspeccion.respuesta == 2 && auditoriaInspeccion.idArchivo >= 1) { totalEnProceso++; totalEnRevision++; }
                                        //totalObservaciones++;
                                    //}
                                    if (auditoriaInspeccion.respuesta == 1) { auxTabla.noviembre++; }
                                    auxTablaTotales.noviembre++;
                                    break;
                                case 12:
                                    //if (auditoriaInspeccion.respuesta == 2 || auditoriaInspeccion.aprobado == 1)
                                    //{
                                        switch (item_inspecciones.inspeccion.checkList.areaId)
                                        {
                                            case 1:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaProduccion[11].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaProduccion[11].concluidas++; }
                                                tablaProduccion[11].total++;
                                                break;
                                            case 2:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaMantenimiento[11].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaMantenimiento[11].concluidas++; }
                                                tablaMantenimiento[11].total++;
                                                break;
                                            case 3:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaOverhaul[11].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaOverhaul[11].concluidas++; }
                                                tablaOverhaul[11].total++;
                                                break;
                                            case 4:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaAlmacen[11].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaAlmacen[11].concluidas++; }
                                                tablaAlmacen[11].total++;
                                                break;
                                            case 5:
                                                if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { tablaAdministrativas[11].pendientes++; }
                                                if (auditoriaInspeccion.respuesta == 1) { tablaAdministrativas[11].concluidas++; }
                                                tablaAdministrativas[11].total++;
                                                break;
                                        }
                                        if (auditoriaInspeccion.respuesta == 2 && (auditoriaInspeccion.idArchivo == null || auditoriaInspeccion.idArchivo < 1)) { totalEnProceso++; }
                                        if (auditoriaInspeccion.respuesta == 1) { totalConcluidas++; }
                                        if (auditoriaInspeccion.respuesta == 2 && auditoriaInspeccion.idArchivo >= 1) { totalEnRevision++; }
                                        //totalObservaciones++;
                                    //}
                                    if (auditoriaInspeccion.respuesta == 1) { auxTabla.diciembre++; }
                                    auxTablaTotales.diciembre++;
                                    break;
                            }   
                        }
                        tablaTendencias.Add(auxTabla);
                    }

                    var datosGraficaTendencias = new List<decimal>(12) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    var datosGraficaRadar = new List<decimal>(5) { 0, 0, 0, 0, 0 };

                    foreach (var item in tablaTendencias) 
                    {
                        datosGraficaTendencias[0] += item.enero;
                        datosGraficaTendencias[1] += item.febrero;
                        datosGraficaTendencias[2] += item.marzo;
                        datosGraficaTendencias[3] += item.abril;
                        datosGraficaTendencias[4] += item.mayo;
                        datosGraficaTendencias[5] += item.junio;
                        datosGraficaTendencias[6] += item.julio;
                        datosGraficaTendencias[7] += item.agosto;
                        datosGraficaTendencias[8] += item.septiembre;
                        datosGraficaTendencias[9] += item.octubre;
                        datosGraficaTendencias[10] += item.noviembre;
                        datosGraficaTendencias[11] += item.diciembre;
                    }

                    datosGraficaTendencias[0] = Math.Truncate((datosGraficaTendencias[0] / (auxTablaTotales.enero == 0 ? 1 : auxTablaTotales.enero) * 100) * 100) / 100;
                    datosGraficaTendencias[1] = Math.Truncate((datosGraficaTendencias[1] / (auxTablaTotales.febrero == 0 ? 1 : auxTablaTotales.febrero) * 100) * 100) / 100;
                    datosGraficaTendencias[2] = Math.Truncate((datosGraficaTendencias[2] / (auxTablaTotales.marzo == 0 ? 1 : auxTablaTotales.marzo) * 100) * 100) / 100;
                    datosGraficaTendencias[3] = Math.Truncate((datosGraficaTendencias[3] / (auxTablaTotales.abril == 0 ? 1 : auxTablaTotales.abril) * 100) *100) / 100;
                    datosGraficaTendencias[4] = Math.Truncate((datosGraficaTendencias[4] / (auxTablaTotales.mayo == 0 ? 1 : auxTablaTotales.mayo) * 100) * 100) / 100;
                    datosGraficaTendencias[5] = Math.Truncate((datosGraficaTendencias[5] / (auxTablaTotales.junio == 0 ? 1 : auxTablaTotales.junio) * 100) * 100) / 100;
                    datosGraficaTendencias[6] = Math.Truncate((datosGraficaTendencias[6] / (auxTablaTotales.julio == 0 ? 1 : auxTablaTotales.julio) * 100) * 100) / 100;
                    datosGraficaTendencias[7] = Math.Truncate((datosGraficaTendencias[7] / (auxTablaTotales.agosto == 0 ? 1 : auxTablaTotales.agosto) * 100) * 100) / 100;
                    datosGraficaTendencias[8] = Math.Truncate((datosGraficaTendencias[8] / (auxTablaTotales.septiembre == 0 ? 1 : auxTablaTotales.septiembre) * 100) * 100) / 100;
                    datosGraficaTendencias[9] = Math.Truncate((datosGraficaTendencias[9] / (auxTablaTotales.octubre == 0 ? 1 : auxTablaTotales.octubre) * 100) * 100) / 100;
                    datosGraficaTendencias[10] = Math.Truncate((datosGraficaTendencias[10] / (auxTablaTotales.noviembre == 0 ? 1 : auxTablaTotales.noviembre) * 100) * 100) / 100;
                    datosGraficaTendencias[11] = Math.Truncate((datosGraficaTendencias[11] / (auxTablaTotales.diciembre == 0 ? 1 : auxTablaTotales.diciembre) * 100) * 100) / 100;

                    tablaTendencias.Add(auxTablaTotales);

                    porcentajeClasificar = Math.Truncate((porcentajeClasificar / (totalObservaciones[0] == 0 ? 1 : totalObservaciones[0]) * 100) * 100) / 100;
                    porcentajeLimpieza = Math.Truncate((porcentajeLimpieza / (totalObservaciones[1] == 0 ? 1 : totalObservaciones[1]) * 100) * 100) / 100;
                    porcentajeOrden = Math.Truncate((porcentajeOrden / (totalObservaciones[2] == 0 ? 1 : totalObservaciones[2]) * 100) * 100) / 100;
                    porcentajeEstandarizar = Math.Truncate((porcentajeEstandarizar / (totalObservaciones[3] == 0 ? 1 : totalObservaciones[3]) * 100) * 100) / 100;
                    porcentajeDisciplina = Math.Truncate((porcentajeDisciplina / (totalObservaciones[4] == 0 ? 1 : totalObservaciones[4]) * 100) * 100) / 100;

                    datosGraficaRadar[0] = porcentajeClasificar;
                    datosGraficaRadar[1] = porcentajeLimpieza;
                    datosGraficaRadar[2] = porcentajeOrden;
                    datosGraficaRadar[3] = porcentajeEstandarizar;
                    datosGraficaRadar[4] = porcentajeDisciplina;

                    resultado.Add(SUCCESS, true);
                    resultado.Add("tablaTendencias", tablaTendencias);

                    resultado.Add("datosGraficaTendencias", datosGraficaTendencias);

                    resultado.Add("totalObservaciones", totalObservaciones.Sum(x => x));
                    resultado.Add("totalEnRevision", totalEnRevision);
                    resultado.Add("totalEnProceso", totalEnProceso);
                    resultado.Add("totalConcluidas", totalConcluidas);

                    resultado.Add("tablaProduccion", tablaProduccion);
                    resultado.Add("tablaMantenimiento", tablaMantenimiento);
                    resultado.Add("tablaOverhaul", tablaOverhaul);
                    resultado.Add("tablaAlmacen", tablaAlmacen);
                    resultado.Add("tablaAdministrativas", tablaAdministrativas);

                    resultado.Add("datosGraficaRadar", datosGraficaRadar);
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetReporteEjecutivo(List<string> CCs, List<int> areas, DateTime fechaInicio, DateTime fechaFin)
        {
            var ccFS = new CCFactoryService().getCCServiceSP();
            using (var ctx = new MainContext())
            {
                try
                {
                    List<TablaReporteEjecutivoDTO> tablaReporteEjecutivo = new List<TablaReporteEjecutivoDTO>();
                    List<decimal> porcentajesObras = new List<decimal>();
                    List<List<decimal>> datosPorMes = new List<List<decimal>>();
                    
                    var cincoS = ctx.tbl5s_5s.Where(x => x.registroActivo).ToList();

                    var checkListsIdCC = ctx.tbl5s_CC.Where(x => (CCs.Count() > 0 ? CCs.Contains(x.cc) : true) && x.registroActivo).Select(x => x.checkList).Where(x => (areas.Count() > 0 ? areas.Contains(x.areaId) : true) && x.registroActivo).Select(x => x.id).ToList();
                    var auditorias = ctx.tbl5s_Auditorias.Where(x => checkListsIdCC.Contains(x.checkListId) && x.registroActivo && x.fecha >= fechaInicio && x.fecha <= fechaFin && x.auditoriaCompleta).ToList();
                    var auditoriasIDs = auditorias.Select(x => x.id).ToList();
                    //--> Auditorias Cerradas con archivo
                    List<AuditoriaDetDTO> auditoriaDet = ctx.tbl5s_AuditoriasDet.Where(x => auditoriasIDs.Contains(x.auditoriaId) && x.registroActivo && x.respuesta == 1).Select(x => new AuditoriaDetDTO()
                    {
                        id = x.id,
                        auditoriaId = x.auditoriaId,
                        inspeccionId = x.inspeccionId,
                        descripcion = x.descripcion,
                        respuesta = x.respuesta,
                        accion = x.accion,
                        usuario5sId = x.usuario5sId,
                        fecha = x.fecha,
                        fechaCreacion = x.fechaCreacion,
                        fechaModificacion = x.fechaModificacion ?? new DateTime(),
                        usuarioCreacionId = x.usuarioCreacionId,
                        usuarioModificacionId = x.usuarioModificacionId,
                        registroActivo = x.registroActivo,

                    }).ToList();

                    var auditoriasDetIDs = auditoriaDet.Select(x => x.id).ToList();
                    var archivosAuditoria = ctx.tbl5s_AuditoriasArchivos.Where(x => auditoriasDetIDs.Contains(x.auditoriaDetId) && x.tipoArchivo == 2 && x.registroActivo).ToList();

                    foreach (var itemAuditoria in auditoriaDet)
                    {
                        var archivo = archivosAuditoria.FirstOrDefault(x => x.auditoriaDetId == itemAuditoria.id);
                        if (archivo != null)
                        {
                            itemAuditoria.aprobado = archivo.aprobado;
                            itemAuditoria.idArchivo = archivo.id;
                        }
                    }

                    //auditoriaDet = auditoriaDet.Where(x => x.aprobado == 1).ToList();
                    //<--

                    //--> Auditorias con respuesta 2
                    List<AuditoriaDetDTO> auditoriaProcesoDet = ctx.tbl5s_AuditoriasDet.Where(x => auditoriasIDs.Contains(x.auditoriaId) && x.registroActivo && x.respuesta == 2).Select(x => new AuditoriaDetDTO()
                    {
                        id = x.id,
                        auditoriaId = x.auditoriaId,
                        inspeccionId = x.inspeccionId,
                        descripcion = x.descripcion,
                        respuesta = x.respuesta,
                        accion = x.accion,
                        usuario5sId = x.usuario5sId,
                        fecha = x.fecha,
                        fechaCreacion = x.fechaCreacion,
                        fechaModificacion = x.fechaModificacion ?? new DateTime(),
                        usuarioCreacionId = x.usuarioCreacionId,
                        usuarioModificacionId = x.usuarioModificacionId,
                        registroActivo = x.registroActivo,
                    }).ToList();

                    var auditoriasProcesoDetIDs = auditoriaProcesoDet.Select(x => x.id).ToList();
                    var archivosAuditoriaProceso = ctx.tbl5s_AuditoriasArchivos.Where(x => auditoriasDetIDs.Contains(x.auditoriaDetId) && x.tipoArchivo == 2 && x.registroActivo).ToList();

                    foreach (var itemAuditoria in auditoriaProcesoDet)
                    {
                        var archivo = archivosAuditoriaProceso.FirstOrDefault(x => x.auditoriaDetId == itemAuditoria.id);
                        if (archivo != null)
                        {
                            itemAuditoria.aprobado = archivo.aprobado;
                            itemAuditoria.idArchivo = archivo.id;
                        }
                    }
                    auditoriaDet.AddRange(auditoriaProcesoDet);
                    //<--


                    var inspeccionesAuditoriaIDs = auditoriaDet.Select(x => x.inspeccionId).ToList();
                    var inspecciones = ctx.tbl5s_Inspeccion.Where(x => inspeccionesAuditoriaIDs.Contains(x.id)).SelectMany(x => x.detalles).ToList();

                    var CCAplica = ccFS.GetCCsNominaFiltrados(CCs);

                    foreach (var itemCC in CCAplica) 
                    {
                        TablaReporteEjecutivoDTO auxTablaReporteEjecutivo = new TablaReporteEjecutivoDTO();
                        auxTablaReporteEjecutivo.cc = itemCC.descripcion;

                        List<decimal> auxDatosPorMes = new List<decimal>(12) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                        var auditoriasCC = auditorias.Where(x => x.cc == itemCC.cc).ToList();
                        var auditoriasCCIDs = auditoriasCC.Select(x => x.id).ToList();
                        var auditoriasDetCC = auditoriaDet.Where(x => auditoriasCCIDs.Contains(x.auditoriaId)).ToList();
                        var inspeccionesCC = auditoriasDetCC.Select(x => x.inspeccionId).ToList();
                        var inspeccionesDetCC = inspecciones.Where(x => inspeccionesCC.Contains(x.inspeccionId)).ToList();

                        var InspeccionesCCOK = auditoriasDetCC.Where(x => x.respuesta == 1).Select(x => x.inspeccionId).ToList();

                        decimal sumAuditoriasCC = inspeccionesCC.Count();
                        decimal sumAuditoriasCCOK = InspeccionesCCOK.Count();

                        auxTablaReporteEjecutivo.total = Math.Truncate(((sumAuditoriasCC / (sumAuditoriasCC == 0 ? 1 : sumAuditoriasCC)) * 100M) * 100M) / 100M;

                        decimal sumAuditoriasCCOKClasificar = inspeccionesDetCC.Where(x => InspeccionesCCOK.Contains(x.inspeccionId) && x.cincoId == 1).Count();
                        decimal sumAuditoriasCCClasificar = inspeccionesDetCC.Where(x => inspeccionesCC.Contains(x.inspeccionId) && x.cincoId == 1).Count();

                        auxTablaReporteEjecutivo.clasificar = Math.Truncate(((sumAuditoriasCCOKClasificar / (sumAuditoriasCCClasificar == 0 ? 1 : sumAuditoriasCCClasificar)) * 100M) * 100M) / 100M;

                        decimal sumAuditoriasCCOKOrdenar = inspeccionesDetCC.Where(x => InspeccionesCCOK.Contains(x.inspeccionId) && x.cincoId == 2).Count();
                        decimal sumAuditoriasCCOrdenar = inspeccionesDetCC.Where(x => inspeccionesCC.Contains(x.inspeccionId) && x.cincoId == 2).Count();

                        auxTablaReporteEjecutivo.ordenar = Math.Truncate(((sumAuditoriasCCOKOrdenar / (sumAuditoriasCCOrdenar == 0 ? 1 : sumAuditoriasCCOrdenar)) * 100M) * 100M) / 100M;

                        decimal sumAuditoriasCCOKLimpiar = inspeccionesDetCC.Where(x => InspeccionesCCOK.Contains(x.inspeccionId) && x.cincoId == 3).Count();
                        decimal sumAuditoriasCCLimpiar = inspeccionesDetCC.Where(x => inspeccionesCC.Contains(x.inspeccionId) && x.cincoId == 3).Count();

                        auxTablaReporteEjecutivo.limpiar = Math.Truncate(((sumAuditoriasCCOKLimpiar / (sumAuditoriasCCLimpiar == 0 ? 1 : sumAuditoriasCCLimpiar)) * 100M) * 100M) / 100M;

                        decimal sumAuditoriasCCOKEstandarizar = inspeccionesDetCC.Where(x => InspeccionesCCOK.Contains(x.inspeccionId) && x.cincoId == 4).Count();
                        decimal sumAuditoriasCCEstandarizar = inspeccionesDetCC.Where(x => inspeccionesCC.Contains(x.inspeccionId) && x.cincoId == 4).Count();

                        auxTablaReporteEjecutivo.estandarizar = Math.Truncate(((sumAuditoriasCCOKEstandarizar / (sumAuditoriasCCEstandarizar == 0 ? 1 : sumAuditoriasCCEstandarizar)) * 100M) * 100M) / 100M;

                        decimal sumAuditoriasCCOKDisciplina = inspeccionesDetCC.Where(x => InspeccionesCCOK.Contains(x.inspeccionId) && x.cincoId == 5).Count();
                        decimal sumAuditoriasCCDisciplina = inspeccionesDetCC.Where(x => inspeccionesCC.Contains(x.inspeccionId) && x.cincoId == 5).Count();

                        auxTablaReporteEjecutivo.disciplina = Math.Truncate(((sumAuditoriasCCOKDisciplina / (sumAuditoriasCCDisciplina == 0 ? 1 : sumAuditoriasCCDisciplina)) * 100M) * 100M) / 100M;
                        tablaReporteEjecutivo.Add(auxTablaReporteEjecutivo);

                        decimal sumatoriaOKTotal = (sumAuditoriasCCOKClasificar + sumAuditoriasCCOKOrdenar + sumAuditoriasCCOKLimpiar + sumAuditoriasCCOKEstandarizar + sumAuditoriasCCOKDisciplina);
                        decimal sumatoriaTotal = sumAuditoriasCCClasificar + sumAuditoriasCCOrdenar + sumAuditoriasCCLimpiar + sumAuditoriasCCEstandarizar + sumAuditoriasCCDisciplina;

                        decimal porcentajeTotal = Math.Truncate(((sumatoriaOKTotal / (sumatoriaTotal == 0 ? 1 : sumatoriaTotal)) * 100M) * 100M) / 100M;
                        porcentajesObras.Add(porcentajeTotal);

                        for (int i = 0; i < 12; i++)
                        {
                            var auditoriasMesCCIDs = auditoriasCC.Where(x => x.fecha.Month == (i + 1)).Select(x => x.id).ToList();
                            var auditoriasMesDetCC = auditoriaDet.Where(x => auditoriasMesCCIDs.Contains(x.auditoriaId)).ToList();
                            decimal inspeccionesMesCC = auditoriasMesDetCC.Select(x => x.inspeccionId).Count();
                            decimal InspeccionesMesCCOK = auditoriasMesDetCC.Where(x => x.respuesta == 1).Select(x => x.inspeccionId).Count();

                            auxDatosPorMes[i] = Math.Truncate(((InspeccionesMesCCOK / (inspeccionesMesCC == 0 ? 1 : inspeccionesMesCC)) * 100M) * 100M) / 100M;
                        }
                        datosPorMes.Add(auxDatosPorMes);
                    }

                    resultado.Add("tablaReporteEjecutivo", tablaReporteEjecutivo);
                    resultado.Add("obras", CCAplica.Select(x => x.cc + " " + x.descripcion));
                    resultado.Add("porcentajesObras", porcentajesObras);
                    resultado.Add("datosPorMes", datosPorMes);

                    resultado.Add(SUCCESS, true);
                    
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        #endregion
		#endregion
        
    }
}