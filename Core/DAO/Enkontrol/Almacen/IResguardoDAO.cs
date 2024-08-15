using Core.DTO.Enkontrol.Alamcen;
using Core.DTO.RecursosHumanos;
using Core.Entity.RecursosHumanos.Catalogo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Enkontrol.Almacen
{
    public interface IResguardoDAO
    {
        Dictionary<string, object> cambiarCCoEmpleado(int numEmpleado, int claveEmpleado, string ccNuevo, List<ResguardoEKDTO> resguardos);
        Dictionary<string, object> guardarAsignacionNormal(List<ResguardoEKDTO> resguardos);
        List<ResguardoEKDTO> getResguardoReporte(string cc, int folio);
        Dictionary<string, object> GetResguardo(string cc, int almacen, int folio);
        Dictionary<string, object> guardarDevolucionNormal(List<ResguardoEKDTO> resguardos);
        Dictionary<string, object> getEmpleados(int sesionEmpresaActual);
        Dictionary<string, object> guardarNuevoEmpleado(EmpleadoResguardoDTO empleado);
        Dictionary<string, object> getUltimoFolio(string cc, int alm_salida);
        EmpleadoPuestoDTO getEmpleadoNomina(int claveEmpleado);
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboAlmacenVirtual();
        List<tblRH_CatEmpleados> getEmpleadosAutoComplete(string term);
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcTodosExistentes();
        Tuple<MemoryStream, string> descargarExcelUsuariosEnkontrolNoCoinciden();
        Dictionary<string, object> cargarSesionReporteBitacoraResguardos(string centroCostoInicio, string centroCostoFin, int empleadoInicio, int empleadoFin, List<string> listaEstatus, List<string> listaNumeroSerie);
        List<rptResguardoDTO> cargarSesionReporteBitacoraResguardosCrystal(string centroCostoInicio, string centroCostoFin, int empleadoInicio, int empleadoFin, List<string> listaEstatus, List<string> listaNumeroSerie);
        
        Dictionary<string, object> getCentrosCostos(int sesionEmpresaActual);

    }
}
