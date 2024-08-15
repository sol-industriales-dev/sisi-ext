using Core.Entity.Maquinaria.Inventario;
using Core.Entity.Principal.Configuraciones;
using Core.Enum.Maquinaria.Reportes.CuadroComparativo.Equipo;
using Core.Enum.Principal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.CuadroComparativo.Equipo
{
    public class tblM_CCE_EncEquipo : RegistroDTO
    {
        public int Id { get; set; }
        public int IdAsignacion { get; set; }
        public AdquisicionEnum IdAdquisicion { get; set; }
        public DateTime FechaElaboracion { get; set; }
        public authEstadoEnum Estado { get; set; }
        [ForeignKey("IdCuadro")]
        public virtual IEnumerable<tblM_CCE_DetEquipo> Equipos { get; set; }
        [ForeignKey("IdCuadro")]
        public virtual IEnumerable<tblM_CCE_Auth> Autorizantes { get; set; }

        public tblM_CCE_EncEquipo()
        {
            Estado = authEstadoEnum.EnEspera;
            Equipos = Enumerable.Empty<tblM_CCE_DetEquipo>();
            Autorizantes = Enumerable.Empty<tblM_CCE_Auth>();
        }
    }
}
