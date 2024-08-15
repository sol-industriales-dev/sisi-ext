using Core.DAO.Maquinaria.Captura;
using Core.Entity.Maquinaria.Captura;
using Core.Enum.Maquinaria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Capturas
{
    public class AceitesLubricantesService : IAceitesLubricantesDAO
    {
        #region Atributos
        private IAceitesLubricantesDAO m_AceitesLubricantesDAO;
        #endregion
        #region Propiedades
        public IAceitesLubricantesDAO AceitesLubricantesDAO
        {
            get { return m_AceitesLubricantesDAO; }
            set { m_AceitesLubricantesDAO = value; }
        }
        #endregion
        #region Constructor
        public AceitesLubricantesService(IAceitesLubricantesDAO aceitesLubricantesDAO)
        {
            this.AceitesLubricantesDAO = aceitesLubricantesDAO;
        }
        #endregion

        public List<tblM_CatAceitesLubricantes> GetAllAceitesLubricantes(int tipoId, string economico)
        {
            return this.AceitesLubricantesDAO.GetAllAceitesLubricantes(tipoId, economico);
        }

        public  object ExistenciaLubricante(string almacen)
        {
            return this.AceitesLubricantesDAO.ExistenciaLubricante(almacen);
        }

        #region Catálogo Lubricantes
        public Dictionary<string, object> CargarCatalogoLubricantes()
        {
            return this.AceitesLubricantesDAO.CargarCatalogoLubricantes();
        }

        public Dictionary<string, object> GetComboModelos()
        {
            return this.AceitesLubricantesDAO.GetComboModelos();
        }

        public Dictionary<string, object> GuardarNuevoLubricante(tblM_CatAceitesLubricantes lubricante)
        {
            return this.AceitesLubricantesDAO.GuardarNuevoLubricante(lubricante);
        }

        public Dictionary<string, object> EditarLubricante(tblM_CatAceitesLubricantes lubricante, AceiteLubricanteEnum subConjuntoID_Anterior)
        {
            return this.AceitesLubricantesDAO.EditarLubricante(lubricante, subConjuntoID_Anterior);
        }

        public Dictionary<string, object> EliminarLubricante(tblM_CatAceitesLubricantes lubricante)
        {
            return this.AceitesLubricantesDAO.EliminarLubricante(lubricante);
        }
        #endregion
    }
}
