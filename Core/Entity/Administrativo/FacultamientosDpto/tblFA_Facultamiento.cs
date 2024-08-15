using System.Collections.Generic;

namespace Core.Entity.Administrativo.FacultamientosDpto
{
    public class tblFA_Facultamiento
    {
        public int id { get; set; }
        public int plantillaID { get; set; }
        public virtual tblFA_Plantilla plantilla { get; set; }
        public int paqueteID { get; set; }
        public virtual tblFA_Paquete paquete { get; set; }
        public bool aplica { get; set; }
        public virtual List<tblFA_Empleado> empleados { get; set; }
    }
}
