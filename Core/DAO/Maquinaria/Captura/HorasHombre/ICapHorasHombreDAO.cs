using Core.DTO.Maquinaria.Captura.OT;
using Core.DTO.Maquinaria.Captura.OT.rptConcentradoHH;
using Core.DTO.Principal.Generales;
using Core.DTO.RecursosHumanos;
using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Captura.HorasHombre
{
    public interface ICapHorasHombreDAO
    {
        void setRptMaquinariaHorasHombre(List<int> economicos, DateTime fechaInicio, DateTime fechaFin, List<int> Puestos, List<int> Empleados, string cc);
        List<DistribucionHHPersonalDTO> rptGeneralDistribucion(List<string> cc, DateTime fechaInicio, DateTime fechaFin, List<int> puestos, List<int> empleados);
        List<ComboDTO> getListaPersonalPuestos(List<int> puestos, List<string> ccs);
        List<ComboDTO> getListaPuestos();
        List<ComboDTO> fillCboCC();
        Dictionary<string, object> FillComboDepartamentos(string cc);
        GraficaParetoDTO getParetoCategorias(List<string> ccs, DateTime fechaInicio, DateTime fechaFin, List<int> listaCategorias, List<int> listaSubCategoria);
        List<CapDistribucionHHDTO> loadDataCapHorasHombre(string cc, DateTime fecha, int turno);
        List<tblM_CatCategoriasHH> getCategorias();
        List<tblM_CatSubCategoriasHH> getSubCategorias(List<int> listCategorias);
        Dictionary<string, object> getSubCategorias();
        void guardarInformacion(List<tblM_CapHorasHombre> array);
        List<GeneralConcentradoHHDTO> getConcentradoGeneral(List<string> cc, DateTime fechaInicio, DateTime fechaFin, List<int> listaCategorias, List<int> subCategoriasHH);
        List<DistribucionHHDTO> getDistribucionGeneral(List<string> cc, DateTime fechaInicio, DateTime fechaFin, List<int> listaCategorias, List<int> subCategoriasHH);
        List<ComboDTO> fillCboPuestos();
        List<ComboDTO> getNombreEmpleado(string term, int puesto);
        ComboDTO searchNumEmpleado(string term, int puesto,int numEmpleado);
        
        List<DistribucionHHPersonalDTO> DetalleDistribucionGeneral(List<string> cc, DateTime fechaInicio, DateTime fechaFin, int puestoID);
        List<EmpleadoConcentradoGeneralDTO> getInfoByEmpleado(List<string> cc, DateTime fechaInicio, DateTime fechaFin, int numEmpleado);
        PuestosDTO getinfoEmpleadoGeneral(int numEmpleado);

        List<rptUtilizacionDTO> getDetalleConcentradoGeneral(List<string> cc, DateTime fechaInicio, DateTime fechaFin, int puestoID);

        List<DistribucionHHPersonalDTO> rptGeneralCCPorPuesto(List<string> cc, DateTime fechaInicio, DateTime fechaFin, List<int> puestoID);
        List<newCapHorasHombreDTO> loadTblHorasHombre(string cc, int clave_depto, DateTime fecha, int turno,int usuarioActual, int puesto, int personal);
    }
}
