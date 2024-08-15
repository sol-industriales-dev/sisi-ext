using Core.DAO.Administracion.TransferenciasBancarias;
using Core.DTO.Administracion.TransferenciasBancarias;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Core.Service.Administracion.TransferenciasBancarias
{
    public class TransferenciasBancariasService : ITransferenciasBancariasDAO
    {
        public ITransferenciasBancariasDAO transferenciasBancariasDAO;
        public ITransferenciasBancariasDAO TransferenciasBancariasDAO
        {
            get { return transferenciasBancariasDAO; }
            set { transferenciasBancariasDAO = value; }
        }
        public TransferenciasBancariasService(ITransferenciasBancariasDAO transferenciasBancariasDAO)
        {
            this.TransferenciasBancariasDAO = transferenciasBancariasDAO;
        }

        public Dictionary<string, object> CargarMovimientosProveedorAutorizados(int proveedorInicial, int proveedorFinal, DateTime fechaInicial, DateTime fechaFinal)
        {
            return transferenciasBancariasDAO.CargarMovimientosProveedorAutorizados(proveedorInicial, proveedorFinal, fechaInicial, fechaFinal);
        }

        public Tuple<Stream, string> CargarArchivoComprimido(List<RegistroArchivoDTO> registros)
        {
            return transferenciasBancariasDAO.CargarArchivoComprimido(registros);
        }

        public Dictionary<string, object> GenerarCheques(List<FacturaDTO> facturas, int cuentaBancaria)
        {
            return transferenciasBancariasDAO.GenerarCheques(facturas, cuentaBancaria);
        }

        public Dictionary<string, object> FillComboCuentasBancarias()
        {
            return transferenciasBancariasDAO.FillComboCuentasBancarias();
        }
    }
}
