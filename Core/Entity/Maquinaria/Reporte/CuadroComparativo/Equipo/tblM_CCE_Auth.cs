using Core.Entity.Principal.Configuraciones;
using Core.Enum.Principal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.CuadroComparativo.Equipo
{
    public class tblM_CCE_Auth : RegistroDTO
    {
        public int Id { get; set; }
        public int IdCuadro { get; set; }
        public int Orden { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaFirma { get; set; }
        public string Firma { get; set; }
        public authEstadoEnum Estado { get; set; }
        [ForeignKey("IdCuadro")]
        public virtual tblM_CCE_EncEquipo Cuadro { get; set; }
    }
}
