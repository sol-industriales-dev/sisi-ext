using System;

namespace Core.DTO.Administracion.Comercializacion.CRM
{
    public class ComentarioProyectoDTO
    {
        #region SQL
        public int id { get; set; }
        public int FK_Proyecto { get; set; }
        public string ultimoComentario { get; set; }
        public int FK_UsuarioComentario { get; set; }
        public DateTime fechaComentario { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public string nombreCompletoResponsable { get; set; }
        #endregion
    }
}