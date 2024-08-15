using Core.Entity.Principal.Configuraciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.CuadroComparativo.Equipo
{
    public class tblM_CCE_DetCaracteristicas : RegistroDTO
    {
        public int Id { get; set; }
        public int IdEquipo { get; set; }
        public int Orden { get; set; }
        public string Descripcion { get; set; }
        public string Valor { get; set; }
        [ForeignKey("IdEquipo")]
        public virtual tblM_CCE_DetEquipo Equipo { get; set; }
        public tblM_CCE_DetCaracteristicas()
        {
            Equipo = new tblM_CCE_DetEquipo();
        }
    }
}
