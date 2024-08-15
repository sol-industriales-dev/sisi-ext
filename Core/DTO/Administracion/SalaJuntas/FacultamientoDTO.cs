using Core.Enum.Administracion.AgendarJunta;
using System;

namespace Core.DTO.Administracion.SalaJuntas
{
    public class FacultamientoDTO
    {
        #region SQL
        public int id { get; set; }
        public int FK_Usuario { get; set; }
        public TipoFacultamientosEnum tipoFacultamiento { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public string nombreCompleto { get; set; }
        public string facultamiento { get; set; }
        #endregion
    }
}
