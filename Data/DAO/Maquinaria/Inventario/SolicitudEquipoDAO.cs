using Core.DAO.Maquinaria.Inventario;
using Core.DAO.Maquinaria.Rastreo;
using Core.DTO;
using Core.DTO.Captura;
using Core.DTO.Maquinaria.Inventario;
using Core.DTO.Maquinaria.Rastreo;
using Core.Entity.Maquinaria.Inventario;
using Core.Entity.Principal.Usuarios;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.FileManager;
using Core.Enum.Multiempresa;

namespace Data.DAO.Maquinaria.Inventario
{
    public class SolicitudEquipoDAO : GenericDAO<tblM_SolicitudEquipo>, ISolicitudEquipoDAO
    {

        private readonly Dictionary<string, object> resultado = new Dictionary<string, object>();

        public void Guardar(tblM_SolicitudEquipo obj)
        {
            if (true)
            {
                if (obj.id == 0)
                    SaveEntity(obj, (int)BitacoraEnum.SOLICITUDEQUIPO);
                else
                    Update(obj, obj.id, (int)BitacoraEnum.SOLICITUDEQUIPO);
            }
            else
            {
                throw new Exception("Ya se genero un folio con ese consecutivo consultar con sistemas...");
            }

        }
        public void GuardarJustificaciones(int solicitudID,List<tblM_SM_Justificacion> array)
        {
            if (true)
            {
                _context.tblM_SM_Justificacion.AddRange(array);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Error ocurrio un error al insertar un registro");
            }

        }
        public void GuardarSolicitudEvidencia(int solicitudID, string dir)
        {
            if (true)
            {
                var data = _context.tblM_SolicitudEquipo.FirstOrDefault(x => x.id == solicitudID);
                data.link = dir;
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Error ocurrio un error al insertar un registro");
            }
        }
        public List<SolicitudEquipoJustificacionDTO> getListaJustificacionSolicitud(int idSolicitud)
        {

            var res = _context.tblM_SM_Justificacion.Where(x=>x.solicitudID == idSolicitud).ToList();

            var final = res.Select(x => new SolicitudEquipoJustificacionDTO
            {
                id=x.id,
                grupo = x.grupo,
                grupoID = x.grupoID,
                modelo = x.modelo,
                modeloID = x.modeloID,
                solicitudID = x.solicitudID,
                justificacion = x.justificacion
            });


            return final.ToList();
        }
        public List<tblM_SolicitudEquipo> getListPendientes(int idUsuario, int tipo)
        {

            List<tblM_AutorizacionSolicitudes> rawSolicitudes = new List<tblM_AutorizacionSolicitudes>();
            List<tblM_AutorizacionSolicitudes> ListaSolicitudes = new List<tblM_AutorizacionSolicitudes>();

            rawSolicitudes = _context.tblM_AutorizacionSolicitudes
                                          .Where(x => x.usuarioElaboro.Equals(idUsuario)
                                                || x.altaDireccion.Equals(idUsuario)
                                                || x.directorDivision.Equals(idUsuario)
                                                || x.gerenteObra.Equals(idUsuario)
                                                || x.GerenteDirector.Equals(idUsuario)
                                                || x.directorServicios.Equals(idUsuario)
                                                || idUsuario == 4 || idUsuario == 7 || idUsuario == 13
                                                )
                                                .ToList();
            List<int> SolicitudesPendientes = new List<int>();
            SolicitudesPendientes = getIDAutorizaciones(rawSolicitudes, idUsuario, tipo);

            var result = from c in _context.tblM_SolicitudEquipo
                         where SolicitudesPendientes.Contains(c.id)
                         select c;

            return result.ToList();
        }

        public string obtenerComentarioSolicitud(int solicitudID)
        {
            try
            {
                var comentario = from aut in _context.tblM_AutorizacionSolicitudes
                                 where solicitudID == aut.solicitudEquipoID
                                 select aut.observaciones.Trim();
                if (comentario.Count() > 0)
                {
                    return WebUtility.HtmlEncode(comentario.FirstOrDefault().Replace("\n", " "));
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        private List<int> getIDAutorizaciones(List<tblM_AutorizacionSolicitudes> rawSolicitudes, int idUsuario, int tipo)
        {

            List<int> ListaIdSolicitud = new List<int>();

            foreach (var item in rawSolicitudes)
            {
                if (item.solicitudEquipoID == 3367)
                {

                }
                if (item.usuarioElaboro == idUsuario)
                {
                    switch (tipo)
                    {
                        case 1:
                            if (item.cadenaFirmaDireccion == null || item.cadenaFirmaDirector == null || item.cadenaFirmaGerenteDirector == null || item.cadenaFirmaGerenteObra == null)
                            {
                                if (item.cadenaFirmaDireccion == null && item.cadenaFirmaDirector == null && item.cadenaFirmaGerenteDirector == null && item.cadenaFirmaGerenteObra == null)
                                {
                                    ListaIdSolicitud.Add(item.solicitudEquipoID);
                                }

                            }
                            break;
                        case 2:
                            if (item.firmaAltaDireccion == true && item.firmaDirectorDivision == true && item.firmaElaboro == true && item.firmaGerenteDirector == true && item.firmaGerenteObra && true)
                            {
                                ListaIdSolicitud.Add(item.solicitudEquipoID);
                            }
                            break;
                        case 3:
                            if (item.firmaAltaDireccion == false || item.firmaDirectorDivision == false || item.firmaGerenteDirector == false || item.firmaGerenteObra == false)
                            {
                                if (item.cadenaFirmaDireccion != null || item.cadenaFirmaDirector != null || item.cadenaFirmaGerenteDirector != null || item.cadenaFirmaGerenteObra != null)
                                {
                                    ListaIdSolicitud.Add(item.solicitudEquipoID);
                                }

                            }
                            break;
                        default:
                            break;
                    }

                }
                if (item.gerenteObra == idUsuario)
                {
                    switch (tipo)
                    {
                        case 1:
                            {
                                if (item.firmaElaboro.Equals(true) && item.firmaGerenteObra.Equals(false) && item.cadenaFirmaGerenteObra == null)
                                {
                                    ListaIdSolicitud.Add(item.solicitudEquipoID);
                                }
                            }
                            break;
                        case 2:
                            {
                                if (item.firmaElaboro.Equals(true) && item.firmaGerenteObra.Equals(true) && item.cadenaFirmaGerenteObra != null)
                                {
                                    ListaIdSolicitud.Add(item.solicitudEquipoID);
                                }
                            }
                            break;
                        case 3:
                            {
                                if (item.firmaGerenteObra.Equals(false) && item.cadenaFirmaGerenteObra != null)
                                {
                                    ListaIdSolicitud.Add(item.solicitudEquipoID);
                                }
                            }
                            break;
                        default:
                            break;
                    }

                }
                if (item.GerenteDirector == idUsuario)
                {
                    switch (tipo)
                    {
                        case 1:
                            {
                                if (GetBanderaAutorizadores(item.firmaGerenteObra, item.cadenaFirmaGerenteObra, 2) && GetBanderaAutorizadores(item.firmaGerenteDirector, item.cadenaFirmaGerenteDirector, 1))
                                {
                                    ListaIdSolicitud.Add(item.solicitudEquipoID);
                                }
                            }
                            break;
                        case 2:
                            {
                                if (item.firmaElaboro.Equals(true) && item.firmaGerenteObra.Equals(true) && item.cadenaFirmaGerenteObra != null && item.firmaGerenteDirector.Equals(true) && item.cadenaFirmaGerenteDirector != null)
                                {
                                    ListaIdSolicitud.Add(item.solicitudEquipoID);
                                }
                            }
                            break;
                        case 3:
                            {
                                if (item.firmaGerenteObra.Equals(false) && item.cadenaFirmaGerenteObra != null || item.firmaGerenteObra.Equals(false) && item.cadenaFirmaGerenteObra != null)
                                {
                                    ListaIdSolicitud.Add(item.solicitudEquipoID);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
                if (item.directorDivision == idUsuario)
                {
                    switch (tipo)
                    {
                        case 1:
                            {
                                if (GetBanderaAutorizadores(item.firmaGerenteObra, item.cadenaFirmaGerenteObra, 2) && GetBanderaAutorizadores(item.firmaGerenteDirector, item.cadenaFirmaGerenteDirector, 2) && GetBanderaAutorizadores(item.firmaDirectorDivision, item.cadenaFirmaDirector, 1))
                                {
                                    ListaIdSolicitud.Add(item.solicitudEquipoID);
                                }
                            }
                            break;
                        case 2:
                            {
                                if (GetBanderaAutorizadores(item.firmaGerenteObra, item.cadenaFirmaGerenteObra, 2) && GetBanderaAutorizadores(item.firmaGerenteDirector, item.cadenaFirmaGerenteDirector, 2) && GetBanderaAutorizadores(item.firmaDirectorDivision, item.cadenaFirmaDirector, 2))
                                {
                                    ListaIdSolicitud.Add(item.solicitudEquipoID);
                                }
                            }
                            break;
                        case 3:
                            {
                                if (GetBanderaAutorizadores(item.firmaGerenteObra, item.cadenaFirmaGerenteObra, 3) || GetBanderaAutorizadores(item.firmaGerenteDirector, item.cadenaFirmaGerenteDirector, 3) || GetBanderaAutorizadores(item.firmaDirectorDivision, item.cadenaFirmaDirector, 3))
                                {
                                    ListaIdSolicitud.Add(item.solicitudEquipoID);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
                if (item.directorServicios == idUsuario)
                {
                    switch (tipo)
                    {
                        case 1:
                            {
                                if (GetBanderaAutorizadores(item.firmaGerenteObra, item.cadenaFirmaGerenteObra, 2) && GetBanderaAutorizadores(item.firmaGerenteDirector, item.cadenaFirmaGerenteDirector, 2) && GetBanderaAutorizadores(item.firmaDirectorDivision, item.cadenaFirmaDirector, 2) && GetBanderaAutorizadores(item.firmaServicios, item.cadenaFirmaServicios, 1))
                                {
                                    ListaIdSolicitud.Add(item.solicitudEquipoID);
                                }
                            }
                            break;
                        case 2:
                            {
                                if (GetBanderaAutorizadores(item.firmaGerenteObra, item.cadenaFirmaGerenteObra, 2) && GetBanderaAutorizadores(item.firmaGerenteDirector, item.cadenaFirmaGerenteDirector, 2) && GetBanderaAutorizadores(item.firmaDirectorDivision, item.cadenaFirmaDirector, 2) && GetBanderaAutorizadores(item.firmaServicios, item.cadenaFirmaServicios, 2))
                                {
                                    ListaIdSolicitud.Add(item.solicitudEquipoID);
                                }
                            }
                            break;
                        case 3:
                            {
                                if (GetBanderaAutorizadores(item.firmaGerenteObra, item.cadenaFirmaGerenteObra, 3) || GetBanderaAutorizadores(item.firmaGerenteDirector, item.cadenaFirmaGerenteDirector, 3) || GetBanderaAutorizadores(item.firmaDirectorDivision, item.cadenaFirmaDirector, 3) && GetBanderaAutorizadores(item.firmaServicios, item.cadenaFirmaServicios, 3))
                                {
                                    ListaIdSolicitud.Add(item.solicitudEquipoID);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
                if (item.altaDireccion == idUsuario)
                {
                    switch (tipo)
                    {
                        case 1:
                            {
                                if (GetBanderaAutorizadores(item.firmaGerenteObra, item.cadenaFirmaGerenteObra, 2) && GetBanderaAutorizadores(item.firmaGerenteDirector, item.cadenaFirmaGerenteDirector, 2) && GetBanderaAutorizadores(item.firmaDirectorDivision, item.cadenaFirmaDirector, 2) && GetBanderaAutorizadores(item.firmaServicios, item.cadenaFirmaServicios, 2) && GetBanderaAutorizadores(item.firmaAltaDireccion, item.cadenaFirmaDireccion, 1))
                                {
                                    ListaIdSolicitud.Add(item.solicitudEquipoID);
                                }
                            }
                            break;
                        case 2:
                            {
                                if (GetBanderaAutorizadores(item.firmaGerenteObra, item.cadenaFirmaGerenteObra, 2) && GetBanderaAutorizadores(item.firmaGerenteDirector, item.cadenaFirmaGerenteDirector, 2) && GetBanderaAutorizadores(item.firmaDirectorDivision, item.cadenaFirmaDirector, 2) && GetBanderaAutorizadores(item.firmaServicios, item.cadenaFirmaServicios, 2) && GetBanderaAutorizadores(item.firmaAltaDireccion, item.cadenaFirmaDireccion, 2))
                                {
                                    ListaIdSolicitud.Add(item.solicitudEquipoID);
                                }
                            }
                            break;
                        case 3:
                            {
                                if (GetBanderaAutorizadores(item.firmaGerenteObra, item.cadenaFirmaGerenteObra, 3) || GetBanderaAutorizadores(item.firmaGerenteDirector, item.cadenaFirmaGerenteDirector, 3) || GetBanderaAutorizadores(item.firmaDirectorDivision, item.cadenaFirmaDirector, 3) && GetBanderaAutorizadores(item.firmaServicios, item.cadenaFirmaServicios, 3) || GetBanderaAutorizadores(item.firmaAltaDireccion, item.cadenaFirmaDireccion, 3))
                                {
                                    ListaIdSolicitud.Add(item.solicitudEquipoID);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }

            }

            return ListaIdSolicitud;
        }

        private bool GetBanderaAutorizadores(bool firma, string cadena, int tipo)
        {
            switch (tipo)
            {
                case 1:// Si Firma Pendiente
                    {
                        if (!firma && cadena == null)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                case 2:// Si ya firmo y es Aceptada
                    {
                        if (firma && cadena != null)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                case 3:// Si ya firmo y es rechazada
                    {
                        if (!firma && cadena != null)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                default:
                    return false;
            }
        }

        public string GetFolio(string cc)
        {
            int result = _context.tblM_SolicitudEquipo.Where(x => x.CC.Equals(cc)).Count();
            return (result + 1).ToString();

        }

        private int tipoAutorizador(tblM_AutorizacionSolicitudes obj, int idUsuario)
        {
            if (obj.usuarioElaboro.Equals(idUsuario))
            {
                return obj.solicitudEquipoID;
            }

            if (obj.gerenteObra.Equals(idUsuario))
            {

                if (obj.firmaElaboro.Equals(true) && obj.firmaGerenteObra.Equals(false))
                {
                    return obj.solicitudEquipoID;
                }
            }
            if (obj.GerenteDirector.Equals(idUsuario))
            {

                if (obj.firmaElaboro.Equals(true) && obj.firmaGerenteObra.Equals(true) && obj.firmaGerenteDirector.Equals(false))
                {
                    return obj.solicitudEquipoID;
                }
            }
            if (obj.directorDivision.Equals(idUsuario))
            {
                if (obj.firmaElaboro.Equals(true) && obj.firmaGerenteObra.Equals(true) && obj.firmaDirectorDivision.Equals(false))
                {
                    return obj.solicitudEquipoID;
                }
            }
            if (obj.altaDireccion.Equals(idUsuario))
            {
                if (obj.firmaElaboro.Equals(true) && obj.firmaGerenteObra.Equals(true) && obj.firmaDirectorDivision.Equals(true) && obj.firmaAltaDireccion.Equals(false))
                {
                    return obj.solicitudEquipoID;
                }
            }
            return 0;
        }

        public List<SolicitudEquipoDTO> getListaDetalleSolicitud(int idSolicitud)
        {

            var res = (from sd in _context.tblM_SolicitudEquipoDet
                       join s in _context.tblM_SolicitudEquipo on sd.solicitudEquipoID equals s.id
                       where s.id == idSolicitud
                       select new { sd, s }).ToList();

            var final = res.Select(x => new SolicitudEquipoDTO
            {
                Descripcion = string.IsNullOrWhiteSpace(x.s.descripcion) ? "" : x.s.descripcion,
                Folio = x.s.folio,
                Tipo = x.sd.TipoMaquinaria.descripcion,
                TipoId = x.sd.tipoMaquinariaID,
                Grupo = x.sd.GrupoMaquinaria.descripcion,
                Grupoid = x.sd.grupoMaquinariaID,
                Modelo = x.sd.ModeloEquipo.descripcion,
                Modeloid = x.sd.modeloEquipoID,
                pCapacidad = 0,
                pFechaInicio = x.sd.fechaInicio.ToString("dd/MM/yyyy"),
                pFechaFin = x.sd.fechaFin.ToString("dd/MM/yyyy"),
                pHoras = x.sd.horas,
                pTipoPrioridad = getPrioridad(x.sd.prioridad),// == 0 ? "Programada" : "Urgente",
                id = x.sd.id,
                estatus = x.sd.estatus,
                Comentario = x.sd.Comentario

            });


            return final.ToList();
        }

        public List<SolicitudEquipoDTO> getListaDetalleSolicitudAutorizacion(int idSolicitud)
        {

            var res = (from sd in _context.tblM_SolicitudEquipoDet
                       join s in _context.tblM_SolicitudEquipo on sd.solicitudEquipoID equals s.id
                       where s.id == idSolicitud
                       select new { sd, s }).ToList();

            var final = res.Select(x => new SolicitudEquipoDTO
            {
                Descripcion = string.IsNullOrWhiteSpace(x.sd.Comentario) ? "" : x.sd.Comentario,
                Folio = x.s.folio,
                Tipo = x.sd.TipoMaquinaria.descripcion,
                TipoId = x.sd.tipoMaquinariaID,
                Grupo = x.sd.GrupoMaquinaria.descripcion,
                Grupoid = x.sd.grupoMaquinariaID,
                Modelo = x.sd.ModeloEquipo.descripcion,
                Modeloid = x.sd.modeloEquipoID,
                pCapacidad = 0,
                pFechaInicio = x.sd.fechaInicio.ToString("dd/MM/yyyy"),
                pFechaFin = x.sd.fechaFin.ToString("dd/MM/yyyy"),
                pHoras = x.sd.horas,
                pTipoPrioridad = getPrioridad(x.sd.prioridad),// == 0 ? "Programada" : "Urgente",
                id = x.sd.id, // idSolicitudDetalle
                SolicitudDetalleId = x.s.id, //idSolicitud
                condicionInicial = x.s.condicionInicial,
                condicionActual = x.s.condicionActual,
                justificacion = x.s.justificacion,
                link = x.s.link
            });


            return final.ToList();
        }
        private string getPrioridad(int obj)
        {
            switch (obj)
            {
                case 0:
                    return "Programada";
                case 1:
                    return "Urgente";
                case 2:
                    return "Normal";
                default:
                    break;
            }
            return "";
        }
        public List<tblM_SolicitudEquipo> getListAutorizadas(int idUsuario)
        {
            var result = from c in _context.tblM_SolicitudEquipo
                         join
                               a in _context.tblM_AutorizacionSolicitudes on
                               c.id equals a.solicitudEquipoID
                         where a.firmaAltaDireccion.Equals(true) &&
                               a.firmaDirectorDivision.Equals(true) &&
                               a.firmaElaboro.Equals(true) &&
                               a.firmaGerenteObra.Equals(true) &&
                               c.Estatus.Equals(false) || c.ArranqueObra
                         select c;

            return result.ToList();
        }
        public List<tblM_SolicitudEquipo> GetListaSolicitudesAutorizadas(int idUsuario)
        {
            var result = from c in _context.tblM_SolicitudEquipo
                         join
                               a in _context.tblM_AutorizacionSolicitudes on
                               c.id equals a.solicitudEquipoID
                         where a.firmaAltaDireccion.Equals(true) &&
                               a.firmaDirectorDivision.Equals(true) &&
                               a.firmaElaboro.Equals(true) &&
                               a.firmaGerenteObra.Equals(true) || c.ArranqueObra == true
                         select c;

            return result.ToList();
        }
        public List<SolicitudDetalleDTO> getListaSolicitudesPendientes(int idSolicitud)
        {

            var res = (from sd in _context.tblM_SolicitudEquipoDet
                       join s in _context.tblM_SolicitudEquipo on sd.solicitudEquipoID equals s.id
                       where s.id == idSolicitud && sd.estatus.Equals(false)
                       select new { sd, s }).ToList();

            var final = res.Select(x => new SolicitudDetalleDTO
            {
                id = x.sd.id,
                FechaInicio = x.sd.fechaInicio,
                FechaFin = x.sd.fechaFin,
                idTipoPrioridad = x.sd.prioridad,
                TipoId = x.sd.tipoMaquinariaID,
                GrupoId = x.sd.grupoMaquinariaID,
                Modeloid = x.sd.modeloEquipoID,
                Comentario = x.sd.Comentario,
                Tipo = x.sd.TipoMaquinaria.descripcion,
                Grupo = x.sd.GrupoMaquinaria.descripcion,
                Modelo = x.sd.ModeloEquipo.descripcion,
                pHoras = x.sd.horas,
                pFechaInicio = x.sd.fechaInicio.ToString("dd/MM/yyyy"),
                pFechaFin = x.sd.fechaFin.ToString("dd/MM/yyyy"),
                tipoUtilizacion = x.sd.tipoUtilizacion

            });
            return final.ToList();

        }
        public tblM_SolicitudEquipo listaSolicitudEquipo(int idSolicitud)
        {
            return _context.tblM_SolicitudEquipo.FirstOrDefault(x => x.id.Equals(idSolicitud));
        }

        public tblM_SolicitudEquipoDet GetSolicitudEquipoDet(int obj)
        {
            return _context.tblM_SolicitudEquipoDet.FirstOrDefault(x => x.id.Equals(obj));
        }
        public tblM_SolicitudEquipo LoadDataSolicitud(string Folio)
        {
            return _context.tblM_SolicitudEquipo.FirstOrDefault(x => x.folio.Equals(Folio) && x.Estatus == false);
        }
        public tblM_SolicitudEquipo loadSolicitudById(int id)
        {
            return _context.tblM_SolicitudEquipo.FirstOrDefault(x => x.id.Equals(id));
        }
        public List<tblP_Autoriza> GetAutorizadores(string cc)
        {
            var resultado = from a in _context.tblP_Autoriza
                            join pa in _context.tblP_PerfilAutoriza on a.perfilAutorizaID equals pa.id
                            join ccU in _context.tblP_CC_Usuario on a.cc_usuario_ID equals ccU.id
                            where (!string.IsNullOrEmpty(cc) ? ccU.cc == cc : a.cc_usuario_ID == ccU.id)
                            select a;

            return resultado.ToList();
        }
        public List<SolicitudesDetalleVacantesDTO> GetSolicitudesEquipo(string CentroCostos)
        {
            string nombreCC = "";


            //string centro_costos = "SELECT descripcion FROM cc WHERE cc = '" + CentroCostos + "';";

            //var resultado = (IList<economicoDTO>)_contextEnkontrol.Where(centro_costos).ToObject<IList<economicoDTO>>();
            //if (resultado.Count > 0)
            //{
            //    nombreCC = resultado.FirstOrDefault().descripcion;
            //}

            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
            {
                case EmpresaEnum.Peru:
                    {
                        var cc = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == CentroCostos);
                        if (cc != null)
                        {
                            nombreCC = cc.descripcion.Trim();
                        }
                    }
                    break;
                default:
                    {
                        nombreCC = _context.tblP_CC.FirstOrDefault(e => e.cc == CentroCostos).descripcion;
                    }
                    break;
            }

            List<SolicitudesDetalleVacantesDTO> listObjVacantes = new List<SolicitudesDetalleVacantesDTO>();

            var Resultado = _context.tblM_AutorizacionSolicitudes.Where(x => x.SolicitudEquipo.CC == CentroCostos).Select(x => x.SolicitudEquipo).ToList();

            foreach (var Solicitudes in Resultado)
            {
                SolicitudesDetalleVacantesDTO SingleObjVacantes = new SolicitudesDetalleVacantesDTO();

                var SolicitudesDet = _context.tblM_SolicitudEquipoDet.Where(x => x.solicitudEquipoID == Solicitudes.id);
                var SolicitudesOcupadas = SolicitudesDet.Where(x => x.estatus == true).Select(x => x.id).ToList();
                var SolicitudesLibres = SolicitudesDet.Where(x => x.estatus == false).Select(x => x.id).ToList();
                var AsignacionesRealizadas = _context.tblM_AsignacionEquipos.Where(x => x.solicitudEquipoID == Solicitudes.id && SolicitudesOcupadas.Contains(x.SolicitudDetalleId)).ToList();
                var TotalesOcupadas = AsignacionesRealizadas.Where(x => x.estatus != 10).Select(x => x.SolicitudDetalleId).Distinct();
                var EquiposNoUtilizados = _context.tblM_AsignacionEquipos.Where(x => x.solicitudEquipoID == Solicitudes.id && x.estatus == 10).Select(x => x.SolicitudDetalleId).Distinct().ToList();
                var Cantidad = EquiposNoUtilizados.Where(x => !TotalesOcupadas.Contains(x)).Distinct().Count();

                SingleObjVacantes.CentroCostos = nombreCC;
                SingleObjVacantes.noSolicitudes = Solicitudes.id;
                SingleObjVacantes.Folio = Solicitudes.folio;
                SingleObjVacantes.TotalDeVacantes = SolicitudesDet.Count();
                SingleObjVacantes.TotalOcupadas = TotalesOcupadas.Count();
                SingleObjVacantes.TotalLibres = Cantidad + SolicitudesLibres.Count(); // 20;//AsignacionesRealizadas.Where(x => x.estatus == 10 && !TotalesOcupadas.Contains(x.SolicitudDetalleId)).Select(x => x.SolicitudDetalleId).Distinct().Count() + SolicitudesDet.Where(s => !s.estatus).Count();


                listObjVacantes.Add(SingleObjVacantes);
            }

            return listObjVacantes;
        }
        private string GetEconomico(int id)
        {
            string Economico = "";

            var Maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == id);

            if (Maquina != null)
            {
                return Maquina.noEconomico;
            }
            else
            {
                return "";
            }
        }
        public List<DetalleVacantesSolicitudDTO> GetDetalleSolicitud(int solicitudes)
        {
            List<DetalleVacantesSolicitudDTO> solicitudesDetalleVacantesDTOList = new List<DetalleVacantesSolicitudDTO>();

            var SolicitudDet = _context.tblM_SolicitudEquipoDet.Where(x => x.solicitudEquipoID == solicitudes).ToList();

            var Grupo = SolicitudDet.GroupBy(x => new { TipoID = x.TipoMaquinaria.id, GrupoID = x.GrupoMaquinaria.id, ModeloID = x.modeloEquipoID }).
                                      Select(x => new { Grupo = x.Key.GrupoID, Tipo = x.Key.TipoID, Cantidad = x.Count(), Modelo = x.Key.ModeloID });

            var SolicitudDetTotal = SolicitudDet.Count();

            var GrupoSolcitudDet = _context.tblM_AsignacionEquipos.Where(x => x.solicitudEquipoID == solicitudes).Select(x => x.SolicitudDetalleId).ToList();

            foreach (var item in Grupo)
            {
                DetalleVacantesSolicitudDTO solicitudesDetalleVacantesDTO = new DetalleVacantesSolicitudDTO();

                var SolicitudesGrupo = SolicitudDet.Where(x => x.grupoMaquinariaID == item.Grupo).ToList();

                solicitudesDetalleVacantesDTO.Grupo = _context.tblM_CatGrupoMaquinaria.FirstOrDefault(x => x.id.Equals(item.Grupo)).descripcion;
                solicitudesDetalleVacantesDTO.Tipo = _context.tblM_CatTipoMaquinaria.FirstOrDefault(x => x.id.Equals(item.Tipo)).descripcion;
                solicitudesDetalleVacantesDTO.Modelo = _context.tblM_CatModeloEquipo.FirstOrDefault(x => x.id.Equals(item.Modelo)).descripcion;
                int counter = 0;

                foreach (var sd in SolicitudesGrupo)
                {

                    var sinAsignar = _context.tblM_SolicitudEquipoDet.Where(x => x.id == sd.id && !x.estatus).FirstOrDefault();

                    if (sinAsignar != null)
                    {
                        counter++;

                    }
                    else
                    {
                        var ListaAsignados = _context.tblM_AsignacionEquipos.Where(x => x.SolicitudDetalleId == sd.id);

                        if (ListaAsignados.Where(x => x.estatus != 10).Count() == 0)
                        {
                            counter++;
                        }
                    }

                }

                if (counter > 0)
                {
                    solicitudesDetalleVacantesDTO.CantidadSolicitudes = item.Cantidad - counter;
                }
                else
                {
                    solicitudesDetalleVacantesDTO.CantidadSolicitudes = item.Cantidad;
                }
                solicitudesDetalleVacantesDTO.CantidadVacantes = counter;
                solicitudesDetalleVacantesDTO.Solicitud = solicitudes;
                solicitudesDetalleVacantesDTO.IDGrupo = item.Grupo;
                solicitudesDetalleVacantesDTOList.Add(solicitudesDetalleVacantesDTO);

            }

            return solicitudesDetalleVacantesDTOList;
        }
        public List<DetalleGrupoPlantillaDTO> GetDataSolicitudesGrupo(int solicitudes, int grupo)
        {
            List<DetalleGrupoPlantillaDTO> Data = new List<DetalleGrupoPlantillaDTO>();

            var result = _context.tblM_SolicitudEquipoDet.Where(x => x.grupoMaquinariaID == grupo && x.solicitudEquipoID == solicitudes).ToList();

            foreach (var item in result)
            {
                DetalleGrupoPlantillaDTO dto = new DetalleGrupoPlantillaDTO();

                var objGrupo = _context.tblM_CatGrupoMaquinaria.FirstOrDefault(x => item.grupoMaquinariaID == x.id);
                dto.Descripcion = objGrupo.descripcion;
                dto.Tipo = objGrupo.tipoEquipo.descripcion;

                var Asignacion = _context.tblM_AsignacionEquipos.FirstOrDefault(x => x.SolicitudDetalleId == item.id && x.estatus != 10);

                if (Asignacion != null)
                {
                    string Economico = "";
                    if (Asignacion.noEconomicoID != 0)
                    {
                        var Maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == Asignacion.noEconomicoID);

                        dto.Economico = Maquina.noEconomico;
                    }
                }

                Data.Add(dto);
            }

            return Data;
        }
        public List<tblM_SolicitudEquipo> GetEquipoNoMovimientos()
        {
            var SolicitudesAsignadas = _context.tblM_AsignacionEquipos.Select(x => x.solicitudEquipoID).Distinct().ToList();

            var Regresar = (from a in _context.tblM_SolicitudEquipo
                            where !SolicitudesAsignadas.Contains(a.id) && a.EstatdoSolicitud
                            select a).ToList();

            return Regresar;
        }
        public List<tblM_AutorizacionSolicitudes> getTiemposAsignacion(List<string> CentroCostos)
        {

            if (CentroCostos != null)
            {
                var ListaReporteAsignacion = from x in _context.tblM_AutorizacionSolicitudes
                                             where x.cadenaFirmaDireccion != null
                                                 && x.cadenaFirmaDirector != null
                                                 && x.cadenaFirmaElabora != null
                                                 && x.cadenaFirmaGerenteDirector != null
                                                 && x.cadenaFirmaGerenteObra != null
                                                 && CentroCostos.Contains(x.SolicitudEquipo.CC)
                                                 && x.altaDireccion != 13
                                             select x;
                return ListaReporteAsignacion.ToList();
            }
            else
            {
                return null;
            }

        }
        public List<EquiposPendientesDTO> getEquiposPendientes(string cc, int Tipo, int grupo)
        {
            List<EquiposPendientesDTO> respuesta = new List<EquiposPendientesDTO>();
            List<int> dataList = new List<int>();

            var data1 = _context.tblM_SolicitudEquipoDet.Where(x => x.estatus == false && (string.IsNullOrEmpty(cc) ? x.id == x.id : x.SolicitudEquipo.CC == cc) && (grupo != 0 ? x.grupoMaquinariaID == grupo : x.id == x.id)).Select(x => x.id).ToList();

            var data = (from sd in _context.tblM_SolicitudEquipoDet
                        join a in _context.tblM_AsignacionEquipos on sd.id equals a.SolicitudDetalleId
                        where a.estatus < 3 && (string.IsNullOrEmpty(cc) ? sd.id == sd.id : sd.SolicitudEquipo.CC == cc)

                         && (grupo != 0 ? sd.grupoMaquinariaID == grupo : sd.id == sd.id)
                        select sd.id).ToList();

            dataList.AddRange(data1);
            dataList.AddRange(data);

            foreach (var item in dataList.Distinct())
            {
                EquiposPendientesDTO equiposPendientesDTO = new EquiposPendientesDTO();
                var asignacion = _context.tblM_AsignacionEquipos.Where(x => x.SolicitudDetalleId == item && x.estatus < 3).OrderByDescending(x => x.id).FirstOrDefault();



                if (asignacion != null)
                {
                    var SolicitudDetalle = _context.tblM_SolicitudEquipoDet.FirstOrDefault(x => x.id == asignacion.SolicitudDetalleId);

                    equiposPendientesDTO.ProyectoID = SolicitudDetalle.SolicitudEquipo.CC;
                    equiposPendientesDTO.Proyecto = regresarCentroCostos(asignacion.cc);
                    equiposPendientesDTO.GrupoEquipo = SolicitudDetalle.GrupoMaquinaria.descripcion;
                    equiposPendientesDTO.Modelo = SolicitudDetalle.ModeloEquipo.descripcion;
                    equiposPendientesDTO.Prioridad = SolicitudDetalle.tipoUtilizacion.ToString();
                    equiposPendientesDTO.EquipoPropio = asignacion.noEconomicoID != 0 ? asignacion.Economico : "";
                    equiposPendientesDTO.EquipoRenta = asignacion.noEconomicoID == 0 ? asignacion.Economico : "";
                    equiposPendientesDTO.FechaSolicitud = asignacion.SolicitudEquipo.fechaElaboracion.ToShortDateString();
                    equiposPendientesDTO.FechaRequerida = asignacion.FechaPromesa.ToShortDateString();
                    equiposPendientesDTO.Estatus = DescripcionStatus(asignacion.noEconomicoID != 0 ? asignacion.estatus : 111);
                }
                else
                {
                    var SolicitudDetalle = _context.tblM_SolicitudEquipoDet.FirstOrDefault(x => x.id == item);
                    equiposPendientesDTO.ProyectoID = SolicitudDetalle.SolicitudEquipo.CC;
                    equiposPendientesDTO.Proyecto = regresarCentroCostos(SolicitudDetalle.SolicitudEquipo.CC);
                    equiposPendientesDTO.GrupoEquipo = SolicitudDetalle.GrupoMaquinaria.descripcion;
                    equiposPendientesDTO.Modelo = SolicitudDetalle.ModeloEquipo.descripcion;
                    equiposPendientesDTO.Prioridad = getTipoP(SolicitudDetalle.tipoUtilizacion);
                    equiposPendientesDTO.EquipoPropio = "";
                    equiposPendientesDTO.EquipoRenta = "";
                    equiposPendientesDTO.FechaSolicitud = SolicitudDetalle.SolicitudEquipo.fechaElaboracion.ToShortDateString();
                    equiposPendientesDTO.FechaRequerida = "";
                    equiposPendientesDTO.Estatus = DescripcionStatus(201);
                }

                equiposPendientesDTO.Cantidad = "1";

                respuesta.Add(equiposPendientesDTO);

            }

            return respuesta.OrderBy(x => x.ProyectoID).ToList();
        }

        public List<EquiposPendientesReemplazoDTO> getEquiposReemplazo(string cc, int Tipo, int grupo)
        {
            List<EquiposPendientesReemplazoDTO> respuesta = new List<EquiposPendientesReemplazoDTO>();
            List<int> dataList = new List<int>();

            var data1 = _context.tblM_SolicitudReemplazoDet.Where(x => x.estatus == 0 && (string.IsNullOrEmpty(cc) ? x.id == x.id : x.AsignacionEquipos.SolicitudEquipo.CC == cc)).Select(x => x.AsignacionEquiposID).ToList();

            var data = (from sd in _context.tblM_AsignacionEquipos
                        join a in _context.tblM_SolicitudReemplazoDet on sd.id equals a.AsignacionEquiposID
                        where sd.estatus < 3 && (string.IsNullOrEmpty(cc) ? sd.id == sd.id : sd.SolicitudEquipo.CC == cc)
                        select sd.id).ToList();

            dataList.AddRange(data1);
            dataList.AddRange(data);

            foreach (var item in dataList.Distinct())
            {
                EquiposPendientesReemplazoDTO equiposPendientesDTO = new EquiposPendientesReemplazoDTO();
                var asignacion = _context.tblM_AsignacionEquipos.Where(x => x.id == item && x.estatus < 3).OrderByDescending(x => x.id).FirstOrDefault();

                if (asignacion != null)
                {
                    var SolicitudDetalleRemp = _context.tblM_SolicitudReemplazoDet.FirstOrDefault(x => x.AsignacionEquiposID == asignacion.id);
                    var SolicitudDetalle = _context.tblM_SolicitudEquipoDet.FirstOrDefault(x => x.id == asignacion.SolicitudDetalleId);

                    equiposPendientesDTO.ProyectoID = SolicitudDetalle.SolicitudEquipo.CC;
                    equiposPendientesDTO.Proyecto = regresarCentroCostos(asignacion.cc);
                    equiposPendientesDTO.GrupoEquipo = SolicitudDetalle.GrupoMaquinaria.descripcion;
                    equiposPendientesDTO.Modelo = SolicitudDetalle.ModeloEquipo.descripcion;
                    equiposPendientesDTO.Prioridad = SolicitudDetalle.tipoUtilizacion.ToString();
                    equiposPendientesDTO.EquipoPropio = asignacion.noEconomicoID != 0 ? asignacion.Economico : "";
                    equiposPendientesDTO.EquipoRenta = asignacion.noEconomicoID == 0 ? asignacion.Economico : "";
                    equiposPendientesDTO.FechaSolicitud = asignacion.SolicitudEquipo.fechaElaboracion.ToShortDateString();
                    equiposPendientesDTO.FechaRequerida = asignacion.FechaPromesa.ToShortDateString();
                    equiposPendientesDTO.Estatus = DescripcionStatus(asignacion.noEconomicoID != 0 ? asignacion.estatus : 111);
                    respuesta.Add(equiposPendientesDTO);
                }
                else
                {

                    var AsignacionDet = _context.tblM_AsignacionEquipos.Where(x => x.id == item).OrderByDescending(x => x.id).FirstOrDefault();
                    var SolicitudDetalle = _context.tblM_SolicitudEquipoDet.FirstOrDefault(x => x.id == AsignacionDet.SolicitudDetalleId);

                    equiposPendientesDTO.ProyectoID = SolicitudDetalle.SolicitudEquipo.CC;
                    equiposPendientesDTO.Proyecto = regresarCentroCostos(SolicitudDetalle.SolicitudEquipo.CC);
                    equiposPendientesDTO.GrupoEquipo = SolicitudDetalle.GrupoMaquinaria.descripcion;
                    equiposPendientesDTO.Modelo = SolicitudDetalle.ModeloEquipo.descripcion;
                    equiposPendientesDTO.Prioridad = getTipoP(SolicitudDetalle.tipoUtilizacion);
                    equiposPendientesDTO.EquipoPropio = "";
                    equiposPendientesDTO.EquipoRenta = "";
                    equiposPendientesDTO.FechaSolicitud = SolicitudDetalle.SolicitudEquipo.fechaElaboracion.ToShortDateString();
                    equiposPendientesDTO.FechaRequerida = "";
                    equiposPendientesDTO.Estatus = DescripcionStatus(201);
                    equiposPendientesDTO.economicoAsignado = AsignacionDet.Economico;
                    respuesta.Add(equiposPendientesDTO);
                }
            }

            return respuesta.OrderBy(x => x.ProyectoID).ToList();

        }

        private string DescripcionStatus(int estatus)
        {

            switch (estatus)
            {
                case 1:
                    return "Asignado En Proceso de envío";
                case 2:
                    return "En transito a Obra";
                case 111:
                    return "Proceso de Compra o Renta Equipo";
                case 201:
                    return "Pendiente de asignación";
                default:
                    return "";
                    break;
            }
        }

        private string getTipoP(int estatus)
        {

            switch (estatus)
            {
                case 1:
                    return "A";
                case 2:
                    return "B";
                case 3:
                    return "C";
                default:
                    return "";
                    break;
            }
        }

        //private string regresarCentroCostos(string centro_costos)
        //{
        //    string centro_costosName = "";


        //    try
        //    {
        //        string centro_costosC = "SELECT descripcion FROM cc WHERE cc = '" + centro_costos + "';";

        //        var resultado = (IList<economicoDTO>)_contextEnkontrol.Where(centro_costosC).ToObject<IList<economicoDTO>>();
        //        centro_costosName = resultado.FirstOrDefault().descripcion;

        //        return centro_costosName;
        //    }
        //    catch (Exception)
        //    {

        //        return "";
        //    }

        //}

        private string regresarCentroCostos(string centro_costosName)
        {
            if (vSesiones.sesionEmpresaActual != 1)
            {
                var _CCEmpresa = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == centro_costosName);
                if (_CCEmpresa != null)
                {
                    centro_costosName = _CCEmpresa.descripcion;
                }
                else
                {
                    var _CCEmpresa2 = _context.tblP_CC.FirstOrDefault(x => x.cc == centro_costosName);
                    if (_CCEmpresa2 != null)
                    {
                        centro_costosName = _CCEmpresa.descripcion;
                    }
                }
            }
            else
            {
                var _CCEmpresa = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == centro_costosName);
                if (_CCEmpresa != null)
                {
                    centro_costosName = _CCEmpresa.descripcion;
                }
                else
                {
                    var resultado = (IList<economicoDTO>)ContextEnKontrolNominaArrendadora.Where("SELECT descripcion FROM cc WHERE cc = '" + centro_costosName + "';", 1).ToObject<IList<economicoDTO>>();
                    return resultado.FirstOrDefault().descripcion;
                }
            }

            return centro_costosName;

        }

        public List<tblM_AutorizacionSolicitudes> GetListSolicitudesCC(List<string> centro_costos)
        {
            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
            {
                case EmpresaEnum.Colombia:
                    {
                        var areasCuentas = _context.tblP_CC.Where(x => centro_costos.Contains(x.cc)).Select(x => x.areaCuenta).ToList();
                        centro_costos.AddRange(areasCuentas);
                    }
                    break;
            }
            var data = (from a in _context.tblM_AutorizacionSolicitudes
                        where centro_costos.Contains(a.SolicitudEquipo.CC)
                        select a).ToList();

            return data;
        }

        //Envio de Documento a Gestor

        public bool insertEnvioGestor(int solicitudID)
        {
            try
            {
                var empresa = vSesiones.sesionEmpresaActual;
                var solicitud = _context.tblM_SolicitudEquipo.FirstOrDefault(x => x.id == solicitudID);
                var CC = _context.tblP_CC.FirstOrDefault(x => x.cc == solicitud.CC);
                using (var ctx = new MainContext((int)EmpresaEnum.Construplan))
                {
                    tblFM_EnvioDocumento documento = new tblFM_EnvioDocumento();
                    documento.id = 0;
                    documento.documentoID = solicitudID;
                    documento.descripcion = empresa == 2 ? ("(" + solicitud.folio + ")-" + solicitud.CC) : ("(" + solicitud.folio + ")-" + CC.descripcion);
                    documento.tipoDocumento = 17;
                    documento.usuarioID = solicitud.usuarioID;
                    documento.estatus = 0;
                    documento.empresa = empresa;
                    documento.fecha = DateTime.Today;
                    ctx.tblFM_EnvioDocumento.Add(documento);
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public tblM_SolicitudEquipo listaSolicitudEquipoPorEmpresa(int idSolicitud, int empresa = 0)
        {
            if (empresa == 0) { empresa = vSesiones.sesionEmpresaActual; }
            tblM_SolicitudEquipo data = new tblM_SolicitudEquipo();
            using (var ctx = new MainContext(empresa))
            {
                data = ctx.tblM_SolicitudEquipo.FirstOrDefault(x => x.id.Equals(idSolicitud));
            }
            return data;
        }

        public List<SolicitudEquipoDTO> getListaDetalleSolicitudAutorizacionPorEmpresa(int idSolicitud, int empresa = 0)
        {
            if (empresa == 0) { empresa = vSesiones.sesionEmpresaActual; }
            List<SolicitudEquipoDTO> data = new List<SolicitudEquipoDTO>();
            using (var ctx = new MainContext(empresa))
            {
                var res = (from sd in ctx.tblM_SolicitudEquipoDet
                           join s in ctx.tblM_SolicitudEquipo on sd.solicitudEquipoID equals s.id
                           where s.id == idSolicitud
                           select new { sd, s }).ToList();

                var final = res.Select(x => new SolicitudEquipoDTO
                {
                    Descripcion = string.IsNullOrWhiteSpace(x.sd.Comentario) ? "" : x.sd.Comentario,
                    Folio = x.s.folio,
                    Tipo = x.sd.TipoMaquinaria.descripcion,
                    TipoId = x.sd.tipoMaquinariaID,
                    Grupo = x.sd.GrupoMaquinaria.descripcion,
                    Grupoid = x.sd.grupoMaquinariaID,
                    Modelo = x.sd.ModeloEquipo.descripcion,
                    Modeloid = x.sd.modeloEquipoID,
                    pCapacidad = 0,
                    pFechaInicio = x.sd.fechaInicio.ToString("dd/MM/yyyy"),
                    pFechaFin = x.sd.fechaFin.ToString("dd/MM/yyyy"),
                    pHoras = x.sd.horas,
                    pTipoPrioridad = getPrioridad(x.sd.prioridad),// == 0 ? "Programada" : "Urgente",
                    id = x.sd.id, // idSolicitudDetalle
                    SolicitudDetalleId = x.s.id //idSolicitud
                });
                data = final.ToList();
            }
            return data;
        }

        public tblM_AutorizacionSolicitudes getAutorizadoresPorEmpresa(int idSolicitud, int empresa = 0)
        {
            if (empresa == 0) { empresa = vSesiones.sesionEmpresaActual; }
            tblM_AutorizacionSolicitudes data = new tblM_AutorizacionSolicitudes();
            using (var ctx = new MainContext(empresa))
            {
                data = ctx.tblM_AutorizacionSolicitudes.FirstOrDefault(x => x.solicitudEquipoID.Equals(idSolicitud));
            }
            return data;
        }

        public List<tblM_SolicitudEquipoDet> listaDetalleSolicitudPorEmpresa(int obj, int empresa = 0)
        {
            if (empresa == 0) { empresa = vSesiones.sesionEmpresaActual; }
            List<tblM_SolicitudEquipoDet> data = new List<tblM_SolicitudEquipoDet>();
            var ctx = new MainContext(empresa);
                data = ctx.tblM_SolicitudEquipoDet.Where(x => x.solicitudEquipoID.Equals(obj)).ToList();
            return data;
        }

        public List<tblM_AsignacionEquipos> getAsignacionesByIDPorEmpresa(int id, int empresa = 0)
        {
            if (empresa == 0) { empresa = vSesiones.sesionEmpresaActual; }
            List<tblM_AsignacionEquipos> data = new List<tblM_AsignacionEquipos>();
            var ctx = new MainContext(empresa);
            data = ctx.tblM_AsignacionEquipos.Where(x => x.solicitudEquipoID == id).ToList();
            
            return data;
        }
        public Dictionary<string, object> getAsginaciones(int economicoID)
        {

            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;
                var flag = false;
                if (economicoID > 3)
                {
                    bool result = _context.tblM_AsignacionEquipos.Where(x => economicoID == x.noEconomicoID && x.estatus != 3).Any(r => r.estatus != 10);
                    flag = result;
                }
                var flag2 = false;
                if (economicoID > 3)
                {
                    bool result = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == economicoID ).redireccionamientoVenta;
                    flag2 = result;
                }
                resultado.Add("asignacionExiste", flag);
                resultado.Add("redireccionamientoVenta", flag2);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al obtener los operadores de la barrenadora.");
            }

            return resultado;

        }
        public Dictionary<string, object> GetAutorizadoresAC()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            try
            {
                var ccs = _context.tblP_CC.Where(e => e.estatus && e.areaCuenta.Contains("-")).OrderBy(x=>x.area).ThenBy(x=>x.cuenta).ToList();
                var dataLst = new List<dynamic>();
                foreach (var i in ccs)
                {
                    var usuariosAC = _context.tblP_CC_Usuario.Where(x => x.cc.Equals(i.areaCuenta)).Select(x=>x.id).ToList();
                    var autorizadores = _context.tblP_Autoriza.Where(x => usuariosAC.Contains(x.cc_usuario_ID)).ToList();

                    var adminMaq = autorizadores.FirstOrDefault(x=>x.perfilAutorizaID == 5);
                    var gerenteObra = autorizadores.FirstOrDefault(x => x.perfilAutorizaID == 1);
                    var directorArea = autorizadores.FirstOrDefault(x => x.perfilAutorizaID == 4);
                    var directoDivision = autorizadores.FirstOrDefault(x => x.perfilAutorizaID == 2);
                    var directorServicios = autorizadores.FirstOrDefault(x => x.perfilAutorizaID == 11);
                    var altaDireccion = autorizadores.FirstOrDefault(x => x.perfilAutorizaID == 3);
                    dataLst.Add(new 
                    {
                        cc = i.areaCuenta,
                        ccDesc = i.descripcion,
                        adminMaq = adminMaq != null ? adminMaq.usuarioID : 0,
                        gerenteObra = gerenteObra != null ? gerenteObra.usuarioID : 0,
                        directorArea = directorArea != null ? directorArea.usuarioID : 0,
                        directoDivision = directoDivision != null ? directoDivision.usuarioID : 0,
                        directorServicios = directorServicios != null ? directorServicios.usuarioID : 0,
                        altaDireccion = altaDireccion != null ? altaDireccion.usuarioID : 0,
                        adminMaqNom = adminMaq != null ? getNombreUsuario(adminMaq.usuarioID) : "",
                        gerenteObraNom = gerenteObra != null ? getNombreUsuario(gerenteObra.usuarioID) : "",
                        directorAreaNom = directorArea != null ? getNombreUsuario(directorArea.usuarioID) : "",
                        directoDivisionNom = directoDivision != null ? getNombreUsuario(directoDivision.usuarioID) : "",
                        directorServiciosNom = directorServicios != null ? getNombreUsuario(directorServicios.usuarioID) : "",
                        altaDireccionNom = altaDireccion != null ? getNombreUsuario(altaDireccion.usuarioID) : "",
                    });
                }

                result.Add(ITEMS, dataLst);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {

                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return result;
        }
        public string getNombreUsuario(int id)
        {
            var u = _context.tblP_Usuario.FirstOrDefault(x=>x.id == id);
            var nombre = u.nombre + " " + u.apellidoPaterno + " " + u.apellidoMaterno;
            return nombre;
        }
        public void SetAutorizadorAC(int usuarioID, string ac, int perfil) {
            var regAC = _context.tblP_CC_Usuario.FirstOrDefault(x => x.usuarioID == usuarioID && x.cc.Equals(ac));

            var usuariosAC = _context.tblP_CC_Usuario.Where(x => x.cc.Equals(ac)).Select(x => x.id).ToList();
            var autorizadores = _context.tblP_Autoriza.Where(x => usuariosAC.Contains(x.cc_usuario_ID)).ToList();
            var existe = autorizadores.FirstOrDefault(x=>x.perfilAutorizaID == perfil);

            if(existe != null)
            {
                _context.tblP_Autoriza.Remove(existe);
                _context.SaveChanges();
            }

            if(regAC != null)
            {
                int id = regAC.id;
                var o = new tblP_Autoriza();
                o.usuarioID = usuarioID;
                o.perfilAutorizaID = perfil;
                o.cc_usuario_ID = id;
                _context.tblP_Autoriza.Add(o);
                _context.SaveChanges();
            }
            else
            {
                var a = new tblP_CC_Usuario();
                a.usuarioID = usuarioID;
                a.cc = ac;
                _context.tblP_CC_Usuario.Add(a);
                _context.SaveChanges();
                int id = a.id;
                var o = new tblP_Autoriza();
                o.usuarioID = usuarioID;
                o.perfilAutorizaID = perfil;
                o.cc_usuario_ID = id;
                _context.tblP_Autoriza.Add(o);
                _context.SaveChanges();
            }
        }
    }
}
