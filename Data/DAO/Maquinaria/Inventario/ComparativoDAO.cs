using Core.DAO.Maquinaria.Inventario;
using Core.DTO.Maquinaria.Inventario;
using Core.DTO.Maquinaria.Inventario.Comparativos;
using Core.Entity.Maquinaria.Inventario;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Utils;
using Core.Enum.Principal;
using Core.Entity.Principal.Alertas;
using Core.Enum.Principal.Bitacoras;
using Core.Entity.Principal.Usuarios;
using Core.DTO.Principal.Generales;
using Microsoft.VisualBasic;
using System.Web;
using System.IO;
using Core.DTO.Maquinaria;
using Core.DTO;

namespace Data.DAO.Maquinaria.Inventario
{
    public class ComparativoDAO : GenericDAO<tblM_ComparativoAdquisicionyRenta>, IComparativoDAO
    {
        string Controller = "CatMaquinaController";

        private readonly string RutaBase = @"\\REPOSITORIO\Proyecto\SIGOPLAN\MAQUINARIA";
        private const string RutaLocal = @"C:\Proyecto\SIGOPLAN\MAQUINARIA";

        private readonly string rutaArchivos;

        public ComparativoDAO()
        {
#if DEBUG
            RutaBase = RutaLocal;
#endif

            rutaArchivos = Path.Combine(RutaBase, "CATALOGO");
        }
        private Dictionary<string, object> resultado;
        public List<ComparativoDTO> getTablaComparativoAdquisicion(ComparativoDTO objFiltro)
        {
            resultado = new Dictionary<string, object>();

            try
            {
                tblM_ComparativoAdquisicionyRenta objRenta = null;
                tblM_ComparativoAdquisicionyRentaDet objDetalle = null;

                objRenta = _context.tblM_ComparativoAdquisicionyRenta.FirstOrDefault(x => x.idAsignacion == objFiltro.idAsignacion);

                if (objRenta != null)
                {
                    objDetalle = _context.tblM_ComparativoAdquisicionyRentaDet.FirstOrDefault(x => x.idComparativo == objRenta.id);
                }

                return new List<ComparativoDTO>()
                {
                    new ComparativoDTO(){ header = "Marca Modelo", txtIdnumero1 = "idMarcaNum1Marca", txtIdnumero2 = "idMarcaNum2Marca", txtIdnumero3 = "idMarcaNum3Marca", txtIdnumero4 = "idMarcaNum4Marca", txtIdnumero5 = "idMarcaNum5Marca", txtIdnumero6 = "idMarcaNum6Marca", txtIdnumero7 = "idMarcaNum7Marca" },
                    new ComparativoDTO(){ header = "Proveedor", txtIdnumero1 = "idMarcaNum1proveedor", txtIdnumero2 = "idMarcaNum2proveedor", txtIdnumero3 = "idMarcaNum3proveedor", txtIdnumero4 = "idMarcaNum4proveedor", txtIdnumero5 = "idMarcaNum5proveedor", txtIdnumero6 = "idMarcaNum6proveedor", txtIdnumero7 = "idMarcaNum7proveedor" },
                    new ComparativoDTO(){ header = "Precio de venta", txtIdnumero1 = "idMarcaNum1precio", txtIdnumero2 = "idMarcaNum2precio", txtIdnumero3 = "idMarcaNum3precio", txtIdnumero4 = "idMarcaNum4precio", txtIdnumero5 = "idMarcaNum5precio", txtIdnumero6 = "idMarcaNum6precio", txtIdnumero7 = "idMarcaNum7precio" },
                    new ComparativoDTO(){ header = "Trade IN", txtIdnumero1 = "idMarcaNum1Trade", txtIdnumero2 = "idMarcaNum2Trade", txtIdnumero3 = "idMarcaNum3Trade", txtIdnumero4 = "idMarcaNum4Trade", txtIdnumero5 = "idMarcaNum5Trade", txtIdnumero6 = "idMarcaNum6Trade", txtIdnumero7 = "idMarcaNum7Trade" },
                    new ComparativoDTO(){ header = "Valores de recompra", txtIdnumero1 = "idMarcaNum1Valores", txtIdnumero2 = "idMarcaNum2Valores", txtIdnumero3 = "idMarcaNum3Valores", txtIdnumero4 = "idMarcaNum4Valores", txtIdnumero5 = "idMarcaNum5Valores", txtIdnumero6 = "idMarcaNum6Valores", txtIdnumero7 = "idMarcaNum7Valores" },
                    new ComparativoDTO(){ header = "Precio de renta pura", txtIdnumero1 = "idMarcaNum1Precio", txtIdnumero2 = "idMarcaNum2Precio", txtIdnumero3 = "idMarcaNum3Precio", txtIdnumero4 = "idMarcaNum4Precio", txtIdnumero5 = "idMarcaNum5Precio", txtIdnumero6 = "idMarcaNum6Precio", txtIdnumero7 = "idMarcaNum7Precio" },
                    new ComparativoDTO(){ header = "Precio de renta en ROC", txtIdnumero1 = "idMarcaNum1PrecioRoc", txtIdnumero2 = "idMarcaNum2PrecioRoc", txtIdnumero3 = "idMarcaNum3PrecioRoc", txtIdnumero4 = "idMarcaNum4PrecioRoc", txtIdnumero5 = "idMarcaNum5PrecioRoc", txtIdnumero6 = "idMarcaNum6PrecioRoc", txtIdnumero7 = "idMarcaNum7PrecioRoc" },
                    new ComparativoDTO(){ header = "Base horas", txtIdnumero1 = "idMarcaNum1BaseHoras", txtIdnumero2 = "idMarcaNum2BaseHoras", txtIdnumero3 = "idMarcaNum3BaseHoras", txtIdnumero4 = "idMarcaNum4BaseHoras", txtIdnumero5 = "idMarcaNum5BaseHoras", txtIdnumero6 = "idMarcaNum6BaseHoras", txtIdnumero7 = "idMarcaNum7BaseHoras" },
                    new ComparativoDTO(){ header = "Tiempo de entrega", txtIdnumero1 = "idMarcaNum1Tiempo", txtIdnumero2 = "idMarcaNum2Tiempo", txtIdnumero3 = "idMarcaNum3Tiempo", txtIdnumero4 = "idMarcaNum4Tiempo", txtIdnumero5 = "idMarcaNum5Tiempo", txtIdnumero6 = "idMarcaNum6Tiempo", txtIdnumero7 = "idMarcaNum7Tiempo" },
                    new ComparativoDTO(){ header = "Ubicación", txtIdnumero1 = "idMarcaNum1Ubicacion", txtIdnumero2 = "idMarcaNum2Ubicacion", txtIdnumero3 = "idMarcaNum3Ubicacion", txtIdnumero4 = "idMarcaNum4Ubicacion", txtIdnumero5 = "idMarcaNum5Ubicacion", txtIdnumero6 = "idMarcaNum6Ubicacion", txtIdnumero7 = "idMarcaNum7Ubicacion" },
                    new ComparativoDTO(){ header = "Horas", txtIdnumero1 = "idMarcaNum1Horas", txtIdnumero2 = "idMarcaNum2Horas", txtIdnumero3 = "idMarcaNum3Horas", txtIdnumero4 = "idMarcaNum4Horas", txtIdnumero5 = "idMarcaNum5Horas", txtIdnumero6 = "idMarcaNum6Horas", txtIdnumero7 = "idMarcaNum7Horas" },
                    new ComparativoDTO(){ header = "Seguro", txtIdnumero1 = "idMarcaNum1Seguro", txtIdnumero2 = "idMarcaNum2Seguro", txtIdnumero3 = "idMarcaNum3Seguro", txtIdnumero4 = "idMarcaNum4Seguro", txtIdnumero5 = "idMarcaNum5Seguro", txtIdnumero6 = "idMarcaNum6Seguro", txtIdnumero7 = "idMarcaNum7Seguro" },
                    new ComparativoDTO(){ header = "Garantía", txtIdnumero1 = "idMarcaNum1Garantia", txtIdnumero2 = "idMarcaNum2Garantia", txtIdnumero3 = "idMarcaNum3Garantia", txtIdnumero4 = "idMarcaNum4Garantia", txtIdnumero5 = "idMarcaNum5Garantia", txtIdnumero6 = "idMarcaNum6Garantia", txtIdnumero7 = "idMarcaNum7Garantia" },
                    new ComparativoDTO(){ header = "Servicios preventivos", txtIdnumero1 = "idMarcaNum1Servicios", txtIdnumero2 = "idMarcaNum2Servicios", txtIdnumero3 = "idMarcaNum3Servicios", txtIdnumero4 = "idMarcaNum4Servicios", txtIdnumero5 = "idMarcaNum5Servicios", txtIdnumero6 = "idMarcaNum6Servicios", txtIdnumero7 = "idMarcaNum7Servicios" },
                    new ComparativoDTO(){ header = "Capacitación", txtIdnumero1 = "idMarcaNum1Capacitacion", txtIdnumero2 = "idMarcaNum2Capacitacion", txtIdnumero3 = "idMarcaNum3Capacitacion", txtIdnumero4 = "idMarcaNum4Capacitacion", txtIdnumero5 = "idMarcaNum5Capacitacion", txtIdnumero6 = "idMarcaNum6Capacitacion", txtIdnumero7 = "idMarcaNum7Capacitacion" },
                    new ComparativoDTO(){ header = "Deposito en garantía", txtIdnumero1 = "idMarcaNum1Deposito", txtIdnumero2 = "idMarcaNum2Deposito", txtIdnumero3 = "idMarcaNum3Deposito", txtIdnumero4 = "idMarcaNum4Deposito", txtIdnumero5 = "idMarcaNum5Deposito", txtIdnumero6 = "idMarcaNum6Deposito", txtIdnumero7 = "idMarcaNum7Deposito" },
                    new ComparativoDTO(){ header = "Lugar de entrega", txtIdnumero1 = "idMarcaNum1Lugar", txtIdnumero2 = "idMarcaNum2Lugar", txtIdnumero3 = "idMarcaNum3Lugar", txtIdnumero4 = "idMarcaNum4Lugar", txtIdnumero5 = "idMarcaNum5Lugar", txtIdnumero6 = "idMarcaNum6Lugar", txtIdnumero7 = "idMarcaNum7Lugar" },
                    new ComparativoDTO(){ header = "Flete", txtIdnumero1 = "idMarcaNum1Flete", txtIdnumero2 = "idMarcaNum2Flete", txtIdnumero3 = "idMarcaNum3Flete", txtIdnumero4 = "idMarcaNum4Flete", txtIdnumero5 = "idMarcaNum5Flete", txtIdnumero6 = "idMarcaNum6Flete", txtIdnumero7 = "idMarcaNum7Flete" },
                    new ComparativoDTO(){ header = "Condiciones de pago entrega", txtIdnumero1 = "idMarcaNum1Condiciones", txtIdnumero2 = "idMarcaNum2Condiciones", txtIdnumero3 = "idMarcaNum3Condiciones", txtIdnumero4 = "idMarcaNum4Condiciones", txtIdnumero5 = "idMarcaNum5Condiciones", txtIdnumero6 = "idMarcaNum6Condiciones", txtIdnumero7 = "idMarcaNum7Condiciones" },
                    new ComparativoDTO(){ header = "Comentarios", txtIdnumero1 = "textareaComentarios1", txtIdnumero2 = "textareaComentarios2", txtIdnumero3 = "textareaComentarios3", txtIdnumero4 = "textareaComentarios4", txtIdnumero5 = "textareaComentarios5", txtIdnumero6 = "textareaComentarios6", txtIdnumero7 = "textareaComentarios7" },
                    new ComparativoDTO(){ header = "Característica del equipo", txtIdnumero1 = "idMarcaNum1Caracteristicas", txtIdnumero2 = "idMarcaNum2Caracteristicas", txtIdnumero3 = "idMarcaNum3Caracteristicas", txtIdnumero4 = "idMarcaNum4Caracteristicas", txtIdnumero5 = "idMarcaNum5Caracteristicas", txtIdnumero6 = "idMarcaNum6Caracteristicas", txtIdnumero7 = "idMarcaNum7Caracteristicas" },
                    new ComparativoDTO(){ header = "Agregar imágenes", txtIdnumero1 = "inputAgregarImagen1", txtIdnumero2 = "inputAgregarImagen2", txtIdnumero3 = "inputAgregarImagen3", txtIdnumero4 = "inputAgregarImagen4", txtIdnumero5 = "inputAgregarImagen5", txtIdnumero6 = "inputAgregarImagen6", txtIdnumero7 = "inputAgregarImagen7" },
                    new ComparativoDTO(){ header = (objDetalle != null && objFiltro.idAsignacion > 0) ? objDetalle.caracteristicasDelEquipo1 : "Caracteristica1", txtIdnumero1 = "idMarcaNum1Caracteristicas11", txtIdnumero2 = "idMarcaNum2Caracteristicas21", txtIdnumero3 = "idMarcaNum3Caracteristicas31", txtIdnumero4 = "idMarcaNum4Caracteristicas41", txtIdnumero5 = "idMarcaNum5Caracteristicas51", txtIdnumero6 = "idMarcaNum6Caracteristicas61", txtIdnumero7 = "idMarcaNum7Caracteristicas71" },
                    new ComparativoDTO(){ header = (objDetalle != null && objFiltro.idAsignacion > 0) ? objDetalle.caracteristicasDelEquipo2 : "Caracteristica2", txtIdnumero1 = "idMarcaNum1Caracteristicas12", txtIdnumero2 = "idMarcaNum2Caracteristicas22", txtIdnumero3 = "idMarcaNum3Caracteristicas32", txtIdnumero4 = "idMarcaNum4Caracteristicas42", txtIdnumero5 = "idMarcaNum5Caracteristicas52", txtIdnumero6 = "idMarcaNum6Caracteristicas62", txtIdnumero7 = "idMarcaNum7Caracteristicas72" },
                    new ComparativoDTO(){ header = (objDetalle != null && objFiltro.idAsignacion > 0) ? objDetalle.caracteristicasDelEquipo3 : "Caracteristica3", txtIdnumero1 = "idMarcaNum1Caracteristicas13", txtIdnumero2 = "idMarcaNum2Caracteristicas23", txtIdnumero3 = "idMarcaNum3Caracteristicas33", txtIdnumero4 = "idMarcaNum4Caracteristicas43", txtIdnumero5 = "idMarcaNum5Caracteristicas53", txtIdnumero6 = "idMarcaNum6Caracteristicas63", txtIdnumero7 = "idMarcaNum7Caracteristicas73" },
                    new ComparativoDTO(){ header = (objDetalle != null && objFiltro.idAsignacion > 0) ? objDetalle.caracteristicasDelEquipo4 : "Caracteristica4", txtIdnumero1 = "idMarcaNum1Caracteristicas14", txtIdnumero2 = "idMarcaNum2Caracteristicas24", txtIdnumero3 = "idMarcaNum3Caracteristicas34", txtIdnumero4 = "idMarcaNum4Caracteristicas44", txtIdnumero5 = "idMarcaNum5Caracteristicas54", txtIdnumero6 = "idMarcaNum6Caracteristicas64", txtIdnumero7 = "idMarcaNum7Caracteristicas74" },
                    new ComparativoDTO(){ header = (objDetalle != null && objFiltro.idAsignacion > 0) ? objDetalle.caracteristicasDelEquipo5 : "Caracteristica5", txtIdnumero1 = "idMarcaNum1Caracteristicas15", txtIdnumero2 = "idMarcaNum2Caracteristicas25", txtIdnumero3 = "idMarcaNum3Caracteristicas35", txtIdnumero4 = "idMarcaNum4Caracteristicas45", txtIdnumero5 = "idMarcaNum5Caracteristicas55", txtIdnumero6 = "idMarcaNum6Caracteristicas65", txtIdnumero7 = "idMarcaNum7Caracteristicas75" },
                    new ComparativoDTO(){ header = (objDetalle != null && objFiltro.idAsignacion > 0) ? objDetalle.caracteristicasDelEquipo6 : "Caracteristica6", txtIdnumero1 = "idMarcaNum1Caracteristicas16", txtIdnumero2 = "idMarcaNum2Caracteristicas26", txtIdnumero3 = "idMarcaNum3Caracteristicas36", txtIdnumero4 = "idMarcaNum4Caracteristicas46", txtIdnumero5 = "idMarcaNum5Caracteristicas56", txtIdnumero6 = "idMarcaNum6Caracteristicas66", txtIdnumero7 = "idMarcaNum7Caracteristicas76" },
                    new ComparativoDTO(){ header = (objDetalle != null && objFiltro.idAsignacion > 0) ? objDetalle.caracteristicasDelEquipo7 : "Caracteristica7", txtIdnumero1 = "idMarcaNum1Caracteristicas17", txtIdnumero2 = "idMarcaNum2Caracteristicas27", txtIdnumero3 = "idMarcaNum3Caracteristicas37", txtIdnumero4 = "idMarcaNum4Caracteristicas47", txtIdnumero5 = "idMarcaNum5Caracteristicas57", txtIdnumero6 = "idMarcaNum6Caracteristicas67", txtIdnumero7 = "idMarcaNum7Caracteristicas77" }
                };
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error interno al intentar obtener el catálogo tipos de emleados");
                return null;
            }
        }
        public List<ComparativoDTO> getTablaComparativoAdquisicionDetalle(int idAsignacion, int idUsuario)
        {

            List<ComparativoDTO> lstreturn = new List<ComparativoDTO>();
            var objComparativo = _context.tblM_ComparativoAdquisicionyRenta.FirstOrDefault(r => r.idAsignacion == idAsignacion);
            if (objComparativo != null)
            {
                var lstDetalleAdquisicion = _context.tblM_ComparativoAdquisicionyRentaDet.Where(r => r.idComparativo == objComparativo.id).ToList();
                var lstUsuarios = _context.tblM_ComparativoAdquisicionyRentaAutorizante.Where(r => r.idAsignacion == objComparativo.idAsignacion).Select(y => y.autorizanteID).ToList();

                lstreturn = lstDetalleAdquisicion.ToList().Select(x => new ComparativoDTO
                {
                    idDet = x.id,
                    idRow = x.idRow,
                    idComparativo = x.idComparativo,
                    marcaModelo = x.marcaModelo,
                    proveedor = x.proveedor,
                    precioDeVenta = x.precioDeVenta,
                    tradeIn = x.tradeIn,
                    valoresDeRecompra = x.valoresDeRecompra,
                    precioDeRentaPura = x.precioDeRentaPura,
                    precioDeRentaEnRoc = x.precioDeRentaEnRoc,
                    baseHoras = x.baseHoras,
                    tiempoDeEntrega = x.tiempoDeEntrega,
                    ubicacion = x.ubicacion,
                    horas = x.horas,
                    seguro = x.seguro,
                    garantia = x.garantia,
                    serviciosPreventivos = x.serviciosPreventivos,
                    capacitacion = x.capacitacion,
                    depositoEnGarantia = x.depositoEnGarantia,
                    lugarDeEntrega = x.lugarDeEntrega,
                    flete = x.flete,
                    //rutaArchivo = x.rutaArchivo.Split('\\')[5],
                    //rutaArchivo = RetornarBase64(x.rutaArchivo),
                    condicionesDePagoEntrega = x.condicionesDePagoEntrega,
                    caracteristicasDelEquipo1 = x.caracteristicasDelEquipo1,
                    caracteristicasDelEquipo2 = x.caracteristicasDelEquipo2,
                    caracteristicasDelEquipo3 = x.caracteristicasDelEquipo3,
                    caracteristicasDelEquipo4 = x.caracteristicasDelEquipo4,
                    caracteristicasDelEquipo5 = x.caracteristicasDelEquipo5,
                    caracteristicasDelEquipo6 = x.caracteristicasDelEquipo6,
                    caracteristicasDelEquipo7 = x.caracteristicasDelEquipo7,
                    lstCaracteristicas = _context.tblM_ComparativosAdquisicionyRentaCaracteristicasDet.Where(r => r.idComparativoDetalle == x.id).ToList(),
                    numeroMayor = _context.tblM_ComparativosAdquisicionyRentaCaracteristicasDet.Where(r => r.idComparativoDetalle == x.id).ToList().Count(),
                    check = _context.tblM_ComparativoAdquisicionyRentaAutorizante.Where(r => r.idComparativoDetalle == x.id && r.autorizanteID == idUsuario).Select(y => y.idComparativoDetalle).FirstOrDefault() == 0 ? false : true,
                    fechaDeElaboracion = objComparativo.fechaDeElaboracion,
                    obra = _context.tblM_ComparativoAdquisicionyRenta.Where(s => s.id == x.idComparativo).ToList().Select(y => y.obra).FirstOrDefault(),
                    nombreDelEquipo = _context.tblM_ComparativoAdquisicionyRenta.Where(s => s.id == x.idComparativo).ToList().Select(y => y.nombreDelEquipo).FirstOrDefault(),
                    compra = _context.tblM_ComparativoAdquisicionyRenta.Where(s => s.id == x.idComparativo).ToList().Select(y => y.compra).FirstOrDefault(),
                    renta = _context.tblM_ComparativoAdquisicionyRenta.Where(s => s.id == x.idComparativo).ToList().Select(y => y.renta).FirstOrDefault(),
                    roc = _context.tblM_ComparativoAdquisicionyRenta.Where(s => s.id == x.idComparativo).ToList().Select(y => y.roc).FirstOrDefault(),
                    tipoMoneda = objComparativo.tipoMoneda
                }).ToList();

            }


            return lstreturn;
        }

        public List<ComparativoDTO> getTablaComparativoAdquisicionDetallePorCuadro(int idCuadro, int idUsuario)
        {
            var registroCuadro = _context.tblM_ComparativoAdquisicionyRenta.FirstOrDefault(x => x.id == idCuadro);

            if (registroCuadro == null)
            {
                throw new Exception("No se encuentra la información del cuadro.");
            }

            return _context.tblM_ComparativoAdquisicionyRentaDet.Where(x => x.idComparativo == registroCuadro.id).ToList().Select(x => new ComparativoDTO
            {
                idDet = x.id,
                idRow = x.idRow,
                idComparativo = x.idComparativo,
                marcaModelo = x.marcaModelo,
                proveedor = x.proveedor,
                precioDeVenta = x.precioDeVenta,
                tradeIn = x.tradeIn,
                valoresDeRecompra = x.valoresDeRecompra,
                precioDeRentaPura = x.precioDeRentaPura,
                precioDeRentaEnRoc = x.precioDeRentaEnRoc,
                baseHoras = x.baseHoras,
                tiempoDeEntrega = x.tiempoDeEntrega,
                ubicacion = x.ubicacion,
                horas = x.horas,
                seguro = x.seguro,
                garantia = x.garantia,
                serviciosPreventivos = x.serviciosPreventivos,
                capacitacion = x.capacitacion,
                depositoEnGarantia = x.depositoEnGarantia,
                lugarDeEntrega = x.lugarDeEntrega,
                flete = x.flete,
                condicionesDePagoEntrega = x.condicionesDePagoEntrega,
                caracteristicasDelEquipo1 = x.caracteristicasDelEquipo1,
                caracteristicasDelEquipo2 = x.caracteristicasDelEquipo2,
                caracteristicasDelEquipo3 = x.caracteristicasDelEquipo3,
                caracteristicasDelEquipo4 = x.caracteristicasDelEquipo4,
                caracteristicasDelEquipo5 = x.caracteristicasDelEquipo5,
                caracteristicasDelEquipo6 = x.caracteristicasDelEquipo6,
                caracteristicasDelEquipo7 = x.caracteristicasDelEquipo7,
                lstCaracteristicas = _context.tblM_ComparativosAdquisicionyRentaCaracteristicasDet.Where(r => r.idComparativoDetalle == x.id).ToList(),
                numeroMayor = _context.tblM_ComparativosAdquisicionyRentaCaracteristicasDet.Where(r => r.idComparativoDetalle == x.id).ToList().Count(),
                check = _context.tblM_ComparativoAdquisicionyRentaAutorizante.Where(r => r.idComparativoDetalle == x.id && r.autorizanteID == idUsuario).Select(y => y.idComparativoDetalle).FirstOrDefault() == 0 ? false : true,
                fechaDeElaboracion = registroCuadro.fechaDeElaboracion,
                obra = _context.tblM_ComparativoAdquisicionyRenta.Where(s => s.id == x.idComparativo).ToList().Select(y => y.obra).FirstOrDefault(),
                nombreDelEquipo = _context.tblM_ComparativoAdquisicionyRenta.Where(s => s.id == x.idComparativo).ToList().Select(y => y.nombreDelEquipo).FirstOrDefault(),
                compra = _context.tblM_ComparativoAdquisicionyRenta.Where(s => s.id == x.idComparativo).ToList().Select(y => y.compra).FirstOrDefault(),
                renta = _context.tblM_ComparativoAdquisicionyRenta.Where(s => s.id == x.idComparativo).ToList().Select(y => y.renta).FirstOrDefault(),
                roc = _context.tblM_ComparativoAdquisicionyRenta.Where(s => s.id == x.idComparativo).ToList().Select(y => y.roc).FirstOrDefault(),
                tipoMoneda = registroCuadro.tipoMoneda,
                comentario = x.comentarios
            }).ToList();
        }

        public string RetornarBase64(string rutaArchivo)
        {
            string base64 = "";

            if (rutaArchivo != null && rutaArchivo != "")
            {
                byte[] AsBytes = File.ReadAllBytes(rutaArchivo);
                base64 = "data:image/jpeg;base64," + Convert.ToBase64String(AsBytes);
            }

            return base64;
        }

        public string SolicitarNombre(string banco)
        {
            int a = Convert.ToInt32(banco);
            string retornar = "";
            return retornar = _context.tblM_Comp_CatFinanciero.Where(r => r.id == a).FirstOrDefault().descripcion;
        }
        public Dictionary<string, object> getTablaComparativoFinancieroDetalle(int idFinanciero, int idUsuario)
        {
            var result = new Dictionary<string, object>();

            var datos = _context.tblM_ComparativoFinancieroDet.Where(n => n.idFinanciero == idFinanciero).ToList().Select(x => new ComparativoDTO
            {
                id = x.id,
                idFinanciero = x.idFinanciero,
                idRow = x.idRow,
                banco = SolicitarNombre(x.banco),
                plazo = x.plazo,
                precioDelEquipo = x.precioDelEquipo,
                tiempoRestanteProyecto = x.tiempoRestanteProyecto,
                iva = x.iva,
                total = x.total,
                montoFinanciar = x.montoFinanciar,
                tipoOperacion = x.tipoOperacion,
                opcionCompra = x.opcionCompra,
                valorResidual = x.valorResidual,
                depositoEfectivo = x.depositoEfectivo,
                moneda = x.moneda,
                plazoMeses = x.plazoMeses,
                tasaDeInteres = x.tasaDeInteres,
                gastosFijos = x.gastosFijos,
                comision = x.comision,
                montoComision = x.montoComision,
                rentasEnGarantia = x.rentasEnGarantia,
                crecimientoPagos = x.crecimientoPagos,
                pagoInicial = x.pagoInicial,
                pagoTotalIntereses = x.pagoTotalIntereses,
                tasaEfectiva = x.tasaEfectiva,
                mensualidad = x.mensualidad,
                mensualidadSinIVA = x.mensualidadSinIVA,
                pagoTotal = x.pagoTotal,
                ComentarioGeneral = _context.tblM_ComparativoAdquisicionyRenta.Where(r => r.idAsignacion == x.idFinanciero).ToList().Select(y => y.ComentarioGeneral).FirstOrDefault(),
                //ESTA TABLA DEBE DE CAMBIARSE A FINANCIERO AUTORIZANTE
                check = _context.tblM_ComparativoFinancieroAutorizante.Where(r => r.idFinanciero == x.id && r.autorizanteID == idUsuario).Select(y => y.idFinanciero).FirstOrDefault() == 0 ? false : true,
                fechaDeElaboracionFinanciero = _context.tblM_ComparativoAdquisicionyRenta.Where(r => r.idAsignacion == x.idFinanciero).ToList().Select(y => y.fechaDeElaboracionFinanciero).FirstOrDefault(),
            }).ToList();

            var gpxBarras = new GpxsHighCharts();
            var barraExterna = new GpxSerieBarrasDoblesDTO();
            var barraInterna = new GpxSerieBarrasDoblesDTO();

            var gpxLineas = new List<GpxSerieLineasBasicasDTO>();

            barraExterna.name = "Intereses";
            barraExterna.color = "rgba(30, 144, 255, 1)";
            barraExterna.pointPadding = 0.3M;
            barraExterna.pointPlacement = -0.2M;

            barraInterna.name = "Tasa efectiva";
            barraInterna.color = "rgba(255, 140, 0, .9)";
            barraInterna.pointPadding = 0.4M;
            barraInterna.pointPlacement = -0.2M;
            barraInterna.yAxis = 1;

            foreach (var item in datos)
            {
                gpxBarras.categories.Add(item.banco);

                var interesString = string.Join("", item.pagoTotalIntereses.Where(w => Char.IsDigit(w) || w == '.'));
                barraExterna.data.Add(Convert.ToDecimal(interesString));

                var efectivaString = string.Join("", item.tasaEfectiva.Where(w => Char.IsDigit(w) || w == '.'));
                barraInterna.data.Add(Convert.ToDecimal(efectivaString) * 100);

                var gpxLinea = new GpxSerieLineasBasicasDTO();

                var _plazos = Convert.ToInt32(item.plazo);
                for (int i = 1; i <= _plazos + 1; i++)
                {
                    if (i == 1)
                    {
                        var inicialString = string.Join("", item.pagoInicial.Where(w => Char.IsDigit(w) || w == '.'));
                        gpxLinea.data.Add(Convert.ToDecimal(inicialString));
                        gpxLinea.name = item.banco;
                    }
                    else
                    {
                        var mensualidad = string.Join("", item.mensualidad.Where(w => Char.IsDigit(w) || w == '.'));
                        gpxLinea.data.Add(Convert.ToDecimal(mensualidad));
                    }
                }
                gpxLinea.data.Add(0);

                gpxLineas.Add(gpxLinea);
            }

            gpxBarras.series.Add(barraExterna);
            gpxBarras.series.Add(barraInterna);

            result.Add(SUCCESS, true);
            result.Add(ITEMS, datos);
            result.Add("gpxBarra", gpxBarras);
            result.Add("gpxLinea", gpxLineas);

            return result;
        }
        public ComparativoDTO addAdquisisionP(ComparativoDTO objComparativo)
        {
            ComparativoDTO objComparado = new ComparativoDTO();
            try
            {
                tblM_ComparativoAdquisicionyRenta obj = _context.tblM_ComparativoAdquisicionyRenta.Where(r => r.idAsignacion == objComparativo.idAsignacion).FirstOrDefault();

                if (obj == null || objComparativo.idAsignacion == 0)
                {
                    obj = new tblM_ComparativoAdquisicionyRenta();

                    obj.idAsignacion = objComparativo.idAsignacion;
                    obj.estatus = objComparativo.idAsignacion > 0 ? 1 : 2;
                    obj.estatusFinanciera = 1;
                    obj.idMegusta = 0;
                    obj.fechaDeElaboracion = DateTime.Now;

                    if (objComparativo.idAsignacion == 0)
                    {
                        obj.obra = objComparativo.obra;
                        obj.nombreDelEquipo = objComparativo.nombreDelEquipo;
                    }

                    _context.tblM_ComparativoAdquisicionyRenta.Add(obj);
                    _context.SaveChanges();

                    if (objComparativo.idAsignacion == 0)
                    {
                        obj.folioAdquisicion = Folio("A", obj.id.ToString());
                        obj.folioFinanciera = Folio("F", obj.id.ToString());
                    }

                    objComparado.msjExito = "Guardado con exito";

                    if (objComparativo.idAsignacion > 0)
                    {
                        tblM_ComparativoAdquisicionyRenta obj2 = _context.tblM_ComparativoAdquisicionyRenta.Where(r => r.idAsignacion == objComparativo.idAsignacion).FirstOrDefault();
                        objComparado.idComparativo = obj2.id;
                        objComparado.estatus = obj2.estatus;
                        objComparado.estatusFinanciera = obj2.estatusFinanciera;

                        tblM_ComparativoAdquisicionyRenta obj3 = _context.tblM_ComparativoAdquisicionyRenta.Where(r => r.idAsignacion == objComparativo.idAsignacion).FirstOrDefault();
                        obj3.folioAdquisicion = Folio("A", obj3.id.ToString());
                        obj3.folioFinanciera = Folio("F", obj3.id.ToString());
                        objComparado.folioFinanciera = Folio("F", obj3.id.ToString());
                        objComparado.folioAdquisicion = Folio("A", obj3.id.ToString());
                    }

                    _context.SaveChanges();
                }
                else
                {
                    objComparado.idComparativo = obj.id;
                    objComparado.estatus = obj.estatus;
                    objComparado.estatusFinanciera = obj.estatusFinanciera;
                    objComparado.folioFinanciera = obj.folioFinanciera;
                    objComparado.folioAdquisicion = obj.folioAdquisicion;
                }

                return objComparado;
            }
            catch (Exception)
            {
                objComparado.msjExito = "Ocurrio algun error favor de contactar con el departamento de TI";
                return objComparado;
            }
        }
        public string Folio(string _folio, string id)
        {
            string str = "";
            str += _folio + "-";
            str += new String('0', 9 - id.Length) + id;
            return str;
        }
        private string ObtenerFormatoNombreArchivoA(string nombreBase, string fileName)
        {
            return String.Format("{0} {1}{2}", nombreBase, fileName.Split('.')[0], Path.GetExtension(fileName));
        }

        public Dictionary<string, object> ObtenerInformacionCuadro(int idCuadro)
        {
            resultado = new Dictionary<string, object>();

            try
            {
                var registroCuadro = _context.tblM_ComparativoAdquisicionyRenta.FirstOrDefault(x => x.id == idCuadro);

                if (registroCuadro == null)
                {
                    throw new Exception("No se encuentra la información del cuadro.");
                }

                #region Información General del Cuadro
                var cuadro = new ComparativoDTO()
                {
                    obra = registroCuadro.obra,
                    nombreDelEquipo = registroCuadro.nombreDelEquipo,
                    compra = registroCuadro.compra,
                    renta = registroCuadro.renta,
                    roc = registroCuadro.roc,
                    tipoMoneda = registroCuadro.tipoMoneda,
                };

                resultado.Add("cuadro", cuadro);
                #endregion

                #region Información del Detalle y sus Características
                var listaDetalle = _context.tblM_ComparativoAdquisicionyRentaDet.Where(x => x.idComparativo == idCuadro).ToList().Select(x => new ComparativoAdquisicion
                {
                    marcaModelo = x.marcaModelo,
                    proveedor = x.proveedor,
                    precioDeVenta = x.precioDeVenta,
                    tradeIn = x.tradeIn,
                    valoresDeRecompra = x.valoresDeRecompra,
                    precioDeRentaPura = x.precioDeRentaPura,
                    precioDeRentaEnRoc = x.precioDeRentaEnRoc,
                    baseHoras = x.baseHoras,
                    tiempoDeEntrega = x.tiempoDeEntrega,
                    ubicacion = x.ubicacion,
                    horas = x.horas,
                    seguro = x.seguro,
                    garantia = x.garantia,
                    serviciosPreventivos = x.serviciosPreventivos,
                    capacitacion = x.capacitacion,
                    depositoEnGarantia = x.depositoEnGarantia,
                    lugarDeEntrega = x.lugarDeEntrega,
                    flete = x.flete,
                    condicionesDePagoEntrega = x.condicionesDePagoEntrega,
                    //rutaArchivo = ,
                    caracteristicasDelEquipo1 = x.caracteristicasDelEquipo1,
                    caracteristicasDelEquipo2 = x.caracteristicasDelEquipo2,
                    caracteristicasDelEquipo3 = x.caracteristicasDelEquipo3,
                    caracteristicasDelEquipo4 = x.caracteristicasDelEquipo4,
                    caracteristicasDelEquipo5 = x.caracteristicasDelEquipo5,
                    caracteristicasDelEquipo6 = x.caracteristicasDelEquipo6,
                    caracteristicasDelEquipo7 = x.caracteristicasDelEquipo7,
                    comentarios = x.comentarios,
                    lstCaracteristicas = _context.tblM_ComparativosAdquisicionyRentaCaracteristicasDet.Where(y => y.idComparativoDetalle == x.id).ToList(),
                });

                resultado.Add("listaDetalle", listaDetalle);
                #endregion

                #region Información de los Autorizantes
                var listaAutorizantes = _context.tblM_ComparativoAdquisicionyRentaAutorizante.Where(x => x.idCuadro == idCuadro).ToList();

                resultado.Add("listaAutorizantes", listaAutorizantes);
                #endregion

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(1, 0, "CatMaquinaController", "ObtenerInformacionCuadro", e, AccionEnum.CONSULTA, 0, idCuadro);

                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarCuadroIndependiente(ComparativoDTO comparativo, List<ComparativoAdquisicion> detalle, List<tblM_ComparativoAdquisicionyRentaAutorizante> listaAutorizantes, List<HttpPostedFileBase> listaArchivos)
        {
            resultado = new Dictionary<string, object>();

            using (var dbSigoplanTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (comparativo.idCuadro == 0) //Guardar Nuevo Cuadro
                    {
                        var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();

                        #region Guardar Registro Principal (tblM_ComparativoAdquisicionyRenta)
                        var nuevoRegistroComparativo = new tblM_ComparativoAdquisicionyRenta();

                        nuevoRegistroComparativo.idAsignacion = 0;
                        nuevoRegistroComparativo.estatus = 2;
                        nuevoRegistroComparativo.idMegusta = 0;
                        nuevoRegistroComparativo.estatusFinanciera = 1;
                        nuevoRegistroComparativo.ComentarioGeneral = "";
                        nuevoRegistroComparativo.fechaDeElaboracion = DateTime.Now;
                        nuevoRegistroComparativo.fechaDeElaboracionFinanciero = null;
                        nuevoRegistroComparativo.obra = comparativo.obra;
                        nuevoRegistroComparativo.nombreDelEquipo = comparativo.nombreDelEquipo;
                        nuevoRegistroComparativo.compra = comparativo.compra;
                        nuevoRegistroComparativo.renta = comparativo.renta;
                        nuevoRegistroComparativo.roc = comparativo.roc;
                        nuevoRegistroComparativo.tipoMoneda = comparativo.tipoMoneda;

                        _context.tblM_ComparativoAdquisicionyRenta.Add(nuevoRegistroComparativo);
                        _context.SaveChanges();

                        var folioCuadro = Folio("A", nuevoRegistroComparativo.id.ToString());
                        nuevoRegistroComparativo.folioAdquisicion = folioCuadro;
                        nuevoRegistroComparativo.folioFinanciera = Folio("F", nuevoRegistroComparativo.id.ToString());
                        _context.SaveChanges();
                        #endregion

                        #region Guardar Registros Detalle (tblM_ComparativoAdquisicionyRentaDet)
                        foreach (var det in detalle)
                        {
                            var nuevoRegistroDetalle = new tblM_ComparativoAdquisicionyRentaDet();

                            nuevoRegistroDetalle.idComparativo = nuevoRegistroComparativo.id;
                            nuevoRegistroDetalle.idRow = det.idRow;
                            nuevoRegistroDetalle.marcaModelo = det.marcaModelo;
                            nuevoRegistroDetalle.proveedor = det.proveedor;
                            nuevoRegistroDetalle.precioDeVenta = det.precioDeVenta;
                            nuevoRegistroDetalle.tradeIn = det.tradeIn;
                            nuevoRegistroDetalle.valoresDeRecompra = det.valoresDeRecompra;
                            nuevoRegistroDetalle.precioDeRentaPura = det.precioDeRentaPura;
                            nuevoRegistroDetalle.precioDeRentaEnRoc = det.precioDeRentaEnRoc;
                            nuevoRegistroDetalle.baseHoras = det.baseHoras;
                            nuevoRegistroDetalle.tiempoDeEntrega = det.tiempoDeEntrega;
                            nuevoRegistroDetalle.ubicacion = det.ubicacion;
                            nuevoRegistroDetalle.horas = det.horas;
                            nuevoRegistroDetalle.seguro = det.seguro;
                            nuevoRegistroDetalle.garantia = det.garantia;
                            nuevoRegistroDetalle.serviciosPreventivos = det.serviciosPreventivos;
                            nuevoRegistroDetalle.capacitacion = det.capacitacion;
                            nuevoRegistroDetalle.depositoEnGarantia = det.depositoEnGarantia;
                            nuevoRegistroDetalle.lugarDeEntrega = det.lugarDeEntrega;
                            nuevoRegistroDetalle.flete = det.flete;
                            nuevoRegistroDetalle.condicionesDePagoEntrega = det.condicionesDePagoEntrega;
                            nuevoRegistroDetalle.caracteristicasDelEquipo1 = det.caracteristicasDelEquipo1;
                            nuevoRegistroDetalle.caracteristicasDelEquipo2 = det.caracteristicasDelEquipo2;
                            nuevoRegistroDetalle.caracteristicasDelEquipo3 = det.caracteristicasDelEquipo3;
                            nuevoRegistroDetalle.caracteristicasDelEquipo4 = det.caracteristicasDelEquipo4;
                            nuevoRegistroDetalle.caracteristicasDelEquipo5 = det.caracteristicasDelEquipo5;
                            nuevoRegistroDetalle.caracteristicasDelEquipo6 = det.caracteristicasDelEquipo6;
                            nuevoRegistroDetalle.caracteristicasDelEquipo7 = det.caracteristicasDelEquipo7;
                            nuevoRegistroDetalle.comentarios = det.comentarios;

                            if (listaArchivos != null)
                            {
                                if (listaArchivos.ElementAtOrDefault((det.idRow - 1)) != null)
                                {
                                    var archivoDetalle = listaArchivos[det.idRow - 1];
                                    var rutaArchivo = Path.Combine(rutaArchivos, DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + ObtenerFormatoNombreArchivoA("COM_", archivoDetalle.FileName));
                                    listaRutaArchivos.Add(Tuple.Create(archivoDetalle, rutaArchivo));
                                    nuevoRegistroDetalle.rutaArchivo = rutaArchivo;
                                }
                            }

                            _context.tblM_ComparativoAdquisicionyRentaDet.Add(nuevoRegistroDetalle);
                            _context.SaveChanges();

                            #region Guardar Registros Características (tblM_ComparativosAdquisicionyRentaCaracteristicasDet)
                            int contadorCaracteristica = 1;

                            foreach (var car in det.lstCaracteristicas)
                            {
                                if (car.Descripcion != "")
                                {
                                    var nuevoRegistroCaracteristica = new tblM_ComparativosAdquisicionyRentaCaracteristicasDet();

                                    nuevoRegistroCaracteristica.idRow = contadorCaracteristica;
                                    nuevoRegistroCaracteristica.idComparativoDetalle = nuevoRegistroDetalle.id;
                                    nuevoRegistroCaracteristica.Descripcion = car.Descripcion;

                                    _context.tblM_ComparativosAdquisicionyRentaCaracteristicasDet.Add(nuevoRegistroCaracteristica);
                                    _context.SaveChanges();

                                    contadorCaracteristica++;
                                }
                            }
                            #endregion
                        }
                        #endregion

                        #region Guardar Registros Autorizantes (tblM_ComparativoAdquisicionyRentaAutorizante)
                        var ultimoAutorizante = listaAutorizantes.Max(x => x.orden);
                        var listaAutorizantesCorreo = new List<tblM_ComparativoAdquisicionyRentaAutorizante>();

                        foreach (var aut in listaAutorizantes)
                        {
                            var usuarioAutorizante = _context.tblP_Usuario.FirstOrDefault(x => x.id == aut.autorizanteID);

                            aut.idAsignacion = 0;
                            aut.idCuadro = nuevoRegistroComparativo.id;
                            aut.idComparativoDetalle = 0;
                            aut.autorizantePuesto = "";

                            if (usuarioAutorizante != null)
                            {
                                aut.autorizantePuesto = _context.tblP_Puesto.Where(x => x.id == usuarioAutorizante.puestoID).Select(y => y.descripcion).FirstOrDefault();
                            }

                            aut.autorizanteStatus = false;
                            aut.autorizanteFinal = aut.orden == ultimoAutorizante ? true : false;
                            aut.autorizanteFecha = null;
                            aut.firma = "";
                            aut.tipo = null;
                            aut.comentario = null;

                            _context.tblM_ComparativoAdquisicionyRentaAutorizante.Add(aut);
                            _context.SaveChanges();

                            listaAutorizantesCorreo.Add(aut);
                        }
                        #endregion

                        #region Guardar Archivos
                        foreach (var arch in listaRutaArchivos)
                        {
                            var guardarArchivo = GlobalUtils.SaveHTTPPostedFileValidacion(arch.Item1, arch.Item2);

                            if (guardarArchivo.Item1 == false)
                            {
                                dbSigoplanTransaction.Rollback();
                                resultado.Clear();
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                                LogError(0, 0, "CatMaquinaController", "GuardarCuadroIndependiente", guardarArchivo.Item2, AccionEnum.AGREGAR, 0, new { comparativo = comparativo, listaAutorizantes = listaAutorizantes, nombreArchivo = arch.Item2 });
                                return resultado;
                            }
                        }
                        #endregion

                        #region Enviar Correo Autorizantes
                        var lstAutorizadores = listaAutorizantesCorreo.Select(y => new ComparativoDTO
                        {
                            autorizanteID = y.autorizanteID,
                            autorizanteNombre = y.autorizanteNombre,
                            autorizantePuesto = y.autorizantePuesto,
                            autorizanteFecha = y.autorizanteFecha,
                            header = y.firma != "" ? "AUTORIZADO" : "PENDIENTE",
                            firma = y.firma,
                            voto = "Opcion " + _context.tblM_ComparativoAdquisicionyRentaDet.Where(s => s.id == y.idComparativoDetalle).ToList().Select(n => n.idRow).FirstOrDefault().ToString()
                        }).ToList();

                        foreach (var item in lstAutorizadores)
                        {
                            List<tblP_Usuario> lstUsuarios = new List<tblP_Usuario>();
                            lstUsuarios = _context.tblP_Usuario.Where(r => r.id == item.autorizanteID).ToList();
                            var subject = "Cuadro Comparativo Adquisicion y Renta de Maquinaria y Equipo ";
                            var body = @"Buen dia " + lstUsuarios.Select(y => y.nombre).FirstOrDefault() + " " + lstUsuarios.Select(y => y.apellidoPaterno).FirstOrDefault() + " " + lstUsuarios.Select(y => y.apellidoMaterno).FirstOrDefault()
                                + " usted tiene una autorizacion de cuadro comparativo de adquisicion y renta pendiente <br>"
                                + htmlCorreo(lstAutorizadores);
                            var correos = lstUsuarios.Select(y => y.correo).ToList();

#if DEBUG
                            correos = new List<string> { "oscar.valencia@construplan.com.mx" };
#endif

                            GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, correos);
                        }
                        #endregion

                        resultado.Add("folio", folioCuadro);
                    }
                    else if (comparativo.idCuadro > 0) //Editar Cuadro Existente
                    {
                        var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();

                        #region Registro Principal (tblM_ComparativoAdquisicionyRenta)
                        var registroCuadro = _context.tblM_ComparativoAdquisicionyRenta.FirstOrDefault(x => x.id == comparativo.idCuadro);

                        if (registroCuadro == null)
                        {
                            throw new Exception("No se encuentra la información del cuadro.");
                        }

                        registroCuadro.obra = comparativo.obra;
                        registroCuadro.nombreDelEquipo = comparativo.nombreDelEquipo;
                        registroCuadro.compra = comparativo.compra;
                        registroCuadro.renta = comparativo.renta;
                        registroCuadro.roc = comparativo.roc;

                        _context.SaveChanges();
                        #endregion

                        #region Registros Detalle (tblM_ComparativoAdquisicionyRentaDet) y Características (tblM_ComparativosAdquisicionyRentaCaracteristicasDet)
                        #region Se eliminan los registros anteriores
                        var listaDetalleAnterior = _context.tblM_ComparativoAdquisicionyRentaDet.Where(x => x.idComparativo == comparativo.idCuadro).ToList();

                        foreach (var det in listaDetalleAnterior)
                        {
                            var listaCaracteristicasAnterior = _context.tblM_ComparativosAdquisicionyRentaCaracteristicasDet.Where(x => x.idComparativoDetalle == det.id).ToList();

                            _context.tblM_ComparativosAdquisicionyRentaCaracteristicasDet.RemoveRange(listaCaracteristicasAnterior);
                        }

                        _context.tblM_ComparativoAdquisicionyRentaDet.RemoveRange(listaDetalleAnterior);
                        _context.SaveChanges();
                        #endregion

                        #region Se registra la información nueva
                        foreach (var det in detalle)
                        {
                            var nuevoRegistroDetalle = new tblM_ComparativoAdquisicionyRentaDet();

                            nuevoRegistroDetalle.idComparativo = comparativo.idCuadro;
                            nuevoRegistroDetalle.idRow = det.idRow;
                            nuevoRegistroDetalle.marcaModelo = det.marcaModelo;
                            nuevoRegistroDetalle.proveedor = det.proveedor;
                            nuevoRegistroDetalle.precioDeVenta = det.precioDeVenta;
                            nuevoRegistroDetalle.tradeIn = det.tradeIn;
                            nuevoRegistroDetalle.valoresDeRecompra = det.valoresDeRecompra;
                            nuevoRegistroDetalle.precioDeRentaPura = det.precioDeRentaPura;
                            nuevoRegistroDetalle.precioDeRentaEnRoc = det.precioDeRentaEnRoc;
                            nuevoRegistroDetalle.baseHoras = det.baseHoras;
                            nuevoRegistroDetalle.tiempoDeEntrega = det.tiempoDeEntrega;
                            nuevoRegistroDetalle.ubicacion = det.ubicacion;
                            nuevoRegistroDetalle.horas = det.horas;
                            nuevoRegistroDetalle.seguro = det.seguro;
                            nuevoRegistroDetalle.garantia = det.garantia;
                            nuevoRegistroDetalle.serviciosPreventivos = det.serviciosPreventivos;
                            nuevoRegistroDetalle.capacitacion = det.capacitacion;
                            nuevoRegistroDetalle.depositoEnGarantia = det.depositoEnGarantia;
                            nuevoRegistroDetalle.lugarDeEntrega = det.lugarDeEntrega;
                            nuevoRegistroDetalle.flete = det.flete;
                            nuevoRegistroDetalle.condicionesDePagoEntrega = det.condicionesDePagoEntrega;
                            nuevoRegistroDetalle.caracteristicasDelEquipo1 = det.caracteristicasDelEquipo1;
                            nuevoRegistroDetalle.caracteristicasDelEquipo2 = det.caracteristicasDelEquipo2;
                            nuevoRegistroDetalle.caracteristicasDelEquipo3 = det.caracteristicasDelEquipo3;
                            nuevoRegistroDetalle.caracteristicasDelEquipo4 = det.caracteristicasDelEquipo4;
                            nuevoRegistroDetalle.caracteristicasDelEquipo5 = det.caracteristicasDelEquipo5;
                            nuevoRegistroDetalle.caracteristicasDelEquipo6 = det.caracteristicasDelEquipo6;
                            nuevoRegistroDetalle.caracteristicasDelEquipo7 = det.caracteristicasDelEquipo7;
                            nuevoRegistroDetalle.comentarios = det.comentarios;

                            if (listaArchivos != null)
                            {
                                if (listaArchivos.ElementAtOrDefault((det.idRow - 1)) != null)
                                {
                                    var archivoDetalle = listaArchivos[det.idRow - 1];
                                    var rutaArchivo = Path.Combine(rutaArchivos, DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + ObtenerFormatoNombreArchivoA("COM_", archivoDetalle.FileName));
                                    listaRutaArchivos.Add(Tuple.Create(archivoDetalle, rutaArchivo));
                                    nuevoRegistroDetalle.rutaArchivo = rutaArchivo;
                                }
                            }

                            _context.tblM_ComparativoAdquisicionyRentaDet.Add(nuevoRegistroDetalle);
                            _context.SaveChanges();

                            #region Guardar Registros Características (tblM_ComparativosAdquisicionyRentaCaracteristicasDet)
                            int contadorCaracteristica = 1;

                            foreach (var car in det.lstCaracteristicas)
                            {
                                if (car.Descripcion != "")
                                {
                                    var nuevoRegistroCaracteristica = new tblM_ComparativosAdquisicionyRentaCaracteristicasDet();

                                    nuevoRegistroCaracteristica.idRow = contadorCaracteristica;
                                    nuevoRegistroCaracteristica.idComparativoDetalle = nuevoRegistroDetalle.id;
                                    nuevoRegistroCaracteristica.Descripcion = car.Descripcion;

                                    _context.tblM_ComparativosAdquisicionyRentaCaracteristicasDet.Add(nuevoRegistroCaracteristica);
                                    _context.SaveChanges();

                                    contadorCaracteristica++;
                                }
                            }
                            #endregion
                        }
                        #endregion
                        #endregion

                        #region Registros Autorizantes (tblM_ComparativoAdquisicionyRentaAutorizante)
                        #region Se eliminan los registros anteriores
                        var listaAutorizantesAnterior = _context.tblM_ComparativoAdquisicionyRentaAutorizante.Where(x => x.idCuadro == comparativo.idCuadro).ToList();

                        _context.tblM_ComparativoAdquisicionyRentaAutorizante.RemoveRange(listaAutorizantesAnterior);
                        #endregion

                        var ultimoAutorizante = listaAutorizantes.Max(x => x.orden);

                        foreach (var aut in listaAutorizantes)
                        {
                            var usuarioAutorizante = _context.tblP_Usuario.FirstOrDefault(x => x.id == aut.autorizanteID);

                            aut.idAsignacion = 0;
                            aut.idCuadro = comparativo.idCuadro;
                            aut.idComparativoDetalle = 0;
                            aut.autorizantePuesto = "";

                            if (usuarioAutorizante != null)
                            {
                                aut.autorizantePuesto = _context.tblP_Puesto.Where(x => x.id == usuarioAutorizante.puestoID).Select(y => y.descripcion).FirstOrDefault();
                            }

                            aut.autorizanteStatus = false;
                            aut.autorizanteFinal = aut.orden == ultimoAutorizante ? true : false;
                            aut.autorizanteFecha = null;
                            aut.firma = "";
                            aut.tipo = null;
                            aut.comentario = null;

                            _context.tblM_ComparativoAdquisicionyRentaAutorizante.Add(aut);
                            _context.SaveChanges();
                        }
                        #endregion

                        #region Guardar Archivos
                        foreach (var arch in listaRutaArchivos)
                        {
                            var guardarArchivo = GlobalUtils.SaveHTTPPostedFileValidacion(arch.Item1, arch.Item2);

                            if (guardarArchivo.Item1 == false)
                            {
                                dbSigoplanTransaction.Rollback();
                                resultado.Clear();
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                                LogError(0, 0, "CatMaquinaController", "GuardarCuadroIndependiente", guardarArchivo.Item2, AccionEnum.AGREGAR, 0, new { comparativo = comparativo, listaAutorizantes = listaAutorizantes, nombreArchivo = arch.Item2 });
                                return resultado;
                            }
                        }
                        #endregion
                    }

                    dbSigoplanTransaction.Commit();
                    SaveBitacora(0, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(new { comparativo = comparativo, detalle = detalle, listaAutorizantes = listaAutorizantes }));
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbSigoplanTransaction.Rollback();

                    LogError(1, 0, "CatMaquinaController", "GuardarCuadroIndependiente", e, AccionEnum.AGREGAR, 0, new { comparativo = comparativo, detalle = detalle, listaAutorizantes = listaAutorizantes });

                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public ComparativoDTO addeditTablaComparativoAdiquisicion(List<HttpPostedFileBase> file, List<ComparativoAdquisicion> objComparativo)
        {
            resultado = new Dictionary<string, object>();
            ComparativoDTO objComparado = new ComparativoDTO();
            try
            {
                int idAsignacion = objComparativo.Select(y => y.idComparativo).FirstOrDefault();
                var objComparativos = _context.tblM_ComparativoAdquisicionyRenta.Where(r => r.id == idAsignacion).FirstOrDefault();

                if (objComparativos != null)
                {
                    objComparativos.tipoMoneda = objComparativo[0].tipoMoneda;
                    objComparativos.fechaDeElaboracion = DateTime.Now;
                    _context.SaveChanges();
                }

                int contador = 0;
                foreach (var item in objComparativo)
                {
                    if (file != null)
                    {
                        if (contador < file.Count)
                        {
                            item.file = file[contador];
                        }
                        contador++;
                    }
                    else
                    {
                        break;
                    }
                }
                var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();


                foreach (var item in objComparativo)
                {
                    tblM_ComparativoAdquisicionyRentaDet objCompa = _context.tblM_ComparativoAdquisicionyRentaDet.Where(r => r.id == item.idDet && r.idComparativo == item.idComparativo && r.idRow == item.idRow).FirstOrDefault();
                    if (item.marcaModelo != null)
                    {
                        if (objCompa == null)
                        {

                            if (item.marcaModelo != "undefined" && item.precioDeVenta != "undefined" && item.proveedor != "undefined")
                            {
                                objCompa = new tblM_ComparativoAdquisicionyRentaDet();

                                objCompa.idRow = item.idRow;
                                objCompa.idComparativo = item.idComparativo;
                                objCompa.marcaModelo = item.marcaModelo;
                                objCompa.proveedor = item.proveedor;
                                objCompa.precioDeVenta = item.precioDeVenta;
                                objCompa.tradeIn = item.tradeIn;
                                objCompa.valoresDeRecompra = item.valoresDeRecompra;
                                objCompa.precioDeRentaPura = item.precioDeRentaPura;
                                objCompa.precioDeRentaEnRoc = item.precioDeRentaEnRoc;
                                objCompa.baseHoras = item.baseHoras;
                                objCompa.tiempoDeEntrega = item.tiempoDeEntrega;
                                objCompa.ubicacion = item.ubicacion;
                                objCompa.horas = item.horas;
                                objCompa.seguro = item.seguro;
                                objCompa.garantia = item.garantia;
                                objCompa.serviciosPreventivos = item.serviciosPreventivos;
                                objCompa.capacitacion = item.capacitacion;
                                objCompa.depositoEnGarantia = item.depositoEnGarantia;

                                if (item.file != null)
                                {
                                    string nombreArchivo = ObtenerFormatoNombreArchivoA("COM_", item.file.FileName);
                                    var CarpetaNueva = rutaArchivos;
                                    string rutaArchivo = Path.Combine(CarpetaNueva, DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + nombreArchivo);
                                    listaRutaArchivos.Add(Tuple.Create(item.file, rutaArchivo));
                                    objCompa.rutaArchivo = rutaArchivo;
                                }

                                // GUARDAR TABLA ARCHIVOS

                                objCompa.lugarDeEntrega = item.lugarDeEntrega;
                                objCompa.flete = item.flete;
                                objCompa.condicionesDePagoEntrega = item.condicionesDePagoEntrega;
                                objCompa.caracteristicasDelEquipo1 = item.caracteristicasDelEquipo1;
                                objCompa.caracteristicasDelEquipo2 = item.caracteristicasDelEquipo2;
                                objCompa.caracteristicasDelEquipo3 = item.caracteristicasDelEquipo3;
                                objCompa.caracteristicasDelEquipo4 = item.caracteristicasDelEquipo4;
                                objCompa.caracteristicasDelEquipo5 = item.caracteristicasDelEquipo5;
                                objCompa.caracteristicasDelEquipo6 = item.caracteristicasDelEquipo6;
                                objCompa.caracteristicasDelEquipo7 = item.caracteristicasDelEquipo7;

                                _context.tblM_ComparativoAdquisicionyRentaDet.Add(objCompa);
                                _context.SaveChanges();
                            }

                            foreach (var objetos in item.lstCaracteristicas)
                            {

                                tblM_ComparativosAdquisicionyRentaCaracteristicasDet objCaracteristicas = _context.tblM_ComparativosAdquisicionyRentaCaracteristicasDet.Where(r => r.id == objetos.id && r.idComparativoDetalle == objetos.idComparativoDetalle).FirstOrDefault();
                                if (objetos.Descripcion != null)
                                {
                                    if (objCaracteristicas == null)
                                    {
                                        var id = (from i in _context.tblM_ComparativoAdquisicionyRentaDet orderby i.id descending select i.id).FirstOrDefault();
                                        if (objetos.Descripcion != "undefined")
                                        {
                                            objCaracteristicas = new tblM_ComparativosAdquisicionyRentaCaracteristicasDet();
                                            objCaracteristicas.idComparativoDetalle = id;
                                            objCaracteristicas.idRow = objetos.idRow;
                                            objCaracteristicas.Descripcion = objetos.Descripcion;
                                            _context.tblM_ComparativosAdquisicionyRentaCaracteristicasDet.Add(objCaracteristicas);
                                            _context.SaveChanges();
                                        }
                                    }
                                    else
                                    {
                                        if (objetos.Descripcion != "")
                                        {
                                            objCaracteristicas.Descripcion = objetos.Descripcion;
                                        }
                                        _context.SaveChanges();
                                    }
                                }
                            }

                            objComparado.msjExito = "Guardado con exito";
                        }
                        else
                        {
                            objCompa.idRow = item.idRow;
                            objCompa.idComparativo = item.idComparativo;
                            objCompa.marcaModelo = item.marcaModelo;
                            objCompa.proveedor = item.proveedor;
                            objCompa.precioDeVenta = item.precioDeVenta;
                            objCompa.tradeIn = item.tradeIn;
                            objCompa.valoresDeRecompra = item.valoresDeRecompra;
                            objCompa.precioDeRentaPura = item.precioDeRentaPura;
                            objCompa.precioDeRentaEnRoc = item.precioDeRentaEnRoc;
                            objCompa.baseHoras = item.baseHoras;
                            objCompa.tiempoDeEntrega = item.tiempoDeEntrega;
                            objCompa.ubicacion = item.ubicacion;
                            objCompa.horas = item.horas;
                            objCompa.seguro = item.seguro;
                            objCompa.garantia = item.garantia;
                            objCompa.serviciosPreventivos = item.serviciosPreventivos;
                            objCompa.capacitacion = item.capacitacion;
                            objCompa.depositoEnGarantia = item.depositoEnGarantia;
                            objCompa.lugarDeEntrega = item.lugarDeEntrega;
                            objCompa.flete = item.flete;
                            objCompa.condicionesDePagoEntrega = item.condicionesDePagoEntrega;
                            objCompa.caracteristicasDelEquipo1 = item.caracteristicasDelEquipo1;
                            objCompa.caracteristicasDelEquipo2 = item.caracteristicasDelEquipo2;
                            objCompa.caracteristicasDelEquipo3 = item.caracteristicasDelEquipo3;
                            objCompa.caracteristicasDelEquipo4 = item.caracteristicasDelEquipo4;
                            objCompa.caracteristicasDelEquipo5 = item.caracteristicasDelEquipo5;
                            objCompa.caracteristicasDelEquipo6 = item.caracteristicasDelEquipo6;
                            objCompa.caracteristicasDelEquipo7 = item.caracteristicasDelEquipo7;


                            _context.SaveChanges();
                            foreach (var objetos in item.lstCaracteristicas)
                            {
                                tblM_ComparativosAdquisicionyRentaCaracteristicasDet objCaracteristicas = _context.tblM_ComparativosAdquisicionyRentaCaracteristicasDet.Where(r => r.id == objetos.id && r.idComparativoDetalle == objetos.idComparativoDetalle).FirstOrDefault();
                                if (objetos.Descripcion != null)
                                {
                                    if (objCaracteristicas != null)
                                    {
                                        if (objetos.Descripcion != "")
                                        {
                                            objCaracteristicas.Descripcion = objetos.Descripcion;
                                        }
                                        _context.SaveChanges();
                                    }
                                    else
                                    {
                                        objCaracteristicas = new tblM_ComparativosAdquisicionyRentaCaracteristicasDet();
                                        objCaracteristicas.idComparativoDetalle = objetos.idComparativoDetalle;
                                        objCaracteristicas.idRow = objetos.idRow;
                                        objCaracteristicas.Descripcion = objetos.Descripcion;
                                        _context.tblM_ComparativosAdquisicionyRentaCaracteristicasDet.Add(objCaracteristicas);
                                        _context.SaveChanges();
                                    }
                                }
                            }



                            objComparado.msjExito = "Editado con exito";
                        }
                    }
                }


                foreach (var arch in listaRutaArchivos)
                {
                    if (GlobalUtils.SaveHTTPPostedFile(arch.Item1, arch.Item2) == false)
                    {
                        resultado.Clear();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                    }
                }


                return objComparado;


            }
            catch (Exception e)
            {
                objComparado.msjExito = "Algo ocurrio favor de ponerse en contacto con el departamento de TI";
                return objComparado;
            }
        }
        public Dictionary<string, object> CargarCuadrosComparativos()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var listaCuadros = _context.tblM_ComparativoAdquisicionyRenta.ToList();

                resultado.Add("data", listaCuadros);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, "CatMaquinaController", "CargarCuadrosComparativos", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }
        public Dictionary<string, object> GuardarAsignacionSolicitud(int idCuadro, string folio)
        {
            resultado = new Dictionary<string, object>();

            using (var dbSigoplanTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var cuadroSIGOPLAN = _context.tblM_ComparativoAdquisicionyRenta.FirstOrDefault(x => x.id == idCuadro);

                    if (cuadroSIGOPLAN != null)
                    {
                        var solicitudSIGOPLAN = _context.tblM_SolicitudEquipo.FirstOrDefault(x => x.folio == folio);

                        if (solicitudSIGOPLAN != null)
                        {
                            var asignacionSIGOPLAN = _context.tblM_AsignacionEquipos.FirstOrDefault(x => x.solicitudEquipoID == solicitudSIGOPLAN.id);

                            if (asignacionSIGOPLAN != null)
                            {
                                cuadroSIGOPLAN.idAsignacion = asignacionSIGOPLAN.id;
                                _context.SaveChanges();
                            }
                            else
                            {
                                throw new Exception("No se encuentra la información de la asignación.");
                            }
                        }
                        else
                        {
                            throw new Exception("No se encuentra la información de la solicitud.");
                        }
                    }
                    else
                    {
                        throw new Exception("No se encuentra la información del cuadro.");
                    }

                    dbSigoplanTransaction.Commit();
                    SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, 0, JsonUtils.convertNetObjectToJson(new { idCuadro = idCuadro, folio = folio }));

                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbSigoplanTransaction.Rollback();

                    LogError(0, 0, "CatMaquinaController", "GuardarAsignacionSolicitud", e, AccionEnum.AGREGAR, 0, new { idCuadro = idCuadro, folio = folio });

                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }
        public bool deleteTablaComparativoAdiquisicion(int id)
        {
            return true;
        }
        public ComparativoDTO addeditTablaComparativoFinanciero(List<ComparativoDTO> lstFinanciero)
        {
            ComparativoDTO objComparativo = new ComparativoDTO();
            tblM_ComparativoFinancieroDet objCom;
            try
            {

                if (lstFinanciero != null)
                {
                    int idAsignacion = lstFinanciero.Select(y => y.idFinanciero).FirstOrDefault();
                    var objComparativos = _context.tblM_ComparativoAdquisicionyRenta.Where(r => r.idAsignacion == idAsignacion).FirstOrDefault();
                    objComparativos.ComentarioGeneral = lstFinanciero.Select(y => y.ComentarioGeneral).FirstOrDefault();
                    objComparativos.fechaDeElaboracionFinanciero = DateTime.Now;
                    _context.SaveChanges();

                    foreach (var item in lstFinanciero)
                    {

                        objCom = _context.tblM_ComparativoFinancieroDet.Where(x => x.id == item.id).FirstOrDefault();
                        if (objCom == null)
                        {
                            objCom = new tblM_ComparativoFinancieroDet();

                            objCom.idFinanciero = item.idFinanciero;
                            objCom.idRow = item.idRow;
                            objCom.banco = item.banco;
                            objCom.plazo = item.plazo;
                            objCom.precioDelEquipo = item.precioDelEquipo;
                            objCom.tiempoRestanteProyecto = item.tiempoRestanteProyecto;
                            objCom.iva = item.iva;
                            objCom.total = item.total;
                            objCom.montoFinanciar = item.montoFinanciar;
                            objCom.tipoOperacion = item.tipoOperacion;
                            objCom.opcionCompra = item.opcionCompra;
                            objCom.valorResidual = item.valorResidual;
                            objCom.depositoEfectivo = item.depositoEfectivo;
                            objCom.moneda = item.moneda;
                            objCom.plazoMeses = item.plazoMeses;
                            objCom.tasaDeInteres = item.tasaDeInteres;
                            objCom.gastosFijos = item.gastosFijos;
                            objCom.comision = item.comision;
                            objCom.montoComision = item.montoComision;
                            objCom.rentasEnGarantia = item.rentasEnGarantia;
                            objCom.crecimientoPagos = item.crecimientoPagos;
                            objCom.pagoInicial = item.pagoInicial;
                            objCom.pagoTotalIntereses = item.pagoTotalIntereses;
                            objCom.tasaEfectiva = item.tasaEfectiva;
                            objCom.mensualidad = item.mensualidad;
                            objCom.mensualidadSinIVA = item.mensualidadSinIVA;
                            objCom.pagoTotal = item.pagoTotal;


                            _context.tblM_ComparativoFinancieroDet.Add(objCom);
                            _context.SaveChanges();

                        }
                        else
                        {
                            objCom.idFinanciero = item.idFinanciero;
                            objCom.idRow = item.idRow;
                            objCom.banco = item.banco;
                            objCom.plazo = item.plazo;
                            objCom.precioDelEquipo = item.precioDelEquipo;
                            objCom.tiempoRestanteProyecto = item.tiempoRestanteProyecto;
                            objCom.iva = item.iva;
                            objCom.total = item.total;
                            objCom.montoFinanciar = item.montoFinanciar;
                            objCom.tipoOperacion = item.tipoOperacion;
                            objCom.opcionCompra = item.opcionCompra;
                            objCom.valorResidual = item.valorResidual;
                            objCom.depositoEfectivo = item.depositoEfectivo;
                            objCom.moneda = item.moneda;
                            objCom.plazoMeses = item.plazoMeses;
                            objCom.tasaDeInteres = item.tasaDeInteres;
                            objCom.gastosFijos = item.gastosFijos;
                            objCom.comision = item.comision;
                            objCom.montoComision = item.montoComision;
                            objCom.rentasEnGarantia = item.rentasEnGarantia;
                            objCom.crecimientoPagos = item.crecimientoPagos;
                            objCom.pagoInicial = item.pagoInicial;
                            objCom.pagoTotalIntereses = item.pagoTotalIntereses;
                            objCom.tasaEfectiva = item.tasaEfectiva;
                            objCom.mensualidad = item.mensualidad;
                            objCom.mensualidadSinIVA = item.mensualidadSinIVA;
                            objCom.pagoTotal = item.pagoTotal;


                            _context.SaveChanges();
                        }

                    }
                }

                return objComparativo;
            }
            catch (Exception ex)
            {
                objComparativo.msjExito = ex.Message.ToString();
                return objComparativo;
            }
        }
        public bool deleteTablaComparativoFinanciero(int id)
        {
            return true;
        }
        public List<ComparativoDTO> CargarCuadroComparativo()
        {
            List<ComparativoDTO> lstComparativo = new List<ComparativoDTO>();
            try
            {

                lstComparativo = _context.tblM_ComparativoAdquisicionyRenta.ToList().Select(y => new ComparativoDTO
                {
                    id = y.id,
                }).ToList();

                return lstComparativo;
            }
            catch (Exception)
            {
                return lstComparativo;
            }
        }
        public List<ComparativoDTO> getTablaComparativoAutorizar(ComparativoDTO objFiltro)
        {
            try
            {
                resultado = new Dictionary<string, object>();
                ComparativoDTO obj = new ComparativoDTO();
                List<ComparativoDTO> lstComparativo = new List<ComparativoDTO>();
                #region HEADER


                obj = new ComparativoDTO();
                obj.header = "check";
                obj.txtIdnumero1 = "btnidMarcaNum1Marca";
                obj.txtIdnumero2 = "btnidMarcaNum2Marca";
                obj.txtIdnumero3 = "btnidMarcaNum3Marca";
                obj.txtIdnumero4 = "btnidMarcaNum4Marca";
                obj.txtIdnumero5 = "btnidMarcaNum5Marca";
                obj.txtIdnumero6 = "btnidMarcaNum6Marca";
                obj.txtIdnumero7 = "btnidMarcaNum7Marca";
                lstComparativo.Add(obj);

                obj = new ComparativoDTO();
                obj.header = "Marca Modelo";
                obj.txtIdnumero1 = "idMarcaNum1Marca";
                obj.txtIdnumero2 = "idMarcaNum2Marca";
                obj.txtIdnumero3 = "idMarcaNum3Marca";
                obj.txtIdnumero4 = "idMarcaNum4Marca";
                obj.txtIdnumero5 = "idMarcaNum5Marca";
                obj.txtIdnumero6 = "idMarcaNum6Marca";
                obj.txtIdnumero7 = "idMarcaNum7Marca";
                lstComparativo.Add(obj);

                obj = new ComparativoDTO();
                obj.header = "proveedor";
                obj.txtIdnumero1 = "idMarcaNum1proveedor";
                obj.txtIdnumero2 = "idMarcaNum2proveedor";
                obj.txtIdnumero3 = "idMarcaNum3proveedor";
                obj.txtIdnumero4 = "idMarcaNum4proveedor";
                obj.txtIdnumero5 = "idMarcaNum5proveedor";
                obj.txtIdnumero6 = "idMarcaNum6proveedor";
                obj.txtIdnumero7 = "idMarcaNum7proveedor";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Precio de venta";
                obj.txtIdnumero1 = "idMarcaNum1precio";
                obj.txtIdnumero2 = "idMarcaNum2precio";
                obj.txtIdnumero3 = "idMarcaNum3precio";
                obj.txtIdnumero4 = "idMarcaNum4precio";
                obj.txtIdnumero5 = "idMarcaNum5precio";
                obj.txtIdnumero6 = "idMarcaNum6precio";
                obj.txtIdnumero7 = "idMarcaNum7precio";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Trade IN";
                obj.txtIdnumero1 = "idMarcaNum1Trade";
                obj.txtIdnumero2 = "idMarcaNum2Trade";
                obj.txtIdnumero3 = "idMarcaNum3Trade";
                obj.txtIdnumero4 = "idMarcaNum4Trade";
                obj.txtIdnumero5 = "idMarcaNum5Trade";
                obj.txtIdnumero6 = "idMarcaNum6Trade";
                obj.txtIdnumero7 = "idMarcaNum7Trade";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Valores de recompra";
                obj.txtIdnumero1 = "idMarcaNum1Valores";
                obj.txtIdnumero2 = "idMarcaNum2Valores";
                obj.txtIdnumero3 = "idMarcaNum3Valores";
                obj.txtIdnumero4 = "idMarcaNum4Valores";
                obj.txtIdnumero5 = "idMarcaNum5Valores";
                obj.txtIdnumero6 = "idMarcaNum6Valores";
                obj.txtIdnumero7 = "idMarcaNum7Valores";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Precio de renta pura";
                obj.txtIdnumero1 = "idMarcaNum1Precio";
                obj.txtIdnumero2 = "idMarcaNum2Precio";
                obj.txtIdnumero3 = "idMarcaNum3Precio";
                obj.txtIdnumero4 = "idMarcaNum4Precio";
                obj.txtIdnumero5 = "idMarcaNum5Precio";
                obj.txtIdnumero6 = "idMarcaNum6Precio";
                obj.txtIdnumero7 = "idMarcaNum7Precio";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Precio de renta en ROC";
                obj.txtIdnumero1 = "idMarcaNum1PrecioRoc";
                obj.txtIdnumero2 = "idMarcaNum2PrecioRoc";
                obj.txtIdnumero3 = "idMarcaNum3PrecioRoc";
                obj.txtIdnumero4 = "idMarcaNum4PrecioRoc";
                obj.txtIdnumero5 = "idMarcaNum5PrecioRoc";
                obj.txtIdnumero6 = "idMarcaNum6PrecioRoc";
                obj.txtIdnumero7 = "idMarcaNum7PrecioRoc";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Base horas";
                obj.txtIdnumero1 = "idMarcaNum1BaseHoras";
                obj.txtIdnumero2 = "idMarcaNum2BaseHoras";
                obj.txtIdnumero3 = "idMarcaNum3BaseHoras";
                obj.txtIdnumero4 = "idMarcaNum4BaseHoras";
                obj.txtIdnumero5 = "idMarcaNum5BaseHoras";
                obj.txtIdnumero6 = "idMarcaNum6BaseHoras";
                obj.txtIdnumero7 = "idMarcaNum7BaseHoras";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Tiempo de entrega";
                obj.txtIdnumero1 = "idMarcaNum1Tiempo";
                obj.txtIdnumero2 = "idMarcaNum2Tiempo";
                obj.txtIdnumero3 = "idMarcaNum3Tiempo";
                obj.txtIdnumero4 = "idMarcaNum4Tiempo";
                obj.txtIdnumero5 = "idMarcaNum5Tiempo";
                obj.txtIdnumero6 = "idMarcaNum6Tiempo";
                obj.txtIdnumero7 = "idMarcaNum7Tiempo";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Ubicacion";
                obj.txtIdnumero1 = "idMarcaNum1Ubicacion";
                obj.txtIdnumero2 = "idMarcaNum2Ubicacion";
                obj.txtIdnumero3 = "idMarcaNum3Ubicacion";
                obj.txtIdnumero4 = "idMarcaNum4Ubicacion";
                obj.txtIdnumero5 = "idMarcaNum5Ubicacion";
                obj.txtIdnumero6 = "idMarcaNum6Ubicacion";
                obj.txtIdnumero7 = "idMarcaNum7Ubicacion";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Horas";
                obj.txtIdnumero1 = "idMarcaNum1Horas";
                obj.txtIdnumero2 = "idMarcaNum2Horas";
                obj.txtIdnumero3 = "idMarcaNum3Horas";
                obj.txtIdnumero4 = "idMarcaNum4Horas";
                obj.txtIdnumero5 = "idMarcaNum5Horas";
                obj.txtIdnumero6 = "idMarcaNum6Horas";
                obj.txtIdnumero7 = "idMarcaNum7Horas";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Seguro";
                obj.txtIdnumero1 = "idMarcaNum1Seguro";
                obj.txtIdnumero2 = "idMarcaNum2Seguro";
                obj.txtIdnumero3 = "idMarcaNum3Seguro";
                obj.txtIdnumero4 = "idMarcaNum4Seguro";
                obj.txtIdnumero5 = "idMarcaNum5Seguro";
                obj.txtIdnumero6 = "idMarcaNum6Seguro";
                obj.txtIdnumero7 = "idMarcaNum7Seguro";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Garantia";
                obj.txtIdnumero1 = "idMarcaNum1Garantia";
                obj.txtIdnumero2 = "idMarcaNum2Garantia";
                obj.txtIdnumero3 = "idMarcaNum3Garantia";
                obj.txtIdnumero4 = "idMarcaNum4Garantia";
                obj.txtIdnumero5 = "idMarcaNum5Garantia";
                obj.txtIdnumero6 = "idMarcaNum6Garantia";
                obj.txtIdnumero7 = "idMarcaNum7Garantia";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Servicios preventivos";
                obj.txtIdnumero1 = "idMarcaNum1Servicios";
                obj.txtIdnumero2 = "idMarcaNum2Servicios";
                obj.txtIdnumero3 = "idMarcaNum3Servicios";
                obj.txtIdnumero4 = "idMarcaNum4Servicios";
                obj.txtIdnumero5 = "idMarcaNum5Servicios";
                obj.txtIdnumero6 = "idMarcaNum6Servicios";
                obj.txtIdnumero7 = "idMarcaNum7Servicios";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Capacitacion";
                obj.txtIdnumero1 = "idMarcaNum1Capacitacion";
                obj.txtIdnumero2 = "idMarcaNum2Capacitacion";
                obj.txtIdnumero3 = "idMarcaNum3Capacitacion";
                obj.txtIdnumero4 = "idMarcaNum4Capacitacion";
                obj.txtIdnumero5 = "idMarcaNum5Capacitacion";
                obj.txtIdnumero6 = "idMarcaNum6Capacitacion";
                obj.txtIdnumero7 = "idMarcaNum7Capacitacion";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Deposito en garantia";
                obj.txtIdnumero1 = "idMarcaNum1Deposito";
                obj.txtIdnumero2 = "idMarcaNum2Deposito";
                obj.txtIdnumero3 = "idMarcaNum3Deposito";
                obj.txtIdnumero4 = "idMarcaNum4Deposito";
                obj.txtIdnumero5 = "idMarcaNum5Deposito";
                obj.txtIdnumero6 = "idMarcaNum6Deposito";
                obj.txtIdnumero7 = "idMarcaNum7Deposito";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Lugar de entrega";
                obj.txtIdnumero1 = "idMarcaNum1Lugar";
                obj.txtIdnumero2 = "idMarcaNum2Lugar";
                obj.txtIdnumero3 = "idMarcaNum3Lugar";
                obj.txtIdnumero4 = "idMarcaNum4Lugar";
                obj.txtIdnumero5 = "idMarcaNum5Lugar";
                obj.txtIdnumero6 = "idMarcaNum6Lugar";
                obj.txtIdnumero7 = "idMarcaNum7Lugar";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Flete";
                obj.txtIdnumero1 = "idMarcaNum1Flete";
                obj.txtIdnumero2 = "idMarcaNum2Flete";
                obj.txtIdnumero3 = "idMarcaNum3Flete";
                obj.txtIdnumero4 = "idMarcaNum4Flete";
                obj.txtIdnumero5 = "idMarcaNum5Flete";
                obj.txtIdnumero6 = "idMarcaNum6Flete";
                obj.txtIdnumero7 = "idMarcaNum7Flete";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Condiciones de pago entrega";
                obj.txtIdnumero1 = "idMarcaNum1Condiciones";
                obj.txtIdnumero2 = "idMarcaNum2Condiciones";
                obj.txtIdnumero3 = "idMarcaNum3Condiciones";
                obj.txtIdnumero4 = "idMarcaNum4Condiciones";
                obj.txtIdnumero5 = "idMarcaNum5Condiciones";
                obj.txtIdnumero6 = "idMarcaNum6Condiciones";
                obj.txtIdnumero7 = "idMarcaNum7Condiciones";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO()
                {
                    header = "Comentarios",
                    txtIdnumero1 = "idMarcaNum1Comentarios",
                    txtIdnumero2 = "idMarcaNum2Comentarios",
                    txtIdnumero3 = "idMarcaNum3Comentarios",
                    txtIdnumero4 = "idMarcaNum4Comentarios",
                    txtIdnumero5 = "idMarcaNum5Comentarios",
                    txtIdnumero6 = "idMarcaNum6Comentarios",
                    txtIdnumero7 = "idMarcaNum7Comentarios"
                };
                obj = new ComparativoDTO();
                obj.header = "Caracteristicas del equipo";
                obj.txtIdnumero1 = "idMarcaNum1Caracteristicas";
                obj.txtIdnumero2 = "idMarcaNum2Caracteristicas";
                obj.txtIdnumero3 = "idMarcaNum3Caracteristicas";
                obj.txtIdnumero4 = "idMarcaNum4Caracteristicas";
                obj.txtIdnumero5 = "idMarcaNum5Caracteristicas";
                obj.txtIdnumero6 = "idMarcaNum6Caracteristicas";
                obj.txtIdnumero7 = "idMarcaNum7Caracteristicas";
                lstComparativo.Add(obj);

                tblM_ComparativoAdquisicionyRenta objRenta;

                if (objFiltro.idAsignacion > 0)
                {
                    objRenta = _context.tblM_ComparativoAdquisicionyRenta.Where(x => x.idAsignacion == objFiltro.idAsignacion).FirstOrDefault();
                }
                else
                {
                    objRenta = _context.tblM_ComparativoAdquisicionyRenta.Where(x => x.id == objFiltro.idCuadro).FirstOrDefault();
                }
                
                var objDetalle = _context.tblM_ComparativoAdquisicionyRentaDet.Where(x => x.idComparativo == objRenta.id).FirstOrDefault();
                obj = new ComparativoDTO();
                obj.header = objDetalle.caracteristicasDelEquipo1 != null ? objDetalle.caracteristicasDelEquipo1 : "Caracteristica1";
                obj.txtIdnumero1 = "idMarcaNum1Caracteristicas11";
                obj.txtIdnumero2 = "idMarcaNum2Caracteristicas21";
                obj.txtIdnumero3 = "idMarcaNum3Caracteristicas31";
                obj.txtIdnumero4 = "idMarcaNum4Caracteristicas41";
                obj.txtIdnumero5 = "idMarcaNum5Caracteristicas51";
                obj.txtIdnumero6 = "idMarcaNum6Caracteristicas61";
                obj.txtIdnumero7 = "idMarcaNum7Caracteristicas71";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = objDetalle.caracteristicasDelEquipo2 != null ? objDetalle.caracteristicasDelEquipo2 : "Caracteristica2";
                obj.txtIdnumero1 = "idMarcaNum1Caracteristicas12";
                obj.txtIdnumero2 = "idMarcaNum2Caracteristicas22";
                obj.txtIdnumero3 = "idMarcaNum3Caracteristicas32";
                obj.txtIdnumero4 = "idMarcaNum4Caracteristicas42";
                obj.txtIdnumero5 = "idMarcaNum5Caracteristicas52";
                obj.txtIdnumero6 = "idMarcaNum6Caracteristicas62";
                obj.txtIdnumero7 = "idMarcaNum7Caracteristicas72";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = objDetalle.caracteristicasDelEquipo3 != null ? objDetalle.caracteristicasDelEquipo3 : "Caracteristica3";
                obj.txtIdnumero1 = "idMarcaNum1Caracteristicas13";
                obj.txtIdnumero2 = "idMarcaNum2Caracteristicas23";
                obj.txtIdnumero3 = "idMarcaNum3Caracteristicas33";
                obj.txtIdnumero4 = "idMarcaNum4Caracteristicas43";
                obj.txtIdnumero5 = "idMarcaNum5Caracteristicas53";
                obj.txtIdnumero6 = "idMarcaNum6Caracteristicas63";
                obj.txtIdnumero7 = "idMarcaNum7Caracteristicas73";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = objDetalle.caracteristicasDelEquipo4 != null ? objDetalle.caracteristicasDelEquipo4 : "Caracteristica4";
                obj.txtIdnumero1 = "idMarcaNum1Caracteristicas14";
                obj.txtIdnumero2 = "idMarcaNum2Caracteristicas24";
                obj.txtIdnumero3 = "idMarcaNum3Caracteristicas34";
                obj.txtIdnumero4 = "idMarcaNum4Caracteristicas44";
                obj.txtIdnumero5 = "idMarcaNum5Caracteristicas54";
                obj.txtIdnumero6 = "idMarcaNum6Caracteristicas64";
                obj.txtIdnumero7 = "idMarcaNum7Caracteristicas74";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = objDetalle.caracteristicasDelEquipo5 != null ? objDetalle.caracteristicasDelEquipo5 : "Caracteristica5";
                obj.txtIdnumero1 = "idMarcaNum1Caracteristicas15";
                obj.txtIdnumero2 = "idMarcaNum2Caracteristicas25";
                obj.txtIdnumero3 = "idMarcaNum3Caracteristicas35";
                obj.txtIdnumero4 = "idMarcaNum4Caracteristicas45";
                obj.txtIdnumero5 = "idMarcaNum5Caracteristicas55";
                obj.txtIdnumero6 = "idMarcaNum6Caracteristicas65";
                obj.txtIdnumero7 = "idMarcaNum7Caracteristicas75";
                lstComparativo.Add(obj);

                obj = new ComparativoDTO();
                obj.header = objDetalle.caracteristicasDelEquipo6 != null ? objDetalle.caracteristicasDelEquipo6 : "Caracteristica6";
                obj.txtIdnumero1 = "idMarcaNum1Caracteristicas16";
                obj.txtIdnumero2 = "idMarcaNum2Caracteristicas26";
                obj.txtIdnumero3 = "idMarcaNum3Caracteristicas36";
                obj.txtIdnumero4 = "idMarcaNum4Caracteristicas46";
                obj.txtIdnumero5 = "idMarcaNum5Caracteristicas56";
                obj.txtIdnumero6 = "idMarcaNum6Caracteristicas66";
                obj.txtIdnumero7 = "idMarcaNum7Caracteristicas76";
                lstComparativo.Add(obj);

                obj = new ComparativoDTO();
                obj.header = objDetalle.caracteristicasDelEquipo7 != null ? objDetalle.caracteristicasDelEquipo7 : "Caracteristica7";
                obj.txtIdnumero1 = "idMarcaNum1Caracteristicas17";
                obj.txtIdnumero2 = "idMarcaNum2Caracteristicas27";
                obj.txtIdnumero3 = "idMarcaNum3Caracteristicas37";
                obj.txtIdnumero4 = "idMarcaNum4Caracteristicas47";
                obj.txtIdnumero5 = "idMarcaNum5Caracteristicas57";
                obj.txtIdnumero6 = "idMarcaNum6Caracteristicas67";
                obj.txtIdnumero7 = "idMarcaNum7Caracteristicas77";
                lstComparativo.Add(obj);
                #endregion
                return lstComparativo;
            }
            catch (Exception e)
            {

                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error interno al intentar obtener el catálogo tipos de emleados");
                return null;
            }
        }
        public ComparativoDTO guardarAutorizacion(List<tblM_ComparativoAdquisicionyRentaAutorizante> lstComparativo, ComparativoDTO objFiltro, bool Financiero, int idUsuario)
        {
            ComparativoDTO objComparativo = new ComparativoDTO();
            try
            {
                int idAsignacion = 0;
                tblP_Alerta objAlerta;
                if (Financiero == false)
                {
                    #region ADD
                    List<tblM_ComparativoAdquisicionyRentaAutorizante> lstCom = new List<tblM_ComparativoAdquisicionyRentaAutorizante>();
                    tblM_ComparativoAdquisicionyRentaAutorizante objComparativos;
                    int conCom = 0;
                    foreach (var item in lstComparativo)
                    {
                        if (item.autorizanteID != 0)
                        {
                            conCom++;
                            #region ADD COMPARATIVO
                            idAsignacion = item.idAsignacion;
                            objComparativos = _context.tblM_ComparativoAdquisicionyRentaAutorizante.Where(r => r.id == item.id).FirstOrDefault();
                            if (objComparativos == null)
                            {
                                objComparativos = new tblM_ComparativoAdquisicionyRentaAutorizante();
                                var obj = _context.tblP_Usuario.Where(x => x.id == item.autorizanteID).FirstOrDefault();
                                objComparativos.idAsignacion = item.idAsignacion;
                                objComparativos.idComparativoDetalle = item.idComparativoDetalle;
                                objComparativos.autorizanteID = item.autorizanteID;
                                objComparativos.autorizanteNombre = item.autorizanteNombre;
                                objComparativos.autorizantePuesto = _context.tblP_Puesto.Where(x => x.id == obj.puestoID).Select(y => y.descripcion).FirstOrDefault();
                                objComparativos.autorizanteStatus = item.autorizanteStatus;
                                objComparativos.autorizanteFinal = item.autorizanteFinal;
                                objComparativos.firma = "";
                                objComparativos.tipo = item.tipo;
                                objComparativos.orden = conCom;
                                objComparativos.comentario = item.comentario;
                                _context.tblM_ComparativoAdquisicionyRentaAutorizante.Add(objComparativos);
                                _context.SaveChanges();

                                var objFolio = _context.tblM_AsignacionEquipos.Where(r => r.id == idAsignacion).FirstOrDefault();

                                var objC = _context.tblM_ComparativoAdquisicionyRentaAutorizante.OrderByDescending(y => y.id).FirstOrDefault();
                                objAlerta = new tblP_Alerta();
                                objAlerta.userEnviaID = idUsuario;
                                objAlerta.userRecibeID = item.autorizanteID;
                                objAlerta.tipoAlerta = 2;
                                objAlerta.sistemaID = 1;
                                objAlerta.url = "/CatMaquina/AutorizarAsignacionNoEconomico";
                                objAlerta.objID = objC.id;
                                objAlerta.obj = null;
                                objAlerta.msj = "Cuadro comparativo " + objFolio.folio;
                                objAlerta.documentoID = item.idAsignacion;
                                objAlerta.moduloID = Convert.ToInt32(BitacoraEnum.AutorizacionFinanciero);
                                _context.tblP_Alerta.Add(objAlerta);
                                _context.SaveChanges();



                            }
                            else
                            {
                                var obj = _context.tblP_Usuario.Where(x => x.id == item.autorizanteID).FirstOrDefault();
                                objComparativos.idAsignacion = item.idAsignacion;
                                objComparativos.idComparativoDetalle = item.idComparativoDetalle;
                                objComparativos.autorizanteID = item.autorizanteID;
                                objComparativos.autorizanteNombre = item.autorizanteNombre;
                                objComparativos.autorizantePuesto = _context.tblP_Puesto.Where(x => x.id == obj.puestoID).Select(y => y.descripcion).FirstOrDefault();
                                objComparativos.autorizanteStatus = item.autorizanteStatus;
                                objComparativos.autorizanteFinal = item.autorizanteFinal;
                                objComparativos.firma = "";
                                objComparativos.tipo = item.tipo;
                                objComparativos.orden = item.orden;
                                objComparativos.comentario = item.comentario;
                                _context.SaveChanges();
                            }
                            #endregion
                        }

                    }

                    #endregion
                    var a = editEstatusComparativo(0, idAsignacion, objFiltro);
                    var obtenerAutorizanteFinal = _context.tblM_ComparativoAdquisicionyRentaAutorizante.Where(r => r.idAsignacion == idAsignacion).OrderByDescending(n => n.orden).FirstOrDefault();
                    obtenerAutorizanteFinal.autorizanteFinal = true;
                    _context.SaveChanges();

                }
                else
                {
                    List<tblM_ComparativoFinancieroAutorizante> lstCom = new List<tblM_ComparativoFinancieroAutorizante>();
                    tblM_ComparativoFinancieroAutorizante objComparativos;
                    int contFin = 0;
                    foreach (var item in lstComparativo)
                    {
                        if (item.autorizanteID != 0)
                        {
                            contFin++;
                            #region ADD FINANCIERO
                            idAsignacion = item.idAsignacion;
                            objComparativos = _context.tblM_ComparativoFinancieroAutorizante.Where(r => r.id == item.id).FirstOrDefault();
                            if (objComparativos == null)
                            {

                                objComparativos = new tblM_ComparativoFinancieroAutorizante();
                                var obj = _context.tblP_Usuario.Where(x => x.id == item.autorizanteID).FirstOrDefault();
                                objComparativos.idAsignacion = item.idAsignacion;
                                objComparativos.idFinanciero = item.idComparativoDetalle;
                                objComparativos.autorizanteID = item.autorizanteID;
                                objComparativos.autorizanteNombre = item.autorizanteNombre;
                                objComparativos.autorizantePuesto = _context.tblP_Puesto.Where(x => x.id == obj.puestoID).Select(y => y.descripcion).FirstOrDefault();
                                objComparativos.autorizanteStatus = item.autorizanteStatus;
                                objComparativos.autorizanteFinal = item.autorizanteFinal;
                                objComparativos.firma = "";
                                objComparativos.tipo = item.tipo;
                                objComparativos.orden = contFin;
                                objComparativos.comentario = item.comentario;
                                _context.tblM_ComparativoFinancieroAutorizante.Add(objComparativos);
                                _context.SaveChanges();
                                var objFolio = _context.tblM_AsignacionEquipos.Where(r => r.id == idAsignacion).FirstOrDefault();

                                var objC = _context.tblM_ComparativoFinancieroAutorizante.OrderByDescending(y => y.id).FirstOrDefault();
                                objAlerta = new tblP_Alerta();
                                objAlerta.userEnviaID = idUsuario;
                                objAlerta.userRecibeID = item.autorizanteID;
                                objAlerta.tipoAlerta = 2;
                                objAlerta.sistemaID = 1;
                                objAlerta.url = "/CatMaquina/AutorizarAsignacionNoEconomico";
                                objAlerta.objID = objC.id;
                                objAlerta.obj = null;
                                objAlerta.msj = "Cuadro comparativo " + objFolio.folio;
                                objAlerta.documentoID = item.idAsignacion;
                                objAlerta.moduloID = Convert.ToInt32(BitacoraEnum.AutorizacionFinanciero);
                                _context.tblP_Alerta.Add(objAlerta);
                                _context.SaveChanges();




                            }
                            else
                            {
                                var obj = _context.tblP_Usuario.Where(x => x.id == item.autorizanteID).FirstOrDefault();
                                objComparativos.idAsignacion = item.idAsignacion;
                                objComparativos.idFinanciero = item.idComparativoDetalle;
                                objComparativos.autorizanteID = item.autorizanteID;
                                objComparativos.autorizanteNombre = item.autorizanteNombre;
                                objComparativos.autorizantePuesto = _context.tblP_Puesto.Where(x => x.id == obj.puestoID).Select(y => y.descripcion).FirstOrDefault();
                                objComparativos.autorizanteStatus = item.autorizanteStatus;
                                objComparativos.autorizanteFinal = item.autorizanteFinal;
                                objComparativos.firma = "";
                                objComparativos.tipo = item.tipo;
                                objComparativos.orden = item.orden;
                                objComparativos.comentario = item.comentario;
                                _context.SaveChanges();
                            }
                            #endregion
                        }
                    }

                    var a = editEstatusComparativo(2, idAsignacion, objFiltro);
                }
                if (Financiero == false)
                {

                    var lstAutorizadores = _context.tblM_ComparativoAdquisicionyRentaAutorizante.Where(r => r.idAsignacion == idAsignacion).ToList().Select(y => new ComparativoDTO
                    {

                        autorizanteID = y.autorizanteID,
                        autorizanteNombre = y.autorizanteNombre,
                        autorizantePuesto = y.autorizantePuesto,
                        autorizanteFecha = y.autorizanteFecha,
                        header = y.firma != "" ? "AUTORIZADO" : "PENDIENTE",
                        firma = y.firma,
                        voto = "Opcion " + _context.tblM_ComparativoAdquisicionyRentaDet.Where(s => s.id == y.idComparativoDetalle).ToList().Select(n => n.idRow).FirstOrDefault().ToString()
                    }).ToList();
                    foreach (var item in lstAutorizadores)
                    {
                        List<tblP_Usuario> lstUsuarios = new List<tblP_Usuario>();
                        lstUsuarios = _context.tblP_Usuario.Where(r => r.id == item.autorizanteID).ToList();
                        var subject = "Cuadro Comparativo Adquisicion y Renta de Maquinaria y Equipo ";
                        var body = @"Buen dia " + lstUsuarios.Select(y => y.nombre).FirstOrDefault() + " " + lstUsuarios.Select(y => y.apellidoPaterno).FirstOrDefault() + " " + lstUsuarios.Select(y => y.apellidoMaterno).FirstOrDefault()
                            + " usted tiene una autorizacion de cuadro comparativo de adquisicion y renta pendiente <br>"
                            + htmlCorreo(lstAutorizadores);

                        GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, lstUsuarios.Select(y => y.correo).ToList());
                    }
                }
                else
                {
                    var lstAutorizadores = _context.tblM_ComparativoFinancieroAutorizante.Where(r => r.idAsignacion == idAsignacion).ToList().Select(y => new ComparativoDTO
                    {
                        autorizanteID = y.autorizanteID,
                        autorizanteNombre = y.autorizanteNombre,
                        autorizantePuesto = y.autorizantePuesto,
                        autorizanteFecha = y.autorizanteFecha,
                        header = y.firma != "" ? "AUTORIZADO" : "PENDIENTE",
                        firma = y.firma,
                        voto = "Opcion " + _context.tblM_ComparativoFinancieroDet.Where(s => s.id == y.idFinanciero).ToList().Select(n => n.plazoMeses).FirstOrDefault().ToString()

                    }).ToList();
                    foreach (var item in lstAutorizadores)
                    {
                        List<tblP_Usuario> lstUsuarios = new List<tblP_Usuario>();
                        lstUsuarios = _context.tblP_Usuario.Where(r => r.id == item.autorizanteID).ToList();
                        var subject = "Cuadro Comparativo Adquisicion y/o Renta de Maquinaria de Equipo ";
                        var body = @"Buen dia " + lstUsuarios.Select(y => y.nombre).FirstOrDefault() + " " + lstUsuarios.Select(y => y.apellidoPaterno).FirstOrDefault() + " " + lstUsuarios.Select(y => y.apellidoMaterno).FirstOrDefault()
                            + " usted tiene una autorizacion de cuadro comparativo de adquisicion y renta pendiente <br>"
                            + htmlCorreo(lstAutorizadores);

                        GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, lstUsuarios.Select(y => y.correo).ToList());
                    }
                }
                return objComparativo;
            }
            catch (Exception ex)
            {
                return objComparativo;
            }
        }
        public string htmlCorreo(List<ComparativoDTO> lstAutorizadores)
        {
            string html = "";

            html += "<style>h3 {text-align: center;}table.dataTable tbody tr td, table thead tr th, table.dataTable, .dataTables_scrollBody {";
            html += "border: 0px solid #81bd72 !important;}table.dataTable thead {font-size: 15px;background-color: #81bd72;color: white;}";
            html += ".select2-container {width: 100% !important;}.seccion {padding: 15px 25px 15px 25px;margin: 10px 5px;background-color: white;";
            html += "border-radius: 4px 4px;box-shadow: 0 0 2px 0 rgba(0,0,0,0.14), 0 2px 2px 0 rgba(0,0,0,0.12), 0 1px 3px 0 rgba(0,0,0,0.2);}";
            html += ".my-card {position: absolute;left: 40%;top: -20px;border-radius: 50%;}#txtFechaInicio {background-color: #fff;}";
            html += "</style><br><table id='tblM_AutorizanteAdquisicion' class='table-bordered hover stripe order-column dataTable no-footer' role='grid'";
            html += "aria-describedby='tblM_AutorizanteAdquisicion_info' style='width: 0px;'>";
            html += "<thead>";
            html += "<tr role='row'>";
            html += "<th class='sorting_disabled' rowspan='1' colspan='1' style='width: 0px;'>Nombre</th>";
            html += "<th class='sorting_disabled' rowspan='1' colspan='1' style='width: 0px;'>Puesto</th>";
            html += "<th class='sorting_disabled' rowspan='1' colspan='1' style='width: 0px;'>Fecha Autorizacion</th>";
            html += "<th class='sorting_disabled' rowspan='1' colspan='1' style='width: 0px;'>Voto</th>";
            html += "<th class='sorting_disabled' rowspan='1' colspan='1' style='width: 0px;'>Estatus</th>";
            html += "<th class='sorting_disabled' rowspan='1' colspan='1' style='width: 0px;'>Firma</th>";
            html += "</tr>";
            html += "</thead>";
            html += "<tbody>";

            foreach (var item in lstAutorizadores)
            {
                string opcion = item.header == "PENDIENTE" ? "NO HA VOTADO" : item.voto;
                html += "<tr>";
                html += "<td>" + item.autorizanteNombre + "</td>";
                html += "<td>" + item.autorizantePuesto + "</td>";
                html += "<td>" + item.autorizanteFecha + "</td>";
                html += "<td>" + opcion + "</td>";
                html += "<td>" + item.header + "</td>";
                html += "<td>" + item.firma + "</td>";

                html += "</tr>";
            }

            html += "</tbody>";
            html += "</table>";
            html += "</div>";

            return html;
        }
        public List<tblM_ComparativoAdquisicionyRentaAutorizante> CargarAutorizadores(int idAsignacion)
        {
            List<tblM_ComparativoAdquisicionyRentaAutorizante> lstCom = new List<tblM_ComparativoAdquisicionyRentaAutorizante>();
            try
            {
                lstCom = _context.tblM_ComparativoAdquisicionyRentaAutorizante.Where(r => r.idAsignacion == idAsignacion).ToList();
                return lstCom;
            }
            catch (Exception)
            {
                return lstCom;
            }
        }
        public List<tblM_ComparativoFinancieroAutorizante> CargarAutorizadoresFinanciero(int idAsignacion)
        {
            List<tblM_ComparativoFinancieroAutorizante> lstCom = new List<tblM_ComparativoFinancieroAutorizante>();
            try
            {
                lstCom = _context.tblM_ComparativoFinancieroAutorizante.Where(r => r.idAsignacion == idAsignacion).ToList();
                return lstCom;
            }
            catch (Exception)
            {
                return lstCom;
            }
        }
        public ComparativoDTO editEstatusComparativo(int Dif, int idComparativo, ComparativoDTO objFiltro)
        {
            ComparativoDTO objComparativo = new ComparativoDTO();
            try
            {
                if (Dif == 0)
                {
                    if (idComparativo > 0)
                    {
                        var obj = _context.tblM_ComparativoAdquisicionyRenta.Where(r => r.idAsignacion == idComparativo).FirstOrDefault();
                        if (obj != null)
                        {
                            var lst = _context.tblM_ComparativoAdquisicionyRentaDet.Where(r => r.idComparativo == obj.id).ToList();
                            if (lst.Count() != 0)
                            {
                                obj.estatus = 2;
                                obj.nombreDelEquipo = objFiltro.nombreDelEquipo;
                                obj.obra = objFiltro.obra;
                                obj.compra = objFiltro.compra;
                                obj.renta = objFiltro.renta;
                                obj.roc = objFiltro.roc;
                                _context.SaveChanges();

                                objComparativo.estatusExito = 2;
                                objComparativo.msjExito = "Se ha cerrado correctamente el cuadro comparativo.";
                            }
                            else
                            {
                                objComparativo.estatusExito = 1;
                                objComparativo.msjExito = "No puede cerrar este cuadro comparativo ya que no tiene registros";
                            }
                        }
                    }
                }
                else
                {
                    var obj = _context.tblM_ComparativoAdquisicionyRenta.Where(r => r.idAsignacion == idComparativo).FirstOrDefault();
                    if (obj != null)
                    {
                        var lst = _context.tblM_ComparativoFinancieroDet.Where(r => r.idFinanciero == obj.idAsignacion).ToList();
                        if (lst.Count() != 0)
                        {
                            obj.estatusFinanciera = 2;
                            _context.SaveChanges();
                            objComparativo.estatusExito = 2;
                            objComparativo.msjExito = "Se ha cerrado correctamente el cuadro comparativo.";
                        }
                        else
                        {
                            objComparativo.estatusExito = 1;
                            objComparativo.msjExito = "No puede cerrar este cuadro comparativo ya que no tiene registros";
                        }
                    }
                }

                return objComparativo;
            }
            catch (Exception)
            {
                return objComparativo;
            }
        }
        public ComparativoDTO AutorizandoComparativo(int idComparativoDetalle, int idAsignacion, int idCuadro, int idUsuario)
        {
            ComparativoDTO objComparativo = new ComparativoDTO();
            try
            {
                if (idCuadro == 0)
                {
                    #region Autorización de cuadro normal
                    tblP_Alerta objAlerta;
                    var obj = _context.tblM_ComparativoAdquisicionyRentaAutorizante.Where(r => r.autorizanteID == idUsuario && r.idAsignacion == idAsignacion).FirstOrDefault();
                    if (obj != null)
                    {

                        //int ordenar = obj.orden - 1;
                        //int ordenarAlerta = obj.orden + 1;
                        //var objCompa = _context.tblM_ComparativoAdquisicionyRentaAutorizante.Where(r => r.idAsignacion == idAsignacion && r.orden == ordenar).FirstOrDefault();
                        //var objCompaAlerta = _context.tblM_ComparativoAdquisicionyRentaAutorizante.Where(r => r.idAsignacion == idAsignacion && r.orden == ordenarAlerta).FirstOrDefault();


                        //if (objCompa != null)
                        //{
                        //    if (objCompa.autorizanteStatus)
                        //    {
                        //        if (objCompa.autorizanteFinal == true)
                        //        {


                        //}

                        var objComparativom = _context.tblM_ComparativoAdquisicionyRentaAutorizante.Where(r => r.id == obj.id).FirstOrDefault();
                        objComparativom.idComparativoDetalle = idComparativoDetalle;
                        objComparativom.autorizanteFecha = DateTime.Now;
                        objComparativom.firma = GlobalUtils.CrearFirmaDigital(idAsignacion, DocumentosEnum.CuadroComparativoAdquisicionYRenta, idUsuario);
                        objComparativom.autorizanteStatus = true;
                        _context.SaveChanges();



                        objComparativo.estatusExito = 2;
                        objComparativo.msjExito = "Operacion realizada con exito.";
                        if (idUsuario == 1164)
                        {
                            var objalertaEliminar = _context.tblP_Alerta.Where(r => r.moduloID == 1033 && r.documentoID == idAsignacion).ToList();
                            if (objalertaEliminar != null || objalertaEliminar.Count() != 0)
                            {
                                _context.tblP_Alerta.RemoveRange(objalertaEliminar);
                                _context.SaveChanges();

                            }
                        }
                        else
                        {
                            var objalertaEliminar = _context.tblP_Alerta.Where(r => r.userRecibeID == idUsuario && r.moduloID == 1033 && r.documentoID == idAsignacion && r.objID == obj.id).FirstOrDefault();
                            if (objalertaEliminar != null)
                            {
                                _context.tblP_Alerta.Remove(objalertaEliminar);
                                _context.SaveChanges();

                            }
                        }



                        if (obj.autorizanteFinal == true)
                        {
                            var objup = _context.tblM_ComparativoAdquisicionyRenta.Where(r => r.idAsignacion == idAsignacion).FirstOrDefault();
                            objup.estatus = 3;
                            _context.SaveChanges();
                        }

                        //            if (objCompaAlerta != null)
                        //            {
                        var objFolio = _context.tblM_AsignacionEquipos.Where(r => r.id == idAsignacion).FirstOrDefault();
                        //var objAutorizantes = _context.tblM_ComparativoAdquisicionyRentaAutorizante.Where(r => r.idAsignacion == idAsignacion && r.firma == "").ToList();
                        //foreach (var item in objAutorizantes)
                        //{
                        //    objAlerta = new tblP_Alerta();
                        //    objAlerta.userEnviaID = idUsuario;
                        //    objAlerta.userRecibeID = item.autorizanteID;
                        //    objAlerta.tipoAlerta = 2;
                        //    objAlerta.sistemaID = 1;
                        //    objAlerta.url = "/CatMaquina/AutorizarAsignacionNoEconomico";
                        //    objAlerta.objID = objComparativom.id;
                        //    objAlerta.obj = null;
                        //    objAlerta.msj = "Cuadro comparativo Autorizacion " + objFolio.folio;
                        //    objAlerta.documentoID = objComparativom.idComparativoDetalle;
                        //    objAlerta.moduloID = Convert.ToInt32(BitacoraEnum.AutorizacionFinanciero);
                        //    _context.tblP_Alerta.Add(objAlerta);
                        //    _context.SaveChanges();
                        //}

                        //            }
                        //        }
                        //        else
                        //        {
                        //            objComparativo.estatusExito = 1;
                        //            objComparativo.msjExito = "la autizacion escalonada no te permite realizar una autorizacion.";
                        //            return objComparativo;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        var objComparativom = _context.tblM_ComparativoAdquisicionyRentaAutorizante.Where(r => r.id == obj.id).FirstOrDefault();
                        //        objComparativom.idComparativoDetalle = idComparativoDetalle;
                        //        objComparativom.autorizanteFecha = DateTime.Now;
                        //        objComparativom.firma = GlobalUtils.CrearFirmaDigital(idAsignacion, DocumentosEnum.CuadroComparativoAdquisicionYRenta, idUsuario);
                        //        objComparativom.autorizanteStatus = true;
                        //        _context.SaveChanges();
                        //        objComparativo.estatusExito = 2;
                        //        objComparativo.msjExito = "Operacion realizada con exito.";

                        //        var objalertaEliminar = _context.tblP_Alerta.Where(r => r.userRecibeID == idUsuario && r.moduloID == 1034 && r.documentoID == idAsignacion && r.objID == obj.id).FirstOrDefault();
                        //        _context.tblP_Alerta.Remove(objalertaEliminar);
                        //        _context.SaveChanges();


                        //        if (objCompaAlerta != null)
                        //        {
                        //            objAlerta = new tblP_Alerta();
                        //            objAlerta.userEnviaID = idUsuario;
                        //            objAlerta.userRecibeID = objCompaAlerta.autorizanteID;
                        //            objAlerta.tipoAlerta = 2;
                        //            objAlerta.sistemaID = 1;
                        //            objAlerta.url = "/CatMaquina/AutorizarAsignacionNoEconomico";
                        //            objAlerta.objID = objComparativom.id;
                        //            objAlerta.obj = null;
                        //            objAlerta.msj = "Cuadro comparativo " + idAsignacion;
                        //            objAlerta.documentoID = objComparativom.idComparativoDetalle;
                        //            objAlerta.moduloID = Convert.ToInt32(BitacoraEnum.AutorizacionFinanciero);
                        //            _context.tblP_Alerta.Add(objAlerta);
                        //            _context.SaveChanges();
                        //        }
                        //    }
                        //}
                    }
                    else
                    {
                        objComparativo.estatusExito = 1;
                        objComparativo.msjExito = "No tienes asignado este cuadro comparativo.";
                        return objComparativo;
                    }

                    var lstAutorizadores = _context.tblM_ComparativoAdquisicionyRentaAutorizante.Where(r => r.idAsignacion == idAsignacion).ToList().Select(y => new ComparativoDTO
                    {

                        autorizanteID = y.autorizanteID,
                        autorizanteNombre = y.autorizanteNombre,
                        autorizantePuesto = y.autorizantePuesto,
                        autorizanteFecha = y.autorizanteFecha,
                        header = y.firma != "" ? "AUTORIZADO" : "PENDIENTE",
                        firma = y.firma,
                        voto = "Opcion " + _context.tblM_ComparativoAdquisicionyRentaDet.Where(s => s.id == y.idComparativoDetalle).ToList().Select(n => n.idRow).FirstOrDefault().ToString()


                    }).ToList();
                    foreach (var item in lstAutorizadores)
                    {
                        List<tblP_Usuario> lstUsuarios = new List<tblP_Usuario>();
                        lstUsuarios = _context.tblP_Usuario.Where(r => r.id == item.autorizanteID).ToList();
                        var subject = "Cuadro Comparativo Adquisicion y Renta de Maquinaria y Equipo ";
                        var body = @"Buen dia " + lstUsuarios.Select(y => y.nombre).FirstOrDefault() + " " + lstUsuarios.Select(y => y.apellidoPaterno).FirstOrDefault() + " " + lstUsuarios.Select(y => y.apellidoMaterno).FirstOrDefault()
                            + " usted tiene una autorizacion de cuadro comparativo de adquisicion y renta pendiente <br>"
                            + htmlCorreo(lstAutorizadores);

                        GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, lstUsuarios.Select(y => y.correo).ToList());
                    }
                    #endregion
                }
                else
                {
                    #region Autorización de cuadro independiente
                    var registroAutorizante = _context.tblM_ComparativoAdquisicionyRentaAutorizante.FirstOrDefault(x => x.idCuadro == idCuadro && x.autorizanteID == idUsuario && x.idComparativoDetalle == 0);

                    if (registroAutorizante == null)
                    {
                        objComparativo.estatusExito = 1;
                        objComparativo.msjExito = "No se encuentra la información del autorizante.";

                        return objComparativo;
                    }

                    registroAutorizante.idComparativoDetalle = idComparativoDetalle;
                    registroAutorizante.autorizanteFecha = DateTime.Now;
                    registroAutorizante.firma = GlobalUtils.CrearFirmaDigital(idCuadro, DocumentosEnum.CuadroComparativoAdquisicionYRenta, idUsuario);
                    registroAutorizante.autorizanteStatus = true;
                    _context.SaveChanges();

                    objComparativo.estatusExito = 2;
                    objComparativo.msjExito = "Operacion realizada con exito.";

                    var registroAlerta = _context.tblP_Alerta.FirstOrDefault(x => x.userRecibeID == idUsuario && x.moduloID == 1033 && x.objID == registroAutorizante.id);

                    if (registroAlerta != null)
                    {
                        _context.tblP_Alerta.Remove(registroAlerta);
                        _context.SaveChanges();
                    }

                    var listaAutorizantes = _context.tblM_ComparativoAdquisicionyRentaAutorizante.Where(x => x.idCuadro == idCuadro).ToList();
                    var ultimoAutorizante = listaAutorizantes.OrderByDescending(x => x.orden).FirstOrDefault();

                    //if (registroAutorizante.autorizanteFinal || registroAutorizante.orden == ultimoAutorizante)
                    //if (listaAutorizantes.All(x => x.autorizanteStatus))
                    //if (idUsuario == ultimoAutorizante.autorizanteID)
                    if (idUsuario == 1164) //Usuario de Gerardo Reina está autorizando, por lo tanto el cuadro se toma como completamente autorizado.
                    {
                        var registroCuadro = _context.tblM_ComparativoAdquisicionyRenta.FirstOrDefault(x => x.id == idCuadro);
                        registroCuadro.estatus = 3;
                        _context.SaveChanges();
                    }

                    var lstAutorizadores = listaAutorizantes.Select(y => new ComparativoDTO
                    {
                        autorizanteID = y.autorizanteID,
                        autorizanteNombre = y.autorizanteNombre,
                        autorizantePuesto = y.autorizantePuesto,
                        autorizanteFecha = y.autorizanteFecha,
                        header = y.firma != "" ? "AUTORIZADO" : "PENDIENTE",
                        firma = y.firma,
                        voto = "Opcion " + _context.tblM_ComparativoAdquisicionyRentaDet.Where(s => s.id == y.idComparativoDetalle).ToList().Select(n => n.idRow).FirstOrDefault().ToString()
                    }).ToList();

                    foreach (var item in lstAutorizadores)
                    {
                        List<tblP_Usuario> lstUsuarios = new List<tblP_Usuario>();
                        lstUsuarios = _context.tblP_Usuario.Where(r => r.id == item.autorizanteID).ToList();
                        var subject = "Cuadro Comparativo Adquisicion y Renta de Maquinaria y Equipo ";
                        var body = @"Buen dia " + lstUsuarios.Select(y => y.nombre).FirstOrDefault() + " " + lstUsuarios.Select(y => y.apellidoPaterno).FirstOrDefault() + " " + lstUsuarios.Select(y => y.apellidoMaterno).FirstOrDefault()
                            + " usted tiene una autorizacion de cuadro comparativo de adquisicion y renta pendiente <br>"
                            + htmlCorreo(lstAutorizadores);
                        var correos = lstUsuarios.Select(y => y.correo).ToList();

#if DEBUG
                        correos = new List<string> { "oscar.valencia@construplan.com.mx" };
#endif

                        GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, correos);
                    }
                    #endregion
                }

                return objComparativo;
            }
            catch (Exception)
            {
                return objComparativo;
            }
        }

        public Tuple<DateTime?, string> GetUltimaAutorizacionCuadro(int idCuadro)
        {
            Tuple<DateTime?, string> resultado = new Tuple<DateTime?, string>(null, "");

            var listaCuadroDetalle_id = _context.tblM_ComparativoAdquisicionyRentaDet.Where(x => x.idComparativo == idCuadro).Select(x => x.id).ToList();
            var listaAutorizantes = _context.tblM_ComparativoAdquisicionyRentaAutorizante.Where(x => listaCuadroDetalle_id.Contains(x.idComparativoDetalle) && x.autorizanteStatus && x.autorizanteFecha != null).ToList();

            if (listaAutorizantes.Count() > 0)
            {
                var ultimaAutorizacion = listaAutorizantes.OrderByDescending(x => x.autorizanteFecha).FirstOrDefault();

                resultado = new Tuple<DateTime?, string>(ultimaAutorizacion.autorizanteFecha, ((DateTime)ultimaAutorizacion.autorizanteFecha).ToShortDateString());
            }

            return resultado;
        }
        public ComparativoDTO indicadorColumnaMaximoVoto(int idAsignacion, int Tipo)
        {
            ComparativoDTO objComparativo = new ComparativoDTO();
            try
            {
                if (Tipo == 0)
                {
                    var lstAutorizantes = _context.tblM_ComparativoAdquisicionyRentaAutorizante.Where(r => r.idAsignacion == idAsignacion && r.autorizanteStatus).ToList().OrderByDescending(s => s.autorizanteFecha).FirstOrDefault();

                    objComparativo.votoMayor = 1;
                    objComparativo.id = lstAutorizantes.idComparativoDetalle;
                }
                else
                {
                    var lstAutorizantes = _context.tblM_ComparativoFinancieroAutorizante.Where(r => r.idAsignacion == idAsignacion && r.autorizanteStatus).ToList().OrderByDescending(s => s.autorizanteFecha).FirstOrDefault();

                    objComparativo.votoMayor = 1;
                    objComparativo.id = lstAutorizantes.idFinanciero;
                }


                return objComparativo;
            }
            catch (Exception ex)
            {
                return objComparativo;
            }
        }
        public List<ComparativoDTO> getTablaComparativoFinanciero()
        {
            try
            {
                resultado = new Dictionary<string, object>();
                ComparativoDTO obj = new ComparativoDTO();
                List<ComparativoDTO> lstComparativo = new List<ComparativoDTO>();
                #region HEADER


                obj = new ComparativoDTO();
                obj.header = "botones";
                obj.txtIdnumero1 = "btnAgregarInput";
                obj.txtIdnumero2 = "btnAgregarInput2";
                obj.txtIdnumero3 = "btnAgregarInput3";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Banco";
                obj.txtIdnumero1 = "banco1";
                obj.txtIdnumero2 = "banco2";
                obj.txtIdnumero3 = "banco3";
                obj.classColor = "#f4b084";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Plazo";
                obj.txtIdnumero1 = "plazo1";
                obj.txtIdnumero2 = "plazo2";
                obj.txtIdnumero3 = "plazo3";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Precio del equipo";
                obj.txtIdnumero1 = "precio1";
                obj.txtIdnumero2 = "precio2";
                obj.txtIdnumero3 = "precio3";
                obj.classColor = "#f4b084";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Tiempo restante del proyecto";
                obj.txtIdnumero1 = "tiempoRestanteProyecto1";
                obj.txtIdnumero2 = "tiempoRestanteProyecto2";
                obj.txtIdnumero3 = "tiempoRestanteProyecto3";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "IVA";
                obj.txtIdnumero1 = "iva1";
                obj.txtIdnumero2 = "iva2";
                obj.txtIdnumero3 = "iva3";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Total";
                obj.txtIdnumero1 = "total1";
                obj.txtIdnumero2 = "total2";
                obj.txtIdnumero3 = "total3";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Monto a financiar";
                obj.txtIdnumero1 = "montoFinanciar1";
                obj.txtIdnumero2 = "montoFinanciar2";
                obj.txtIdnumero3 = "montoFinanciar3";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Tipo de operación";
                obj.txtIdnumero1 = "tipoOperacion1";
                obj.txtIdnumero2 = "tipoOperacion2";
                obj.txtIdnumero3 = "tipoOperacion3";
                obj.classColor = "#d0cece";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Opcion Compra";
                obj.txtIdnumero1 = "opcionCompra1";
                obj.txtIdnumero2 = "opcionCompra2";
                obj.txtIdnumero3 = "opcionCompra3";
                obj.classColor = "#d0cece";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Valor Residual";
                obj.txtIdnumero1 = "valorResidual1";
                obj.txtIdnumero2 = "valorResidual2";
                obj.txtIdnumero3 = "valorResidual3";
                obj.classColor = "#d0cece";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Deposito en efectivo";
                obj.txtIdnumero1 = "depositoEfectivo1";
                obj.txtIdnumero2 = "depositoEfectivo2";
                obj.txtIdnumero3 = "depositoEfectivo3";
                obj.classColor = "#d0cece";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Moneda";
                obj.txtIdnumero1 = "moneda1";
                obj.txtIdnumero2 = "moneda2";
                obj.txtIdnumero3 = "moneda3";
                obj.classColor = "#d0cece";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Plazo (meses)";
                obj.txtIdnumero1 = "plazoMeses1";
                obj.txtIdnumero2 = "plazoMeses2";
                obj.txtIdnumero3 = "plazoMeses3";
                obj.classColor = "#d0cece";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Tasa de interés";
                obj.txtIdnumero1 = "tasaDeInteres1";
                obj.txtIdnumero2 = "tasaDeInteres2";
                obj.txtIdnumero3 = "tasaDeInteres3";
                obj.classColor = "#d0cece";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Gastos Fijos";
                obj.txtIdnumero1 = "gastosFijos1";
                obj.txtIdnumero2 = "gastosFijos2";
                obj.txtIdnumero3 = "gastosFijos3";
                obj.classColor = "#d0cece";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Comision (%)";
                obj.txtIdnumero1 = "comision1";
                obj.txtIdnumero2 = "comision2";
                obj.txtIdnumero3 = "comision3";
                obj.classColor = "#d0cece";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Monto Comision (%)";
                obj.txtIdnumero1 = "montoComision1";
                obj.txtIdnumero2 = "montoComision2";
                obj.txtIdnumero3 = "montoComision3";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Rentas en Garantía";
                obj.txtIdnumero1 = "rentasEnGarantia1";
                obj.txtIdnumero2 = "rentasEnGarantia2";
                obj.txtIdnumero3 = "rentasEnGarantia3";
                obj.classColor = "#d0cece";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Crecimiento Pagos";
                obj.txtIdnumero1 = "crecimientoPagos1";
                obj.txtIdnumero2 = "crecimientoPagos2";
                obj.txtIdnumero3 = "crecimientoPagos3";
                obj.classColor = "#d0cece";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Pago Inicial";
                obj.txtIdnumero1 = "pagoInicial1";
                obj.txtIdnumero2 = "pagoInicial2";
                obj.txtIdnumero3 = "pagoInicial3";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Pago Total de intereses";
                obj.txtIdnumero1 = "pagoTotalIntereses1";
                obj.txtIdnumero2 = "pagoTotalIntereses2";
                obj.txtIdnumero3 = "pagoTotalIntereses3";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Tasa Efectiva";
                obj.txtIdnumero1 = "tasaEfectiva1";
                obj.txtIdnumero2 = "tasaEfectiva2";
                obj.txtIdnumero3 = "tasaEfectiva3";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Mensualidad";
                obj.txtIdnumero1 = "mensualidad1";
                obj.txtIdnumero2 = "mensualidad2";
                obj.txtIdnumero3 = "mensualidad3";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Mensualidad sin IVA";
                obj.txtIdnumero1 = "mensualidadSinIVA1";
                obj.txtIdnumero2 = "mensualidadSinIVA2";
                obj.txtIdnumero3 = "mensualidadSinIVA3";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Pago total";
                obj.txtIdnumero1 = "pagoTotal1";
                obj.txtIdnumero2 = "pagoTotal2";
                obj.txtIdnumero3 = "pagoTotal3";
                lstComparativo.Add(obj);

                #endregion

                return lstComparativo;
            }
            catch (Exception e)
            {

                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error interno al intentar obtener el catálogo tipos de emleados");
                return null;
            }
        }
        public ComparativoDTO AutorizandoComparativoFinanciera(int idRow, int idAsignacion, int idUsuario)
        {
            ComparativoDTO objComparativo = new ComparativoDTO();
            try
            {
                tblP_Alerta objAlerta;
                var obj = _context.tblM_ComparativoFinancieroAutorizante.Where(r => r.autorizanteID == idUsuario && r.idAsignacion == idAsignacion).FirstOrDefault();
                if (obj != null)
                {
                    int ordenar = obj.orden - 1;
                    int ordenarAlerta = obj.orden + 1;
                    var objCompa = _context.tblM_ComparativoFinancieroAutorizante.Where(r => r.idAsignacion == idAsignacion && r.orden == ordenar).FirstOrDefault();
                    var objCompaAlerta = _context.tblM_ComparativoFinancieroAutorizante.Where(r => r.idAsignacion == idAsignacion && r.orden == ordenarAlerta).FirstOrDefault();
                    if (objCompa != null)
                    {
                        if (objCompa.autorizanteStatus)
                        {
                            var objComparativom = _context.tblM_ComparativoFinancieroAutorizante.Where(r => r.id == obj.id).FirstOrDefault();

                            objComparativom.idFinanciero = idRow;
                            objComparativom.autorizanteFecha = DateTime.Now;
                            objComparativom.firma = GlobalUtils.CrearFirmaDigital(idAsignacion, DocumentosEnum.AutorizacionFinanciero, idUsuario);
                            objComparativom.autorizanteStatus = true;
                            _context.SaveChanges();
                            objComparativo.estatusExito = 2;
                            objComparativo.msjExito = "Operacion realizada con exito.";

                            var objalertaEliminar = _context.tblP_Alerta.Where(r => r.userRecibeID == idUsuario && r.moduloID == 1033 && r.documentoID == idAsignacion && r.objID == obj.id).FirstOrDefault();
                            _context.tblP_Alerta.Remove(objalertaEliminar);
                            _context.SaveChanges();

                            var objFolio = _context.tblM_AsignacionEquipos.Where(r => r.id == idAsignacion).FirstOrDefault();

                            if (objCompaAlerta != null)
                            {
                                objAlerta = new tblP_Alerta();
                                objAlerta.userEnviaID = idUsuario;
                                objAlerta.userRecibeID = objCompaAlerta.autorizanteID;
                                objAlerta.tipoAlerta = 2;
                                objAlerta.sistemaID = 1;
                                objAlerta.url = "/CatMaquina/AutorizarAsignacionNoEconomico";
                                objAlerta.objID = objComparativom.id;
                                objAlerta.obj = null;
                                objAlerta.msj = "Cuadro comparativo " + objFolio.folio;
                                objAlerta.documentoID = objComparativom.idFinanciero;
                                objAlerta.moduloID = Convert.ToInt32(BitacoraEnum.AutorizacionFinanciero);
                                _context.tblP_Alerta.Add(objAlerta);
                                _context.SaveChanges();
                            }
                        }
                        else
                        {
                            objComparativo.estatusExito = 1;
                            objComparativo.msjExito = "la autorizacion escalonada no te permite realizar una autorizacion.";
                            return objComparativo;
                        }


                    }
                    else
                    {
                        var objComparativom = _context.tblM_ComparativoFinancieroAutorizante.Where(r => r.id == obj.id).FirstOrDefault();

                        objComparativom.idFinanciero = idRow;
                        objComparativom.autorizanteFecha = DateTime.Now;
                        objComparativom.firma = GlobalUtils.CrearFirmaDigital(idAsignacion, DocumentosEnum.AutorizacionFinanciero, idUsuario);
                        objComparativom.autorizanteStatus = true;
                        _context.SaveChanges();
                        objComparativo.estatusExito = 2;
                        objComparativo.msjExito = "Operacion realizada con exito.";

                        var objalertaEliminar = _context.tblP_Alerta.Where(r => r.userRecibeID == idUsuario && r.moduloID == 1033 && r.documentoID == idAsignacion && r.objID == obj.id).FirstOrDefault();
                        _context.tblP_Alerta.Remove(objalertaEliminar);
                        _context.SaveChanges();

                        var objFolio = _context.tblM_AsignacionEquipos.Where(r => r.id == idAsignacion).FirstOrDefault();

                        if (objCompaAlerta != null)
                        {
                            objAlerta = new tblP_Alerta();
                            objAlerta.userEnviaID = idUsuario;
                            objAlerta.userRecibeID = objCompaAlerta.autorizanteID;
                            objAlerta.tipoAlerta = 2;
                            objAlerta.sistemaID = 1;
                            objAlerta.url = "/CatMaquina/AutorizarAsignacionNoEconomico";
                            objAlerta.objID = objComparativom.id;
                            objAlerta.obj = null;
                            objAlerta.msj = "Cuadro comparativo " + objFolio.folio;
                            objAlerta.documentoID = objComparativom.idFinanciero;
                            objAlerta.moduloID = Convert.ToInt32(BitacoraEnum.AutorizacionFinanciero);
                            _context.tblP_Alerta.Add(objAlerta);
                            _context.SaveChanges();
                        }

                    }



                }
                else
                {
                    objComparativo.estatusExito = 1;
                    objComparativo.msjExito = "No tienes asignado este cuadro comparativo.";
                }

                var lstAutorizadores = _context.tblM_ComparativoFinancieroAutorizante.Where(r => r.idAsignacion == idAsignacion).ToList().Select(y => new ComparativoDTO
                {

                    autorizanteID = y.autorizanteID,
                    autorizanteNombre = y.autorizanteNombre,
                    autorizantePuesto = y.autorizantePuesto,
                    autorizanteFecha = y.autorizanteFecha,
                    header = y.firma != "" ? "AUTORIZADO" : "PENDIENTE",
                    firma = y.firma

                }).ToList();
                foreach (var item in lstAutorizadores)
                {
                    List<tblP_Usuario> lstUsuarios = new List<tblP_Usuario>();
                    lstUsuarios = _context.tblP_Usuario.Where(r => r.id == item.autorizanteID).ToList();
                    var subject = "Cuadro Comparativo Adquisicion y Renta de Maquinaria y Equipo ";
                    var body = @"Buen dia " + lstUsuarios.Select(y => y.nombre).FirstOrDefault() + " " + lstUsuarios.Select(y => y.apellidoPaterno).FirstOrDefault() + " " + lstUsuarios.Select(y => y.apellidoMaterno).FirstOrDefault()
                        + " usted tiene una autorizacion de cuadro comparativo de adquisicion y renta pendiente <br>"
                        + htmlCorreo(lstAutorizadores);

                    GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, lstUsuarios.Select(y => y.correo).ToList());
                }

                return objComparativo;
            }
            catch (Exception)
            {
                return objComparativo;
            }
        }
        public List<ComparativoDTO> getTablaComparativoFinancieroAutorizar()
        {
            try
            {
                resultado = new Dictionary<string, object>();
                ComparativoDTO obj = new ComparativoDTO();
                List<ComparativoDTO> lstComparativo = new List<ComparativoDTO>();
                #region HEADER
                obj = new ComparativoDTO();
                obj.header = "";
                obj.txtIdnumero1 = "btnAgregarInput1";
                obj.txtIdnumero2 = "btnAgregarInput2";
                obj.txtIdnumero3 = "btnAgregarInput3";
                obj.txtIdnumero4 = "btnAgregarInput4";
                obj.txtIdnumero5 = "btnAgregarInput5";
                obj.txtIdnumero6 = "btnAgregarInput6";
                obj.txtIdnumero7 = "btnAgregarInput7";
                obj.txtIdnumero8 = "btnAgregarInput8";
                obj.txtIdnumero9 = "btnAgregarInput9";
                obj.txtIdnumero10 = "btnAgregarInput10";
                obj.txtIdnumero11 = "btnAgregarInput11";
                obj.txtIdnumero12 = "btnAgregarInput12";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Selector";
                obj.txtIdnumero1 = "Color1";
                obj.txtIdnumero2 = "Color2";
                obj.txtIdnumero3 = "Color3";
                obj.txtIdnumero4 = "Color4";
                obj.txtIdnumero5 = "Color5";
                obj.txtIdnumero6 = "Color6";
                obj.txtIdnumero7 = "Color7";
                obj.txtIdnumero8 = "Color8";
                obj.txtIdnumero9 = "Color9";
                obj.txtIdnumero10 = "Color10";
                obj.txtIdnumero11 = "Color11";
                obj.txtIdnumero12 = "Color12";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Banco";
                obj.txtIdnumero1 = "banco1";
                obj.txtIdnumero2 = "banco2";
                obj.txtIdnumero3 = "banco3";
                obj.txtIdnumero4 = "banco4";
                obj.txtIdnumero5 = "banco5";
                obj.txtIdnumero6 = "banco6";
                obj.txtIdnumero7 = "banco7";
                obj.txtIdnumero8 = "banco8";
                obj.txtIdnumero9 = "banco9";
                obj.txtIdnumero10 = "banco10";
                obj.txtIdnumero11 = "banco11";
                obj.txtIdnumero12 = "banco12";
                obj.classColor = "#f4b084";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Plazos de financiamiento";
                obj.txtIdnumero1 = "plazo1";
                obj.txtIdnumero2 = "plazo2";
                obj.txtIdnumero3 = "plazo3";
                obj.txtIdnumero4 = "plazo4";
                obj.txtIdnumero5 = "plazo5";
                obj.txtIdnumero6 = "plazo6";
                obj.txtIdnumero7 = "plazo7";
                obj.txtIdnumero8 = "plazo8";
                obj.txtIdnumero9 = "plazo9";
                obj.txtIdnumero10 = "plazo10";
                obj.txtIdnumero11 = "plazo11";
                obj.txtIdnumero12 = "plazo12";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Precio del equipo";
                obj.txtIdnumero1 = "precioDelEquipo1";
                obj.txtIdnumero2 = "precioDelEquipo2";
                obj.txtIdnumero3 = "precioDelEquipo3";
                obj.txtIdnumero4 = "precioDelEquipo4";
                obj.txtIdnumero5 = "precioDelEquipo5";
                obj.txtIdnumero6 = "precioDelEquipo6";
                obj.txtIdnumero7 = "precioDelEquipo7";
                obj.txtIdnumero8 = "precioDelEquipo8";
                obj.txtIdnumero9 = "precioDelEquipo9";
                obj.txtIdnumero10 = "precioDelEquipo10";
                obj.txtIdnumero11 = "precioDelEquipo11";
                obj.txtIdnumero12 = "precioDelEquipo12";
                obj.classColor = "#f4b084";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Tiempo restante";
                obj.txtIdnumero1 = "tiempoRestanteProyecto1";
                obj.txtIdnumero2 = "tiempoRestanteProyecto2";
                obj.txtIdnumero3 = "tiempoRestanteProyecto3";
                obj.txtIdnumero4 = "tiempoRestanteProyecto4";
                obj.txtIdnumero5 = "tiempoRestanteProyecto5";
                obj.txtIdnumero6 = "tiempoRestanteProyecto6";
                obj.txtIdnumero7 = "tiempoRestanteProyecto7";
                obj.txtIdnumero8 = "tiempoRestanteProyecto8";
                obj.txtIdnumero9 = "tiempoRestanteProyecto9";
                obj.txtIdnumero10 = "tiempoRestanteProyecto10";
                obj.txtIdnumero11 = "tiempoRestanteProyecto11";
                obj.txtIdnumero12 = "tiempoRestanteProyecto12";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "IVA";
                obj.txtIdnumero1 = "iva1";
                obj.txtIdnumero2 = "iva2";
                obj.txtIdnumero3 = "iva3";
                obj.txtIdnumero4 = "iva4";
                obj.txtIdnumero5 = "iva5";
                obj.txtIdnumero6 = "iva6";
                obj.txtIdnumero7 = "iva7";
                obj.txtIdnumero8 = "iva8";
                obj.txtIdnumero9 = "iva9";
                obj.txtIdnumero10 = "iva10";
                obj.txtIdnumero11 = "iva11";
                obj.txtIdnumero12 = "iva12";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Total";
                obj.txtIdnumero1 = "total1";
                obj.txtIdnumero2 = "total2";
                obj.txtIdnumero3 = "total3";
                obj.txtIdnumero4 = "total4";
                obj.txtIdnumero5 = "total5";
                obj.txtIdnumero6 = "total6";
                obj.txtIdnumero7 = "total7";
                obj.txtIdnumero8 = "total8";
                obj.txtIdnumero9 = "total9";
                obj.txtIdnumero10 = "total10";
                obj.txtIdnumero11 = "total11";
                obj.txtIdnumero12 = "total12";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Monto financiero";
                obj.txtIdnumero1 = "montoFinanciar1";
                obj.txtIdnumero2 = "montoFinanciar2";
                obj.txtIdnumero3 = "montoFinanciar3";
                obj.txtIdnumero4 = "montoFinanciar4";
                obj.txtIdnumero5 = "montoFinanciar5";
                obj.txtIdnumero6 = "montoFinanciar6";
                obj.txtIdnumero7 = "montoFinanciar7";
                obj.txtIdnumero8 = "montoFinanciar8";
                obj.txtIdnumero9 = "montoFinanciar9";
                obj.txtIdnumero10 = "montoFinanciar10";
                obj.txtIdnumero11 = "montoFinanciar11";
                obj.txtIdnumero12 = "montoFinanciar12";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Tipo operación";
                obj.txtIdnumero1 = "tipoOperacion1";
                obj.txtIdnumero2 = "tipoOperacion2";
                obj.txtIdnumero3 = "tipoOperacion3";
                obj.txtIdnumero4 = "tipoOperacion4";
                obj.txtIdnumero5 = "tipoOperacion5";
                obj.txtIdnumero6 = "tipoOperacion6";
                obj.txtIdnumero7 = "tipoOperacion7";
                obj.txtIdnumero8 = "tipoOperacion8";
                obj.txtIdnumero9 = "tipoOperacion9";
                obj.txtIdnumero10 = "tipoOperacion10";
                obj.txtIdnumero11 = "tipoOperacion11";
                obj.txtIdnumero12 = "tipoOperacion12";
                obj.classColor = "#d0cece";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Opción compra";
                obj.txtIdnumero1 = "opcionCompra1";
                obj.txtIdnumero2 = "opcionCompra2";
                obj.txtIdnumero3 = "opcionCompra3";
                obj.txtIdnumero4 = "opcionCompra4";
                obj.txtIdnumero5 = "opcionCompra5";
                obj.txtIdnumero6 = "opcionCompra6";
                obj.txtIdnumero7 = "opcionCompra7";
                obj.txtIdnumero8 = "opcionCompra8";
                obj.txtIdnumero9 = "opcionCompra9";
                obj.txtIdnumero10 = "opcionCompra10";
                obj.txtIdnumero11 = "opcionCompra11";
                obj.txtIdnumero12 = "opcionCompra12";
                obj.classColor = "#d0cece";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Valor residual";
                obj.txtIdnumero1 = "valorResidual1";
                obj.txtIdnumero2 = "valorResidual2";
                obj.txtIdnumero3 = "valorResidual3";
                obj.txtIdnumero4 = "valorResidual4";
                obj.txtIdnumero5 = "valorResidual5";
                obj.txtIdnumero6 = "valorResidual6";
                obj.txtIdnumero7 = "valorResidual7";
                obj.txtIdnumero8 = "valorResidual8";
                obj.txtIdnumero9 = "valorResidual9";
                obj.txtIdnumero10 = "valorResidual10";
                obj.txtIdnumero11 = "valorResidual11";
                obj.txtIdnumero12 = "valorResidual12";
                obj.classColor = "#d0cece";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Deposito en efectivo";
                obj.txtIdnumero1 = "depositoEfectivo1";
                obj.txtIdnumero2 = "depositoEfectivo2";
                obj.txtIdnumero3 = "depositoEfectivo3";
                obj.txtIdnumero4 = "depositoEfectivo4";
                obj.txtIdnumero5 = "depositoEfectivo5";
                obj.txtIdnumero6 = "depositoEfectivo6";
                obj.txtIdnumero7 = "depositoEfectivo7";
                obj.txtIdnumero8 = "depositoEfectivo8";
                obj.txtIdnumero9 = "depositoEfectivo9";
                obj.txtIdnumero10 = "depositoEfectivo10";
                obj.txtIdnumero11 = "depositoEfectivo11";
                obj.txtIdnumero12 = "depositoEfectivo12";
                obj.classColor = "#d0cece";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Moneda";
                obj.txtIdnumero1 = "moneda1";
                obj.txtIdnumero2 = "moneda2";
                obj.txtIdnumero3 = "moneda3";
                obj.txtIdnumero4 = "moneda4";
                obj.txtIdnumero5 = "moneda5";
                obj.txtIdnumero6 = "moneda6";
                obj.txtIdnumero7 = "moneda7";
                obj.txtIdnumero8 = "moneda8";
                obj.txtIdnumero9 = "moneda9";
                obj.txtIdnumero10 = "moneda10";
                obj.txtIdnumero11 = "moneda11";
                obj.txtIdnumero12 = "moneda12";
                obj.classColor = "#d0cece";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Plazo meses";
                obj.txtIdnumero1 = "plazoMeses1";
                obj.txtIdnumero2 = "plazoMeses2";
                obj.txtIdnumero3 = "plazoMeses3";
                obj.txtIdnumero4 = "plazoMeses4";
                obj.txtIdnumero5 = "plazoMeses5";
                obj.txtIdnumero6 = "plazoMeses6";
                obj.txtIdnumero7 = "plazoMeses7";
                obj.txtIdnumero8 = "plazoMeses8";
                obj.txtIdnumero9 = "plazoMeses9";
                obj.txtIdnumero10 = "plazoMeses10";
                obj.txtIdnumero11 = "plazoMeses11";
                obj.txtIdnumero12 = "plazoMeses12";
                obj.classColor = "#d0cece";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Tasa de interés";
                obj.txtIdnumero1 = "tasaDeInteres1";
                obj.txtIdnumero2 = "tasaDeInteres2";
                obj.txtIdnumero3 = "tasaDeInteres3";
                obj.txtIdnumero4 = "tasaDeInteres4";
                obj.txtIdnumero5 = "tasaDeInteres5";
                obj.txtIdnumero6 = "tasaDeInteres6";
                obj.txtIdnumero7 = "tasaDeInteres7";
                obj.txtIdnumero8 = "tasaDeInteres8";
                obj.txtIdnumero9 = "tasaDeInteres9";
                obj.txtIdnumero10 = "tasaDeInteres10";
                obj.txtIdnumero11 = "tasaDeInteres11";
                obj.txtIdnumero12 = "tasaDeInteres12";
                obj.classColor = "#d0cece";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Gastos fijos";
                obj.txtIdnumero1 = "gastosFijos1";
                obj.txtIdnumero2 = "gastosFijos2";
                obj.txtIdnumero3 = "gastosFijos3";
                obj.txtIdnumero4 = "gastosFijos4";
                obj.txtIdnumero5 = "gastosFijos5";
                obj.txtIdnumero6 = "gastosFijos6";
                obj.txtIdnumero7 = "gastosFijos7";
                obj.txtIdnumero8 = "gastosFijos8";
                obj.txtIdnumero9 = "gastosFijos9";
                obj.txtIdnumero10 = "gastosFijos10";
                obj.txtIdnumero11 = "gastosFijos11";
                obj.txtIdnumero12 = "gastosFijos12";
                obj.classColor = "#d0cece";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Comisión";
                obj.txtIdnumero1 = "comision1";
                obj.txtIdnumero2 = "comision2";
                obj.txtIdnumero3 = "comision3";
                obj.txtIdnumero4 = "comision4";
                obj.txtIdnumero5 = "comision5";
                obj.txtIdnumero6 = "comision6";
                obj.txtIdnumero7 = "comision7";
                obj.txtIdnumero8 = "comision8";
                obj.txtIdnumero9 = "comision9";
                obj.txtIdnumero10 = "comision10";
                obj.txtIdnumero11 = "comision11";
                obj.txtIdnumero12 = "comision12";
                obj.classColor = "#d0cece";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Monto comisión";
                obj.txtIdnumero1 = "montoComision1";
                obj.txtIdnumero2 = "montoComision2";
                obj.txtIdnumero3 = "montoComision3";
                obj.txtIdnumero4 = "montoComision4";
                obj.txtIdnumero5 = "montoComision5";
                obj.txtIdnumero6 = "montoComision6";
                obj.txtIdnumero7 = "montoComision7";
                obj.txtIdnumero8 = "montoComision8";
                obj.txtIdnumero9 = "montoComision9";
                obj.txtIdnumero10 = "montoComision10";
                obj.txtIdnumero11 = "montoComision11";
                obj.txtIdnumero12 = "montoComision12";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Rentas en garantía";
                obj.txtIdnumero1 = "rentasEnGarantia1";
                obj.txtIdnumero2 = "rentasEnGarantia2";
                obj.txtIdnumero3 = "rentasEnGarantia3";
                obj.txtIdnumero4 = "rentasEnGarantia4";
                obj.txtIdnumero5 = "rentasEnGarantia5";
                obj.txtIdnumero6 = "rentasEnGarantia6";
                obj.txtIdnumero7 = "rentasEnGarantia7";
                obj.txtIdnumero8 = "rentasEnGarantia8";
                obj.txtIdnumero9 = "rentasEnGarantia9";
                obj.txtIdnumero10 = "rentasEnGarantia10";
                obj.txtIdnumero11 = "rentasEnGarantia11";
                obj.txtIdnumero12 = "rentasEnGarantia12";
                obj.classColor = "#d0cece";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Crecimiento pagos";
                obj.txtIdnumero1 = "crecimientoPagos1";
                obj.txtIdnumero2 = "crecimientoPagos2";
                obj.txtIdnumero3 = "crecimientoPagos3";
                obj.txtIdnumero4 = "crecimientoPagos4";
                obj.txtIdnumero5 = "crecimientoPagos5";
                obj.txtIdnumero6 = "crecimientoPagos6";
                obj.txtIdnumero7 = "crecimientoPagos7";
                obj.txtIdnumero8 = "crecimientoPagos8";
                obj.txtIdnumero9 = "crecimientoPagos9";
                obj.txtIdnumero10 = "crecimientoPagos10";
                obj.txtIdnumero11 = "crecimientoPagos11";
                obj.txtIdnumero12 = "crecimientoPagos12";
                obj.classColor = "#d0cece";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Pago inicial";
                obj.txtIdnumero1 = "pagoInicial1";
                obj.txtIdnumero2 = "pagoInicial2";
                obj.txtIdnumero3 = "pagoInicial3";
                obj.txtIdnumero4 = "pagoInicial4";
                obj.txtIdnumero5 = "pagoInicial5";
                obj.txtIdnumero6 = "pagoInicial6";
                obj.txtIdnumero7 = "pagoInicial7";
                obj.txtIdnumero8 = "pagoInicial8";
                obj.txtIdnumero9 = "pagoInicial9";
                obj.txtIdnumero10 = "pagoInicial10";
                obj.txtIdnumero11 = "pagoInicial11";
                obj.txtIdnumero12 = "pagoInicial12";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Pago total intereses";
                obj.txtIdnumero1 = "pagoTotalIntereses1";
                obj.txtIdnumero2 = "pagoTotalIntereses2";
                obj.txtIdnumero3 = "pagoTotalIntereses3";
                obj.txtIdnumero4 = "pagoTotalIntereses4";
                obj.txtIdnumero5 = "pagoTotalIntereses5";
                obj.txtIdnumero6 = "pagoTotalIntereses6";
                obj.txtIdnumero7 = "pagoTotalIntereses7";
                obj.txtIdnumero8 = "pagoTotalIntereses8";
                obj.txtIdnumero9 = "pagoTotalIntereses9";
                obj.txtIdnumero10 = "pagoTotalIntereses10";
                obj.txtIdnumero11 = "pagoTotalIntereses11";
                obj.txtIdnumero12 = "pagoTotalIntereses12";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Tasa efectivo";
                obj.txtIdnumero1 = "tasaEfectiva1";
                obj.txtIdnumero2 = "tasaEfectiva2";
                obj.txtIdnumero3 = "tasaEfectiva3";
                obj.txtIdnumero4 = "tasaEfectiva4";
                obj.txtIdnumero5 = "tasaEfectiva5";
                obj.txtIdnumero6 = "tasaEfectiva6";
                obj.txtIdnumero7 = "tasaEfectiva7";
                obj.txtIdnumero8 = "tasaEfectiva8";
                obj.txtIdnumero9 = "tasaEfectiva9";
                obj.txtIdnumero10 = "tasaEfectiva10";
                obj.txtIdnumero11 = "tasaEfectiva11";
                obj.txtIdnumero12 = "tasaEfectiva12";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Mensualidad";
                obj.txtIdnumero1 = "mensualidad1";
                obj.txtIdnumero2 = "mensualidad2";
                obj.txtIdnumero3 = "mensualidad3";
                obj.txtIdnumero4 = "mensualidad4";
                obj.txtIdnumero5 = "mensualidad5";
                obj.txtIdnumero6 = "mensualidad6";
                obj.txtIdnumero7 = "mensualidad7";
                obj.txtIdnumero8 = "mensualidad8";
                obj.txtIdnumero9 = "mensualidad9";
                obj.txtIdnumero10 = "mensualidad10";
                obj.txtIdnumero11 = "mensualidad11";
                obj.txtIdnumero12 = "mensualidad12";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Mensualidad sin IVA";
                obj.txtIdnumero1 = "mensualidadSinIVA1";
                obj.txtIdnumero2 = "mensualidadSinIVA2";
                obj.txtIdnumero3 = "mensualidadSinIVA3";
                obj.txtIdnumero4 = "mensualidadSinIVA4";
                obj.txtIdnumero5 = "mensualidadSinIVA5";
                obj.txtIdnumero6 = "mensualidadSinIVA6";
                obj.txtIdnumero7 = "mensualidadSinIVA7";
                obj.txtIdnumero8 = "mensualidadSinIVA8";
                obj.txtIdnumero9 = "mensualidadSinIVA9";
                obj.txtIdnumero10 = "mensualidadSinIVA10";
                obj.txtIdnumero11 = "mensualidadSinIVA11";
                obj.txtIdnumero12 = "mensualidadSinIVA12";
                lstComparativo.Add(obj);
                obj = new ComparativoDTO();
                obj.header = "Pago total";
                obj.txtIdnumero1 = "pagoTotal1";
                obj.txtIdnumero2 = "pagoTotal2";
                obj.txtIdnumero3 = "pagoTotal3";
                obj.txtIdnumero4 = "pagoTotal4";
                obj.txtIdnumero5 = "pagoTotal5";
                obj.txtIdnumero6 = "pagoTotal6";
                obj.txtIdnumero7 = "pagoTotal7";
                obj.txtIdnumero8 = "pagoTotal8";
                obj.txtIdnumero9 = "pagoTotal9";
                obj.txtIdnumero10 = "pagoTotal10";
                obj.txtIdnumero11 = "pagoTotal11";
                obj.txtIdnumero12 = "pagoTotal12";
                lstComparativo.Add(obj);

                #endregion
                return lstComparativo;
            }
            catch (Exception e)
            {

                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error interno al intentar obtener el catálogo tipos de emleados");
                return null;
            }
        }
        public List<ComparativoDTO> getAutorizanteBotonPlus(int idAsignacion, int AutFin)
        {
            try
            {
                List<ComparativoDTO> lstComparativo = new List<ComparativoDTO>();
                #region HEADER
                if (AutFin == 0)
                {
                    var a = _context.tblM_ComparativoAdquisicionyRentaAutorizante.Where(r => r.autorizanteStatus).ToList();
                    if (a.Count() == 6)
                    {
                        lstComparativo = _context.tblM_ComparativoAdquisicionyRentaAutorizante.Where(r => r.autorizanteFinal == true && r.idAsignacion == idAsignacion && (r.autorizanteStatus)).ToList().Select(y => new ComparativoDTO
                        {
                            check = y.id != 0 ? true : false,
                            id = y.id,
                            idAsignacion = y.idAsignacion
                        }).ToList();
                    }
                }
                else
                {
                    var a = _context.tblM_ComparativoFinancieroAutorizante.Where(r => r.autorizanteStatus).ToList();
                    if (a.Count() == 6)
                    {
                        lstComparativo = _context.tblM_ComparativoFinancieroAutorizante.Where(r => r.autorizanteFinal == true && r.idAsignacion == idAsignacion && (r.autorizanteStatus)).ToList().Select(y => new ComparativoDTO
                        {
                            check = y.id != 0 ? true : false,
                            id = y.id,
                            idAsignacion = y.idAsignacion
                        }).ToList();
                    }
                }

                #endregion
                return lstComparativo;
            }
            catch (Exception e)
            {

                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error interno al intentar obtener el catálogo tipos de emleados");
                return null;
            }
        }
        public List<ComparativoDTO> getAutFin(int idAsignacion, int AutFin)
        {
            try
            {
                List<ComparativoDTO> lstComparativo = new List<ComparativoDTO>();
                #region HEADER
                if (AutFin == 0)
                {
                    lstComparativo = _context.tblM_ComparativoAdquisicionyRenta.Where(r => r.idAsignacion == idAsignacion).ToList().Select(y => new ComparativoDTO
                    {
                        check = y.estatus != 1 ? true : false,
                        id = y.id,
                        idAsignacion = y.idAsignacion
                    }).ToList();
                }
                else
                {
                    lstComparativo = _context.tblM_ComparativoAdquisicionyRenta.Where(r => r.idAsignacion == idAsignacion).ToList().Select(y => new ComparativoDTO
                    {
                        check = y.estatusFinanciera != 1 ? true : false,
                        id = y.id,
                        idAsignacion = y.idAsignacion
                    }).ToList();
                }

                #endregion
                return lstComparativo;
            }
            catch (Exception e)
            {

                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error interno al intentar obtener el catálogo tipos de emleados");
                return null;
            }
        }

        public List<ComparativoDTO> getAutorizanteAdquisicion(int idAsignacion)
        {
            try
            {
                List<ComparativoDTO> lstComparativo = new List<ComparativoDTO>();
                #region HEADER

                lstComparativo = _context.tblM_ComparativoAdquisicionyRentaAutorizante.Where(r => r.idAsignacion == idAsignacion).ToList().Select(x => new ComparativoDTO
                {
                    id = x.id,
                    idAsignacion = x.idAsignacion,
                    check = x.idComparativoDetalle != 0 ? true : false,
                    autorizanteID = x.autorizanteID,
                    autorizanteNombre = x.autorizanteNombre,
                    autorizantePuesto = x.autorizantePuesto,
                    autorizanteStatus = x.autorizanteStatus,
                    autorizanteFinal = x.autorizanteFinal,
                    autorizanteFecha = x.autorizanteFecha,
                    firma = x.firma,
                    tipo = x.tipo,
                    orden = x.orden,
                    comentario = x.comentario,
                    voto = "Opcion " + _context.tblM_ComparativoAdquisicionyRentaDet.Where(s => s.id == x.idComparativoDetalle).ToList().Select(y => y.idRow).FirstOrDefault()

                }).ToList();

                #endregion
                return lstComparativo;
            }
            catch (Exception e)
            {

                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error interno al intentar obtener el catálogo tipos de emleados");
                return null;
            }
        }
        public List<ComparativoDTO> getAutorizanteAdquisicionPorCuadro(int idCuadro)
        {
            try
            {
                return _context.tblM_ComparativoAdquisicionyRentaAutorizante.Where(x => x.idCuadro == idCuadro).ToList().Select(x => new ComparativoDTO
                {
                    id = x.id,
                    idAsignacion = x.idAsignacion,
                    check = x.idComparativoDetalle != 0 ? true : false,
                    autorizanteID = x.autorizanteID,
                    autorizanteNombre = x.autorizanteNombre,
                    autorizantePuesto = x.autorizantePuesto,
                    autorizanteStatus = x.autorizanteStatus,
                    autorizanteFinal = x.autorizanteFinal,
                    autorizanteFecha = x.autorizanteFecha,
                    firma = x.firma,
                    tipo = x.tipo,
                    orden = x.orden,
                    comentario = x.comentario,
                    voto = "Opcion " + _context.tblM_ComparativoAdquisicionyRentaDet.Where(s => s.id == x.idComparativoDetalle).ToList().Select(y => y.idRow).FirstOrDefault()

                }).ToList();
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error interno al intentar obtener el catálogo tipos de emleados");
                return null;
            }
        }
        public List<ComparativoDTO> getAutorizanteFinanciero(int idAsignacion)
        {
            try
            {
                List<ComparativoDTO> lstComparativo = new List<ComparativoDTO>();
                #region HEADER

                lstComparativo = _context.tblM_ComparativoFinancieroAutorizante.Where(r => r.idAsignacion == idAsignacion).ToList().Select(x => new ComparativoDTO
                {
                    id = x.id,
                    idAsignacion = x.idAsignacion,
                    check = x.idFinanciero != 0 ? true : false,
                    autorizanteID = x.autorizanteID,
                    autorizanteNombre = x.autorizanteNombre,
                    autorizantePuesto = x.autorizantePuesto,
                    autorizanteStatus = x.autorizanteStatus,
                    autorizanteFinal = x.autorizanteFinal,
                    autorizanteFecha = x.autorizanteFecha,
                    firma = x.firma,
                    tipo = x.tipo,
                    orden = x.orden,
                    comentario = x.comentario,
                    voto = "Opcion " + _context.tblM_ComparativoFinancieroDet.Where(s => s.id == x.idFinanciero).ToList().Select(y => y.idRow).FirstOrDefault()

                }).ToList();

                #endregion
                return lstComparativo;
            }
            catch (Exception e)
            {

                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error interno al intentar obtener el catálogo tipos de emleados");
                return null;
            }
        }

        public List<tblM_Comp_CatFinanciero> FillFinanciero()
        {
            List<tblM_Comp_CatFinanciero> data = new List<tblM_Comp_CatFinanciero>();
            data = _context.tblM_Comp_CatFinanciero.Where(x => x.estado == 1).ToList();
            return data;
        }

        public Dictionary<string, object> GuardarFinanciero(tblM_Comp_CatFinanciero financiero)
        {
            resultado = new Dictionary<string, object>();
            var existe = _context.tblM_Comp_CatFinanciero.Where(x => x.descripcion == financiero.descripcion && x.estado == 1).ToList();
            if (!existe.Any())
            {
                using (var dbContextTransaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.tblM_Comp_CatFinanciero.Add(financiero);
                        _context.SaveChanges();
                        dbContextTransaction.Commit();
                        resultado.Add("exito", true);
                        resultado.Add(SUCCESS, true);
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        LogError(1, 0, "CatMaquinaController", "GuardarFinanciero", e, AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(financiero));
                        resultado.Clear();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, e.Message);
                    }
                }

            }
            else
            {
                resultado.Add("exito", false);
                resultado.Add(SUCCESS, true);
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarPlazo(tblM_Comp_CapPlazo plazo)
        {
            resultado = new Dictionary<string, object>();
            var existe = _context.tblM_Comp_CapPlazo.Where(x => x.financieroID == plazo.financieroID && x.plazo == plazo.plazo && x.estado == 1).ToList();
            if (!existe.Any())
            {
                using (var dbContextTransaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.tblM_Comp_CapPlazo.Add(plazo);
                        _context.SaveChanges();
                        dbContextTransaction.Commit();
                        resultado.Add("exito", true);
                        resultado.Add(SUCCESS, true);
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        LogError(1, 0, "CatMaquinaController", "GuardarFinanciero", e, AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(plazo));
                        resultado.Clear();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, e.Message);
                    }
                }

            }
            else
            {
                resultado.Add("exito", false);
                resultado.Add(SUCCESS, true);
            }

            return resultado;
        }

        public Dictionary<string, object> GetPlazo(int financieroID, int plazoMeses)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                var plazos = _context.tblM_Comp_CapPlazo
                    .Where(x => (financieroID == 0 ? true : x.financieroID == financieroID) && (plazoMeses == 0 ? true : x.plazo == plazoMeses))
                    .OrderBy(x => x.financieroID)
                    .ToList();

                resultado.Add(ITEMS, plazos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> GetPlazoByID(int plazoID)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                var plazo = _context.tblM_Comp_CapPlazo
                    .FirstOrDefault(x => x.id == plazoID);

                resultado.Add("plazo", plazo);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }
        public Dictionary<string, object> EditarPlazo(tblM_Comp_CapPlazo plazo)
        {
            resultado = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var existe = _context.tblM_Comp_CapPlazo.FirstOrDefault(x => x.id == plazo.id);
                    if (existe != null)
                    {
                        existe.tipoOperacion = plazo.tipoOperacion;
                        existe.opcionCompra = plazo.opcionCompra;
                        existe.enganche = plazo.enganche;
                        existe.depositoPorcentaje = plazo.depositoPorcentaje;
                        existe.depositoMoneda = plazo.depositoMoneda;
                        existe.moneda = plazo.moneda;
                        existe.tasaInteres = plazo.tasaInteres;
                        existe.gastosFijos = plazo.gastosFijos;
                        existe.comision = plazo.comision;
                        existe.rentasGarantia = plazo.rentasGarantia;
                        existe.crecimientoPagos = plazo.crecimientoPagos;
                        _context.SaveChanges();
                        dbContextTransaction.Commit();
                        resultado.Add("exito", true);
                        resultado.Add(SUCCESS, true);
                    }
                    else
                    {
                        resultado.Add("exito", false);
                        resultado.Add(SUCCESS, true);
                    }
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(1, 0, "CatMaquinaController", "GuardarFinanciero", e, AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(plazo));
                    resultado.Clear();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }
        public Dictionary<string, object> FillCboFinancieros()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                var financieros = _context.tblM_Comp_CatFinanciero.Where(x => x.estado == 1).Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.descripcion
                }).ToList();
                resultado.Add(ITEMS, financieros);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;

        }


        public Dictionary<string, object> LlenarDatosFinanciero(int financieraID, int plazoMeses, decimal precio, int mesesRestante)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                var plazos = _context.tblM_Comp_CapPlazo.Where(x => x.financieroID == financieraID && x.plazo == plazoMeses).ToList();
                if (plazos.Count() > 0)
                {
                    var plazo = plazos.FirstOrDefault();
                    var total = precio * (decimal)1.16;
                    var monto = plazo.tipoOperacion == 1 ? precio : ((plazo.tipoOperacion == 2) ? total * (1 - (plazo.enganche / 100)) : precio);
                    var comisionDinero = monto * plazo.comision / 100;

                    decimal[] capital = new decimal[plazo.plazo];
                    decimal[] intereses = new decimal[plazo.plazo];
                    decimal[] ivaCapital = new decimal[plazo.plazo];
                    decimal[] ivaIntereses = new decimal[plazo.plazo];
                    decimal[] pagoTotal = new decimal[plazo.plazo];
                    decimal[] pagoFinal = new decimal[plazo.plazo];
                    decimal restaIntereses = 0;

                    for (int i = 0; i < plazo.plazo; i++)
                    {
                        capital[i] = plazo.tipoOperacion == 1 ? ((decimal)Financial.PPmt(((double)plazo.tasaInteres / 1200), (i + 1), plazo.plazo, -(double)monto * (1 - ((double)plazo.enganche / 100)))) : (monto / plazo.plazo);
                        if (plazo.tipoOperacion == 1 && i == (plazo.plazo - 1)) capital[i] += precio * plazo.enganche / 100;
                        if (i == 0) intereses[i] = (plazo.tipoOperacion == 1 ? precio : monto) * (plazo.tasaInteres / 1200);
                        else intereses[i] = (monto - restaIntereses) * (plazo.tasaInteres / 1200);
                        if (i == (plazo.plazo - 1)) intereses[i] += precio * plazo.opcionCompra / 100;
                        ivaCapital[i] = plazo.tipoOperacion == 1 ? capital[i] * (decimal)0.16 : 0;
                        ivaIntereses[i] = intereses[i] * (decimal)0.16;
                        pagoTotal[i] = capital[i] + intereses[i] + ivaCapital[i] + ivaIntereses[i];
                        pagoFinal[i] = (plazo.tipoOperacion == 1 && plazo.rentasGarantia > 0 && (i + 1) > (plazo.plazo - plazo.rentasGarantia)) ? pagoTotal[i] - pagoFinal[0] : pagoTotal[i];
                        if (i == (plazo.plazo - 1)) pagoFinal[i] -= plazo.depositoMoneda;
                        restaIntereses += capital[i];
                    }
                    decimal pagoInicial = (plazo.tipoOperacion == 1 ? ((plazo.rentasGarantia * pagoFinal[0]) + comisionDinero + plazo.gastosFijos) : (comisionDinero + plazo.gastosFijos + (plazo.enganche / 100 * total))) + plazo.depositoMoneda;
                    decimal importeEfectivo = (plazo.tipoOperacion == 1 ? -total : -monto) + pagoInicial;
                    double[] pagoFinalDouble = Array.ConvertAll(pagoFinal, x => (double)x);
                    pagoFinalDouble = (new double[] { (double)importeEfectivo }).Concat(pagoFinalDouble).ToArray();

                    double guessValue = 0.01;
                    decimal tasaEfectiva = 0;


                    while (tasaEfectiva == 0 && guessValue < 1)
                    {
                        try
                        {
                            tasaEfectiva = (decimal)Financial.IRR(ref pagoFinalDouble, guessValue) * 12;
                        }
                        catch (Exception)
                        {
                            guessValue = guessValue + 0.01;
                        }
                    }

                    var data = new
                    {
                        financiera = plazo.financiero.descripcion,
                        plazo = plazo.plazo,
                        precio = precio,
                        iva = precio * (decimal)0.16,
                        total = total,
                        monto = monto,
                        tipoDeArrendamiento = plazo.tipoOperacion,
                        opcionDeCompra = plazo.opcionCompra,
                        residual = plazo.enganche,
                        deposito = plazo.depositoMoneda,
                        moneda = plazo.moneda,
                        plazoFinal = plazo.plazo,
                        tasa = plazo.tasaInteres,
                        gastos = plazo.gastosFijos,
                        comision = plazo.comision,
                        comisionDinero = comisionDinero,
                        rentaFija = plazo.rentasGarantia,
                        crecimiento = plazo.crecimientoPagos,
                        enganche = pagoInicial,
                        intereses = intereses.Sum(),
                        tasaEfectiva = tasaEfectiva,
                        mensualidad = pagoTotal[0],
                        mensualidadNoIVA = pagoTotal[0] / (decimal)1.16,
                        totalAPagar = pagoFinal.Sum() + pagoInicial,
                        tiempoRestante = mesesRestante,
                    };
                    resultado.Add("datos", data);
                    resultado.Add("exito", true);

                    var categorias = new List<string>();
                    var calculos = new List<string>();
                    var series = new List<GpxSerieBarrasDoblesDTO>();

                    categorias.Add(plazos.First().financiero.descripcion);

                    var gpxBarras = new GpxsHighCharts();
                    var barraExterna = new GpxSerieBarrasDoblesDTO();
                    var barraInterna = new GpxSerieBarrasDoblesDTO();

                    var gpxLineas = new List<GpxSerieLineasBasicasDTO>();

                    barraExterna.name = "Intereses";
                    barraExterna.color = "rgba(30, 144, 255, 1)";
                    barraExterna.data = new List<decimal> { data.intereses };
                    barraExterna.pointPadding = 0.3M;
                    barraExterna.pointPlacement = -0.2M;

                    barraInterna.name = "Tasa efectiva";
                    barraInterna.color = "rgba(255, 140, 0, .9)";
                    barraInterna.data = new List<decimal> { (data.tasaEfectiva * 100) };
                    barraInterna.pointPadding = 0.4M;
                    barraInterna.pointPlacement = -0.2M;
                    barraInterna.yAxis = 1;

                    gpxBarras.categories = categorias;
                    gpxBarras.series.Add(barraExterna);
                    gpxBarras.series.Add(barraInterna);

                    resultado.Add("gpxBarra", gpxBarras);

                    var gpxLinea = new GpxSerieLineasBasicasDTO();

                    var _plazos = Convert.ToInt32(data.plazo);
                    for (int i = 1; i <= _plazos; i++)
                    {
                        if (i == 1)
                        {
                            gpxLinea.data.Add(data.enganche);
                            gpxLinea.name = plazos.First().financiero.descripcion;
                        }
                        else
                        {
                            gpxLinea.data.Add(data.mensualidad);
                        }
                    }
                    gpxLinea.data.Add(0);

                    gpxLineas.Add(gpxLinea);

                    calculos.Add("Intereses");
                    calculos.Add("Efectiva");

                    resultado.Add("gpxLinea", gpxLineas);


                    resultado.Add(SUCCESS, true);
                }
                else
                {
                    resultado.Add("exito", false);
                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;

        }

        public Dictionary<string, object> ObtenerMensualidades(int financieraID, int plazoMeses, decimal precio)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                var plazos = _context.tblM_Comp_CapPlazo.Where(x => x.financieroID == financieraID && x.plazo == plazoMeses).ToList();
                if (plazos.Count() > 0)
                {
                    var plazo = plazos.FirstOrDefault();
                    var total = precio * (decimal)1.16;
                    var monto = plazo.tipoOperacion == 1 ? precio : ((plazo.tipoOperacion == 2) ? total * (1 - (plazo.enganche / 100)) : precio);
                    var comisionDinero = monto * plazo.comision / 100;

                    decimal[] capital = new decimal[plazo.plazo];
                    decimal[] intereses = new decimal[plazo.plazo];
                    decimal[] ivaCapital = new decimal[plazo.plazo];
                    decimal[] ivaIntereses = new decimal[plazo.plazo];
                    decimal[] pagoTotal = new decimal[plazo.plazo];
                    decimal[] pagoFinal = new decimal[plazo.plazo];
                    decimal restaIntereses = 0;
                    List<MensualidadesFinancieroDTO> data = new List<MensualidadesFinancieroDTO>();

                    for (int i = 0; i < plazo.plazo; i++)
                    {
                        capital[i] = plazo.tipoOperacion == 1 ? ((decimal)Financial.PPmt(((double)plazo.tasaInteres / 1200), (i + 1), plazo.plazo, -(double)monto * (1 - ((double)plazo.enganche / 100)))) : (monto / plazo.plazo);
                        if (plazo.tipoOperacion == 1 && i == (plazo.plazo - 1)) capital[i] += precio * plazo.enganche / 100;
                        if (i == 0) intereses[i] = (plazo.tipoOperacion == 1 ? precio : monto) * (plazo.tasaInteres / 1200);
                        else intereses[i] = (monto - restaIntereses) * (plazo.tasaInteres / 1200);
                        if (i == (plazo.plazo - 1)) intereses[i] += precio * plazo.opcionCompra / 100;
                        ivaCapital[i] = plazo.tipoOperacion == 1 ? capital[i] * (decimal)0.16 : 0;
                        ivaIntereses[i] = intereses[i] * (decimal)0.16;
                        pagoTotal[i] = capital[i] + intereses[i] + ivaCapital[i] + ivaIntereses[i];
                        pagoFinal[i] = (plazo.tipoOperacion == 1 && plazo.rentasGarantia > 0 && (i + 1) > (plazo.plazo - plazo.rentasGarantia)) ? pagoTotal[i] - pagoFinal[0] : pagoTotal[i];
                        if (i == (plazo.plazo - 1)) pagoFinal[i] -= plazo.depositoMoneda;
                        restaIntereses += capital[i];
                        MensualidadesFinancieroDTO auxData = new MensualidadesFinancieroDTO
                        {
                            periodo = i + 1,
                            capital = Math.Round(capital[i] * 100, 2),
                            intereses = Math.Round(intereses[i] * 100, 2),
                            ivaCapital = Math.Round(ivaCapital[i] * 100, 2),
                            ivaIntereses = Math.Round(ivaIntereses[i] * 100, 2),
                            pagoTotal = Math.Round(pagoTotal[i] * 100, 2),
                            pagoFinal = Math.Round(pagoFinal[i] * 100, 2)
                        };
                        data.Add(auxData);
                    }
                    decimal pagoInicial = (plazo.tipoOperacion == 1 ? ((plazo.rentasGarantia * pagoFinal[0]) + comisionDinero + plazo.gastosFijos) : (comisionDinero + plazo.gastosFijos + (plazo.enganche / 100 * total))) + plazo.depositoMoneda;
                    decimal importeEfectivo = (plazo.tipoOperacion == 1 ? -total : -monto) + pagoInicial;
                    double[] pagoFinalDouble = Array.ConvertAll(pagoFinal, x => (double)x);
                    pagoFinalDouble = (new double[] { (double)importeEfectivo }).Concat(pagoFinalDouble).ToArray();

                    double guessValue = 0.01;
                    decimal tasaEfectiva = 0;


                    while (tasaEfectiva == 0 && guessValue < 1)
                    {
                        try
                        {
                            tasaEfectiva = (decimal)Financial.IRR(ref pagoFinalDouble, guessValue) * 12;
                        }
                        catch (Exception)
                        {
                            guessValue = guessValue + 0.01;
                        }
                    }
                    double mensualidad = 45046.07;

                    var tasa = CalcularTasa((double)precio, mensualidad, (double)plazo.enganche, plazo.tipoOperacion, plazo.plazo);

                    resultado.Add("mensualidades", data);

                    resultado.Add("importeEfectivo", -importeEfectivo);
                    resultado.Add("tasaEfectiva", tasaEfectiva);

                    resultado.Add("exito", true);
                    resultado.Add(SUCCESS, true);
                }
                else
                {
                    resultado.Add("exito", false);
                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;

        }
        public byte[] descargarArchivo(long examen_id)
        {
            var resultado = new Dictionary<string, object>();
            Stream fileStream;
            try
            {
                string pathExamen = _context.tblM_ComparativoAdquisicionyRentaDet.Where(x => x.id == examen_id).FirstOrDefault().rutaArchivo;
                if (pathExamen != null)
                {
                    string rutaFisica = Path.Combine(rutaArchivos, pathExamen);
                    fileStream = GlobalUtils.GetFileAsStream(rutaFisica);

                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                fileStream = null;
            }

            //resultado.Add("nombreDescarga", version.nombre);
            //resultado.Add(SUCCESS, true);
            return ReadFully(fileStream);
        }
        public string getFileName(long examen_id)
        {
            string fileName = "";
            try
            {
                string pathExamen = _context.tblM_ComparativoAdquisicionyRentaDet.Where(x => x.id == examen_id).FirstOrDefault().rutaArchivo;
                fileName = pathExamen.Split('\\')[5];
            }
            catch (Exception e)
            {
                fileName = "";
            }

            return fileName;
        }
        public static byte[] ReadFully(Stream input)
        {
            MemoryStream ms = new MemoryStream();
            if (input != null)
            {
                byte[] buffer = new byte[16 * 1024];
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
            }
            return ms.ToArray();
        }
        public string getDescripcion(string AreaCuenta)
        {
            string returngetDescripcion = "";
            return returngetDescripcion = _context.tblP_CC.Where(x => x.cc == AreaCuenta).Select(y => y.descripcion).FirstOrDefault();
        }
        public int ObtenerEstatus(int idAsignacion)
        {
            int retornar = 0;

            var obj = _context.tblM_ComparativoAdquisicionyRenta.Where(r => r.idAsignacion == idAsignacion).FirstOrDefault();
            if (obj != null)
            {
                retornar = obj.estatus;
            }
            else
            {
                retornar = 0;
            }

            return retornar;
        }
        public bool ActivarBtn(int id)
        {
            bool retornarActivarBtn = false;
            try
            {
                var lstAutorizante = _context.tblM_ComparativoAdquisicionyRentaAutorizante.Where(x => x.idAsignacion == id).ToList();
                if (lstAutorizante.Count == 5)
                {
                    return retornarActivarBtn = true;
                }
                else
                {
                    retornarActivarBtn = false;
                }
                var lstAutorizanteFin = _context.tblM_ComparativoFinancieroAutorizante.Where(x => x.idAsignacion == id).ToList();
                if (lstAutorizanteFin.Count == 5)
                {
                    return retornarActivarBtn = true;
                }
                else
                {
                    retornarActivarBtn = false;
                }
            }
            catch (Exception)
            {
                retornarActivarBtn = false;
                throw;
            }

            return retornarActivarBtn;
        }

        private decimal CalcularTasa(double precio, double mensualidad, double enganche, int tipoOperacion, int plazo)
        {
            double tasa = 0;
            if (tipoOperacion == 1)
            {
                double guessValue = 0.01;
                List<double> pagoFinal = new List<double>();

                pagoFinal.Add(-precio * 1.16);
                for (int i = 0; i < plazo; i++)
                {
                    if (i == plazo - 1) { pagoFinal.Add(mensualidad + (precio * (enganche / 100) * 1.16)); }
                    else pagoFinal.Add(mensualidad);
                }
                double[] pagoFinalDouble = pagoFinal.ToArray();

                while (tasa == 0 && guessValue < 1)
                {
                    try
                    {
                        tasa = Financial.IRR(ref pagoFinalDouble, guessValue) * 12;
                    }
                    catch (Exception)
                    {
                        guessValue = guessValue + 0.01;
                    }
                }
            }
            else
            {
                tasa = ((double)1200 / (double)1.16) * (mensualidad / (precio) - 1 / (double)plazo);
            }
            return (decimal)tasa;
        }
        public int obtenerEstatusAutorizado(int Financiero, int idAsignacion, int idUsuario)
        {
            int rotornarv = 0;
            if (Financiero == 1)
            {
                var obj = _context.tblM_ComparativoFinancieroAutorizante.Where(r => r.autorizanteID == idUsuario && r.idAsignacion == idAsignacion).FirstOrDefault();
                if (obj != null)
                {
                    if (obj.firma != "")
                    {
                        return rotornarv = 1;
                    }
                    else
                    {
                        return rotornarv = 0;
                    }
                }
            }
            else
            {
                var obj = _context.tblM_ComparativoAdquisicionyRentaAutorizante.Where(r => r.autorizanteID == idUsuario && r.idAsignacion == idAsignacion).FirstOrDefault();
                if (obj != null)
                {
                    if (obj.firma != "")
                    {
                        return rotornarv = 1;
                    }
                    else
                    {
                        return rotornarv = 0;
                    }
                }
            }
            return rotornarv;
        }
    }
}
