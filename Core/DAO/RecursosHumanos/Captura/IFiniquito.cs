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

namespace Core.DAO.RecursosHumanos.Captura
{
    public interface IFiniquito
    {
        tblRH_Finiquito getEmpleadoForId(int idEmpleado);
        List<ComboDTO> getListaConceptos();
        tblRH_Finiquito GuardarFiniquito(tblRH_Finiquito general, List<tblRH_FiniquitoDetalle> detalle);
        List<FiniquitoDTO> getFiniquitos(int clave, string nombre, string cc, int aut);
        FiniquitoDTO GetDetalleFin(int id);
        List<FiniquitoFirmasDTO> GetAutorizaciones(int finiquitoID);
        tblRH_Finiquito AutorizaFiniquito(int aut, int finiquitoID);
        tblP_Usuario getUsuario(int id);
        List<tblRH_FiniquitoConceptos> getConceptos();
        List<tblRH_CatEmpleados> getEmpleadosTodos(string term);
        tblRH_FiniquitoConceptos GetDetalleConcepto(int id);
        void GuardarConcepto(string concepto, string detalle, bool operador);
        void UpdateConcepto(int id, string concepto, string detalle, bool operador);
        void RemoveConcepto(int id);
        decimal GetPrimaAntiguedad(string ingreso, string egreso);
    }
}
