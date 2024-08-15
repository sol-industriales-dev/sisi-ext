﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.OrdenCompra
{
    [Table("ANEXO")]
    public class ANEXO
    {
        [Key]
        public string ANEX_CODIGO { get; set; }
        public string TIPOANEX_CODIGO { get; set; }
        public string ANEX_RUC { get; set; }
        public string ANEX_DESCRIPCION { get; set; }
        public string ANEX_REFERENCIA { get; set; }
        public string ANEX_DIRECCION { get; set; }
        public string ANEX_TELEFONO { get; set; }
        public string ANEX_REPRESENTANTE { get; set; }
        public string ANEX_GIRO { get; set; }
        public bool NRETENCION { get; set; }
        public string ANEX_NOMBRE { get; set; }
        public string ANEX_APE_PAT { get; set; }
        public string ANEX_APE_MAT { get; set; }
        public string TIPOPERSONA { get; set; }
        public string TIPODOCUMENTO { get; set; }
        public string DOCUMENTOIDENTIDAD { get; set; }
        public bool ANE_DETRACCION { get; set; }
        public decimal ANE_TASA_DETRACC { get; set; }
        public string COD_DETRAC { get; set; }
        public string ANEX_NOMBRE_2 { get; set; }
        public string ANEX_NACIONALIDAD { get; set; }
        public bool ANEX_SEXO { get; set; }
        public string TCL_CODIGO { get; set; }
        public string ANEX_GLOSA { get; set; }
        public string ANEX_EST_CODIGO { get; set; }
        public string ANEX_COND_CODIGO { get; set; }
        public DateTime? FECHA_NACIMIENTO { get; set; }
        public string COD_AFP_ONP { get; set; }
        public string ANEX_TIPO_COMISION { get; set; }
        public string COD_DOBLE_TRIB { get; set; }
        public string ANEX_DISTRITO { get; set; }
        public string ANEX_BENE_CTA { get; set; }
        public string ANEX_BENE_METODOENRUTA { get; set; }
        public string ANEX_BENE_CODIGOENRUTA { get; set; }
        public string ANEX_BENE_NOMBRE { get; set; }
        public string ANEX_BENE_DIRECCION1 { get; set; }
        public string ANEX_BENE_DIRECCION2 { get; set; }
        public string ANEX_INTERME_METODOENRUTA { get; set; }
        public string ANEX_INTERME_CODIGOENRUTA { get; set; }
        public string ANEX_INTERME_NOMBRE { get; set; }
        public string ANEX_INTERME_DIRECCION1 { get; set; }
        public string ANEX_INTERME_DIRECCION2 { get; set; }
        public string ANEX_CORREO { get; set; }
        public string ANEX_UBIGEO { get; set; }
        public string TIPO_PROVEEDOR { get; set; }
        public bool ANEX_PORTAL_PROVEEDOR { get; set; }
    }
}