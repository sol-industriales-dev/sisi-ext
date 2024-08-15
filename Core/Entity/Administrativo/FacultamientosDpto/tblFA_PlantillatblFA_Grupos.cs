using Core.Entity.Principal.Usuarios;

namespace Core.Entity.Administrativo.FacultamientosDpto
{
    public class tblFA_PlantillatblFA_Grupos
    {
        public int id { get; set; }
        public int plantillaID { get; set; }
        public virtual tblFA_Plantilla plantilla { get; set; }
        public int grupoID { get; set; }
        public virtual tblFA_Grupos grupo { get; set; }
    }
}
