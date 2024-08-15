using Core.DTO;
using Core.Entity.Maquinaria.Catalogo;
using Data.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Catalogos
{
    public class AlmacenDAO
    {
        public IList<tblM_Almacen> FillGridAlmacen(tblM_Almacen obje)
        {
           
            if(vSesiones.sesionEmpresaActual==3)
            {
                return (IList<tblM_Almacen>)_contextEnkontrol.Where("SELECT almacen as almacen,descripcion as descripcion,direccion as direccion FROM DBA.si_almacen where descripcion like '%" + obje.descripcion + "%' AND direccion like '%" + obje.direccion + "%'").ToObject<IList<tblM_Almacen>>();
            }
            else
            {
                return (IList<tblM_Almacen>)_contextEnkontrol.Where("SELECT almacen as almacen,descripcion as descripcion,direccion as direccion FROM si_almacen where descripcion like '%" + obje.descripcion + "%' AND direccion like '%" + obje.direccion + "%'").ToObject<IList<tblM_Almacen>>();
            }
        }
    }
}
