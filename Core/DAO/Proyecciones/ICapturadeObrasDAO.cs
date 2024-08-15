using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Proyecciones
{
    public interface ICapturadeObrasDAO
    {
        tblPro_CapturadeObras GetJsonData(int escenario, int meses, int anio);
        void GuardarActualizarCapturadeObras(tblPro_CapturadeObras obj);
        Dictionary<string, object> getinfoCapturaObras(int Escenario, decimal divisor, int mes, int anio);
        List<tblPro_Obras> dataEscenarios(List<tblPro_Obras> listas, int escenario);
        int getUltimoMesCapturado();
        List<tblPro_CapturadeObras> FillCboObra();
        tblPro_CapturadeObras GetJsonDataID(int idData);
    }
}
