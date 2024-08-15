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
using Newtonsoft.Json;
using Core.Entity.Maquinaria;
using Core.Entity.Maquinaria.Captura;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using Core.DTO.Maquinaria.Overhaul;
using Core.DTO.Principal.Generales;
using Core.Enum.Maquinaria;
using Core.Enum;
using Core.DTO.Maquinaria.Captura;

namespace Data.DAO.Maquinaria.Overhaul
{
    public class PlaneacionOverhaulDAO : GenericDAO<tblM_CalendarioPlaneacionOverhaul>, IPlaneacionOverhaulDAO
    {
        public KPIDAO kpidao = new KPIDAO();
        public int GuardarCalendario(tblM_CalendarioPlaneacionOverhaul calendario, List<tblM_CapPlaneacionOverhaul> listaOverhauls) 
        {
            listaOverhauls = listaOverhauls.Where(x => x.fecha.Year == calendario.anio).ToList();
            var idCalendario = 0;
            int existe = ExisteCalendario(calendario);

            if (existe < 0)
            {
                switch (existe)
                {
                    case -3:
                        var updateEntradas = _context.tblM_CapPlaneacionOverhaul.Where(x => x.calendarioID == calendario.id).ToList();
                        List<tblM_CapPlaneacionOverhaul> agregarEntradas = new List<tblM_CapPlaneacionOverhaul>();
                        foreach (var item in listaOverhauls) 
                        {
                            if (item.id > 0) 
                            {
                                var coincidencia = updateEntradas.FirstOrDefault(x => x.id == item.id);
                                if (coincidencia != null) 
                                {
                                    coincidencia.idComponentes = item.idComponentes;
                                    coincidencia.fecha = item.fecha;
                                    coincidencia.tipo = item.tipo;
                                    coincidencia.indexCal = item.indexCal;
                                }
                            }
                            else
                            {
                                item.calendarioID = calendario.id;
                                agregarEntradas.Add(item);
                            }
                        }
                        foreach (var item in updateEntradas) 
                        {
                            var coincidencia = listaOverhauls.FirstOrDefault(x => x.id == item.id);
                            if (coincidencia == null && !item.terminado && item.estatus < 2 && item.tipo < 3) 
                            {
                                _context.tblM_CapPlaneacionOverhaul.Remove(item);
                            }
                        }
                        _context.tblM_CapPlaneacionOverhaul.AddRange(agregarEntradas);
                        _context.SaveChanges();
                        Update(calendario, calendario.id, (int)BitacoraEnum.CALENDARIO_OVERHAUL);
                        break;
                    case -2:
                    default:
                        idCalendario = 0;
                        break;
                }
            }
            else {
                if (calendario.id == 0)
                {
                    if (existe == 0) { 
                        SaveEntity(calendario, (int)BitacoraEnum.CALENDARIO_OVERHAUL);
                        listaOverhauls.ForEach(x => x.calendarioID = calendario.id);
                        _context.tblM_CapPlaneacionOverhaul.AddRange(listaOverhauls);
                        _context.SaveChanges();
                    }
                    else { idCalendario = existe; }
                }            
            }
            var calendarioGuardado = _context.tblM_CalendarioPlaneacionOverhaul.FirstOrDefault(x => x.nombre == calendario.nombre && x.grupoMaquinaID == calendario.grupoMaquinaID 
                && x.modeloMaquinaID == calendario.modeloMaquinaID && x.obraID == calendario.obraID && x.subConjuntoID == calendario.subConjuntoID && x.anio == calendario.anio && x.tipo == calendario.tipo);
            if (calendarioGuardado != null) { idCalendario = calendarioGuardado.id; }
            //if (idCalendario > 0)
            //{
            //    listaOverhauls = listaOverhauls.Where(x => x.fecha.Year == calendario.anio).ToList();
            //    foreach (var item in listaOverhauls)
            //    {
            //        item.calendarioID = idCalendario;
            //        _context.tblM_CapPlaneacionOverhaul.Add(item);
            //    }
            //    _context.SaveChanges();
            //}            
            return idCalendario;
        }

        public bool GuardarNuevoCalendario(tblM_CalendarioPlaneacionOverhaul calendario, List<tblM_CapPlaneacionOverhaul> listaOverhauls)
        {
            listaOverhauls = listaOverhauls.Where(x => x.fecha.Year == calendario.anio).ToList();
            int existe = ExisteCalendario(calendario);
            bool exito = false;

            if (existe == 0)
            {
                if (calendario.id == 0)
                {
                    if (existe == 0)
                    {
                        SaveEntity(calendario, (int)BitacoraEnum.CALENDARIO_OVERHAUL);
                        listaOverhauls.ForEach(x => x.calendarioID = calendario.id);
                        _context.tblM_CapPlaneacionOverhaul.AddRange(listaOverhauls);
                        _context.SaveChanges();
                        exito = true;
                    }
                }
            }        
            return exito;
        }

        private int ExisteCalendario(tblM_CalendarioPlaneacionOverhaul calendario) 
        {
            var calendariosMatch = _context.tblM_CalendarioPlaneacionOverhaul.Where(x => x.id == calendario.id).ToList();
            if (calendariosMatch.Count > 0) { return -3; }
            //calendariosMatch = _context.tblM_CalendarioPlaneacionOverhaul.Where(x => ).ToList();
            //if (calendariosMatch.Count > 0) { return -1; }
            calendariosMatch = _context.tblM_CalendarioPlaneacionOverhaul.Where(x => x.anio == calendario.anio && x.grupoMaquinaID == calendario.grupoMaquinaID && x.modeloMaquinaID == calendario.modeloMaquinaID && x.obraID == calendario.obraID && x.subConjuntoID == calendario.subConjuntoID && x.tipo == calendario.tipo).ToList();
            if (calendariosMatch.Count > 0) { return -2; }

            return 0;
        }

        public tblM_CapPlaneacionOverhaul getEventoByID(int id) 
        {
            var data = _context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.id == id);
            return data;
        }

        public List<tblM_CapPlaneacionOverhaul> getEventosOverhaul(int grupoMaquina, List<int> modeloMaquina, string obra, string subconjunto, int tipoSubConjunto)
        {
            var data = new List<tblM_CapPlaneacionOverhaul>();
            subconjunto = subconjunto.Trim();
            if (modeloMaquina == null) modeloMaquina = new List<int>();
            var trackActual = _context.tblM_trackComponentes.GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault());
            var maquinas = _context.tblM_CatMaquina.Where(x => x.estatus == 1 && (grupoMaquina == -1 ? true : x.grupoMaquinariaID == grupoMaquina) && (modeloMaquina.Count() == 0 ? true : modeloMaquina.Contains(x.modeloEquipoID)) && (obra == "" ? true : x.centro_costos == obra)).Join(trackActual, x => x.id, y => y.locacionID, (x, y) => new { x.id, x.noEconomico, y.fecha }).GroupBy(x => x.id).ToList();
            foreach (var item in maquinas) 
            {
                List<EventoPlaneacionOverhaulDTO> componentes = new List<EventoPlaneacionOverhaulDTO>();
                int j = 0;
                int i = 0;
                decimal hrsPromedio = CalculoHrsPromDiarioPub(item.Select(x => x.noEconomico).FirstOrDefault());
                var auxComponentes = trackActual.Where(x => x.locacionID == item.Key).Select(x => x.componente).Where(x => x.subConjunto.estatus == true).OrderBy(x => (x.cicloVidaHoras - x.horaCicloActual)).ToList();
                var componentesIDs = auxComponentes.Select(x => x.id).ToList();

                var horometros = GetHrsCicloActualComponentes(componentesIDs);
                
                componentes = auxComponentes
                .Select(x => new EventoPlaneacionOverhaulDTO
                {
                    cicloVidaHoras = x.cicloVidaHoras,
                    horaCicloActual = horometros.FirstOrDefault(y => y.componenteID == x.id).horometroActual,
                    //horaCicloActual = x.horaCicloActual,
                    id = x.id,
                    nombre = x.noComponente,
                    descripcion = x.subConjunto.descripcion,
                    tipo = x.subConjuntoID == 1 ? 2 : 0,
                    subConjuntoID = x.subConjuntoID ?? default (int),
                    subConjunto = x.subConjunto.descripcion.ToUpper(),
                    posicion = x.posicionID,
                    fechaRemocion = "--"
                }).ToList();
                componentes.AddRange(_context.tblM_CatServicioOverhaul.Where(x => x.maquinaID == item.Key && x.estatus == true).Join(_context.tblM_CatTipoServicioOverhaul.Where(z => z.planeacion == true), x => x.tipoServicioID, y => y.id, (x, y) => new { x, y }).OrderBy(x => (x.x.cicloVidaHoras - x.x.horasCicloActual)).Select(w => new EventoPlaneacionOverhaulDTO
                {
                    cicloVidaHoras = w.x.cicloVidaHoras,
                    horaCicloActual = w.x.horasCicloActual,
                    id = w.x.id,
                    nombre = w.y.nombre,
                    descripcion = w.y.descripcion,
                    tipo = 1,
                    subConjuntoID = w.y.id,
                    subConjunto = w.y.descripcion.ToUpper(),
                    fechaRemocion = "--"
                }));
                componentes = componentes.OrderBy(x => (x.cicloVidaHoras - x.horaCicloActual)).ToList();
                while (j < componentes.Count)
                {
                    var tipoOverhaul = 2;
                    var primerComponente = componentes[j];
                    tblM_CapPlaneacionOverhaul aux = new tblM_CapPlaneacionOverhaul();
                    List<ComponentePlaneacionDTO> comp = new List<ComponentePlaneacionDTO>();
                    var resta = (primerComponente.cicloVidaHoras + 1000 - primerComponente.horaCicloActual);
                    
                    aux.maquinaID = item.Key;
                    aux.ritmo = hrsPromedio;
                    
                    while ((j < componentes.Count) && ((componentes[j].cicloVidaHoras - componentes[j].horaCicloActual) < resta))
                    {
                        if (subconjunto == "")
                        {
                            ComponentePlaneacionDTO comboComp = new ComponentePlaneacionDTO();
                            comboComp.tipoOverhaul = 2;
                            if (componentes[j].subConjuntoID == 1) {                                 
                                tipoOverhaul = 1;
                                comboComp.tipoOverhaul = 1;
                            }
                            if (componentes[j].tipo == 1) { 
                                tipoOverhaul = 0;
                                comboComp.tipoOverhaul = 0;
                            }
                            comboComp.Value = "0";
                            comboComp.componenteID = componentes[j].id;
                            comboComp.nombre = componentes[j].nombre;
                            comboComp.descripcion = componentes[j].descripcion;
                            comboComp.posicion = componentes[j].posicion > 0 ? EnumHelper.GetDescription((PosicionesEnum)componentes[j].posicion) : "";
                            comboComp.Tipo = componentes[j].tipo;
                            comboComp.horasCiclo = componentes[j].horaCicloActual;
                            comboComp.target = componentes[j].cicloVidaHoras;
                            comboComp.fechaRemocion = "--";
                            comp.Add(comboComp);                                    
                        }
                        else 
                        {
                            if (componentes[j].subConjunto == subconjunto) 
                            {
                                ComponentePlaneacionDTO comboComp = new ComponentePlaneacionDTO();
                                comboComp.tipoOverhaul = 2;
                                if (componentes[j].subConjuntoID == 1)
                                {
                                    tipoOverhaul = 1;
                                    comboComp.tipoOverhaul = 1;
                                }
                                if (componentes[j].tipo == 1)
                                {
                                    tipoOverhaul = 0;
                                    comboComp.tipoOverhaul = 0;
                                }
                                comboComp.Value = "0";
                                comboComp.componenteID = componentes[j].id;
                                comboComp.nombre = componentes[j].nombre;
                                comboComp.descripcion = componentes[j].descripcion;
                                comboComp.posicion = componentes[j].posicion > 0 ? EnumHelper.GetDescription((PosicionesEnum)componentes[j].posicion) : "";
                                comboComp.Tipo = componentes[j].tipo;
                                comboComp.horasCiclo = componentes[j].horaCicloActual;
                                comboComp.target = componentes[j].cicloVidaHoras;
                                comboComp.fechaRemocion = "--";
                                comp.Add(comboComp);                                        
                            }
                        }
                        j++;
                    }
                    if(comp.Count() > 0)
                    {
                        aux.id = 0;
                        var ultimoComponente = comp.OrderBy(x => (x.target - x.horasCiclo)).LastOrDefault();
                        resta = (primerComponente.cicloVidaHoras + ultimoComponente.target - primerComponente.horaCicloActual - ultimoComponente.horasCiclo) / 2;
                        int dias = (int)(Math.Ceiling(resta / hrsPromedio));
                        aux.fecha = DateTime.Today.AddDays(dias);
                        aux.indexCal = tipoSubConjunto.ToString() + aux.fecha.ToString("yyyyMMdd") + item.Key.ToString();
                        comp = comp.Distinct().ToList();
                        aux.idComponentes = JsonConvert.SerializeObject(comp);

                        aux.tipo = tipoOverhaul;
                        data.Add(aux);
                        i++;
                    }
                }
                
            }
            List<tblM_CapPlaneacionOverhaul> eliminar = new List<tblM_CapPlaneacionOverhaul>();
            foreach (var item in data)
            {
                if (item.fecha < DateTime.Today)
                {
                    item.fecha = DateTime.Today;
                }
            }
            foreach (var item in data) 
            {
                if (!eliminar.Contains(item))
                {
                    var repeated = data.Where(x => x.fecha == item.fecha && item.maquinaID == x.maquinaID && x.idComponentes != item.idComponentes).ToList();
                    if (repeated.Count() > 0)
                    {
                        foreach (var item2 in repeated)
                        {
                            var componentesEliminado = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(item2.idComponentes);
                            var componentesAgregado = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(item.idComponentes);
                            componentesAgregado.AddRange(componentesEliminado);
                            componentesAgregado = componentesAgregado.Distinct().ToList();
                            item.idComponentes = JsonConvert.SerializeObject(componentesAgregado);

                            if (item.tipo == 1)
                            {
                                if (item2.tipo == 0)
                                    item.tipo = 0;
                            }
                            if (item.tipo == 2) { item.tipo = item2.tipo; }
                            eliminar.Add(item2);
                        }
                    }
                }
            }
            data = data.Except(eliminar).ToList();
            return data;
        }

        //public List<string> getComponentes(List<ComponentePlaneacionDTO> arrComponentes)
        //{
        //    List<string> data = new List<string>();
        //    foreach (var item in arrComponentes)
        //    {                
        //        if (!item.Prefijo.Contains("servicio-"))
        //        {
        //            var componente = _context.tblM_CatComponente.FirstOrDefault(x => x.id == item.componenteID);
        //            var auxComponente = componente.noComponente + ": " + componente.subConjunto.descripcion + " " + (componente.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)componente.posicionID).ToUpper() : "");
        //            data.Add(auxComponente);
        //        }
        //        else
        //        {
        //            var servicio = _context.tblM_CatServicioOverhaul.FirstOrDefault(x => x.id == item.componenteID);
        //            int indexString = item.Prefijo.IndexOf("servicio-") + 9;
        //            var auxServicio = item.Prefijo.Substring(indexString);
        //            data.Add(auxServicio);
        //        }                
        //    }
        //    return data;
        //}

        public decimal CalculoHrsPromDiarioPub(string economico)
        {
            decimal result = 1;            
            result = _context.tblM_CapRitmoHorometro.Where(y => y.economico == economico).Select(X => X.horasDiarias).FirstOrDefault();
            if (result == 0) { result = (_context.tblM_CapHorometro.Where(y => y.Economico == economico).GroupBy(x => x.Fecha).OrderByDescending(r => r.Key).Take(20).ToArray().Sum(y => y.Sum(z => z.HorasTrabajo)) / 20); }
            return result;
        }

        public List<tblM_CalendarioPlaneacionOverhaul> CargarCalendariosGuardados() 
        {
            return _context.tblM_CalendarioPlaneacionOverhaul.Where(x => x.grupoMaquinaID == 0 && x.modeloMaquinaID == 0 && x.subConjuntoID == null && x.estatus == 0).ToList();
        }

        public List<tblM_CalendarioPlaneacionOverhaul> CargarCalendariosTaller()
        {
            return _context.tblM_CalendarioPlaneacionOverhaul.Where(x => x.grupoMaquinaID == 0 && x.modeloMaquinaID == 0 && x.subConjuntoID == null).ToList();
        }

        public List<tblM_CalendarioPlaneacionOverhaul> CargarCalendarios(int anio)
        {
            return _context.tblM_CalendarioPlaneacionOverhaul.Where(x => x.estatus == 0 && x.anio == anio).ToList();
        }

        public List<tblM_CapPlaneacionOverhaul> getEventosOverhaulGuardado(int idCalendario) 
        {
            var data = _context.tblM_CapPlaneacionOverhaul.Where(x => x.calendarioID == idCalendario && x.tipo < 3 && (x.terminado ? x.estatus > 0 : true)).ToList();
            foreach (var item in data) 
            {
                var componentes = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(item.idComponentes);
                var componentesIDs = componentes.Select(x => x.componenteID).ToList();
                var horometros = GetHrsCicloActualComponentes(componentesIDs);
                foreach (var item2 in componentes) 
                {
                    if (item2.fechaRemocion == null) item2.fechaRemocion = "--";
                    if (item2.Value == "0")
                    {
                        if (item2.Tipo == 1) {
                            var servicio = _context.tblM_CatServicioOverhaul.FirstOrDefault(x => x.id == item2.componenteID);
                            item2.horasCiclo = servicio.horasCicloActual;
                            item2.target = servicio.cicloVidaHoras;
                            
                        }
                        else {
                            var componente = _context.tblM_CatComponente.FirstOrDefault(x => x.id == item2.componenteID);
                            item2.horasCiclo = horometros.FirstOrDefault(x => x.componenteID == item2.componenteID).horometroActual;
                            //item2.horasCiclo = componente.horaCicloActual;
                            item2.target = componente.cicloVidaHoras;
                        }
                    }
                }
                item.idComponentes = JsonConvert.SerializeObject(componentes);
            }
            return data;
        }

        public tblM_CalendarioPlaneacionOverhaul getCalendarioByID(int idCalendario) 
        {
            return _context.tblM_CalendarioPlaneacionOverhaul.FirstOrDefault(x => x.id == idCalendario);
        }

        public List<tblM_CalendarioPlaneacionOverhaul> getCalendarioByObra(string obra, int anio)
        {
            return _context.tblM_CalendarioPlaneacionOverhaul.Where(x => (obra == "" ? x.obraID != null : x.obraID == obra) && x.anio == anio && x.modeloMaquinaID == 0 && x.subConjuntoID == null).ToList();
        }

        public void ActualizarComponenteRemovido(int idMaquina, int idComponente, DateTime fecha) 
        {
            try
            {
                var eventos = _context.tblM_CapPlaneacionOverhaul.Where(x => x.maquinaID == idMaquina && x.tipo < 3).ToList();
                var componente = _context.tblM_CatComponente.FirstOrDefault(x => x.id == idComponente);
                int anio = fecha.Year;
                var maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == idMaquina);
                string obra = "";
                if (maquina != null)
                    obra = maquina.centro_costos;
                foreach (var item in eventos)
                {
                    var arreglo = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(item.idComponentes);
                    var componentePlaneacion = arreglo.FirstOrDefault(x => x.componenteID == idComponente);
                    //var componentes = arreglo.Select(x => x.componenteID).ToList();
                    if (componentePlaneacion != null)
                    {
                        //checar si las fechas coinciden
                        var index = arreglo.IndexOf(componentePlaneacion);
                        var tipoOverhaul = item.tipo;
                        var tipo = arreglo[index].Tipo;
                        //if (arreglo[index].tipoOverhaul < 3)
                        //{
                            if (item.fecha.Year == anio)
                            {
                                //if (/*item.fecha.Month == fecha.Month && */!componentePlaneacion.falla)
                                //{
                                    arreglo[index].Value = "1";
                                    arreglo[index].horasCiclo = componente.horaCicloActual;
                                    arreglo[index].target = componente.cicloVidaHoras;
                                    arreglo[index].fechaRemocion = fecha.ToString("dd/MM/yyyy");
                                    var componentesPendientes = arreglo.Where(x => x.Value == "0");
                                    if (componentesPendientes.Count() == 0) { item.terminado = true; }
                                    item.idComponentes = JsonConvert.SerializeObject(arreglo);
                                //}
                                //else
                                //{
                                //    ComponentePlaneacionDTO componenteRemovido = new ComponentePlaneacionDTO();
                                //    componenteRemovido.Value = "1";
                                //    componenteRemovido.componenteID = arreglo[index].componenteID;
                                //    componenteRemovido.nombre = arreglo[index].nombre;
                                //    componenteRemovido.descripcion = arreglo[index].descripcion;
                                //    componenteRemovido.posicion = arreglo[index].posicion;
                                //    componenteRemovido.horasCiclo = componente.horaCicloActual;
                                //    componenteRemovido.target = componente.cicloVidaHoras;
                                //    componenteRemovido.Tipo = arreglo[index].Tipo;
                                //    componenteRemovido.tipoOverhaul = arreglo[index].tipoOverhaul;
                                //    List<ComponentePlaneacionDTO> nuevoComponentes = new List<ComponentePlaneacionDTO>() { componenteRemovido };
                                //    //eliminar componente de evento guardado
                                //    arreglo.Remove(arreglo[index]);
                                //    //guardar
                                //    tblM_CapPlaneacionOverhaul eventoGuardado = new tblM_CapPlaneacionOverhaul();
                                //    eventoGuardado.id = 0;
                                //    eventoGuardado.idComponentes = JsonConvert.SerializeObject(nuevoComponentes);
                                //    eventoGuardado.maquinaID = item.maquinaID;
                                //    eventoGuardado.fecha = fecha;
                                //    eventoGuardado.tipo = item.tipo;
                                //    eventoGuardado.estatus = 7;
                                //    eventoGuardado.calendarioID = item.calendarioID;
                                //    eventoGuardado.indexCal = "7" + fecha.Year + fecha.Month + fecha.Day + idMaquina;
                                //    //eventoGuardado.actividades = item.actividades;
                                //    //eventoGuardado.fechaInicio = item.fechaInicio;
                                //    //eventoGuardado.fechaFin = item.fechaFin;
                                //    //eventoGuardado.diasDuracionP = item.diasDuracionP;
                                //    //eventoGuardado.diasTrabajados = item.diasTrabajados;
                                //    //eventoGuardado.fechaFinP = item.fechaFinP;
                                //    eventoGuardado.ritmo = item.ritmo;
                                //    eventoGuardado.terminado = true;
                                //    item.idComponentes = JsonConvert.SerializeObject(arreglo);
                                //    _context.tblM_CapPlaneacionOverhaul.Add(eventoGuardado);
                                //}
                            }
                            else
                            {
                                arreglo.Remove(arreglo[index]);
                                if (arreglo.Count() == 0) {
                                    //eliminar evento de calendario
                                    _context.tblM_CapPlaneacionOverhaul.Remove(item);
                                }
                                else {
                                    if (tipoOverhaul < 2) {
                                        if (tipoOverhaul == 1 && tipo == 2) {
                                            arreglo.ForEach(x => x.tipoOverhaul = 2);
                                            item.tipo = 2;
                                        }
                                        if (tipoOverhaul == 0 && tipo == 1) {
                                            var motores = arreglo.Where(x => x.tipoOverhaul == 2);
                                            if (motores.Count() > 0) {
                                                arreglo.ForEach(x => x.tipoOverhaul = 1);
                                                item.tipo = 1;
                                            }
                                            else {
                                                arreglo.ForEach(x => x.tipoOverhaul = 2);
                                                item.tipo = 2;
                                            }
                                        }
                                    }
                                    item.idComponentes = JsonConvert.SerializeObject(arreglo);
                                }
                            }
                        //}
                    }
                    _context.SaveChanges();
                }
            }
            catch (Exception e) 
            {

            }
        }

        //public string CargarActividadesOverhaul(string idEvento) 
        //{
        //    List<tblM_CatActividadOverhaul> data = new List<tblM_CatActividadOverhaul>();
        //    var planeacion = _context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.indexCal == idEvento);
        //    if (planeacion.tipo != 0) 
        //    {
        //        return "-1";
        //    }

        //    if (planeacion.actividades != null) 
        //    {
        //        var lstActividades = JsonConvert.DeserializeObject<List<ActividadOverhaulDTO>>(planeacion.actividades).Select(x => x.id);
        //        data = _context.tblM_CatActividadOverhaul.Where(x => lstActividades.Contains(x.id.ToString())).ToList();
        //    }
        //    return JsonConvert.SerializeObject(data);
        //}

        //public List<tblM_CatActividadOverhaul> CargarDatosDiagramaGantt(string idEvento) 
        //{
        //    List<tblM_CatActividadOverhaul> data = new List<tblM_CatActividadOverhaul>();
        //    var evento = _context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.indexCal == idEvento);
            
        //    if (evento != null) 
        //    {
        //        var actividadesID = evento.actividades == null ? new List<string>() : JsonConvert.DeserializeObject<List<ActividadOverhaulDTO>>(evento.actividades).Select(x => x.id).ToList();
        //        var modelo = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == evento.maquinaID).modeloEquipoID;
        //        data = _context.tblM_CatActividadOverhaul.Where(x => x.modeloID == modelo && (actividadesID.Count > 0 ? actividadesID.Contains((x.id).ToString()) : true)).ToList();
        //    }
        //    return data;
        //}

        //public bool GuardarDiagramaGantt(string idEvento, string actividadesID) 
        //{
        //    var evento = _context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.indexCal == idEvento);
        //    if (evento != null) 
        //    {
        //        evento.actividades = actividadesID;
        //        _context.SaveChanges();
        //        return true;
        //    }
        //    return false; ;
        //}

        //public bool IniciarActividadesOverhaul(string idEvento) 
        //{
        //    var evento = _context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.indexCal == idEvento);
        //    if (evento != null) 
        //    {
        //        evento.estatus = 1;
        //        evento.fechaInicio = DateTime.Now;
        //        _context.SaveChanges();
        //        return true;
        //    }
        //    return false; ;
        //}


        ///////Reportes

        public List<tblM_CapPlaneacionOverhaul> CargarTblInversion(int calendarioID)
        {
            var calendarios = _context.tblM_CapPlaneacionOverhaul.Where(x => x.calendarioID == calendarioID && x.tipo < 4 && (x.terminado ? x.estatus > 0 : true)).ToList();
            return calendarios;
        }

        public List<tblM_CalendarioPlaneacionOverhaul> CargarCalendariosEjec (string obra, int anio)
        {
            var calendarios = _context.tblM_CalendarioPlaneacionOverhaul.Where(x => x.anio == anio && x.obraID == obra && x.grupoMaquinaID == 0 && x.modeloMaquinaID == 0 && x.subConjuntoID == null && x.estatus == 0).ToList();

            return calendarios;
        }

        public List<ReporteKPIDTO> GetReporteDisponibilidad(List<string> cc, int anio)
        {
            List<ReporteKPIDTO> data = new List<ReporteKPIDTO>();
            var overhauls = _context.tblM_CapPlaneacionOverhaul.Where(x => cc.Contains(x.calendario.obraID) && (x.fechaInicio.Value.Year == anio || x.fechaFin.Value.Year == anio));
            kpiInfoGeneralDTO[] lista = new kpiInfoGeneralDTO[12];
            foreach (var item in overhauls)
            {
                ReporteKPIDTO aux = new ReporteKPIDTO();
                for (int i = 0; i < 12; i++)
                {
                    lista[i] = kpidao.getInfoGeneral(item.maquinaID, cc, new DateTime(anio, (i + 1), 1), new DateTime(anio, (i + 1), DateTime.DaysInMonth(anio, (i + 1))));
                }
                aux.idMaquina = item.maquinaID;
                aux.disponibilidad = lista;
                aux.fechaInicio = item.fechaInicio ?? default(DateTime);
                aux.fechaFin = item.fechaFin ?? default(DateTime);
                data.Add(aux);
            }
            return data;
        }

        public List<rptCalendarioEjecutadoDTO> GetReporteCalenEjecOverhaul(string cc, int anio)
        {
            List<rptCalendarioEjecutadoDTO> data = new List<rptCalendarioEjecutadoDTO>();
            var overhauls = _context.tblM_CapPlaneacionOverhaul.Where(x => x.calendario.obraID == cc && (x.fechaInicio.Value.Year == anio || x.fechaFin.Value.Year == anio)).ToList();
            foreach (var item in overhauls) {
                List<ComboDTO> componentes = JsonConvert.DeserializeObject<List<ComboDTO>>(item.idComponentes);
                var equipo = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == item.maquinaID);
                foreach (var item2 in componentes) 
                {
                    int index = 0;
                    Int32.TryParse(item2.Text, out index);
                    var componente = _context.tblM_CatComponente.FirstOrDefault(x => x.id == index);
                    var maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == item.maquinaID);
                    if(componente != null)
                    {
                        rptCalendarioEjecutadoDTO aux = new rptCalendarioEjecutadoDTO();
                        aux.equipo = equipo.noEconomico;
                        aux.ritmo = CalculoHrsPromDiarioPub(maquina != null ? maquina.noEconomico : "");
                        aux.componente = componente.subConjunto.descripcion;
                        aux.hrsComponente = componente.horaCicloActual;
                        aux.target = componente.cicloVidaHoras;
                        aux.proximoPCR = item.fecha;
                        switch(item.tipo)
                        {
                            case 0:
                                aux.resumen = "OVERHAUL GENERAL";
                                break;
                            case 1:
                                aux.resumen = "CAMBIO DE MOTOR";
                                break;
                            case 2:
                                aux.resumen = "COMPONENTES DESFASADOS";
                                break;
                            default:
                                aux.resumen = "";
                                break;
                        }
                        aux.fechaOverhaul = item.fecha;
                        data.Add(aux);
                    }
                }
            }

            return data;
        }

        public List<tblM_CapPlaneacionOverhaul> GetReportePrecisionOverhaul(DateTime fechaInicio, DateTime fechaFin, int tipo)
        {
            var calendarios = _context.tblM_CalendarioPlaneacionOverhaul.Where(x => x.grupoMaquinaID == 0 && x.modeloMaquinaID == 0 && x.subConjuntoID == null && x.estatus == 0 && (x.anio >= fechaInicio.Year && x.anio <= fechaFin.Year)).Select(x => x.id).ToList();
            var eventos = _context.tblM_CapPlaneacionOverhaul.Where(x => calendarios.Contains(x.calendarioID) && (tipo == -1 ? true : x.tipo == tipo) && x.estatus > 2).ToList();
            return eventos;
        }

        public List<horometrosComponentesDTO> GetHrsCicloActualComponentes(List<int> componentesIDs)
        {
            List<horometrosComponentesDTO> data = new List<horometrosComponentesDTO>();
            DateTime fechaMinima = DateTime.Now;
            DateTime fechaInicioHorometros = new DateTime(2011, 3, 28);
            bool banderaReciclado = false;
            bool banderaEntraCiclo = false;

            var componentesHorasInicio = _context.tblM_CatComponente.Where(x => componentesIDs.Contains(x.id)).Select(x => new { id = x.id, horasInicio = x.horasCicloInicio }).ToList();

            var tracking = _context.tblM_trackComponentes.Where(x => componentesIDs.Contains(x.componenteID)).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).Select(x => new TrackingHistorialDTO
            {
                componenteID = x.componenteID,
                fecha = x.fecha ?? default(DateTime),
                locacion = x.locacion,
                reciclado = x.reciclado,
                tipoLocacion = x.tipoLocacion ?? false,
                horasHistorial = x.horasCiclo
            }).ToList();

            var trackingGrouped = _context.tblM_trackComponentes.Where(x => componentesIDs.Contains(x.componenteID)).Select(x => new TrackingHistorialDTO
            {
                componenteID = x.componenteID,
                fecha = x.fecha ?? default(DateTime),
                locacion = x.locacion,
                reciclado = x.reciclado,
                tipoLocacion = x.tipoLocacion ?? false,
                id = x.id
            }).GroupBy(x => x.componenteID).ToList();

            List<string> maquinas = new List<string>();
            foreach (var item in trackingGrouped)
            {
                foreach (var item2 in item.OrderByDescending(x => x.fecha).ThenByDescending(x => x.id))
                {
                    if (item2.tipoLocacion == false)
                    {
                        if (item2.fecha < fechaMinima) fechaMinima = item2.fecha;
                        maquinas.Add(item2.locacion);
                    }
                    if (item2.reciclado) break;
                }
            }
            maquinas = maquinas.Distinct().ToList();

            var horometros = _context.tblM_CapHorometro.GroupJoin(maquinas, x => x.Economico, economico => economico, (x, economico) => new { x, economico })
                .Where(e => e.economico.Any() /*&& e.x.Fecha >= fechaMinima*/).OrderByDescending(e => e.x.Fecha).ThenByDescending(e => e.x.turno)
                .Select(e => new HorasTrackingDTO
                {
                    Fecha = e.x.Fecha,
                    Economico = e.x.Economico,
                    HorometroAcumulado = e.x.HorometroAcumulado,
                    HorasTrabajo = e.x.HorasTrabajo,
                    turno = e.x.turno
                }).GroupBy(x => x.Economico).ToList();

            data = componentesIDs.Select(x =>
            {
                DateTime fechaFin = DateTime.Now;
                DateTime fechaInicio = DateTime.Now;
                var auxTracking = tracking.Where(y => y.componenteID == x).ToList();
                var auxMaquinasTrack = auxTracking.Select(y => y.locacion).ToList();
                decimal horometroActual = 0;
                if (auxTracking.Count() > 0)
                {
                    var lstHorometros = horometros.Where(y => auxMaquinasTrack.Contains(y.Key)).SelectMany(y => y).OrderByDescending(y => y.Fecha).ThenByDescending(y => y.turno).ToList();
                    var componente = _context.tblM_CatComponente.FirstOrDefault(y => y.id == x);
                    decimal horaAnterior = componente.horaCicloActual;
                    decimal horasInicio = 0;
                    decimal horasFinal = 0;
                    banderaReciclado = false;
                    banderaEntraCiclo = false;
                    foreach (var item in auxTracking)
                    {
                        fechaInicio = item.fecha;
                        if (item.tipoLocacion == false)
                        {
                            if (/*fechaInicio < fechaInicioHorometros ||*/ fechaFin < fechaInicioHorometros)
                            {
                                horometroActual += horaAnterior;
                                //if (fechaFin < fechaInicioHorometros) break;
                            }
                            else
                            {
                                var horometroInicial = lstHorometros.LastOrDefault(y => y.Economico == item.locacion && y.Fecha >= fechaInicio && y.Fecha <= fechaFin);
                                horasInicio = horometroInicial == null ? 0 : horometroInicial.HorometroAcumulado - horometroInicial.HorasTrabajo;
                                var horometroFinal = lstHorometros.FirstOrDefault(y => y.Economico == item.locacion && y.Fecha <= fechaFin && y.Fecha >= fechaInicio && y.HorometroAcumulado >= horasInicio);
                                horasFinal = horometroFinal == null ? 0 : horometroFinal.HorometroAcumulado;
                                decimal restaHorometros = horasFinal - horasInicio;
                                horometroActual += restaHorometros;
                                horasFinal = horasInicio;
                                banderaEntraCiclo = true;
                                if (fechaInicio < fechaInicioHorometros && horasInicio != horaAnterior) horometroActual += horasInicio;
                            }
                        }
                        if (banderaEntraCiclo) horaAnterior = item.horasHistorial;
                        fechaFin = fechaInicio;
                        if (item.reciclado)
                        {
                            banderaReciclado = true;
                            break;
                        }
                    }
                }
                if (!banderaReciclado)
                {
                    var auxComponente = componentesHorasInicio.FirstOrDefault(y => y.id == x);
                    if (auxComponente != null) horometroActual += auxComponente.horasInicio;
                }
                return new horometrosComponentesDTO
                {
                    componenteID = x,
                    horometroActual = horometroActual
                };
            }).ToList();
            return data;
        }
        
    }
}
