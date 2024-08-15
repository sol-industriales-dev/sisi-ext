using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Administrativo.Seguridad.Capacitacion;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class ListaAutorizacionDTO
    {
        public int id { get; set; }
        public string claveLista { get; set; }
        public int cursoID { get; set; }
        public string claveCurso { get; set; }
        public string cursoNombre { get; set; }
        public string revision { get; set; }
        public string cc { get; set; }
        public string ccDesc { get; set; }
        public int departamento { get; set; }
        public string departamentoDesc { get; set; }
        public int jefeDepartamento { get; set; }
        public string jefeDepartamentoDesc { get; set; }
        public int gerenteProyecto { get; set; }
        public string gerenteProyectoDesc { get; set; }
        public int coordinadorCSH { get; set; }
        public string coordinadorCSHDesc { get; set; }
        public int secretarioCSH { get; set; }
        public string secretarioCSHDesc { get; set; }
        public int seguridad { get; set; }
        public string seguridadDesc { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string fechaCreacionString { get; set; }

        public List<ListaAutorizacionCCDTO> listaCC { get; set; }
        public List<tblS_CapacitacionListaAutorizacionRFC> listaRFC { get; set; }
        public List<ListaAsistentesDTO> listaAsistentes { get; set; }
        public List<ListaInteresadosDTO> listaInteresados { get; set; }
    }
}
