using Core.DTO;
using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Poliza
{
    public class tblC_Poliza : InfoRegistroTablaDTO
    {
        public int id { get; set; }
        public int year { get; set; }
        public int mes { get; set; }
        public int poliza { get; set; }
        public string tp { get; set; }
        public DateTime fechapol { get; set; }
        public decimal cargos { get; set; }
        public decimal abonos { get; set; }
        public string generada { get; set; }
        public string status { get; set; }
        public string error { get; set; }
        public string status_lock { get; set; }
        public DateTime? fec_hora_movto { get; set; }
        public int? usuario_movto { get; set; }
        public DateTime? fecha_hora_crea { get; set; }
        public int? usuario_crea { get; set; }
        public int? socio_inversionista { get; set; }
        public string status_carga_pol { get; set; }
        public string concepto { get; set; }

        [ForeignKey("polizaId")]
        public virtual List<tblC_Movpol> movimientos { get; set; }
    }
}
