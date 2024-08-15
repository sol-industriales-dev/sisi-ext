using Core.DAO.ControlObra.AdministradorProyectos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.ControlObra.AdministradorProyectos
{
    public class CGPService : ICGPDAO
    {
        private ICGPDAO ap_IcgpDAO;
        public ICGPDAO ICGPDAO
        {
            get { return ap_IcgpDAO; }
            set { ap_IcgpDAO = value; }
        }
        public CGPService(ICGPDAO ICGPDAO)
        {
            this.ICGPDAO = ICGPDAO;
        }
        public string RutaArchivoDeLaVistaId(int vistaID)
        {
            return ICGPDAO.RutaArchivoDeLaVistaId(vistaID);
        }
    }
}
