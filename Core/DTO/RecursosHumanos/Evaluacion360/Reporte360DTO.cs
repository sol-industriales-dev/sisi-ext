using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Evaluacion360
{
    public class Reporte360DTO
    {
        #region ADICIONAL
        public int id { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string nombreCompleto { get; set; }
        public int idPeriodo { get; set; }
        public string estatusCuestionario { get; set; }
        public int idPersonalEvaluado { get; set; }
        public int idPersonalEvaluador { get; set; }
        public int idRelacion { get; set; }
        public int tipoRelacion { get; set; }
        public int idGrupo { get; set; }
        public decimal limSuperior { get; set; }
        public decimal promedio { get; set; }
        public int cantCriterios { get; set; }
        public string nombreGrupo { get; set; }
        public string nombreRelacion { get; set; }
        public decimal limSuperior_Autoevaluacion { get; set; }
        public decimal limSuperior_Pares { get; set; }
        public decimal limSuperior_ClientesInternos { get; set; }
        public decimal limSuperior_Colaboradores { get; set; }
        public decimal limSuperior_Jefe { get; set; }
        public decimal cantCriterios_Autoevaluacion { get; set; }
        public decimal cantCriterios_Pares { get; set; }
        public decimal cantCriterios_ClientesInternos { get; set; }
        public decimal cantCriterios_Colaboradores { get; set; }
        public decimal cantCriterios_Jefe { get; set; }
        public decimal promedio_Autoevaluacion { get; set; }
        public decimal promedio_Pares { get; set; }
        public decimal promedio_ClientesInternos { get; set; }
        public decimal promedio_Colaboradores { get; set; }
        public decimal promedio_Jefe { get; set; }
        public int idCompetencia { get; set; }
        public string nombreCompetencia { get; set; }
        public int idEvaluacion { get; set; }
        public string grafica { get; set; }
        public string nombreJefe { get; set; }
        public string puesto { get; set; }
        public DateTime fechaIngreso { get; set; }
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

        #region RGB EN PROMEDIOS
        public int R_Autoevaluacion { get; set; }
        public int G_Autoevaluacion { get; set; }
        public int B_Autoevaluacion { get; set; }

        public int R_Pares { get; set; }
        public int G_Pares { get; set; }
        public int B_Pares { get; set; }

        public int R_ClientesInternos { get; set; }
        public int G_ClientesInternos { get; set; }
        public int B_ClientesInternos { get; set; }

        public int R_Jefe { get; set; }
        public int G_Jefe { get; set; }
        public int B_Jefe { get; set; }

        public int R_Colaboradores { get; set; }
        public int G_Colaboradores { get; set; }
        public int B_Colaboradores { get; set; }

        public int R_Promedio { get; set; }
        public int G_Promedio { get; set; }
        public int B_Promedio { get; set; }
        #endregion

        public string iconoUno { get; set; }
        public string iconoDos { get; set; }
        public string iconoTres { get; set; }
        public string iconoCuatro { get; set; }
        public string iconoCinco { get; set; }
        public string iconoSeis { get; set; }

        public int ordenImagenPromedio { get; set; }
        public string imagenPromedio { get; set; }

        public string lstFooter { get; set; }

        public string comentarioGeneral { get; set; }
        #endregion
    }
}