using Core.DAO.Proyecciones;
using Core.DTO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Proyecciones
{
    public class PagosDiversosServices : IPagosDiversosDAO
    {
        #region Atributos
        private IPagosDiversosDAO m_interfazDAO;
        #endregion

        #region Propiedades
        private IPagosDiversosDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion

        #region Constructores
        public PagosDiversosServices(IPagosDiversosDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion

        public tblPro_PagosDiversos GetJsonData(FiltrosGeneralDTO filtro)
        {
            return interfazDAO.GetJsonData(filtro);
        }
        public void GuardarActualizarPagosDiversos(FiltrosGeneralDTO objFiltro, PagosDivDTO obj)
        {
            interfazDAO.GuardarActualizarPagosDiversos(objFiltro,obj);
        }

        public void GuardarFormExcel(tblPro_PagosDiversos obj)
        {
            interfazDAO.GuardarFormExcel(obj);
        }

        public MesDTO getLN4(MesDTO ln2, MesDTO ln4, FiltrosGeneralDTO objFiltro, int? col)
        {
            return interfazDAO.getLN4(ln2, ln4, objFiltro, col);
        }
        public MesDTO getLN6(MesDTO ln5, MesDTO ln6, FiltrosGeneralDTO objFiltro, int? col)
        {
            return interfazDAO.getLN6(ln5, ln6, objFiltro, col);
        }
        public MesDTO getLN7(decimal valor, FiltrosGeneralDTO objFiltro, int? col)
        {
            return interfazDAO.getLN7(valor, objFiltro,col);
        }
        public MesDTO getLN13(MesDTO ln12, MesDTO ln13, FiltrosGeneralDTO objFiltro, int? col)
        {
            return interfazDAO.getLN13(ln12, ln13, objFiltro, col);
        }
    }
}
