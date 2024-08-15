using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Evaluacion360
{
    public class CatCriterioDTO
    {
        #region SQL
        public int id { get; set; }
        public decimal limInferior { get; set; }
        public decimal limSuperior { get; set; }
        public string etiqueta { get; set; }
        public string descripcionEtiqueta { get; set; }
        public string color { get; set; }
        public int idCuestionario { get; set; }
        public int idPlantilla { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public string nombreCuestionario { get; set; }
        #endregion
    }
}
