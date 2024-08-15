using Core.DAO.Principal.Alertas;
using Core.DTO;
using Core.Entity.Principal.Alertas;
using Core.Enum.Multiempresa;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.DAO.Principal.Alertas
{
    public class AlertaMantenimientoDAO : GenericDAO<tblP_AlertaMantenimiento>, IAlertaMantenimientoDAO
    {
        private readonly Dictionary<string, object> _resultado = new Dictionary<string, object>();
        private const string RedirectURL = "/Usuario/Login";

        public AlertaMantenimientoDAO()
        {
            _resultado.Clear();
        }

        public Dictionary<string, object> VerificarAlertaMantenimiento()
        {
            try
            {
                // Si está en local no verifica el estatus de la alarma
                if (vSesiones.sesionBestRouting == 1)
                {
                    _resultado.Add("activo", false);
                    _resultado.Add(SUCCESS, true);
                    return _resultado;
                }
                else
                {
                    var contextConstruplan = new MainContext((int)EmpresaEnum.Construplan);
                    var alarma = contextConstruplan.tblP_AlertaMantenimiento.First();

                    if (alarma.activo)
                    {
                        AgregarInformacionMantenimiento(alarma);
                    }

                    _resultado.Add("activo", alarma.activo);
                    _resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception)
            {
                _resultado.Add(SUCCESS, true);
            }

            return _resultado;
        }

        private void AgregarInformacionMantenimiento(tblP_AlertaMantenimiento alarma)
        {
            TimeSpan diferencia = alarma.fechaProgramada - DateTime.Now;

            if (diferencia.TotalSeconds > 10)
            {
                var tiempoRestante = String.Format("Se publicará en: {0} minutos", diferencia.ToString(@"mm\:ss"));

                _resultado.Add("tiempoRestante", tiempoRestante);
            }
            else
            {
                _resultado.Add("redirectURL", RedirectURL);
            }

            _resultado.Add("vencido", diferencia.TotalSeconds < 10);
            _resultado.Add("mensajeAlerta", alarma.mensaje ?? "Mantenimiento del servidor.");
        }
    }
}
