using Core.DAO.Maquinaria.Catalogos;
using Core.DTO.Maquinaria.Catalogos;
using Core.DTO.Principal.Generales;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Principal.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Catalogos
{
    public class CentroCostosService : ICentroCostosDAO
    {
        #region Atributos
        private ICentroCostosDAO m_centroCostosDAO;
        #endregion

        #region Propiedades
        public ICentroCostosDAO CentroCostosDAO
        {
            get { return m_centroCostosDAO; }
            set { m_centroCostosDAO = value; }
        }
        #endregion
        #region Constructores
        public CentroCostosService(ICentroCostosDAO centroCostosDAO)
        {
            this.CentroCostosDAO = centroCostosDAO;
        }
        #endregion
        public IList<tblM_CentroCostos> FillGridCC(tblM_CentroCostos cc)
        {
            return CentroCostosDAO.FillGridCC(cc);
        }

        public IList<InventarioDTO> fillGridMaquinaria(int cc, int idGrupo)
        {
            return CentroCostosDAO.fillGridMaquinaria(cc, idGrupo);
        }
        public string getNombreCC(int cc)
        {
            return CentroCostosDAO.getNombreCC(cc);
        }
        public string getNombreCcFromSIGOPLAN(string centroCosto)
        {
            return CentroCostosDAO.getNombreCcFromSIGOPLAN(centroCosto);
        }
        public string getNombreCC(string cc)
        {
            return CentroCostosDAO.getNombreCC(cc);
        }

        public IList<InventarioDTO> fillListaMaquinaria(string grupo, string tipo, string modelo)
        {
            return CentroCostosDAO.fillListaMaquinaria(grupo, tipo, modelo);
        }

        public string getNombreCCFix(string centroCostos)
        {
            return CentroCostosDAO.getNombreCCFix(centroCostos);
        }

        public List<ComboDTO> getListaCC()
        {
            return CentroCostosDAO.getListaCC();

        }
        public List<ComboDTO> getListaCC_Rep_Costos()
        {
            return CentroCostosDAO.getListaCC_Rep_Costos();

        }
        
        public List<ComboDTO> ListCC()
        {
            return CentroCostosDAO.ListCC();
        }

        public string getNombreCCArrendadoraRH(string centroCostos)
        {
            return CentroCostosDAO.getNombreCCArrendadoraRH(centroCostos);
        }

        public tblP_CC getEntityCCConstruplan(int ccID)
        {
            return CentroCostosDAO.getEntityCCConstruplan(ccID);
        }

        public List<ComboDTO> getListaCCConstruplan()
        {
            return CentroCostosDAO.getListaCCConstruplan();
        }

        public List<ComboDTO> getListaCCSIGOPLAN()
        {
            return CentroCostosDAO.getListaCCSIGOPLAN();
        }
        public List<ComboDTO> getLstCcArrendadoraProd()
        {
            return CentroCostosDAO.getLstCcArrendadoraProd();
        }
       public string getNombreAreaCuent(string areaCuenta)
        {
            return CentroCostosDAO.getNombreAreaCuent(areaCuenta);
        }

    }
}
