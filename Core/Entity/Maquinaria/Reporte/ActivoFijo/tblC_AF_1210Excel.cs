using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo
{
    public class tblC_AF_1210Excel
    {
        public int Id { get; set; }
        public int Cuenta { get; set; }
        public int? Subcuenta { get; set; }
        public int? SubSubcuenta { get; set; }
        public string FechaAlta { get; set; }
        public string FechaInicioDep { get; set; }
        public string Factura { get; set; }
        public string CC { get; set; }
        public string Clave { get; set; }
        public string Descripcion { get; set; }
        public string TipoActivo { get; set; }
        public string Tipo { get; set; }
        public decimal? MOI { get; set; }
        public decimal? AltasEquipo { get; set; }
        public decimal? Componentes { get; set; }
        public string FechaBaja { get; set; }
        public string PolizaBaja { get; set; }
        public decimal? MontoBaja { get; set; }
        public decimal? PorcentajeDep { get; set; }
        public int? MesesTotalesDep { get; set; }
    }

    public class tblC_AF_1210ExcelFaltantes
    {
        public int Id { get; set; }
        public int Cuenta { get; set; }
        public int NumeroRenglonExcel { get; set; }
        public string NumeroEconomico { get; set; }
        public string Motivo { get; set; }
    }
}