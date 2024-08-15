using Core.DTO.Contabilidad.Poliza;
using Core.Entity.Administrativo.Contabilidad.Poliza;
using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad.Poliza
{
    public interface IConversionPolizaDAO
    {
        Dictionary<string, object> CargarPolizas(int poliza, int year, int mes);
        Dictionary<string, object> BuscarCaptura(DateTime fechaConversion);
        Dictionary<string, object> ObtenerPolizaDetalle(int year, int mes, string tp, int poliza, int empresa);
        Dictionary<string, object> ObtenerPolizaDetallePeru(int year, int mes, string tp, int poliza, int empresa);
        Dictionary<string, object> CargarConversionPolizas(int year, int mes, string tp, int poliza, decimal monto);
        Dictionary<string, object> saveOrUpdatePoliza(List<tblC_SC_ConversionPoliza> listaPolizas, DateTime fechaConversion, bool aplicaFechaPoliza);
        Dictionary<string, object> ObtenerPolizaEnkontrol(PolizasDTO poliza, EmpresaEnum empresa);
    }

}
