using Core.Entity.Principal.Usuarios;

namespace Core.Entity.Administrativo.FacultamientosDpto
{
    public class tblFA_Autorizante
    {
        public int id { get; set; }
        public bool esAutorizante { get; set; }
        public int? usuarioID { get; set; }
        public virtual tblP_Usuario usuario { get; set; }
        public bool? autorizado { get; set; }
        public int orden { get; set; }
        public int paqueteID { get; set; }
        public virtual tblFA_Paquete paquete { get; set; }
        public string firma { get; set; }
    }
}
