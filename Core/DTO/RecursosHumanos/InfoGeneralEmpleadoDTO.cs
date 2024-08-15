using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos
{
    public class InfoGeneralEmpleadoDTO
    {
        #region Informacion General
        public int id { get; set; }
        public int Clave_Empleado { get; set; }
        public string Nombre { get; set; }
        public string Ape_Paterno { get; set; }
        public string Ape_Materno { get; set; }
        public DateTime Fecha_Alta { get; set; }
        public int PuestoID { get; set; }
        public string Puesto { get; set; }
        public int TipoNominaID { get; set; }
        public string TipoNomina { get; set; }
        public string CcID { get; set; }
        public string CC { get; set; }
        public int RegistroPatronalID { get; set; }
        public string RegistroPatronal { get; set; }
        public int Clave_Jefe_Inmediato { get; set; }
        public string Nombre_Jefe_Inmediato { get; set; }
        public decimal Salario_Base { get; set; }
        public decimal Complemento { get; set; }
        #endregion
    }
}
