using Core.DAO.Maquinaria.Catalogos;
using Core.DTO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Overhaul;
using Core.Entity.Principal.Multiempresa;
using Core.DTO.Principal.Generales;
using Core.DTO.Maquinaria.Overhaul;
using Newtonsoft.Json;
using Core.Enum;
using Core.Enum.Maquinaria;

namespace Data.DAO.Maquinaria.Catalogos
{
    public class ComponenteDAO : GenericDAO<tblM_CatComponente>, IComponenteDAO
    {
        public void Guardar(tblM_CatComponente obj)
        {
            if (!Exists(obj))
            {
                if (obj.id == 0)
                    SaveEntity(obj, (int)BitacoraEnum.CONJUNTO);
                else
                    Update(obj, obj.id, (int)BitacoraEnum.CONJUNTO);
            }
            else
            {
                throw new Exception("Ya existe un componente con esa descripción seleccionada");
            }
        }
        public int GuardarTrackingComponente(tblM_CatComponente obj, int locacion, DateTime fecha, int tipoLocacion, bool reciclado, string ordenCompra, string costo)
        {
            var locacionActual = _context.tblM_trackComponentes.Where(x => x.componenteID == obj.id).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).FirstOrDefault();
            int idTracking = 0;
            //string locacionStr = "";
            decimal hrsCompFecha = CalcularHrsCompRemovido(obj.id, fecha);
            decimal diferenciaHrs = obj.horaCicloActual - hrsCompFecha;

            //if (locacionActual != null && locacionActual.estatus == 0)
            //{
            //    locacionStr = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == locacionActual.locacionID).noEconomico;
            //    if (fecha.Date < DateTime.Today && locacionStr != "")
            //    {
            //        diferenciaHrs = GetHorasMaquinaPorFecha(locacionStr, DateTime.Now) -
            //        GetHorasMaquinaPorFecha(locacionStr, fecha);
            //    }
            //}
            if (locacionActual == null || locacionActual.locacionID != locacion)
            {
                tblM_trackComponentes tracking = new tblM_trackComponentes();
                FechasTrackingComponenteCRC fechas = new FechasTrackingComponenteCRC();
                if (ordenCompra != null && ordenCompra != "") fechas.OC = ordenCompra;
                if (costo != null && costo != "") fechas.costo = costo;
                fechas.fechaEnvio = fecha.ToString("dd/MM/yy");
                var jsonFechas = JsonConvert.SerializeObject(fechas);

                tracking.componenteID = obj.id;
                tracking.locacionID = locacion;
                tracking.tipoLocacion = (tipoLocacion == 0 ? false : true);
                tracking.locacion = getLocacionByID(locacion, tipoLocacion != 0);
                tracking.fecha = fecha;
                tracking.estatus = tipoLocacion;
                tracking.JsonFechasCRC = jsonFechas;
                tracking.reciclado = reciclado;
                obj.horaCicloActual = obj.horaCicloActual - diferenciaHrs;
                obj.horasAcumuladas = obj.horasAcumuladas - diferenciaHrs;
                Guardar(obj);
                tracking.horasCiclo = obj.horaCicloActual;
                tracking.horasAcumuladas = obj.horasAcumuladas;
                _context.tblM_trackComponentes.Add(tracking);
                _context.SaveChanges();
                idTracking = tracking.id;
            }

            return idTracking;
        }

        private decimal CalcularHrsCompRemovido(int idComponente, DateTime fecha)
        {
            decimal hrsCiclo = 0;
            try
            {
                var trackUltimoReciclado = _context.tblM_trackComponentes.OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).FirstOrDefault(x => x.componenteID == idComponente && x.reciclado == true && x.fecha <= fecha);
                if (trackUltimoReciclado == null)
                {
                    trackUltimoReciclado = _context.tblM_trackComponentes.OrderBy(x => x.fecha).ThenBy(x => x.id).FirstOrDefault();
                }
                var trackFechaUltimo = trackUltimoReciclado.fecha;
                var trackIdUltimo = trackUltimoReciclado.id;
                var trackComponente = _context.tblM_trackComponentes.Where(x => x.componenteID == idComponente && x.fecha >= trackFechaUltimo && x.fecha <= fecha && x.id > trackIdUltimo).OrderBy(x => x.fecha).ThenBy(x => x.id).ToList();
                DateTime fechaActual = new DateTime();
                DateTime fechaSiguiente = new DateTime();
                string Economico = "";
                int locacion = 0;
                for (int i = 0; i < trackComponente.Count(); i++)
                {
                    fechaActual = trackComponente[i].fecha ?? default(DateTime);
                    if ((i + 1) < trackComponente.Count()) { fechaSiguiente = trackComponente[i + 1].fecha ?? default(DateTime); }
                    else { fechaSiguiente = fecha; }
                    if (trackComponente[i].tipoLocacion == false)
                    {
                        locacion = trackComponente[i].locacionID ?? default(int);
                        Economico = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == locacion).noEconomico;
                        var HorometroFinal = _context.tblM_CapHorometro.OrderByDescending(x => x.Fecha).FirstOrDefault(x => x.Economico == Economico && x.Fecha <= fechaSiguiente);
                        if (HorometroFinal == null)
                        {
                            HorometroFinal = _context.tblM_CapHorometro.OrderByDescending(x => x.Fecha).FirstOrDefault(x => x.Economico == Economico);
                        }

                        var HorometroInicial = _context.tblM_CapHorometro.OrderByDescending(x => x.Fecha).FirstOrDefault(x => x.Economico == Economico && x.Fecha < fechaActual);
                        if (HorometroInicial != null)
                        {
                            hrsCiclo += (HorometroFinal.HorometroAcumulado - HorometroInicial.HorometroAcumulado);
                        }
                        else
                        {
                            hrsCiclo += HorometroFinal.HorometroAcumulado;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                hrsCiclo = 0;
            }
            return hrsCiclo;
        }

        public decimal GetHorasMaquinaPorFecha(string noEconomico, DateTime fecha)
        {
            try
            {
                return _context.tblM_CapHorometro.Where(x => x.Economico.Contains(noEconomico) && x.Fecha <= fecha).OrderByDescending(y => y.Fecha).FirstOrDefault().HorometroAcumulado;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        private decimal GetHorasMaquinaPorFecha(string noEconomico, DateTime fechaInicio, DateTime FechaFin)
        {
            try
            {
                var horometrosIntervalo = _context.tblM_CapHorometro.Where(x => x.Economico.Equals(noEconomico) && x.Fecha <= fechaInicio);
                return _context.tblM_CapHorometro.Where(x => x.Economico.Equals(noEconomico) && x.Fecha <= fechaInicio).OrderByDescending(y => y.Fecha).FirstOrDefault().HorometroAcumulado;
            }
            catch (Exception e)
            {
                return 0;
            }
        }


        public bool Exists(tblM_CatComponente obj)
        {
            return _context.tblM_CatComponente.Where(x => x.noComponente == obj.noComponente &&
                                        x.id != obj.id).ToList().Count > 0 ? true : false;
        }
        public List<ComponenteDTO> FillGrid_Componente(ComponenteDTO obj)
        {
            var componentes = _context.tblM_trackComponentes.Where(x => (string.IsNullOrEmpty(obj.noComponente) == true ? true : x.componente.noComponente.Contains(obj.noComponente)) &&
                x.componente.estatus == obj.estatus && (obj.conjuntoID == -1 ? true : x.componente.conjuntoID == obj.conjuntoID)
                          && (obj.subConjuntoID == -1 ? true : x.componente.subConjuntoID == obj.subConjuntoID) && (obj.modeloEquipoID == -1 ? true : obj.modeloEquipoID == x.componente.modeloEquipoID)
                          && x.componente.subConjunto.estatus == true && (obj.locacion == null ? true : x.locacion.Contains(obj.locacion)) && x.componente.trackComponenteID == x.id).Select(x => x.componente).Distinct().ToList();
            
            //var componentes = _context.tblM_CatComponente.Where(x => (string.IsNullOrEmpty(obj.noComponente) == true ? true : x.noComponente.Contains(obj.noComponente)) &&
            //    x.estatus == obj.estatus && (obj.conjuntoID == -1 ? true : x.conjuntoID == obj.conjuntoID)
            //              && (obj.subConjuntoID == -1 ? true : x.subConjuntoID == obj.subConjuntoID) && (obj.modeloEquipoID == -1 ? true : obj.modeloEquipoID == x.modeloEquipoID)
            //              && x.subConjunto.estatus == true).ToList();

            List<ComponenteDTO> result = new List<ComponenteDTO>();
            result = componentes.Select(z =>
            {
                var subconjunto = z.subConjunto.descripcion + " " + (z.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)z.posicionID).ToUpper() : "");
                var tracking = _context.tblM_trackComponentes.FirstOrDefault(x => x.id == z.trackComponenteID);
                return new ComponenteDTO
                {
                    id = z.id,
                    noComponente = z.noComponente,
                    centroCostos = z.centroCostos,
                    numParte = z.numParte,
                    costo = z.costo,
                    cicloVidaHoras = z.cicloVidaHoras,
                    horaCicloActual = z.horaCicloActual,
                    horasAcumuladas = z.horasAcumuladas,
                    conjuntoID = z.conjuntoID,
                    conjunto = z.conjunto.descripcion,
                    modeloEquipoID = z.modeloEquipoID,
                    grupoID = z.grupoID,
                    posicionID = z.posicionID,
                    subConjuntoID = z.subConjuntoID,
                    subConjunto = //z.componente.subConjunto.descripcion,
                        subconjunto,
                    marcaComponenteID = z.marcaComponenteID,
                    nombre_Corto = z.nombre_Corto,
                    estatus = z.estatus,
                    fecha = z.fecha,
                    proveedorID = z.proveedorID,
                    garantia = z.garantia,
                    locacionID = tracking == null ? 0 : tracking.locacionID ?? 0,
                    locacion = tracking == null ? "" : tracking.locacion,
                    vidaInicio = z.vidaInicio,
                    tipoLocacion = tracking == null ? 0 : tracking.estatus,
                    intercambio = z.intercambio,
                    ordenCompra = z.ordenCompra
                };
            }).ToList();
            return result;
        }
        public List<tblM_CatConjunto> FillCboConjuntos(bool estatus)
        {
            return _context.tblM_CatConjunto.Where(x => x.estatus == estatus).ToList(); ///item;
        }
        public List<tblM_CatSubConjunto> FillCboSubConjuntos(int idConjunto)
        {
            return _context.tblM_CatSubConjunto.Where(x => (idConjunto == -1 ? true : x.conjuntoID == idConjunto) && x.estatus == true && x.conjunto.estatus == true).OrderBy(x => x.descripcion).ToList(); ///item;
        }
        //public List<tblM_CatGrupoMaquinaria> FillCboGrupoMaquinaria()
        //{
        //    return _context.tblM_CatGrupoMaquinaria.Where(x => x.estatus == true && x.tipoEquipo.estatus==true).OrderBy(x=>x.descripcion).ToList(); ///item;
        //}
        public List<ComboDTO> FillCboGrupoMaquinaria()
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

        //public List<ComboDTO> FillCboGrupoMaquinaria()
        //{
        //    var gruposID = _context.tblM_CatComponente.Select(x => x.grupoID).Distinct();
        //    var grupos = _context.tblM_CatGrupoMaquinaria.Where(x => gruposID.Contains(x.id)).ToList();
        //    List<ComboDTO> data = new List<ComboDTO>();
        //    foreach (var item in grupos)
        //    {
        //        ComboDTO aux = new ComboDTO();
        //        aux.Value = item.id.ToString();
        //        aux.Text = item.descripcion;
        //        data.Add(aux);
        //    }
        //    return data;
        //}

        public List<tblM_CatModeloEquipo> FillCboModeloEquipo(int idGrupo)
        {
            var modelos = _context.tblM_CatModeloEquipotblM_CatSubConjunto.Select(x => x.modeloID).Distinct().ToList();            
            return _context.tblM_CatModeloEquipo.
                Where(x => (idGrupo == -1 ? true : x.idGrupo == idGrupo) && modelos.Contains(x.id) && x.estatus == true && x.marcaEquipo.estatus == true).OrderBy(x => x.descripcion).ToList();
        }
        public List<tblM_CatModeloEquipo> FillCboFiltroModeloEquipo()
        {
            return _context.tblM_CatModeloEquipo.Where(x => x.estatus == true && x.marcaEquipo.estatus == true).OrderBy(x => x.descripcion).ToList();
        }
        public List<ComboDTO> FillCboConjunto(int idModelo)
        {
            var conjuntos = _context.tblM_CatModeloEquipotblM_CatSubConjunto.Where(x => idModelo != -1 ? x.modeloID == idModelo : true).Select(x => new
            {
                Value = x.subconjunto.conjuntoID,
                Text = x.subconjunto.conjunto.descripcion,
                Prefijo = x.subconjunto.conjunto.prefijo
            }).GroupBy(x => x.Value, (key, g) => g.OrderByDescending(e => e.Value).FirstOrDefault()).ToList();
            List<ComboDTO> data = new List<ComboDTO>();
            foreach (var item in conjuntos)
            {
                ComboDTO aux = new ComboDTO();
                aux.Value = item.Value.ToString();
                aux.Text = item.Text;
                aux.Prefijo = item.Prefijo;
                data.Add(aux);
            }
            return data;
            
            //return _context.tblM_CatConjunto.Where(x => x.estatus == true ).ToList();
        }
        //public List<int> getSubConjuntoPosiciones(int idSubConjunto)
        //{
        //    return _context.tblM_CatSubConjunto.Where(x => x.id == idSubConjunto && x.estatus == true && x.conjunto.estatus == true).OrderBy(x => x.descripcion).Select(y => y.posicionID).ToList();
        //}

        public List<cboPrefijoModeloDTO> FillCboPrefijoModelo(int modelo)
        {
            List<cboPrefijoModeloDTO> data = new List<cboPrefijoModeloDTO>();
            var obj = _context.tblM_CatModeloEquipo.FirstOrDefault(y => y.id == modelo);
            if (obj != null) 
            {
                var aux = JsonConvert.DeserializeObject<List<string>>(obj.nomCorto);
                data = aux.Select(x => new cboPrefijoModeloDTO { prefijoText = x, prefijoValue = x, idModelo = modelo }).ToList(); ;
            }
            //data = _context.tblM_CatModeloEquipo.Where(y => y.id == modelo).Select(x => new cboPrefijoModeloDTO { prefijoText = x.nomCorto , prefijoValue=x.nomCorto , idModelo = x.id}).ToList();
            return data;
        }

        public List<tblM_CatLocacionesComponentes> FillCboLocaciones(int tipoBusqueda) 
        {
            return _context.tblM_CatLocacionesComponentes.Where(x => tipoBusqueda == 1 ? x.tipoLocacion == 1 : true).ToList();
        }
        public List<tblM_CatMaquina> FillCboLocacionesMaquina(int idModelo) 
        {
            return _context.tblM_CatMaquina.Where(x => x.modeloEquipoID == idModelo).ToList();
        }

        public void DeleteComponente(int idComponente) 
        {
            var componenteBorrar = _context.tblM_CatComponente.FirstOrDefault(x => x.id == idComponente);
            componenteBorrar.estatus = false;
            _context.SaveChanges();
        }

        public List<tblP_CC> FillCboCentroCostros() 
        {
            return _context.tblP_CC.Where(x => x.estatus == true).ToList();
        }

        public List<tblM_CatSubConjunto> FillCboSubConjuntos(List<int> idConjunto, int idModelo)
        {
            var subconjuntos = _context.tblM_CatModeloEquipotblM_CatSubConjunto.Where(x => (idModelo != -1 ? x.modeloID == idModelo : true) && idConjunto.Contains(x.subconjunto.conjuntoID) && x.subconjunto.estatus == true)
                .GroupBy(x => x.subconjuntoID, (key, g) => g.OrderByDescending(e => e.subconjuntoID).FirstOrDefault()).ToList();
            List<tblM_CatSubConjunto> data = new List<tblM_CatSubConjunto>();
            foreach (var item in subconjuntos)
            {
                tblM_CatSubConjunto aux = new tblM_CatSubConjunto();
                aux.descripcion = item.subconjunto.descripcion;
                aux.id = item.subconjunto.id;
                aux.prefijo = item.subconjunto.prefijo;
                aux.conjuntoID = item.subconjunto.conjuntoID;
                aux.estatus = item.subconjunto.estatus;
                aux.hasPosicion = item.subconjunto.hasPosicion;
                aux.posicionID = item.subconjunto.posicionID;
                aux.prefijo = item.subconjunto.prefijo;
                data.Add(aux);
            }
            return data;
        }

        public tblM_trackComponentes getLocacion(int idComponente) 
        {
            var tracking = _context.tblM_trackComponentes.Where(x => x.componenteID == idComponente).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).FirstOrDefault();
            //var locacion = _context.tblM_trackComponentes.Where(x => x.componenteID == idComponente).OrderByDescending(x => x.fecha).FirstOrDefault().locacionID;
            
            return tracking;
        }

        public string getLocacionDescripcion(int idComponente)
        {
            var data = "";
            var track = _context.tblM_trackComponentes.Where(x => x.componenteID == idComponente).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).FirstOrDefault();
            var loacion = track.locacionID;
            if (track.tipoLocacion == true) { data = _context.tblM_CatLocacionesComponentes.FirstOrDefault(x => x.id == track.locacionID).descripcion; }
            else { data = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == track.locacionID).noEconomico; }
            return data;
        }
        
        public List<tblM_CatMarcasComponentes> getMarcas() 
        {
            return _context.tblM_CatMarcasComponentes.Where(x => x.estatus == true).ToList();
        }

        public tblM_CatMarcasComponentes getMarcaComponenteByID(int id)
        {
            return _context.tblM_CatMarcasComponentes.FirstOrDefault(x => x.id == id && x.estatus == true);
        }

        public string getNumParte(int idModelo, int idSubconjunto)
        {
            
            var data = _context.tblM_CatModeloEquipotblM_CatSubConjunto.FirstOrDefault(x => x.modeloID == idModelo && x.subconjuntoID == idSubconjunto).numParte;
            return data;
        }
        public tblM_CatComponente getComponenteByID(int idComponente) 
        {
            return _context.tblM_CatComponente.FirstOrDefault(x => x.id == idComponente);
        }

        public List<tblM_CatComponente> getComponentesByIDs(List<int> arrComponentes)
        {
            return _context.tblM_CatComponente.Where(x => arrComponentes.Contains(x.id)).ToList();
        }

        public string getCCByID(int id) 
        {
            if (id == 0) { return "N/A"; }
            return _context.tblP_CC.FirstOrDefault(x => x.id == id && x.estatus == true).descripcion;
        }

        public void guardarModificaciones(int cicloVidaHoras, int garantia, int estatusNuevo, List<ComboDTO> cc, string descripcionComponente, string locacion, int subconjunto, bool estatusActual, int modelo) 
        {
            List<string> obraID = cc.Where(x => x.Prefijo == "0").Select(x => x.Value).ToList();
            var locacionIDs = GetMaquinaByListaCC(obraID);
            var almacenesYCRC = cc.Where(x => x.Prefijo == "1" || x.Prefijo == "2").Select(x => Int32.Parse(x.Value)).ToList();
            
            var trackActual = _context.tblM_trackComponentes.GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault()).Where(x => x.locacion.Contains(locacion));
            var trackActual1 = trackActual.Where(x => almacenesYCRC.Contains(x.locacionID ?? default(int)) && x.estatus > 0).ToList();
            var trackActual2 = trackActual.Where(x => locacionIDs.Contains(x.locacionID ?? default(int)) && x.estatus == 0).ToList();
            trackActual1.AddRange(trackActual2);
            List<int> componentesLocacion = new List<int>();
            if (trackActual.Count() > 0) 
            {
                componentesLocacion = trackActual1.Select(x => x.componenteID).ToList();
            }
            var componentes = _context.tblM_CatComponente.Where(x => x.noComponente.Contains(descripcionComponente) 
                && componentesLocacion.Contains(x.id) && (subconjunto == -1 ? true : x.subConjuntoID == subconjunto) 
                && x.estatus == estatusActual && (modelo == -1 ? true : x.modeloEquipoID == modelo)).ToList();
            if (cicloVidaHoras != -1) { componentes.ForEach(x => x.cicloVidaHoras = cicloVidaHoras); }
            if (garantia != -1) { componentes.ForEach(x => x.garantia = garantia); }
            if (estatusNuevo != -1) { componentes.ForEach(x => x.estatus = (estatusNuevo == 0 ? false : true )); }
            _context.SaveChanges();
        }


        public List<ComboDTO> FillCboPosicionesComponente(int idSubconjunto) 
        {
            var data = new List<ComboDTO>();
            var posiciones = _context.tblM_CatSubConjunto.FirstOrDefault(x => x.id == idSubconjunto).posicionID.Split(',');
            foreach (var posicion in posiciones) 
            {
                var aux = new ComboDTO();
                aux.Value = posicion;
                aux.Text = EnumHelper.GetDescription((PosicionesEnum)Int32.Parse(posicion));
                data.Add(aux);
            }
            return data;
        }
        private string getLocacionByID(int ? idLocacion, bool ? tipoLocacion)
        {
            var data = "N/A";
            if (tipoLocacion == true) { data = _context.tblM_CatLocacionesComponentes.FirstOrDefault(x => x.id == idLocacion).descripcion; }
            else { data = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == idLocacion).noEconomico; }            
            return data;
        }

        public bool ActualizarTracking(int idComponente, int idTracking) 
        {
            try
            {
                var componente = _context.tblM_CatComponente.FirstOrDefault(x => x.id == idComponente);
                componente.trackComponenteID = idTracking;
                Guardar(componente);
                return true;
            }
            catch (Exception e) {
                return false;
            }

        }

        public List<int> GetMaquinaByListaCC(List<string> obras)
        {
            var data = _context.tblM_CatMaquina.Where(x => obras.Contains(x.centro_costos)).Select(x => x.id).ToList();
            return data;
        }

    }
}
