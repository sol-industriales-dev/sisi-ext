using Core.DTO.RecursosHumanos;
using Core.Entity.RecursosHumanos.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.RecursosHumanos.Reportes
{
    public interface IIncidencias
    {
        List<IncidenciasDTO> getLstIncidencias(IncidenciasDTO objBuscar);
        List<IncidenciasDTO> getLstIncidencias2Fechas(IncidenciasDTO objBuscar);
        List<int> CatAnios();
        List<CatIncidencias> CatIncidencia();
        List<string> CatPeriodo(int anio, string cc);
        List<int> CatDias();
    }
}
