using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.BackLogs
{
    public  class SeguimientoPptoDTO
    {
        public int id { get; set; }
        public string areaCuenta { get; set; }
        public string folioPpto { get; set; }
        public string cc { get; set; }
        public string descripcion { get; set; }
        public string modelo { get; set; }
        public DateTime fechaPpto { get; set; }
        public string noEconomico { get; set; }
        public decimal horas { get; set; }
        public int idMotivo { get; set; }

        public string motivo { get; set; }


        public int modeloId { get; set; }
        public string tipoMotivo { get; set; }
        public decimal Ppto { get; set; }
        public int idInspTMC { get; set; }
        public DateTime fechaRequerido { get; set; }
        public bool esVobo1 { get; set; }
        public DateTime fechaVobo1 { get; set; }
        public string comentRechaVobo1 { get; set; }
        public bool esVobo2 { get; set; }
        public DateTime fechaVobo2 { get; set; }
        public string comentRechaVobo2 { get; set; }
        public bool esAutorizado { get; set; }
        public DateTime fechaAutorizado { get; set; }
        public string comentRechaAutorizado { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public string nombre { get; set; }
        public int EstatusSegPpto { get; set; }
        public string estado { get; set; }

        public string esactivo { get; set; }
        public string fecharequerido { get; set; }

     
    }
}
