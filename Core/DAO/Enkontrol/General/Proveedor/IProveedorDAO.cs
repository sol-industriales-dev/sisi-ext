using Core.DTO.Enkontrol.Tablas.Proveedor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Enkontrol.General.Proveedor
{
    public interface IProveedorDAO
    {
        List<sp_proveedoresDTO> GetProveedores();
    }
}
