using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.BackLogs
{
    public class DetFrentesDTO
    {
        public int id { get; set; }
        public int idFrente { get; set; }
        public string folioPpto { get; set; }
        public int idSeguimientoPpto { get; set; }
        public DateTime fechaAsignacion { get; set; }
        public decimal avance { get; set; }
        public int idInspTMC { get; set; }
        public bool esActivo { get; set; }
        public string tipoMotivo { get; set; }
        public DateTime fechaPromesa { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }     
        public string lstCatFrentes { get; set; }
        public string lstSeguimiento { get; set; }
        public DateTime fechaRequerido { get; set; }
        public string Frente { get; set; }
        public string areaCuenta { get; set; }
    }
}
