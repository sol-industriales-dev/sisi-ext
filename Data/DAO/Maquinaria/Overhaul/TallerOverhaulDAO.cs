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
using Core.Enum.Principal;

namespace Data.DAO.Maquinaria.Overhaul
{
    public class TallerOverhaulDAO : GenericDAO<tblM_CapPlaneacionOverhaul>, ITallerOverhaulDAO
    {
        
        
        public List<tblM_CapPlaneacionOverhaul> CargarGridTallerEstatus(int idCalendario, int estatus, int tipo) 
        {
            List<tblM_CapPlaneacionOverhaul> data = new List<tblM_CapPlaneacionOverhaul>();            
            var calendario = _context.tblM_CalendarioPlaneacionOverhaul.FirstOrDefault(x => x.id == idCalendario);
            if(calendario != null)
                data = _context.tblM_CapPlaneacionOverhaul.Where(x => x.calendarioID == idCalendario && x.estatus < 7 && (estatus == -1 ? true : (estatus == 1 ? x.estatus == 2 : (estatus == 0 ? x.estatus < 2 : x.estatus > 2))) && (tipo == -1 ? true : (tipo == 0 ? x.tipo < 3 : x.tipo == 3))).ToList();
            return data;
        }

        public string getCCByMaquinaID(int id) 
        {
            var maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == id);
            if (maquina != null)
            {
                var cc = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == maquina.centro_costos);
                if (cc != null)
                    return cc.descripcion;
                else
                    return maquina.centro_costos;
            }
            else 
                return "N/A";            
        }

        public int getModeloIDByMaquinaID(int id)
        {
            var maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == id);
            if (maquina != null)
            {
                return maquina.modeloEquipoID;
            }
            else
                return -1;
        }

        public string getEconomicoByID(int id) 
        {
            var maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == id);
            if (maquina != null)
            {
                return maquina.noEconomico;
            }
            else 
                return "N/A";            
        }

        public List<ActividadOverhaulDTO> CargarGridModalTallerEstatus(int index) 
        {
            List<ActividadOverhaulDTO> data = new List<ActividadOverhaulDTO>();
            var evento = _context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.id == index);
            if (evento != null) 
            {
                var actividades = evento.actividades;
                if (actividades != "")
                    return JsonConvert.DeserializeObject<List<ActividadOverhaulDTO>>(evento.actividades);
            }
            return data;
        }

        public string getActividadByID(int id) 
        {
            var actividad = _context.tblM_CatActividadOverhaul.FirstOrDefault(x => x.id == id);
            if (actividad != null)
            {
                return actividad.descripcion;
            }
            else 
                return "N/A";            
        }

        public bool IniciarActividadOverhaul(int actividadID, int eventoID) 
        {
            var evento = _context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.id == eventoID);
            if (evento != null) 
            {
                var stringActividades = evento.actividades;
                if (stringActividades != "") 
                {
                    var actividades = JsonConvert.DeserializeObject<List<ActividadOverhaulDTO>>(stringActividades);
                    var actividad = actividades.FirstOrDefault(x => x.id == actividadID);
                    actividad.fechaInicio = DateTime.Now;
                    actividad.estatus = 1;
                    evento.actividades = JsonConvert.SerializeObject(actividades);
                    _context.SaveChanges();
                    return true;
                }
            }

            return false;
        }

        public bool FinalizarActividadOverhaul(int actividadID, int eventoID)
        {
            var evento = _context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.id == eventoID);
            if (evento != null)
            {
                var stringActividades = evento.actividades;
                if (stringActividades != "")
                {
                    var actividades = JsonConvert.DeserializeObject<List<ActividadOverhaulDTO>>(stringActividades);
                    var actividad = actividades.FirstOrDefault(x => x.id == actividadID);
                    actividad.fechaFin = DateTime.Now;
                    actividad.estatus = 2;
                    evento.actividades = JsonConvert.SerializeObject(actividades);
                    _context.SaveChanges();
                    return true;
                }
            }

            return false; ;
        }

        public bool AgregarOverhaulTaller(int maquinaID, List<int> actividadesID)
        {
            try
            {
                tblM_CapPlaneacionOverhaul evento = new tblM_CapPlaneacionOverhaul();
                List<ActividadOverhaulDTO> actividades = new List<ActividadOverhaulDTO>();
                evento.idComponentes = "";
                evento.maquinaID = maquinaID;
                evento.fecha = DateTime.Now;
                evento.estatus = 1;
                evento.calendarioID = -1;
                evento.indexCal = "-1";
                evento.fechaInicio = DateTime.Now;
                evento.tipo = 3;
                foreach (var item in actividadesID) 
                {
                    ActividadOverhaulDTO aux = new ActividadOverhaulDTO();
                    aux.estatus = 0;
                    aux.id = item;
                    actividades.Add(aux);
                }
                evento.actividades = actividades.Count > 0 ? JsonConvert.SerializeObject(actividades) : "";
                _context.tblM_CapPlaneacionOverhaul.Add(evento);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e) 
            {
                return false;
            }
        }

        public List<ComboDTO> FillCboEconomico_Componente(int modeloID) 
        {
            var maquinas = _context.tblM_CatMaquina.Where(x => x.modeloEquipoID == modeloID).Select(y => new ComboDTO { Value = y.id.ToString(), Text = y.noEconomico }).ToList();
            return maquinas;
        }

        public List<tblM_CatActividadOverhaul> CargarDatosDiagramaGantt(int idModelo, string descripcion, bool estatus)
        {
            List<tblM_CatActividadOverhaul> data = new List<tblM_CatActividadOverhaul>();
            data = _context.tblM_CatActividadOverhaul.Where(x => (x.modeloID == 0 || (idModelo == -1 ? true : x.modeloID == idModelo)) && x.descripcion.Contains(descripcion) && x.estatus == estatus).ToList();
            return data;
        }

        public List<ActividadOverhaulDTO> CargarDatosDiagramaGantt(int idModelo, int eventoID)
        {
            List<ActividadOverhaulDTO> data = new List<ActividadOverhaulDTO>();
            var evento = _context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.id == eventoID);
            if (evento != null) 
            {
                data = JsonConvert.DeserializeObject<List<ActividadOverhaulDTO>>(evento.actividades);
            }
            return data;
        }

        public tblM_CatActividadOverhaul CargarActividad(int actividadID, List<tblM_CatActividadOverhaul> _lstCatActividadOverhaulDapperDTO = null)
        {
            #region v1
            //var actividad = _context.tblM_CatActividadOverhaul.FirstOrDefault(x => x.id == actividadID);
            //if (actividad == null) return new tblM_CatActividadOverhaul();
            //else return actividad;
            #endregion

            #region v2
            if (_lstCatActividadOverhaulDapperDTO != null && _lstCatActividadOverhaulDapperDTO.Count() > 0)
            {
                var actividad = _lstCatActividadOverhaulDapperDTO.FirstOrDefault(x => x.id == actividadID);
                if (actividad == null)
                    return new tblM_CatActividadOverhaul();
                else
                    return actividad;
            }
            else
            {
                var actividad = _context.tblM_CatActividadOverhaul.FirstOrDefault(x => x.id == actividadID);
                if (actividad == null) 
                    return new tblM_CatActividadOverhaul();
                else 
                    return actividad;
            }
            #endregion
        }

        public bool GuardarActividad(int actividadID, string descripcion, int modeloID, bool estatus, bool reporteEjecutivo, decimal horasDuracion) 
        {
            try
            {
                if (actividadID == 0)
                {
                    tblM_CatActividadOverhaul actividad = new tblM_CatActividadOverhaul { 
                        id = 0,
                        descripcion = descripcion,
                        modeloID = modeloID,
                        estatus = estatus,
                        reporteEjecutivo = reporteEjecutivo,
                        horasDuracion = horasDuracion
                    };
                    _context.tblM_CatActividadOverhaul.Add(actividad);
                }
                else
                {
                    tblM_CatActividadOverhaul actividad = _context.tblM_CatActividadOverhaul.FirstOrDefault(x => x.id == actividadID);
                    if (actividad == null) { return false; }
                    else 
                    {
                        actividad.descripcion = descripcion;
                        actividad.modeloID = modeloID;
                        actividad.estatus = estatus;
                        actividad.reporteEjecutivo = reporteEjecutivo;
                        actividad.horasDuracion = horasDuracion;
                    }
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception e) 
            {
                return false;
            }
        }

        public bool GuardarDiagramaGantt(int idEvento, string actividades, decimal sumaHoras, List<decimal> horasTrabajadas)
        {
            var evento = _context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.id == idEvento);

            if (evento != null)
            {
                decimal diasDuracion = 0;
                decimal horasSemana = horasTrabajadas.Where(x => x > 0).Sum();
                int ultimoDiaTrab = (horasTrabajadas.FindLastIndex(x => x > 0)) + 1;
                //List<decimal> diasSemana = horasTrabajadas.Where(item => item > 0).ToList();
                decimal semanas = Math.Floor(sumaHoras / horasSemana);
                decimal residuo = sumaHoras % horasSemana;
                decimal sumatoria = 0;// horasTrabajadas[0];
                int i = 0;
                while (sumatoria < residuo) {
                    sumatoria += horasTrabajadas[i];
                    i++; 
                }
                if (sumatoria == residuo) { diasDuracion = ((semanas - 1) * 7) + ultimoDiaTrab; }
                else { diasDuracion = (semanas * 7) + i; }
                evento.actividades = actividades;
                evento.diasDuracionP = diasDuracion;
                evento.diasTrabajados = JsonConvert.SerializeObject(horasTrabajadas);
                evento.estatus = 1;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<ActividadOverhaulDTO> CargarActGuardadasDiagramaGantt(int idEvento)
        {
            List<ActividadOverhaulDTO> actividadesID = new List<ActividadOverhaulDTO>();
            var evento = _context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.id == idEvento);
            if (evento != null)
            {
                actividadesID = evento.actividades == null ? null : JsonConvert.DeserializeObject<List<ActividadOverhaulDTO>>(evento.actividades);
            }
            return actividadesID;
        }

        private decimal CalculoHrsPromDiarioPub(int economicoID)
        {
            IObjectSet<tblM_CapHorometro> _objectSetCapHorometro = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CapHorometro>();
            var noEconomico = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == economicoID).noEconomico;
            decimal result;
            try
            {
                result = _context.tblM_CapRitmoHorometro.Where(y => y.economico == noEconomico).Select(X => X.horasDiarias).FirstOrDefault();
                if (result == 0)
                {
                    result = (_objectSetCapHorometro.Where(y => y.Economico == noEconomico).OrderByDescending(r => r.id).Take(20).ToArray().Sum(y => y.HorasTrabajo) / 20);
                }
            }
            catch (Exception)
            {
                result = (_objectSetCapHorometro.Where(y => y.Economico == noEconomico).OrderByDescending(r => r.id).Take(20).ToArray().Sum(y => y.HorasTrabajo) / 20);
            }
            return result;
        }


        public List<decimal> CargarDiasTrabajadosDG(int idEvento)
        {
            List<decimal> horasTrabajadas = new List<decimal>();
            var evento = _context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.id == idEvento);
            if (evento != null)
            {
                horasTrabajadas = evento.diasTrabajados == null ? null : JsonConvert.DeserializeObject<List<decimal>>(evento.diasTrabajados);
            }
            return horasTrabajadas;
        }

        public bool IniciarOverhaul(int idEvento, DateTime fechaInicio, int tipoOverhaul) 
        {
            try
            {
                var evento = _context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.id == idEvento);
                //fechaInicio = new DateTime(2020, 2, 15);
                if (evento != null)
                {
                    if (evento.fecha.Year == fechaInicio.Year/*true*/)
                    {
                        if (tipoOverhaul == 0)
                        {
                            int diaSemana = ((int)fechaInicio.DayOfWeek) - 1;
                            List<decimal> arregloDiasTrabajados = JsonConvert.DeserializeObject<List<decimal>>(evento.diasTrabajados);
                            List<ActividadOverhaulDTO> actividades = JsonConvert.DeserializeObject<List<ActividadOverhaulDTO>>(evento.actividades);
                            int diasExtra = -1;

                            for (int i = 0; i < actividades.Count(); i++)
                            {
                                var numDiaActual = actividades[i].numDia - 1;
                                diasExtra++;
                                while (arregloDiasTrabajados[((int)fechaInicio.AddDays(diasExtra).DayOfWeek + 6) % 7] <= 0) { diasExtra++; }
                                while (i < actividades.Count() && numDiaActual + 1 == actividades[i].numDia)
                                {
                                    actividades[i].fechaInicioP = fechaInicio.AddDays(diasExtra);
                                    actividades[i].fechaFinP = fechaInicio.AddDays(diasExtra);
                                    i++;
                                }
                                i--;
                            }
                            evento.actividades = JsonConvert.SerializeObject(actividades);
                            evento.diasDuracionP = diasExtra + 1;
                            evento.fechaFinP = fechaInicio.AddDays(diasExtra);
                            _context.SaveChanges();
                        }

                        evento.fechaInicio = fechaInicio;
                        evento.estatus = 2;
                        _context.SaveChanges();
                        return true;
                    }
                    else { return false; }
                }
                return true;
            }
            catch (Exception e) 
            {
                return false;
            }
        }

        //public bool IniciarOverhaul(int idEvento, DateTime fechaInicio, int tipoOverhaul)
        //{
        //    var evento = _context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.id == idEvento);
        //    if (evento != null)
        //    {
        //        if (evento.fecha.Year == fechaInicio.Year)
        //        {
        //            if (tipoOverhaul == 0)
        //            {
        //                int diaSemana = ((int)fechaInicio.DayOfWeek) - 1;
        //                if (diaSemana == -1) diaSemana = 7;
        //                List<decimal> arregloDiasTrabajados = JsonConvert.DeserializeObject<List<decimal>>(evento.diasTrabajados);
        //                List<ActividadOverhaulDTO> actividades = JsonConvert.DeserializeObject<List<ActividadOverhaulDTO>>(evento.actividades);
        //                List<ActividadOverhaulDTO> actividadesFinal = new List<ActividadOverhaulDTO>();
        //                decimal sumatoriaHorasTrabajadas = arregloDiasTrabajados[diaSemana] - 2;
        //                decimal sumatoriaHorasActividades = 0;
        //                var i = 0;
        //                bool cambioDeDia = false;
        //                foreach (var item in actividades)
        //                {
        //                    cambioDeDia = false;
        //                    while (arregloDiasTrabajados[(i + diaSemana) % 7] == 0) { i++; }
        //                    item.fechaInicioP = fechaInicio.AddDays(i);
        //                    if (sumatoriaHorasTrabajadas > 0) { i++; }
        //                    sumatoriaHorasActividades = item.horasDuracion;
        //                    while (sumatoriaHorasActividades > sumatoriaHorasTrabajadas)
        //                    {                                
        //                        sumatoriaHorasTrabajadas += arregloDiasTrabajados[(i + diaSemana) % 7] - 2;
        //                        i++;
        //                        cambioDeDia = true;
        //                    }
        //                    i--;
        //                    item.fechaFinP = fechaInicio.AddDays(i);
        //                    actividadesFinal.Add(item);
        //                    //if (cambioDeDia) 
        //                    //{
        //                    //    ActividadOverhaulDTO horaComida = new ActividadOverhaulDTO();
        //                    //    horaComida.estatus = 0;
        //                    //    horaComida.fechaFinP = fechaInicio.AddDays(i-1);
        //                    //    horaComida.fechaInicioP = fechaInicio.AddDays(i-1);
        //                    //    horaComida.horasDuracion = 1;
        //                    //    horaComida.id = 510;
        //                    //    ActividadOverhaulDTO ordeYLimpieza = new ActividadOverhaulDTO();
        //                    //    ordeYLimpieza.estatus = 0;
        //                    //    ordeYLimpieza.fechaFinP = fechaInicio.AddDays(i-1);
        //                    //    ordeYLimpieza.fechaInicioP = fechaInicio.AddDays(i-1);
        //                    //    ordeYLimpieza.horasDuracion = 1;
        //                    //    ordeYLimpieza.id = 511;
        //                    //    actividadesFinal.Add(horaComida);
        //                    //    actividadesFinal.Add(ordeYLimpieza);
        //                    //}
        //                    if (sumatoriaHorasActividades == sumatoriaHorasTrabajadas) { i++; }
        //                    sumatoriaHorasTrabajadas = sumatoriaHorasTrabajadas - sumatoriaHorasActividades;
        //                }
        //                evento.diasDuracionP = i;
        //                evento.fechaFinP = fechaInicio.AddDays(i);
        //                evento.actividades = JsonConvert.SerializeObject(actividadesFinal);
        //            }
        //            evento.fechaInicio = fechaInicio;
        //            evento.estatus = 2;
        //            _context.SaveChanges();
        //            return true;
        //        }
        //    }
        //    return false; ;
        //}

        public List<ActividadOverhaulDTO> CargarDatosActividadesTaller(int idEvento)
        {
            List<ActividadOverhaulDTO> actividades = new List<ActividadOverhaulDTO>();
            List<ActividadOverhaulDTO> auxActividades = new List<ActividadOverhaulDTO>();
            var evento = _context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.id == idEvento);
            if (evento != null)
            {
                actividades = evento.actividades == null ? null : JsonConvert.DeserializeObject<List<ActividadOverhaulDTO>>(evento.actividades);
                var actiID = 0;
                for (int i = 0; i < actividades.Count(); i++) {
                    if (actividades[i].idAct == 463) {
                        int a = 0;
                    }
                    ActividadOverhaulDTO auxAct = actividades[i];
                    actiID = actividades[i].idAct;
                    while (i < actividades.Count() && actividades[i].idAct == actiID) { i++; }
                    i--;
                    auxAct.fechaFinP = actividades[i].fechaFinP;
                    //auxAct.fechaFin = actividades[i].fechaFin;
                    auxActividades.Add(auxAct);
                }
            }
            return auxActividades;
        }

        public string getDescripcionActividadOverhaul(int idActividad, List<tblM_CatActividadOverhaul> _lstCatActividadOverhaulDapperDTO = null)
        {
            #region v1
            //var actividad = _context.tblM_CatActividadOverhaul.FirstOrDefault(x => x.id == idActividad);
            //if (actividad != null) { return actividad.descripcion; }
            //else { return "N/A"; }
            #endregion

            #region v2
            if (_lstCatActividadOverhaulDapperDTO != null && _lstCatActividadOverhaulDapperDTO.Count() > 0)
            {
                var actividad = _lstCatActividadOverhaulDapperDTO.FirstOrDefault(x => x.id == idActividad);
                if (actividad != null)
                    return actividad.descripcion;
                else
                    return "N/A"; 
            }
            else
            {
                var actividad = _context.tblM_CatActividadOverhaul.FirstOrDefault(x => x.id == idActividad);
                if (actividad != null) 
                    return actividad.descripcion;
                else 
                    return "N/A";
            }
            #endregion
        }

        public bool IniciarActividad(int idEvento, int idActividad, int id) 
        {
            var evento = _context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.id == idEvento);
            if (evento != null)
            {
                var actividades = evento.actividades == null ? null : JsonConvert.DeserializeObject<List<ActividadOverhaulDTO>>(evento.actividades);
                var indexActividad = actividades.FindIndex(x => x.idAct == idActividad && x.id == id);
                var i = indexActividad;
                DateTime fechaFinP = new DateTime();
                actividades[i].fechaInicio = DateTime.Today;
                actividades[i].estatus = 1;
                fechaFinP = actividades[i].fechaFinP ?? default(DateTime);
                i++;
                
                while (i < actividades.Count() && actividades[i].idAct == idActividad) 
                {
                    fechaFinP = actividades[i].fechaFinP ?? default(DateTime);
                    actividades.RemoveAt(i);
                    i++;
                }
                actividades[indexActividad].fechaFinP = fechaFinP;
                evento.actividades = JsonConvert.SerializeObject(actividades);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool FinalizarActividad(int idEvento, int idActividad, int id)
        {
            var evento = _context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.id == idEvento);
            if (evento != null)
            {
                var actividades = evento.actividades == null ? null : JsonConvert.DeserializeObject<List<ActividadOverhaulDTO>>(evento.actividades);
                var indexActividad = actividades.FindIndex(x => x.idAct == idActividad && x.id == id);
                var fechaInicio = actividades[indexActividad].fechaInicio ?? default (DateTime);
                var fechaFin = DateTime.Today;
                var fechaInicioP = actividades[indexActividad].fechaInicioP;
                var fechaFinP = actividades[indexActividad].fechaFinP;
                var numDia = actividades[indexActividad].numDia;
                actividades.RemoveAt(indexActividad);
                //actividades[indexActividad].fechaInicio = fechaFin;
                //actividades[indexActividad].fechaFin = fechaFin;
                //actividades[indexActividad].estatus = 2;
                while (fechaInicio <= fechaFin) 
                {

                    ActividadOverhaulDTO auxAct = new ActividadOverhaulDTO(); 
                    auxAct.estatus = 2;
                    auxAct.fechaFin = fechaFin;
                    auxAct.fechaInicio = fechaFin;
                    auxAct.id = id;
                    auxAct.idAct = idActividad;
                    auxAct.numDia = 0;
                    auxAct.horasDuracion = 0;
                    auxAct.fechaInicioP = fechaInicioP;
                    auxAct.fechaFinP = fechaFinP;
                    auxAct.numDia = numDia;
                    fechaFin = fechaFin.AddDays(-1);
                    actividades.Insert(indexActividad, auxAct);
                }
                //indexActividad++;


                //while (indexActividad < actividades.Count() && actividades[indexActividad].id == idActividad)
                //{
                //    actividades[indexActividad].fechaFin = DateTime.Today;
                //    actividades[indexActividad].estatus = 2;
                //    indexActividad++;
                //}
                //var actividadActual = actividades.FirstOrDefault(x => x.id == idActividad && x.fechaInicioP == fecha);
                //actividadActual.fechaFin = DateTime.Today;
                //actividadActual.estatus = 2;
                evento.actividades = JsonConvert.SerializeObject(actividades);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool GuardarComentarioActividad(int actividadID, int eventoID, string comentario, int tipo, int numDia)
        {
            try
            {
                tblM_ComentarioActividadOverhaul data = new tblM_ComentarioActividadOverhaul();
                var comentarioGuardado = _context.tblM_ComentarioActividadOverhaul.FirstOrDefault(x => x.eventoID == eventoID && x.actividadID == actividadID && x.tipo == tipo && x.numDia == numDia);
                if (comentarioGuardado != null) { data = comentarioGuardado; }
                data.fecha = DateTime.Now;
                data.comentario = comentario;
                if (comentarioGuardado == null)
                {
                    data.actividadID = actividadID;
                    data.eventoID = eventoID;
                    data.tipo = tipo;
                    data.numDia = numDia;
                    _context.tblM_ComentarioActividadOverhaul.Add(data);
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public tblM_ComentarioActividadOverhaul CargarComentarioActividad(int actividadID, int eventoID, int tipo, int numDia)
        {
            tblM_ComentarioActividadOverhaul data = new tblM_ComentarioActividadOverhaul();
            try
            {
                data = _context.tblM_ComentarioActividadOverhaul.FirstOrDefault(x => x.eventoID == eventoID && x.actividadID == actividadID && x.tipo == tipo && x.numDia == numDia);
            }
            catch (Exception e)
            {
                return data;
            }
            return data;
        }

        public string CargarStringComentarioActividad(int actividadID, int eventoID, int tipo, DateTime fecha, int numDia, List<tblM_ComentarioActividadOverhaul> _lstComentarioActividadOverhaulDapperDTO = null)
        {
            tblM_ComentarioActividadOverhaul data = new tblM_ComentarioActividadOverhaul();
            try
            {
                if (_lstComentarioActividadOverhaulDapperDTO.Count() > 0)
                {
                    data = _lstComentarioActividadOverhaulDapperDTO.FirstOrDefault(x => x.eventoID == eventoID && x.actividadID == actividadID && x.tipo == tipo && x.numDia == numDia);
                    if (data != null)
                        return data.comentario;
                }
                else
                {
                    data = _context.tblM_ComentarioActividadOverhaul.FirstOrDefault(x => x.eventoID == eventoID && x.actividadID == actividadID && x.tipo == tipo && x.numDia == numDia);
                    if (data != null)
                        return data.comentario;
                }
            }
            catch (Exception e)
            {
                return " ";
            }
            return " ";
        }

        public tblM_CapPlaneacionOverhaul getEventoOHByID(int id, List<tblM_CapPlaneacionOverhaul> _lstCapPlaneacionOverhaulDapperDTO = null)
        {
            #region v1
            //var evento = _context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.id == id);
            //return evento;
            #endregion

            #region v2
            if (_lstCapPlaneacionOverhaulDapperDTO != null && _lstCapPlaneacionOverhaulDapperDTO.Count() > 0)
                return _lstCapPlaneacionOverhaulDapperDTO.FirstOrDefault(f => f.id == id);
            else
                return _context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.id == id);
            #endregion
        }

        public bool GuardarArchivoActividad(tblM_ComentarioActividadOverhaul archivo) 
        {
            try {
                if(archivo.tipo == 2){
                    var archivosGuardados = _context.tblM_ComentarioActividadOverhaul.Where(x => x.eventoID == archivo.eventoID && x.actividadID == archivo.actividadID && x.tipo == archivo.tipo).ToList();
                    archivo.nombreArchivo += (archivosGuardados.Count() + 1).ToString();
                }
                _context.tblM_ComentarioActividadOverhaul.Add(archivo);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e) {
                return false;
            }
        }

        public List<tblM_ComentarioActividadOverhaul> CargarArchivosActividad(int idEvento, int idActividad, int tipo, int numDia = -1, List<tblM_ComentarioActividadOverhaul> _lstComentarioActividadOverhaulDapperDTO = null)
        {
            #region v1
            //var archivos = _context.tblM_ComentarioActividadOverhaul.Where(x => x.eventoID == idEvento && (idActividad == -1 ? true : x.actividadID == idActividad) && x.tipo == tipo && (numDia == -1 ? true : x.numDia == numDia)).ToList();
            //return archivos;
            #endregion

            #region v2
            if (_lstComentarioActividadOverhaulDapperDTO != null && _lstComentarioActividadOverhaulDapperDTO.Count() > 0)
                return _lstComentarioActividadOverhaulDapperDTO.Where(x => x.eventoID == idEvento && (idActividad == -1 ? true : x.actividadID == idActividad) && x.tipo == tipo && (numDia == -1 ? true : x.numDia == numDia)).ToList();
            else
                return _context.tblM_ComentarioActividadOverhaul.Where(x => x.eventoID == idEvento && (idActividad == -1 ? true : x.actividadID == idActividad) && x.tipo == tipo && (numDia == -1 ? true : x.numDia == numDia)).ToList();
            #endregion
        }

        public tblM_ComentarioActividadOverhaul getComentarioByID(int idComentario) 
        {
            return _context.tblM_ComentarioActividadOverhaul.FirstOrDefault(x => x.id == idComentario);
        }

        public bool EliminarArchivo(int idArchivo) 
        {
            try {
                var archivo = _context.tblM_ComentarioActividadOverhaul.FirstOrDefault(x => x.id == idArchivo);
                var tipo = archivo.tipo;
                var eventoID = archivo.eventoID;
                var actividadID = archivo.actividadID;
                if (archivo != null) {
                    _context.tblM_ComentarioActividadOverhaul.Remove(archivo);
                    _context.SaveChanges();
                    if (tipo == 2) 
                    {
                        var archivosGuardados = _context.tblM_ComentarioActividadOverhaul.Where(x => x.eventoID == eventoID && x.actividadID == actividadID && x.tipo == tipo).OrderBy(x => x.fecha).ToList();
                        for(int i = 0; i < archivosGuardados.Count(); i++)
                        {
                            string auxNombre = archivosGuardados[i].nombreArchivo.Split('.')[0];
                            archivosGuardados[i].nombreArchivo = auxNombre + "." + (i + 1).ToString();
                        }
                    }
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception e) { return false; }
        }

        public int getArchivosCount(int idActividad) 
        {
            try {
                var archivos = _context.tblM_ComentarioActividadOverhaul.Where(x => x.actividadID == idActividad && x.tipo == 2);
                if (archivos != null) {
                    return archivos.Count();
                }
                return 0;
            }
            catch (Exception e) { return 0; }
        }

        public bool TerminarOverhaul(int idEvento, DateTime fechaFin, int tipoOverhaul, int estatus)
        {
            var evento = _context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.id == idEvento);
            List<ActividadOverhaulDTO> auxActividades = new List<ActividadOverhaulDTO>();
            if (evento != null)
            {
                
                if (estatus == 2)
                {
                    if (tipoOverhaul == 0)
                    {
                        List<ActividadOverhaulDTO> actividades = JsonConvert.DeserializeObject<List<ActividadOverhaulDTO>>(evento.actividades);
                        var actiID = 0;
                        for (int i = 0; i < actividades.Count(); i++)
                        {
                            if (actividades[i].idAct == 463)
                            {
                                int a = 0;
                            }
                            ActividadOverhaulDTO auxAct = actividades[i];
                            actiID = actividades[i].idAct;
                            while (i < actividades.Count() && actividades[i].idAct == actiID) { i++; }
                            i--;
                            auxAct.fechaFinP = actividades[i].fechaFinP;
                            auxActividades.Add(auxAct);
                        }
                        var actividadesSinFechaFin = auxActividades.Where(x => x.fechaFin == null).ToList();
                        if (actividadesSinFechaFin.Count() > 0) { return false; }
                        evento.estatus = 3;
                    }
                    else { evento.estatus = 5; }
                    evento.fechaFin = fechaFin;                    
                    _context.SaveChanges();
                    return true;
                }
                else {
                    if (estatus == 3)
                    {
                        evento.estatus = 4;
                        _context.SaveChanges();
                        return true;
                    }
                    else 
                    {
                        if (estatus == 4) 
                        {
                            evento.estatus = 5;
                            _context.SaveChanges();
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public string getCCByCalendarioID(int idCalendario)
        {
            var calendario = _context.tblM_CalendarioPlaneacionOverhaul.FirstOrDefault(x => x.id == idCalendario);
            if (calendario != null)
            {
                return calendario.obraID;
            }
            return "";
        }

        public List<tblM_CatMaquina> getEconomicosByObraID(string obra)
        {
            var modelosOverhauleables = _context.tblM_CatModeloEquipotblM_CatSubConjunto.Select(x => x.modeloID).Distinct().ToList();
            var maquinas = _context.tblM_CatMaquina.Where(x => (obra == "-1" ? true : x.centro_costos == obra) && modelosOverhauleables.Contains(x.modeloEquipoID)).ToList();
            return maquinas;
        }

        public List<tblM_CatMaquina> getEconomicosByCalendarioID(int idCalendario)
        {
            var modelosOverhauleables = _context.tblM_CatModeloEquipotblM_CatSubConjunto.Select(x => x.modeloID).Distinct().ToList();
            var maquinasIDs = _context.tblM_CapPlaneacionOverhaul.Where(x => x.calendarioID == idCalendario && x.estatus < 5).Select(x => x.maquinaID).ToList();
            var maquinas = _context.tblM_CatMaquina.Where(x => maquinasIDs.Contains(x.id) && modelosOverhauleables.Contains(x.modeloEquipoID)).ToList();
            return maquinas;
        }

        public List<ComponentePlaneacionDTO> CargarGridOHFallaTaller(int idMaquina, bool planeacion)
        {
            List<ComponentePlaneacionDTO> data = new List<ComponentePlaneacionDTO>();
            
            var trackActual = _context.tblM_trackComponentes.GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault()).Where(x => x.locacionID == idMaquina).Where(x => x.componente != null).Select(x => x.componente).ToList();

            data = trackActual.Select(x => new ComponentePlaneacionDTO
            {
                Value = "0",
                componenteID = x.id,
                nombre = x.noComponente,
                descripcion = x.subConjunto.descripcion,
                posicion = x.posicionID > 0 ? Core.Enum.EnumHelper.GetDescription((Core.Enum.Maquinaria.PosicionesEnum)x.posicionID) : "",
                Tipo = 0,
                horasCiclo = x.horaCicloActual,
                target = x.cicloVidaHoras,
                tipoOverhaul = x.subConjuntoID == 1 ? 1 : 2
            }).ToList();
            var trackServicios = _context.tblM_CatServicioOverhaul.Where(x => x.maquinaID == idMaquina &&  x.estatus == true)
                .Join(_context.tblM_CatTipoServicioOverhaul.Where(x => (planeacion ? true : !x.planeacion)), x => x.tipoServicioID, y => y.id, (x, y) => new { x, y })
                .Select(x => new ComponentePlaneacionDTO {
                    Value = "0",
                    componenteID = x.x.id,
                    nombre = x.y.nombre,
                    descripcion = x.y.descripcion,
                    posicion = "",
                    Tipo = 1,
                    horasCiclo = x.x.horasCicloActual,
                    target = x.x.cicloVidaHoras,
                    tipoOverhaul = 0
                }).ToList();
            data.AddRange(trackServicios);
            return data;
        }

        public bool GuardarOHFallaTaller(int idMaquina, List<ComponentePlaneacionDTO> componentes, DateTime fecha, int calendarioID, int tipo)
        {
            try
            {
                List<tblM_CapPlaneacionOverhaul> eventosGuardados = _context.tblM_CapPlaneacionOverhaul.Where(x => x.maquinaID == idMaquina && x.calendarioID == calendarioID).ToList();
                List<tblM_CapPlaneacionOverhaul> eventosAbiertos = _context.tblM_CapPlaneacionOverhaul.Where(x => x.calendarioID == calendarioID && x.estatus < 5).ToList();
                var componetesSaveIds = componentes.Select(x => x.componenteID).ToList();
                foreach (var item in eventosAbiertos) {
                    var auxComponentes = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(item.idComponentes);
                    var componentesIDs = auxComponentes.Select(x => x.componenteID).ToList();
                    foreach (var item2 in componentesIDs) {
                        if(componetesSaveIds.Contains(item2)) {
                            var componenteActual = auxComponentes.FirstOrDefault(x => x.componenteID == item2);
                            var estado = componenteActual == null ? "0" : componenteActual.Value;
                            if (item.fechaInicio == null) {
                                if (item.fecha.Month == fecha.Month) { }//return false;
                            }
                            else {
                                if (estado == "0") return false;
                            }
                        }
                    }
                }

                var componenteIDs = componentes.Select(x => x.componenteID).ToList();
                foreach (var eventoGuardado in eventosGuardados) 
                {
                    var auxComponentes = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(eventoGuardado.idComponentes);
                    foreach (var comp in auxComponentes) 
                    {
                        if (componenteIDs.Contains(comp.componenteID) ) 
                        {
                            comp.falla = true;
                        }
                    }
                    eventoGuardado.idComponentes = JsonConvert.SerializeObject(auxComponentes);
                }
                var fechaActual = DateTime.Now;
                tblM_CapPlaneacionOverhaul evento = new tblM_CapPlaneacionOverhaul();
                evento.calendarioID = calendarioID;
                evento.fecha = fecha;
                evento.estatus = 0;
                evento.tipo = tipo;
                evento.idComponentes = JsonConvert.SerializeObject(componentes);
                evento.maquinaID = idMaquina;
                evento.id = 0;                
                _context.tblM_CapPlaneacionOverhaul.Add(evento);
                _context.SaveChanges();
                evento.indexCal = evento.id + "-3-" + fechaActual.Year.ToString() + fechaActual.Month.ToString() + idMaquina;
                _context.SaveChanges();
                return true;
            }
            catch (Exception e) 
            {
                return false;
            }            
        }

        public bool UpdateOHFallaTaller(int id, List<ComponentePlaneacionDTO> componentes, int calendarioID)
        {
            try
            {
                var evento = _context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.id == id);
                List<string> busquedaComponentesIds = componentes.Select(x => x.componenteID.ToString()).ToList();

                List<tblM_CapPlaneacionOverhaul> eventosAbiertos = _context.tblM_CapPlaneacionOverhaul.Where(x => x.calendarioID == calendarioID && x.estatus < 5 && busquedaComponentesIds.Any(y => x.idComponentes.Contains("\"componenteID\":" + y + ","))).ToList();
                var componetesSaveIds = componentes.Select(x => x.componenteID).ToList();
                foreach (var item in eventosAbiertos)
                {
                    var auxComponentesAbiertos = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(item.idComponentes);
                    var componentesIDs = auxComponentesAbiertos.Select(x => x.componenteID).ToList();
                    foreach (var item2 in componentesIDs)
                    {
                        if (item.fechaInicio == null)
                        {
                            if (item.fecha.Month == evento.fecha.Month) return false;
                            else {
                                var componentesEliminar = auxComponentesAbiertos.Where(x => componetesSaveIds.Contains(x.componenteID)).ToList();
                                foreach(var componenteEliminar in componentesEliminar) auxComponentesAbiertos.Remove(componenteEliminar);
                                item.idComponentes = JsonConvert.SerializeObject(auxComponentesAbiertos);
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                List<ComponentePlaneacionDTO> auxComponentes = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(evento.idComponentes);
                auxComponentes.AddRange(componentes);
                evento.idComponentes = JsonConvert.SerializeObject(auxComponentes);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public tblM_CapPlaneacionOverhaul VerificarOHFallaTaller(int idMaquina, DateTime fecha, int calendarioID)
        {
            tblM_CapPlaneacionOverhaul data = new tblM_CapPlaneacionOverhaul();
            DateTime fechaFin = fecha.AddMonths(1);
            data = _context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.maquinaID == idMaquina && /*(x.fechaInicio == null ? (x.fecha >= fecha && x.fecha < fechaFin) : (x.fechaInicio >= fecha && x.fechaInicio < fechaFin)) &&*/ x.calendarioID == calendarioID && x.estatus < 5);

            return data;
        }

        public bool GuardarOHParoTaller(int idMaquina, List<ComponentePlaneacionDTO> componentes, DateTime fecha, int calendarioID, string indexCal)
        {
            try
            {
                List<tblM_CapPlaneacionOverhaul> eventosGuardados = _context.tblM_CapPlaneacionOverhaul.Where(x => x.maquinaID == idMaquina && x.calendarioID == calendarioID).ToList();
                var componenteIDs = componentes.Select(x => x.componenteID).ToList();
                var tipoParo = 2;
                foreach (var item in componentes) 
                {
                    if (item.tipoOverhaul < tipoParo) tipoParo = item.tipoOverhaul;
                    if (item.posicion == null) item.posicion = "";
                }

                //_context.tblM_CapPlaneacionOverhaul.RemoveRange(eventosGuardados.Where(x => x.estatus == -2));
                tblM_CapPlaneacionOverhaul evento = new tblM_CapPlaneacionOverhaul();
                evento.calendarioID = calendarioID;
                evento.fecha = fecha;
                evento.estatus = 0;
                evento.tipo = tipoParo;
                evento.idComponentes = JsonConvert.SerializeObject(componentes);
                evento.maquinaID = idMaquina;
                evento.id = 0;
                evento.indexCal = indexCal;
                evento.indexCalOriginal = indexCal;
                _context.tblM_CapPlaneacionOverhaul.Add(evento);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool CheckTallerRemocion(int componenteID, int maquinaID) {
            var ohActivos = _context.tblM_CapPlaneacionOverhaul.Where(x => x.estatus > 0 && x.estatus < 5 && x.maquinaID == maquinaID).ToList();
            foreach (var item in ohActivos) 
            {
                var componentes = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(item.idComponentes);
                foreach (var item2 in componentes) 
                {
                    if (item2.componenteID == componenteID) { return true; }
                }
            }
            return false;
        }

        public bool TerminarOverhaul(int index) 
        {
            var evento = _context.tblM_CapPlaneacionOverhaul.FirstOrDefault(x => x.id == index);
            if (evento != null) 
            {
                evento.estatus = 6;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        #region CONSULTAS CON DAPPER
        public List<tblM_CapPlaneacionOverhaul> _lstCapPlaneacionOverhaulDapperDTO(MainContextEnum idEmpresa)
        {
            try
            {
                List<tblM_CapPlaneacionOverhaul> _lstCapPlaneacionOverhaulDapperDTO = _context.Select<tblM_CapPlaneacionOverhaul>(new Core.DTO.Utils.Data.DapperDTO
                {
                    baseDatos = idEmpresa,
                    consulta = @"SELECT id, idComponentes, maquinaID, fecha, tipo, estatus, calendarioID, indexCal, actividades, fechaInicio, fechaFin, diasDuracionP, diasTrabajados, 
		                                            fechaFinP, ritmo, terminado, indexCalOriginal 
			                                            FROM tblM_CapPlaneacionOverhaul"
                });
                return _lstCapPlaneacionOverhaulDapperDTO;
            }
            catch (Exception e)
            {
                LogError(16, 16, "_lstCapPlaneacionOverhaulDapperDTO", "Controller", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public List<tblM_ComentarioActividadOverhaul> _lstComentarioActividadOverhaulDapperDTO(MainContextEnum idEmpresa)
        {
            try
            {
                List<tblM_ComentarioActividadOverhaul> _lstComentarioActividadOverhaulDapperDTO = _context.Select<tblM_ComentarioActividadOverhaul>(new Core.DTO.Utils.Data.DapperDTO
                {
                    baseDatos = idEmpresa,
                    consulta = @"SELECT id, comentario, eventoID, actividadID, fecha, tipo, nombreArchivo, numDia FROM tblM_ComentarioActividadOverhaul"
                });
                return _lstComentarioActividadOverhaulDapperDTO;
            }
            catch (Exception e)
            {
                LogError(16, 16, "_lstComentarioActividadOverhaulDapperDTO", "Controller", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public List<tblM_CatActividadOverhaul> _lstCatActividadOverhaulDapperDTO(MainContextEnum idEmpresa)
        {
            try
            {
                List<tblM_CatActividadOverhaul> _lstCatActividadOverhaulDapperDTO = _context.Select<tblM_CatActividadOverhaul>(new Core.DTO.Utils.Data.DapperDTO
                {
                    baseDatos = idEmpresa,
                    consulta = @"SELECT id, descripcion, horasDuracion, modeloID, dia, estatus, reporteEjecutivo FROM tblM_CatActividadOverhaul"
                });
                return _lstCatActividadOverhaulDapperDTO;
            }
            catch (Exception e)
            {
                LogError(16, 16, "_lstCatActividadOverhaulDapperDTO", "Controller", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }
        #endregion
    }
}
