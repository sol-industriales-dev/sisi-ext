using Core.DAO.RecursosHumanos.Reportes;
using Core.DTO.RecursosHumanos;
using Core.Entity.RecursosHumanos.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.RecursosHumanos.Reportes
{
    public class IncidenciasReporteService : IIncidencias
    {
        private IIncidencias m_Incidencias;

        public IIncidencias Incidencias
        {
            get { return m_Incidencias; }
            set { m_Incidencias = value; }
        }

        public IncidenciasReporteService(IIncidencias Incidencias)
        {
            this.Incidencias = Incidencias;
        }

        public List<IncidenciasDTO> getLstIncidencias(IncidenciasDTO objBuscar)
        {
            return this.Incidencias.getLstIncidencias(objBuscar);
        }

        public List<IncidenciasDTO> getLstIncidencias2Fechas(IncidenciasDTO objBuscar)
        {
            return this.Incidencias.getLstIncidencias2Fechas(objBuscar);
        }


        public List<int> CatAnios()
        {
            return this.Incidencias.CatAnios();
        }


        public List<CatIncidencias> CatIncidencia()
        {
            return this.Incidencias.CatIncidencia();
        }


        public List<string> CatPeriodo(int anio, string cc)
        {
            return this.Incidencias.CatPeriodo(anio, cc);
        }

        public List<int> CatDias()
        {
            return this.Incidencias.CatDias();
        }
    }
}
