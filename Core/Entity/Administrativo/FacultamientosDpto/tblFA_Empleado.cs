namespace Core.Entity.Administrativo.FacultamientosDpto
{
    public class tblFA_Empleado
    {
        public int id { get; set; }
        public string nombreEmpleado { get; set; }
        public int? claveEmpleado { get; set; }
        public int conceptoID { get; set; }
        public virtual tblFA_ConceptoPlantilla concepto { get; set; }
        public int facultamientoID { get; set; }
        public virtual tblFA_Facultamiento facultamiento { get; set; }
        public bool editado { get; set; }
        public bool esActivo { get; set; }
        public bool aplica { get; set; }
    }
}
