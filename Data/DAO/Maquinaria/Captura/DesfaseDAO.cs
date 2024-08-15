using Core.DAO.Maquinaria.Captura;
using Core.DTO;
using Core.DTO.Captura;
using Core.Entity.Maquinaria;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.Maquinaria.Catalogo;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Captura
{
    public class DesfaseDAO : GenericDAO<tblM_CapDesfase>, ICapturaDesfaseDAO
    {
        public IList<economicoDTO> getEconomicos(int cc)
        {
            string centro_costos = "SELECT descripcion FROM si_area_cuenta WHERE centro_costo = '" + cc + "' and cc_activo = 1 ;;";

            return (IList<economicoDTO>)_contextEnkontrol.Where(centro_costos).ToObject<IList<economicoDTO>>();
        }




        public void Guardar(tblM_CapDesfase obj)
        {
            //if (!Exists(obj))
            //{
            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.DESFASE);
            else
                Update(obj, obj.id, (int)BitacoraEnum.DESFASE);
            //}
            //else
            //{
            //    if (obj.id == 0)
            //        throw new Exception("Se encuentra un desfase pendiente por aplicar");
            //    else
            //        Update(obj, obj.id, (int)BitacoraEnum.MAQUINA);
            //}
        }
        public bool Exists(tblM_CapDesfase obj)
        {
            return _context.tblM_CapDesfase.Where(x => x.Economico == obj.Economico &&
                                        x.estado == true).ToList().Count > 0 ? true : false;
        }

        public tblM_CapDesfase getDesfase(string economico)
        {
            if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual == 2)
            {
                return _context.tblM_CapDesfase.Where(x => x.Economico == economico).OrderByDescending(x => x.horasDesfaseAcumulado).FirstOrDefault();
            }
            else
            {
                return _context.tblM_CapDesfase.Where(x => x.Economico == economico).FirstOrDefault();
            }

            
        }


    }
}
