using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.Usuario
{
    public class USUARIO_COMP
    {
        public string USU_CODIGO { get; set; }
        public string EMP_CODIGO { get; set; }
        public string USU_PASSWORD { get; set; }
        public string USU_NIVEL { get; set; }
        public string USU_NOMBRE { get; set; }
        public string EMAIL { get; set; }
        public bool? DIGITALIZA_FIRMA { get; set; }
        //public string ARCHIVO_FIRMA { get; set; }
        public string NOMBRE_ARCHIVO { get; set; }
        public string CARGO { get; set; }
        public bool? FLGPORTAL_APROB { get; set; }
        public string COD_AUDITORIA { get; set; }
        public bool? FLGPORTAL_COMPRAS { get; set; }
        public bool? USU_ESTADO { get; set; }
        public string USU_FEC_CREA { get; set; }
        public string USU_FEC_INACTIVO { get; set; }

    }
}
