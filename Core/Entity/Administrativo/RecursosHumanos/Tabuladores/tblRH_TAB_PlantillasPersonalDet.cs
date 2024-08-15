using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Tabuladores
{
    public class tblRH_TAB_PlantillasPersonalDet
    {
        #region SQL
        public int id { get; set; }
        public int FK_Plantilla { get; set; }
        public int FK_Puesto { get; set; }
        public int personalNecesario { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        [ForeignKey("FK_Puesto")]
        public virtual tblRH_EK_Puestos puesto { get; set; }
    }
}
