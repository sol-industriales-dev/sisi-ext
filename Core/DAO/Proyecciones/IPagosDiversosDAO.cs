using Core.DTO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Proyecciones
{
    public interface IPagosDiversosDAO
    {
        tblPro_PagosDiversos GetJsonData(FiltrosGeneralDTO filtro);
        void GuardarActualizarPagosDiversos(FiltrosGeneralDTO objFiltro, PagosDivDTO obj);
        void GuardarFormExcel(tblPro_PagosDiversos obj);
        MesDTO getLN4(MesDTO ln2, MesDTO ln4, FiltrosGeneralDTO objFiltro, int? col);
        MesDTO getLN6(MesDTO ln5, MesDTO ln6, FiltrosGeneralDTO objFiltro, int? col);
        MesDTO getLN7(decimal valor, FiltrosGeneralDTO objFiltro, int? col);
        MesDTO getLN13(MesDTO ln12, MesDTO ln13, FiltrosGeneralDTO objFiltro, int? col);
    }
}
