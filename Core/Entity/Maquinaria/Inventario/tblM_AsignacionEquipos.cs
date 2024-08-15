using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_AsignacionEquipos
    {
        public int id { get; set; }
        public int solicitudEquipoID { get; set; }
        public string cc { get; set; }
        public string folio { get; set; }
        public int noEconomicoID { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public int Horas { get; set; }
        public int estatus { get; set; }
        public DateTime fechaAsignacion { get; set; }
        public int SolicitudDetalleId { get; set; }
        public string CCOrigen { get; set; }
        public DateTime FechaPromesa { get; set; }
        public string Economico { get; set; }
        //campo prueba raguiar paso intermedio liberacion entre el estatus 4 y los finales 10 0 4.
        public bool? StepPen { get; set; }
        public virtual tblM_SolicitudEquipo SolicitudEquipo { get; set; }
        public virtual tblM_SolicitudEquipoDet SolicitudDetalle { get; set; }
        //public virtual tblM_CatMaquina CatMaquina { get; set; }

    }
}
