using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Multiempresa;

namespace Core.DTO.RecursosHumanos.Capacitacion
{
    public class resultCapacitacionDTO
    {
        public int id { get; set; }
        public int usuarioCap { get; set; }


        public string folio { get; set; }
        public int InicioNomina { get; set; }
        public DateTime FechaInicioCambio { get; set; }
        public string Justificacion { get; set; }
        public string CamposCambiados { get; set; }
        public bool Aprobado { get; set; }
        public bool Rechazado { get; set; }
        public string nomUsuarioCap { get; set; }
        public bool editable { get; set; }
        public decimal Bono { get; set; }
        public DateTime fechaCaptura { get; set; }
        public decimal SalarioAnt { get; set; }
        public decimal ComplementoAnt { get; set; }
        public decimal BonoAnt { get; set; }
        public string CCAntID { get; set; }
        public string CCAnt { get; set; }
        public string PuestoAnt { get; set; }
        public string RegistroPatronalAnt { get; set; }
        public string Nombre_Jefe_InmediatoAnt { get; set; }
        public string TipoNominaAnt { get; set; }

        public bool btnSubirArchivo { get; set; }
        public bool btnEliminarArchivos { get; set; }
        public EmpresaEnum empresa { get; set; }
    }
}
