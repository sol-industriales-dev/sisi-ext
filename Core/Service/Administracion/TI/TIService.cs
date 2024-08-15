using Core.DAO.Administracion.TI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Administracion.TI
{
    public class TIService : ITIDAO
    {
        #region INIT
        public ITIDAO r_TI { get; set; }
        public ITIDAO TI
        {
            get { return r_TI; }
            set { r_TI = value; }
        }
        public TIService(ITIDAO TI)
        {
            this.TI = TI;
        }
        #endregion
    }
}
