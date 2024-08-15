using Core.DAO.Enkontrol.Almacen;
using Core.DTO.Enkontrol.Alamcen;
using Core.DTO.RecursosHumanos;
using Core.Entity.RecursosHumanos.Catalogo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Enkontrol.Resguardo
{
    public class ResguardoService : IResguardoDAO
    {
        public IResguardoDAO iResguardoDAO;
        public IResguardoDAO resguardoDAO 
        {
            get { return iResguardoDAO; }
            set { iResguardoDAO = value; }
        }
        public ResguardoService(IResguardoDAO resDAO)
        {
            this.resguardoDAO = resDAO;
        }

        public Dictionary<string, object> cambiarCCoEmpleado(int numEmpleado, int claveEmpleado, string ccNuevo, List<ResguardoEKDTO> resguardos)
        {
            return resguardoDAO.cambiarCCoEmpleado(numEmpleado, claveEmpleado, ccNuevo, resguardos);
        }

        public Dictionary<string, object> guardarAsignacionNormal(List<ResguardoEKDTO> resguardos)
        {
            return resguardoDAO.guardarAsignacionNormal(resguardos);
        }

        public List<ResguardoEKDTO> getResguardoReporte(string cc, int folio)
        {
            return resguardoDAO.getResguardoReporte(cc, folio);
        }

        public Dictionary<string, object> GetResguardo(string cc, int almacen, int folio)
        {
            return resguardoDAO.GetResguardo(cc, almacen, folio);
        }

        public Dictionary<string, object> guardarDevolucionNormal(List<ResguardoEKDTO> resguardos)
        {
            return resguardoDAO.guardarDevolucionNormal(resguardos);
        }

        public Dictionary<string, object> getEmpleados(int sesionEmpresaActual)
        {
            return resguardoDAO.getEmpleados(sesionEmpresaActual);
        }

        public Dictionary<string, object> guardarNuevoEmpleado(EmpleadoResguardoDTO empleado)
        {
            return resguardoDAO.guardarNuevoEmpleado(empleado);
        }

        public Dictionary<string, object> getUltimoFolio(string cc, int alm_salida)
        {
            return resguardoDAO.getUltimoFolio(cc, alm_salida);
        }

        public EmpleadoPuestoDTO getEmpleadoNomina(int claveEmpleado)
        {
            return resguardoDAO.getEmpleadoNomina(claveEmpleado);
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboAlmacenVirtual()
        {
            return resguardoDAO.FillComboAlmacenVirtual();
        }

        public List<tblRH_CatEmpleados> getEmpleadosAutoComplete(string term)
        {
            return resguardoDAO.getEmpleadosAutoComplete(term);
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcTodosExistentes()
        {
            return resguardoDAO.FillComboCcTodosExistentes();
        }

        public Tuple<MemoryStream, string> descargarExcelUsuariosEnkontrolNoCoinciden()
        {
            return resguardoDAO.descargarExcelUsuariosEnkontrolNoCoinciden();
        }

        public Dictionary<string, object> cargarSesionReporteBitacoraResguardos(string centroCostoInicio, string centroCostoFin, int empleadoInicio, int empleadoFin, List<string> listaEstatus, List<string> listaNumeroSerie)
        {
            return resguardoDAO.cargarSesionReporteBitacoraResguardos(centroCostoInicio, centroCostoFin, empleadoInicio, empleadoFin, listaEstatus, listaNumeroSerie);
        }
        public List<rptResguardoDTO> cargarSesionReporteBitacoraResguardosCrystal(string centroCostoInicio, string centroCostoFin, int empleadoInicio, int empleadoFin, List<string> listaEstatus, List<string> listaNumeroSerie)
        {
            return resguardoDAO.cargarSesionReporteBitacoraResguardosCrystal(centroCostoInicio, centroCostoFin, empleadoInicio, empleadoFin, listaEstatus, listaNumeroSerie);
        }
        
        public Dictionary<string,object> getCentrosCostos(int sesionEmpresaActual){
            return resguardoDAO.getCentrosCostos(sesionEmpresaActual);
        }
    }
}
