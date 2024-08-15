using Core.DAO.Maquinaria.Inventario;
using Core.DTO;
using Core.DTO.Enkontrol.Tablas.CC;
using Core.DTO.Maquinaria.Inventario;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Data;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario.ControlCalidad;
using Core.Entity.Principal.Usuarios;
using Core.Enum.Principal.Bitacoras;
using Core.Enum.Principal.Usuario;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Principal.Archivos;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Core.Enum.Multiempresa;
using Core.Entity.Principal.Multiempresa;

namespace Data.DAO.Maquinaria.Inventario
{
    public class ControlRecepcionyEnvioDAO : GenericDAO<tblM_ControlEnvioMaquinaria>, IControlRecepcionyEnvioDAO
    {
        private readonly string RutaBase = @"\\REPOSITORIO\Proyecto\SIGOPLAN\MAQUINARIA\INVENTARIO\ControlesEnvioyRecepcion";
        private const string RutaLocal = @"C:\Proyecto\SIGOPLAN\MAQUINARIA\INVENTARIO\ControlesEnvioyRecepcion";

        ArchivoFactoryServices archivofs = new ArchivoFactoryServices();
        public List<tblM_AsignacionEquipos> getListEquiposPendientes(int obj)
        {
            return _context.tblM_AsignacionEquipos.OrderBy(x => x.fechaAsignacion).ToList();
        }

        public string getCorreoGerente(string centroCostos)
        {

            var autorizacion = (from cc in _context.tblP_CC_Usuario
                                join a in _context.tblP_Autoriza on cc.id equals a.cc_usuario_ID
                                where cc.cc == centroCostos && a.perfilAutorizaID == 1
                                select a).ToList();

            var returnData = autorizacion.FirstOrDefault();
            if (returnData != null)
            {
                return returnData.usuario.correo;
            }
            else
            {
                return "";
            }

        }

        public List<tblM_AsignacionEquipos> GetListaControles(int obj, List<tblP_CC_Usuario> listObj, int tipoFiltro)
        {
            var listCC = listObj.Select(x => x.cc).ToList();

            switch (obj)
            {
                case 1:
                    {
                        if (tipoFiltro == 1)
                        {
                            var res = _context.tblM_AsignacionEquipos.Where(x => listCC.Contains(x.CCOrigen.ToString()) && x.estatus == 2);
                            return res.ToList();
                        }
                        else
                        {
                            var asignacioneslist = _context.tblM_AsignacionEquipos.Where(x => listCC.Contains(x.CCOrigen)).ToList();
                            var controleslist = _context.tblM_ControlEnvioMaquinaria.Where(y => y.tipoControl == 1).ToList();
                            var res = _context.tblM_AsignacionEquipos.Where(x => listCC.Contains(x.CCOrigen.ToString()) && x.estatus != 1);

                            var res1 = asignacioneslist.Join(controleslist,
                                                                             a => a.id,
                                                                             c => c.asignacionEquipoId,
                                                                        (a, c) => new { a, c }).Select(r => r.a).ToList();
                            return res1.ToList();
                        }

                    }
                case 2:
                    {
                        if (tipoFiltro == 1)
                        {
                            var res = _context.tblM_AsignacionEquipos.Where(x => listCC.Contains(x.cc.ToString()) && x.estatus == 4);
                            return res.ToList();
                        }
                        else
                        {
                            var asignacioneslist = _context.tblM_AsignacionEquipos.Where(x => listCC.Contains(x.cc) && x.estatus > 4).ToList().OrderByDescending(x => x.id);
                            var controleslist = _context.tblM_ControlEnvioMaquinaria.Where(y => y.tipoControl == 2).ToList();
                            var res = _context.tblM_AsignacionEquipos.Where(x => listCC.Contains(x.CCOrigen.ToString()) && x.estatus > 4);

                            var res1 = asignacioneslist.Join(controleslist,
                                                                             a => a.id,
                                                                             c => c.asignacionEquipoId,
                                                                        (a, c) => new { a, c }).Select(r => r.a).ToList();
                            return res1.ToList();
                        }
                    }
                case 3:
                    {
                        if (tipoFiltro == 1)
                        {
                            var res = _context.tblM_AsignacionEquipos.Where(x => listCC.Contains(x.cc.ToString()) && x.estatus == 6);
                            return res.ToList();
                        }
                        else
                        {
                            var asignacioneslist = _context.tblM_AsignacionEquipos.Where(x => listCC.Contains(x.cc)).ToList();
                            var controleslist = _context.tblM_ControlEnvioMaquinaria.Where(y => y.tipoControl == 3).ToList();
                            var res = _context.tblM_AsignacionEquipos.Where(x => listCC.Contains(x.CCOrigen.ToString()) && x.estatus > 6);

                            var res1 = asignacioneslist.Join(controleslist,
                                                                             a => a.id,
                                                                             c => c.asignacionEquipoId,
                                                                        (a, c) => new { a, c }).Select(r => r.a).ToList();
                            return res1.ToList();
                        }

                    }
                case 4:
                    {
                        //var res = _context.tblM_AsignacionEquipos.Where(x => x.estatus == 7 || x.estatus == 6).ToList();
                        //return res.ToList();
                        if (listCC.Contains("997") || listCC.Contains("1010") || listCC.Contains("1015"))
                        {
                            if (tipoFiltro == 1)
                            {
                                var res = _context.tblM_AsignacionEquipos.Where(x => x.estatus == 8);
                                return res.ToList();
                            }
                            else
                            {
                                var asignacioneslist = _context.tblM_AsignacionEquipos.Where(x => x.estatus > 8).ToList();
                                var controleslist = _context.tblM_ControlEnvioMaquinaria.Where(y => y.tipoControl == 4).ToList();
                                var res = _context.tblM_AsignacionEquipos.Where(x => x.estatus > 7);

                                var res1 = asignacioneslist.Join(controleslist,
                                                                                 a => a.id,
                                                                                 c => c.asignacionEquipoId,
                                                                            (a, c) => new { a, c }).Select(r => r.a).ToList();
                                return res1.ToList();
                            }

                        }
                        else
                        {
                            return null;
                        }
                    }

                case 7:
                    {
                        if (listCC.Contains("997"))
                        {
                            if (tipoFiltro == 1)
                            {
                                var res = _context.tblM_AsignacionEquipos.Where(x => x.estatus == obj).ToList();
                                return res.ToList();

                            }
                            else
                            {
                                var res = _context.tblM_AsignacionEquipos.Where(x => x.estatus >= 8).ToList();
                                return res.ToList();
                            }
                        }
                        return null;
                    }
                case 8:
                    {

                        if (tipoFiltro == 1)
                        {
                            var res = _context.tblM_AsignacionEquipos.Where(x => x.estatus == 8).ToList();

                            return res.ToList();
                        }
                        else
                        {
                            var res = _context.tblM_AsignacionEquipos.Where(x => x.estatus > 8).ToList();

                            return res.ToList();
                        }

                    }
                default:
                    return null;
            }


            //if (obj == 1)
            //{
            //    var listCC = listObj.Select(x => x.cc).ToList();
            //    var res = _context.tblM_AsignacionEquipos.Where(x => listCC.Contains(x.CCOrigen.ToString()));
            //    return res.ToList();
            //}

            //if (obj == 3)
            //{
            //    var listCC = listObj.Select(x => x.cc).ToList();
            //    var res = _context.tblM_AsignacionEquipos.Where(x => listCC.Contains(x.CCOrigen.ToString()) || x.CCOrigen == 997).ToList();
            //    return res.ToList();
            //}


        }

        public List<tblM_AsignacionEquipos> GetListaControlesCalidad(int obj, List<tblP_CC_Usuario> listObj, int tipoFiltro, DateTime? fechaInicio, DateTime? fechaFin, string cc, int? numEconomico)
        {

            tblP_CC objCC = null;
            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
            {
                case EmpresaEnum.Peru:
                    objCC = _context.tblP_CC.FirstOrDefault(x => x.cc == cc);
                    if (objCC != null)
                    {
                        cc = objCC.areaCuenta;
                    }
                    break;
            }

            var listCC = listObj.Select(x => x.cc).ToList();

            #region OG VERSION
            //switch (obj)
            //{
            //    case 1:
            //        {
            //            var res = _context.tblM_AsignacionEquipos.Where(x => listCC.Contains(x.CCOrigen.ToString()) && (tipoFiltro == 1 ? x.estatus == obj : x.estatus > obj));
            //            return res.ToList();
            //        }
            //    case 2:
            //        {
            //            if (tipoFiltro == 1)//pendiente
            //            {
            //                var res = _context.tblM_AsignacionEquipos.Where(x => x.estatus == obj && listCC.Contains(x.cc.ToString())).ToList();
            //                return res.ToList();
            //            }
            //            else
            //            {
            //                var ctrls = _context.tblM_CatControlCalidad.Where(x => x.TipoControl == 2).Select(x => x.IdAsignacion).ToList();
            //                var res = _context.tblM_AsignacionEquipos.Where(x => ctrls.Contains(x.id) && listCC.Contains(x.cc.ToString())).ToList();
            //                return res.ToList();
            //            }
            //        }
            //    case 3:
            //        {
            //            var res = _context.tblM_AsignacionEquipos.Where(x => listCC.Contains(x.cc.ToString()) && (tipoFiltro == 1 ? x.estatus.Equals(obj) : x.estatus >= (obj + 1))).ToList();
            //            return res.ToList();
            //        }
            //    case 4:
            //        {
            //            if (tipoFiltro == 1)//pendiente
            //            {
            //                var res = _context.tblM_AsignacionEquipos.Where(x => x.estatus == obj /*&& listCC.Contains(x.cc.ToString())*/).ToList();
            //                return res.ToList();
            //            }
            //            else
            //            {
            //                var ctrls = _context.tblM_CatControlCalidad.Where(x => x.TipoControl == 4).Select(x => x.IdAsignacion).ToList();
            //                var res = _context.tblM_AsignacionEquipos.Where(x => ctrls.Contains(x.id) && listCC.Contains(x.cc.ToString())).ToList();
            //                return res.ToList();
            //            }
            //        }
            //    default:
            //        return null;
            //}
            #endregion

            List<tblM_AsignacionEquipos> lstControles = null;
            try
            {

                #region SWITCH OBJ
                switch (obj)
                {
                    case 1:
                        {
                            var res = _context.tblM_AsignacionEquipos.Where(x => listCC.Contains(x.CCOrigen.ToString()) && (tipoFiltro == 1 ? x.estatus == obj : x.estatus > obj));
                            lstControles = res.ToList();
                            break;
                        }
                    case 2:
                        {
                            if (tipoFiltro == 1)//pendiente
                            {
                                var res = _context.tblM_AsignacionEquipos.Where(x => x.estatus == obj && listCC.Contains(x.cc.ToString())).ToList();
                                lstControles = res.ToList();
                                break;
                            }
                            else
                            {
                                var ctrls = _context.tblM_CatControlCalidad.Where(x => x.TipoControl == 2).Select(x => x.IdAsignacion).ToList();
                                var res = _context.tblM_AsignacionEquipos.Where(x => ctrls.Contains(x.id) && listCC.Contains(x.cc.ToString())).ToList();
                                lstControles = res.ToList();
                                break;
                            }
                        }
                    case 3:
                        {
                            var res = _context.tblM_AsignacionEquipos.Where(x => listCC.Contains(x.cc.ToString()) && (tipoFiltro == 1 ? x.estatus.Equals(obj) : x.estatus >= (obj + 1))).ToList();
                            lstControles = res.ToList();
                            break;
                        }
                    case 4:
                        {
                            if (tipoFiltro == 1)//pendiente
                            {
                                var res = _context.tblM_AsignacionEquipos.Where(x => x.estatus == obj /*&& listCC.Contains(x.cc.ToString())*/).ToList();
                                lstControles = res.ToList();
                                break;
                            }
                            else
                            {
                                var ctrls = _context.tblM_CatControlCalidad.Where(x => x.TipoControl == 4).Select(x => x.IdAsignacion).ToList();
                                var res = _context.tblM_AsignacionEquipos.Where(x => ctrls.Contains(x.id) && listCC.Contains(x.cc.ToString())).ToList();
                                lstControles = res.ToList();
                                break;
                            }
                        }
                    default:
                        lstControles = null;
                        break;
                }
                #endregion


                if (lstControles != null)
                {
                    if (fechaInicio != null && fechaFin != null)
	                {
                        lstControles = lstControles.Where(e =>
                                           (e.fechaInicio >= fechaInicio && e.fechaInicio <= fechaFin)
                                        && (!string.IsNullOrEmpty(cc) ? e.cc == cc : true)
                                       && (numEconomico != null ? e.noEconomicoID == numEconomico : true)
                                       ).ToList();
                    }
                    else
                    {
                        lstControles = lstControles.Where(e =>
                                       (!string.IsNullOrEmpty(cc) ? e.cc == cc : true)
                                       && (numEconomico != null ? e.noEconomicoID == numEconomico : true)
                                       ).ToList();
                    }
                    
                }

            }
            catch (Exception e)
            {

                throw;
            }

            return lstControles;

        }
        public List<tblM_AsignacionEquipos> GetListaControlesPendientesRecepcion()
        {

            var data = _context.tblM_AsignacionEquipos.Where(x => x.estatus == 2 || x.estatus == 4).ToList();
            return data;
        }

        public List<tblM_AsignacionEquipos> GetPendientesEnvio(List<tblP_CC_Usuario> listObj)
        {
            var listCC = listObj.Select(x => x.cc).ToList();

            var ListaPendientesEnvio = _context.tblM_AsignacionEquipos.Where(x => listCC.Contains(x.CCOrigen.ToString())).ToList();

            return ListaPendientesEnvio;
        }

        public tblM_CatMaquina GetInfoMaquinaria(int idEconomico)
        {

            return _context.tblM_CatMaquina.FirstOrDefault(x => x.id.Equals(idEconomico) && x.noEconomico != null);
        }


        public void QuitarComponentes(int maquinaid)
        {
            try
            {
                var barrenadora = _context.tblB_Barrenadora.FirstOrDefault(r => r.maquinaID == maquinaid);

                var piezas = _context.tblB_PiezaBarrenadora.Where(r => r.barrenadoraID == barrenadora.id && r.montada);

                foreach (var item in piezas)
                {
                    item.montada = false;
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }
        }
        public void SaveOrUpdate(tblM_ControlEnvioMaquinaria obj, HttpPostedFileBase file)
        {
            if (true)
            {
                if (obj.id == 0)
                {

                    if (file != null)
                    {
                        var noEconomico = _context.tblM_CatMaquina.FirstOrDefault(x => x.id.Equals(obj.noEconomico)).noEconomico;
                        var Prefijo = obj.tipoControl == 1 ? "E" : "R";
                        string extension = Path.GetExtension(file.FileName);

                        SaveEntity(obj, (int)BitacoraEnum.CONTROLENVIO);

                        string FileName = noEconomico + "-" + Prefijo.ToString() + obj.id.ToString() + extension;
                        string Ruta = archivofs.getArchivo().getUrlDelServidor(10) + FileName;
                        SaveArchivo(file, Ruta);

                        obj.Nombre = FileName;
                        obj.RutaArchivo = Ruta;
                        Update(obj, obj.id, (int)BitacoraEnum.CONTROLENVIO);
                    }
                    else
                    {
                        obj.Nombre = "";
                        obj.RutaArchivo = "";
                        SaveEntity(obj, (int)BitacoraEnum.CONTROLENVIO);
                    }
                }

                else
                    Update(obj, obj.id, (int)BitacoraEnum.CONTROLENVIO);
            }
            else
            {
                if (obj.id == 0)
                    throw new Exception("Ya se capturo el registro.");
                else
                    Update(obj, obj.id, (int)BitacoraEnum.CONTROLENVIO);
            }
        }

        public string SaveArchivo(HttpPostedFileBase archivo, string ruta)
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
            ruta = ruta.Replace("C:\\", "\\REPOSITORIO\\");
            File.WriteAllBytes(ruta, data);
            return "";
        }

        public tblM_AsignacionEquipos getInfoAsignacion(int obj)
        {
            return _context.tblM_AsignacionEquipos.FirstOrDefault(x => x.id.Equals(obj));
        }



        public string nombreEconomico(int obj)
        {
            return _context.tblM_CatMaquina.FirstOrDefault(x => x.id.Equals(obj)).noEconomico;
        }

        public tblM_ControlEnvioMaquinaria getReporteEnvio(int obj)
        {
            return _context.tblM_ControlEnvioMaquinaria.FirstOrDefault(x => x.id.Equals(obj));
        }
        public tblM_ControlEnvioMaquinaria getReporteEnvioTipo(int obj, int tipo)
        {
            return _context.tblM_ControlEnvioMaquinaria.FirstOrDefault(x => x.id == obj && x.tipoControl == tipo);
        }
        public tblM_ControlEnvioMaquinaria getReporteRecepcion(int idEconomico, int idSolicitud, int tipo)
        {
            return _context.tblM_ControlEnvioMaquinaria.OrderByDescending(x => x.id).FirstOrDefault(x => x.solicitudEquipoID == idSolicitud && x.noEconomico.Equals(idEconomico) && x.tipoControl == (tipo - 1));
        }

        public tblM_ControlEnvioMaquinaria getInfoControl(int idAsignacion, int TipoControl, int idSolicitud)
        {
            return _context.tblM_ControlEnvioMaquinaria.OrderByDescending(x => x.id).FirstOrDefault(x => x.asignacionEquipoId.Equals(idAsignacion) && x.tipoControl.Equals(TipoControl) && x.solicitudEquipoID.Equals(idSolicitud));

        }

        public List<LiberacionDTO> getMaquinariaAsignada(int cc)
        {

            string centroCostos = cc.ToString();
            if (cc == 997)
            {
                var resultado = (from m in _context.tblM_CatMaquina
                                 where m.centro_costos == centroCostos && (m.noEconomico != null || m.noEconomico != "")
                                 select m).ToList();




                return resultado.Select(m => new LiberacionDTO
                                 {
                                     idEconomico = m.id,
                                     Economico = m.noEconomico,
                                     CC = m.centro_costos,
                                     FechaFin = DateTime.Now,
                                     Horas = 0,
                                     idAsignacion = 0,
                                     estatusMaquina = m.estatus,
                                     Comentario = m.ComentarioStandBy,
                                     descripcion = m.descripcion,
                                     estatus = m.estatus
                                 }).ToList();
            }
            else
            {
                var resultado = (from a in _context.tblM_AsignacionEquipos
                                 join m in _context.tblM_CatMaquina on a.noEconomicoID equals m.id
                                 where
                                 a.estatus.Equals(5)
                                 && (cc == 0 ? a.id.Equals(a.id) : a.cc.Equals(cc)) && a.noEconomicoID != 0
                                 select new LiberacionDTO
                                 {
                                     idEconomico = m.id,
                                     Economico = m.noEconomico,
                                     CC = a.cc,
                                     FechaFin = a.fechaFin,
                                     Horas = a.Horas,
                                     idAsignacion = a.id,
                                     estatusMaquina = m.estatus,
                                     Comentario = m.ComentarioStandBy,
                                     descripcion = m.descripcion,
                                     estatus = m.estatus
                                 }).ToList();


                return resultado;
            }

        }

        public List<LiberacionDTO> getMaquinariaAsignadaPendienteAutorizar(int cc)
        {
            var resultado = (from a in _context.tblM_AsignacionEquipos
                             join m in _context.tblM_CatMaquina on a.noEconomicoID equals m.id
                             join auto in _context.tblM_AutorizacionStandBy on a.noEconomicoID equals auto.idEconomico
                             where a.noEconomicoID != 0 && auto.autorizacion == 0 && auto.usuarioAutoriza == 0
                             select new LiberacionDTO
                             {
                                 idEconomico = m.id,
                                 Economico = m.noEconomico,
                                 CC = a.cc,
                                 FechaFin = a.fechaFin,
                                 Horas = a.Horas,
                                 idAsignacion = a.id,
                                 estatusMaquina = auto.tipoStandBy,
                                 Comentario = m.ComentarioStandBy,
                                 descripcion = m.descripcion
                             }).ToList();
            return resultado;
        }

        public string getAC(string ac_cc)
        {
            var cc = _context.tblP_CC.FirstOrDefault(f => f.areaCuenta == ac_cc);

            if (cc == null)
            {
                cc = _context.tblP_CC.FirstOrDefault(f => f.cc == ac_cc);
            }

            return ac_cc + " - " + (cc != null ? cc.descripcion : "");
        }

        public List<string> getCorreosAdministradoresMaquinaria(tblM_CatControlCalidad objCalidad)
        {
            List<string> correos = new List<string>();

            try
            {
                correos = (
                    from ccUsu in _context.tblP_CC_Usuario
                    join aut in _context.tblP_Autoriza on ccUsu.id equals aut.cc_usuario_ID
                    join usu in _context.tblP_Usuario on aut.usuarioID equals usu.id
                    where aut.perfilAutorizaID == 5 && (ccUsu.cc == objCalidad.CcOrigen || ccUsu.cc == objCalidad.CcDestino)
                    select usu.correo
                ).ToList();
            }
            catch (Exception e)
            {
                LogError(0, 0, "ControlCalidadController", "getCorreosAdministradoresMaquinaria", e, AccionEnum.CONSULTA, 0, objCalidad);
            }

            return correos;
        }

        public Tuple<Stream, string> descargarArchivos(int idAsignacion, int solicitudID)
        {
            string rutaFolderTemp = "";
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var asignacion = _context.tblM_AsignacionEquipos.FirstOrDefault(x => x.id == idAsignacion);
                var listaControlCalidad = _context.tblM_CatControlCalidad.Where(x => x.IdAsignacion == idAsignacion).ToList();

                var nombreFolderTemp = String.Format("{0} {1}", "tmp", DateTime.Now.ToString("dd-MM-y HH:mm:ss").Replace(":", "-"));
                var RutaTemp = "";

#if DEBUG
                RutaTemp = RutaLocal + "\\CARPETA_TEMPORAL";
#else
                RutaTemp = RutaBase + "\\CARPETA_TEMPORAL";
#endif

                rutaFolderTemp = Path.Combine(RutaTemp, nombreFolderTemp);

                Directory.CreateDirectory(rutaFolderTemp);

                foreach (var controlCalidad in listaControlCalidad)
                {
                    if (controlCalidad.archivoSetFotografico.Trim() != "")
                    {
#if DEBUG
                        controlCalidad.archivoSetFotografico = controlCalidad.archivoSetFotografico.Replace("\\\\REPOSITORIO", "C:");
#endif

                        if (File.Exists(controlCalidad.archivoSetFotografico.Trim()))
                        {
                            var nombreArchivo = (controlCalidad.archivoSetFotografico.Trim()).Split('\\')[7];
                            var rutaArchivoDestino = rutaFolderTemp + "\\" + nombreArchivo;

                            if (File.Exists(rutaArchivoDestino))
                            {
                                rutaArchivoDestino = nuevoNombreArchivoConsecutivo(rutaArchivoDestino);
                            }

                            File.Copy(controlCalidad.archivoSetFotografico.Trim(), rutaArchivoDestino);
                        }
                    }

                    if (controlCalidad.archivoRehabilitacion.Trim() != "")
                    {
#if DEBUG
                        controlCalidad.archivoRehabilitacion = controlCalidad.archivoRehabilitacion.Replace("\\\\REPOSITORIO", "C:");
#endif

                        if (File.Exists(controlCalidad.archivoRehabilitacion.Trim()))
                        {
                            var nombreArchivo = (controlCalidad.archivoRehabilitacion.Trim()).Split('\\')[7];
                            var rutaArchivoDestino = rutaFolderTemp + "\\" + nombreArchivo;

                            if (File.Exists(rutaArchivoDestino))
                            {
                                rutaArchivoDestino = nuevoNombreArchivoConsecutivo(rutaArchivoDestino);
                            }

                            File.Copy(controlCalidad.archivoRehabilitacion.Trim(), rutaArchivoDestino);
                        }
                    }

                    if (controlCalidad.archivoDN.Trim() != "")
                    {
#if DEBUG
                        controlCalidad.archivoDN = controlCalidad.archivoDN.Replace("\\\\REPOSITORIO", "C:");
#endif

                        if (File.Exists(controlCalidad.archivoDN.Trim()))
                        {
                            var nombreArchivo = (controlCalidad.archivoDN.Trim()).Split('\\')[7];
                            var rutaArchivoDestino = rutaFolderTemp + "\\" + nombreArchivo;

                            if (File.Exists(rutaArchivoDestino))
                            {
                                rutaArchivoDestino = nuevoNombreArchivoConsecutivo(rutaArchivoDestino);
                            }

                            File.Copy(controlCalidad.archivoDN.Trim(), rutaArchivoDestino);
                        }
                    }

                    if (controlCalidad.archivoSOS.Trim() != "")
                    {
#if DEBUG
                        controlCalidad.archivoSOS = controlCalidad.archivoSOS.Replace("\\\\REPOSITORIO", "C:");
#endif

                        if (File.Exists(controlCalidad.archivoSOS.Trim()))
                        {
                            var nombreArchivo = (controlCalidad.archivoSOS.Trim()).Split('\\')[7];
                            var rutaArchivoDestino = rutaFolderTemp + "\\" + nombreArchivo;

                            if (File.Exists(rutaArchivoDestino))
                            {
                                rutaArchivoDestino = nuevoNombreArchivoConsecutivo(rutaArchivoDestino);
                            }

                            File.Copy(controlCalidad.archivoSOS.Trim(), rutaArchivoDestino);
                        }
                    }

                    if (controlCalidad.archivoBitacora.Trim() != "")
                    {
#if DEBUG
                        controlCalidad.archivoBitacora = controlCalidad.archivoBitacora.Replace("\\\\REPOSITORIO", "C:");
#endif

                        if (File.Exists(controlCalidad.archivoBitacora.Trim()))
                        {
                            var nombreArchivo = (controlCalidad.archivoBitacora.Trim()).Split('\\')[7];
                            var rutaArchivoDestino = rutaFolderTemp + "\\" + nombreArchivo;

                            if (File.Exists(rutaArchivoDestino))
                            {
                                rutaArchivoDestino = nuevoNombreArchivoConsecutivo(rutaArchivoDestino);
                            }

                            File.Copy(controlCalidad.archivoBitacora.Trim(), rutaArchivoDestino);
                        }
                    }

                    if (controlCalidad.archivoCheckList.Trim() != "")
                    {
#if DEBUG
                        controlCalidad.archivoCheckList = controlCalidad.archivoCheckList.Replace("\\\\REPOSITORIO", "C:");
#endif

                        if (File.Exists(controlCalidad.archivoCheckList.Trim()))
                        {
                            var nombreArchivo = (controlCalidad.archivoCheckList.Trim()).Split('\\')[7];
                            var rutaArchivoDestino = rutaFolderTemp + "\\" + nombreArchivo;

                            if (File.Exists(rutaArchivoDestino))
                            {
                                rutaArchivoDestino = nuevoNombreArchivoConsecutivo(rutaArchivoDestino);
                            }

                            File.Copy(controlCalidad.archivoCheckList.Trim(), rutaArchivoDestino);
                        }
                    }

                    if (controlCalidad.archivoVidaAceites.Trim() != "")
                    {
#if DEBUG
                        controlCalidad.archivoVidaAceites = controlCalidad.archivoVidaAceites.Replace("\\\\REPOSITORIO", "C:");
#endif

                        if (File.Exists(controlCalidad.archivoVidaAceites.Trim()))
                        {
                            var nombreArchivo = (controlCalidad.archivoVidaAceites.Trim()).Split('\\')[7];
                            var rutaArchivoDestino = rutaFolderTemp + "\\" + nombreArchivo;

                            if (File.Exists(rutaArchivoDestino))
                            {
                                rutaArchivoDestino = nuevoNombreArchivoConsecutivo(rutaArchivoDestino);
                            }

                            File.Copy(controlCalidad.archivoVidaAceites.Trim(), rutaArchivoDestino);
                        }
                    }
                }

                // Ya que esta la carpeta temporal creada, se crea el zip
                string rutaNuevoZip = Path.Combine(RutaTemp, nombreFolderTemp + ".zip");
                GlobalUtils.ComprimirCarpeta(rutaFolderTemp, rutaNuevoZip);

                // Una vez creado el zip, se elimina el folder temporal 
                // y se obtiene el stream de bytes del zip
                Directory.Delete(rutaFolderTemp, true);
                var zipStream = GlobalUtils.GetFileAsStream(rutaNuevoZip);

                // Una vez cargado el stream, se elimina el zip
                File.Delete(rutaNuevoZip);

                string nombreZip = String.Format("{0}_{1}_{2}.zip", asignacion.folio, asignacion.Economico, asignacion.fechaInicio.ToShortDateString().Replace("/", "-"));

                return Tuple.Create(zipStream, nombreZip);
            }
            catch (Exception e)
            {
                try
                {
                    Directory.Delete(rutaFolderTemp);
                }
                catch (Exception)
                {
                    LogError(0, 0, "ControlCalidadController", "descargarArchivosEliminarFolderTemp", e, AccionEnum.ELIMINAR, solicitudID, 0);
                }

                LogError(0, 0, "ControlCalidadController", "descargarArchivos", e, AccionEnum.DESCARGAR, solicitudID, 0);
                return null;
            }
        }

        private static string nuevoNombreArchivoConsecutivo(string rutaArchivo)
        {
            int count = 1;

            string fileNameOnly = Path.GetFileNameWithoutExtension(rutaArchivo);
            string extension = Path.GetExtension(rutaArchivo);
            string path = Path.GetDirectoryName(rutaArchivo);
            string newFullPath = rutaArchivo;

            while (File.Exists(newFullPath))
            {
                string tempFileName = string.Format("{0} ({1})", fileNameOnly, count++);
                newFullPath = Path.Combine(path, tempFileName + extension);
            }

            return newFullPath;
        }

        public bool asignacionContieneArchivos(int idAsignacion)
        {
            var listaControlCalidad = _context.tblM_CatControlCalidad.Where(x =>
                x.IdAsignacion == idAsignacion &&
                (
                    x.archivoSetFotografico.Trim() != "" ||
                    x.archivoRehabilitacion.Trim() != "" ||
                    x.archivoDN.Trim() != "" ||
                    x.archivoSOS.Trim() != "" ||
                    x.archivoBitacora.Trim() != "" ||
                    x.archivoCheckList.Trim() != "" ||
                    x.archivoVidaAceites.Trim() != ""
                )
            ).ToList();

            return listaControlCalidad.Count() > 0;
        }

        #region FULL COMBOS

        public Dictionary<string, object> GetCCs()
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();

            try
            {
                if (vSesiones.sesionEmpresaActual == 2)
                {
                    var query_ccs = new OdbcConsultaDTO();

                    var filtroCCs = "";
                    if (vSesiones.sesionUsuarioDTO.idPerfil == (int)PerfilUsuarioEnum.ADMINISTRADOR)
                    {
                        filtroCCs = "1 = 1";
                    }
                    if (vSesiones.sesionUsuarioDTO.idPerfil == (int)PerfilUsuarioEnum.USUARIO)
                    {
                        var usuarioCCs = _context.tblP_CC_Usuario.Where(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id).Select(x => x.cc).ToList();
                        if (usuarioCCs.Count > 0)
                        {
                            filtroCCs = string.Format("cc in {0}", usuarioCCs.ToParamInValue());
                            query_ccs.parametros.AddRange(usuarioCCs.Select(x => new OdbcParameterDTO
                            {
                                nombre = "cc",
                                tipo = OdbcType.NVarChar,
                                valor = x
                            }).ToList());
                        }
                        else
                        {
                            filtroCCs = "1 = 2";
                        }
                    }

                    query_ccs.consulta = string.Format(
                        @"SELECT
                        cc,
                        descripcion
                    FROM
                        cc
                    WHERE
                        {0}", filtroCCs);

                    var ccs = _contextEnkontrol.Select<ccDTO>(EnkontrolAmbienteEnum.ProdCPLAN, query_ccs).Select(x => new ComboDTO
                    {
                        Value = x.cc,
                        Text = "[" + x.cc + "] " + x.descripcion.Trim()
                    }).OrderBy(x => x.Value).ToList();

                    var _ccs = ccs.Select(x => x.Value).ToList();
                    var _ccsSIGOPLAN = _context.tblP_CC.Where(x => _ccs.Contains(x.cc) && x.areaCuenta != "0" && x.areaCuenta != "0-0").OrderBy(x => x.area).ThenBy(x => x.cuenta).ToList();
                    List<ComboDTO> lstCCs = new List<ComboDTO>();

                    foreach (var item in _ccsSIGOPLAN) 
                    {
                        ComboDTO _cc = new ComboDTO
                        {
                            Value = item.areaCuenta,
                            Text = "[" + item.areaCuenta + "] " + item.descripcion.Trim()
                        };
                        lstCCs.Add(_cc);
                    }
                    resultado.Add(ITEMS, lstCCs);
                }
                else
                {

                    var lstCCEmrpesas = _context.tblP_CC.Where(x => x.estatus).Select(x => new Core.DTO.Principal.Generales.ComboDTO
                    {
                        Value = x.cc,
                        Text = x.cc + " - " + x.descripcion
                    }).ToList();
                    resultado.Add(ITEMS, lstCCEmrpesas);
                }
               

                resultado.Add(SUCCESS, true);
                //resultado.Add(ITEMS, ccs);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, ex.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> GetEconomicos()
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();

            try
            {
                List<ComboDTO> lstEconomicos = new List<ComboDTO>();
                lstEconomicos = _context.tblM_CatMaquina.Select(s => new ComboDTO
                {
                    Value = s.id.ToString(),
                    Text = s.noEconomico.Trim()
                }).ToList();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstEconomicos);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        #endregion

    }
}
