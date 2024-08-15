using Core.DAO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Captura;
using Core.DTO.Utils.Data;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.Maquinaria.Catalogo;
using Core.Enum.Maquinaria;
using Core.Enum.Multiempresa;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using Infrastructure.Utils;
using Infrastructure.DTO;
using Core.DTO;

namespace Data.DAO.Maquinaria.Captura
{
    public class AceitesLubricantesDAO : GenericDAO<tblM_CatAceitesLubricantes>, IAceitesLubricantesDAO
    {
        public List<tblM_CatAceitesLubricantes> GetAllAceitesLubricantes(int tipoId, string economico)
        {

            tblM_CatMaquina maquinaria = new tblM_CatMaquina();
            if (tipoId != 0)
            {
                maquinaria = _context.tblM_CatMaquina.FirstOrDefault(x => x.noEconomico == economico);
            }


            if (!string.IsNullOrEmpty(economico))
            {
                return _context.tblM_CatAceitesLubricantes.Where(x => x.modeloID == maquinaria.modeloEquipoID && x.subConjuntoID == tipoId).ToList();

            }
            else
            {
                return _context.tblM_CatAceitesLubricantes.ToList();
            }


        }
        public object ExistenciaLubricante(string almacen)
        {
            try
            {
                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {
                    case EmpresaEnum.Peru:
                        {
                            using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                            {
                                var listaInsumos = _starsoft.MAEART.ToList();
                                var listaExistencias = _starsoft.STKART.Where(x => x.STALMA == almacen && x.STSKDIS > 0).ToList().Where(x => x.STCODIGO.StartsWith("010020")).Select(x => new buscarPorAlmacenDTO
                                {
                                    almacen = Int32.Parse(x.STALMA),
                                    descripcion = listaInsumos.Where(y => y.ACODIGO == x.STCODIGO).Select(z => z.ADESCRI).FirstOrDefault(),
                                    existencia = (int)x.STSKDIS,
                                    insumo = Int32.Parse(x.STCODIGO)
                                });

                                return listaExistencias;
                            }
                        }
                    default:
                        {
                            List<buscarPorAlmacenDTO> listaLubricanteARR = _contextEnkontrol.Select<buscarPorAlmacenDTO>(EnkontrolEnum.ArrenProd, new OdbcConsultaDTO()
                            {
                                consulta = @"
                                    SELECT
                                        mov.almacen,
                                        det.insumo,
                                        inn.descripcion,
                                        SUM(det.Cantidad * IF mov.tipo_mov IN (1,2,3,4,5) THEN 1 ELSE -1 ENDIF) AS Existencia 
                                    FROM si_movimientos mov                                           
                                        INNER JOIN si_movimientos_det det ON det.almacen = mov.almacen AND det.tipo_mov = mov.tipo_mov AND det.numero = mov.numero
                                        INNER JOIN insumos inn ON det.insumo = inn.insumo
                                    WHERE mov.almacen = " + almacen + @" AND det.insumo < 1040000                                
                                    GROUP BY  mov.almacen, det.insumo, det.area_alm, det.lado_alm, det.estante_alm, det.nivel_alm, inn.descripcion
                                    ORDER BY det.insumo DESC, descripcion ASC",
                                parametros = new List<OdbcParameterDTO> { new OdbcParameterDTO() { nombre = "almacen", tipo = OdbcType.VarChar, valor = String.Format("%{0}%", almacen) } }
                            }).Where(x => x.existencia != 0 && x.insumo != 101).ToList();

                            return listaLubricanteARR;
                        }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region Catálogo Lubricantes
        public Dictionary<string, object> CargarCatalogoLubricantes()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var listaModelos = _context.tblM_CatModeloEquipo.Where(x => x.estatus).ToList();
                var listaSubconjuntos = Enum.GetValues(typeof(AceiteLubricanteEnum)).Cast<AceiteLubricanteEnum>();
                var listaLubricantes = _context.tblM_CatAceitesLubricantes.Where(x => x.registroActivo).ToList().Select(x => new
                {
                    id = x.id,
                    Descripcion = x.Descripcion,
                    modeloID = x.modeloID,
                    modeloDesc = listaModelos.Where(y => y.id == x.modeloID).Select(z => z.descripcion).FirstOrDefault(),
                    subConjuntoID = x.subConjuntoID,
                    subconjuntoDesc = listaSubconjuntos.Where(y => y == (AceiteLubricanteEnum)x.subConjuntoID).Select(z => z.GetDescription()).FirstOrDefault()
                }).ToList();

                resultado.Add("data", listaLubricantes);
                resultado.Add(SUCCESS, true);
            }

            catch (Exception e)
            {
                LogError(0, 0, "AceitesLubricantesController", "CargarCatalogoLubricantes", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> GetComboModelos()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var listaModelos = _context.tblM_CatModeloEquipo.Where(x => x.estatus).ToList().Select(x => new ComboDTO
                {
                    Value = x.id,
                    Text = x.descripcion
                });

                resultado.Add(ITEMS, listaModelos);
                resultado.Add(SUCCESS, true);
            }

            catch (Exception e)
            {
                LogError(0, 0, "AceitesLubricantesController", "GetComboModelos", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarNuevoLubricante(tblM_CatAceitesLubricantes lubricante)
        {
            var resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var registroExistente = _context.tblM_CatAceitesLubricantes.FirstOrDefault(x => x.registroActivo && x.Descripcion == lubricante.Descripcion && x.modeloID == lubricante.modeloID && x.subConjuntoID == lubricante.subConjuntoID);

                    if (registroExistente != null)
                    {
                        throw new Exception("Ya existe un lubricante con esa información.");
                    }

                    _context.tblM_CatAceitesLubricantes.Add(lubricante);
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, "AceitesLubricantesController", "GuardarNuevoLubricante", e, AccionEnum.AGREGAR, 0, lubricante);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EditarLubricante(tblM_CatAceitesLubricantes lubricante, AceiteLubricanteEnum subConjuntoID_Anterior)
        {
            var resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region Verificar Registro tblM_MaquinariaAceitesLubricantes Existente
                    tblM_MaquinariaAceitesLubricantes registroMaquinaria = null;

                    switch (subConjuntoID_Anterior)
                    {
                        case AceiteLubricanteEnum.MOTOR:
                            {
                                registroMaquinaria = _context.tblM_MaquinariaAceitesLubricantes.FirstOrDefault(x => x.MotorId == lubricante.id);
                                break;
                            }
                        case AceiteLubricanteEnum.TRANSMISION:
                            {
                                registroMaquinaria = _context.tblM_MaquinariaAceitesLubricantes.FirstOrDefault(x => x.TransmisionID == lubricante.id);
                                break;
                            }
                        case AceiteLubricanteEnum.HIDRAULICO:
                            {
                                registroMaquinaria = _context.tblM_MaquinariaAceitesLubricantes.FirstOrDefault(x => x.HidraulicoID == lubricante.id);
                                break;
                            }
                        case AceiteLubricanteEnum.DIFERENCIAL:
                            {
                                registroMaquinaria = _context.tblM_MaquinariaAceitesLubricantes.FirstOrDefault(x => x.DiferencialId == lubricante.id);
                                break;
                            }
                        case AceiteLubricanteEnum.MANDOS_FINALES_TANDEMS:
                            {
                                registroMaquinaria = _context.tblM_MaquinariaAceitesLubricantes.FirstOrDefault(x => x.MFTIzqId == lubricante.id);
                                break;
                            }
                        case AceiteLubricanteEnum.DIRECCION:
                            {
                                registroMaquinaria = _context.tblM_MaquinariaAceitesLubricantes.FirstOrDefault(x => x.DirId == lubricante.id);
                                break;
                            }
                        case AceiteLubricanteEnum.OTROS1:
                            {
                                registroMaquinaria = _context.tblM_MaquinariaAceitesLubricantes.FirstOrDefault(x => x.otroId1 == lubricante.id);
                                break;
                            }
                        case AceiteLubricanteEnum.OTROS2:
                            {
                                registroMaquinaria = _context.tblM_MaquinariaAceitesLubricantes.FirstOrDefault(x => x.otroId2 == lubricante.id);
                                break;
                            }
                        case AceiteLubricanteEnum.OTROS3:
                            {
                                registroMaquinaria = _context.tblM_MaquinariaAceitesLubricantes.FirstOrDefault(x => x.otroId3 == lubricante.id);
                                break;
                            }
                        case AceiteLubricanteEnum.OTROS4:
                            {
                                registroMaquinaria = _context.tblM_MaquinariaAceitesLubricantes.FirstOrDefault(x => x.otroId4 == lubricante.id);
                                break;
                            }
                    }
                    #endregion

                    if ((AceiteLubricanteEnum)lubricante.subConjuntoID != subConjuntoID_Anterior)
                    {
                        var registroExistente = _context.tblM_CatAceitesLubricantes.FirstOrDefault(x => x.registroActivo && x.Descripcion == lubricante.Descripcion && x.modeloID == lubricante.modeloID && x.subConjuntoID == lubricante.subConjuntoID);

                        if (registroExistente != null)
                        {
                            throw new Exception("Ya existe un lubricante con esa información.");
                        }
                    }

                    var registroLubricante = _context.tblM_CatAceitesLubricantes.FirstOrDefault(x => x.id == lubricante.id);

                    if (registroMaquinaria == null) //Registro tblM_MaquinariaAceitesLubricantes no existente. Se puede editar toda la información.
                    {
                        registroLubricante.Descripcion = lubricante.Descripcion;
                        registroLubricante.modeloID = lubricante.modeloID;
                        registroLubricante.subConjuntoID = lubricante.subConjuntoID;
                        _context.SaveChanges();
                    }
                    else //Registro tblM_MaquinariaAceitesLubricantes existente. Nomás se puede editar la descripción.
                    {
                        registroLubricante.Descripcion = lubricante.Descripcion;
                        _context.SaveChanges();
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, "AceitesLubricantesController", "EditarLubricante", e, AccionEnum.ACTUALIZAR, 0, lubricante);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EliminarLubricante(tblM_CatAceitesLubricantes lubricante)
        {
            var resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region Validación Registro tblM_MaquinariaAceitesLubricantes Existente
                    tblM_MaquinariaAceitesLubricantes registroMaquinaria = null;

                    switch ((AceiteLubricanteEnum)lubricante.subConjuntoID)
                    {
                        case AceiteLubricanteEnum.MOTOR:
                            {
                                registroMaquinaria = _context.tblM_MaquinariaAceitesLubricantes.FirstOrDefault(x => x.MotorId == lubricante.id);
                                break;
                            }
                        case AceiteLubricanteEnum.TRANSMISION:
                            {
                                registroMaquinaria = _context.tblM_MaquinariaAceitesLubricantes.FirstOrDefault(x => x.TransmisionID == lubricante.id);
                                break;
                            }
                        case AceiteLubricanteEnum.HIDRAULICO:
                            {
                                registroMaquinaria = _context.tblM_MaquinariaAceitesLubricantes.FirstOrDefault(x => x.HidraulicoID == lubricante.id);
                                break;
                            }
                        case AceiteLubricanteEnum.DIFERENCIAL:
                            {
                                registroMaquinaria = _context.tblM_MaquinariaAceitesLubricantes.FirstOrDefault(x => x.DiferencialId == lubricante.id);
                                break;
                            }
                        case AceiteLubricanteEnum.MANDOS_FINALES_TANDEMS:
                            {
                                registroMaquinaria = _context.tblM_MaquinariaAceitesLubricantes.FirstOrDefault(x => x.MFTIzqId == lubricante.id);
                                break;
                            }
                        case AceiteLubricanteEnum.DIRECCION:
                            {
                                registroMaquinaria = _context.tblM_MaquinariaAceitesLubricantes.FirstOrDefault(x => x.DirId == lubricante.id);
                                break;
                            }
                        case AceiteLubricanteEnum.OTROS1:
                            {
                                registroMaquinaria = _context.tblM_MaquinariaAceitesLubricantes.FirstOrDefault(x => x.otroId1 == lubricante.id);
                                break;
                            }
                        case AceiteLubricanteEnum.OTROS2:
                            {
                                registroMaquinaria = _context.tblM_MaquinariaAceitesLubricantes.FirstOrDefault(x => x.otroId2 == lubricante.id);
                                break;
                            }
                        case AceiteLubricanteEnum.OTROS3:
                            {
                                registroMaquinaria = _context.tblM_MaquinariaAceitesLubricantes.FirstOrDefault(x => x.otroId3 == lubricante.id);
                                break;
                            }
                        case AceiteLubricanteEnum.OTROS4:
                            {
                                registroMaquinaria = _context.tblM_MaquinariaAceitesLubricantes.FirstOrDefault(x => x.otroId4 == lubricante.id);
                                break;
                            }
                    }

                    if (registroMaquinaria != null)
                    {
                        throw new Exception("No se puede eliminar el registro porque ya se está utilizando.");
                    }
                    #endregion

                    var registroLubricante = _context.tblM_CatAceitesLubricantes.FirstOrDefault(x => x.id == lubricante.id);

                    registroLubricante.registroActivo = false;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, "AceitesLubricantesController", "EliminarLubricante", e, AccionEnum.ELIMINAR, 0, lubricante);
                }
            }

            return resultado;
        }
        #endregion
    }
}
