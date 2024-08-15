using Core.Enum.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.SistemaContable
{
    public class MesProcResumenDTO
    {
        public SistemasEnkontrolEnum sistema { get; set; }
        public DateTime fecha { get; set; }
        public bool estado { get; set; }
        public int usuario { get; set; }
        public MesProcResumenDTO()
        {

        }
        public MesProcResumenDTO(SistemasEnkontrolEnum sis, MesProcDTO proceso)
        {
            fecha = new DateTime(proceso.year, proceso.mes, 1);
            sistema = sis;
            switch (sistema)
            {
                case SistemasEnkontrolEnum.General:
                    estado = proceso.st_validacion == "S";
                    break;
                case SistemasEnkontrolEnum.Contabilidad:
                    estado = proceso.sco == "S";
                    break;
                case SistemasEnkontrolEnum.Bancos:
                    estado = proceso.sbo == "S";
                    break;
                case SistemasEnkontrolEnum.Proveedores:
                    estado = proceso.scp == "S";
                    break;
                case SistemasEnkontrolEnum.Clientes:
                    estado = proceso.scx == "S";
                    break;
                case SistemasEnkontrolEnum.snd:
                    estado = proceso.snd == "S";
                    break;
                case SistemasEnkontrolEnum.Inventario:
                    estado = proceso.sin == "S";
                    break;
                case SistemasEnkontrolEnum.Compras:
                    estado = proceso.soc == "S";
                    break;
                case SistemasEnkontrolEnum.Facturacion:
                    estado = proceso.sfa == "S";
                    break;
                case SistemasEnkontrolEnum.Vivienda:
                    estado = proceso.scv == "S";
                    break;
                case SistemasEnkontrolEnum.sac:
                    estado = proceso.sac == "S";
                    break;
                case SistemasEnkontrolEnum.st_codigo_agrupador:
                    estado = proceso.st_codigo_agrupador == "S";
                    usuario = proceso.usuario_codigo_agrupador ?? 0;
                    break;
                case SistemasEnkontrolEnum.st_estatus_poliza:
                    estado = proceso.st_estatus_poliza == "S";
                    usuario = proceso.usuario_estatus_poliza ?? 0;
                    break;
                case SistemasEnkontrolEnum.st_saldo_mayor:
                    estado = proceso.st_saldo_mayor == "S";
                    usuario = proceso.usuario_saldo_mayor ?? 0;
                    break;
                case SistemasEnkontrolEnum.st_poliza_iva:
                    estado = proceso.st_poliza_iva == "S";
                    usuario = proceso.usuario_poliza_iva ?? 0;
                    break;
                case SistemasEnkontrolEnum.st_sbo:
                    estado = proceso.st_sbo == "S";
                    usuario = proceso.usuario_sbo ?? 0;
                    break;
                case SistemasEnkontrolEnum.st_scx:
                    estado = proceso.st_scx == "S";
                    break;
                case SistemasEnkontrolEnum.st_scp:
                    estado = proceso.st_scp == "S";
                    usuario = proceso.usuario_scp ?? 0;
                    break;
                case SistemasEnkontrolEnum.st_saldo_ini:
                    estado = proceso.st_saldo_ini == "S";
                    usuario = proceso.usuario_saldo_ini ?? 0;
                    break;
                case SistemasEnkontrolEnum.st_tipo_contable:
                    estado = proceso.st_tipo_contable == "S";
                    usuario = proceso.usuario_tipo_contable ?? 0;
                    break;
                case SistemasEnkontrolEnum.st_cta_cfdi:
                    estado = proceso.st_cta_cfdi == "S";
                    usuario = proceso.usuario_cta_cfdi ?? 0;
                    break;
                default:
                    estado = proceso.st_validacion == "S";
                    break;
            }
        }
    }
}
