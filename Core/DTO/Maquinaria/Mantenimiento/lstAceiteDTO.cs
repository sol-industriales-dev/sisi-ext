using Core.Entity.Maquinaria.Mantenimiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Mantenimiento
{
    public class lstAceiteDTO
    {
        public int suministroID { get; set; }
        public int componenteID { get; set; }
        public List<tblM_CatSuministros> descripcion { get; set; }
        public List<tblM_MiscelaneoMantenimiento> edadSuministro { get; set; }
        
    }
}
