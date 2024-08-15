using Core.DAO.ReportesContabilidad;
using Core.Service.ReportesContabilidad;
using Data.DAO.ReportesContabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Enum.Multiempresa;
using Core.DTO;

namespace Data.Factory.ReportesContabilidad
{
    public class ReportesContabilidadFactoryService
    {
        public IReportesContabilidadDAO getReportesContabilidad()
        {
            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
            {
                case EmpresaEnum.Construplan: return new ReportesContabilidadServices(new ReportesContabilidadConstruplanDAO());
                default: return new ReportesContabilidadServices(new ReportesContabilidadArrendadoraDAO());
            }
        }
    }
}
