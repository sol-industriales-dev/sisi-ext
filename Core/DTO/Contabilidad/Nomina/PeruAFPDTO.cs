using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Nomina
{
    public class PeruAFPDTO
    {
        #region SQL
        public int id { get; set; }
        public int FK_AFP { get; set; }
        public int anio { get; set; }
        public int numMes { get; set; }
        public decimal comisionSobreFlujo { get; set; }
        public decimal comisionAnualSobreSaldo { get; set; }
        public decimal primaDeSeguros { get; set; }
        public decimal aporteObligatorioFondoPensiones { get; set; }
        public decimal remuneracionMaximaAsegurable { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public string mes { get; set; }
        public string afp { get; set; }
        #endregion
    }
}
