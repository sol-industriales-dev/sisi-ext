using Core.Enum.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.KPI
{
    public class tblM_KPI_AuthHomologado
    {
        public int Id { get; set; }
        public int Año { get; set; }
        public int Semana { get; set; }
        public string AC { get; set; }
        public authEstadoEnum AuthEstado { get; set; }
        public string Comentario { get; set; }
        public int UsuarioElaboraID { get; set; }
        public int UsuarioVobo1 { get; set; }
        public int UsuarioVobo2 { get; set; }
        public int UsuarioAutoriza { get; set; }
        public string CadenaElabora { get; set; }
        public string CadenaVobo1 { get; set; }
        public string CadenaVobo2 { get; set; }
        public string CadenaAutoriza { get; set; }
        public int FirmaElabora { get; set; }
        public int FirmaVobo1 { get; set; }
        public int FirmaVobo2 { get; set; }
        public int FirmaAutoriza { get; set; }
        public DateTime FechaElaboracion { get; set; }
        public DateTime FechaVobo1 { get; set; }
        public DateTime FechaVobo2 { get; set; }
        public DateTime FechaAutoriza { get; set; }
        public bool Activo { get; set; }
        public DateTime fechaInicio { get; set; }
    }
}
