using Core.DAO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Proyecciones
{
    public class TESTDATAServices : ITESTDATADAO
    {
                        #region Atributos
        private ITESTDATADAO m_TESTDATADAO;
        #endregion Atributos

        #region Propiedades
        private ITESTDATADAO tESTDATADAO
        {
            get { return m_TESTDATADAO; }
            set { m_TESTDATADAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public TESTDATAServices(ITESTDATADAO TESTDATADAO)
        {
            this.tESTDATADAO = TESTDATADAO;
        }
        #endregion Constructores

        public List<tbl_TESTDATA> getListaDATA(int tipo)
        {
            return tESTDATADAO.getListaDATA(tipo);
        }
    }
}
