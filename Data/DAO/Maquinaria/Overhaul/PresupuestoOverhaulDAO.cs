using Core.DAO.Maquinaria.Overhaul;
using Core.Entity.Maquinaria.Overhaul;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria;
using Core.DTO.Maquinaria.Overhaul;
using Core.DTO.Principal.Generales;
using Newtonsoft.Json;
using Data.EntityFramework.Context;
using Infrastructure.Utils;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;

namespace Data.DAO.Maquinaria.Overhaul
{
    public class PresupuestoOverhaulDAO : GenericDAO<tblM_PresupuestoOverhaul>, IPresupuestoOverhaulDAO
    {
        public RemocionComponenteDAO remocionDAO = new RemocionComponenteDAO();
        public List<PresupuestoOverhaulDTO> CargarTblPresupuesto(List<string> obras, int modeloID, int anio, List<tblM_CatMaquina> maquinas, tblM_PresupuestoOverhaul objPresupuesto)
        {
            List<PresupuestoOverhaulDTO> data = new List<PresupuestoOverhaulDTO>();

            if (objPresupuesto != null)
            {
                //var obrasPresupuesto = JsonConvert.DeserializeObject<List<FechasPresupuestoObraDTO>>(objPresupuesto.JsonObras);
                //var obrasNoAutorizadas = obrasPresupuesto.Where(x => x.estado < 5).Select(z => z.obraID).ToList();
                //var detallePresupuesto = _context.tblM_DetallePresupuestoOverhaul.Where(x => x.presupuestoID == objPresupuesto.id && obras.Contains(x.obra) /*&& obrasNoAutorizadas.Contains(x.obra)*/).ToList();
                //var componentes = detallePresupuesto.Select(x => x.componenteID).ToList();
                ////var auxMaquinas = detallePresupuesto.Select(x => x.maquinaID).ToList();

                //var trackActual = _context.tblM_trackComponentes.Where(x => x.estatus == 0 && componentes.Contains(x.componenteID)).GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault())
                //    .ToList();

                //var subConjuntosAPresupuestar = detallePresupuesto.Select(x => x.componente.subConjunto).Distinct();
                ////var prueba = detallePresupuesto.Select(x => x.componenteID);
                //foreach (var item in subConjuntosAPresupuestar)
                //{
                //    var auxComponentes = trackActual.Where(x => x.componente.subConjuntoID == item.id).ToList();
                //    var auxComponentesID = auxComponentes.Select(x => x.componenteID).ToList();
                //    var auxDetalles = detallePresupuesto.Where(x => auxComponentesID.Contains(x.componenteID));
                //    if (auxComponentes.Count() > 0)
                //    {
                //        PresupuestoOverhaulDTO presupuesto = new PresupuestoOverhaulDTO();
                //        presupuesto.maquinasComponentes = auxComponentes.Select(x => x.locacion).ToList();
                //        presupuesto.subconjunto = item.descripcion;
                //        presupuesto.subconjuntoID = item.id;
                //        presupuesto.obras = GetObrasCompAPresupuestar(obras, obrasNoAutorizadas, auxComponentes, maquinas, objPresupuesto.id);
                //        presupuesto.costoTotal = auxDetalles.Sum(x => x.costoPresupuesto);
                //        var vidaMaxima = auxDetalles.Max(x => x.vida);
                //        presupuesto.costoVida = new List<decimal>(vidaMaxima);
                //        presupuesto.vida = new List<int>(vidaMaxima);
                //        for (int i = 0; i <= vidaMaxima; i++) 
                //        {
                //            presupuesto.costoVida.Add(auxDetalles.Where(x => x.vida == i).Sum(x => x.costoPresupuesto));
                //            presupuesto.vida.Add(auxDetalles.Where(x => x.vida == i).Count());
                //        }
                //        data.Add(presupuesto);
                //    }
                //}


                //var obrasPresupuesto = JsonConvert.DeserializeObject<List<FechasPresupuestoObraDTO>>(objPresupuesto.JsonObras);
                //var obrasNoAutorizadas = obrasPresupuesto.Where(x => x.estado < 5).Select(z => z.obraID).ToList();
                var detallePresupuesto = _context.tblM_DetallePresupuestoOverhaul.Where(x => x.presupuestoID == objPresupuesto.id /* && obras.Contains(x.obra) && obrasNoAutorizadas.Contains(x.obra)*/).ToList();

                var auxMaquinas = maquinas.Select(y => y.id).ToList();

                var calendarios = _context.tblM_CalendarioPlaneacionOverhaul.Where(x => (anio == -1 ? true : x.anio == anio) && x.grupoMaquinaID == 0 && x.modeloMaquinaID == 0 /*&& (obras.Contains(x.obraID) || x.obraID == "")*/);
                var calendariosID = calendarios.Select(x => x.id).ToList();

                var eventosAPresupuestar = _context.tblM_CapPlaneacionOverhaul.Where(x => calendariosID.Contains(x.calendarioID) && auxMaquinas.Contains(x.maquinaID)).ToList();
                var EntradasEnCalendario = new List<ComponentePlaneacionDTO>();
                foreach (var item in eventosAPresupuestar)
                {
                    EntradasEnCalendario.AddRange(JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(item.idComponentes));
                }
                EntradasEnCalendario = EntradasEnCalendario.Distinct().ToList();

                var componentesID = EntradasEnCalendario.Where(x => x.Tipo != 1).Select(x => x.componenteID).ToList();
                var serviciosID = EntradasEnCalendario.Where(x => x.Tipo == 1).Select(x => x.componenteID).ToList();

                var componentes = _context.tblM_CatComponente.Where(x => componentesID.Contains(x.id));
                var trackActual = _context.tblM_trackComponentes.Where(x => componentesID.Contains(x.componenteID)).GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault()).ToList();

                var servicios = _context.tblM_CatServicioOverhaul.Where(x => serviciosID.Contains(x.id)).ToList(); ;
                var trackActualServicios = _context.tblM_trackServicioOverhaul.Where(x => serviciosID.Contains(x.servicioID)).GroupBy(x => x.servicioID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault()).ToList();

                var subConjuntosAPresupuestar = componentes.Select(x => x.subConjunto).Distinct().ToList();
                var tipoServiciosAPresupuestarID = servicios.Select(x => x.tipoServicioID).Distinct().ToList();
                var tipoServiciosAPresupuestar = _context.tblM_CatTipoServicioOverhaul.Where(x => tipoServiciosAPresupuestarID.Contains(x.id)).ToList();


                foreach (var item in tipoServiciosAPresupuestar)
                {
                    var auxServicios = servicios.Where(x => x.tipoServicioID == item.id && serviciosID.Contains(x.id)).ToList();
                    var auxDetalles = detallePresupuesto.Where(x => serviciosID.Contains(x.componenteID) && x.esServicio);
                    //auxServicios.ForEach(x => x.vidaInicio += GetVidasComponente(x.id));
                    var trackingServicios = trackActualServicios.Where(x => serviciosID.Contains(x.servicioID) && x.servicio.tipoServicioID == item.id).ToList();
                    if (auxServicios.Count() > 0)
                    {
                        PresupuestoOverhaulDTO presupuesto = new PresupuestoOverhaulDTO();
                        presupuesto.maquinasComponentes = servicios.Select(x => x.maquina.noEconomico).ToList();
                        presupuesto.subconjunto = item.descripcion;
                        presupuesto.subconjuntoID = item.id;
                        presupuesto.obras = GetObrasCompAPresupuestar(obras, obras, servicios, maquinas, objPresupuesto.id);
                        int vidaMaxima = trackingServicios.Count() > 0 ? trackingServicios.GroupBy(x => x.servicioID).Select(x => x.Count()).Max() : 0;
                        presupuesto.costoTotal = auxDetalles.Count() > 0 ? auxDetalles.Sum(x => x.costoPresupuesto) : 0;
                        presupuesto.vida = new List<int>(vidaMaxima);
                        presupuesto.costoVida = new List<decimal>(vidaMaxima);
                        //presupuesto.vida = trackingServicios.GroupBy(x => x.servicioID).Select(x => x.Count()).ToList();
                        for (int i = 0; i <= vidaMaxima; i++) {
                            var numVida = trackingServicios.Count() > 0 ? trackingServicios.GroupBy(x => x.servicioID).Select(x => x.Count()).Where(x => x == i).Count() : 0;
                            presupuesto.vida.Add(numVida);
                            presupuesto.costoVida.Add(0); 
                        }
                        presupuesto.esServicio = true;
                        data.Add(presupuesto);
                    }
                }

                foreach (var item in subConjuntosAPresupuestar)
                {
                    var auxComponentes = componentes.Where(x => x.subConjuntoID == item.id && componentesID.Contains(x.id)).ToList();
                    auxComponentes.ForEach(x => x.vidaInicio += GetVidasComponente(x.id));
                    var auxComponentesID = auxComponentes.Select(x => x.id).ToList();
                    var trackingComponentes = trackActual.Where(x => auxComponentesID.Contains(x.componenteID) && x.componente.subConjuntoID == item.id).ToList();
                    var auxDetalles = detallePresupuesto.Where(x => x.subconjuntoID == item.id && auxComponentesID.Contains(x.componenteID) && !x.esServicio).ToList();
                    List<string> auxMaquinaPres = new List<string>();
                    if (auxDetalles.Count() > 0) auxMaquinaPres = auxDetalles.Select(x => x.maquina.noEconomico).ToList();
                    if (auxComponentes.Count() > 0)
                    {
                        PresupuestoOverhaulDTO presupuesto = new PresupuestoOverhaulDTO();
                        presupuesto.maquinasComponentes = /*auxMaquinaPres.Count() > 0 ? auxMaquinaPres :*/ trackingComponentes.Select(x => x.locacion).ToList();
                        presupuesto.subconjunto = item.descripcion;
                        presupuesto.subconjuntoID = item.id;
                        presupuesto.obras = GetObrasCompAPresupuestar(obras, obras, trackingComponentes, maquinas, objPresupuesto.id, item.id);
                        presupuesto.costoTotal = auxDetalles.Count() > 0 ? auxDetalles.Sum(x => x.costoPresupuesto) : 0;
                        int vidaMaxima = auxDetalles.Count() > 0 ? auxDetalles.Max(x => x.vida) : auxComponentes.Max(x => x.vidaInicio);
                        presupuesto.vida = new List<int>(vidaMaxima + 1);
                        presupuesto.costoVida = new List<decimal>(vidaMaxima + 1);
                        for (int i = 0; i <= vidaMaxima; i++)
                        {
                            if (auxDetalles.Count() > 0)
                            {
                                presupuesto.costoVida.Add(auxDetalles.Where(x => x.vida == i).Sum(x => x.costoPresupuesto));
                                presupuesto.vida.Add(auxDetalles.Where(x => x.vida == i).Count());
                            }
                            else
                            {
                                presupuesto.vida.Add(auxComponentes.Where(x => x.vidaInicio == i).Count());
                                presupuesto.costoVida.Add(0);
                            }
                        }
                        presupuesto.esServicio = false;
                        data.Add(presupuesto);
                    }
                }
            }
            else
            {
                var auxMaquinas = maquinas.Select(y => y.id).ToList();
                //var trackActual = _context.tblM_trackComponentes.GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault())
                //    .ToList();

                var calendarios = _context.tblM_CalendarioPlaneacionOverhaul.Where(x => (anio == -1 ? true : x.anio == anio) && x.grupoMaquinaID == 0 && x.modeloMaquinaID == 0 && obras.Contains(x.obraID));
                var calendariosID = calendarios.Select(x => x.id).ToList();
                var eventosAPresupuestar = _context.tblM_CapPlaneacionOverhaul.Where(x => calendariosID.Contains(x.calendarioID) && auxMaquinas.Contains(x.maquinaID)).ToList();
                var EntradasEnCalendario = new List<ComponentePlaneacionDTO>();
                foreach (var item in eventosAPresupuestar)
                {
                    EntradasEnCalendario.AddRange(JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(item.idComponentes));
                }
                EntradasEnCalendario = EntradasEnCalendario.Distinct().ToList();

                var componentesID = EntradasEnCalendario.Where(x => x.Tipo != 1).Select(x => x.componenteID).ToList();
                var serviciosID = EntradasEnCalendario.Where(x => x.Tipo == 1).Select(x => x.componenteID).ToList();

                var componentes = _context.tblM_CatComponente.Where(x => componentesID.Contains(x.id));
                var trackActual = _context.tblM_trackComponentes.Where(x => componentesID.Contains(x.componenteID) && !(x.tipoLocacion ?? false)).GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault()).ToList();
                //var trackActual = _context.tblM_trackComponentes.Where(x => prueba.Contains(x.componenteID) && x.componente.subConjuntoID == subConjunto && x.estatus == 0).GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault());


                var servicios = _context.tblM_CatServicioOverhaul.Where(x => serviciosID.Contains(x.id) && x.estatus).ToList(); ;
                var trackActualServicios = _context.tblM_trackServicioOverhaul.Where(x => serviciosID.Contains(x.servicioID)).GroupBy(x => x.servicioID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault()).ToList();

                var subConjuntosAPresupuestar = componentes.Select(x => x.subConjunto).Distinct().ToList();
                var tipoServiciosAPresupuestarID = servicios.Select(x => x.tipoServicioID).Distinct().ToList();
                var tipoServiciosAPresupuestar = _context.tblM_CatTipoServicioOverhaul.Where(x => tipoServiciosAPresupuestarID.Contains(x.id)).ToList();


                foreach (var item in tipoServiciosAPresupuestar)
                {
                    var auxServicios = servicios.Where(x => x.tipoServicioID == item.id && serviciosID.Contains(x.id)).ToList();
                    //auxServicios.ForEach(x => x.vidaInicio += GetVidasComponente(x.id));
                    var trackingServicios = trackActualServicios.Where(x => serviciosID.Contains(x.servicioID) && x.servicio.tipoServicioID == item.id).ToList();
                    if (auxServicios.Count() > 0)
                    {
                        PresupuestoOverhaulDTO presupuesto = new PresupuestoOverhaulDTO();
                        presupuesto.maquinasComponentes = servicios.Select(x => x.maquina.noEconomico).ToList();
                        presupuesto.subconjunto = item.descripcion;
                        presupuesto.subconjuntoID = item.id;
                        presupuesto.obras = GetObrasCompAPresupuestar(obras, obras, servicios, maquinas, 0);
                        int vidaMaxima = 0; // trackingServicios.Count() > 0 ? trackingServicios.GroupBy(x => x.servicioID).Select(x => x.Count()).Max() : 0;
                        presupuesto.vida = new List<int>(vidaMaxima + 1);
                        presupuesto.costoVida = new List<decimal>(vidaMaxima + 1);
                        presupuesto.vida = trackingServicios.GroupBy(x => x.servicioID).Select(x => x.Count()).ToList();
                        for (int i = 0; i <= vidaMaxima; i++) presupuesto.costoVida.Add(0);
                        presupuesto.esServicio = true;
                        data.Add(presupuesto);
                    }
                }

                foreach (var item in subConjuntosAPresupuestar)
                {
                    var auxComponentes = componentes.Where(x => x.subConjuntoID == item.id && componentesID.Contains(x.id)).ToList();
                    auxComponentes.ForEach(x => x.vidaInicio += GetVidasComponente(x.id));
                    var trackingComponentes = trackActual.Where(x => componentesID.Contains(x.componenteID) && x.componente.subConjuntoID == item.id).ToList();
                    if (auxComponentes.Count() > 0)
                    {
                        PresupuestoOverhaulDTO presupuesto = new PresupuestoOverhaulDTO();
                        presupuesto.maquinasComponentes = trackingComponentes.Select(x => x.locacion).ToList();
                        presupuesto.subconjunto = item.descripcion;
                        presupuesto.subconjuntoID = item.id;
                        presupuesto.obras = GetObrasCompAPresupuestar(obras, obras, trackingComponentes, maquinas, 0, item.id);
                        int vidaMaxima = auxComponentes.Max(x => x.vidaInicio);
                        presupuesto.vida = new List<int>(vidaMaxima + 1);
                        presupuesto.costoVida = new List<decimal>(vidaMaxima + 1);
                        for (int i = 0; i <= vidaMaxima; i++)
                        {
                            presupuesto.vida.Add(auxComponentes.Where(x => x.vidaInicio == i).Count());
                            presupuesto.costoVida.Add(0);
                        }
                        presupuesto.esServicio = false;
                        data.Add(presupuesto);
                    }
                }
            }
            return data;
        }

        private List<PropiedadOverhaulDTO> GetObrasCompAPresupuestar(List<string> obras, List<string> obrasNoAutorizadas, List<tblM_trackComponentes> componentes, List<tblM_CatMaquina> maquinas, int presupuestoID, int subconjuntoID)
        {

            var presupuesto = _context.tblM_DetallePresupuestoOverhaul.Where(x => x.presupuestoID == presupuestoID).ToList();
            List<string> obrasNuevo = new List<string>();
            //if (presupuesto.Count() > 0) obrasNuevo = presupuesto.Select(x => x.obra).Distinct().ToList();
            /*else*/
            obrasNuevo = obras;
            List<PropiedadOverhaulDTO> auxObras = new List<PropiedadOverhaulDTO>();
            PropiedadOverhaulDTO aux;
            var componentesGuardados = presupuesto.Where(x => obras.Contains(x.obra) && x.subconjuntoID == subconjuntoID).ToList();
            var componentesGuardadosID = componentesGuardados.Select(x => x.componenteID).ToList();

            foreach (var item in obrasNuevo)
            {
                if (item != null)
                {
                    aux = new PropiedadOverhaulDTO();
                    aux.Propiedad = item;
                    aux.Nombre = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == item).descripcion;
                    var maquinasObra = maquinas.Where(x => x.centro_costos == item).Select(x => x.id).ToList();
                    //var componentesGuardadosObra = componentesGuardados.Where(x => x.obra == item).Select(x => x.componenteID).ToList();
                    //var componentesSinGuardar = componentes.Where(x => maquinasObra.Contains(x.locacionID ?? default(int)) && !componentesGuardadosID.Contains(x.componenteID)).Select(x => x.componenteID).ToList();

                    //aux.Valor = componentesGuardadosObra.Count() + componentesSinGuardar.Count();
                    aux.Valor = componentes.Where(x => maquinasObra.Contains(x.locacionID ?? default(int))).Count();
                    var componentesID = componentes.Where(x => maquinasObra.Contains(x.locacionID ?? default(int))).Select(x => x.componenteID).ToList();

                    if (aux.Valor > 0)
                    {
                        aux.Costo = presupuesto.Where(x => x.obra == item && componentesID.Contains(x.componenteID)).Sum(x => x.costoPresupuesto);
                    }
                    else
                    {
                        aux.Costo = 0;
                    }
                    if (obrasNoAutorizadas.Contains(item)) { aux.Autorizado = false; }
                    else { aux.Autorizado = true; }
                    auxObras.Add(aux);
                }
            }
            auxObras = auxObras.OrderByDescending(x => x.Valor).ToList();
            auxObras = auxObras.Where(x => x.Valor > 0).ToList();

            return auxObras;
        }

        private List<PropiedadOverhaulDTO> GetObrasCompAPresupuestar(List<string> obras, List<string> obrasNoAutorizadas, List<tblM_CatServicioOverhaul> servicios, List<tblM_CatMaquina> maquinas, int presupuestoID)
        {

            var presupuesto = _context.tblM_DetallePresupuestoOverhaul.Where(x => x.presupuestoID == presupuestoID).ToList();
            List<string> obrasNuevo = new List<string>();
            //if (presupuesto.Count() > 0) obrasNuevo = presupuesto.Select(x => x.obra).Distinct().ToList();
            /*else*/
            obrasNuevo = obras;
            List<PropiedadOverhaulDTO> auxObras = new List<PropiedadOverhaulDTO>();
            PropiedadOverhaulDTO aux;
            foreach (var item in obrasNuevo)
            {
                if (item != null)
                {
                    aux = new PropiedadOverhaulDTO();
                    aux.Propiedad = item;
                    aux.Nombre = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == item).descripcion;
                    var maquinasObra = maquinas.Where(x => x.centro_costos == item).Select(x => x.id).ToList();
                    aux.Valor = servicios.Where(x => maquinasObra.Contains(x.maquinaID)).Count();
                    var serviciosID = servicios.Select(x => x.id).ToList();

                    if (presupuesto.Count() > 0)
                    {
                        aux.Costo = presupuesto.Where(x => x.obra == item && serviciosID.Contains(x.componenteID)).Sum(x => x.costoPresupuesto);
                    }
                    else
                    {
                        aux.Costo = 0;
                    }
                    if (obrasNoAutorizadas.Contains(item)) { aux.Autorizado = false; }
                    else { aux.Autorizado = true; }
                    auxObras.Add(aux);
                }
            }
            auxObras = auxObras.OrderByDescending(x => x.Valor).ToList();
            auxObras = auxObras.Where(x => x.Valor > 0).ToList();

            return auxObras;
        }

        public List<tblM_CatMaquina> GetMaquinasPresupuestar(List<string> obras, int modeloID, int anio)
        {
            List<tblM_CatMaquina> data = new List<tblM_CatMaquina>();
            data = _context.tblM_CatMaquina.Where(x => x.modeloEquipoID == modeloID && (obras.Count() > 0 ? obras.Contains(x.centro_costos) : true)).ToList();
            return data;
        }

        public List<DetallePresupuestoDTO> GetDetallePresupuesto(List<string> obras, int vidas, int modelo, int anio, int subConjunto, int presupuestoID, bool esServicio)
        {
            List<DetallePresupuestoDTO> data = new List<DetallePresupuestoDTO>();

            if (presupuestoID != 0)
            {
                var presupuesto = _context.tblM_PresupuestoOverhaul.FirstOrDefault(x => x.id == presupuestoID);
                List<string> obrasNoAutorizadas = new List<string>();
                if (presupuesto != null) 
                {
                    obrasNoAutorizadas = (JsonConvert.DeserializeObject<List<FechasPresupuestoObraDTO>>(presupuesto.JsonObras)).Where(x => x.estado < 5).Select(x => x.obraID).ToList();
                }
                var detallesPresupuesto = _context.tblM_DetallePresupuestoOverhaul.Where(x => x.presupuestoID == presupuestoID && x.subconjuntoID == subConjunto && (vidas == -1 ? true : x.vida == vidas) && obras.Contains(x.obra) && obrasNoAutorizadas.Contains(x.obra) && x.esServicio == esServicio).ToList();
                var maquinas = GetMaquinasPresupuestar(obras, modelo, anio);
                var auxMaquinas = maquinas.Select(x => x.id).Distinct();

                var componentesAPresupuestar = detallesPresupuesto.Select(x => x.componente).Distinct().ToList();

                foreach (var item in detallesPresupuesto)
                {
                    var auxDetalle = new DetallePresupuestoDTO();
                    auxDetalle.color = "#80ffaa";
                    auxDetalle.componenteID = item.componenteID;
                    auxDetalle.costo = item.costoPresupuesto;
                    auxDetalle.costoSugerido = item.costoSugerido;
                    auxDetalle.fecha = item.fecha;
                    auxDetalle.tipo = item.tipo;
                    auxDetalle.horometroAcumulado = item.componente.horasAcumuladas;
                    auxDetalle.horometroCiclo = item.componente.horaCicloActual;
                    auxDetalle.maquinaID = item.maquinaID;
                    auxDetalle.noComponente = item.componente.noComponente;
                    auxDetalle.noEconomico = item.maquina.noEconomico;
                    auxDetalle.target = item.componente.cicloVidaHoras;
                    auxDetalle.vida = item.vida;
                    auxDetalle.guardado = true;
                    auxDetalle.esServicio = esServicio;
                    data.Add(auxDetalle);
                }

                var calendarios = _context.tblM_CalendarioPlaneacionOverhaul.Where(x => (anio == -1 ? true : x.anio == anio) && ((x.grupoMaquinaID == 0 && x.modeloMaquinaID == 0 && obras.Contains(x.obraID)) || x.obraID == null)).Select(x => x.id).ToList();
                var detallesPresupuestadosIDs = componentesAPresupuestar.Select(x => x.id).ToList();
                var componentesSinGuardar = _context.tblM_CapPlaneacionOverhaul.Where(x => calendarios.Contains(x.calendarioID) && auxMaquinas.Contains(x.maquinaID)).ToList();
                
                foreach (var item in componentesSinGuardar)
                {
                    var auxParoDetID = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(item.idComponentes).Where(x => (esServicio ? x.Tipo == 1 : x.Tipo != 1)).Select(x => x.componenteID).ToList();

                    if (esServicio)
                    {
                        var auxParoDet = _context.tblM_CatServicioOverhaul.Where(x => auxParoDetID.Contains(x.id) && x.tipoServicioID == subConjunto).ToList();
                        foreach (var item2 in auxParoDet)
                        {
                            var auxDetalleCoincidencia = detallesPresupuestadosIDs.FirstOrDefault(x => x == item2.id);
                            var indexDetalle = detallesPresupuestadosIDs.FindIndex(x => x == item2.id);
                            if (indexDetalle < 0)
                            {
                                var auxDetalle = new DetallePresupuestoDTO();
                                auxDetalle.color = "#f9f9f9";
                                auxDetalle.componenteID = item2.id;
                                auxDetalle.costo = 0;
                                auxDetalle.costoSugerido = 0;
                                auxDetalle.fecha = item.fecha;
                                auxDetalle.tipo = item.tipo;
                                auxDetalle.horometroAcumulado = item2.horasCicloActual;
                                auxDetalle.horometroCiclo = item2.horasCicloActual;
                                auxDetalle.maquinaID = item2.maquinaID;
                                auxDetalle.noComponente = item2.servicio.descripcion;
                                auxDetalle.noEconomico = item2.maquina.noEconomico;
                                auxDetalle.target = item2.cicloVidaHoras;
                                auxDetalle.vida = 0;
                                auxDetalle.guardado = false;
                                auxDetalle.esServicio = esServicio;
                                data.Add(auxDetalle);
                            }
                            else
                            {
                                detallesPresupuestadosIDs.RemoveAt(indexDetalle);
                            }

                        }
                    }
                    else 
                    {
                        var auxParoDet = _context.tblM_CatComponente.Where(x => auxParoDetID.Contains(x.id) && x.subConjuntoID == subConjunto).ToList();
                        foreach (var item2 in auxParoDet)
                        {
                            var auxDetalleCoincidencia = detallesPresupuestadosIDs.FirstOrDefault(x => x == item2.id);
                            var indexDetalle = detallesPresupuestadosIDs.FindIndex(x => x == item2.id);
                            if (indexDetalle < 0)
                            {
                                var auxDetalle = new DetallePresupuestoDTO();
                                auxDetalle.color = "#f9f9f9";
                                auxDetalle.componenteID = item2.id;
                                auxDetalle.costo = 0;
                                auxDetalle.costoSugerido = 0;
                                auxDetalle.fecha = item.fecha;
                                auxDetalle.tipo = item.tipo;
                                auxDetalle.horometroAcumulado = item2.horaCicloActual;
                                auxDetalle.horometroCiclo = item2.horaCicloActual;
                                auxDetalle.maquinaID = item.maquinaID;
                                auxDetalle.noComponente = item2.noComponente;
                                auxDetalle.noEconomico = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == item.maquinaID).noEconomico;
                                auxDetalle.target = item2.cicloVidaHoras;
                                auxDetalle.vida = 0;
                                auxDetalle.guardado = false;
                                auxDetalle.esServicio = esServicio;
                                data.Add(auxDetalle);
                            }
                            else
                            {
                                detallesPresupuestadosIDs.RemoveAt(indexDetalle);
                            }

                        }
                    }
                }
            }
            else
            {

            var presupuesto = _context.tblM_PresupuestoOverhaul.FirstOrDefault(x => x.id == presupuestoID);
            //List<string> obrasNoAutorizadas = new List<string>();
            List<tblM_DetallePresupuestoOverhaul> detallesPresupuesto = new List<tblM_DetallePresupuestoOverhaul>();
            List<int> componentesPresupuestoSinFiltrar = new List<int>();
            if (presupuesto != null)
            {
                //obrasNoAutorizadas = (JsonConvert.DeserializeObject<List<FechasPresupuestoObraDTO>>(presupuesto.JsonObras)).Where(x => x.estado < 5).Select(x => x.obraID).ToList();
                componentesPresupuestoSinFiltrar = _context.tblM_DetallePresupuestoOverhaul.Where(x => x.presupuestoID == presupuestoID && x.subconjuntoID == subConjunto && (vidas == -1 ? true : x.vida == vidas) && x.esServicio == esServicio).Select(x => x.componenteID).ToList();
                detallesPresupuesto = _context.tblM_DetallePresupuestoOverhaul.Where(x => x.presupuestoID == presupuestoID && x.subconjuntoID == subConjunto && (vidas == -1 ? true : x.vida == vidas) && obras.Contains(x.obra) && x.esServicio == esServicio).ToList();
            }

            var maquinas = GetMaquinasPresupuestar(obras, modelo, anio);
            // _context.tblM_CatMaquina.Where(x => x.estatus == 1 &&  x.modeloEquipoID == modelo && (obras.Contains(x.centro_costos))).ToList();
            var auxMaquinas = maquinas.Select(x => x.id).ToList();

            var calendarios = _context.tblM_CalendarioPlaneacionOverhaul.Where(x => (anio == -1 ? true : x.anio == anio) && ((x.grupoMaquinaID == 0 && x.modeloMaquinaID == 0 && obras.Contains(x.obraID)) || x.obraID == null) ).Select(x => x.id).ToList();
            //calendarios.Add(72);
            var componentesAPresupuestar = _context.tblM_CapPlaneacionOverhaul.Where(x => calendarios.Contains(x.calendarioID) && auxMaquinas.Contains(x.maquinaID)).ToList();
            var prueba = new List<int>();// detallesPresupuesto.Select(x => x.componenteID).ToList();
            var prueba2 = new List<PropiedadOverhaulDTO>();
            //foreach (var item2 in prueba)
            //{
            //    var auxComponente = new PropiedadOverhaulDTO();
            //    auxComponente.Valor = item2;
            //    auxComponente.Propiedad = "0";
            //    prueba2.Add(auxComponente);
            //}

            foreach (var item in componentesAPresupuestar)
            {
                var auxComponentes = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(item.idComponentes).Where(x => (esServicio ? x.Tipo == 1 : x.Tipo != 1) /*&& x.Value == "0"*/).Select(x => x.componenteID).ToList();
                //auxComponentes = auxComponentes.Where(x => !componentesPresupuestoSinFiltrar.Contains(x)).ToList();
                prueba.AddRange(auxComponentes);
                foreach (var item2 in prueba)
                {
                    var auxComponente = new PropiedadOverhaulDTO();
                    auxComponente.Valor = item2;
                    auxComponente.Propiedad = item.id.ToString();
                    prueba2.Add(auxComponente);
                }
            }

            List<int> auxDetallesPresupuestoID = detallesPresupuesto.Where(x => !prueba.Contains(x.componenteID)).Select(x => x.componenteID).ToList();
            prueba.AddRange(auxDetallesPresupuestoID);

            foreach (var item in auxDetallesPresupuestoID)
            {
                var auxComponente = new PropiedadOverhaulDTO();
                auxComponente.Valor = item;
                auxComponente.Propiedad = item.ToString();
                prueba2.Add(auxComponente);
            }
            


            //DateTime limiteFecha = DateTime.Today.AddYears(-1);
            var trackPresupuesto = _context.tblM_trackComponentes.Where(x => x.componente.modeloEquipoID == modelo && x.componente.subConjuntoID == subConjunto && x.costoCRC != 0 /*&& x.fecha > limiteFecha*/);
            decimal presupuestoSugerido = 0;
            if (trackPresupuesto.Count() > 0) { presupuestoSugerido = trackPresupuesto.Sum(x => x.costoCRC) / trackPresupuesto.Count(); }

            if (esServicio)
            {
                var trackActual = _context.tblM_CatServicioOverhaul.Where(x => prueba.Contains(x.id) && x.tipoServicioID == subConjunto);

                trackActual = trackActual.Where(x => prueba.Contains(x.id));
                int auxEventoID;
                foreach (var item in trackActual.ToList())
                {
                    if (prueba.Contains(item.id))
                    {
                        DetallePresupuestoDTO aux = new DetallePresupuestoDTO();
                        var auxDetallesPresupuesto = detallesPresupuesto.FirstOrDefault(x => x.componenteID == item.id);
                        auxEventoID = 0;
                        Int32.TryParse(prueba2.FirstOrDefault(y => y.Valor == item.id).Propiedad, out auxEventoID);
                        int vidasActual = GetVidasServicio(item.id);

                        if (vidasActual == vidas || vidas < 0)
                        {

                            if (auxDetallesPresupuesto == null)
                            {
                                aux.costo = Math.Round(presupuestoSugerido, 2);
                                aux.costoSugerido = Math.Round(presupuestoSugerido, 2);
                                aux.color = "#f9f9f9";

                                aux.componenteID = item.id;
                                aux.noComponente = item.servicio.descripcion;
                                aux.noEconomico = maquinas.FirstOrDefault(x => x.id == item.maquinaID).noEconomico;
                                aux.maquinaID = item.maquinaID;
                                aux.horometroAcumulado = item.horasCicloActual;
                                aux.horometroCiclo = item.horasCicloActual;
                                aux.vida = vidasActual;
                                aux.esServicio = esServicio;
                                aux.guardado = false;

                                data.Add(aux);
                            }
                            else
                            {
                                aux.color = "#80ffaa";
                                aux.componenteID = auxDetallesPresupuesto.componenteID;
                                aux.costo = auxDetallesPresupuesto.costoPresupuesto;
                                aux.costoSugerido = auxDetallesPresupuesto.costoSugerido;
                                aux.fecha = auxDetallesPresupuesto.fecha;
                                aux.tipo = auxDetallesPresupuesto.tipo;
                                aux.horometroAcumulado = auxDetallesPresupuesto.componente.horasAcumuladas;
                                aux.horometroCiclo = auxDetallesPresupuesto.componente.horaCicloActual;
                                aux.maquinaID = auxDetallesPresupuesto.maquinaID;
                                aux.noComponente = item.servicio.descripcion;
                                aux.noEconomico = auxDetallesPresupuesto.maquina.noEconomico;
                                aux.target = auxDetallesPresupuesto.componente.cicloVidaHoras;
                                aux.vida = auxDetallesPresupuesto.vida;
                                aux.esServicio = esServicio;
                                aux.guardado = true;
                                data.Add(aux);
                            }
                        }
                    }
                }
            }
            else
            {
                var trackActual = _context.tblM_trackComponentes.Where(x => prueba.Contains(x.componenteID) && x.componente.subConjuntoID == subConjunto && x.tipoLocacion == false && auxMaquinas.Contains(x.locacionID ?? 0) && (x.fecha ?? default(DateTime)).Year == anio).GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault());

                trackActual = trackActual.Where(x => prueba.Contains(x.componenteID) && auxMaquinas.Contains(x.locacionID ?? 0));
                int auxEventoID;
                foreach (var itemPrueba in prueba) 
                {
                    var auxTrackActual = trackActual.FirstOrDefault(x => x.componenteID == itemPrueba);
                    if (auxTrackActual == null)
                    {
                        DetallePresupuestoDTO aux = new DetallePresupuestoDTO();
                        var auxDetallesPresupuesto = detallesPresupuesto.FirstOrDefault(x => x.componenteID == itemPrueba);
                        if (auxDetallesPresupuesto != null)
                        {
                            aux.color = "#80ffaa";
                            aux.componenteID = auxDetallesPresupuesto.componenteID;
                            aux.costo = auxDetallesPresupuesto.costoPresupuesto;
                            aux.costoSugerido = auxDetallesPresupuesto.costoSugerido;
                            aux.fecha = auxDetallesPresupuesto.fecha;
                            aux.tipo = auxDetallesPresupuesto.tipo;
                            aux.horometroAcumulado = auxDetallesPresupuesto.componente.horasAcumuladas;
                            aux.horometroCiclo = auxDetallesPresupuesto.componente.horaCicloActual;
                            aux.maquinaID = auxDetallesPresupuesto.maquinaID;
                            aux.noComponente = auxDetallesPresupuesto.componente.noComponente;
                            aux.noEconomico = auxDetallesPresupuesto.maquina.noEconomico;
                            aux.target = auxDetallesPresupuesto.componente.cicloVidaHoras;
                            aux.vida = auxDetallesPresupuesto.vida;
                            aux.esServicio = esServicio;
                            aux.guardado = true;
                            data.Add(aux);
                        }
                    }
                    else 
                    {
                        DetallePresupuestoDTO aux = new DetallePresupuestoDTO();
                        var auxDetallesPresupuesto = detallesPresupuesto.FirstOrDefault(x => x.componenteID == auxTrackActual.componenteID);
                        auxEventoID = 0;
                        Int32.TryParse(prueba2.FirstOrDefault(y => y.Valor == auxTrackActual.componenteID).Propiedad, out auxEventoID);
                        int vidasActual = GetVidasComponente(auxTrackActual.componenteID) + auxTrackActual.componente.vidaInicio;

                        if (vidasActual == vidas || vidas < 0)
                        {

                            if (auxDetallesPresupuesto == null)
                            {
                                aux.costo = Math.Round(presupuestoSugerido, 2);
                                aux.costoSugerido = Math.Round(presupuestoSugerido, 2);
                                aux.color = "#f9f9f9";

                                aux.componenteID = auxTrackActual.componenteID;
                                aux.noComponente = auxTrackActual.componente.noComponente;
                                aux.noEconomico = maquinas.FirstOrDefault(x => x.id == (auxTrackActual.locacionID ?? default(int))).noEconomico;
                                aux.maquinaID = auxTrackActual.locacionID ?? default(int);
                                aux.horometroAcumulado = auxTrackActual.componente.horasAcumuladas;
                                aux.horometroCiclo = auxTrackActual.componente.horaCicloActual;
                                aux.vida = vidasActual;
                                aux.esServicio = esServicio;
                                aux.guardado = false;
                                data.Add(aux);
                            }
                            else
                            {
                                aux.color = "#80ffaa";
                                aux.componenteID = auxDetallesPresupuesto.componenteID;
                                aux.costo = auxDetallesPresupuesto.costoPresupuesto;
                                aux.costoSugerido = auxDetallesPresupuesto.costoSugerido;
                                aux.fecha = auxDetallesPresupuesto.fecha;
                                aux.tipo = auxDetallesPresupuesto.tipo;
                                aux.horometroAcumulado = auxDetallesPresupuesto.componente.horasAcumuladas;
                                aux.horometroCiclo = auxDetallesPresupuesto.componente.horaCicloActual;
                                aux.maquinaID = auxDetallesPresupuesto.maquinaID;
                                aux.noComponente = auxDetallesPresupuesto.componente.noComponente;
                                aux.noEconomico = auxDetallesPresupuesto.maquina.noEconomico;
                                aux.target = auxDetallesPresupuesto.componente.cicloVidaHoras;
                                aux.vida = auxDetallesPresupuesto.vida;
                                aux.esServicio = esServicio;
                                aux.guardado = true;
                                data.Add(aux);
                            }
                        }
                    }
                }

                //foreach (var item in trackActual.ToList())
                //{
                //    if (prueba.Contains(item.componenteID))
                //    {
                //        DetallePresupuestoDTO aux = new DetallePresupuestoDTO();
                //        var auxDetallesPresupuesto = detallesPresupuesto.FirstOrDefault(x => x.componenteID == item.componenteID);
                //        auxEventoID = 0;
                //        Int32.TryParse(prueba2.FirstOrDefault(y => y.Valor == item.componenteID).Propiedad, out auxEventoID);
                //        int vidasActual = GetVidasComponente(item.componenteID) + item.componente.vidaInicio;

                //        if (vidasActual == vidas || vidas < 0)
                //        {

                //            if (auxDetallesPresupuesto == null)
                //            {
                //                aux.costo = Math.Round(presupuestoSugerido, 2);
                //                aux.costoSugerido = Math.Round(presupuestoSugerido, 2);
                //                aux.color = "#f9f9f9";

                //                aux.componenteID = item.componenteID;
                //                aux.noComponente = item.componente.noComponente;
                //                aux.noEconomico = maquinas.FirstOrDefault(x => x.id == (item.locacionID ?? default(int))).noEconomico;
                //                aux.maquinaID = item.locacionID ?? default(int);
                //                aux.horometroAcumulado = item.componente.horasAcumuladas;
                //                aux.horometroCiclo = item.componente.horaCicloActual;
                //                aux.vida = vidasActual;
                //                aux.esServicio = esServicio;
                //                aux.guardado = false;
                //                data.Add(aux);
                //            }
                //            else
                //            {
                //                aux.color = "#80ffaa";
                //                aux.componenteID = auxDetallesPresupuesto.componenteID;
                //                aux.costo = auxDetallesPresupuesto.costoPresupuesto;
                //                aux.costoSugerido = auxDetallesPresupuesto.costoSugerido;
                //                aux.fecha = auxDetallesPresupuesto.fecha;
                //                aux.tipo = auxDetallesPresupuesto.tipo;
                //                aux.horometroAcumulado = auxDetallesPresupuesto.componente.horasAcumuladas;
                //                aux.horometroCiclo = auxDetallesPresupuesto.componente.horaCicloActual;
                //                aux.maquinaID = auxDetallesPresupuesto.maquinaID;
                //                aux.noComponente = auxDetallesPresupuesto.componente.noComponente;
                //                aux.noEconomico = auxDetallesPresupuesto.maquina.noEconomico;
                //                aux.target = auxDetallesPresupuesto.componente.cicloVidaHoras;
                //                aux.vida = auxDetallesPresupuesto.vida;
                //                aux.esServicio = esServicio;
                //                aux.guardado = true;
                //                data.Add(aux);
                //            }
                //        }
                //    }
                //}
            }

            }
            return data;
        }


        public List<ReporteInversionOverhaulDTO> GetDetallePresupuestos(List<int> presupuestosID, List<string> obras, int anio, List<int> modelos)
        {
            if (obras == null) obras = new List<string>();
            if (modelos == null) modelos = new List<int>();
            List<string> meses = (new string[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" }).ToList();
            List<string> auxTipoParo = (new string[] { "Overhaul General", "Cambio de Motor", "Componentes Desfasados", "Fallo" }).ToList();
            var detallesRaw = _context.tblM_DetallePresupuestoOverhaul.Where(x => presupuestosID.Contains(x.presupuestoID) && (obras.Count() == 0 ? true : obras.Contains(x.obra)) && x.estado < 2 && x.programado).ToList();
            var componentesDetallesRaw = detallesRaw.Select(x => x.componenteID).ToList();
            List<ReporteInversionOverhaulDTO> data = new List<ReporteInversionOverhaulDTO>();
            List<tblM_CatSubConjunto> subconjuntos = _context.tblM_CatSubConjunto.Where(x => x.estatus).ToList();
            List<tblM_CatServicioOverhaul> servicios = _context.tblM_CatServicioOverhaul.Where(x => x.estatus).ToList();
            var remociones = _context.tblM_trackComponentes.Where(x => x.estatus > 1 && x.estatus != 3 && (x.fecha ?? default(DateTime)).Year == anio).ToList();
            var almacenesID = _context.tblM_CatLocacionesComponentes.Where(x => x.tipoLocacion == 1).Select(x => x.id).ToList();
            var reportesRemocion = _context.tblM_ReporteRemocionComponente.Where(x => x.estatus == 5 && x.motivoRemocionID < 2 && almacenesID.Contains(x.destinoID) && x.fechaRemocion.Year == anio).ToList();
            var trackServicios = _context.tblM_trackServicioOverhaul.ToList();

            var calendarios = _context.tblM_CalendarioPlaneacionOverhaul.Where(x => x.anio == anio).ToList();
            var calendariosIDs = calendarios.Select(x => x.id);
            List<tblM_CapPlaneacionOverhaul> detallesCalendarios = _context.tblM_CapPlaneacionOverhaul.Where(x => calendariosIDs.Contains(x.calendarioID)).ToList();
            List<ComponentePlaneacionInversionDTO> detallesCalendariosProgramados = new List<ComponentePlaneacionInversionDTO>();
            List<ComponentePlaneacionInversionDTO> detallesCalendariosNoProgramados = new List<ComponentePlaneacionInversionDTO>();
            foreach (var item in detallesCalendarios)
            {
                var componentesDetalleCalendario = JsonConvert.DeserializeObject<List<ComponentePlaneacionInversionDTO>>(item.idComponentes);
                foreach (var item2 in componentesDetalleCalendario)
                {
                    item2.planeacionID = item.id;
                    item2.obra = item.calendario.obraID;
                    item2.maquinaID = item.maquinaID;
                    item2.fecha = item.fecha;
                    item2.tipoParo = item.tipo;
                    item2.estatus = item.estatus;
                    item2.ritmo = item.ritmo;
                    if (componentesDetallesRaw.Contains(item2.componenteID)) { detallesCalendariosProgramados.Add(item2); }
                    else { detallesCalendariosNoProgramados.Add(item2); }
                }
            }
            var maquinasModelo = _context.tblM_CatMaquina.Where(x => (modelos.Count() == 0 ? true : modelos.Contains(x.modeloEquipoID))).Select(x => x.id).ToList();
            detallesCalendariosNoProgramados = detallesCalendariosNoProgramados.Where(x => (obras.Count() == 0 ? true : obras.Contains(x.obra)) && maquinasModelo.Contains(x.maquinaID)).ToList();

            foreach (var item in detallesRaw)
            {
                if (item.esServicio) { 
                    ReporteInversionOverhaulDTO detallePresupuesto = new ReporteInversionOverhaulDTO();
                    var servicio = servicios.FirstOrDefault(x => x.id == item.componenteID);
                    var auxRemocion = trackServicios.Where(x => x.servicioID == item.componenteID && x.fecha.Year == anio).ToList();

                    var numRemociones = auxRemocion.Count();
                    if (numRemociones == 0)
                    {
                        var mesIndex = item.fecha.Month - 1;
                        detallePresupuesto.componente = servicio == null ? "" : servicio.servicio.descripcion;
                        detallePresupuesto.equipo = item.maquina == null ? "" : item.maquina.noEconomico;
                        detallePresupuesto.erogado = 0;
                        detallePresupuesto.fechaRemocion = "--";
                        detallePresupuesto.horasComponente = item.horasCiclo;
                        detallePresupuesto.mes = meses[mesIndex];
                        detallePresupuesto.numMes = mesIndex;
                        detallePresupuesto.numTipoParo = item.tipo;
                        detallePresupuesto.paroID = 0;
                        detallePresupuesto.paroTerminado = false;
                        detallePresupuesto.presupuesto = item.costoPresupuesto;
                        detallePresupuesto.proximoPCR = item.fecha.ToString("dd/MM/yyyy");
                        detallePresupuesto.ritmo = 0;
                        detallePresupuesto.subconjunto = servicio == null ? "N/A" : servicio.servicio.descripcion;
                        detallePresupuesto.target = servicio == null ? 0 : servicio.cicloVidaHoras;
                        detallePresupuesto.tipoParo = auxTipoParo[item.tipo] == null ? "" : auxTipoParo[item.tipo];
                        detallePresupuesto.programado = true;
                        data.Add(detallePresupuesto);
                    }
                    else
                    {
                        if (numRemociones > 0)
                        {
                            var mesIndex = auxRemocion[0].fecha.Month - 1;
                            detallePresupuesto.componente = servicio == null ? "" : servicio.servicio.descripcion;
                            detallePresupuesto.equipo = item.maquina == null ? "" : item.maquina.noEconomico;
                            detallePresupuesto.erogado = item.costoPresupuesto;
                            detallePresupuesto.fechaRemocion = auxRemocion[0].fecha.ToString("dd/MM/yyyy");
                            detallePresupuesto.horasComponente = item.horasCiclo;
                            detallePresupuesto.mes = meses[mesIndex];
                            detallePresupuesto.numMes = mesIndex;
                            detallePresupuesto.numTipoParo = item.tipo;
                            detallePresupuesto.paroID = 0;
                            detallePresupuesto.paroTerminado = true;
                            detallePresupuesto.presupuesto = item.costoPresupuesto;
                            detallePresupuesto.proximoPCR = item.fecha.ToString("dd/MM/yyyy");
                            detallePresupuesto.ritmo = 0;
                            detallePresupuesto.subconjunto = servicio == null ? "N/A" : servicio.servicio.descripcion;
                            detallePresupuesto.target = servicio == null ? 0 : servicio.cicloVidaHoras;
                            detallePresupuesto.tipoParo = auxTipoParo[item.tipo] == null ? "" : auxTipoParo[item.tipo];
                            detallePresupuesto.programado = true;
                            data.Add(detallePresupuesto);

                            if (numRemociones > 1)
                            {
                                for (int i = 2; i < numRemociones; i++)
                                {
                                    ReporteInversionOverhaulDTO detallePresupuestoFalla = new ReporteInversionOverhaulDTO();
                                    var mesIndexFalla = auxRemocion[i].fecha.Month - 1;
                                    detallePresupuestoFalla.componente = servicio == null ? "" : servicio.servicio.descripcion;
                                    detallePresupuestoFalla.equipo = item.maquina == null ? "" : item.maquina.noEconomico;
                                    detallePresupuestoFalla.erogado = item.costoPresupuesto;
                                    detallePresupuestoFalla.fechaRemocion = auxRemocion[i].fecha.ToString("dd/MM/yyyy");
                                    detallePresupuestoFalla.horasComponente = item.horasCiclo;
                                    detallePresupuestoFalla.mes = meses[mesIndexFalla];
                                    detallePresupuestoFalla.numMes = mesIndexFalla;
                                    detallePresupuestoFalla.numTipoParo = 3;
                                    detallePresupuestoFalla.paroID = 0;
                                    detallePresupuestoFalla.paroTerminado = true;
                                    detallePresupuestoFalla.presupuesto = item.costoPresupuesto;
                                    detallePresupuestoFalla.proximoPCR = item.fecha.ToString("dd/MM/yyyy");
                                    detallePresupuestoFalla.ritmo = 0;
                                    detallePresupuestoFalla.subconjunto = servicio == null ? "N/A" : servicio.servicio.descripcion;
                                    detallePresupuestoFalla.target = servicio == null ? 0 : servicio.cicloVidaHoras;
                                    detallePresupuestoFalla.tipoParo = auxTipoParo[3];
                                    detallePresupuestoFalla.programado = false;
                                    data.Add(detallePresupuestoFalla);
                                }

                            }
                        }
                    }
                }
                else { 
                    ReporteInversionOverhaulDTO detallePresupuesto = new ReporteInversionOverhaulDTO();
                    var subconjunto = subconjuntos.FirstOrDefault(x => x.id == item.subconjuntoID);
                    var auxRemocion = remociones.Where(x => x.componenteID == item.componenteID).ToList();
                    var auxReportesRemocion = reportesRemocion.Where(x => x.componenteRemovidoID == item.componenteID).OrderByDescending(x => x.fechaRemocion).ToList();
                    var numRemociones = auxRemocion.Count();
                    var auxHorometro = remocionDAO.GetHrsCicloActualComponente(item.componenteID, item.fecha, 0, true);
                    if (numRemociones == 0)
                    {
                        decimal erogado = 0;
                        DateTime fechaRemocion = new DateTime();
                        if (auxReportesRemocion.Count() > 0) erogado = auxReportesRemocion[0].componenteInstalado.costo;
                        if (auxReportesRemocion.Count() > 0) fechaRemocion = auxReportesRemocion[0].fechaRemocion;
                        var mesIndex = item.fecha.Month - 1;
                        detallePresupuesto.componente = item.componente == null ? "" : item.componente.noComponente;
                        detallePresupuesto.equipo = item.maquina == null ? "" : item.maquina.noEconomico;
                        detallePresupuesto.erogado = erogado;
                        detallePresupuesto.fechaRemocion = auxReportesRemocion.Count() > 0 ? fechaRemocion.ToString("dd/MM/yyyy") : "--";
                        detallePresupuesto.horasComponente = auxHorometro;
                        detallePresupuesto.mes = meses[mesIndex];
                        detallePresupuesto.numMes = mesIndex;
                        detallePresupuesto.numTipoParo = item.tipo;
                        detallePresupuesto.paroID = 0;
                        detallePresupuesto.paroTerminado = auxReportesRemocion.Count() > 0;
                        detallePresupuesto.presupuesto = item.costoPresupuesto;
                        detallePresupuesto.proximoPCR = item.fecha.ToString("dd/MM/yyyy");
                        detallePresupuesto.ritmo = 0;
                        detallePresupuesto.subconjunto = subconjunto == null ? "N/A" : subconjunto.descripcion;
                        detallePresupuesto.target = item.componente == null ? 0 : item.componente.cicloVidaHoras;
                        detallePresupuesto.tipoParo = auxTipoParo[item.tipo] == null ? "" : auxTipoParo[item.tipo];
                        detallePresupuesto.programado = true;
                        data.Add(detallePresupuesto);
                    }
                    else
                    {
                        if (numRemociones > 0)
                        {
                            var mesIndex = (auxRemocion[0].fecha ?? default(DateTime)).Month - 1;
                            detallePresupuesto.componente = item.componente == null ? "" : item.componente.noComponente;
                            detallePresupuesto.equipo = item.maquina == null ? "" : item.maquina.noEconomico;
                            detallePresupuesto.erogado = auxRemocion[0].estatus == 3 ? CalcularCostoDesecho(auxRemocion[0]) : auxRemocion[0].costoCRC;
                            detallePresupuesto.fechaRemocion = (auxRemocion[0].fecha ?? default(DateTime)).ToString("dd/MM/yyyy");
                            detallePresupuesto.horasComponente = auxHorometro;
                            detallePresupuesto.mes = meses[mesIndex];
                            detallePresupuesto.numMes = mesIndex;
                            detallePresupuesto.numTipoParo = item.tipo;
                            detallePresupuesto.paroID = 0;
                            detallePresupuesto.paroTerminado = true;
                            detallePresupuesto.presupuesto = item.costoPresupuesto;
                            detallePresupuesto.proximoPCR = item.fecha.ToString("dd/MM/yyyy");
                            detallePresupuesto.ritmo = 0;
                            detallePresupuesto.subconjunto = subconjunto == null ? "N/A" : subconjunto.descripcion;
                            detallePresupuesto.target = item.componente == null ? 0 : item.componente.cicloVidaHoras;
                            detallePresupuesto.tipoParo = auxTipoParo[item.tipo] == null ? "" : auxTipoParo[item.tipo];
                            detallePresupuesto.programado = true;
                            data.Add(detallePresupuesto);

                            if (numRemociones > 1)
                            {
                                for (int i = 2; i < numRemociones; i++)
                                {
                                    ReporteInversionOverhaulDTO detallePresupuestoFalla = new ReporteInversionOverhaulDTO();
                                    var mesIndexFalla = (auxRemocion[i].fecha ?? default(DateTime)).Month - 1;
                                    detallePresupuestoFalla.componente = item.componente == null ? "" : item.componente.noComponente;
                                    detallePresupuestoFalla.equipo = item.maquina == null ? "" : item.maquina.noEconomico;
                                    detallePresupuestoFalla.erogado = auxRemocion[i].estatus == 3 ? CalcularCostoDesecho(auxRemocion[i]) : auxRemocion[i].costoCRC;
                                    detallePresupuestoFalla.fechaRemocion = (auxRemocion[i].fecha ?? default(DateTime)).ToString("dd/MM/yyyy");
                                    detallePresupuestoFalla.horasComponente = auxHorometro;
                                    detallePresupuestoFalla.mes = meses[mesIndexFalla];
                                    detallePresupuestoFalla.numMes = mesIndexFalla;
                                    detallePresupuestoFalla.numTipoParo = 3;
                                    detallePresupuestoFalla.paroID = 0;
                                    detallePresupuestoFalla.paroTerminado = true;
                                    detallePresupuestoFalla.presupuesto = item.costoPresupuesto;
                                    detallePresupuestoFalla.proximoPCR = item.fecha.ToString("dd/MM/yyyy");
                                    detallePresupuestoFalla.ritmo = 0;
                                    detallePresupuestoFalla.subconjunto = subconjunto == null ? "N/A" : subconjunto.descripcion;
                                    detallePresupuestoFalla.target = item.componente == null ? 0 : item.componente.cicloVidaHoras;
                                    detallePresupuestoFalla.tipoParo = auxTipoParo[3];
                                    detallePresupuestoFalla.programado = false;
                                    data.Add(detallePresupuestoFalla);
                                }

                            }
                        }
                    }
                }
            }
            foreach (var item in detallesCalendariosNoProgramados)
            {
                ReporteInversionOverhaulDTO detallePresupuesto = new ReporteInversionOverhaulDTO();
                var auxComponente = _context.tblM_CatComponente.FirstOrDefault(x => x.id == item.componenteID);
                var auxMaquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == item.maquinaID);
                var auxHorometro = remocionDAO.GetHrsCicloActualComponente(item.componenteID, item.fecha, 0, true);
                if (auxComponente != null)
                {
                    var subconjunto = auxComponente.subConjunto == null ? "" : auxComponente.subConjunto.descripcion;
                    var auxRemocion = remociones.Where(x => x.componenteID == item.componenteID).ToList();
                    var numRemociones = auxRemocion.Count();
                    for (int i = 0; i < numRemociones; i++)
                    {
                        ReporteInversionOverhaulDTO detallePresupuestoFalla = new ReporteInversionOverhaulDTO();
                        var mesIndexFalla = (auxRemocion[i].fecha ?? default(DateTime)).Month - 1;
                        detallePresupuestoFalla.componente = auxComponente.noComponente;
                        detallePresupuestoFalla.equipo = auxMaquina == null ? "" : auxMaquina.noEconomico;
                        detallePresupuestoFalla.erogado = auxRemocion[i].estatus == 3 ? CalcularCostoDesecho(auxRemocion[i]) : auxRemocion[i].costoCRC;
                        detallePresupuestoFalla.fechaRemocion = (auxRemocion[i].fecha ?? default(DateTime)).ToString("dd/MM/yyyy");
                        detallePresupuestoFalla.horasComponente = auxHorometro;
                        detallePresupuestoFalla.mes = meses[mesIndexFalla];
                        detallePresupuestoFalla.numMes = mesIndexFalla;
                        detallePresupuestoFalla.numTipoParo = 3;
                        detallePresupuestoFalla.paroID = 0;
                        detallePresupuestoFalla.paroTerminado = true;
                        detallePresupuestoFalla.presupuesto = 0;
                        detallePresupuestoFalla.proximoPCR = item.fecha.ToString("dd/MM/yyyy");
                        detallePresupuestoFalla.ritmo = 0;
                        detallePresupuestoFalla.subconjunto = subconjunto;
                        detallePresupuestoFalla.target = auxComponente.cicloVidaHoras;
                        detallePresupuestoFalla.tipoParo = auxTipoParo[3];
                        detallePresupuestoFalla.programado = false;
                        data.Add(detallePresupuestoFalla);
                    }

                }
            }
            return data;
        }

        private decimal CalcularCostoDesecho(tblM_trackComponentes auxRemocion)
        {
            decimal data = 0;
            try
            {
                var reporteRemocion = _context.tblM_ReporteRemocionComponente.Where(x => x.componenteRemovidoID == auxRemocion.componenteID && x.fechaRemocion <= auxRemocion.fecha).OrderByDescending(x => x.fechaRemocion).FirstOrDefault();
                if (reporteRemocion != null)
                {
                    var componente = _context.tblM_CatComponente.FirstOrDefault(x => x.id == reporteRemocion.componenteInstaladoID);
                    if (componente != null && componente.ordenCompra != null && componente.ordenCompra.Trim() != "")
                    {
                        data = componente.costo;
                    }
                }
            }
            catch (Exception e) 
            {
                data = 0;
            }
            return data;
        }

        private int GetVidasComponente(int index)
        {
            int data = 0;
            var tracks = _context.tblM_trackComponentes.Where(x => x.componenteID == index).ToList();
            if (tracks.Count() > 0) { data = tracks.Where(x => x.reciclado == true).Count(); }
            return data;
        }

        private int GetVidasServicio(int index)
        {
            var tracks = _context.tblM_trackServicioOverhaul.Where(x => x.servicioID == index).ToList();
            return tracks.Count();
        }

        public List<ComboDTO> fillCboAnioPresupuesto()
        {
            List<ComboDTO> data = new List<ComboDTO>();
            int anioFin = _context.tblM_CalendarioPlaneacionOverhaul.Max(x => x.anio);
            int anioInicio = _context.tblM_CalendarioPlaneacionOverhaul.Min(x => x.anio);
            for (int i = anioInicio; i <= anioFin; i++)
            {
                ComboDTO aux = new ComboDTO();
                aux.Value = i.ToString();
                aux.Text = i.ToString();
                data.Add(aux);
            }
            return data.OrderByDescending(x => x.Value).ToList();
        }

        public int GuardarCostoPresupuesto(int componenteID, int maquinaID, decimal costo, decimal costoSugerido, int eventoID, int modelo, int anio, int presupuestoID, int vidas, bool esServicio)
        {
            using (var context = new MainContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        int presupID = 0;
                        var presupuestoGuardado = context.tblM_PresupuestoOverhaul.FirstOrDefault(x => x.id == presupuestoID);

                        if (presupuestoGuardado != null)
                        {
                            presupID = presupuestoID;
                            var detalleGuardado = context.tblM_DetallePresupuestoOverhaul.FirstOrDefault(x => x.presupuestoID == presupuestoGuardado.id && x.componenteID == componenteID && x.esServicio == esServicio);
                            tblM_DetallePresupuestoOverhaul presupuesto = new tblM_DetallePresupuestoOverhaul();
                            if (detalleGuardado != null)
                            {
                                detalleGuardado.costoPresupuesto = costo;
                                detalleGuardado.estado = 1;
                            }
                            else
                            {
                                decimal horaCicloActual = 0;
                                decimal horasAcumuladas = 0;
                                int subconjuntoID = 0;
                                if (esServicio)
                                {
                                    var servicio = context.tblM_CatServicioOverhaul.FirstOrDefault(x => x.id == componenteID);
                                    horaCicloActual = servicio.horasCicloActual;
                                    horasAcumuladas = servicio.horasCicloActual;
                                    subconjuntoID = servicio.tipoServicioID;
                                }
                                else
                                {
                                    var componente = context.tblM_CatComponente.FirstOrDefault(x => x.id == componenteID);
                                    horaCicloActual = componente.horaCicloActual;
                                    horasAcumuladas = componente.horasAcumuladas;
                                    subconjuntoID = componente.subConjuntoID ?? default(int);
                                }

                                var maquina = context.tblM_CatMaquina.FirstOrDefault(x => x.id == maquinaID);
                                tblM_CapPlaneacionOverhaul evento = new tblM_CapPlaneacionOverhaul();

                                var calendarios = context.tblM_CalendarioPlaneacionOverhaul.Where(x => x.anio == anio && x.grupoMaquinaID == 0 && x.modeloMaquinaID == 0 && x.subConjuntoID == null && x.estatus == 0).Select(x => x.id).ToList();
                                var eventosPresupuesto = context.tblM_CapPlaneacionOverhaul.Where(x => calendarios.Contains(x.calendarioID)).ToList();
                                foreach (var item in eventosPresupuesto)
                                {
                                    var auxComponentes = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(item.idComponentes).ToList();
                                    foreach (var item2 in auxComponentes)
                                    {
                                        if (item2.componenteID == componenteID && (esServicio ? item2.Tipo == 1 : item2.Tipo != 1)) evento = item;
                                    }
                                }


                                presupuesto.id = 0;
                                presupuesto.componenteID = componenteID;
                                presupuesto.maquinaID = maquinaID;
                                presupuesto.fecha = evento.fecha;
                                presupuesto.tipo = evento.tipo;
                                presupuesto.costoSugerido = costoSugerido;
                                presupuesto.costoPresupuesto = costo;
                                presupuesto.horasCiclo = horaCicloActual;
                                presupuesto.horasAcumuladas = horasAcumuladas;
                                presupuesto.estado = 1;
                                presupuesto.presupuestoID = presupuestoGuardado.id;
                                presupuesto.subconjuntoID = subconjuntoID;
                                presupuesto.vida = vidas;
                                presupuesto.programado = true;
                                presupuesto.obra = evento.calendario.obraID == null ? maquina.centro_costos : evento.calendario.obraID;
                                presupuesto.esServicio = esServicio;
                                context.tblM_DetallePresupuestoOverhaul.Add(presupuesto);

                            }
                            context.SaveChanges();
                            var detallesPresupuestados = context.tblM_DetallePresupuestoOverhaul.Where(x => x.presupuestoID == presupID && x.estado == 0);

                            if (detallesPresupuestados.Count() == 0 && presupuestoGuardado.estado == 0 && presupuesto == null)
                            {
                                presupuestoGuardado.estado = 1;
                            }
                            List<FechasPresupuestoObraDTO> obras = JsonConvert.DeserializeObject<List<FechasPresupuestoObraDTO>>(presupuestoGuardado.JsonObras);
                            foreach (var obra in obras)
                            {
                                var detNoGuardado = detallesPresupuestados.Where(x => x.obra == obra.obraID && x.estado == 0).Count();
                                if (detNoGuardado == 0 && obra.estado == 0) { obra.estado = 1; }
                            }
                            presupuestoGuardado.JsonObras = JsonConvert.SerializeObject(obras);
                        }

                        else
                        {
                            var maquinas = context.tblM_CatMaquina.Where(x => x.modeloEquipoID == modelo).ToList();
                            var auxMaquinas = maquinas.Select(x => x.id).ToList();
                            var trackActual = context.tblM_trackComponentes.GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault());
                            trackActual = trackActual.Where(x => !(x.tipoLocacion ?? false) && auxMaquinas.Contains(x.locacionID ?? default(int)));
                            //DateTime limiteFecha = DateTime.Today.AddYears(-1);

                            var calendarios = context.tblM_CalendarioPlaneacionOverhaul.Where(x => x.anio == anio && x.grupoMaquinaID == 0 && x.modeloMaquinaID == 0 && x.subConjuntoID == null && x.estatus == 0).Select(x => x.id).ToList();

                            var eventosPresupuesto = context.tblM_CapPlaneacionOverhaul.Where(x => calendarios.Contains(x.calendarioID)).ToList();
                            var compPresup = new List<int>();
                            var eventoComp = new List<ComboDTO>();
                            var prueba2 = new List<PropiedadOverhaulDTO>();
                            foreach (var item in eventosPresupuesto)
                            {
                                compPresup.AddRange(JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(item.idComponentes).Select(x => x.componenteID));
                                foreach (var item2 in compPresup)
                                {
                                    ComboDTO auxEventoComp = new ComboDTO();
                                    auxEventoComp.Value = item2.ToString();
                                    auxEventoComp.Text = item.id.ToString();
                                    eventoComp.Add(auxEventoComp);
                                }
                            }
                            compPresup = compPresup.Distinct().ToList();
                            var componentes = trackActual.Where(x => compPresup.Contains(x.componenteID)).ToList();
                            var joinMaquinas = maquinas.Join(componentes, (x => x.id), (y => y.locacionID), ((x, y) => new { x, y })).ToList();
                            var obrasIDs = joinMaquinas.Select(x => x.x.centro_costos).Distinct().ToList();
                            List<FechasPresupuestoObraDTO> obras = new List<FechasPresupuestoObraDTO>();
                            foreach (var obraID in obrasIDs)
                            {
                                FechasPresupuestoObraDTO auxObra = new FechasPresupuestoObraDTO();
                                auxObra.estado = 0;
                                auxObra.obraID = obraID;
                                var obraAlmacenada = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == obraID);
                                auxObra.obra = obraAlmacenada != null ? obraAlmacenada.descripcion : obraID;
                                auxObra.fechaEnvio = default(DateTime);
                                auxObra.fechaVoBo1 = default(DateTime);
                                auxObra.fechaVoBo2 = default(DateTime);
                                auxObra.fechaAutorizacion = default(DateTime);
                                obras.Add(auxObra);
                            }

                            //Guardar Presupuestos
                            tblM_PresupuestoOverhaul nuevoPresupuesto = new tblM_PresupuestoOverhaul();
                            nuevoPresupuesto.modelo = modelo;
                            nuevoPresupuesto.anio = anio;
                            nuevoPresupuesto.estado = 0;
                            nuevoPresupuesto.JsonObras = JsonConvert.SerializeObject(obras);
                            nuevoPresupuesto.fecha = DateTime.Now;
                            context.tblM_PresupuestoOverhaul.Add(nuevoPresupuesto);
                            context.SaveChanges();

                            presupID = nuevoPresupuesto.id;
                            //Guardar Detalle Presupuesto

                            var subconjuntosPres = componentes.Select(x => x.componente.subConjuntoID).Distinct().ToList();
                            decimal presupuestoSugerido;
                            foreach (var item in subconjuntosPres)
                            {
                                var compSubPres = componentes.Where(x => x.componente.subConjuntoID == item);

                                var trackPresupuesto = context.tblM_trackComponentes.Where(x => x.componente.modeloEquipoID == modelo && x.costoCRC != 0 && /*x.fecha > limiteFecha &&*/ x.componente.subConjuntoID == item);
                                presupuestoSugerido = 0;
                                if (trackPresupuesto.Count() > 0)
                                {
                                    presupuestoSugerido = trackPresupuesto.Sum(x => x.costoCRC) / trackPresupuesto.Count();
                                }

                                foreach (var item2 in compSubPres)
                                {
                                    tblM_DetallePresupuestoOverhaul aux = new tblM_DetallePresupuestoOverhaul();
                                    int eventoCompID = 0;
                                    var evento = eventoComp.FirstOrDefault(x => x.Value == item2.componenteID.ToString());
                                    if (evento != null) { Int32.TryParse(evento.Text, out eventoCompID); }
                                    var eventoCompleto = context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.id == eventoCompID);
                                    var maquina = context.tblM_CatMaquina.FirstOrDefault(x => x.id == item2.locacionID);
                                    aux.componenteID = item2.componenteID;
                                    aux.costoPresupuesto = item2.componenteID == componenteID ? costo : presupuestoSugerido;
                                    aux.costoSugerido = presupuestoSugerido;
                                    aux.estado = item2.componenteID == componenteID ? 1 : 0;
                                    aux.horasAcumuladas = item2.componente.horasAcumuladas;
                                    aux.horasCiclo = item2.componente.horaCicloActual;
                                    aux.id = 0;
                                    aux.maquinaID = item2.locacionID ?? default(int);
                                    aux.presupuestoID = nuevoPresupuesto.id;
                                    aux.subconjuntoID = item ?? default(int);
                                    aux.obra = maquina.centro_costos;
                                    aux.vida = vidas;
                                    aux.fecha = eventoCompleto != null ? eventoCompleto.fecha : default(DateTime);
                                    aux.tipo = eventoCompleto != null ? eventoCompleto.tipo : -1;
                                    aux.costoReal = 0;
                                    aux.programado = true;
                                    context.tblM_DetallePresupuestoOverhaul.Add(aux);
                                }
                            }
                        }
                        context.SaveChanges();
                        dbContextTransaction.Commit();
                        return presupID;
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        return 0;
                    }
                }
            }
        }

        public int IniciarPresupuesto(int modelo, int anio)
        {
            using (var context = new MainContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var maquinas = context.tblM_CatMaquina.Where(x => x.modeloEquipoID == modelo).ToList();
                        var auxMaquinas = maquinas.Select(x => x.id).ToList();

                        //DateTime limiteFecha = DateTime.Today.AddYears(-1);

                        var calendarios = context.tblM_CalendarioPlaneacionOverhaul.Where(x => x.anio == anio && x.grupoMaquinaID == 0 && x.modeloMaquinaID == 0 && x.subConjuntoID == null && x.estatus == 0).Select(x => x.id).ToList();

                        var eventosPresupuesto = context.tblM_CapPlaneacionOverhaul.Where(x => calendarios.Contains(x.calendarioID) && auxMaquinas.Contains(x.maquinaID)).ToList();
                        var compPresup = new List<int>();
                        var eventoComp = new List<ComboDTO>();
                        var prueba2 = new List<PropiedadOverhaulDTO>();
                        foreach (var item in eventosPresupuesto)
                        {
                            compPresup.AddRange(JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(item.idComponentes).Select(x => x.componenteID));
                            foreach (var item2 in compPresup)
                            {
                                ComboDTO auxEventoComp = new ComboDTO();
                                auxEventoComp.Value = item2.ToString();
                                auxEventoComp.Text = item.id.ToString();
                                eventoComp.Add(auxEventoComp);
                            }
                        }
                        compPresup = compPresup.Distinct().ToList();
                        var trackActual = _context.tblM_trackComponentes.Where(x => compPresup.Contains(x.componenteID) && !(x.tipoLocacion ?? false)).GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault())
                        .ToList();
                        var componentes = trackActual.Where(x => compPresup.Contains(x.componenteID));
                        var joinMaquinas = maquinas.Join(componentes, (x => x.id), (y => y.locacionID), ((x, y) => new { x, y })).ToList();
                        var obrasIDs = joinMaquinas.Select(x => x.x.centro_costos).Distinct().ToList();
                        List<FechasPresupuestoObraDTO> obras = new List<FechasPresupuestoObraDTO>();
                        foreach (var obraID in obrasIDs)
                        {
                            FechasPresupuestoObraDTO auxObra = new FechasPresupuestoObraDTO();
                            auxObra.estado = 1;
                            auxObra.obraID = obraID;
                            var obraAlmacenada = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == obraID);
                            auxObra.obra = obraAlmacenada != null ? obraAlmacenada.descripcion : obraID;
                            auxObra.fechaEnvio = default(DateTime);
                            auxObra.fechaVoBo1 = default(DateTime);
                            auxObra.fechaVoBo2 = default(DateTime);
                            auxObra.fechaAutorizacion = default(DateTime);
                            obras.Add(auxObra);
                        }

                        //Guardar Presupuestos
                        tblM_PresupuestoOverhaul nuevoPresupuesto = new tblM_PresupuestoOverhaul();
                        nuevoPresupuesto.modelo = modelo;
                        nuevoPresupuesto.anio = anio;
                        nuevoPresupuesto.estado = 0;
                        nuevoPresupuesto.JsonObras = JsonConvert.SerializeObject(obras);
                        nuevoPresupuesto.fecha = DateTime.Now;
                        nuevoPresupuesto.cerrado = false;
                        context.tblM_PresupuestoOverhaul.Add(nuevoPresupuesto);
                        context.SaveChanges();

                        int presupID = nuevoPresupuesto.id;
                        //Guardar Detalle Presupuesto

                        var subconjuntosPres = componentes.Select(x => x.componente.subConjuntoID).Distinct().ToList();
                        decimal presupuestoSugerido;
                        foreach (var item in subconjuntosPres)
                        {
                            var compSubPres = componentes.Where(x => x.componente.subConjuntoID == item && compPresup.Contains(x.componenteID));

                            var trackPresupuesto = context.tblM_trackComponentes.Where(x => x.componente.modeloEquipoID == modelo && x.costoCRC > 0 /*&& x.fecha > limiteFecha*/ && x.componente.subConjuntoID == item);
                            //var trackPresupuesto = _context.tblM_trackComponentes.Where(x => compPresup.Contains(x.componenteID) && !(x.tipoLocacion ?? false) && x.costoCRC > 0).GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault()).ToList();
                            presupuestoSugerido = 0;
                            if (trackPresupuesto.Count() > 0)
                            {
                                presupuestoSugerido = trackPresupuesto.Sum(x => x.costoCRC) / trackPresupuesto.Count();
                            }

                            foreach (var item2 in compSubPres)
                            {
                                tblM_DetallePresupuestoOverhaul aux = new tblM_DetallePresupuestoOverhaul();
                                int eventoCompID = 0;
                                var evento = eventoComp.FirstOrDefault(x => x.Value == item2.componenteID.ToString());
                                if (evento != null) { Int32.TryParse(evento.Text, out eventoCompID); }
                                var eventoCompleto = context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.id == eventoCompID);
                                var maquina = context.tblM_CatMaquina.FirstOrDefault(x => x.id == item2.locacionID);
                                int vidasActual = GetVidasComponente(item2.componenteID) + item2.componente.vidaInicio;
                                aux.componenteID = item2.componenteID;
                                aux.costoPresupuesto = presupuestoSugerido;
                                aux.costoSugerido = presupuestoSugerido;
                                aux.estado = 1;
                                aux.horasAcumuladas = item2.componente.horasAcumuladas;
                                aux.horasCiclo = item2.componente.horaCicloActual;
                                aux.id = 0;
                                aux.maquinaID = item2.locacionID ?? default(int);
                                aux.presupuestoID = nuevoPresupuesto.id;
                                aux.subconjuntoID = item ?? default(int);
                                aux.obra = maquina.centro_costos;
                                aux.vida = vidasActual;
                                aux.fecha = eventoCompleto != null ? eventoCompleto.fecha : default(DateTime);
                                aux.tipo = eventoCompleto != null ? eventoCompleto.tipo : -1;
                                aux.costoReal = 0;
                                aux.programado = true;
                                context.tblM_DetallePresupuestoOverhaul.Add(aux);
                            }
                        }
                        context.SaveChanges();
                        dbContextTransaction.Commit();
                        return presupID;
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        return 0;
                    }
                }
            }
        }

        public bool CerrarPresupuesto(int presupuestoID)
        {
            try
            {
                var presupuesto = _context.tblM_PresupuestoOverhaul.FirstOrDefault(x => x.id == presupuestoID);
                presupuesto.cerrado = true;
                presupuesto.estado = 1;
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<tblM_DetallePresupuestoOverhaul> GetDetalleModalAuto(int presupuestoID, string areaCuenta)
        {
            List<tblM_DetallePresupuestoOverhaul> data = new List<tblM_DetallePresupuestoOverhaul>();
            var obra = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == areaCuenta);
            data = _context.tblM_DetallePresupuestoOverhaul.Where(x => x.presupuestoID == presupuestoID && obra == null ? true : x.obra == areaCuenta).ToList();
            return data;
        }


        public List<DetallePresupuestoDTO> GetDetallePresAuto(List<string> obras, int modelo, int anio)
        {
            List<DetallePresupuestoDTO> data = new List<DetallePresupuestoDTO>();

            var maquinas = _context.tblM_CatMaquina.Where(x => x.estatus == 1 && x.modeloEquipoID == modelo && (obras.Contains(x.centro_costos))).ToList();
            var auxMaquinas = maquinas.Select(x => x.id).ToList();
            var trackActual = _context.tblM_trackComponentes.GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault());
            trackActual = trackActual.Where(x => !(x.tipoLocacion ?? false) && auxMaquinas.Contains(x.locacionID ?? default(int)));

            var calendarios = _context.tblM_CalendarioPlaneacionOverhaul.Where(x => (anio == -1 ? true : x.anio == anio) && x.grupoMaquinaID == 0 && x.modeloMaquinaID == 0 && obras.Contains(x.obraID)).Select(x => x.id).ToList();
            var componentesAPresupuestar = _context.tblM_CapPlaneacionOverhaul.Where(x => calendarios.Contains(x.calendarioID)).ToList();
            var prueba = new List<int>();
            var prueba2 = new List<PropiedadOverhaulDTO>();
            foreach (var item in componentesAPresupuestar)
            {
                prueba.AddRange(JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(item.idComponentes).Select(x => x.componenteID));
                foreach (var item2 in prueba)
                {
                    var auxComponente = new PropiedadOverhaulDTO();
                    auxComponente.Valor = item2;
                    auxComponente.Propiedad = item.id.ToString();
                    prueba2.Add(auxComponente);
                }
            }

            trackActual = trackActual.Where(x => prueba.Contains(x.componenteID));
            //int auxEventoID;
            foreach (var item in trackActual.ToList())
            {
                if (prueba.Contains(item.componenteID))
                {
                    DetallePresupuestoDTO aux = new DetallePresupuestoDTO();
                    //auxEventoID = 0;
                    //Int32.TryParse(prueba2.FirstOrDefault(y => y.Valor == item.componenteID).Propiedad, out auxEventoID);
                    //aux.eventoID = auxEventoID;
                    //var presupuestoAlmacenado = _context.tblM_DetallePresupuestoOverhaul.FirstOrDefault(x => x.componenteID == item.componenteID && x.maquinaID == item.locacionID && x.eventoID == auxEventoID);
                    //if (presupuestoAlmacenado != null)
                    //{
                    //    aux.costo = presupuestoAlmacenado.costoPresupuesto;
                    //    aux.costoSugerido = presupuestoAlmacenado.costoSugerido;
                    //    aux.color = "#b4e4b4";
                    //}
                    //else
                    //{
                    aux.costo = item.componente.costo;
                    aux.costoSugerido = item.componente.costo;
                    aux.color = "#f9f9f9";
                    //}
                    aux.componenteID = item.componenteID;
                    aux.noComponente = item.componente.noComponente;
                    aux.noEconomico = maquinas.FirstOrDefault(x => x.id == (item.locacionID ?? default(int))).noEconomico;
                    aux.maquinaID = item.locacionID ?? default(int);
                    aux.horometroAcumulado = item.componente.horasAcumuladas;
                    aux.horometroCiclo = item.componente.horaCicloActual;
                    aux.target = item.componente.cicloVidaHoras;
                    data.Add(aux);
                }
            }

            return data;
        }

        public List<PresupuestoPorObraDTO> CargarTblAvance(List<string> obras, int modeloID, int anio, int estatus)
        {
            List<PresupuestoPorObraDTO> data = new List<PresupuestoPorObraDTO>();
            if (anio == -1) anio = DateTime.Now.Year;
            var fechaInicio = new DateTime(anio, 1, 1);
            var presupuestosGuardados = _context.tblM_PresupuestoOverhaul.Where(x => x.estado > -1 && (anio != -1 ? x.anio == anio : true) && (modeloID != 0 ? x.modelo == modeloID : true)).ToList();
            var componentes = _context.tblM_trackComponentes.Where(x => x.fecha >= fechaInicio && x.estatus == 10).ToList();
            foreach (var item in presupuestosGuardados)
            {
                var detalle = _context.tblM_DetallePresupuestoOverhaul.Where(x => x.presupuestoID == item.id && x.estado == 1 && x.programado).ToList();

                var componente = _context.tblM_CatComponente.Where(r => r.modeloEquipoID == item.modelo).Select(y => y.id).ToList();

                var trackComponentes = _context.tblM_trackComponentes.Where(x => x.estatus == 10 && x.fecha >= fechaInicio && componente.Contains(x.componenteID)).ToList();

                var modelo = _context.tblM_CatModeloEquipo.FirstOrDefault(x => x.id == item.modelo);
                PresupuestoPorObraDTO presupuesto = new PresupuestoPorObraDTO();
                presupuesto.presupuestoID = item.id;
                presupuesto.modeloID = item.modelo;
                presupuesto.costo = detalle.Sum(x => x.costoPresupuesto);
                presupuesto.modelo = modelo != null ? modelo.descripcion : item.modelo.ToString();
                presupuesto.presupuesto = trackComponentes.Sum(x => x.costoCRC);

                data.Add(presupuesto);
            }
            return data;
        }

        public List<DetallePresupuestoOverhaul> CargarTblAvanceDetalle(int idDetalle)
        {
            List<DetallePresupuestoOverhaul> data = new List<DetallePresupuestoOverhaul>();

            data = _context.tblM_DetallePresupuestoOverhaul.Where(x => x.presupuestoID == idDetalle && x.estado == 1).ToList().Select(y => new DetallePresupuestoOverhaul
            {
                id = y.id,
                componenteID = _context.tblM_CatComponente.Where(s => s.id == y.componenteID).Select(c => c.noComponente).FirstOrDefault(),
                maquinaID = _context.tblM_CatMaquina.Where(s => s.id == y.maquinaID).Select(c => c.noEconomico).FirstOrDefault(),
                costoSugerido = y.costoSugerido,
                costoPresupuesto = y.costoPresupuesto,
                horasCiclo = y.horasCiclo,
                horasAcumuladas = y.horasAcumuladas,
                presupuestoID = y.presupuestoID,
                estado = y.estado,
                subconjuntoID = y.subconjuntoID,
                obra = _context.tblP_CC.Where(s => s.areaCuenta == y.obra).Select(c => c.descripcion).FirstOrDefault(),
                vida = y.vida,
                costoReal = y.costoReal,
                fecha = y.fecha,
                tipo = y.tipo,
                comentarioAumento = y.comentarioAumento,
                programado = y.programado,
                esServicio = y.esServicio,
            }).ToList();

            return data;
        }


        public List<PresupuestoPorObraDTO> CargarAvanceGeneral(List<string> obras, int modeloID, int anio, int estatus)
        {
            List<PresupuestoPorObraDTO> data = new List<PresupuestoPorObraDTO>();
            var result = new Dictionary<string, object>();

            if (anio == -1) anio = DateTime.Now.Year;
            var fechaInicio = new DateTime(anio, 1, 1);
            var presupuestosGuardados = _context.tblM_PresupuestoOverhaul.Where(x => x.estado > -1 && (anio != -1 ? x.anio == anio : true)).ToList();
            var tracking = _context.tblM_trackComponentes.Where(x => x.fecha >= fechaInicio && x.estatus == 10).ToList();
            var componentes = tracking.Select(y => y.componenteID).ToList();
            var auxReporteRemocion = _context.tblM_ReporteRemocionComponente.Where(x => componentes.Contains(x.componenteRemovidoID) && x.fechaRemocion >= fechaInicio && x.estatus == 5).ToList();
            List<reporteRemocionComponenteDTO> reporteRemocionComponenteDTO = _context.tblM_ReporteRemocionComponente.Where(x => componentes.Contains(x.componenteRemovidoID) && x.fechaRemocion >= fechaInicio && x.estatus == 5).ToList().
                Select(y => new reporteRemocionComponenteDTO
                {
                    obra = _context.tblP_CC.Where(x => x.cc == y.areaCuenta).Select(r => r.areaCuenta).FirstOrDefault(),
                    componenteID = y.componenteRemovidoID,
                    costoCRC = _context.tblM_trackComponentes.Where(x => y.componenteRemovidoID == x.componenteID && x.fecha >= fechaInicio && x.estatus == 10).Select(g => g.costoCRC).FirstOrDefault()
                }).ToList();

            //var error1 = reporteRemocionComponenteDTO.Where(x =>  x.componenteID).ToList();
            List<PresupuestoPorObraDTO> lstdetalleOverhaul = new List<PresupuestoPorObraDTO>();

            foreach (var item in presupuestosGuardados)
            {

                var detalle = _context.tblM_DetallePresupuestoOverhaul.Where(x => x.presupuestoID == item.id && x.estado == 1 && x.programado).ToList();
                var componentesDetallePr = detalle.Select(x => x.componenteID).ToList();
                var detallePasado = _context.tblM_DetallePresupuestoOverhaul.Where(x => x.presupuestoID == item.id && x.estado == -1 && x.programado).ToList();
                var componentesDetallePasadoPr = detallePasado.Select(x => x.componenteID).ToList();
                var lst = _context.tblM_DetallePresupuestoOverhaul.Where(x => x.presupuestoID == item.id && x.estado == 1 && x.programado).ToList().Select(y => 
                    {
                        var auxErogado = reporteRemocionComponenteDTO.FirstOrDefault(x => x.obra == y.obra && x.componenteID == y.componenteID);
                        return new PresupuestoPorObraDTO
                        {
                            cc = _context.tblP_CC.Where(x => x.areaCuenta == y.obra).Select(n => n.cc).FirstOrDefault(),
                            obra = _context.tblP_CC.Where(x => x.areaCuenta == y.obra).Select(n => n.descripcion).FirstOrDefault(),
                            presupuesto = y.costoPresupuesto,
                            avance = componentes.Contains(y.componenteID) ? y.costoPresupuesto : 0,
                            avanceErogado = auxErogado == null ? 0 : auxErogado.costoCRC,
                            bolsaRestante = (detalle.Where(x => x.obra == y.obra).Sum(s => s.costoPresupuesto) - reporteRemocionComponenteDTO.Where(x => x.obra == y.obra).Sum(g => g.costoCRC)),
                            estado = 1
                        };
                    }).GroupBy(n => new { n.obra, n.estado }).ToList();

                var lstPasado = _context.tblM_DetallePresupuestoOverhaul.Where(x => x.presupuestoID == item.id && x.estado == -1 && x.programado).ToList().Select(y => 
                    {
                        var auxErogado = reporteRemocionComponenteDTO.FirstOrDefault(x => x.obra == y.obra && x.componenteID == y.componenteID);
                        return new PresupuestoPorObraDTO
                        {
                            cc = _context.tblP_CC.Where(x => x.areaCuenta == y.obra).Select(n => n.cc).FirstOrDefault(),
                            obra = _context.tblP_CC.Where(x => x.areaCuenta == y.obra).Select(n => n.descripcion).FirstOrDefault(),
                            presupuesto = y.costoPresupuesto,
                            avance = componentes.Contains(y.componenteID) ? y.costoPresupuesto : 0,
                            avanceErogado = auxErogado == null ? 0 : auxErogado.costoCRC,
                            bolsaRestante = (detalle.Where(x => x.obra == y.obra).Sum(s => s.costoPresupuesto) - reporteRemocionComponenteDTO.Where(x => x.obra == y.obra).Sum(g => g.costoCRC)),
                            estado = -1
                        };
                    }).GroupBy(n => new { n.obra, n.estado }).ToList();
                lst.AddRange(lstPasado);
                //avance erogado

                //SELECT * FROM tblM_ReporteRemocionComponente WHERE componenteRemovidoID IN(SELECT componenteID FROM tblM_trackComponentes WHERE estatus = 10 and year(fecha)=2021) AND YEAR(fechaRemocion )=2021

                //var trackActual = _context.tblM_trackComponentes.Where(x => compPresup.Contains(x.componenteID) && x.estatus == 0).GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault())
                foreach (var item2 in lst)
                {
                    PresupuestoPorObraDTO obj = new PresupuestoPorObraDTO();
                    obj.Descripcion = "Reparacion de componentes" + (item2.Key.estado == -1 ? (anio - 1) : anio);
                    obj.obra = item2.Select(g => g.obra).FirstOrDefault();
                    obj.cc = item2.Select(g => g.cc).FirstOrDefault();
                    obj.avance = item2.Sum(g => g.avance);
                    //obj.Descripcion = item2.Select(g => g.Descripcion).FirstOrDefault();
                    obj.presupuesto = item2.Sum(g => g.presupuesto);
                    obj.avanceErogado = item2.Sum(g => g.avanceErogado);
                    obj.bolsaRestante = obj.presupuesto - obj.avanceErogado;
                    obj.estado = item2.Key.estado;
                    data.Add(obj);
                }

            }

            var lstDatos = data.GroupBy(y => new { y.obra, y.estado }).ToList().Select(x => new PresupuestoPorObraDTO
             {
                 obra = x.Select(n => n.obra).FirstOrDefault(),
                 Descripcion = x.Select(n => n.Descripcion).FirstOrDefault(),
                 cc = x.Select(n => n.cc).FirstOrDefault(),
                 presupuesto = x.Where(y => y.obra == x.Select(n => n.obra).FirstOrDefault()).Sum(s => s.presupuesto),
                 avance = x.Where(y => y.obra == x.Select(n => n.obra).FirstOrDefault()).Sum(s => s.avance),
                 avanceErogado = x.Where(y => y.obra == x.Select(n => n.obra).FirstOrDefault()).Sum(s => s.avanceErogado),
                 bolsaRestante = x.Where(y => y.obra == x.Select(n => n.obra).FirstOrDefault()).Sum(s => s.bolsaRestante),
                 estado = x.Key.estado

             }).ToList();

            return lstDatos;
        }




        public List<tblM_PresupuestoOverhaul> CargarTblAutorizacion(List<string> obras, int modeloID, int anio)
        {
            //List<PresupuestoPorObraDTO> data = new List<PresupuestoPorObraDTO>();

            //var presupuestosGuardados = _context.tblM_PresupuestoOverhaul.Where(x => x.estado > -1 && x.estado < 5 && (anio != -1 ? x.anio == anio : true) && (modeloID != 0 ? x.modelo == modeloID : true) && x.cerrado).ToList();
            //foreach (var item in presupuestosGuardados) 
            //{
            //    var detalle = _context.tblM_DetallePresupuestoOverhaul.Where(x => x.presupuestoID == item.id).ToList();
            //    var obrasDetalle = JsonConvert.DeserializeObject<List<FechasPresupuestoObraDTO>>(item.JsonObras);
            //    foreach(var obra in obrasDetalle) {                    
            //        var detallePorObra = detalle.Where(x => x.obra == obra.obraID).ToList();
            //        if(obra.estado > 0 && obra.estado < 6 && obras.Contains(obra.obraID))
            //        {
            //            var modelo = _context.tblM_CatModeloEquipo.FirstOrDefault(x => x.id == item.modelo);
            //            PresupuestoPorObraDTO aux = new PresupuestoPorObraDTO();
            //            aux.presupuestoID = item.id;
            //            aux.anio = item.anio;
            //            aux.costo = detallePorObra.Sum(x => x.costoPresupuesto);
            //            aux.estado = obra.estado;
            //            aux.modeloID = item.modelo;
            //            aux.modelo = modelo != null ? modelo.descripcion : item.modelo.ToString();
            //            aux.obraID = obra.obraID;
            //            aux.obra = obra.obra;
            //            switch (obra.estado) 
            //            {
            //                case 2:
            //                    aux.fecha = obra.fechaEnvio.ToString("dd/MM/yyyy");
            //                    break;
            //                case 3:
            //                    aux.fecha = obra.fechaVoBo1.ToString("dd/MM/yyyy");
            //                    break;
            //                case 4:
            //                    aux.fecha = obra.fechaVoBo2.ToString("dd/MM/yyyy");
            //                    break;
            //                case 5:
            //                    aux.fecha = obra.fechaVoBo3.ToString("dd/MM/yyyy");
            //                    break;
            //                default:
            //                    aux.fecha = item.fecha.ToString("dd/MM/yyyy");
            //                    break;

            //            }
            //            aux.numComponentes = detallePorObra.Count();
            //            aux.avance = 0;
            //            aux.numCompAvance = 0;
            //            data.Add(aux);
            //        }
            //    }
            //}
            //return data;
            var presupuestosGuardados = _context.tblM_PresupuestoOverhaul.Where(x => x.estado > -1 && x.estado < 6 && (anio != -1 ? x.anio == anio : true) && (modeloID != 0 ? x.modelo == modeloID : true) && x.cerrado).ToList();

            return presupuestosGuardados;
        }

        public tblM_PresupuestoOverhaul GetPresupuesto(int modeloID, int anio)
        {
            var presupuesto = _context.tblM_PresupuestoOverhaul.FirstOrDefault(x => x.modelo == modeloID && x.anio == anio);
            return presupuesto;
        }


        public List<tblM_PresupuestoOverhaul> GetPresupuestos(List<int> modelos, int anio)
        {
            if (modelos == null) modelos = new List<int>();
            var presupuestos = _context.tblM_PresupuestoOverhaul.Where(x => (modelos.Count() == 0 ? true : modelos.Contains(x.modelo)) && x.anio == anio).ToList();
            return presupuestos;
        }

        public tblM_DetallePresupuestoOverhaul GetDetalleByComp(int componenteID, int anio)
        {
            var detalle = _context.tblM_DetallePresupuestoOverhaul.FirstOrDefault(x => x.componenteID == componenteID && x.presupuesto.anio == anio);

            if (detalle != null)
            {
                var erogado = _context.tblM_trackComponentes.FirstOrDefault
                    (f =>
                        f.componenteID == detalle.componenteID &&
                        f.estatus == 10 &&
                        f.fecha.Value.Year == anio
                    );

                detalle.costoReal = erogado != null ? erogado.costoCRC : 0;
            }

            return detalle;
        }

        public bool AutorizarPresupuesto(int presupuestoID, int modelo, int anio, string obra, int tipo)
        {
            using (var context = new MainContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var presupuesto = context.tblM_PresupuestoOverhaul.FirstOrDefault(x => x.id == presupuestoID);
                        List<tblM_DetallePresupuestoOverhaul> detalles = new List<tblM_DetallePresupuestoOverhaul>();
                        if (presupuesto != null)
                        {
                            //var detalle = context.tblM_DetallePresupuestoOverhaul.Where(x => x.presupuestoID == presupuesto.id && x.obra == obra).ToList();
                            var obras = JsonConvert.DeserializeObject<List<FechasPresupuestoObraDTO>>(presupuesto.JsonObras);
                            if (obra != "")
                            {
                                obras = obras.Where(x => x.obraID == obra).ToList();
                            }
                            //var obraAutorizada = obras.FirstOrDefault(x => x.obraID == obra);
                            switch (tipo)
                            {
                                case 0:
                                    presupuesto.estado = 2;
                                    foreach (var item in obras)
                                    {
                                        if (item.estado == 1)
                                        {
                                            item.estado = 2;
                                            item.fechaEnvio = DateTime.Now;
                                        }
                                    }
                                    break;
                                case 1:
                                    presupuesto.estado = 3;
                                    foreach (var item in obras)
                                    {
                                        if (item.estado == 2)
                                        {
                                            item.estado = 3;
                                            item.fechaVoBo1 = DateTime.Now;
                                        }
                                    }
                                    break;
                                case 2:
                                    presupuesto.estado = 4;
                                    foreach (var item in obras)
                                    {
                                        if (item.estado == 3)
                                        {
                                            item.estado = 4;
                                            item.fechaVoBo2 = DateTime.Now;
                                        }
                                    }
                                    break;
                                case 3:
                                    presupuesto.estado = 5;
                                    foreach (var item in obras)
                                    {
                                        if (item.estado == 4)
                                        {
                                            item.estado = 5;
                                            item.fechaVoBo3 = DateTime.Now;
                                        }
                                    }
                                    break;
                                case 4:
                                    presupuesto.estado = 6;
                                    foreach (var item in obras)
                                    {
                                        if (item.estado == 5)
                                        {
                                            item.estado = 6;
                                            item.fechaAutorizacion = DateTime.Now;
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                            presupuesto.JsonObras = JsonConvert.SerializeObject(obras);
                            context.SaveChanges();
                            dbContextTransaction.Commit();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public decimal GuardarAumentoPresupuesto(decimal aumento, string comentario, int presupuestoID, int componenteID, int tipo)
        {
            using (var context = new MainContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var detalle = context.tblM_DetallePresupuestoOverhaul.FirstOrDefault(x => x.presupuestoID == presupuestoID && x.componenteID == componenteID);
                        detalle.costoPresupuesto = aumento;
                        //if (tipo == 0)
                        //{
                        //    detalle.costoPresupuesto -= aumento;
                        //    if (detalle.costoPresupuesto < 0)
                        //        detalle.costoPresupuesto = 0;

                        //}
                        //else
                        //{
                        //    detalle.costoPresupuesto += aumento;
                        //}
                        detalle.comentarioAumento = comentario;
                        detalle.estado = 1;
                        context.SaveChanges();
                        dbContextTransaction.Commit();
                        return detalle.costoPresupuesto;
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        return -1;
                    }
                }
            }
        }

        public string ComentarioAumPresupuesto(int presupuestoID, int componenteID)
        {
            var detalle = _context.tblM_DetallePresupuestoOverhaul.FirstOrDefault(x => x.presupuestoID == presupuestoID && x.componenteID == componenteID);
            if (detalle != null)
            {
                return detalle.comentarioAumento;
            }
            else
            {
                return "";
            }
        }


        public List<ComboDTO> CargarModelosRptInversion()
        {
            var modelosID = _context.tblM_PresupuestoOverhaul.Select(x => x.modelo).Distinct().ToList();
            var modelos = _context.tblM_CatModeloEquipo.Where(x => modelosID.Contains(x.id)).Select(x => new ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.descripcion
            }).ToList();
            return modelos;
        }

        public List<ComboDTO> CargarObrasRptInversion()
        {
            var obrasID = _context.tblM_DetallePresupuestoOverhaul.Select(x => x.obra).Distinct().ToList();
            var obras = _context.tblP_CC.Where(x => obrasID.Contains(x.areaCuenta)).Select(x => new ComboDTO
            {
                Value = x.areaCuenta,
                Text = x.descripcion
            }).ToList();
            return obras;
        }

        public List<ComboDTO> CargarAnioRptInversion()
        {
            var anios = _context.tblM_PresupuestoOverhaul.Select(x => x.anio).Distinct().Select(x => new ComboDTO
            {
                Value = x.ToString(),
                Text = x.ToString()
            }).ToList();
            return anios;
        }

        public string cargarComponentesAPresupuesto() 
        {
            List<ComboDTO> auxiliar = new List<ComboDTO>();
            var calendariosActivos = _context.tblM_CalendarioPlaneacionOverhaul.Where(x => x.anio == 2021 && x.estatus == 0).Select(x => x.id).ToList();
            var componentesParos = _context.tblM_CapPlaneacionOverhaul.Where(x => calendariosActivos.Contains(x.calendarioID) && x.tipo < 3).Select(x => x.idComponentes).ToList();
            var Auxcomponentes = componentesParos.SelectMany(x => JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(x)).Where(x => x.Tipo == 0);
            var componentesConCalendario = _context.tblM_CapPlaneacionOverhaul.Where(x => calendariosActivos.Contains(x.calendarioID) && x.tipo < 3).ToList();


            var componentes = Auxcomponentes.Select(x => x.componenteID).ToList();
            var presupuestosActivos = _context.tblM_PresupuestoOverhaul.Where(x => x.anio == 2021).Select(x => x.id).ToList();
            var componentesPresupuesto = _context.tblM_DetallePresupuestoOverhaul.Where(x => presupuestosActivos.Contains(x.presupuestoID)).Select(x => x.componenteID).ToList();
            foreach (var item in componentesPresupuesto) componentes.Remove(item);
            var data = "";
            foreach (var item in componentes) 
            {
                data += item + ", ";
            }

            List<tblM_DetallePresupuestoOverhaul> detallesAGuardar = new List<tblM_DetallePresupuestoOverhaul>();

            foreach (var item in componentesConCalendario)
            {
                var lista = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(item.idComponentes);
                foreach (var item2 in lista) 
                {
                    var maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == item.maquinaID);
                    if (maquina == null) 
                    {
                        int a = 1;
                    }
                    var presupuesto = _context.tblM_PresupuestoOverhaul.FirstOrDefault(x => x.modelo == maquina.modeloEquipoID && x.anio == 2021);
                    if (presupuesto == null)
                    {
                        int c = 3;
                    }
                    if(!componentesPresupuesto.Contains(item2.componenteID) && presupuesto != null)
                    {
                        var componente = _context.tblM_CatComponente.FirstOrDefault(x => x.id == item2.componenteID);
                        if (componente == null) 
                        {
                            int b = 2;
                        }

                        tblM_DetallePresupuestoOverhaul auxDetalle = new tblM_DetallePresupuestoOverhaul();
                        auxDetalle.componenteID = item2.componenteID;
                        auxDetalle.maquinaID = item.maquinaID;
                        auxDetalle.presupuestoID = presupuesto.id;
                        auxDetalle.programado = true;
                        auxDetalle.estado = -1;
                        auxDetalle.fecha = item.fecha;
                        auxDetalle.horasCiclo = item2.horasCiclo;
                        auxDetalle.horasAcumuladas = item2.horasCiclo;
                        auxDetalle.obra = maquina.centro_costos;
                        auxDetalle.subconjuntoID = componente.subConjuntoID ?? default (int);
                        auxDetalle.tipo = item.tipo;
                        auxDetalle.vida = GetVidasComponente(item2.componenteID);
                        detallesAGuardar.Add(auxDetalle);
                    }
                }

            }
            _context.tblM_DetallePresupuestoOverhaul.AddRange(detallesAGuardar);
            _context.SaveChanges();


            return data;
        }

        public List<tblM_PresupuestoHC> GetPresupuestoHC(List<string> obras)
        {
            if (obras == null) obras = new List<string>();
            var detalles = _context.tblM_PresupuestoHC.Where(x => obras.Count() > 0 ? obras.Contains(x.obra) : true).ToList();
            return detalles;
        }

        public List<PresupuestoComponenteDTO> CargarPresupuestoPorComponente(int anio, int modelo, int subconjunto, string noComponente) 
        {
            var componentes = _context.tblM_CatComponente.Where(x => (modelo == 0 ? true : x.modeloEquipoID == modelo) && (subconjunto == 0 ? true : x.subConjuntoID == subconjunto) && (noComponente == "" ? true : x.noComponente.Contains(noComponente))).ToList();
            var componentesID = componentes.Select(x => x.id).ToList();
            var modelosID = componentes.Select(x => x.modeloEquipoID).ToList();
            var subconjuntosID = componentes.Select(x => x.subConjuntoID).ToList();
            var modelos = _context.tblM_CatModeloEquipo.Where(x => modelosID.Contains(x.id)).ToList();
            var subconjuntos = _context.tblM_CatSubConjunto.Where(x => subconjuntosID.Contains(x.id)).ToList();
            var presupuestosID = _context.tblM_PresupuestoOverhaul.Where(x => x.anio == anio).Select(x => x.id).ToList();
            var detallesPresupuesto = _context.tblM_DetallePresupuestoOverhaul.Where(x => presupuestosID.Contains(x.presupuestoID)).ToList();
            var tracking = _context.tblM_trackComponentes.Where(x => componentesID.Contains(x.componenteID) && (x.fecha ?? DateTime.Now).Year == anio).ToList();

            List<PresupuestoComponenteDTO> data = componentes.Select(x => {
                var auxModelo = modelos.FirstOrDefault(y => y.id == x.modeloEquipoID);
                var auxSubconjunto = subconjuntos.FirstOrDefault(y => y.id == x.subConjuntoID);
                var auxPresupuesto = detallesPresupuesto.Where(y => y.componenteID == x.id).ToList();
                var auxTracking = tracking.Where(y => y.componenteID == x.id).ToList();
                return new PresupuestoComponenteDTO {
                    id = x.id,
                    noComponente = x.noComponente,
                    modeloID = x.modeloEquipoID ?? 0,
                    modelo = auxModelo == null ? "" : auxModelo.descripcion,
                    subconjuntoID = x.subConjuntoID ?? 0,
                    subconjunto = auxSubconjunto == null ? "" : auxSubconjunto.descripcion,
                    presupuestoInicial = auxPresupuesto.Sum(y => y.costoPresupuesto),
                    erogado = auxTracking.Sum(y => y.costoCRC),
                    presupuestado = auxPresupuesto.Count() > 0
                };
            }).Where(x => x.presupuestoInicial != 0 || x.erogado != 0).ToList();
            return data;
        }

    }
}

