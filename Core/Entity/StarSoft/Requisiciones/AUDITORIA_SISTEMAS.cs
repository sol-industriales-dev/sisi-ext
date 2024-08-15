using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.Requisiciones
{
    public class AUDITORIA_SISTEMAS
    {
        public string COD_AUDITORIA { get; set; }
        [Key]
        public string SECUENCIA { get; set; }
        public string COD_USUARIO { get; set; }
        public DateTime FECHA_SISTEMA { get; set; }
        public DateTime FECHA_TRABAJO { get; set; }
        public string HORA { get; set; }
        public string OPERACION { get; set; }
        public string COD_UNIVERSAL { get; set; }
        public string DES_CODUNIV { get; set; }
        public string TIPO_ANEXO { get; set; }
        public string COD_ANEXO { get; set; }
        public string CODPROCESO { get; set; }
        public string COD_EMPRESA { get; set; }
        public string COD_MODULO { get; set; }
        public string COD_MODULO_DESTINO { get; set; }
        public string OBSERVACION { get; set; }
        public string NOMBRE_PC { get; set; }
        public string TIPO_USUARIO { get; set; }
        public string TIPO_DOC { get; set; }
        public string SERIE_DOC { get; set; }
        public string NUMERO_DOC { get; set; }
        public DateTime? FECHA_REG_EJE { get; set; }
        public string COD_MENU { get; set; }
        public string COD_CLIENTE { get; set; }
        public string NOMBRE_CLIENTE { get; set; }
        public string COD_ARTICULO { get; set; }
        public string NOMBRE_ARTICULO { get; set; }
        public string CODIGO_USUARIO { get; set; }
        public string COD_PROVEEDOR { get; set; }
        public string NOMBRE_PROVEEDOR { get; set; }
        public string COD_LOTE { get; set; }
        public string CODIGO_RECETA { get; set; }
        public string VERSION_RECETA { get; set; }
        public string NOMBRE_RECETA { get; set; }
    }
}
