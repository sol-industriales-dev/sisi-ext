using Core.DAO.Maquinaria.Overhaul;
using Core.DTO.Maquinaria.Overhaul;
using Core.Entity.Maquinaria.Overhaul;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using Core.DTO.Principal.Generales;
using Data.EntityFramework.Context;
using System.Data.Entity.Migrations;
using Core.DTO;
using Core.Enum.Maquinaria.Overhaul;
using Core.DTO.Utils.Data;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Infrastructure.Utils;
using Core.DTO.Maquinaria.Mantenimiento.Correctivo.ReporteFalla;
using Core.Enum;
using System.IO;

namespace Data.DAO.Maquinaria.Overhaul
{
    public class ReporteFallaOverhaulDAO : GenericDAO<tblM_ReporteFalla>, IReporteFallaOverhaulDAO
    {
        string nombreControlador = "ReporteFallaOverhaul";
        public bool Guardar(tblM_ReporteFalla obj)
        {
            var esGuardado = false;
            var esNuevo = obj.id == 0;
            using (var _ctx = new MainContext())
            using (var _trans = _ctx.Database.BeginTransaction())
                try
                {
                    _ctx.tblM_ReporteFalla.AddOrUpdate(obj);
                    _ctx.SaveChanges();
                    obj.lstArchivos.ToList().ForEach(archivo =>
                    {
                        _ctx.tblM_ReporteFalla_Archivo.AddOrUpdate(archivo);
                        _ctx.SaveChanges();
                    });
                    esGuardado = obj.id > 0;
                    if (esGuardado)
                    {
                        _trans.Commit();
                    }
                }
                catch (Exception o_O)
                {
                    esGuardado = false;
                    var accionEnum = esNuevo ? AccionEnum.AGREGAR : AccionEnum.ACTUALIZAR;
                    _trans.Rollback();
                    LogError(vSesiones.sesionSistemaActual, vSesiones.sesionCurrentView, nombreControlador, "guardarReporteFallaOverhau", o_O, accionEnum, obj.id, obj);
                }
            return esGuardado;
        }

        private Dictionary<string, object> ArchivosNuevo(List<tblM_ReporteFalla_Archivo> lstArchivos)
        {
            try
            {
                #region SE VERIFICA QUE LA EVIDENCIA A GUARDAR, SE ENCUENTRE DISPONIBLE EN LA BASE DE DATOS
                Dictionary<string, object> result = new Dictionary<string, object>();

                List<tblM_ReporteFalla_Archivo> lstArchivosNuevos = new List<tblM_ReporteFalla_Archivo>();

                int idReporteFalla = lstArchivos[0].idReporteFalla;
                List<tblM_ReporteFalla_Archivo> lstArchivosSQL = _context.tblM_ReporteFalla_Archivo.Where(w => w.idReporteFalla == idReporteFalla && w.esActivo).ToList();
                lstArchivosNuevos.AddRange(lstArchivos.Where(w => !lstArchivosSQL.Select(s => s.nombre).Contains(w.nombre)));
                result.Add("lstArchivosNuevos", lstArchivosNuevos);

                #region ARCHIVOS A ELIMINAR
                List<tblM_ReporteFalla_Archivo> lstArchivosEliminar = new List<tblM_ReporteFalla_Archivo>();
                lstArchivosEliminar.AddRange(lstArchivosSQL.Where(w => !lstArchivos.Select(s => s.nombre).Contains(w.nombre)));
                result.Add("lstArchivosEliminar", lstArchivosEliminar);
                #endregion

                return result;
                #endregion
            }
            catch (Exception e)
            {
                LogError(2, 0, "ReporteFallaController", "ArchivosNuevo", e, AccionEnum.CONSULTA, 0, lstArchivos);
                return null;
            }
        }

        public bool Guardar(tblM_ReporteFalla_Componente obj)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            //obj.Reporte.lstArchivos.ToList().ForEach(x => { x.fechaRegistro = DateTime.Now; });
            var esGuardado = false;
            var esNuevo = obj.Reporte.id == 0;

            #region SE OBTIENE LOS ARCHIVOS NUEVOS SOLAMENTE, PARA NO REEINSERTAR LOS EVIDENCIAS YA EXISTENTES
            List<tblM_ReporteFalla_Archivo> lstArchivos = new List<tblM_ReporteFalla_Archivo>();
            List<tblM_ReporteFalla_Archivo> lstArchivosNuevos = new List<tblM_ReporteFalla_Archivo>();
            List<tblM_ReporteFalla_Archivo> lstArchivosEliminar = new List<tblM_ReporteFalla_Archivo>();
            if (obj.Reporte.lstArchivos.Count() > 0)
            {
                var archivos = obj.Reporte.lstArchivos;
                foreach (var item in archivos)
                {
                    tblM_ReporteFalla_Archivo objArchivo = new tblM_ReporteFalla_Archivo();
                    objArchivo.id = item.id;
                    objArchivo.idReporteFalla = item.idReporteFalla;
                    objArchivo.tipo = item.tipo;
                    objArchivo.nombre = item.nombre;
                    objArchivo.ruta = item.ruta;
                    objArchivo.esActivo = true;
                    objArchivo.fechaRegistro = DateTime.Now;
                    lstArchivos.Add(objArchivo);
                }

                #region SE OBTIENE LOS ARCHIVOS NUEVOS A AGREGAR
                int idReporteFalla = obj.Reporte.lstArchivos[0].idReporteFalla;
                List<tblM_ReporteFalla_Archivo> lstArchivosSQL = _context.tblM_ReporteFalla_Archivo.Where(w => w.idReporteFalla == idReporteFalla && w.esActivo).ToList();
                lstArchivosNuevos.AddRange(lstArchivos.Where(w => !lstArchivosSQL.Select(s => s.nombre).Contains(w.nombre)));
                #endregion

                #region ARCHIVOS A ELIMINAR
                lstArchivosEliminar.AddRange(lstArchivosSQL.Where(w => !lstArchivos.Select(s => s.nombre).Contains(w.nombre)));
                #endregion
            }
            else
            {
                lstArchivosEliminar = _context.tblM_ReporteFalla_Archivo.Where(w => w.idReporteFalla == obj.Reporte.id && w.esActivo).ToList();
                foreach (var item in lstArchivosEliminar) { item.esActivo = false; }
            }
            #endregion

            var maxID = _context.tblM_ReporteFalla_Archivo.Max(x => x.id) + 1;
            obj.Reporte.lstArchivos = null;
            using (var _ctx = new MainContext())
            using (var _trans = _ctx.Database.BeginTransaction())
                try
                {
                    _ctx.tblM_ReporteFalla_Componente.AddOrUpdate(obj);
                    _ctx.SaveChanges();
                    //var archivosGuardados = _ctx.tblM_ReporteFalla_Archivo.Where(x => x.idReporteFalla == obj.idReporteFalla);
                    //foreach (var item in lstArchivosEliminar) { item.esActivo = false; }
                    List<int> lstArchivosIDEliminar = new List<int>();
                    foreach (var item in lstArchivosEliminar)
                    {
                        lstArchivosIDEliminar.Add(item.id);
                    }
                    List<tblM_ReporteFalla_Archivo> objEliminarArchivos = _context.tblM_ReporteFalla_Archivo.Where(w => lstArchivosIDEliminar.Contains(w.id) && w.esActivo).ToList();
                    foreach (var item in objEliminarArchivos) { item.esActivo = false; }
                    _context.SaveChanges();
                    //_ctx.SaveChanges();
                    //archivos.ToList().ForEach(archivo =>
                    lstArchivosNuevos.ToList().ForEach(archivo =>
                    {
                        archivo.rptFalla = null;
                        archivo.idReporteFalla = obj.idReporteFalla;
                        _ctx.tblM_ReporteFalla_Archivo.Add(archivo);
                        _ctx.SaveChanges();
                    });
                    esGuardado = obj.id > 0;
                    if (esGuardado)
                    {
                        _trans.Commit();
                    }
                }
                catch (Exception o_O)
                {
                    var accionEnum = esNuevo ? AccionEnum.AGREGAR : AccionEnum.ACTUALIZAR;
                    esGuardado = false;
                    _trans.Rollback();
                    LogError(vSesiones.sesionSistemaActual, vSesiones.sesionCurrentView, nombreControlador, "guardarReporteFallaComponenteOverhau", o_O, accionEnum, obj.id, obj);
                }
            return esGuardado;
        }
        public bool Guardar(tblM_ReporteFalla_Reparacion obj)
        {
            var esGuardado = false;
            var esNuevo = obj.Reporte.id == 0;
            using (var _ctx = new MainContext())
            using (var _trans = _ctx.Database.BeginTransaction())
                try
                {
                    _ctx.tblM_ReporteFalla_Reparacion.AddOrUpdate(obj);
                    _ctx.SaveChanges();
                    obj.Reporte.lstArchivos.ToList().ForEach(archivo =>
                    {
                        _ctx.tblM_ReporteFalla_Archivo.AddOrUpdate(archivo);
                        _ctx.SaveChanges();
                    });
                    esGuardado = obj.id > 0;
                    if (esGuardado)
                    {
                        _trans.Commit();
                    }
                }
                catch (Exception o_O)
                {
                    var accionEnum = esNuevo ? AccionEnum.AGREGAR : AccionEnum.ACTUALIZAR;
                    esGuardado = false;
                    _trans.Rollback();
                    LogError(vSesiones.sesionSistemaActual, vSesiones.sesionCurrentView, nombreControlador, "guardarReporteFallaReparacionOverhau", o_O, accionEnum, obj.id, obj);
                }
            return esGuardado;
        }
        public List<tblM_ReporteFalla> getReporteFallas(FiltrosReporteFallaDTO obj)
        {
            return null;
        }

        public void Eliminar(int id)
        {
            if (ExistsByID(id))
            {
                var aux = _context.tblM_ReporteFalla.FirstOrDefault(x => x.id == id);
                aux.estatus = 3;
                var existeComponente = _context.tblM_ReporteFalla_Componente.Where(x => x.idReporteFalla == id).ToList();
                if (existeComponente.Count() > 0) 
                {
                    foreach (var item in existeComponente) 
                    {
                        var auxComponente = _context.tblM_CatComponente.FirstOrDefault(x => x.id == item.Componente);
                        if (auxComponente != null && auxComponente.falla == true) auxComponente.falla = false;
                    }
                }
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("No se encuenta el registro que desea eliminar");
            }
        }

        private bool ExistsByID(int id)
        {
            return _context.tblM_ReporteFalla.Where(x => x.id == id).ToList().Count > 0 ? true : false;
        }
        public bool EliminarReparacionDesdeComponente(tblM_ReporteFalla_Componente obj)
        {
            var esEliminado = false;
            var esNuevo = obj.Reporte.id == 0;
            using (var _ctx = new MainContext())
            using (var _trans = _ctx.Database.BeginTransaction())
                try
                {
                    var rptReparacion = _ctx.tblM_ReporteFalla_Reparacion.FirstOrDefault(reparacion => reparacion.idReporteFalla == obj.idReporteFalla);
                    if (rptReparacion != null)
                    {
                        rptReparacion.Reporte = null;
                        _ctx.tblM_ReporteFalla_Reparacion.Remove(rptReparacion);
                        _ctx.SaveChanges();
                        esEliminado = true;
                    }
                }
                catch (Exception o_O)
                {
                    esEliminado = false;
                    _trans.Rollback();
                    LogError(vSesiones.sesionSistemaActual, vSesiones.sesionCurrentView, nombreControlador, "eliminarReporteFallaReparacionDesdeComponenteOverhau", o_O, AccionEnum.ELIMINAR, obj.id, obj);
                }
            return esEliminado;
        }
        public bool EliminarComponenteDesdeReparacion(tblM_ReporteFalla_Reparacion obj)
        {
            var esEliminado = false;
            var esNuevo = obj.Reporte.id == 0;
            using (var _ctx = new MainContext())
            using (var _trans = _ctx.Database.BeginTransaction())
                try
                {
                    var rptComponente = _ctx.tblM_ReporteFalla_Componente.FirstOrDefault(componente => componente.idReporteFalla == obj.idReporteFalla);
                    if (rptComponente != null)
                    {
                        rptComponente.Reporte = null;
                        _ctx.tblM_ReporteFalla_Componente.Remove(rptComponente);
                        _ctx.SaveChanges();
                        esEliminado = true;
                    }
                }
                catch (Exception o_O)
                {
                    esEliminado = false;
                    _trans.Rollback();
                    LogError(vSesiones.sesionSistemaActual, vSesiones.sesionCurrentView, nombreControlador, "eliminarReporteFallaComponenteDesdeReparacionOverhau", o_O, AccionEnum.ELIMINAR, obj.id, obj);
                }
            return esEliminado;
        }

        public List<ReporteFallaDTO> cargarReportes(int estatus, /*string descripcionComponente,*/ int cc)
        {

            var areaCuentaActual = "";
            if (cc == -1) { areaCuentaActual = "-1"; }
            else { areaCuentaActual = _context.tblP_CC.FirstOrDefault(x => x.id == cc).areaCuenta; }
            var reportes = _context.tblM_ReporteFalla.Where(x => (cc == -1 ? true : x.cc == areaCuentaActual) && (estatus == -1 ? true : (estatus == 0 ? x.estatus < 2 : x.estatus == 2)) /*&& (descripcionComponente == "" ? true : x.componenteRemovido.subConjunto.descripcion == descripcionComponente)*/)
                .ToList();
            var reportesIDs = reportes.Select(x => x.id).ToList();
            var componentes = _context.tblM_ReporteFalla_Componente.Where(x => reportesIDs.Contains(x.idReporteFalla)).ToList();
            var componentesIds = componentes.Count() > 0 ? componentes.Select(x => x.Componente).ToList() : new List<int>();
            var noComponentes = _context.tblM_CatComponente.Where(x => componentesIds.Contains(x.id)).ToList();
            var insumos = _context.tblM_ReporteFalla_Reparacion.Where(x => reportesIDs.Contains(x.idReporteFalla)).ToList();
            
            var reportesDTO = reportes.Select(x => 
                {
                    var auxComponente = componentes.FirstOrDefault(y => y.idReporteFalla == x.id);
                    var noComponente = auxComponente == null ? new tblM_CatComponente() : noComponentes.FirstOrDefault(y => y.id == auxComponente.Componente);
                    var auxInsumos = insumos.FirstOrDefault(y => y.idReporteFalla == x.id);
                    return new ReporteFallaDTO
                    {
                                id = x.id,
                                maquinaID = x.maquinaID,
                                fechaReporte = x.fechaReporte,
                                fechaParo = x.fechaParo,
                                cc = x.cc,
                                descripcionFalla = x.descripcionFalla,
                                fallaComponente = x.fallaComponente,
                                causaFalla = x.causaFalla,
                                diagnosticosAplicados = x.diagnosticosAplicados,
                                tipoReparacion = x.tipoReparacion,
                                reparaciones = x.reparaciones,
                                destino = x.destino,
                                realiza = x.realiza,
                                revisa = x.revisa,
                                procedencia = x.procedencia,
                                fechaAlta = x.fechaAlta,
                                horometroReporte = x.horometroReporte,
                                estatus = x.estatus,
                                lstArchivos = x.lstArchivos,
                                componenteInsumo = x.fallaComponente == 0 ? (auxInsumos == null ? "N/A" : auxInsumos.Insumo.ToString()) : (noComponente == null ? "N/A" : noComponente.noComponente),
                    };
                })
                .ToList();
            return reportesDTO;
        }

        public tblM_ReporteFalla aprobarReporte(int idReporte)
        {
            var reporte = _context.tblM_ReporteFalla.FirstOrDefault(x => x.id == idReporte && x.estatus != 2);
            reporte.estatus = 2;
            Guardar(reporte);
            return reporte;
        }
        public tblM_ReporteFalla getReporte(int idReporte)
        {
            var reporte = _context.tblM_ReporteFalla.FirstOrDefault(x => x.id == idReporte);
            if (reporte == null)
            {
                var ahora = DateTime.Now;
                reporte = new tblM_ReporteFalla()
                {
                    fallaComponente = 1,
                    fechaReporte = ahora.ToString("dd/MM/yyyy"),
                    fechaAlta = ahora.ToString("dd/MM/yyyy"),
                    fechaParo = ahora.ToString("dd/MM/yyyy"),
                    //maquina = new tblM_CatMaquina(),
                    lstArchivos = new List<tblM_ReporteFalla_Archivo>()
                };
            }
            else {
                //List<tblM_ReporteFalla_Archivo> archivos = _context.tblM_ReporteFalla_Archivo.Where(x => x.idReporteFalla == reporte.id && x.esActivo).ToList();
                reporte.lstArchivos = reporte.lstArchivos.Where(x => x.esActivo).ToList();

                //reporte.lstArchivos = archivos;
            }
            foreach (var item in reporte.lstArchivos)
            {
                item.rptFalla = null;
            }
            return reporte;
        }
        public tblM_ReporteFalla_Componente getRptComoponente(int idReporte)
        {
            var rpt = _context.tblM_ReporteFalla_Componente.FirstOrDefault(componente => componente.idReporteFalla == idReporte);

            if (rpt == null)
            {
                rpt = new tblM_ReporteFalla_Componente()
                {
                    Reporte = getReporteByID(idReporte)
                };
            }
            else 
            {
                var auxComponente = _context.tblM_CatComponente.FirstOrDefault(x => x.id == rpt.Componente);
                if (auxComponente.numParte != null) rpt.Parte = auxComponente.numParte;
            }
            return rpt;
        }
        public tblM_ReporteFalla_Reparacion getRptReparacion(int idReporte)
        {
            var rpt = _context.tblM_ReporteFalla_Reparacion.FirstOrDefault(reparacion => reparacion.idReporteFalla == idReporte);
            if (rpt == null)
            {
                rpt = new tblM_ReporteFalla_Reparacion()
                {
                    Reporte = getReporteByID(idReporte)
                };
            }
            return rpt;
        }
        public tblM_ReporteFalla getReporteByID(int idReporte)
        {
            var reporte = _context.tblM_ReporteFalla.Where(w => w.id == idReporte).FirstOrDefault(x => x.id == idReporte) ?? new tblM_ReporteFalla();
            if (reporte == null)
            {
                reporte = new tblM_ReporteFalla()
                {
                    //maquina = new tblM_CatMaquina(),
                    lstArchivos = new List<tblM_ReporteFalla_Archivo>()
                };
            }
            return reporte;
        }
        public List<tblM_ReporteFalla_Archivo> getLstImagenes(int idReporte)
        {
            var lst = _context.tblM_ReporteFalla_Archivo.Where(w => w.idReporteFalla == idReporte && w.tipo == rptFallaTipoArchivoEnum.EvidenciaFotografica).ToList();
            return lst;
        }
        public int getIdComponenteFromIdReporteFalla(int idReporte)
        {
            var componente = getRptComoponente(idReporte);
            if (componente.id == 0)
            {
                var reparacion = getRptReparacion(idReporte);
                return reparacion.Insumo;
            }
            else
            {
                return componente.Componente;
            }
        }
        public tblM_CatSubConjunto getSubconjuntoComponente(int idComponente)
        {
            return (_context.tblM_CatComponente.FirstOrDefault(x => x.id == idComponente) ?? new tblM_CatComponente()
                                                                                                {
                                                                                                    subConjunto = new tblM_CatSubConjunto()
                                                                                                }).subConjunto;
        }

        //public int getConjuntoComponente(int idSubconjunto) 
        //{
        //    return _context.tblM_CatSubConjunto.FirstOrDefault(x => x.id == idSubconjunto).conjuntoID;
        //}

        public List<tblM_trackComponentes> fillCboComponentes(int idMaquina, int idSubconjunto)
        {
            var trackActual = _context.tblM_trackComponentes.GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault())
            .Where(x => x.locacionID == idMaquina && x.componente.subConjuntoID == idSubconjunto).ToList();
            return trackActual;
        }

        public tblM_AsignacionEquipos getProcedenciaMaquina(int idMaquina)
        {
            return _context.tblM_AsignacionEquipos.Where(x => x.noEconomicoID == idMaquina).OrderByDescending(x => x.fechaAsignacion).FirstOrDefault();
        }

        public string getCCNameByAC(string areaCuenta)
        {
            try
            {
                return _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == areaCuenta).descripcion;
            }
            catch (Exception)
            {
                return areaCuenta;
            }
        }

        public tblM_CatComponente getComponente(int idComponente)
        {
            return _context.tblM_CatComponente.FirstOrDefault(x => x.id == idComponente);
        }

        public tblM_trackComponentes getTrackingActualComponente(int idComponente)
        {
            return _context.tblM_trackComponentes.Where(x => x.componenteID == idComponente).OrderByDescending(x => x.fecha).FirstOrDefault();
        }

        public string getCCNameByID(int idCC)
        {
            return _context.tblP_CC.FirstOrDefault(x => x.id == idCC).descripcion;
        }

        public List<ComboDTO> fillVistoBueno(string CentroCostos)
        {
            //List<ComboDTO> data = new List<ComboDTO>();
            //CentroCostos = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == CentroCostos).cc;
            //var getCatEmpleado =
            //    @"SELECT " +
            //        "a.clave_empleado AS Value, " +
            //        "(LTRIM(RTRIM(a.nombre))+' '+replace(a.ape_paterno, ' ', '')+' '+replace(a.ape_materno, ' ', '')) AS Text, " +
            //        "a.CC_Contable AS Prefijo " +
            //    "FROM DBA.sn_empleados a " +
            //    "INNER JOIN si_puestos b " +
            //    "ON a.puesto = b.puesto AND a.tipo_nomina = b.tipo_nomina " +
            //    "WHERE a.CC_Contable = '" + CentroCostos + "' AND a.estatus_empleado = 'A'";// AND b.descripcion LIKE '%ADMINISTRADOR DE MAQUINARIA%'";
            //try
            //{
            //    var resultado = (IList<ComboDTO>)ContextEnKontrolNominaArrendadora.Where(getCatEmpleado, 1).ToObject<IList<ComboDTO>>();
            //    foreach (var item in resultado) { data.Add(item); }
            //}
            //catch { }
            //try
            //{
            //    var resultado2 = (IList<ComboDTO>)ContextEnKontrolNominaArrendadora.Where(getCatEmpleado, 2).ToObject<IList<ComboDTO>>();
            //    foreach (var item in resultado2) { data.Add(item); }
            //}
            //catch { }
            //List<ComboDTO> usuarios = new List<ComboDTO>();
            var emps = _context.tblP_CC_Usuario.Where(x => x.cc.Equals(CentroCostos)).Select(x=>x.usuarioID).ToList();
            var filtro = _context.tblP_Usuario.Where(x => emps.Contains(x.id) && x.estatus && !x.cliente && (x.cveEmpleado!=null && !x.cveEmpleado.Equals(""))).Select(x => new ComboDTO { Value=x.cveEmpleado, Text=(x.nombre+" "+x.apellidoPaterno+" "+x.apellidoMaterno) }).ToList();
            //foreach (var item in emps)
            //{
            //    var usuarioSIGOPLAN = _context.tblP_Usuario.FirstOrDefault(x => x.cveEmpleado.Equals(item.usuarioID));
            //    if (usuarioSIGOPLAN != null) { item.Value = usuarioSIGOPLAN.id.ToString(); usuarios.Add(item); }
            //}
            return filtro;
        }

        public List<ComboDTO> fillCboRevisa(string CentroCostos)
        {
            List<ComboDTO> data = new List<ComboDTO>();
            CentroCostos = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == CentroCostos).cc;
            var getCatEmpleado =
                @"SELECT " +
                    "a.clave_empleado AS Value, " +
                    "(LTRIM(RTRIM(a.nombre))+' '+replace(a.ape_paterno, ' ', '')+' '+replace(a.ape_materno, ' ', '')+' ('+b.descripcion+')') AS Text, " +
                    "a.CC_Contable AS Prefijo " +
                "FROM DBA.sn_empleados a " +
                "INNER JOIN si_puestos b " +
                "ON a.puesto = b.puesto AND a.tipo_nomina = b.tipo_nomina " +
                "WHERE a.CC_Contable = '" + CentroCostos + "' AND a.estatus_empleado = 'A'";
            try
            {
                var resultado = (IList<ComboDTO>)ContextEnKontrolNominaArrendadora.Where(getCatEmpleado, 1).ToObject<IList<ComboDTO>>();
                foreach (var item in resultado) { data.Add(item); }
            }
            catch { }
            try
            {
                var resultado2 = (IList<ComboDTO>)ContextEnKontrolNominaArrendadora.Where(getCatEmpleado, 2).ToObject<IList<ComboDTO>>();
                foreach (var item in resultado2) { data.Add(item); }
            }
            catch { }
            return data;
        }

        public Dictionary<string, object> GetArchivosReporteFalla(int _idReporteFalla)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region SE OBTIENE LISTADO DE ARCHIVOS RELACIONADOS AL REPORTE FALLA SELECCIONADO
                    List<ReporteFallasArchivosDTO> lstArchivos = _context.Select<ReporteFallasArchivosDTO>(new DapperDTO
                    {
                        baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                        consulta = @"SELECT id, idReporteFalla, tipo, strTipo = '', nombre, ruta, esActivo, fechaRegistro FROM tblM_ReporteFalla_Archivo WHERE esActivo = @esActivo AND idReporteFalla = @idReporteFalla",
                        parametros = new { esActivo = true, idReporteFalla = _idReporteFalla }
                    }).ToList();

                    foreach (var item in lstArchivos)
                    {
                        if (item.tipo == (int)rptFallaTipoArchivoEnum.SOS)
                        {
                            item.strTipo = EnumHelper.GetDescription((rptFallaTipoArchivoEnum)item.tipo);
                        }
                        else if (item.tipo == (int)rptFallaTipoArchivoEnum.PSR)
                        {
                            item.strTipo = EnumHelper.GetDescription((rptFallaTipoArchivoEnum)item.tipo);
                        }
                        else if (item.tipo == (int)rptFallaTipoArchivoEnum.EvidenciaFotografica)
                        {
                            item.strTipo = EnumHelper.GetDescription((rptFallaTipoArchivoEnum)item.tipo);
                        }
                        else
                            item.strTipo = string.Empty;
                    }

                    resultado.Add("data", lstArchivos);
                    resultado.Add(SUCCESS, true);

                    dbContextTransaction.Commit();
                    #endregion

                    #region SE CREA BITACORA
                    SaveBitacora(16, (int)AccionEnum.CONSULTA, _idReporteFalla, JsonUtils.convertNetObjectToJson(lstArchivos));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, "ReporteFallaController", "GetArchivosReporteFalla", e, AccionEnum.CONSULTA, _idReporteFalla, 0);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            return resultado;
        }

        public Tuple<Stream, string> DescargarArchivoReporteFalla(int _idArchivo)
        {
            try
            {
                tblM_ReporteFalla_Archivo objArchivo = _context.Select<tblM_ReporteFalla_Archivo>(new DapperDTO
                {
                    baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                    consulta = @"SELECT id, idReporteFalla, tipo, nombre, ruta, esActivo, fechaRegistro FROM tblM_ReporteFalla_Archivo WHERE id = @idArchivo",
                    parametros = new { idArchivo = _idArchivo }
                }).FirstOrDefault();

#if DEBUG
                string rutaArchivoServidor = objArchivo.ruta;
                string rutaArchivoLocal = string.Empty;
                string[] splitArchivo = rutaArchivoServidor.Split('\\');
                foreach (var item in splitArchivo)
                {
                    rutaArchivoLocal = item;
                }
                string ruta = @"C:\MAQUINARIA\OVERHAUL\" + rutaArchivoLocal;
                var fileStream = GlobalUtils.GetFileAsStream(ruta);
                string name = Path.GetFileName(objArchivo.nombre);
#else
                var fileStream = GlobalUtils.GetFileAsStream(objArchivo.ruta);
                string name = Path.GetFileName(objArchivo.nombre);
#endif

                return Tuple.Create(fileStream, name);
            }
            catch (Exception e)
            {
                LogError(0, 0, "ReporteFallaController", "DescargarArchivoReporteFalla", e, AccionEnum.DESCARGAR, _idArchivo, 0);
                return null;
            }
        }
    }
}
