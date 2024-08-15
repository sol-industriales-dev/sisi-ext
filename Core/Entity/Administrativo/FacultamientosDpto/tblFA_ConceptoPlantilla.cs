using System.Collections.Generic;

namespace Core.Entity.Administrativo.FacultamientosDpto
{
    public class tblFA_ConceptoPlantilla
    {
        public int id { get; set; }
        public int plantillaID { get; set; }
        public virtual tblFA_Plantilla plantilla { get; set; }
        public string concepto { get; set; }
        public bool esAutorizacion { get; set; }
        public int orden { get; set; }
        public bool esActivo { get; set; }
        public virtual List<tblFA_Empleado> empleados { get; set; }
    }
}
