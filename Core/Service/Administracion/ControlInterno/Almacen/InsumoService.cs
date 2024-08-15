using Core.DAO.Administracion.ControlInterno.Almacen;
using Core.DTO.Administracion;
using Core.DTO.Administracion.ControlInterno;
using Core.DTO.Principal.Generales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Administracion.ControlInterno.Almacen
{
    public class InsumoService : IinsumosDAO
    {
        #region Atributos
        private IinsumosDAO m_interfazDAO;
        #endregion

        #region Propiedades
        private IinsumosDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion

        #region Constructores
        public InsumoService(IinsumosDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion

        public List<insumosDTO> getListaInsumo(int insumoID, int anio)
        {
            return interfazDAO.getListaInsumo(insumoID, anio);
        }
        public List<detInsumoDTO> getInsumo(string term, int TipoInsumo, int GrupoInsumo, int sistema)
        {
            return interfazDAO.getInsumo(term, TipoInsumo, GrupoInsumo, sistema);
        }
        public List<ComboDTO> fillGrupoInsumos(int tipo, int sistema)
        {
            return interfazDAO.fillGrupoInsumos(tipo, sistema);
        }
        public List<ComboDTO> fillTipoInsumos(int sistema)
        {
            return interfazDAO.fillTipoInsumos(sistema);
        }
        public List<detInsumoDTO> getListaInsumos(string term, int TipoInsumo, int GrupoInsumo, int sistema)
        {
            return interfazDAO.getListaInsumos(term, TipoInsumo, GrupoInsumo, sistema);
        }

        public List<insumosDTO> getInsumo(int insumo, int anio, int tipo)
        {
            return interfazDAO.getInsumo(insumo, anio, tipo);
        }
        public List<insumosDTO> getInsumoMultiple(List<int> insumos, int anio, int tipo,string almacen)
        {
            return interfazDAO.getInsumoMultiple(insumos,anio,tipo,almacen);
        }
        public ComboDTO getInsumoTipoGrupoByID(int idInsumo)
        {
            return interfazDAO.getInsumoTipoGrupoByID(idInsumo);
        }
    }
}
