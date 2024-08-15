using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.ActoCondicion
{
    public class reporteActoCondicionDTO
    {
        public bool fueContactoPersonal { get; set; }

        public string codigoControl { get; set; }
        public bool esConstruplan { get; set; }
        public bool esContratista { get; set; }
        public bool esActoSeguro { get; set; }
        public bool esActoInseguro { get; set; }
        public bool esCondicion { get; set; }
        public string cc { get; set; }
        public string departamento { get; set; }
        public string fechaSuceso { get; set; }
        public string claveSupervisor { get; set; }
        public string nombreSupervisor { get; set; }
        public string claveEmpleadoInformo { get; set; }
        public string nombreEmpleadoInformo { get; set; }
        public string descripcionActoCondicion { get; set; }
        public string clasificacion { get; set; }
        public string procedimiento { get; set; }
        public string nivelPrioridad { get; set; }
        public string claveEmpleadoActo { get; set; }
        public string nombreEmpleadoActo { get; set; }
        public string claveActoInseguro { get; set; }
        public int accionID { get; set; }

        public bool causa1 { get; set; }
        public bool causa2 { get; set; }
        public bool causa3 { get; set; }
        public bool causa4 { get; set; }
        public bool causa5 { get; set; }
        public bool causa6 { get; set; }
        public bool causa7 { get; set; }
        public bool accion8 { get; set; }
        public bool accion9 { get; set; }
        public bool accion10 { get; set; }
        public bool accion11 { get; set; }
        public bool accion12 { get; set; }
        public bool accion13 { get; set; }
        public bool accion14 { get; set; }
        public bool accion15 { get; set; }

        public string compromisoPersonal { get; set; }
        public string nombrePersonaObservada { get; set; }
        public string nombreSuperior { get; set; }
        public string nombreSMA { get; set; }

        public byte[] firmaEmpleado { get; set; }
        public byte[] firmaSupervisor { get; set; }
        public byte[] firmaSST { get; set; }
    }
}
