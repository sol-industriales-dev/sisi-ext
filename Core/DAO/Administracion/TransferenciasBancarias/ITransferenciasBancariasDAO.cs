using Core.DTO.Administracion.TransferenciasBancarias;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Core.DAO.Administracion.TransferenciasBancarias
{
    public interface ITransferenciasBancariasDAO
    {
        Dictionary<string, object> CargarMovimientosProveedorAutorizados(int proveedorInicial, int proveedorFinal, DateTime fechaInicial, DateTime fechaFinal);
        Tuple<Stream, string> CargarArchivoComprimido(List<RegistroArchivoDTO> registros);
        Dictionary<string, object> GenerarCheques(List<FacturaDTO> facturas, int cuentaBancaria);
        Dictionary<string, object> FillComboCuentasBancarias();
    }
}
