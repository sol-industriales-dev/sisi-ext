using Core.Entity.Principal.Usuarios;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Captura
{
    public class tblRH_BN_Incidencia
    {
        public int id { get; set; }
        public int evaluacionID { get; set; }
        public int bonoUnicoID { get; set; }
        public int usuarioID { get; set; }
        [JsonIgnore]
        public tblP_Usuario usuario { get; set; }
        public int id_incidencia { get; set; }
        public int anio { get; set; }
        public int periodo { get; set; }
        public string cc { get; set; }
        public int tipo_nomina { get; set; }
        public string estatus { get; set; }
        public int empleado_modifica { get; set; }
        public DateTime fecha_modifica { get; set; }
        public int usuario_auto { get; set; }
        public DateTime fecha_auto { get; set; }
        public string estatusDesc { get; set; }
        public string nombreEmpMod { get; set; }
        public int? usuario_autoriza_sigoplan { get; set; }
        public bool? layoutEnviado { get; set; }
        [NotMapped]
        public string nombreAutoriza { get; set; }
    }
}
