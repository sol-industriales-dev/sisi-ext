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
using Core.DTO.Maquinaria.Overhaul;
using Core.DTO.Principal.Generales;
using Newtonsoft.Json;
using Data.EntityFramework.Context;
using Infrastructure.Utils;
using Core.Entity.Principal.Multiempresa;
using System.Text.RegularExpressions;
using Data.DAO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria;
using Core.Entity.Maquinaria.Captura;
using Core.Enum;
using Core.Enum.Maquinaria;
using Core.DTO.Utils.Data;
using Core.Enum.Multiempresa;
using System.Data.Odbc;
using Core.DTO;
using Core.Enum.Principal;

namespace Data.DAO.Maquinaria.Overhaul
{
    public class AdministracionComponentesDAO : GenericDAO<tblM_trackComponentes>, IAdministracionComponentesDAO
    {
        ComponenteDAO componenteDAO = new ComponenteDAO();
        public void Guardar(tblM_trackComponentes obj)
        {
            if (obj.id == 0)
            {
                var componente = _context.tblM_CatComponente.FirstOrDefault(x => x.id == obj.componenteID);
                decimal horasAcumuladas = 0;
                decimal horasCiclo = 0;
                if (componente != null) 
                {
                    horasAcumuladas = componente.horasAcumuladas;
                    horasCiclo = componente.horaCicloActual;
                }
                obj.horasAcumuladas = horasAcumuladas;
                obj.horasCiclo = horasCiclo;
                SaveEntity(obj, (int)BitacoraEnum.GUARDARTRACKCOMPONENTE);
                if (obj.componente != null) { obj.componente.trackComponenteID = obj.id; }
                _context.SaveChanges();
            }
            else
                Update(obj, obj.id, (int)BitacoraEnum.GUARDARTRACKCOMPONENTE);            
        }

        private decimal CalcularHrsCicloActual(int idComponente, List<tblM_CapHorometro> horometros) 
        {
            decimal horometro = 0;
            try {
                var trackingTotal = _context.tblM_trackComponentes.Where(x => x.componenteID == idComponente).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).ToList();
                var fechaSiguiente = DateTime.Today;
                var locacion = "";
                var fechaActual = new DateTime();
                for (int i = 0; i < trackingTotal.Count(); i++)
                {
                    if (trackingTotal[i].reciclado)
                        return horometro;
                    if (trackingTotal[i].tipoLocacion == false)
                    {
                        locacion = trackingTotal[i].locacion;                        
                        fechaActual = trackingTotal[i].fecha ?? default(DateTime);
                        var horometroLocacion = horometros.Where(x => x.Economico == locacion && x.Fecha >= fechaActual && x.Fecha <= fechaSiguiente);
                        horometro += horometroLocacion .Sum(x => x.HorasTrabajo);
                    }
                    fechaSiguiente = trackingTotal[i].fecha ?? default(DateTime);
                }
            }
            catch(Exception e) {
                horometro = 0;
            }
            return horometro;
        }

        public decimal GetHoraCicloActual(int idComponente, DateTime fechaLimite)
        {
            decimal horometro = 0;
            try
            {
                var trackingTotal = _context.tblM_trackComponentes.Where(x => x.componenteID == idComponente).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).ToList();
                var fechaSiguiente = fechaLimite;
                var locacion = "";
                var fechaActual = new DateTime();
                for (int i = 0; i < trackingTotal.Count(); i++)
                {
                    if (trackingTotal[i].reciclado)
                        return horometro;
                    if (trackingTotal[i].tipoLocacion == false)
                    {
                        locacion = trackingTotal[i].locacion;
                        fechaActual = trackingTotal[i].fecha ?? default(DateTime);
                        var horometroLocacion = _context.tblM_CapHorometro.Where(x => x.Economico == locacion && x.Fecha >= fechaActual && x.Fecha <= fechaSiguiente);
                        horometro += horometroLocacion.Sum(x => x.HorasTrabajo);
                    }
                    fechaSiguiente = trackingTotal[i].fecha ?? default(DateTime);
                }
            }
            catch (Exception e)
            {
                horometro = 0;
            }
            return horometro;
        }

        public decimal GetHorasAcumuladas(int idComponente, DateTime fechaLimite)
        {
            decimal horometro = 0;
            try
            {
                var trackingTotal = _context.tblM_trackComponentes.Where(x => x.componenteID == idComponente).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).ToList();
                var fechaSiguiente = fechaLimite;
                var locacion = "";
                var fechaActual = new DateTime();
                for (int i = 0; i < trackingTotal.Count(); i++)
                {
                    if (trackingTotal[i].tipoLocacion == false)
                    {
                        locacion = trackingTotal[i].locacion;
                        fechaActual = trackingTotal[i].fecha ?? default(DateTime);
                        var horometroLocacion = _context.tblM_CapHorometro.Where(x => x.Economico == locacion && x.Fecha >= fechaActual && x.Fecha <= fechaSiguiente);
                        horometro += horometroLocacion.Sum(x => x.HorasTrabajo);
                    }
                    fechaSiguiente = trackingTotal[i].fecha ?? default(DateTime);
                }
            }
            catch (Exception e)
            {
                horometro = 0;
            }
            return horometro;
        }

        public List<tblP_CC> getListaCCByID(List<int> ccID)
        {
            var data = _context.tblP_CC.Where(x => ccID.Contains(x.id)).ToList();
            return data;
        }

        //private decimal CalcularHrsCicloActual(int idComponente)
        //{
        //    decimal hrsCiclo = 0;
        //    try
        //    {
        //        var trackUltimoReciclado = _context.tblM_trackComponentes.OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).FirstOrDefault(x => x.componenteID == idComponente && x.reciclado == true);
        //        var trackFechaUltimo = trackUltimoReciclado.fecha;
        //        var trackIdUltimo = trackUltimoReciclado.id;
        //        var trackComponente = _context.tblM_trackComponentes.Where(x => x.componenteID == idComponente && x.fecha >= trackFechaUltimo && x.id > trackIdUltimo).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).ToList();
        //        DateTime fechaActual = new DateTime();
        //        DateTime fechaSiguiente = new DateTime();
        //        string Economico = "";
        //        int locacion = 0;
        //        for (int i = 0; i < trackComponente.Count(); i++)
        //        {
        //            locacion = trackComponente[i].locacionID ?? default(int);
        //            Economico = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == locacion).noEconomico;
        //            fechaActual = trackComponente[i].fecha ?? default(DateTime);
        //            if ((i + 1) < trackComponente.Count()) { fechaSiguiente = trackComponente[i + 1].fecha ?? default(DateTime); }
        //            else { fechaSiguiente = DateTime.Today; }
        //            var HorometroFinal = _context.tblM_CapHorometro.OrderByDescending(x => x.Fecha).FirstOrDefault(x => x.Economico == Economico && x.Fecha <= fechaSiguiente);
        //            if (HorometroFinal == null)
        //            {
        //                HorometroFinal = _context.tblM_CapHorometro.OrderByDescending(x => x.Fecha).FirstOrDefault(x => x.Economico == Economico);
        //            }

        //            var HorometroInicial = _context.tblM_CapHorometro.OrderByDescending(x => x.Fecha).FirstOrDefault(x => x.Economico == Economico && x.Fecha <= fechaActual);
        //            if (HorometroInicial != null)
        //            {
        //                hrsCiclo += (HorometroFinal.HorometroAcumulado - HorometroInicial.HorometroAcumulado);
        //            }
        //            else
        //            {
        //                hrsCiclo += HorometroFinal.HorometroAcumulado;
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        hrsCiclo = 0;
        //    }
        //    return hrsCiclo;
        //}

        public List<ListaComponentesMaquina> getMaquinas(int grupo, int modelo, string economicoBusqueda, string descripcionComponente, string obra, string noComponente) 
        {
            List<ListaComponentesMaquina> lista = new List<ListaComponentesMaquina>();
            var maquinasBuscar = _context.tblM_CatMaquina
                .Where(x => x.estatus > 0 && x.noEconomico.Contains(economicoBusqueda) && (obra == "" ? true : x.centro_costos == obra)
                    && (grupo == -1 ? true : x.grupoMaquinariaID == grupo) && (modelo == -1 ? true : x.modeloEquipoID == modelo));

            var trackActualComponente = _context.tblM_CatComponente.Select(x => x.trackComponenteID);

            var trackActual = _context.tblM_trackComponentes.Where(x => trackActualComponente.Contains(x.id) && (x.estatus == 0 || x.estatus == 12) && x.componente.noComponente.Contains(noComponente) && (descripcionComponente == "" ? true : x.componente.subConjunto.descripcion.Equals(descripcionComponente))
                && maquinasBuscar.Select(y => y.id).Contains(x.locacionID ?? 0) && x.componente.subConjunto.estatus);

            //var trackActual = _context.tblM_trackComponentes.GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault())
            //    .Where(x => (x.estatus == 0 || x.estatus == 12) && x.componente.noComponente.Contains(noComponente) && (descripcionComponente == "" ? true : x.componente.subConjunto.descripcion.Equals(descripcionComponente))
            //    && maquinasBuscar.Select(y => y.id).Contains(x.locacionID ?? 0) && x.componente.subConjunto.estatus);

            var maquinasFinalIDs = trackActual.Select(x => x.locacionID ?? 0).Distinct().ToList();

            var maquinasFinal = maquinasBuscar.Where(x => maquinasFinalIDs.Contains(x.id));
            //var horometros = _context.tblM_CapHorometro.Where(x => maquinasFinal.Select(y => y.noEconomico).Contains(x.Economico) && x.Fecha >= trackActual.Select(y => y.fecha).Min() ).ToList();

            lista = maquinasFinal.Select(x => new ListaComponentesMaquina
            {
                idLocacion = x.id,
                descripcionLocacion = x.noEconomico,
                modeloEquipoID = x.modeloEquipoID,
                cc = x.centro_costos,
                componente = trackActual.Where(y => y.locacionID == x.id).Select(y => y.componente).ToList(),
                tipoLocacion = false
            }
            ).Where(x => x.componente.Count() > 0).ToList();

            //var componentes = lista.SelectMany(x => x.componente).ToList();
            //componentes = GetHrsCicloActualLista(componentes, horometros);

            //foreach (var item in lista)
            //{
            //    var componentes = item.componente;
            //    foreach (var componente in componentes)
            //    {
            //        componente.horaCicloActual = CalcularHrsCicloActual(componente.id, horometros);
            //    }
            //}

            return lista;
        }

        private List<tblM_CatComponente> GetHrsCicloActualLista(List<tblM_CatComponente> componentes, List<tblM_CapHorometro> horometros) 
        {
            var componentesID = componentes.Select(y => y.id).ToList();
            var trackingTotal = _context.tblM_trackComponentes.Where(x => componentesID.Contains(x.componenteID)).ToList();
            List<tblM_trackComponentes> trackActual = new List<tblM_trackComponentes>();
            foreach (var componente in componentes) 
            {
                try
                {
                    trackActual = trackingTotal.Where(x => x.componenteID == componente.id).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).ToList();
                    componente.horaCicloActual = 0;
                    var fechaSiguiente = DateTime.Today;
                    var locacion = "";
                    var fechaActual = new DateTime();
                    for (int i = 0; i < trackActual.Count(); i++)
                    {
                        if (trackActual[i].reciclado)
                            i = trackActual.Count();
                        else
                        {
                            if (trackActual[i].tipoLocacion == false)
                            {
                                locacion = trackActual[i].locacion;
                                fechaActual = trackActual[i].fecha ?? default(DateTime);
                                var horometroLocacion = _context.tblM_CapHorometro.Where(x => x.Economico == locacion && x.Fecha >= fechaActual && x.Fecha <= fechaSiguiente);
                                componente.horaCicloActual += horometroLocacion.Sum(x => x.HorasTrabajo);
                            }
                            fechaSiguiente = trackActual[i].fecha ?? default(DateTime);
                        }
                    }
                }
                catch (Exception e) {
                    componente.horaCicloActual = 0;
                }
            }
            
            return componentes;
        }

        public List<ListaComponentesMaquina> getMaquinasLocaciones(int estatus)
        {
            List<ListaComponentesMaquina> lista = new List<ListaComponentesMaquina>();
            var trackActual = _context.tblM_trackComponentes.GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault()).Where(x => estatus == 2 ? (x.estatus == estatus || (x.estatus > 3 && x.estatus < 12)) : x.estatus == estatus).ToList();

            var maquinasFinalIDs = trackActual.Select(x => x.locacionID ?? 0).Distinct().ToList();
            var maquinasFinal = _context.tblM_CatLocacionesComponentes.Where(x => x.tipoLocacion == 2 && x.estatus).ToList();
            lista = maquinasFinal.Select(x => new ListaComponentesMaquina
            {
                idLocacion = x.id,
                descripcionLocacion = x.descripcion,
                componente = trackActual.Where(y => y.locacionID == x.id).Select(y => y.componente).ToList(),
                tipoLocacion = true
            }
            ).Where(x => x.componente.Count() > 0).ToList();
            foreach (var item in lista) {
                var componentes = item.componente;
                foreach (var componente in componentes) 
                {
                    var track = trackActual.FirstOrDefault(y => y.id == componente.trackComponenteID);
                    componente.fecha = track == null ? componente.fecha : track.fecha;
                }
            }            
            return lista;
        }

        public string getDescripcionCC(string centro_costos) 
        {
            var data = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == centro_costos);
            if (data != null) return data.descripcion;
            else return "";
        }
        public string getDescripcionCCByCC(string centro_costos)
        {
            var data = _context.tblP_CC.FirstOrDefault(x => x.cc == centro_costos);
            if (data != null) return data.descripcion;
            else return "";
        }

        public List<tblM_CatModeloEquipo> FillCboModeloEquipoGrupo(int idGrupo)
        {
            return _context.tblM_CatModeloEquipo.Where(x => (idGrupo == -1 ? true : x.idGrupo == idGrupo) && x.estatus).ToList();
        }

        public List<tblM_trackComponentes> getListaComponentes(int idMaquina) 
        {
            List<tblM_trackComponentes> aux = new List<tblM_trackComponentes>();
            try {
                aux = _context.tblM_trackComponentes.Where(x => x.locacionID == idMaquina).ToList();
            }
            catch (Exception e) { }
            return aux;
        }

        public List<tblM_trackComponentes> FillModalComponentes(int idMaquina, string componente) 
        {
            List<tblM_trackComponentes> trackActual = _context.tblM_trackComponentes
                .Where(x => x.componente.subConjunto.estatus == true && (idMaquina == -1 ? true : x.locacionID == idMaquina)
                && (componente == "" ? true : x.componente.noComponente.Contains(componente)) && x.componente.trackComponenteID == x.id).ToList();
            return trackActual;
        }

        public string getLocacionByID(int idMaquina, bool tipoLocacion) 
        {
            var data = "";
            if (tipoLocacion) {
                var locacion = _context.tblM_CatLocacionesComponentes.FirstOrDefault(x => x.id == idMaquina);
                data = locacion != null ? locacion.descripcion : "";
            }
            else { 
                var maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == idMaquina);
                data = maquina != null ? maquina.noEconomico : ""; 
            }
            return data;
        }

        public List<tblM_trackComponentes> FillModalComponentesHistorial(int idComponente)
        {
            return _context.tblM_trackComponentes.Where(x => x.componenteID == idComponente).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).ToList();
        }

        public List<ComboDTO> FillCboGrupoMaquinaria(int idTipo)
        {
            var join = _context.tblM_CatModeloEquipo.Join(_context.tblM_CatModeloEquipotblM_CatSubConjunto, (x => x.id), (y => y.modeloID), ((x, y) => new { x, y })).GroupBy(x => x.x.idGrupo).ToList();
            List<ComboDTO> data = new List<ComboDTO>();
            foreach (var item in join) 
            {
                ComboDTO aux = new ComboDTO();
                aux.Value = _context.tblM_CatGrupoMaquinaria.FirstOrDefault(x => x.id == item.Key).id.ToString();
                aux.Text = _context.tblM_CatGrupoMaquinaria.FirstOrDefault(x => x.id == item.Key).descripcion;
                data.Add(aux);
            }
            return data;
            //return _context.tblM_CatGrupoMaquinaria.Where(x => x.estatus == true && (idTipo == 0 ? true : x.tipoEquipoID == idTipo)).OrderBy(x => x.descripcion).ToList();
        }

        public List<ComboDTO> FillCboObraMaquina() 
        {

            var modelos = _context.tblM_CatModeloEquipotblM_CatSubConjunto.Select(x => x.modeloID).Distinct();
            var obras = _context.tblP_CC.Where(x => x.estatus == true);
            var join = _context.tblM_CatMaquina.Where(x => x.estatus == 1 && modelos.Contains(x.modeloEquipoID)).Select(x => x.centro_costos).Distinct().ToList();
            
            var data = join.Select(z => {
                var cc = obras.FirstOrDefault(x => x.areaCuenta == z);
                return new ComboDTO
                {
                    Value = cc != null ? cc.areaCuenta : "",
                    Text = cc != null ? cc.descripcion.ToUpper() : "",
                };
            }).ToList();
            //List<ComboDTO> data = new List<ComboDTO>();
            //foreach(var item in join)
            //{
            //    ComboDTO aux = new ComboDTO();
            //    aux.Value = item.Value.ToString();
            //    aux.Text = item.Text;
            //    data.Add(aux);
            //}
            return data;
        }

        public List<tblP_CC> FillCboObraMaquinaID()
        {
            var centrosCostoCompID = _context.tblM_CatComponente.Select(x => x.centroCostos).Distinct().ToList();
            var centroCostoComp = _context.tblP_CC.Where(x => centrosCostoCompID.Contains(x.id)).Select(x => new ComboDTO
            {
                Value = x.areaCuenta,
                Text = x.descripcion,
            }).ToList(); ;

            var join = _context.tblM_CatMaquina.Where(x => x.estatus == 1).Join(_context.tblM_CatModeloEquipotblM_CatSubConjunto, (x => x.modeloEquipoID), (y => y.modeloID), ((x, y) => new { x, y })).GroupBy(x => x.x.centro_costos)
            .Select(z => new ComboDTO
            {
                Value = z.Key == "997" ? z.Key : _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == z.Key).areaCuenta,
                Text = z.Key == "997" ? z.Key : _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == z.Key).descripcion,

            }).ToList();
            join.AddRange(centroCostoComp);
            join = join.Distinct().ToList();
            List<string> data = new List<string>();
            foreach (var item in join)
            {
                data.Add(item.Value);
            }
            data = data.Distinct().ToList();
            var dataFinal = _context.tblP_CC.Where(x => data.Contains(x.areaCuenta)).ToList();
            return dataFinal;
        }

        public Dictionary<string, object> FillCboObraMaquinaIDComboDTO()
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                var centrosCostoCompID = _context.tblM_CatComponente.Select(x => x.centroCostos).Distinct().ToList();
                var centroCostoComp = _context.tblP_CC.Where(x => centrosCostoCompID.Contains(x.id)).Select(x => new ComboDTO
                {
                    Value = x.areaCuenta,
                    Text = x.descripcion,
                }).ToList(); ;

                var join = _context.tblM_CatMaquina.Where(x => x.estatus == 1).Join(_context.tblM_CatModeloEquipotblM_CatSubConjunto, (x => x.modeloEquipoID), (y => y.modeloID), ((x, y) => new { x, y })).GroupBy(x => x.x.centro_costos)
                .Select(z => new ComboDTO
                {
                    Value = z.Key == "997" ? z.Key : _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == z.Key).areaCuenta,
                    Text = z.Key == "997" ? z.Key : _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == z.Key).descripcion,

                }).ToList();
                join.AddRange(centroCostoComp);
                join = join.Distinct().ToList();
                List<string> data = new List<string>();
                foreach (var item in join)
                {
                    data.Add(item.Value);
                }
                data = data.Distinct().ToList();
                var dataFinal = _context.tblP_CC.Where(x => data.Contains(x.areaCuenta)).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                foreach (var item in dataFinal)
                {
                    //Value = x.id, Text = x.descripcion.ToUpper(), Prefijo = x.abreviacion.ToUpper()
                    ComboDTO obj = new ComboDTO();
                    obj.Value = item.id.ToString();
                    obj.Text = !string.IsNullOrEmpty(item.descripcion) ? item.descripcion.Trim().ToUpper() : string.Empty;
                    obj.Prefijo = !string.IsNullOrEmpty(item.abreviacion) ? item.abreviacion.Trim().ToUpper() : string.Empty;
                    lstComboDTO.Add(obj);
                }
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
            }
            catch (Exception e)
            {
                LogError(0, 0, "OverhaulController", "FillCboObraMaquinaID", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public List<ComboDTO> FillCboLocacionYObra()
        {
            var join = _context.tblM_CatMaquina.Where(x => x.estatus == 1).Join(_context.tblM_CatModeloEquipotblM_CatSubConjunto, (x => x.modeloEquipoID), (y => y.modeloID), ((x, y) => new { x, y })).GroupBy(x => x.x.centro_costos)
            .Select(z => new ComboDTO
            {
                Value = z.Key == "997" ? z.Key : _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == z.Key).areaCuenta,
                Text = z.Key == "997" ? z.Key : _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == z.Key).descripcion,
                Prefijo = "0"
            }).ToList();
            var almacenYCRC = _context.tblM_CatLocacionesComponentes.Where(x => x.tipoLocacion < 3).Select(x => new ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.descripcion,
                Prefijo = x.tipoLocacion.ToString()
            });
            join.AddRange(almacenYCRC);
            return join;
        }

        public int getVidasAcumuladas(int idComponente) 
        {
            int data = 0;     
            var componente = _context.tblM_CatComponente.FirstOrDefault(x => x.id == idComponente);
            if (componente != null) { data = componente.vidaInicio; }
            var tracks = _context.tblM_trackComponentes.Where(x => x.componenteID == idComponente).ToList();
            data = tracks.FirstOrDefault().componente.vidaInicio;
            if (tracks.Count() > 0) {
                data += tracks.Where(x => x.reciclado == true).Count(); 
            }
            return data;
        }

        public int getVidasAcumuladasByFecha(int idComponente, DateTime fecha)
        {
            int data = 0;
            var componente = _context.tblM_CatComponente.FirstOrDefault(x => x.id == idComponente);
            if (componente != null) { data = componente.vidaInicio; }
            var tracks = _context.tblM_trackComponentes.Where(x => x.componenteID == idComponente && x.fecha <= fecha).ToList();
            data = tracks.FirstOrDefault().componente.vidaInicio;
            if (tracks.Count() > 0)
            {
                data += tracks.Where(x => x.reciclado == true).Count();
            }
            return data;
        }

        public List<tblM_CatSubConjunto> getSubConjuntos(string term) 
        {
            List<tblM_CatSubConjunto> data = new List<tblM_CatSubConjunto>();
            data = _context.tblM_CatSubConjunto.Where(x => x.descripcion.Contains(term)).ToList();
            return data;
        }

        public List<tblM_CatComponente> getNoComponente(string term)
        {
            List<tblM_CatComponente> data = new List<tblM_CatComponente>();
            try
            {
                
                data = _context.tblM_CatComponente.Where(x => x.noComponente.Contains(term)).ToList();
                return data;
            }
            catch (Exception e) { return data; }
        }

        public List<tblM_CatComponente> getNoComponenteReporte(string term, int modeloID, int subconjuntoID)
        {
            List<tblM_CatComponente> data = new List<tblM_CatComponente>();
            data = _context.tblM_CatComponente.Where(x => x.noComponente.Contains(term) && x.modeloEquipoID == modeloID && x.subConjuntoID == subconjuntoID).ToList();
            return data;
        }

        public List<tblM_CatMaquina> getEconomico(string term)
        {
            List<tblM_CatMaquina> data = new List<tblM_CatMaquina>();
            data = _context.tblM_CatMaquina.Where(x => x.noEconomico.Contains(term)).ToList();
            return data;
        }

        public List<tblM_CatLocacionesComponentes> FillCboLocacion(int tipoLocacion) 
        {
            return _context.tblM_CatLocacionesComponentes.Where(x => x.estatus == true && x.tipoLocacion == tipoLocacion).ToList();
        }

        public List<tblM_CatLocacionesComponentes> FillCboLocacionByListaTipo(List<int> tipoLocaciones)
        {
            return _context.tblM_CatLocacionesComponentes.Where(x => x.estatus == true && tipoLocaciones.Contains(x.tipoLocacion)).ToList();
        }

        public List<tblM_trackComponentes> getComponentesAlmacenInactivos(string noComponente, List<int> idLocacion, string descripcionComponente, int estatus, int grupoId, int modeloId) 
        {
            var componente = descripcionComponente.Trim();
            List<int> locaciones = new List<int>();
            if(idLocacion != null) locaciones.AddRange(idLocacion);
            var data = _context.tblM_trackComponentes.Where(x => x.componente.trackComponenteID == x.id && x.tipoLocacion == true && x.estatus == estatus
                && x.componente.noComponente.Contains(noComponente) && locaciones.Contains(x.locacionID ?? 0)
                && (componente == "" ? true : x.componente.subConjunto.descripcion.Trim() == componente) &&
                (grupoId == 0 ? true: x.componente.grupoID == grupoId) && (modeloId == 0 ? true: x.componente.modeloEquipoID == modeloId)              
                ).ToList();
            return data;
        }

        public string getLocacionByID(int id) 
        {
            return _context.tblM_CatLocacionesComponentes.FirstOrDefault(x => x.id == id).descripcion;
        }

        public tblM_CatLocacionesComponentes getLocacion(int id)
        {
            return _context.tblM_CatLocacionesComponentes.FirstOrDefault(x => x.id == id);
        }

        public string getAlmacenLocacionByID(int id)
        {
            var idCC = _context.tblM_CatLocacionesComponentes.FirstOrDefault(x => x.id == id).areaCuenta;
            if (idCC == null || idCC == "") { return ""; }
            else { return _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == idCC).descripcion; }            
        }

        public void GuardarFechasCRC(int idTrack, DateTime fecha, int estatus, bool intercambio, string DatosExtra, int usuario) 
        {
            var tracking = _context.tblM_trackComponentes.FirstOrDefault(x => x.id == idTrack);            
            FechasTrackingComponenteCRC fechas = JsonConvert.DeserializeObject<FechasTrackingComponenteCRC>(tracking.JsonFechasCRC);  

            switch (estatus) 
            {
                case 2:
                    fechas.fechaRecepcion = fecha.ToString("dd/MM/yy");
                    tracking.estatus = 4;
                    break;
                case 4:
                    fechas.fechaCotizacion = fecha.ToString("dd/MM/yy");
                    var definition = new { claveCotizacion = "", costo = "", parcial= "" };
                    var info = JsonConvert.DeserializeAnonymousType(DatosExtra, definition);
                    fechas.claveCotizacion = info.claveCotizacion;
                    fechas.costo = info.costo;
                    fechas.parcial = info.parcial;
                    tracking.costoCRC = System.Convert.ToDecimal(info.costo);
                    tracking.estatus = 5;
                    break;
                case 5:
                    var idMaquina = getUltimoEconomicoID(tracking.componenteID);
                    var maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == idMaquina);
                    fechas.fechaAutorizacion = fecha.ToString("dd/MM/yy");
                    var definition5 = new { notaCredito = false };
                    var info5 = JsonConvert.DeserializeAnonymousType(DatosExtra, definition5);
                    decimal costoFinal = 0;
                    Decimal.TryParse(fechas.costo, out costoFinal);
                    //Crear Nota de Crédito
                    if (info5.notaCredito) {
                        var horometroMaquina = maquina != null ? _context.tblM_CapHorometro.Where(x => x.Economico.Contains(maquina.noEconomico)).OrderByDescending(y => y.Fecha).FirstOrDefault() : null;//.HorometroAcumulado;
                        var remocion = _context.tblM_ReporteRemocionComponente.Where(x => x.componenteRemovidoID == tracking.componenteID).OrderByDescending(x => x.fechaRemocion).FirstOrDefault();
                        tblM_CapNotaCredito nota = new tblM_CapNotaCredito();
                        nota.id = 0;
                        nota.Generador = fechas.claveCotizacion;
                        nota.idEconomico = idMaquina;
                        nota.SerieComponente = tracking.componente.noComponente;
                        nota.Descripcion = "";
                        nota.Fecha = DateTime.ParseExact(fechas.fechaCotizacion, "dd/MM/yy", System.Globalization.CultureInfo.InvariantCulture);
                        //nota.CausaRemosion = (tracking. remocion.motivoRemocionID) + 1;
                        //nota.MontoPesos = fechas.costo;                        
                        nota.MontoDLL = costoFinal;
                        //nota.AbonoDLL = ;
                        nota.Estado = 1;                        
                        nota.FechaCaptura = DateTime.Today;
                        nota.idUsuarioModifico = usuario;
                        nota.CadenaModifica = "";
                        nota.EstatusModifica = 0;
                        nota.TipoNC = 1;
                        nota.folio = "";
                        nota.Descripcion = tracking.componente.subConjunto.descripcion;
                        nota.HorometroComponente = tracking.componente.horaCicloActual;
                        nota.HorometroEconomico = horometroMaquina != null ? horometroMaquina.HorometroAcumulado : 0;
                        var notaCreditoID = GuardarNotaCredito(nota);
                        fechas.notaCredito = notaCreditoID;
                    }
                    int vidasActual = GetVidasComponenteTracking(tracking.id) + tracking.componente.vidaInicio;
                    GuardarCostoPresupuesto(tracking.componenteID, costoFinal, tracking.componente.modeloEquipoID ?? default(int), DateTime.Today.Year, idMaquina, vidasActual);
                    tracking.estatus = 6;
                    break;
                case 6:
                    fechas.fechaRequisicion = fecha.ToString("dd/MM/yy");
                    var definition4 = new { folioRequisicion = "" };
                    var info4 = JsonConvert.DeserializeAnonymousType(DatosExtra, definition4);
                    fechas.folioRequisicion = info4.folioRequisicion; 
                    tracking.estatus = 7;
                    break;
                case 7:
                    fechas.fechaEnvioOC = fecha.ToString("dd/MM/yy");
                    var definition2 = new { ordenCompra = "", comprador = ""};
                    var info2 = JsonConvert.DeserializeAnonymousType(DatosExtra, definition2);
                    fechas.OC = info2.ordenCompra;
                    fechas.comprador = info2.comprador;
                    var notaCreditoModificar = _context.tblM_CapNotaCredito.FirstOrDefault(x => x.id == fechas.notaCredito);
                    if (notaCreditoModificar != null) { notaCreditoModificar.OC = info2.ordenCompra; }
                    _context.SaveChanges();
                    tracking.estatus = 8;
                    break;
                case 8:
                    fechas.fechaTerminacion = fecha.ToString("dd/MM/yy");
                    tracking.estatus = 9;
                    break;
                case 9:
                    ///Nuevo tracking
                    tblM_trackComponentes nuevoTrack = new tblM_trackComponentes();
                    FechasTrackingComponenteCRC nuevoTrackFechas = new FechasTrackingComponenteCRC();
                    nuevoTrackFechas.fechaEnvio = DateTime.Today.ToString("dd/MM/yy");
                    var componente = _context.tblM_CatComponente.FirstOrDefault(x => x.id == tracking.componenteID);
                    var definition3 = new { almacen = "", folioFactura="", tipoLocacion="" };
                    var info3 = JsonConvert.DeserializeAnonymousType(DatosExtra, definition3);
                    nuevoTrack.id = 0;
                    nuevoTrack.componenteID = tracking.componenteID;
                    nuevoTrack.tipoLocacion = true;
                    if (intercambio) {
                        nuevoTrack.locacionID = 1019;
                        nuevoTrack.estatus = 3;
                    }
                    else{
                        nuevoTrack.locacionID = Int32.Parse(info3.almacen);
                        nuevoTrack.estatus = Int32.Parse(info3.tipoLocacion);
                    }
                    nuevoTrack.locacion = getLocacionByID(nuevoTrack.locacionID ?? default(int));
                    nuevoTrack.fecha = fecha;
                    nuevoTrack.JsonFechasCRC = JsonConvert.SerializeObject(nuevoTrackFechas);
                    if (fechas.parcial == "false") { nuevoTrack.reciclado = true; }
                    //actualización de horas componente
                    if (fechas.parcial == "false") { componente.horaCicloActual = 0; }
                    componenteDAO.Guardar(componente);
                    Guardar(nuevoTrack);
                    //actualizacion fechas
                    fechas.fechaRecoleccion = fecha.ToString("dd/MM/yy");
                    fechas.almacen = info3.almacen;
                    fechas.folioFactura = info3.folioFactura;
                    tracking.estatus = 10;
                    break;
            }
            tracking.JsonFechasCRC = JsonConvert.SerializeObject(fechas);            
            Guardar(tracking);

        }

        private void GuardarCostoPresupuesto(int componenteID, decimal costo, int modelo, int anio, int idMaquina, int vida)
        {
            using (var context = new MainContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var detalleAModificar = context.tblM_DetallePresupuestoOverhaul.FirstOrDefault(x => x.componenteID == componenteID && x.estado < 2);
                        if (detalleAModificar != null)
                        {
                            var presupuesto = context.tblM_PresupuestoOverhaul.FirstOrDefault(x => x.id == detalleAModificar.presupuestoID);
                            if (presupuesto != null)
                            {
                                if (presupuesto.anio == anio)
                                {
                                    detalleAModificar.costoReal = costo;
                                    detalleAModificar.estado = 2;

                                    var detallesPresupuesto = context.tblM_DetallePresupuestoOverhaul.Where(x => x.presupuestoID == presupuesto.id && x.estado < 2).ToList();
                                    var obrasPresupuesto = JsonConvert.DeserializeObject<List<FechasPresupuestoObraDTO>>(presupuesto.JsonObras);
                                    foreach (var item2 in obrasPresupuesto)
                                    {
                                        var detallesObra = detallesPresupuesto.Where(x => x.obra == item2.obraID && x.estado < 2).ToList();
                                        if (detallesObra.Count() == 0)
                                            item2.estado = 6;
                                    }
                                    presupuesto.JsonObras = JsonConvert.SerializeObject(obrasPresupuesto);
                                    if (detallesPresupuesto.Count() == 0)
                                        presupuesto.estado = 2;
                                }
                                else
                                {
                                    context.tblM_DetallePresupuestoOverhaul.Remove(detalleAModificar);
                                    tblM_DetallePresupuestoOverhaul nuevoDetalle = new tblM_DetallePresupuestoOverhaul();
                                    var maquina = context.tblM_CatMaquina.FirstOrDefault(x => x.id == idMaquina);
                                    var componente = context.tblM_CatComponente.FirstOrDefault(x => x.id == componenteID);
                                    var obras = JsonConvert.DeserializeObject<List<FechasPresupuestoObraDTO>>(presupuesto.JsonObras);
                                    var obraActual = maquina != null ? maquina.centro_costos : "";
                                    var obraDescripcion = context.tblP_CC.FirstOrDefault(x => x.areaCuenta == obraActual);
                                    if (obras != null)
                                    {
                                        var obraPres = obras.FirstOrDefault(x => x.obraID == obraActual);
                                        if (obraPres != null)
                                        {
                                            FechasPresupuestoObraDTO obraAgregar = new FechasPresupuestoObraDTO();
                                            obraAgregar.estado = 6;
                                            obraAgregar.fechaAutorizacion = default(DateTime);
                                            obraAgregar.fechaEnvio = default(DateTime);
                                            obraAgregar.fechaVoBo1 = default(DateTime);
                                            obraAgregar.fechaVoBo2 = default(DateTime);
                                            obraAgregar.obraID = obraActual;
                                            obraAgregar.obra = obraDescripcion != null ? obraDescripcion.descripcion : "";
                                            obras.Add(obraAgregar);
                                            presupuesto.JsonObras = JsonConvert.SerializeObject(obras);
                                        }
                                    }

                                    nuevoDetalle.programado = false;
                                    nuevoDetalle.componenteID = componenteID;
                                    nuevoDetalle.costoReal = costo;
                                    nuevoDetalle.costoPresupuesto = 0;
                                    nuevoDetalle.costoSugerido = 0;
                                    nuevoDetalle.estado = 2;
                                    nuevoDetalle.fecha = DateTime.Today;
                                    nuevoDetalle.horasAcumuladas = componente != null ? componente.horasAcumuladas : 0;
                                    nuevoDetalle.horasCiclo = componente != null ? componente.horaCicloActual : 0;
                                    nuevoDetalle.id = 0;
                                    nuevoDetalle.maquinaID = idMaquina;
                                    nuevoDetalle.obra = maquina != null ? maquina.centro_costos : "";
                                    nuevoDetalle.presupuestoID = presupuesto.id;
                                    nuevoDetalle.tipo = 3;
                                    nuevoDetalle.vida = vida;
                                    context.tblM_DetallePresupuestoOverhaul.Add(nuevoDetalle);
                                }
                            }
                        }
                        else {
                            var maquina = context.tblM_CatMaquina.FirstOrDefault(x => x.id == idMaquina);
                            var componente = context.tblM_CatComponente.FirstOrDefault(x => x.id == componenteID);
                            var presupuesto = context.tblM_PresupuestoOverhaul.FirstOrDefault(x => x.modelo == modelo && x.anio == anio);
                            var obras = JsonConvert.DeserializeObject<List<FechasPresupuestoObraDTO>>(presupuesto.JsonObras);
                            var obraActual = maquina != null ? maquina.centro_costos : "";
                            var obraDescripcion = context.tblP_CC.FirstOrDefault(x => x.areaCuenta == obraActual);
                            if(obras != null)
                            {
                                var obraPres = obras.FirstOrDefault(x => x.obraID == obraActual);
                                if(obraPres == null)
                                {
                                    FechasPresupuestoObraDTO obraAgregar = new FechasPresupuestoObraDTO();
                                    obraAgregar.estado = 6;
                                    obraAgregar.fechaAutorizacion = default(DateTime);
                                    obraAgregar.fechaEnvio = default(DateTime);
                                    obraAgregar.fechaVoBo1 = default(DateTime);
                                    obraAgregar.fechaVoBo2 = default(DateTime);
                                    obraAgregar.obraID = obraActual;
                                    obraAgregar.obra = obraDescripcion != null ? obraDescripcion.descripcion : "";
                                    obras.Add(obraAgregar);
                                    presupuesto.JsonObras = JsonConvert.SerializeObject(obras);
                                }
                            }
                            tblM_DetallePresupuestoOverhaul nuevoDetalle = new tblM_DetallePresupuestoOverhaul();

                            nuevoDetalle.programado = false;
                            nuevoDetalle.componenteID = componenteID;
                            nuevoDetalle.costoReal = costo;
                            nuevoDetalle.costoPresupuesto = 0;
                            nuevoDetalle.costoSugerido = 0;
                            nuevoDetalle.estado = 2;
                            nuevoDetalle.fecha = DateTime.Today;
                            nuevoDetalle.horasAcumuladas = componente != null ? componente.horasAcumuladas : 0;
                            nuevoDetalle.horasCiclo = componente != null ? componente.horaCicloActual : 0;
                            nuevoDetalle.id = 0;
                            nuevoDetalle.maquinaID = idMaquina;
                            nuevoDetalle.obra = maquina != null ? maquina.centro_costos : "";
                            nuevoDetalle.presupuestoID = presupuesto.id;
                            nuevoDetalle.tipo = 3;
                            nuevoDetalle.vida = vida;
                            context.tblM_DetallePresupuestoOverhaul.Add(nuevoDetalle);
                        }
                        context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }                
        }

        public ComboDTO getMaquinaModalCRC(int idComponente) 
        {
            ComboDTO data = new ComboDTO();
            var tracking = _context.tblM_trackComponentes.Where(x => x.componenteID == idComponente).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).Skip(1).First();
            if (tracking.tipoLocacion == false) {
                var maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == tracking.locacionID);
                
                data.Value = maquina.noEconomico;
                data.Prefijo = maquina.modeloEquipo.idGrupo.ToString();
                data.Text = maquina.modeloEquipo.descripcion;
            }
            else {
                data.Value = "N/A";
                data.Prefijo = "-1";
                data.Text = "N/A"; 
            }
            return data;
        }

        public tblM_trackComponentes getTrackingByID(int idTrack)
        {
            tblM_trackComponentes data = _context.tblM_trackComponentes.FirstOrDefault(x => x.id == idTrack);
            return data;
        }

        public tblM_trackComponentes getTrackingByIDComp(int idComponente)
        {
            tblM_trackComponentes data = new tblM_trackComponentes();
            var componente = _context.tblM_CatComponente.FirstOrDefault(x => x.id == idComponente);
            if (componente != null && componente.trackComponenteID != null) { data = _context.tblM_trackComponentes.FirstOrDefault(x => x.id == componente.trackComponenteID); }
            //data = _context.tblM_trackComponentes.Where(x => x.componente.subConjunto.estatus == true).GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault()).FirstOrDefault(x => x.componenteID == idComponente);
            //tblM_trackComponentes data = _context.tblM_trackComponentes.FirstOrDefault(x => x.id == idTrack);
            return data;
        }

        public tblM_trackComponentes getTrackingByComponente(int idComponente)
        {
            tblM_trackComponentes data = new tblM_trackComponentes();
            var trackActual = _context.tblM_trackComponentes.Where(x => x.componente.subConjunto.estatus == true).GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault())/*.Where(x => x.estatus == 0)*/;
            data = trackActual.FirstOrDefault(x => x.componenteID == idComponente);
            return data;
        }

        public void cambioAlmacen(List<int> arrComponentes, int idAlmacen, int estatus) 
        {
            for(int i = 0; i < arrComponentes.Count; i++)
            {
                tblM_trackComponentes nuevoTrack = new tblM_trackComponentes();
                nuevoTrack.id = 0;
                nuevoTrack.fecha = DateTime.Now;
                nuevoTrack.estatus = estatus;
                nuevoTrack.componenteID = arrComponentes[i];
                nuevoTrack.locacionID = idAlmacen;
                nuevoTrack.locacion = getLocacionByID(idAlmacen);
                nuevoTrack.tipoLocacion = true;
                nuevoTrack.reciclado = false;
                Guardar(nuevoTrack);
            }            
        }

        public bool GuardarEntradaAlmacen(int trackingID, DateTime fecha) 
        {
            try 
            {
                var tracking = _context.tblM_trackComponentes.FirstOrDefault(x => x.id == trackingID);
                var fechas = tracking.JsonFechasCRC == null ?  new FechasTrackingComponenteCRC() : JsonConvert.DeserializeObject<FechasTrackingComponenteCRC>(tracking.JsonFechasCRC);
                if (fechas == null) { fechas = new FechasTrackingComponenteCRC(); }
                fechas.entradaAlmacen = fecha;
                tracking.JsonFechasCRC = JsonConvert.SerializeObject(fechas);
                _context.SaveChanges();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool ReactivarComponentesInactivos(int componenteID, int locacionID, DateTime fecha) 
        {
            try
            {
                var componente = _context.tblM_CatComponente.FirstOrDefault(x => x.id == componenteID);
                var locacion = _context.tblM_CatLocacionesComponentes.FirstOrDefault(x => x.tipoLocacion == 1 && x.id == locacionID);
                var trackingAnterior = _context.tblM_trackComponentes.FirstOrDefault(x => x.id == componente.trackComponenteID);
                FechasTrackingComponenteCRC fechas = new FechasTrackingComponenteCRC();
                fechas.fechaEnvio = fecha.ToString("dd/MM/yy");
                tblM_trackComponentes trackNuevo = new tblM_trackComponentes
                {
                    id = 0,
                    componenteID = trackingAnterior.componenteID,
                    costoCRC = 0,
                    estatus = 1,
                    fecha = fecha,
                    horasAcumuladas = componente.horasAcumuladas,
                    horasCiclo = componente.horaCicloActual,
                    JsonFechasCRC = JsonConvert.SerializeObject(fechas),
                    locacion = locacion.descripcion,
                    locacionID = locacion.id,
                    reciclado = false,
                    tipoLocacion = true
                };
                _context.tblM_trackComponentes.Add(trackNuevo);
                _context.SaveChanges();
                componente.trackComponenteID = trackNuevo.id;
                _context.SaveChanges();
   
                return true;
            }
            catch (Exception e) { return false; }
        }

        public List<tblM_CatMaquina> getMaquinasByCC() 
        {
            return _context.tblM_CatMaquina.Where(x => x.grupoMaquinaria.tipoEquipoID == 3 && x.estatus > 0).ToList();
        }

        public List<ComboDTO> getEmpleadosChoferAlmacen(string term)
        {

            string auxTermino = term + "%";
            List<ComboDTO> data = new List<ComboDTO>();
            List<string> terminos = term.Split(' ').ToList();
            if (terminos.Count() > 1)
            {
                auxTermino = "%";
                foreach (var item in terminos)
                {
                    auxTermino += item;
                    auxTermino += "%";
                }
            }
            var getCatEmpleado =
                @"SELECT " +
                    "a.clave_empleado AS Value, " +
                    "(LTRIM(RTRIM(a.nombre))+' '+replace(a.ape_paterno, ' ', '')+' '+replace(a.ape_materno, ' ', '')) AS Text, " +
                    "a.CC_Contable AS Prefijo " +
                "FROM DBA.sn_empleados a " +
                "INNER JOIN si_puestos b " +
                "ON a.puesto = b.puesto AND a.tipo_nomina = b.tipo_nomina " +
                "WHERE a.estatus_empleado = 'A' AND Text like '" + auxTermino + "'";
            try
            {
                //var resultado = (IList<ComboDTO>)ContextEnKontrolNominaArrendadora.Where(getCatEmpleado, 1).ToObject<IList<ComboDTO>>();

                var resultado = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT 
                                    a.clave_empleado AS Value, 
                                    (LTRIM(RTRIM(a.nombre))+' '+replace(a.ape_paterno, ' ', '')+' '+replace(a.ape_materno, ' ', '')) AS Text, 
                                    a.CC_Contable AS Prefijo 
                                FROM tblRH_EK_Empleados a 
                                INNER JOIN tblRH_EK_Puestos b 
                                ON a.puesto = b.puesto AND a.tipo_nomina = b.FK_TipoNomina
                                WHERE a.estatus_empleado = 'A' AND (LTRIM(RTRIM(a.nombre))+' '+replace(a.ape_paterno, ' ', '')+' '+replace(a.ape_materno, ' ', '')) like '%" + auxTermino + "%'",
                });

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

        public List<ComboDTO> getCompradores(string term)
        {

            string auxTermino = term + "%";
            List<ComboDTO> data = new List<ComboDTO>();
            List<string> terminos = term.Split(' ').ToList();
            if (terminos.Count() > 1)
            {
                auxTermino = "%";
                foreach (var item in terminos)
                {
                    auxTermino += item;
                    auxTermino += "%";
                }
            }

            var compradores =
                @"SELECT TOP 10 
                    b.clave_empleado AS Value, 
                    (b.nombre + ' ' + b.ape_paterno + ' ' + b.ape_materno) AS Text 
                    FROM DBA.si_puestos a join DBA.sn_empleados b ON a.puesto = b.puesto 
                    WHERE a.descripcion LIKE '%COMPRA%' AND a.descripcion NOT LIKE '%(NO USAR)%' AND a.descripcion NOT LIKE '%CHOFER%' 
                    AND Text LIKE '" + auxTermino + "' ORDER BY Text";

            try
            {
                //var resultado = (IList<ComboDTO>)ContextEnKontrolNominaArrendadora.Where(compradores, 1).ToObject<IList<ComboDTO>>();

                var resultado = _context.Select<ComboDTO>(new DapperDTO
                {
                baseDatos = MainContextEnum.Construplan,
                consulta = @"SELECT TOP 10 
                                b.clave_empleado AS Value, 
                                (b.nombre + ' ' + b.ape_paterno + ' ' + b.ape_materno) AS Text 
                                FROM tblRH_EK_Puestos a join tblRH_EK_Empleados b ON a.puesto = b.puesto 
                            WHERE a.descripcion LIKE '%COMPRA%' AND a.descripcion NOT LIKE '%(NO USAR)%' AND a.descripcion NOT LIKE '%CHOFER%' 
                            AND (b.nombre + ' ' + b.ape_paterno + ' ' + b.ape_materno) LIKE '%" + auxTermino + "%' ORDER BY (b.nombre + ' ' + b.ape_paterno + ' ' + b.ape_materno)",
                });

                foreach (var item in resultado) { data.Add(item); }
            }
            catch { }
            return data;
        }

        public string getUltimoEconomico(int idComponente)
        {
            var track = _context.tblM_trackComponentes.Where(x => x.componenteID == idComponente).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).ToList();

            for (int i = 0; i < track.Count; i++) 
            {
                if (track[i].estatus == 0) 
                {
                    var idMaquina = track[i].locacionID;
                    var maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == idMaquina);
                    if (maquina != null) { return maquina.noEconomico; }
                }
            }
            return "N/A";
        }

        public int getUltimoEconomicoID(int idComponente)
        {
            var track = _context.tblM_trackComponentes.Where(x => x.componenteID == idComponente).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).ToList();

            for (int i = 0; i < track.Count; i++)
            {
                if (track[i].estatus == 0)
                {
                    return track[i].locacionID ?? default (int);
                    //return _context.tblM_CatMaquina.FirstOrDefault(x => x.id == idMaquina).noEconomico;
                }
            }
            return 0;
        }
        public tblM_CatMaquina getUltimaMaquina(int idComponente)
        {
            tblM_CatMaquina data = new tblM_CatMaquina();
            var track = _context.tblM_trackComponentes.Where(x => x.componenteID == idComponente).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).ToList();

            for (int i = 0; i < track.Count; i++)
            {
                if (track[i].estatus == 0)
                {
                    var id = track[i].locacionID ?? default(int);
                    data = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == id);
                    return data;
                }
            }
            return data;
        }

        public int getUltimoEconomicoPorFecha(int idComponente, DateTime fecha)
        {
            var track = _context.tblM_trackComponentes.Where(x => x.componenteID == idComponente && x.fecha < fecha).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).ToList();

            for (int i = 0; i < track.Count; i++)
            {
                if (track[i].estatus == 0)
                {
                    return track[i].locacionID ?? default(int);
                    //return _context.tblM_CatMaquina.FirstOrDefault(x => x.id == idMaquina).noEconomico;
                }
            }
            return 0;
        }

        public DateTime getEntradaAlmacen(int idComponente)
        {
            var track = _context.tblM_trackComponentes.Where(x => x.componenteID == idComponente).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).ToList();

            for (int i = 1; i < track.Count; i++)
            {
                if (track[i].estatus != 1)
                {
                    return track[i - 1].fecha ?? default(DateTime);
                }
            }
            return track.FirstOrDefault().fecha ?? default(DateTime);
        }

        public int GuardarNotaCredito(tblM_CapNotaCredito obj)
        {
            if (obj.id == 0)
            {
                _context.tblM_CapNotaCredito.Add(obj);
                _context.SaveChanges();
                SaveBitacora((int)BitacoraEnum.NOTACREDITO, (int)AccionEnum.AGREGAR, (int)obj.id, JsonUtils.convertNetObjectToJson(obj));                
            }
            else
            {
                var existing = _context.tblM_CatTipoServicioOverhaul.Find((int)obj.id);
                if (existing != null)
                {
                    _context.Entry(existing).CurrentValues.SetValues(obj);
                    _context.SaveChanges();
                }
            }
            return obj.id;
        }

        public string getObraLocacion(int idLocacion) 
        {
            var locacion = _context.tblM_CatLocacionesComponentes.FirstOrDefault(x => x.id == idLocacion);
            if (locacion != null && locacion.tipoLocacion == 1)
            {
                var data = locacion.areaCuenta;
                if (data != null && data == "") { return ""; } 
                else { 
                    return _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == data).cc; 
                }
            }
            else { return ""; }
        }

        public List<RemocionesVidaUtilDTO> CargarRemocionesVidaUtil(int modelo, int grupo, DateTime fechaInicio, DateTime fechaFin) 
        {
            List<RemocionesVidaUtilDTO> data = new List<RemocionesVidaUtilDTO>();
            decimal costoPromedio;
            var tracking = _context.tblM_trackComponentes.Where(x => x.estatus > 5 && x.estatus < 12 && x.fecha >= fechaInicio && x.fecha <= fechaFin).ToList();
            var remociones = _context.tblM_ReporteRemocionComponente.Where(x => (modelo != -1 ? x.componenteRemovido.modeloEquipoID == modelo : true) && (grupo != -1 ? x.componenteRemovido.grupoID == grupo : true) && x.estatus == 5 && x.motivoRemocionID < 5).ToList();
            foreach (var item in remociones) 
            {
                var trackActual = _context.tblM_trackComponentes.Where(x => x.componente.subConjunto.estatus == true).FirstOrDefault(x => x.id == item.trackID);
                if (trackActual != null)
                {
                    var fechas = JsonConvert.DeserializeObject<FechasTrackingComponenteCRC>(trackActual.JsonFechasCRC);
                    DateTime tempFecha = new DateTime();
                    DateTime fechaAutorizacion = new DateTime();
                    if (DateTime.TryParse(fechas.fechaAutorizacion, out tempFecha)) { fechaAutorizacion = DateTime.Parse(fechas.fechaAutorizacion); }
                    Decimal temp = 0;
                    Decimal costo;
                    if (Decimal.TryParse(fechas.costo, out temp)) { costo = Decimal.Parse(fechas.costo); }
                    else { costo = default(Decimal); }

                    var economico = getUltimoEconomicoPorFecha(trackActual.componenteID, trackActual.fecha ?? default(DateTime));
                    var maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == economico);
                    if (maquina.modeloEquipoID == modelo)
                    {
                        RemocionesVidaUtilDTO aux = new RemocionesVidaUtilDTO();
                        costoPromedio = 0;
                        int vidasActual = GetVidasComponenteTracking(item.id) + trackActual.componente.vidaInicio;

                        aux.equipo = maquina.noEconomico;
                        aux.componente = //trackActual.componente.subConjunto.descripcion;
                            trackActual.componente.subConjunto.descripcion + " " + (trackActual.componente.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)trackActual.componente.posicionID).ToUpper() : "");
                        aux.dlls = costo.ToString("C");
                        aux.fecha = fechas.fechaAutorizacion;
                        aux.noComponente = trackActual.componente.noComponente;
                        aux.horometro = trackActual.horasCiclo.ToString("G");
                        aux.horasAcumuladas = trackActual.horasAcumuladas.ToString("G");
                        aux.motivo = DescripcionMotivoRemocion(item.motivoRemocionID);// item.y.motivoRemocionID.ToString();
                        aux.causa = item.comentario;//item.y.comentario;
                        aux.ordenCRC = fechas.OC;
                        aux.vida = vidasActual.ToString();

                        var trackCRCPasados = tracking.Where(x => x.componenteID == trackActual.componenteID && x.fecha < trackActual.fecha);

                        if (trackCRCPasados.Count() > 0)
                        {
                            foreach (var trackCRCPasado in trackCRCPasados)
                            {
                                var auxCosto = JsonConvert.DeserializeObject<FechasTrackingComponenteCRC>(trackCRCPasado.JsonFechasCRC).costo;
                                if (Decimal.TryParse(auxCosto, out temp)) { costoPromedio += Decimal.Parse(auxCosto); }
                            }
                            costoPromedio = costoPromedio / trackCRCPasados.Count();
                        }
                        else { costoPromedio = trackActual.componente.costo; }
                        aux.costoPromedio = costoPromedio.ToString("C");
                        data.Add(aux);
                    }
                }
            }
            return data;
        }

        public int GetVidasComponenteTracking(int trackID)
        {
            int data = 0;
            var trackComponente = _context.tblM_trackComponentes.FirstOrDefault(x => x.id == trackID);
            if (trackComponente != null) 
            {
                var tracks = _context.tblM_trackComponentes.Where(x => x.componenteID == trackComponente.componenteID && x.fecha <= trackComponente.fecha).ToList();
                if (tracks.Count() > 0) { data = tracks.Where(x => x.reciclado == true).Count(); }
            }            
            return data;
        }

        private string DescripcionMotivoRemocion(int motivoID) 
        {
            string descripcion = "";
            switch (motivoID) 
            {
                case 0:
                    descripcion = "VIDA ÚTIL";
                    break;
                case 1:
                    descripcion = "FALLA";
                    break;
                case 2:
                    descripcion = "ESTRATEGIA";
                    break;
            }
            return descripcion;
        }


        //aqui
        public List<rptComponenteReparacionDTO> CargarReporteCompReparacion(string crc ,int grupo, int modelo, int conjunto, int subconjunto)
        {

            return null;
        }
        

        public List<rptInventarioComponenteDTO> CargarReporteInventario(int grupo, int modelo, int conjunto, int subconjunto, string obra)
        {
            List<rptInventarioComponenteDTO> data = new List<rptInventarioComponenteDTO>();
            var auxTrackActual = _context.tblM_trackComponentes.Where(x => x.componente.subConjunto.estatus == true)
                .GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault());
            var trackActual = auxTrackActual.Where(x => (x.estatus == 1 || x.estatus == 2 || (x.estatus > 3 && x.estatus < 12)) && 
                    (grupo == -1 ? true : x.componente.grupoID == grupo) && (modelo == -1 ? true : x.componente.modeloEquipoID == modelo) && 
                    (conjunto == -1 ? true : x.componente.conjuntoID == conjunto) && (subconjunto == -1 ? true : x.componente.subConjuntoID == subconjunto) && 
                    x.componente.modeloEquipoID != null).ToList();
            var inventario = trackActual.GroupBy(y => y.componente.subConjuntoID);
            var locacionesEstaticas = _context.tblM_CatLocacionesComponentes.Where(x => x.estatus == true).Select(x => x.id).ToList();
            foreach (var item in inventario)
            {
                rptInventarioComponenteDTO aux = new rptInventarioComponenteDTO();
                aux.idModelo = item.FirstOrDefault().componente.modeloEquipoID ?? default(int);
                aux.idSubconjunto = item.FirstOrDefault().componente.subConjuntoID ?? default(int);
                //aux.modelo = item.FirstOrDefault().componente.modeloEquipo == null ? "N/A" : item.FirstOrDefault().componente.modeloEquipo.descripcion;
                aux.subconjunto = item.FirstOrDefault().componente.subConjunto.descripcion;
                aux.total = item.Count();
                aux.locaciones = item.GroupBy(x => x.locacionID).Select(x => x.FirstOrDefault().locacionID ?? default (int)).ToList();
                aux.totalesLocaciones = new List<ComboDTO>();
                foreach (var locacion in aux.locaciones)
                {
                    var descripcionLocacion = _context.tblM_CatLocacionesComponentes.FirstOrDefault(x => x.id == locacion).descripcion;
                    var auxTotal = item.Where(x => x.locacionID == locacion).Count();
                    if (auxTotal > 0)
                    {
                        ComboDTO auxTotalLocacion = new ComboDTO();
                        auxTotalLocacion.Value = descripcionLocacion;
                        auxTotalLocacion.Text = auxTotal.ToString();
                        auxTotalLocacion.Prefijo = locacionesEstaticas.IndexOf(locacion).ToString();
                        aux.totalesLocaciones.Add(auxTotalLocacion);
                    }
                }
                data.Add(aux);
            }
            return data.OrderBy(x => x.idModelo).ToList();
        }

        public List<string> getLocaciones() {
            return _context.tblM_CatLocacionesComponentes.Where(x => x.estatus == true).Select(x => x.descripcion).ToList();
        }

        public List<rptValorAlmacenDTO> CargarReporteValorAlmacen(int anioInicial, int anioFinal) 
        {
            List<rptValorAlmacenDTO> data = new List<rptValorAlmacenDTO>();
            var almacenes = _context.tblM_CatLocacionesComponentes.Where(x => x.tipoLocacion == 1).Select(y => new {y.id, y.descripcion}).ToList();
            foreach (var item in almacenes.Select(x => x.id).Distinct().ToList())
            {
                var indexLocacion = almacenes.Select(x => x.id).ToList().IndexOf(item);
                if (indexLocacion > 0)
                {
                    rptValorAlmacenDTO aux = new rptValorAlmacenDTO();
                    int cantidadTotal = 0;
                    aux.idAlmacen = item;
                    aux.almacen = almacenes[indexLocacion].descripcion;
                    DateTime fecha = new DateTime(anioInicial, 2, 1);
                    var auxComponentesFecha = _context.tblM_trackComponentes.Where(x => x.fecha < fecha).ToList();
                    var auxComponentesActual = auxComponentesFecha.GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault()).ToList();
                    var auxMes = auxComponentesActual.Where(x => x.locacionID == aux.idAlmacen).ToList();
                    if (auxMes.Count > 0 && fecha <= DateTime.Today) aux.enero = auxMes.Sum(y => y.costoCRC);
                    else aux.enero = 0;
                    cantidadTotal += auxMes.Count;
                    fecha = new DateTime(anioInicial, 3, 1);
                    auxComponentesFecha = _context.tblM_trackComponentes.Where(x => x.fecha < fecha).ToList();
                    auxComponentesActual = auxComponentesFecha.GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault()).ToList();
                    auxMes = auxComponentesActual.Where(x => x.locacionID == aux.idAlmacen).ToList();
                    if (auxMes.Count > 0 && fecha <= DateTime.Today) aux.febrero = auxMes.Sum(y => y.costoCRC);
                    else aux.febrero = 0;
                    cantidadTotal += auxMes.Count;
                    fecha = new DateTime(anioInicial, 4, 1);
                    auxComponentesFecha = _context.tblM_trackComponentes.Where(x => x.fecha < fecha).ToList();
                    auxComponentesActual = auxComponentesFecha.GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault()).ToList();
                    auxMes = auxComponentesActual.Where(x => x.locacionID == aux.idAlmacen).ToList();
                    if (auxMes.Count > 0 && fecha <= DateTime.Today) aux.marzo = auxMes.Sum(y => y.costoCRC);
                    else aux.marzo = 0;
                    cantidadTotal += auxMes.Count;
                    fecha = new DateTime(anioInicial, 5, 1);
                    auxComponentesFecha = _context.tblM_trackComponentes.Where(x => x.fecha < fecha).ToList();
                    auxComponentesActual = auxComponentesFecha.GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault()).ToList();
                    auxMes = auxComponentesActual.Where(x => x.locacionID == aux.idAlmacen).ToList();
                    if (auxMes.Count > 0 && fecha <= DateTime.Today) aux.abril = auxMes.Sum(y => y.costoCRC);
                    else aux.abril = 0;
                    cantidadTotal += auxMes.Count;
                    fecha = new DateTime(anioInicial, 6, 1);
                    auxComponentesFecha = _context.tblM_trackComponentes.Where(x => x.fecha < fecha).ToList();
                    auxComponentesActual = auxComponentesFecha.GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault()).ToList();
                    auxMes = auxComponentesActual.Where(x => x.locacionID == aux.idAlmacen).ToList();
                    if (auxMes.Count > 0 && fecha <= DateTime.Today) aux.mayo = auxMes.Sum(y => y.costoCRC);
                    else aux.mayo = 0;
                    cantidadTotal += auxMes.Count;
                    fecha = new DateTime(anioInicial, 7, 1);
                    auxComponentesFecha = _context.tblM_trackComponentes.Where(x => x.fecha < fecha).ToList();
                    auxComponentesActual = auxComponentesFecha.GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault()).ToList();
                    auxMes = auxComponentesActual.Where(x => x.locacionID == aux.idAlmacen).ToList();
                    if (auxMes.Count > 0 && fecha <= DateTime.Today) aux.junio = auxMes.Sum(y => y.costoCRC);
                    else aux.junio = 0;
                    cantidadTotal += auxMes.Count;
                    fecha = new DateTime(anioInicial, 8, 1);
                    auxComponentesFecha = _context.tblM_trackComponentes.Where(x => x.fecha < fecha).ToList();
                    auxComponentesActual = auxComponentesFecha.GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault()).ToList();
                    auxMes = auxComponentesActual.Where(x => x.locacionID == aux.idAlmacen).ToList();
                    if (auxMes.Count > 0 && fecha <= DateTime.Today) aux.julio = auxMes.Sum(y => y.costoCRC);
                    else aux.julio = 0;
                    cantidadTotal += auxMes.Count;
                    fecha = new DateTime(anioInicial, 9, 1);
                    auxComponentesFecha = _context.tblM_trackComponentes.Where(x => x.fecha < fecha).ToList();
                    auxComponentesActual = auxComponentesFecha.GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault()).ToList();
                    auxMes = auxComponentesActual.Where(x => x.locacionID == aux.idAlmacen).ToList();
                    if (auxMes.Count > 0 && fecha <= DateTime.Today) aux.agosto = auxMes.Sum(y => y.costoCRC);
                    else aux.agosto = 0;
                    cantidadTotal += auxMes.Count;
                    fecha = new DateTime(anioInicial, 10, 1);
                    auxComponentesFecha = _context.tblM_trackComponentes.Where(x => x.fecha < fecha).ToList();
                    auxComponentesActual = auxComponentesFecha.GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault()).ToList();
                    auxMes = auxComponentesActual.Where(x => x.locacionID == aux.idAlmacen).ToList();
                    if (auxMes.Count > 0 && fecha <= DateTime.Today) aux.septiembre = auxMes.Sum(y => y.costoCRC);
                    else aux.septiembre = 0;
                    cantidadTotal += auxMes.Count;
                    fecha = new DateTime(anioInicial, 11, 1);
                    auxComponentesFecha = _context.tblM_trackComponentes.Where(x => x.fecha < fecha).ToList();
                    auxComponentesActual = auxComponentesFecha.GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault()).ToList();
                    auxMes = auxComponentesActual.Where(x => x.locacionID == aux.idAlmacen).ToList();
                    if (auxMes.Count > 0 && fecha <= DateTime.Today) aux.octubre = auxMes.Sum(y => y.costoCRC);
                    else aux.octubre = 0;
                    cantidadTotal += auxMes.Count;
                    fecha = new DateTime(anioInicial, 12, 1);
                    auxComponentesFecha = _context.tblM_trackComponentes.Where(x => x.fecha < fecha).ToList();
                    auxComponentesActual = auxComponentesFecha.GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault()).ToList();
                    auxMes = auxComponentesActual.Where(x => x.locacionID == aux.idAlmacen).ToList();
                    if (auxMes.Count > 0 && fecha <= DateTime.Today) aux.noviembre = auxMes.Sum(y => y.costoCRC);
                    else aux.noviembre = 0;
                    cantidadTotal += auxMes.Count;
                    fecha = new DateTime(anioInicial + 1, 1, 1);
                    auxComponentesFecha = _context.tblM_trackComponentes.Where(x => x.fecha < fecha).ToList();
                    auxComponentesActual = auxComponentesFecha.GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault()).ToList();
                    auxMes = auxComponentesActual.Where(x => x.locacionID == aux.idAlmacen).ToList();
                    if (auxMes.Count > 0 && fecha <= DateTime.Today) aux.diciembre = auxMes.Sum(y => y.costoCRC);
                    else aux.diciembre = 0;
                    cantidadTotal += auxMes.Count;
                    if(cantidadTotal > 0) data.Add(aux);
                }
            }
            return data;
        }

        public List<ComboDTO> FillCboAniosValorAlmacen()
        {
            List<ComboDTO> data = new List<ComboDTO>();
            var anioInicial = _context.tblM_trackComponentes.OrderBy(x => x.fecha).FirstOrDefault().fecha.Value.Year;
            for (int i = anioInicial; i <= DateTime.Today.Year; i++)
            {
                ComboDTO aux = new ComboDTO();
                aux.Value = i.ToString();
                aux.Text = i.ToString();
                data.Add(aux);
            }
            return data;
        }

        public List<rptListadoMaestroDTO> CargarReporteMaestro(int idCalendario) {
            List<rptListadoMaestroDTO> data = new List<rptListadoMaestroDTO>();
            var planeaciones = _context.tblM_CapPlaneacionOverhaul.Where(x => x.estatus == 0 && x.calendarioID == idCalendario).ToList();
            foreach (var item in planeaciones) 
            {
                rptListadoMaestroDTO aux = new rptListadoMaestroDTO();
                var maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == item.maquinaID);
                var capRitmoHorometro = _context.tblM_CapRitmoHorometro.FirstOrDefault(x => x.economico == maquina.noEconomico);
                aux.cc = maquina.centro_costos;
                aux.obra = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == maquina.centro_costos).descripcion;
                if(capRitmoHorometro != null) aux.ritmo = capRitmoHorometro.horasDiarias;
                aux.equipo = maquina.noEconomico;
                aux.idMaquina = item.maquinaID;
                aux.idPlaneacionOH = item.id;
                aux.fechaPCR = item.fecha.ToString("dd/MM/yyyy");
                switch (item.tipo) 
                {
                    case 0: aux.tipo = "OVERHAUL GENERAL";
                        var auxServicio = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(item.idComponentes).FirstOrDefault(x => x.Value == "1").componenteID;
                        var servicio = _context.tblM_CatServicioOverhaul.FirstOrDefault(x => x.id == auxServicio);
                        aux.target = servicio.cicloVidaHoras;
                        aux.hrsComponente = servicio.horasCicloActual;
                        break;
                    case 1: aux.tipo = "CAMBIO DE MOTOR";
                        var auxMotor = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(item.idComponentes).FirstOrDefault(x => Regex.IsMatch(x.nombre.Trim(), "^.+MOTOR$"));                        
                        int motorID = 0;
                        bool exito2 = Int32.TryParse(auxMotor.nombre, out motorID);
                        if (exito2)
                        {
                            var motor = _context.tblM_CatComponente.FirstOrDefault(x => x.id == motorID);
                            aux.target = motor.cicloVidaHoras;
                            aux.hrsComponente = motor.horaCicloActual;
                        }
                        else 
                        {
                            aux.target = 0;
                            aux.hrsComponente = 0;
                        }
                        break;
                    case 2: aux.tipo = "CAMBIO DE COMPONENTE DESFASADO";
                        var auxComponentes = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(item.idComponentes).Select(x => x.componenteID);
                        var componentes = _context.tblM_CatComponente.Where(x => auxComponentes.Contains(x.id));
                        var componenteProximo = componentes.FirstOrDefault(x => (x.cicloVidaHoras - x.horaCicloActual) == componentes.Min(y => y.cicloVidaHoras - y.horaCicloActual));
                        aux.target = componenteProximo.cicloVidaHoras;
                        aux.hrsComponente = componenteProximo.horaCicloActual;
                        break;
                    case 3: aux.tipo = "FALLO";
                        break;
                }
                data.Add(aux);
            }
            return data;
        }

        public List<ComboDTO> CargarDatosDetalleMaestro(int idPlaneacionOH) 
        {
            List<ComboDTO> data = new List<ComboDTO>();
            
            var detalles = _context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.id == idPlaneacionOH).idComponentes;
                       
            data = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(detalles).Select(x => new ComboDTO
            {
                Value = x.Value,
                Text = x.componenteID.ToString(),
                Prefijo = x.nombre
            }).ToList();
            return data;
        }

        public tblM_CapPlaneacionOverhaul CargarEventoOverhaul(int idPlaneacionOH)
        {
            var detalles = _context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.id == idPlaneacionOH);
            return detalles;
        }

        public List<ComboDTO> FillCboCalendarioReporteMaestro() 
        {
            return _context.tblM_CalendarioPlaneacionOverhaul.Select(x => new ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.nombre
            }).ToList();
        }

        public List<ComboDTO> CargarDatosDetalleMaestroPlaneacion(string indexCal) 
        {
            List<ComboDTO> data = new List<ComboDTO>();            
            var detalles = _context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.indexCal == indexCal).idComponentes;
            data = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(detalles).Select(x => new ComboDTO 
            {
                Value = x.Value,
                Text = x.componenteID.ToString(),
                Prefijo = x.nombre
            }).ToList();
            return data;
        }

        public List<tblM_trackComponentes> GetReporteConjunto(bool enProceso)
        {
            List<tblM_trackComponentes> data = new List<tblM_trackComponentes>();
            if (enProceso)
            {
                data = _context.tblM_trackComponentes.GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault()).Where(x => x.estatus == 2 || (x.estatus > 3 && x.estatus < 10)).ToList();
                var dataExtra = _context.tblM_trackComponentes.Where(x => x.estatus >= 10).ToList();
                data.AddRange(dataExtra);
            }
            else 
            {
                data = _context.tblM_trackComponentes.Where(x => x.estatus >= 10).ToList();
            }
            
            
            //data = _context.tblM_trackComponentes.Where(x => x.estatus == 2 ||(x.estatus > 3 && x.estatus < 10)).ToList();
            return data;
        }

        public tblP_CC getCCByEconomico(string economico) 
        {
            try
            {
                var centroCostos = _context.tblM_CatMaquina.FirstOrDefault(x => x.noEconomico.Equals(economico)).centro_costos;
                if (centroCostos == "997")
                {
                    centroCostos = "14-1";
                }
                var cc = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta.Equals(centroCostos));
                return cc;
            }
            catch (Exception e) 
            {
                return new tblP_CC();
            }
        }

        public tblM_trackComponentes GetSiguenteTracking(int id, int tipo = -1) 
        {
            var data = new tblM_trackComponentes();
            try
            {
                var trackingActual = _context.tblM_trackComponentes.FirstOrDefault(x => x.id == id);
                data = _context.tblM_trackComponentes.Where(x => x.componenteID == trackingActual.componenteID && x.fecha >= trackingActual.fecha && x.id > trackingActual.id && (tipo == -1 ? true : (tipo == 2 ? (x.estatus == 3 || x.estatus > 3) : x.estatus == tipo)))
                    .OrderBy(x => x.fecha).ThenBy(x => x.id).FirstOrDefault();
                return data;
            }
            catch (Exception e) 
            {
                return data;
            }
        }

        public tblM_trackComponentes getUltimoTrackCRC(int componenteID) 
        {
            var trackActualCRC = _context.tblM_trackComponentes.Where(x => x.componenteID == componenteID && (x.estatus == 2 || x.estatus > 3)).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).FirstOrDefault();
            return trackActualCRC;
        }

        public tblM_trackComponentes getTrackAnterior(int componenteID)
        {
            var trackActualCRC = _context.tblM_trackComponentes.Where(x => x.componenteID == componenteID).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).Skip(1).FirstOrDefault();
            return trackActualCRC;
        }

        public tblM_trackComponentes GetTrackUltimaInstalacion(int componenteID, DateTime fecha)
        {
            var trackActualCRC = _context.tblM_trackComponentes.Where(x => x.componenteID == componenteID && (x.estatus == 0 || x.estatus == 12) && x.fecha <= fecha).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).FirstOrDefault();
            return trackActualCRC;
        }

        public List<ComboDTO> getFacturaTrackAnterior(List<int> componentesID)
        {
            tblM_trackComponentes trackActualCRC = new tblM_trackComponentes();
            List<ComboDTO> data = new List<ComboDTO>();
            foreach (var item in componentesID) 
            {
                ComboDTO auxFolio = new ComboDTO();
                trackActualCRC = _context.tblM_trackComponentes.Where(x => x.componenteID == item).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).Skip(1).FirstOrDefault();
                if (trackActualCRC != null && trackActualCRC.JsonFechasCRC != null && trackActualCRC.JsonFechasCRC != "")
                {
                    FechasTrackingComponenteCRC auxFechas = JsonConvert.DeserializeObject<FechasTrackingComponenteCRC>(trackActualCRC.JsonFechasCRC);
                    if (auxFechas.folioFactura != null) {
                        auxFolio.Value = trackActualCRC.componenteID.ToString();
                        auxFolio.Text = auxFechas.folioFactura;
                        data.Add(auxFolio);
                    }

                }
            }
            return data;
        }

        public List<ComboDTO> getLocacionTrackAnterior(List<int> componentesID)
        {
            tblM_trackComponentes trackActualCRC = new tblM_trackComponentes();
            List<ComboDTO> data = new List<ComboDTO>();
            foreach (var item in componentesID)
            {
                ComboDTO auxFolio = new ComboDTO();
                trackActualCRC = _context.tblM_trackComponentes.Where(x => x.componenteID == item).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).Skip(1).FirstOrDefault();
                if (trackActualCRC != null && trackActualCRC.JsonFechasCRC != null && trackActualCRC.JsonFechasCRC != "")
                {
                    auxFolio.Value = trackActualCRC.componenteID.ToString();
                    auxFolio.Text = trackActualCRC.locacion;
                    data.Add(auxFolio);
                }
            }
            return data;
        }

        #region Reporte Component List

        public List<ComboDTO> FillCboLocacionesComponentList(List<int> modelosID) 
        {
            List<ComboDTO> maquinas = new List<ComboDTO>();
            if (modelosID == null) modelosID = new List<int>();
            var trackings = _context.tblM_CatComponente.Select(x => x.trackComponenteID);
            var locaciones = _context.tblM_trackComponentes.Where(x => x.estatus == 0 && trackings.Contains(x.id)).Select(x => x.locacionID).Distinct().ToList();

            maquinas = _context.tblM_CatMaquina.Where(x => locaciones.Contains(x.id) && (modelosID.Count() > 0 ? modelosID.Contains(x.modeloEquipoID) : true)).Select(x => new ComboDTO
            { 
                Value = x.id.ToString(),
                Text = x.noEconomico
            }).OrderBy(x => x.Text).ToList();

            return maquinas;
        }

        public List<ComboDTO> FillCboAlmacenesInventario()
        {
            var locaciones = _context.tblM_CatLocacionesComponentes.Where(x => x.tipoLocacion == 1 && x.estatus).Select(x => new ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.descripcion
            }).OrderBy(x => x.Text).ToList();

            return locaciones;
        }

        public List<ComboDTO> FillCboComponentes()
        {
            List<ComboDTO> componentes = new List<ComboDTO>();
            componentes = _context.tblM_CatComponente.Select(x => new ComboDTO
            {
                Value = x.noComponente,
                Text = x.noComponente
            }).Distinct().OrderBy(x => x.Text).ToList();
            return componentes;
        }

        public List<ComboDTO> FillCboConjuntos()
        {
            List<ComboDTO> conjuntos = new List<ComboDTO>();
            conjuntos = _context.tblM_CatConjunto.Where(x => x.estatus).Select(x => new ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.descripcion
            }).Distinct().OrderBy(x => x.Text).ToList();
            return conjuntos;
        }

        public List<ComboDTO> FillCboSubconjuntos(int conjunto) 
        {
            List<ComboDTO> subconjuntos = new List<ComboDTO>();
            subconjuntos = _context.tblM_CatSubConjunto.Where(x => (conjunto == -1 ? true : x.conjuntoID == conjunto) && x.estatus).Select(x => new ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.descripcion
            }).Distinct().OrderBy(x => x.Text).ToList();
            return subconjuntos;
        }

        public List<TrackingRitmoDTO> CargarComponentList(int locacion, string noComponente, int conjunto, int subconjunto, List<int> modelo, string obraAC) 
        {
            List<TrackingRitmoDTO> comp = new List<TrackingRitmoDTO>();
            List<RitmoMaquinaDTO> ritmos = new List<RitmoMaquinaDTO>();
            //List<int> maquinasID = new List<int>();
            tblP_CC obra = new tblP_CC();
            var obraID = -1;
            var maquinasID = _context.tblM_CatMaquina.Where(x => (obraAC == "" ? true : x.centro_costos == obraAC) && (modelo.Count() > 0 ? modelo.Contains(x.modeloEquipoID) : true)).Select(x => x.id);

            if (obraAC != "") {
                //maquinasID = _context.tblM_CatMaquina.Where(x => x.centro_costos == obraAC && (modelo.Count() > 0 ? modelo.Contains(x.modeloEquipoID) : true)).Select(x => x.id).ToList();
                obra = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == obraAC);
                if (obra != null) obraID = obra.id;
                else obraID = 0;
            }
            var trackings = _context.tblM_trackComponentes.Where(x => x.componente.trackComponenteID == x.id && x.estatus == 0 && (locacion == -1 ? true : x.locacionID == locacion)
                && x.componente.noComponente.Contains(noComponente) && (conjunto == -1 ? true : x.componente.conjuntoID == conjunto) && (subconjunto == -1 ? true : x.componente.subConjuntoID == subconjunto)
                && (maquinasID.Count() > 0 ? maquinasID.Contains(x.locacionID ?? 0) : true)).ToList();

            ritmos = getRitmoMaquinas(trackings.Where(x => x.estatus == 0).Select(x => new Tuple<int, string>(x.locacionID ?? default (int), x.locacion)).Distinct().ToList());
            
            comp = trackings.Select(x => {
                var auxRitmo = ritmos.FirstOrDefault(y => y.id == x.locacionID);
                x.componente.vidaInicio += _context.tblM_trackComponentes.Where(y => y.componenteID == x.componenteID && y.reciclado).Count();
                return new TrackingRitmoDTO
                {
                    tracking = x,
                    ritmoMaquina = x.estatus == 0 ? ritmos.FirstOrDefault(y => y.id == x.locacionID).ritmo : 1
                };
            }).ToList();            
            return comp;
        }

        public List<tblM_trackComponentes> CargarInventario(int locacion, string noComponente, int conjunto, int subconjunto, List<int> modelo)
        {
            if (modelo == null) modelo = new List<int>();
            var trackings = _context.tblM_trackComponentes.Where(x => x.componente.trackComponenteID == x.id && x.estatus == 1 && (locacion == -1 ? true : x.locacionID == locacion)
                && x.componente.noComponente.Contains(noComponente) && (conjunto == -1 ? true : x.componente.conjuntoID == conjunto) && (subconjunto == -1 ? true : x.componente.subConjuntoID == subconjunto)
                && (modelo.Count() > 0 ? modelo.Contains(x.componente.modeloEquipoID ?? default(int)) : true) ).ToList();
            return trackings;
        }

        private List<RitmoMaquinaDTO> getRitmoMaquinas(List<Tuple<int,string>> trackings)
        {
            var data = new List<RitmoMaquinaDTO>();            
            foreach (var item in trackings) 
            {
                RitmoMaquinaDTO auxRitmoMaquina = new RitmoMaquinaDTO();
                auxRitmoMaquina.id = item.Item1;
                auxRitmoMaquina.noEconomico = item.Item2;
                auxRitmoMaquina.ritmo = CalculoHrsPromDiarioPub(item.Item2);
                data.Add(auxRitmoMaquina);
            }
            return data;
        }

        private decimal CalculoHrsPromDiarioPub(string economico)
        {
            decimal result = 1;
            result = _context.tblM_CapRitmoHorometro.Where(y => y.economico == economico).Select(X => X.horasDiarias).FirstOrDefault();
            if (result == 0) { result = (_context.tblM_CapHorometro.Where(y => y.Economico == economico).GroupBy(x => x.Fecha).OrderByDescending(r => r.Key).Take(20).ToArray().Sum(y => y.Sum(z => z.HorasTrabajo)) / 20); }
            return result;
        }

        #endregion


        #region Historial
        public List<tblM_trackComponentes> CargarTablaHistorial(string componente, int subconjunto, string locacion, DateTime fechaInicio, DateTime fechaFin, int grupo, int modelo)
        {
            //Fecha de liberación
            var fechaLimite = new DateTime(2019, 8, 5);
            var data = _context.tblM_trackComponentes.Where(x => x.componente.noComponente.Contains(componente) && (subconjunto == -1 ? true : x.componente.subConjuntoID == subconjunto)
                && x.locacion.Contains(locacion) && x.fecha >= fechaInicio && x.fecha <= fechaFin && /*x.fecha < fechaLimite &&*/ (grupo == -1 ? true : x.componente.grupoID == grupo) && (modelo == -1 ? true : x.componente.modeloEquipoID == modelo));
            return data.OrderBy(x => x.fecha).ToList();
        }

        public RemocionDTO cargarDatosRemocionHistorial(int idComponente, int trackID)
        {
            RemocionDTO data = new RemocionDTO();
            var trackings = _context.tblM_trackComponentes.Where(x => x.componenteID == idComponente).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).ToList();
            var trackingSiguiente = _context.tblM_trackComponentes.FirstOrDefault(x => x.id == trackID);
            var trackingActual = trackings.SkipWhile(x => x.id != trackID).SkipWhile(x => x.tipoLocacion == true).FirstOrDefault();
            
            
            if (trackingActual != null)
            {
                var reporte = _context.tblM_ReporteRemocionComponente.FirstOrDefault(x => x.trackID == trackID);
                if (reporte != null)
                {
                    var obra = _context.tblP_CC.FirstOrDefault(x => x.cc == reporte.areaCuenta);
                    data.descripcionComponente = reporte.componenteRemovido.subConjunto.descripcion;
                    data.noEconomico = reporte.maquina.noEconomico;
                    data.modelo = reporte.maquina.modeloEquipo.descripcion;

                    data.numParteComponente = reporte.componenteRemovido.numParte;
                    data.serieComponenteRemovido = reporte.componenteRemovido.noComponente;
                    data.serieMaquina = reporte.maquina.noSerie;
                    data.modeloID = reporte.maquina.modeloEquipoID;
                    data.subconjuntoID = reporte.componenteRemovido.subConjuntoID ?? default(int);
                    data.tipoLocacion = false;
                    data.cc = obra != null ? obra.cc : "";
                    data.nombreCC = obra != null ? obra.descripcion : "";
                    data.idModelo = reporte.maquina.modeloEquipoID;
                    data.fechaInstalacionRemovidoRaw = reporte.fechaInstalacionCRemovido ?? default(DateTime);
                    data.fechaInstalacionRemovido = data.fechaInstalacionRemovidoRaw.ToString("dd/MM/yyyy");
                    //data.ultimaReparacion = getUltimaReparacion(idComponente);

                    data.fechaNum = reporte.fechaRemocion;
                    data.componenteRemovidoID = reporte.componenteRemovidoID;
                    data.maquinaID = reporte.maquinaID;
                    data.ccID = reporte.maquina.centro_costos;
                    data.garantia = reporte.garantia;

                    data.motivoID = reporte.motivoRemocionID;
                    data.destinoID = reporte.destinoID;
                    data.comentario = reporte.comentario;
                    data.componenteInstaladoID = reporte.componenteInstaladoID;
                    data.empresaResponsable = reporte.empresaResponsable;
                    data.personal = reporte.personal;
                    data.imgInstalado = reporte.imgComponenteInstalado;
                    data.imgRemovido = reporte.imgComponenteRemovido;
                    data.folioReporte = reporte.id;
                    data.fecha = reporte.fechaRemocion.ToString("dd/MM/yyyy");
                    data.estatus = reporte.estatus;
                    data.empresaInstala = reporte.empresaInstala;
                    data.horas = reporte.horasMaquina;
                    data.horasComponenteRemovido = reporte.horasComponente;
                    data.componenteInstalado = trackingActual.componente.noComponente;
                }
                else
                {
                    var maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == trackingActual.locacionID);
                    var obra = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == maquina.centro_costos);
                    var horometro = maquina != null ? _context.tblM_CapHorometro.Where(x => x.Economico.Contains(maquina.noEconomico) && x.Fecha <= trackingSiguiente.fecha).OrderByDescending(y => y.Fecha).FirstOrDefault() : null;

                    data.descripcionComponente = trackingActual.componente.subConjunto.descripcion;
                    data.noEconomico = trackingActual.locacion;
                    data.modelo = maquina.modeloEquipo.descripcion;

                    data.numParteComponente = trackingActual.componente.numParte;
                    data.serieComponenteRemovido = trackingActual.componente.noComponente;
                    data.serieMaquina = maquina.noSerie;
                    data.modeloID = maquina.modeloEquipoID;
                    data.subconjuntoID = trackingActual.componente.subConjuntoID ?? default(int);
                    data.tipoLocacion = trackingActual.tipoLocacion ?? default(bool);
                    data.cc = obra != null ? obra.cc : "";
                    data.nombreCC = obra != null ? obra.descripcion : "";
                    data.idModelo = maquina.modeloEquipoID;
                    data.fechaInstalacionRemovidoRaw = trackingActual.componente.fecha ?? default(DateTime);
                    data.fechaInstalacionRemovido = data.fechaInstalacionRemovidoRaw.ToString("dd/MM/yyyy");
                    data.horas = horometro != null ? horometro.HorometroAcumulado : 0;

                    data.componenteRemovidoID = trackingActual.componente.id;
                    data.maquinaID = maquina.id;
                    data.ccID = maquina.centro_costos;
                    data.fechaNum = trackingSiguiente == null ? DateTime.Today : trackingSiguiente.fecha ?? default(DateTime);
                    data.fecha = data.fechaNum.ToString("dd/MM/yyyy");

                    data.motivoID = -1;
                    data.destinoID = -1;
                    data.comentario = "";
                    data.componenteInstaladoID = -1;
                    data.empresaResponsable = -1;
                    data.personal = "";
                    data.imgInstalado = "";
                    data.imgRemovido = "";
                    data.folioReporte = 0;
                    data.estatus = -1;
                    data.empresaInstala = -1;
                    data.garantia = true;
                    data.componenteInstalado = "";
                }
            }
            else { data.componenteRemovidoID = -1; }
            return data;
        }

        public int GetReporteDesechoID(int componenteID)
        {
            var reporte = _context.tblM_ReporteRemocionComponente.FirstOrDefault(x => x.componenteRemovidoID == componenteID && x.motivoRemocionID == 5);
            if (reporte != null) { 
                return reporte.id; 
            }
            else { return -1; }
        }

        #endregion

        public DateTime getFechaFacturaEnkontrol(string factura) 
        {
            var lst = new List<OdbcParameterDTO>();
            var definition2 = new { fecha = new DateTime() };
            lst.Add(new OdbcParameterDTO() { nombre = "folio", tipo = OdbcType.VarChar, valor = factura });
            //lst.Add(new OdbcParameterDTO() { nombre = "oc", tipo = OdbcType.VarChar, valor = OC });
            var odbc = new OdbcConsultaDTO()
            {
                consulta = string.Format(@"SELECT fecha FROM sp_movprov where (cfd_serie +  CAST(factura as varchar(20))) = ? OR cc + referenciaoc = ?"),
                parametros = lst
            };
            var data = _contextEnkontrol.Select<FechaFacturaEnkontrolDTO>(EnkontrolAmbienteEnum.Prod, odbc).FirstOrDefault();
            return data == null ? new DateTime() : data.fecha;
        }

        public List<FechaFacturaEnkontrolDTO> getFechaFacturaEnkontrol(List<string> facturas)
        {
            var error = "";
            foreach (var item in facturas) error += "'" + item + "', ";
            var lst = new List<OdbcParameterDTO>();
            var definition2 = new { fecha = new DateTime() };
            //lst.Add(new OdbcParameterDTO() { nombre = "folio", tipo = OdbcType.VarChar, valor = factura });

            lst.AddRange(facturas.Select(s => new OdbcParameterDTO() { nombre = "folio", tipo = OdbcType.VarChar, valor = s }));

            var odbc = new OdbcConsultaDTO()
            {
                consulta = string.Format(@"SELECT * from
                    (SELECT 
                        (cfd_serie + CAST(factura AS varchar)) as factura, 
                        fecha 
                    FROM sp_movprov) X
                    where 
                        X.factura in {0}", facturas.ToParamInValue()),
                parametros = lst
            };
            var data = _contextEnkontrol.Select<FechaFacturaEnkontrolDTO>(EnkontrolAmbienteEnum.Prod, odbc);
            return data;
        }

        public int getNumeroTrackings(int noComponente) 
        {
            int data = 0;
            data = _context.tblM_trackComponentes.Where(x => x.componenteID == noComponente).Count();
            return data;
        }

        public List<tblM_trackComponentes> CargarCompReparacion(int locacion, int grupo, int modelo, int subconjunto)
        {
            var data = _context.tblM_trackComponentes.GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault())
                .Where(x => (x.estatus == 2 || (x.estatus > 3 && x.estatus < 10)) && (locacion == -1 ? true : x.locacionID == locacion) 
                    && (grupo == -1 ? true: x.componente.grupoID == grupo) && (modelo == -1 ? true : x.componente.modeloEquipoID == modelo) && (subconjunto == -1 ? true : x.componente.subConjuntoID == subconjunto) ).ToList();
            return data;
        }

        public string descripcionModelo(int modeloId) {
            tblM_CatModeloEquipo db = _context.tblM_CatModeloEquipo.FirstOrDefault(r => r.id == modeloId);
            if (db != null)
            {
                return db.descripcion;
            }
            else
            {
                return "";
            }
        }

        public List<tblM_trackComponentes> CargarReporteAlmacen(string noComponente, List<int> idLocacion, string descripcionComponente, int estatus, int grupoId, int modeloId)
        {
            var componente = descripcionComponente.Trim();
            List<int> locaciones = new List<int>();
            if (idLocacion != null) locaciones.AddRange(idLocacion);
            var data = _context.tblM_trackComponentes.Where(x => x.componente.trackComponenteID == x.id && x.tipoLocacion == true && x.estatus == estatus
                && x.componente.noComponente.Contains(noComponente) && locaciones.Contains(x.locacionID ?? 0)
                && (componente == "" ? true : x.componente.subConjunto.descripcion.Trim() == componente) &&
                (grupoId == 0 ? true : x.componente.grupoID == grupoId) && (modeloId == 0 ? true : x.componente.modeloEquipoID == modeloId)
                ).ToList();
            return data;
        }      

    }
}