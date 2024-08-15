using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos
{
    public class RepBajasDTO
    {
        public string cCSolo { get; set; }
        public string cC { get; set; }
        public string empleadoID { get; set; }
        public string empleado { get; set; }
        public string jefeInmediatoID { get; set; }
        public string jefeInmediato { get; set; }
        public DateTime fechaBaja { get; set; }
        public string concepto { get; set; }
        public string claveConcepto { get; set; }

        public DateTime? fechaAlta { get; set; }
        public string fechaBajaStr { get; set; }
        public string recontratable { get; set; }
        public string FechaRec { get; set; }
        public string fechaAltaStr { get; set; }
        public string fechaAltaBaja { get; set; }
        public string nss { get; set; }
        public string puestoID { get; set; }
        public string puestosDes { get; set; }
        public string regPatronal { get; set; }
        public string est_inventario_comentario { get; set; }
        public string est_compras_comentario { get; set; }
        public string est_contabilidad_comentario { get; set; }
        public string est_nominas_comentario { get; set; }
        public string est_baja { get; set; }
        public string est_inventario { get; set; }
        public string est_contabilidad { get; set; }
        public string est_compras { get; set; }
        public string est_nominas { get; set; }
        public int estatus_baja { get; set; }
        public string estatus_bajaDesc { get; set; }
        public DateTime fechaAntiguedad { get; set; }
        public DateTime? fechaIngreso { get; set; }
        public string comentarios { get; set; }
        public string categoria { get; set; }
        public DateTime? fechaLiberacion { get; set; }
        public DateTime? est_inventario_fecha { get; set; }
        public DateTime? est_contabilidad_fecha { get; set; }
        public DateTime? est_compras_fecha { get; set; }
        public DateTime? est_nominas_fecha { get; set; }
        public bool? esAnticipada { get; set; }
    }
}
