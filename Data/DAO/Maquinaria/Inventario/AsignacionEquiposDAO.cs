using Core.DAO.Maquinaria.Inventario;
using Core.DTO;
using Core.DTO.Captura;
using Core.DTO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Catalogos;
using Core.DTO.Maquinaria.Inventario.Comparativos;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Inventario
{
    public class AsignacionEquiposDAO : GenericDAO<tblM_AsignacionEquipos>, IAsignacionEquiposDAO
    {


        public void SaveOrUpdate(tblM_AsignacionEquipos obj)
        {
            if (obj.estatus == 3)
            {
                var regMaqEstatus3 = _context.tblM_AsignacionEquipos.Where(w => w.estatus == 3 && w.noEconomicoID == obj.noEconomicoID);
                if (regMaqEstatus3 != null && regMaqEstatus3.Count() > 0)
                    foreach (var item in regMaqEstatus3)
                    {
                        item.estatus = 10;
                    }
                _context.SaveChanges();
            }
            //if(true)
            //{
            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.ASIGNACIONMAQUINARIA);
            else
                Update(obj, obj.id, (int)BitacoraEnum.ASIGNACIONMAQUINARIA);
            //}
            //else
            //{
            //    if(obj.id == 0)
            //        throw new Exception("Ya se capturo el registro.");
            //    else
            //        Update(obj, obj.id, (int)BitacoraEnum.ASIGNACIONMAQUINARIA);
            //}
        }

        public void SaveOrUpdate(List<tblM_AsignacionEquipos> array)
        {
            foreach (tblM_AsignacionEquipos obj in array)
            {
                if (obj.estatus == 3)
                {
                    var regMaqEstatus3 = _context.tblM_AsignacionEquipos.Where(w => w.estatus == 3 && w.noEconomicoID == obj.noEconomicoID);
                    if (regMaqEstatus3 != null && regMaqEstatus3.Count() > 0)
                        foreach (var item in regMaqEstatus3)
                        {
                            item.estatus = 10;
                        }
                    _context.SaveChanges();
                }

                if (true)
                {

                    if (obj.estatus == 5 && obj.StepPen == false)//raguilar pruebas 11:22am
                    {
                        obj.StepPen = true;
                        obj.estatus = 4;
                    }
                    //else if (obj.estatus == 4 || obj.estatus==2)//checar probema validacion error setea todo a 10
                    //{
                    //    obj.StepPen = false;
                    //    //elimina al cambiar el ultimo asignacion
                    //    var lstCambioEstatus = _context.tblM_AsignacionEquipos.Where(x => (x.estatus == 1|| x.estatus==3) && x.id != obj.id).OrderByDescending(x => x.id).ToList();////control de deshabilitado ultimo registro en caso de estar uno habilitado
                    //    lstCambioEstatus.ForEach(x =>
                    //    {
                    //        x.estatus = 10;
                    //    });
                    //    _context.SaveChanges();
                    //}                  
                    if (obj.id == 0)
                        SaveEntity(obj, (int)BitacoraEnum.ASIGNACIONMAQUINARIA);
                    else
                        Update(obj, obj.id, (int)BitacoraEnum.ASIGNACIONMAQUINARIA);
                }
                else
                {
                    throw new Exception("Error ocurrio un error al insertar un registro");
                }
            }
        }
        public tblM_AsignacionEquipos GetAsiganacionBySolicitud(int obj) //Detalle
        {
            return _context.tblM_AsignacionEquipos.FirstOrDefault(x => x.SolicitudDetalleId.Equals(obj));
        }
        public tblM_AsignacionEquipos GetAsiganacionById(int obj)
        {
            return _context.tblM_AsignacionEquipos.FirstOrDefault(x => x.id.Equals(obj));
        }

        public List<tblM_CatMaquina> getMaquinasAsignadas(List<int> idEconomicos)
        {
            var parcial = (from c in _context.tblM_CatMaquina
                           where idEconomicos.Contains(c.id)
                           select c).ToList();
            var grupo = parcial.Select(x => x.grupoMaquinariaID).ToList();
            var res = from r in _context.tblM_CatMaquina
                      where grupo.Contains(r.grupoMaquinariaID) && !idEconomicos.Contains(r.id)
                      select r;


            return res.ToList();
        }
        public List<int> getMaquinariaAsiganadasD(DateTime FechaInico)
        {
            var res = _context.tblM_AsignacionEquipos.Where(x => x.noEconomicoID != 0 && FechaInico < x.fechaFin).Select(x => x.noEconomicoID).ToList();
            return res;
        }

        public List<tblM_AsignacionEquipos> getAsignacionesByEconomicos(int economicoID)
        {
            return _context.tblM_AsignacionEquipos.Where(a => a.noEconomicoID.Equals(economicoID)).ToList();

        }

        public List<tblM_AsignacionEquipos> getAsignadosByDetSolicitud(int solicitudDetID, int idAsignacion)
        {

            return _context.tblM_AsignacionEquipos.Where(x => x.solicitudEquipoID == solicitudDetID && x.id != idAsignacion).ToList();

        }
        public List<tblM_AsignacionEquipos> getAsignacionActivas(List<string> ccs)
        {
            List<string> objcc = new List<string>();
            ccs = ccs ?? new List<string>();
            foreach (var aux in ccs)
            {
                try
                {
                    objcc.Add(aux);
                }
                catch
                {
                }
            }
            try
            {

                var res = _context.tblM_AsignacionEquipos.Where(x => objcc.Count > 0 ? objcc.Contains(x.cc) : true).ToList();
                return res;
            }
            catch
            {
                List<tblM_AsignacionEquipos> res = new List<tblM_AsignacionEquipos>();
                return res;
            }
        }

        public List<tblM_AsignacionEquipos> getAsignacionesCompras()
        {
            var ccs = _context.tblP_CC.Where(x => x.estatus).Select(x => x.areaCuenta);

            return _context.tblM_AsignacionEquipos.Where(x => x.noEconomicoID == 0 && ccs.Contains(x.cc) && x.fechaAsignacion.Year >= 2020 && (x.Economico.Equals("COMPRA") || x.Economico.Equals("RENTA"))).ToList();
        }
        public tblM_AsignacionEquipos getAsignacionesCompras(int id)
        {
            return _context.tblM_AsignacionEquipos.FirstOrDefault(x => x.noEconomicoID == 0 && x.SolicitudDetalleId == id && x.estatus != 10);
        }
        public tblM_AsignacionEquipos getEconomicoAsignado(int obj)
        {
            return _context.tblM_AsignacionEquipos.Where(x => x.noEconomicoID.Equals(obj)).OrderByDescending(x => x.id).FirstOrDefault();
        }

        public bool GetAsginadosRecibidos(int obj)
        {
            bool bandera = true;
            var asginados = (from b in _context.tblM_AsignacionEquipos
                             where b.estatus != 1 && b.solicitudEquipoID.Equals(obj)
                             select b).ToList();
            return bandera = asginados.Count > 0 ? true : false;
        }

        public List<tblM_AsignacionEquipos> getAsignacionesByCC(string obj)
        {
            var res = _context.tblM_AsignacionEquipos.Where(x => x.cc.Equals(obj) && x.estatus == 3);
            return res.ToList();
        }


        public List<tblM_AsignacionEquipos> getAsignacionesByID(int id)
        {
            var res = _context.tblM_AsignacionEquipos.Where(x => x.solicitudEquipoID == id);
            return res.ToList();
        }

        public List<tblM_AsignacionEquipos> getAsignacionesByIDs(int id)
        {
            var res = _context.tblM_AsignacionEquipos.Where(x => x.solicitudEquipoID == id);
            return res.ToList();
        }


        public void delete(tblM_AsignacionEquipos Entidad)
        {

            try
            {
                tblM_AsignacionEquipos entidad = _context.tblM_AsignacionEquipos.FirstOrDefault(x => x.id.Equals(Entidad.id));
                Delete(entidad, (int)BitacoraEnum.ASIGNACIONMAQUINARIA);
            }
            catch (Exception)
            {
                throw new Exception("Error al eliminar el registro");
            }
        }

        public List<tblM_AsignacionEquipos> GetListaAsignaciones()
        {
            return _context.tblM_AsignacionEquipos.Where(x => x.estatus != 10 && x.estatus != 3).ToList();
        }

        public List<tblM_AsignacionEquipos> getAsignacionesbySDet(int id)
        {
            return _context.tblM_AsignacionEquipos.Where(x => x.SolicitudDetalleId == id).ToList();
        }

        public List<tblM_AsignacionEquipos> GetAsignacionControles(int Filtro, string CentroCostos)
        {
            return _context.tblM_AsignacionEquipos.Where(x => x.SolicitudEquipo.CC == CentroCostos && x.estatus > 0 && x.estatus != 10).ToList();
        }

        public List<HistoricoMaquinariaDTO> GetHistorialMaquina(string EconomicoID)
        {

            var Maquinaria = _context.tblM_CatMaquina.FirstOrDefault(x => x.noEconomico == EconomicoID);
            var listaXCC = _context.tblM_CapHorometro.Where(x => x.Economico == Maquinaria.noEconomico).GroupBy(x => x.CC).ToList();

            List<HistoricoMaquinariaDTO> HistoricoMaquinaria = new List<HistoricoMaquinariaDTO>();
            int count = 0;
            foreach (var item in listaXCC.Select(x => x.Key).ToList())
            {
                count++;
                HistoricoMaquinariaDTO historicoMaquinaria = new HistoricoMaquinariaDTO();
                string horasTotalesEconomico = "";
                DateTime fechaentrada = new DateTime();
                DateTime fechaSalida = new DateTime();

                var HorasTotalesRes = _context.tblM_CapHorometro.Where(x => x.CC == item && x.Economico == Maquinaria.noEconomico).ToList();

                horasTotalesEconomico = HorasTotalesRes.Sum(x => x.HorasTrabajo).ToString();

                var resFEchaEntrada = HorasTotalesRes.Where(c => c.CC == item).OrderBy(x => x.Fecha).FirstOrDefault();
                var resFechaSalida = HorasTotalesRes.Where(c => c.CC == item).OrderByDescending(x => x.Fecha).FirstOrDefault();
                if (resFEchaEntrada != null)
                {
                    fechaentrada = resFEchaEntrada.Fecha;//.ToShortDateString();
                }
                if (resFechaSalida != null)
                {
                    fechaSalida = resFechaSalida.Fecha;//.ToShortDateString();
                }
                historicoMaquinaria.id = count;
                historicoMaquinaria.FechaEntrega = fechaentrada.ToShortDateString();
                historicoMaquinaria.FechaLiberacion = fechaSalida.ToShortDateString();
                historicoMaquinaria.totalHoras = horasTotalesEconomico;
                historicoMaquinaria.Centro_Costos = getCentroCostos(item);
                HistoricoMaquinaria.Add(historicoMaquinaria);

            }

            return HistoricoMaquinaria;
        }
        public List<HistoricoMaquinariaDTO> getHistorialEconomicos(string EconomicoID)
        {

            int idEconomico = Convert.ToInt32(EconomicoID);
            var ListaAsignacion = _context.tblM_AsignacionEquipos.Where(x => x.noEconomicoID == idEconomico && (!x.CCOrigen.Equals(x.cc) || x.Economico.Equals("COMPRA") || x.Economico.Equals("RENTA"))).OrderBy(x => x.fechaAsignacion).ToList();
            var maq = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == idEconomico);
            int count = 0;
            List<HistoricoMaquinariaDTO> HistoricoMaquinaria = new List<HistoricoMaquinariaDTO>();
            foreach (var item in ListaAsignacion.Where(x => !x.cc.Equals("1010") && !x.cc.Equals("1018") && !x.cc.Equals("1015")))
            {
                count++;
                HistoricoMaquinariaDTO historicoMaquinaria = new HistoricoMaquinariaDTO();
                string horasTotalesEconomico = "";
                string fechaentrada = "---";
                string fechaSalida = "---";

                int idAsignacion = item.id;
                int idAsignacion2 = 0;


                var lstAsignacionesRE = _context.tblM_ControlEnvioMaquinaria.FirstOrDefault(x => x.asignacionEquipoId == idAsignacion && x.tipoControl == 1);
                var lstAsignacionesSA = _context.tblM_ControlEnvioMaquinaria.FirstOrDefault(x => x.asignacionEquipoId == idAsignacion && x.tipoControl == 2);

                if (lstAsignacionesRE != null)
                {
                    fechaentrada = lstAsignacionesRE.fechaElaboracion.ToShortDateString();
                }

                if (lstAsignacionesSA != null)
                {
                    fechaSalida = lstAsignacionesSA.fechaElaboracion.ToShortDateString();
                }
                string cc = item.cc;

                historicoMaquinaria.id = item.id;
                historicoMaquinaria.FechaAsignacion = item.fechaAsignacion.ToShortDateString();
                historicoMaquinaria.FechaEnvio = fechaentrada;
                historicoMaquinaria.FechaEntrega = fechaentrada;
                historicoMaquinaria.FechaLiberacion = fechaSalida;
                historicoMaquinaria.totalHoras = "" + 0;
                historicoMaquinaria.Centro_Costos = cc;
                historicoMaquinaria.estASig = item.estatus;
                HistoricoMaquinaria.Add(historicoMaquinaria);

            }
            var ctrls = _context.tblM_CatControlCalidad.Where(x => x.IdEconomico == idEconomico && (x.TipoControl == 3 || x.TipoControl == 4)).Select(x => x.IdAsignacion).ToList();
            var asig = _context.tblM_AsignacionEquipos.Where(x => ctrls.Contains(x.id)).ToList();
            foreach (var item in asig)
            {
                count++;
                HistoricoMaquinariaDTO historicoMaquinaria = new HistoricoMaquinariaDTO();
                string horasTotalesEconomico = "";
                string fechaentrada = "---";
                string fechaSalida = "---";

                int idAsignacion = item.id;
                int idAsignacion2 = 0;

                var cal = _context.tblM_CatControlCalidad.Where(x => x.IdAsignacion == idAsignacion && (x.TipoControl == 3 || x.TipoControl == 4)).OrderByDescending(x => x.id).FirstOrDefault();
                var lstAsignacionesRE = _context.tblM_ControlEnvioMaquinaria.FirstOrDefault(x => x.asignacionEquipoId == idAsignacion && x.tipoControl == 3);
                var lstAsignacionesSA = _context.tblM_ControlEnvioMaquinaria.FirstOrDefault(x => x.asignacionEquipoId == idAsignacion && x.tipoControl == 4);

                if (lstAsignacionesRE != null)
                {
                    fechaentrada = lstAsignacionesRE.fechaElaboracion.ToShortDateString();
                }

                if (lstAsignacionesSA != null)
                {
                    fechaSalida = lstAsignacionesSA.fechaElaboracion.ToShortDateString();
                }
                var cc = cal.CcDestino == null ? item.cc : (cal.CcDestino.Contains("CENTRAL") ? "1010" : cal.CcDestino.Contains("PATIO") ? "1015" : cal.CcDestino.Contains("OVERHAUL") ? "1018" : "VENTA/PROVEEDOR");



                historicoMaquinaria.id = item.id;
                historicoMaquinaria.FechaAsignacion = "---";
                historicoMaquinaria.FechaEnvio = fechaentrada;
                historicoMaquinaria.FechaEntrega = fechaentrada;
                historicoMaquinaria.FechaLiberacion = fechaSalida;
                historicoMaquinaria.totalHoras = "" + 0;
                historicoMaquinaria.Centro_Costos = cc;
                historicoMaquinaria.estASig = item.estatus;
                HistoricoMaquinaria.Add(historicoMaquinaria);
            }
            var rest = HistoricoMaquinaria.OrderBy(x => x.id).ToList();

            for (int i = 0; i <= rest.Count() - 1; i++)
            {
                if (rest.Count() == 1)
                {
                    if (!rest[i].FechaLiberacion.Equals("---"))
                    {
                        rest[i].FechaEntrega = rest[i].FechaLiberacion;
                        if (maq.estatus == 0)
                        {
                            if (maq.fechaBaja != null)
                                rest[i].FechaLiberacion = ((DateTime)maq.fechaBaja).ToShortDateString() + " (Baja)";
                        }
                        else
                        {
                            rest[i].FechaLiberacion = "---";
                        }
                    }
                    else if (rest[i].FechaEntrega.Equals("---"))
                    {
                        rest[i].FechaEntrega = "Falta envio / recepción";
                        rest[i].FechaLiberacion = "---";
                    }
                    else if (rest[i].FechaLiberacion.Equals("---"))
                    {

                        rest[i].FechaEntrega = "Falta recepción";
                        rest[i].FechaLiberacion = "---";
                    }
                    else
                    {
                        rest[i].FechaEntrega = rest[i].FechaLiberacion;
                        if (maq.estatus == 0)
                        {
                            if (maq.fechaBaja != null)
                                rest[i].FechaLiberacion = ((DateTime)maq.fechaBaja).ToShortDateString() + " (Baja)";
                        }
                        else
                        {
                            rest[i].FechaLiberacion = "---";
                        }
                    }
                }
                else if (i == (rest.Count() - 1) && rest.Count() > 1)
                {
                    rest[i].FechaEntrega = rest[i].FechaLiberacion;
                    if (maq.estatus == 0)
                    {
                        if (maq.fechaBaja != null)
                            rest[i].FechaLiberacion = ((DateTime)maq.fechaBaja).ToShortDateString() + " (Baja)";
                    }
                    else
                    {
                        rest[i].FechaLiberacion = "---";
                    }
                }
                else
                {
                    if (rest[i].FechaLiberacion.Equals("---"))
                    {
                        rest[i].FechaEntrega = "---";
                    }
                    else
                    {
                        rest[i].FechaEntrega = rest[i].FechaLiberacion;
                        rest[i].FechaLiberacion = rest[i + 1].FechaEnvio;
                    }
                }
            }

            foreach (var i in HistoricoMaquinaria)
            {
                DateTime? inicio = null;
                DateTime? fin = null;
                bool bInicio = false;
                bool bFin = false;

                inicio = !i.FechaAsignacion.Equals("---") ? Convert.ToDateTime(i.FechaAsignacion) : !i.FechaEntrega.Equals("---") ? Convert.ToDateTime(i.FechaEntrega) : (DateTime?)null;
                fin = !i.FechaLiberacion.Equals("---") ? Convert.ToDateTime(i.FechaLiberacion) : (DateTime?)null;
                bInicio = inicio != null ? true : false;
                bFin = fin != null ? true : false;

                DateTime vInicio = bInicio ? (DateTime)inicio : DateTime.Now;
                DateTime vFin = bFin ? (DateTime)fin : DateTime.Now;
                decimal hrs = _context.tblM_CapHorometro.Where(x =>
                            x.CC.Equals(i.Centro_Costos) &&
                            x.Economico.Equals(maq.noEconomico) &&
                            (bInicio ? x.Fecha >= vInicio : true) &&
                            (bFin ? x.Fecha <= vFin : true)
                    ).ToList().Sum(x => x.HorasTrabajo);
                i.totalHoras = "" + hrs;

            }

            return HistoricoMaquinaria.OrderBy(x => x.id).ToList();

        }

        public string getCentroCostos(string cc)
        {
            switch (cc)
            {
                case "1010": { return "1010-TALLER MECANICO CENTRAL"; }
                case "1015": { return "1015-PATIO DE MAQUINARIA"; }
                case "1018": { return "1018-TALLER OVERHAUL (VIRTUAL)"; }
                default:
                    try
                    {
                        var resultado = new List<economicoDTO>();
                        try
                        {
                            resultado = (List<economicoDTO>)ContextArrendadora.Where("SELECT DISTINCT  (CAST(area  AS varchar(4))+'-'+CAST(cuenta AS varchar(4))) AS cc, descripcion FROM si_area_cuenta WHERE cc = '" + cc + "';").ToObject<List<economicoDTO>>();
                            return cc + "-" + resultado.FirstOrDefault().descripcion;
                        }
                        catch (Exception)
                        {
                            resultado = (List<economicoDTO>)ContextNominaConstruplan.Where("SELECT descripcion FROM cc WHERE cc = '" + cc + "';").ToObject<List<economicoDTO>>();
                            return cc + "-" + resultado.FirstOrDefault().descripcion;
                        }
                    }
                    catch (Exception e)
                    {
                        return string.Empty;
                    }
            }
        }

        public void logErrores(int sistema, int modulo, string controlador, string action, Exception exception, AccionEnum tipo, long registroId, object objeto)
        {
            LogError(sistema, modulo, controlador, action, exception, tipo, registroId, objeto);
        }

        public List<CuadroAutorizacionDTO> CargarSolicitudes(int estado, string obra, DateTime fechaInicio, DateTime fechaFin)
        {
            var listaResultado = new List<CuadroAutorizacionDTO>();
            var ComparativoDAO = new ComparativoFactoryServices().getComparativoFactoryService();

            var listaCuadros = _context.tblM_ComparativoAdquisicionyRenta.Where(r => r.estatus == 2 || r.estatus == 3 || r.estatusFinanciera == 2).ToList();

            #region Asignaciones
            var listaCuadros_idAsignacion = listaCuadros.Where(x => x.idAsignacion > 0).Select(x => x.idAsignacion).ToList();

            var listaAsignaciones = _context.tblM_AsignacionEquipos.Where(x =>
                //x.noEconomicoID == 0 &&
                listaCuadros_idAsignacion.Contains(x.id)).ToList().Select(x => new CuadroAutorizacionDTO
            {
                idAsignacion = x.id,
                idSolicitud = x.solicitudEquipoID,
                CentroCostos = x.SolicitudEquipo.CC,
                CCDescripcion = ComparativoDAO.getDescripcion(x.SolicitudEquipo.CC),
                noSolicitud = x.SolicitudEquipo.folio,
                TipoSolicitud = x.Economico,
                GrupoEquipo = x.SolicitudDetalle.GrupoMaquinaria.descripcion,
                Modelo = x.SolicitudDetalle.ModeloEquipo.descripcion,
                Comentario = x.SolicitudDetalle.Comentario,
                FechaPromesaDate = x.FechaPromesa,
                FechaPromesa = x.FechaPromesa == null ? "---" : x.FechaPromesa.ToShortDateString(),
                lstFinanciero = ComparativoDAO.getTablaComparativoFinancieroDetalle(x.id, vSesiones.sesionUsuarioDTO.id).Count(),
                lstComparativo = ComparativoDAO.getTablaComparativoAdquisicionDetalle(x.id, vSesiones.sesionUsuarioDTO.id).Count(),
                estado = ComparativoDAO.ObtenerEstatus(x.id),
                botonFin = ComparativoDAO.obtenerEstatusAutorizado(1, x.id, vSesiones.sesionUsuarioDTO.id),
                botonAdq = ComparativoDAO.obtenerEstatusAutorizado(2, x.id, vSesiones.sesionUsuarioDTO.id),
                registroCuadro = false
            }).ToList();

            foreach (var asig in listaAsignaciones)
            {
                var cuadroAsignacion = listaCuadros.FirstOrDefault(x => x.idAsignacion == asig.idAsignacion);

                if (cuadroAsignacion != null)
                {
                    var ultimaAutorizacionCuadro = ComparativoDAO.GetUltimaAutorizacionCuadro(cuadroAsignacion.id);

                    asig.GrupoEquipo = cuadroAsignacion.nombreDelEquipo; //Se reemplaza esta propiedad para que en el front-end aparezca la información del cuadro en la columna de "Descripción" en dado caso que sí tenga un cuadro ligado.

                    asig.idCuadro = cuadroAsignacion.id;
                    asig.obra = cuadroAsignacion.obra;
                    asig.fechaElaboracionCuadro = cuadroAsignacion.fechaDeElaboracion;
                    asig.fechaElaboracionCuadroString = cuadroAsignacion.fechaDeElaboracion != null ? ((DateTime)cuadroAsignacion.fechaDeElaboracion).ToShortDateString() : "";
                    asig.fechaUltimaAutorizacionCuadro = ultimaAutorizacionCuadro.Item1;
                    asig.fechaUltimaAutorizacionCuadroString = ultimaAutorizacionCuadro.Item2;
                    asig.estatusCuadro = cuadroAsignacion.estatus;
                }
            }

            listaAsignaciones = listaAsignaciones.Where(x =>
                x.fechaElaboracionCuadro != null ?
                    (((DateTime)x.fechaElaboracionCuadro).Date >= fechaInicio.Date && ((DateTime)x.fechaElaboracionCuadro).Date <= fechaFin.Date)
                : false
            ).ToList();

            if (estado == 0)
            {
                listaResultado = listaAsignaciones;
            }
            else if (estado == 2)
            {
                listaResultado = listaAsignaciones.Where(r => r.estado == 1 || r.estado == 2).ToList();
            }
            else
            {
                listaResultado = listaAsignaciones.Where(r => r.estado == estado).ToList();
            }
            #endregion

            #region Cuadros
            var listaCuadrosFiltrados = listaCuadros.Where(x => x.idAsignacion == 0).ToList();
            var cuadrosData = listaCuadrosFiltrados.Select(x => new CuadroAutorizacionDTO
            {
                idAsignacion = x.idAsignacion,
                idSolicitud = 0,
                CentroCostos = "",
                CCDescripcion = "",
                noSolicitud = "",
                TipoSolicitud = "",
                GrupoEquipo = x.nombreDelEquipo, //Se coloca el nombre del equipo como grupo equipo para que en el front-end aparezca en la columna de "descripción".
                Modelo = "",
                Comentario = x.ComentarioGeneral,
                FechaPromesa = "",
                lstFinanciero = 0,
                lstComparativo = 1,
                estado = 0,
                botonFin = 0,
                botonAdq = 0,
                idCuadro = x.id,
                registroCuadro = true,
                obra = x.obra,
                fechaElaboracionCuadro = x.fechaDeElaboracion,
                fechaElaboracionCuadroString = x.fechaDeElaboracion != null ? ((DateTime)x.fechaDeElaboracion).ToShortDateString() : "",
                fechaUltimaAutorizacionCuadro = ComparativoDAO.GetUltimaAutorizacionCuadro(x.id).Item1,
                fechaUltimaAutorizacionCuadroString = ComparativoDAO.GetUltimaAutorizacionCuadro(x.id).Item2,
                estatusCuadro = x.estatus
            }).ToList();

            if (estado == 3) //Autorizados
            {
                cuadrosData = cuadrosData.Where(x => x.estatusCuadro == 3).ToList();
            }
            else if (estado == 2) //Pendientes
            {
                cuadrosData = cuadrosData.Where(x => x.estatusCuadro == 2).ToList();
            }

            cuadrosData = cuadrosData.Where(x =>
                x.fechaElaboracionCuadro != null ?
                    (((DateTime)x.fechaElaboracionCuadro).Date >= fechaInicio.Date && ((DateTime)x.fechaElaboracionCuadro).Date <= fechaFin.Date)
                : false
            ).ToList();

            listaResultado.AddRange(cuadrosData);
            #endregion

            if (obra != null && obra != "")
            {
                listaResultado = listaResultado.Where(x => ((x.obra != null && x.obra != "") ? x.obra.ToUpper().Contains(obra.ToUpper()) : false)).ToList();
            }

            #region Verificar si el cuadro es editable
            foreach (var res in listaResultado)
            {
                if (res.idCuadro > 0)
                {
                    var listaAutorizacionesTerminadas = _context.tblM_ComparativoAdquisicionyRentaAutorizante.Where(x => x.idCuadro == res.idCuadro && x.idComparativoDetalle > 0).ToList();

                    if (listaAutorizacionesTerminadas.Any(x => x.autorizanteID == 1164 || x.orden == 5)) //Se verifica si el usuario de Gerardo Reina ya autorizó el cuadro o si el último autorizante lo hizo.
                    {
                        res.cuadroEditable = false;
                    }
                    else
                    {
                        res.cuadroEditable = true;
                    }
                }
                else
                {
                    res.cuadroEditable = false;
                }
            }
            #endregion

            return listaResultado;
        }
    }
}
