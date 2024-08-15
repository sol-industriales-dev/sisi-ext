using Core.DAO.Enkontrol.Compras;
using Core.DTO;
using Core.Enum.Multiempresa;
using Core.Service.Enkontrol.Compras;
using Data.DAO.Enkontrol.Compras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Enkontrol.Compras
{
    public class AltaProveedorFactoryService
    {
        public IAltaProveedorDAO GetAltaProveedorFactoryService()
        {
            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
            {
                case EmpresaEnum.Arrendadora:
                    return new AltaProveedorService(new AltaProveedorArrendadoraDAO());
                    break;
                case EmpresaEnum.Colombia:
                    return new AltaProveedorService(new AltaProveedorColombiaDAO());
                    break;
                default:
                    return new AltaProveedorService(new AltaProveedorDAO());
                    break;
            }
        }
    }
}
