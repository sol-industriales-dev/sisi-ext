using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Multiempresa;

namespace Core.Entity.ControlObra
{
    public class tblCO_PlantillaInforme
    {
        public int id { get; set; }
        public int cantidadDiapositivas { get; set; }
        public bool estatus { get; set; }

        public int division_id { get; set; }
        public virtual tblCO_Divisiones division { get; set; }

        public virtual List<tblCO_PlantillaInforme_detalle> plantillaDetalles { get; set; }
        public virtual List<tblCO_informeSemanal> informes { get; set; }
    }
}
