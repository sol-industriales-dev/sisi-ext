using Core.Entity.Principal.Configuraciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.CuadroComparativo.Equipo
{
    public class tblM_CCE_DetEquipo : RegistroDTO
    {
        public int Id { get; set; }
        public int IdCuadro { get; set; }
        public int IdProveedor { get; set; }
        public int IdMarca { get; set; }
        public int IdModelo { get; set; }
        public bool EsSeleccionado { get; set; }
        [ForeignKey("IdCuadro")]
        public virtual tblM_CCE_EncEquipo Cuadro { get; set; }
        [ForeignKey("IdEquipo")]
        public virtual IEnumerable<tblM_CCE_DetConcepto> Valores { get; set; }
        [ForeignKey("IdEquipo")]
        public virtual IEnumerable<tblM_CCE_DetCaracteristicas> Caracteristicas { get; set; }
        public tblM_CCE_DetEquipo()
        {
            Cuadro = new tblM_CCE_EncEquipo();
            Valores = Enumerable.Empty<tblM_CCE_DetConcepto>();
            Caracteristicas = Enumerable.Empty<tblM_CCE_DetCaracteristicas>();
        }
        
    }
}
