using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_CapOrdenTrabajo
    {

        public int id { get; set; }
        public int EconomicoID { get; set; }
        public string CC { get; set; }
        public decimal horometro { get; set; }
        public int Turno { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int TipoParo1 { get; set; }
        public int TipoParo2 { get; set; }
        public int TipoParo3 { get; set; }
        public int MotivoParo { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime? FechaSalida { get; set; }
        public decimal TiempoTotalParo { get; set; }
        public decimal TiempoReparacion { get; set; }
        public decimal TiempoMuerto { get; set; }
        public string DescripcionTiempoMuerto { get; set; }
        public string DescripcionMotivo { get; set; }
        public int TiempoHorasTotal { get; set; }
        public int TiempoHorasReparacion { get; set; }
        public int TiempoHorasMuerto { get; set; }
        public int TiempoMinutosTotal { get; set; }
        public int TiempoMinutosReparacion { get; set; }
        public int TiempoMinutosMuerto { get; set; }
        public int usuarioCapturaID { get; set; }
        public int TipoOT { get; set; }
        public bool EstatusOT { get; set; }
        public string Comentario { get; set; }
        public string folio { get; set; }
    }
}
