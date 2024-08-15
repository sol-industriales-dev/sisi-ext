using Core.Entity.Principal.Multiempresa;
using System;
using System.Collections.Generic;

namespace Core.Entity.Administrativo.FacultamientosDpto
{
    public class tblFA_Paquete
    {
        public int id { get; set; }
        public int ccID { get; set; }
        public virtual tblP_CC cc { get; set; }
        /// <summary>
        /// Estado de autorización del paquete (editando, pendienteAutorizacion, autorizado)
        /// </summary>
        public int estado { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        /// <summary>
        /// Guarda el comentario que se deja como justficación al rechazar un paquete o al crear una nueva versión del mismo.
        /// </summary>
        public string comentario { get; set; }
        /// <summary>
        /// Estatus del paquete. 
        /// True - Activo.
        /// False - Inactivo.
        /// Null - Pendiente
        /// </summary>
        public bool? esActivo { get; set; }
        public int? usuarioCreadorID { get; set; }
        public virtual List<tblFA_Facultamiento> facultamientos { get; set; }
        public virtual List<tblFA_Autorizante> autorizantes { get; set; }
    }
}
