using Core.DAO.Contabilidad.Reportes;
using Core.Entity.Administrativo.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Contabilidad.Reportes
{
    public class CatNumNafinServices : ICatNumNafinDAO
    {
        #region Atributos
        private ICatNumNafinDAO m_nafinDAO { get; set; }
        #endregion
        #region Propiedades
        public ICatNumNafinDAO NafinDAO
        {
            get { return m_nafinDAO; }
            set { m_nafinDAO = value; }
        }
        #endregion
        #region Constructor
        public CatNumNafinServices(ICatNumNafinDAO nafinDAO)
        {
            this.NafinDAO = nafinDAO;
        }
        #endregion
        public void Guardar(tblC_CatNumNafin obj)
        {
            this.NafinDAO.Guardar(obj);
        }
        public bool GuardarLstProvNafin(List<tblC_CatNumNafin> lst)
        {
            return NafinDAO.GuardarLstProvNafin(lst);
        }
        public List<tblC_CatNumNafin> GetLstHanilitadosNumNafin()
        {
            return this.NafinDAO.GetLstHanilitadosNumNafin();
        }
        public List<object> GetLstNafin()
        {
            return this.NafinDAO.GetLstNafin();
        }
        public List<tblC_CatNumNafin> GetLstNafin(int moneda)
        {
            return NafinDAO.GetLstNafin(moneda);
        }
        public bool eliminarNafinProv(tblC_CatNumNafin obj)
        {
            return NafinDAO.eliminarNafinProv(obj);
        }
    }
}
