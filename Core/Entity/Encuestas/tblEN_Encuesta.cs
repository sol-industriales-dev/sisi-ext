using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Usuarios;
using System.ComponentModel.DataAnnotations.Schema;
namespace Core.Entity.Encuestas
{
    public class tblEN_Encuesta
    {
        public int id { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public int creadorID { get; set; }
        public virtual tblP_Usuario creador { get; set; }
        public int departamentoID { get; set; }
        //public tblP_Departamento departamento { get; set; }
        public DateTime fecha { get; set; }
        public bool estatus { get; set; }
        public virtual List<tblEN_Preguntas> preguntas { get; set; }
        public int estatusAutoriza { get; set; }
        public int tipo { get; set; }
        public bool? telefonica { get; set; }
        public bool? notificacion { get; set; }
        public bool? papel { get; set; }
        public bool soloLectura { get; set; }

        [ForeignKey("departamentoID")]
        public virtual tblP_Departamento departamento { get; set; }
    }
}
