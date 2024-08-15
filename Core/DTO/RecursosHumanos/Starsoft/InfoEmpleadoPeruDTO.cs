using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Starsoft
{
    public class InfoEmpleadoPeruDTO
    {
        public int id { get; set; }
        public int claveEmpleado { get; set; }
        public string codafp { get; set; }
        public decimal aporobli { get; set; }
        public decimal topeseguro { get; set; }
        public decimal comisionra { get; set; }
        public int? ctbasico { get; set; }
        public decimal basico { get; set; }
        public decimal asigfam { get; set; }
        public string fondopens { get; set; }
        public string ubigeo { get; set; }
        public int? tipotrab { get; set; }
        public int? situacion { get; set; }
        public bool essaludvida { get; set; }
        public string ruceps { get; set; }
        public bool nopdt { get; set; }
        public bool opcion01 { get; set; }
        public bool opcion02 { get; set; }
        public string opciona { get; set; }
        public string opcionb { get; set; }
        public int nocalculo { get; set; }
        public int afectoquinta { get; set; }
        public string afiliado_eps { get; set; }
        public string codsctr { get; set; }
        public int sctr_pension { get; set; }
        public int sctr_salud { get; set; }
        public int? es_comision_mixta { get; set; }
        public int factor_familiar_eps { get; set; }
        public string trabajador_regimen { get; set; }
        public string trabajador_jornada { get; set; }
        public string trabajador_nocturno { get; set; }
        public string otros_ingresos { get; set; }
        public string sindicalizado { get; set; }
        public string discapacitado { get; set; }
        public string domiciliado { get; set; }
        public bool aseg_pension { get; set; }
        public bool rentas_exoneradas { get; set; }
        public string bancocts { get; set; }
        public string ctacts { get; set; }
        public int? regimen_laboral { get; set; }
    }
}
