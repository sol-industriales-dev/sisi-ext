using Core.DAO.RecursosHumanos.Captura;
using Core.DTO.Principal.Generales;
using Core.DTO.RecursosHumanos;
using Core.Entity.Principal.Usuarios;
using Core.Entity.RecursosHumanos.Captura;
using Core.Entity.RecursosHumanos.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.RecursosHumanos.Captura
{
    public class FiniquitoService : IFiniquito
    {
        #region Atributos
        private IFiniquito m_Finiquito;
        #endregion
        #region Propiedades
        public IFiniquito finiquito
        {
            get { return m_Finiquito; }
            set { m_Finiquito = value; }
        }
        #endregion

        public FiniquitoService(IFiniquito Finiquito)
        {
            this.finiquito = Finiquito;
        }

        tblRH_Finiquito IFiniquito.getEmpleadoForId(int id)
        {
            return finiquito.getEmpleadoForId(id);
        }

        List<ComboDTO> IFiniquito.getListaConceptos()
        {
            return finiquito.getListaConceptos();
        }

        tblRH_Finiquito IFiniquito.GuardarFiniquito(tblRH_Finiquito general, List<tblRH_FiniquitoDetalle> detalle)
        {
            return finiquito.GuardarFiniquito(general, detalle);
        }

        List<FiniquitoDTO> IFiniquito.getFiniquitos(int clave, string nombre, string cc, int aut)
        {
            return finiquito.getFiniquitos(clave, nombre, cc, aut);
        }

        FiniquitoDTO IFiniquito.GetDetalleFin(int id)
        {
            return finiquito.GetDetalleFin(id);
        }

        List<FiniquitoFirmasDTO> IFiniquito.GetAutorizaciones(int finiquitoID)
        {
            return finiquito.GetAutorizaciones(finiquitoID);
        }

        tblRH_Finiquito IFiniquito.AutorizaFiniquito(int aut, int finiquitoID)
        {
            return finiquito.AutorizaFiniquito(aut, finiquitoID);
        }

        tblP_Usuario IFiniquito.getUsuario(int id)
        {
            return finiquito.getUsuario(id);
        }

        List<tblRH_FiniquitoConceptos> IFiniquito.getConceptos()
        {
            return finiquito.getConceptos();
        }

        List<tblRH_CatEmpleados> IFiniquito.getEmpleadosTodos(string term)
        {
            return finiquito.getEmpleadosTodos(term);
        }

        tblRH_FiniquitoConceptos IFiniquito.GetDetalleConcepto(int id)
        {
            return finiquito.GetDetalleConcepto(id);
        }

        void IFiniquito.GuardarConcepto(string concepto, string detalle, bool operador)
        {
            finiquito.GuardarConcepto(concepto, detalle, operador);
        }

        void IFiniquito.UpdateConcepto(int id, string concepto, string detalle, bool operador)
        {
            finiquito.UpdateConcepto(id, concepto, detalle, operador);
        }

        void IFiniquito.RemoveConcepto(int id)
        {
            finiquito.RemoveConcepto(id);
        }

        decimal IFiniquito.GetPrimaAntiguedad(string ingreso, string egreso)
        {
            return finiquito.GetPrimaAntiguedad(ingreso, egreso);
        }
    }
}
