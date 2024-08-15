using Core.DAO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Captura.aceites;
using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Capturas
{
    public class MaquinariaAceitesLubricantesService : IMaquinariaAceitesLubricantesDAO
    {
        #region Atributos
        private IMaquinariaAceitesLubricantesDAO m_AceiteLubicante;
        #endregion
        #region Propiedades
        public IMaquinariaAceitesLubricantesDAO MaquinariaAceitesLubricantesDAO
        {
            get { return m_AceiteLubicante; }
            set { m_AceiteLubicante = value; }
        }
        #endregion
        #region Constructores
        public MaquinariaAceitesLubricantesService(IMaquinariaAceitesLubricantesDAO maquinariaAceitesLubricantesDAO)
        {
            this.MaquinariaAceitesLubricantesDAO = maquinariaAceitesLubricantesDAO;
        }
        #endregion
        public tblM_MaquinariaAceitesLubricantes GuardarMaqAceiteLubricante(tblM_MaquinariaAceitesLubricantes obj){
            return MaquinariaAceitesLubricantesDAO.GuardarMaqAceiteLubricante(obj);
        }
        public List<MaquinariaAceitesLubricantesDTO> GetLstMaqAceiteLubricante(string cc, string consumo, int turno, DateTime fecha, int tipo)
        {
            return MaquinariaAceitesLubricantesDAO.GetLstMaqAceiteLubricante(cc, consumo, turno, fecha, tipo);
        }

        public List<tblM_MaquinariaAceitesLubricantes> GetRepMaqAceiteLubricante(string cc, int turno, DateTime inicio, DateTime fin, string economico)
        {
            return MaquinariaAceitesLubricantesDAO.GetRepMaqAceiteLubricante(cc, turno, inicio, fin, economico);
        }
     
    }
}
