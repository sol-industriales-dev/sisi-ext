using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.SalaJuntas
{
    public class SalaJuntasDTO
    {
        #region SQL
        public int id { get; set; }
        public int FK_Sala { get; set; }
        public string asunto { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public string repeticion { get; set; }
        public DateTime fechaFinRepeticion { get; set; }
        public string diasRepeticion { get; set; }
        public string comentarios { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public int FK_Edificio { get; set; }
        #endregion
    }
}
