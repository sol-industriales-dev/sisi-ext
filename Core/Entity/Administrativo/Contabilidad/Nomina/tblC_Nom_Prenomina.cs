using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_Prenomina
    {
        public int id { get; set; }
        public int year { get; set; }
        public int periodo { get; set; }
        public int tipoNomina { get; set; }
        public string CC { get; set; }
        public string nombreCC { get; set; }
        public DateTime fechaCaptura { get; set; }
        public bool validada { get; set; }
        public int usuarioValidaID { get; set; }
        public DateTime? fechaValidacion { get; set; }
        public int estatus { get; set; }
        public int usuarioCapturaID { get; set; }
        public DateTime? fechaAutorizacion { get; set; }
        public bool notificadoOficina { get; set; }
    }
}
