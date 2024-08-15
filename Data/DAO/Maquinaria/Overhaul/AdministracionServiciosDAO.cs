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
using Core.DTO.Maquinaria.Catalogos;

namespace Data.DAO.Maquinaria.Overhaul
{
    public class AdministracionServiciosDAO : GenericDAO<tblM_CatServicioOverhaul>, IAdministracionServiciosDAO
    {
        public bool Guardar(tblM_CatServicioOverhaul obj) {

            obj.centroCostos = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == obj.maquinaID).centro_costos;
            var servicio = ExisteServicio(obj.maquinaID, obj.tipoServicioID);
            if (obj.id == 0)
            {
                if (servicio == null)
                {
                    SaveEntity(obj, (int)BitacoraEnum.SERVICIO_OVERHAUL);
                    tblM_trackServicioOverhaul track = new tblM_trackServicioOverhaul();
                    track.fecha = obj.fechaAsignacion;
                    track.horasCiclo = obj.horasCicloActual;
                    track.servicioID = obj.id;
                    track.target = obj.cicloVidaHoras;
                    track.tipoServicioID = obj.tipoServicioID;
                    _context.tblM_trackServicioOverhaul.Add(track);
                    _context.SaveChanges();
                }
                else {
                    if (servicio.estatus == false)
                    {
                        servicio.estatus = true;
                        servicio.fechaAsignacion = obj.fechaAsignacion;
                        servicio.centroCostos = obj.centroCostos;
                        servicio.cicloVidaHoras = obj.cicloVidaHoras;
                        servicio.horasCicloActual = obj.horasCicloActual;
                        tblM_trackServicioOverhaul track = new tblM_trackServicioOverhaul();
                        track.fecha = obj.fechaAsignacion;
                        track.horasCiclo = obj.horasCicloActual;
                        track.servicioID = obj.id;
                        track.target = obj.cicloVidaHoras;
                        track.tipoServicioID = obj.tipoServicioID;
                        _context.tblM_trackServicioOverhaul.Add(track);
                        _context.SaveChanges();
                    }
                    else { return false; }
                }
            }                    
            else
                Update(obj, obj.id, (int)BitacoraEnum.SERVICIO_OVERHAUL);
            return true;
        }

        private tblM_CatServicioOverhaul ExisteServicio(int maquinaID, int tipoServicioID)
        {
            tblM_CatServicioOverhaul data = _context.tblM_CatServicioOverhaul.FirstOrDefault(x => x.maquinaID == maquinaID && x.tipoServicioID == tipoServicioID);
            return data;
        }
        
        
        public bool GuardarTipo(tblM_CatTipoServicioOverhaul obj)
        {
            if (obj.id == 0)
            {
                if (!ExisteTipo(obj.nombre, obj.modeloMaquinaID))
                {
                    _context.tblM_CatTipoServicioOverhaul.Add(obj);
                    _context.SaveChanges();
                    SaveBitacora((int)BitacoraEnum.TIPO_SERVICIO_OVERHAUL, (int)AccionEnum.AGREGAR, (int)obj.id, JsonUtils.convertNetObjectToJson(obj));
                }
                else
                {
                    return false;
                }
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
            return true;
        }

        private bool ExisteTipo(string nombre, int modeloMaquinaID)
        {
            nombre = nombre.ToUpper();
            var data = _context.tblM_CatTipoServicioOverhaul.Where(x => x.nombre == nombre && x.modeloMaquinaID == modeloMaquinaID).ToList();
            if (data.Count > 0) { return true; }
            else { return false; }
        }

        public List<tblM_CatTipoServicioOverhaul> CargarTipoServicios(string nombre, bool estatus, int grupo, int modelo)
        {
            var data = _context.tblM_CatTipoServicioOverhaul.Where(x => x.nombre.Contains(nombre) && x.estatus == estatus && (grupo == -1 ? true : x.grupoMaquinaID == grupo) && (modelo == -1 ? true : x.modeloMaquinaID == modelo)).ToList();
            return data;
        }

        public string getModeloByID(int id)
        {
            var modelo = _context.tblM_CatModeloEquipo.FirstOrDefault(x => x.id == id);
            if (modelo != null) return _context.tblM_CatModeloEquipo.FirstOrDefault(x => x.id == id).descripcion;
            else return "--";
        }
        public List<tblM_CatMaquina> getMaquinasByModeloID(int modeloID) 
        {
            var maquinas = _context.tblM_CatMaquina.Where(x => x.modeloEquipoID == modeloID && x.estatus == 1).ToList();
            return maquinas;
        }

        public List<ServiciosOverhaulActivosDTO> CargarServiciosActivos(string economico, string servicioNombre, string cc, int grupoMaquina, int modeloMaquina, bool estatus) 
        {
            var data = _context.tblM_CatServicioOverhaul.Where(x => x.maquina.noEconomico.Contains(economico) && (cc == "" ? true : x.centroCostos == cc) && (grupoMaquina == -1 ? true : x.maquina.grupoMaquinariaID == grupoMaquina) && (modeloMaquina == -1 ? true : x.maquina.modeloEquipoID == modeloMaquina) && x.estatus == estatus).Join(_context.tblM_CatTipoServicioOverhaul.Where(x => x.nombre.Contains(servicioNombre) && (grupoMaquina == -1 ? true : x.grupoMaquinaID == grupoMaquina) && (modeloMaquina == -1 ? true : x.modeloMaquinaID == modeloMaquina)), x => x.tipoServicioID, y => y.id, (x, y) => new { x, y });
            
            List<ServiciosOverhaulActivosDTO> lista = new List<ServiciosOverhaulActivosDTO>();
            IQueryable<tblM_CatMaquina> maquinasBuscar;
            maquinasBuscar = _context.tblM_CatMaquina;

            var serviciosID = data.Select(x => x.x.id).ToList();
            var horometros = GetHorasCicloServicios(serviciosID, DateTime.Today);

            var join = maquinasBuscar.Join(data, (x => x.id), (y => y.x.maquinaID), ((x, y) => new { x, y }))
            .Select
            (z => new
            {
                id = z.x.id,
                maquina = z.x,
                cc = z.x.centro_costos,
                tipoServicio = z.y.y,
                servicio = z.y.x,
                fecha = z.y.x.fechaAsignacion,
            }).GroupBy(x => x.id).Select(servicio => servicio.ToList()).ToList();
            for (int i = 0; i < join.Count; i++)
            {
                ServiciosOverhaulActivosDTO aux = new ServiciosOverhaulActivosDTO();
                aux.id = join[i][0].id;
                aux.centroCostos = join[i][0].cc;
                aux.maquina = join[i][0].maquina;
                aux.fecha = join[i][0].fecha;
                aux.servicios = new List<Tuple<tblM_CatTipoServicioOverhaul, tblM_CatServicioOverhaul>>();
                for (int j = 0; j < join[i].Count; j++)
                {
                    var auxTipoServicio = join[i][j].tipoServicio;
                    var servicio = join[i][j].servicio;
                    var auxHorometro = horometros.FirstOrDefault(x => x.componenteID == servicio.id);
                    servicio.horasCicloActual = horometros == null ? 0 : auxHorometro.horometroActual;
                    Tuple<tblM_CatTipoServicioOverhaul, tblM_CatServicioOverhaul> auxServicio = new Tuple<tblM_CatTipoServicioOverhaul, tblM_CatServicioOverhaul>(auxTipoServicio, servicio);
                    join[i][j].servicio.maquina = null;
                    
                    aux.servicios.Add(auxServicio);
                }
                aux.vidaRestanteMaxima = aux.servicios.LastOrDefault().Item2.cicloVidaHoras - aux.servicios.LastOrDefault().Item2.horasCicloActual;
                lista.Add(aux);
            }
            return lista.OrderBy(x => x.maquina.noEconomico).OrderBy(x => x.vidaRestanteMaxima).ToList();
        }

        private List<horometrosComponentesDTO> GetHorasCicloServicios(List<int> serviciosID, DateTime fecha)
        {            
            try
            {
                List<horometrosComponentesDTO> data = new List<horometrosComponentesDTO>();
                var servicios = _context.tblM_CatServicioOverhaul.Where(x => serviciosID.Contains(x.id)).ToList();
                var trackingTotal = _context.tblM_trackServicioOverhaul.Where(x => serviciosID.Contains(x.servicioID) && x.fecha <= fecha).ToList();
                foreach (var servicioID in serviciosID) 
                {
                    var servicio = servicios.FirstOrDefault(x => x.id == servicioID);
                    if (servicio != null) 
                    {
                        horometrosComponentesDTO auxData = new horometrosComponentesDTO();
                        var trackingServicio = trackingTotal.Where(x => x.servicioID == servicioID).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).FirstOrDefault();
                        DateTime fechaUltimaAplicacion = trackingServicio == null ? servicio.fechaAsignacion : trackingServicio.fecha;
                        var horometro = _context.tblM_CapHorometro.Where(x => x.Fecha >= fechaUltimaAplicacion && x.Fecha <= fecha && x.Economico == servicio.maquina.noEconomico).ToList();
                        var suma = horometro.Count() > 0 ? horometro.Sum(x => x.HorasTrabajo) : 0;
                        auxData.componenteID = servicioID;
                        auxData.horometroActual = suma;
                        data.Add(auxData);
                    }
                }
                return data;
            }
            catch(Exception e)
            {
                return new List<horometrosComponentesDTO>();
            }            
        }

        public List<tblM_CatTipoServicioOverhaul> getServiciosOverhaul(string term)
        {
            List<tblM_CatTipoServicioOverhaul> data = new List<tblM_CatTipoServicioOverhaul>();
            data = _context.tblM_CatTipoServicioOverhaul.Where(z => z.nombre.Contains(term)).ToList();
            return data;
        }

        public List<Tuple<tblM_CatTipoServicioOverhaul, tblM_CatServicioOverhaul>> CargarModalServiciosActivos(int idMaquina, string servicioNombre)
        {
            var servicios = _context.tblM_CatServicioOverhaul.Where(x => x.maquinaID == idMaquina).Join(_context.tblM_CatTipoServicioOverhaul.Where(x => x.nombre.Contains(servicioNombre)), x => x.tipoServicioID, y => y.id, (x, y) => new { x, y });
            var serviciosIDs = servicios.Select(x => x.x.id).ToList();
            var horometros = GetHorasCicloServicios(serviciosIDs, DateTime.Today);
            List<Tuple<tblM_CatTipoServicioOverhaul, tblM_CatServicioOverhaul>> data = new List<Tuple<tblM_CatTipoServicioOverhaul, tblM_CatServicioOverhaul>>();
            foreach (var item in servicios)
            {
                var horometro = horometros.FirstOrDefault(x => x.componenteID == item.x.id);
                item.x.horasCicloActual = horometro == null ? 0 : horometro.horometroActual;
                Tuple<tblM_CatTipoServicioOverhaul, tblM_CatServicioOverhaul> aux = new Tuple<tblM_CatTipoServicioOverhaul, tblM_CatServicioOverhaul>(item.y, item.x);
                data.Add(aux);
            }
            return data;
        }

        public bool Aplicar(int id, int idMaquina, bool isPlaneacion, List<ModeloArchivoDTO> archivos, DateTime fecha) 
        {
            var servicio = _context.tblM_CatServicioOverhaul.FirstOrDefault(x => x.id == id);
            if (servicio != null)
            {
                var horasCicloAplicacion = servicio.horasCicloActual;
                servicio.fechaAplicacion = fecha;
                servicio.horasCicloActual = 0;
                if (isPlaneacion)
                {
                    var eventos = _context.tblM_CapPlaneacionOverhaul.Where(x => x.maquinaID == idMaquina && !x.terminado).ToList();
                    foreach (var item in eventos)
                    {
                        var arreglo = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(item.idComponentes);
                        //var componentes = arreglo.Select(x => x.Text).ToList();
                        var aplicarPlaneacion = arreglo.Where(x => x.Value == "0" && x.componenteID == id && x.Tipo == 1).ToList();
                        foreach (var plenacion in aplicarPlaneacion) 
                        {
                            plenacion.Value = "1";
                            plenacion.fechaRemocion = fecha.ToString("dd/MM/yyyy");
                            item.idComponentes = JsonConvert.SerializeObject(arreglo);
                        }
                        if (arreglo.All(x => x.Value == "1")) { item.terminado = true; }
                    }
                }
                tblM_trackServicioOverhaul tracking = new tblM_trackServicioOverhaul();
                tracking.servicioID = servicio.id;
                tracking.tipoServicioID = servicio.tipoServicioID;
                tracking.fecha = fecha;
                tracking.horasCiclo = horasCicloAplicacion;
                tracking.target = servicio.cicloVidaHoras;
                tracking.archivos = JsonConvert.SerializeObject(archivos);
                _context.tblM_trackServicioOverhaul.Add(tracking);
                _context.SaveChanges();
                return true;
            }
            else 
                return false;
        }

        public bool DeshabilitarServicioOverhaul(int idServicio) 
        {
            var servicio = _context.tblM_CatTipoServicioOverhaul.FirstOrDefault(x => x.id == idServicio);
            if (servicio != null)
            {
                try
                {
                    servicio.estatus = false;
                    _context.SaveChanges();
                    return true;
                }
                catch(Exception e) { return false; }
            }
            else 
                return false;
        }

        public bool Desasignar(int idServicio)
        {
            var servicio = _context.tblM_CatServicioOverhaul.FirstOrDefault(x => x.id == idServicio);
            if (servicio != null)
            {
                try
                {
                    servicio.estatus = false;
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception e) { return false; }
            }
            else
                return false;
        }

        public List<tblM_trackServicioOverhaul> CargarHistorialServiciosActivos(int idServicio) 
        {
            List<tblM_trackServicioOverhaul> data = new List<tblM_trackServicioOverhaul>();
            data = _context.tblM_trackServicioOverhaul.Where(x => x.servicioID == idServicio).ToList();
            return data;
        }

        public tblM_CatServicioOverhaul GetServicioByID(int index)
        {
            tblM_CatServicioOverhaul data = new tblM_CatServicioOverhaul();
            data = _context.tblM_CatServicioOverhaul.FirstOrDefault(x => x.id == index);
            return data;
        }

        public List<tblM_CatServicioOverhaul> GetServiciosByID(List<int> index)
        {
            List<tblM_CatServicioOverhaul> data = new List<tblM_CatServicioOverhaul>();
            data = _context.tblM_CatServicioOverhaul.Where(x => index.Contains(x.id)).ToList();
            return data;
        }
        
        public bool guardarModificacionesServicios(decimal cicloVidaHoras, int estatusNuevo, string economico, string servicioNombre, string cc, int grupoMaquina, int modeloMaquina, bool estatus)
        {
            try
            {
                var data = _context.tblM_CatServicioOverhaul.Where(x => x.maquina.noEconomico.Contains(economico) && (cc == "" ? true : x.centroCostos == cc) && (grupoMaquina == -1 ? true : x.maquina.grupoMaquinariaID == grupoMaquina) && (modeloMaquina == -1 ? true : x.maquina.modeloEquipoID == modeloMaquina) && x.estatus == estatus).ToList();

                if (cicloVidaHoras != -1)
                {
                    data.ForEach(x => x.cicloVidaHoras = cicloVidaHoras);
                }
                if (estatusNuevo != -1)
                {
                    data.ForEach(x => x.estatus = (estatusNuevo == 1));
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception e) { return false; }
        }

        public tblM_trackServicioOverhaul getTrackingByID(int trackID) 
        {
            tblM_trackServicioOverhaul data = _context.tblM_trackServicioOverhaul.FirstOrDefault(x => x.id == trackID);
            return data;
        }

        public bool ActualizarArchivoTrack(int trackID, ModeloArchivoDTO archivoNuevo)
        {
            try
            {
                tblM_trackServicioOverhaul tracking = getTrackingByID(trackID);
                List<ModeloArchivoDTO> lstArchivos = new List<ModeloArchivoDTO>();

                if (tracking.archivos != null && tracking.archivos != "" && tracking.archivos != "null")
                {
                    lstArchivos = JsonConvert.DeserializeObject<List<ModeloArchivoDTO>>(tracking.archivos);
                }
                else
                {
                    tracking.archivos = null;
                }

                if (archivoNuevo != null)
                {
                    lstArchivos.Add(archivoNuevo);
                    for (int i = 1; i < lstArchivos.Count(); i++)
                    {
                        lstArchivos[i].id = i;
                    }
                    tracking.archivos = JsonConvert.SerializeObject(lstArchivos);
                }

                _context.SaveChanges();
                return true;
            }
            catch (Exception e) 
            {
                return false;
            }
        }

        public List<ModeloArchivoDTO> CargarArchivosEvidencia(int id) 
        {
            List<ModeloArchivoDTO> archivos = new List<ModeloArchivoDTO>();
            var tracking = _context.tblM_trackServicioOverhaul.FirstOrDefault(x => x.id == id);
            if (tracking != null && tracking.archivos != null && tracking.archivos != "" && tracking.archivos != "null") 
            {
                archivos = JsonConvert.DeserializeObject<List<ModeloArchivoDTO>>(tracking.archivos);
            }
            return archivos;
        }
    }
}
