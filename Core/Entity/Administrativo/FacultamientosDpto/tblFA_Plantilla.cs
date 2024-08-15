using System;
using System.Collections.Generic;

namespace Core.Entity.Administrativo.FacultamientosDpto
{
    public class tblFA_Plantilla
    {
        public int id { get; set; }
        public string titulo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool esActiva { get; set; }
        public int? usuarioCreadorID { get; set; }
        public int orden { get; set; }
        public virtual List<tblFA_ConceptoPlantilla> conceptosPlantilla { get; set; }
        public virtual List<tblFA_PlantillatblFA_Grupos> plantillasDepartamento { get; set; }
        public virtual List<tblFA_Facultamiento> facultamientos { get; set; }
    }
}
