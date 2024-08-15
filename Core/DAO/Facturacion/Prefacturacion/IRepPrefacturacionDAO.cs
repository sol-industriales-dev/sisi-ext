using Core.DTO.Facturacion;
using Core.Entity.Facturacion.Prefacturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Facturacion.Prefacturacion
{
    public interface IRepPrefacturacionDAO
    {
        tblF_RepPrefactura saveRepPrefactura(tblF_RepPrefactura obj);
        tblF_RepPrefactura ActualizaEstatus(int id, int estatus);
        tblF_RepPrefactura ActualizaEstatusCLONE(int id, int estatus);
        List<tblF_RepPrefactura> getPrefactura(DateTime inicio, DateTime fin, string cc);
        List<tblF_RepPrefactura> getPrefactura(int id);
        List<tblF_RepPrefactura> getPrefactura();
        tblF_RepPrefactura getUltimaPrefacturaCliente(string nombre, string moneda, string cc);
        List<ComboDTO> FillComboClienteNombre(string term);
        ClienteDTO objCliente(int cliente);
    }
}
