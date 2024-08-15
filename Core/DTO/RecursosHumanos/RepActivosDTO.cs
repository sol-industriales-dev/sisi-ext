using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos
{
    public class RepActivosDTO
    {
        public string cC { get; set; }
        public string cc_contable { get; set; }
        public int empleadoID { get; set; }
        public string empleado { get; set; }
        public string puesto { get; set; }
        public int idPuesto { get; set; }
        public int tipo_nominaID { get; set; }
        public string tipo_nomina { get; set; }
        public string nss { get; set; }
        public string jefeInmediato { get; set; }
        public DateTime? fechaAlta { get; set; }
        public string fechaAltaStr { get; set; }
        public string fechaRe { get; set; }
        public string fechaAltaRe { get; set; }
        public decimal salario_base { get; set; }
        public decimal complemento { get; set; }
        public decimal bono_zona { get; set; }
        public decimal? total_mensual { get; set; }
        public decimal? total_nominal { get; set; }
        public string departamento { get; set; }
        public int departamentoID { get; set; }
        public int? requisicion { get; set; }
        public string regpat { get; set; }
        public string domicilio { get; set; }
        public string nombre_estado_nac { get; set; }
        public string nombre_ciudad_nac { get; set; }
        public string fecha_nac { get; set; }
        public string email { get; set; }
        public string tel_cel { get; set; }
        public string tel_casa { get; set; }
        public string en_accidente_nombre { get; set; }
        public string en_accidente_telefono { get; set; }
        public string sexo { get; set; }
        public string rfc { get; set; }
        public string curp { get; set; }
        public string estado_civil { get; set; }
        public string tipoSangre { get; set; }
        public string alergias { get; set; }
        public string tipoCasa { get; set; }
        public string ocupacion { get; set; }
        public int? numHijos { get; set; }
        public string nombreConyuge { get; set; }
        public List<tblRH_EK_Empl_Familia> hijxs { get; set; }
        public string contratoDesc { get; set; }
        public string descCategoria { get; set; }
        public string camisa { get; set; }
        public string calzado { get; set; }
        public string pantalon { get; set; }
        public string dni { get; set; }
        public string cuspp { get; set; }
        public string ciudadContacto { get; set; }
        public string estatus_empleado { get; set; }
        public decimal bono_cuadrado { get; set; }
    }
}
