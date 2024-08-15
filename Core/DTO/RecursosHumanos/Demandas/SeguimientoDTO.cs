using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Demandas
{
    public class SeguimientoDTO
    {
        #region SQL
        public int id { get; set; }
        public int FK_Demanda { get; set; }
        public decimal cuantia { get; set; }
        public string abogadoDemandante { get; set; }
        public DateTime? fechaAudiencia { get; set; }
        public int semaforo { get; set; }
        public string estadoActual { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public string strSemaforo { get; set; }
        public int numConsecutivo { get; set; }
        #endregion
    }
}
