using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Metete.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "admin_notificacion",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Titulo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Mensaje = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdEstado = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "categoria_genero",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "criterio_mvp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "funcionalidad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Codigo = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EsDePago = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "metodo_pago",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "nacionalidad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "region",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "rol",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tipo_deporte",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequierePosicion = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tipo_divisa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tipo_genero",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tipo_membresia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tipo_notificacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Titulo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Mensaje = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "detalle_funcionalidad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdFuncionalidad = table.Column<int>(type: "int", nullable: false),
                    NombreFormulario = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Codigo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "fk_detalle_funcionalidad_funcionalidad",
                        column: x => x.IdFuncionalidad,
                        principalTable: "funcionalidad",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "comuna",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdRegion = table.Column<int>(type: "int", nullable: false),
                    Latitud = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    Longitud = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    Ubicacion = table.Column<Point>(type: "point", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_comuna_region",
                        column: x => x.IdRegion,
                        principalTable: "region",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "centro_deporte",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Direccion = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdComuna = table.Column<int>(type: "int", nullable: false),
                    Latitud = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    Longitud = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    Ubicacion = table.Column<Point>(type: "point", nullable: false),
                    Aprobado = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "fk_centro_deporte_comuna",
                        column: x => x.IdComuna,
                        principalTable: "comuna",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Administrador = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApellidoPaterno = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApellidoMaterno = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdTipoGenero = table.Column<int>(type: "int", nullable: false),
                    CodigoPais = table.Column<int>(type: "int", nullable: false),
                    Telefono = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdPaisResidencia = table.Column<int>(type: "int", nullable: false),
                    IdNacionalidad = table.Column<int>(type: "int", nullable: false),
                    Direccion = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdComuna = table.Column<int>(type: "int", nullable: false),
                    Latitud = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    Longitud = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    IdTipoMembresia = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Eliminado = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FcmToken = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RecoveryCode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RecoveryCodeExpiryTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    KmBusqueda = table.Column<int>(type: "int", nullable: true),
                    Foto = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RefreshToken = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "fk_usuario_comuna",
                        column: x => x.IdComuna,
                        principalTable: "comuna",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_usuario_nacionalidad",
                        column: x => x.IdNacionalidad,
                        principalTable: "nacionalidad",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_usuario_pais",
                        column: x => x.IdPaisResidencia,
                        principalTable: "pais",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_usuario_tipo_genero",
                        column: x => x.IdTipoGenero,
                        principalTable: "tipo_genero",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_usuario_tipo_membresia",
                        column: x => x.IdTipoMembresia,
                        principalTable: "tipo_membresia",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "evento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdTipoDeporte = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaEvento = table.Column<DateTime>(type: "datetime", nullable: false),
                    ZonaHorariaEvento = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Duracion = table.Column<int>(type: "int", nullable: false),
                    IdCentroDeporte = table.Column<int>(type: "int", nullable: true),
                    OtroCentroDeporte = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NombreCentroDeporte = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DireccionCentroDeporte = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LatitudCentroDeporte = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: true),
                    LongitudCentroDeporte = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: true),
                    UbicacionCentroDeporte = table.Column<Point>(type: "point", nullable: true),
                    NumJugadores = table.Column<int>(type: "int", nullable: false),
                    PrecioPorPersona = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IdMetodoPago = table.Column<int>(type: "int", nullable: false),
                    IdTipoDivisa = table.Column<int>(type: "int", nullable: false),
                    DevolucionAbandono = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RecordarEventoJugador = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ObligatorioDisponerTelefono = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IdCategoriaGeneroPermitido = table.Column<int>(type: "int", nullable: false),
                    IdCreador = table.Column<int>(type: "int", nullable: false),
                    Creador = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", maxLength: 6, nullable: false),
                    Modificador = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime(6)", maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "fk_evento_categoriagenero",
                        column: x => x.IdCategoriaGeneroPermitido,
                        principalTable: "categoria_genero",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_evento_centro_deporte",
                        column: x => x.IdCentroDeporte,
                        principalTable: "centro_deporte",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_evento_metodo_pago",
                        column: x => x.IdMetodoPago,
                        principalTable: "metodo_pago",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_evento_tipo_deporte",
                        column: x => x.IdTipoDeporte,
                        principalTable: "tipo_deporte",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_evento_tipo_divisa",
                        column: x => x.IdTipoDivisa,
                        principalTable: "tipo_divisa",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_evento_usuario",
                        column: x => x.IdCreador,
                        principalTable: "usuario",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "usuario_comuna",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdComuna = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "fk_usuario_comuna_comuna",
                        column: x => x.IdComuna,
                        principalTable: "comuna",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_usuario_comuna_usuario",
                        column: x => x.IdUsuario,
                        principalTable: "usuario",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "usuario_horario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    DiaDeLaSemana = table.Column<int>(type: "int", nullable: false),
                    HorarioInicio = table.Column<TimeSpan>(type: "time", nullable: false),
                    HorarioTermino = table.Column<TimeSpan>(type: "time", nullable: false),
                    ZonaHoraria = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Disponible = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "fk_usuario_horario_usuario",
                        column: x => x.IdUsuario,
                        principalTable: "usuario",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "usuario_tipo_deporte",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdTipoDeporte = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "fk_usuario_tipo_deporte_tipo_deporte",
                        column: x => x.IdTipoDeporte,
                        principalTable: "tipo_deporte",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_usuario_tipo_deporte_usuario",
                        column: x => x.IdUsuario,
                        principalTable: "usuario",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "usuario_tipo_notificacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdTipoNotificacion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "fk_usuario_tipo_notificacion_tipo_notificacion",
                        column: x => x.IdTipoNotificacion,
                        principalTable: "tipo_notificacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_usuario_tipo_notificacion_usuario",
                        column: x => x.IdUsuario,
                        principalTable: "usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "notificacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdTipoNotificacion = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Mensaje = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdEvento = table.Column<int>(type: "int", nullable: true),
                    IdEstadoNotificacion = table.Column<int>(type: "int", nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "datetime", nullable: true),
                    Leido = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "fk_notificacion_evento",
                        column: x => x.IdEvento,
                        principalTable: "evento",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_notificacion_tipo_notificacion",
                        column: x => x.IdTipoNotificacion,
                        principalTable: "tipo_notificacion",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_notificacion_usuario",
                        column: x => x.IdUsuario,
                        principalTable: "usuario",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "usuario_evaluacion",
                columns: table => new
                {
                    IdUsuarioEvaluacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdEvento = table.Column<int>(type: "int", nullable: false),
                    IdEvaluacion = table.Column<int>(type: "int", nullable: false),
                    IdCriterioMvp = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdEvaluador = table.Column<int>(type: "int", nullable: false),
                    Nota = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.IdUsuarioEvaluacion);
                    table.ForeignKey(
                        name: "fk_usuario_evaluacion_criterio_Mvp",
                        column: x => x.IdCriterioMvp,
                        principalTable: "criterio_mvp",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_usuario_evaluacion_evento",
                        column: x => x.IdEvento,
                        principalTable: "evento",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_usuario_evaluacion_usuario",
                        column: x => x.IdUsuario,
                        principalTable: "usuario",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "usuario_evento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdEvento = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdRol = table.Column<int>(type: "int", nullable: false),
                    Aprobado = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Posicion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "fk_usuario_evento_evento",
                        column: x => x.IdEvento,
                        principalTable: "evento",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_usuario_evento_rol",
                        column: x => x.IdRol,
                        principalTable: "rol",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_usuario_evento_usuario",
                        column: x => x.IdUsuario,
                        principalTable: "usuario",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "fk_centro_deporte_comuna_idx",
                table: "centro_deporte",
                column: "IdComuna");

            migrationBuilder.CreateIndex(
                name: "fk_comuna_region_idx",
                table: "comuna",
                column: "IdRegion");

            migrationBuilder.CreateIndex(
                name: "fk_detalle_funcionalidad_funcionalidad_idx",
                table: "detalle_funcionalidad",
                column: "IdFuncionalidad");

            migrationBuilder.CreateIndex(
                name: "fk_evento_categoriagenero_idx",
                table: "evento",
                column: "IdCategoriaGeneroPermitido");

            migrationBuilder.CreateIndex(
                name: "fk_evento_centro_deporte_idx",
                table: "evento",
                column: "IdCentroDeporte");

            migrationBuilder.CreateIndex(
                name: "fk_evento_metodo_pago_idx",
                table: "evento",
                column: "IdMetodoPago");

            migrationBuilder.CreateIndex(
                name: "fk_evento_tipo_deporte_idx",
                table: "evento",
                column: "IdTipoDeporte");

            migrationBuilder.CreateIndex(
                name: "fk_evento_tipo_divisa_idx",
                table: "evento",
                column: "IdTipoDivisa");

            migrationBuilder.CreateIndex(
                name: "fk_evento_usuario_idx",
                table: "evento",
                column: "IdCreador");

            migrationBuilder.CreateIndex(
                name: "fk_notificacion_evento_idx",
                table: "notificacion",
                column: "IdEvento");

            migrationBuilder.CreateIndex(
                name: "fk_notificacion_tipo_notificacion_idx",
                table: "notificacion",
                column: "IdTipoNotificacion");

            migrationBuilder.CreateIndex(
                name: "fk_notificacion_usuario_idx",
                table: "notificacion",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "fk_usuario_comuna_idx",
                table: "usuario",
                column: "IdComuna");

            migrationBuilder.CreateIndex(
                name: "fk_usuario_nacionalidad_idx",
                table: "usuario",
                column: "IdNacionalidad");

            migrationBuilder.CreateIndex(
                name: "fk_usuario_pais_idx",
                table: "usuario",
                column: "IdPaisResidencia");

            migrationBuilder.CreateIndex(
                name: "fk_usuario_tipo_genero_idx",
                table: "usuario",
                column: "IdTipoGenero");

            migrationBuilder.CreateIndex(
                name: "fk_usuario_tipo_membresia_idx",
                table: "usuario",
                column: "IdTipoMembresia");

            migrationBuilder.CreateIndex(
                name: "fk_usuario_comuna_comuna_idx",
                table: "usuario_comuna",
                column: "IdComuna");

            migrationBuilder.CreateIndex(
                name: "fk_usuario_comuna_usuario_idx",
                table: "usuario_comuna",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "fk_usuario_evaluacion_criterio_Mvp_idx",
                table: "usuario_evaluacion",
                column: "IdCriterioMvp");

            migrationBuilder.CreateIndex(
                name: "fk_usuario_evaluacion_evento_idx",
                table: "usuario_evaluacion",
                column: "IdEvento");

            migrationBuilder.CreateIndex(
                name: "fk_usuario_evaluacion_usuario_idx",
                table: "usuario_evaluacion",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "fk_usuario_evento_evento_idx",
                table: "usuario_evento",
                column: "IdEvento");

            migrationBuilder.CreateIndex(
                name: "fk_usuario_evento_rol_idx",
                table: "usuario_evento",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "fk_usuario_evento_usuario_idx",
                table: "usuario_evento",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "fk_usuario_horario_usuario1_idx",
                table: "usuario_horario",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "fk_usuario_tipo_deporte_tipo_deporte_idx",
                table: "usuario_tipo_deporte",
                column: "IdTipoDeporte");

            migrationBuilder.CreateIndex(
                name: "fk_usuario_tipo_deporte_usuario_idx",
                table: "usuario_tipo_deporte",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "fk_usuario_tipo_notificacion_tipo_notificacion_idx",
                table: "usuario_tipo_notificacion",
                column: "IdTipoNotificacion");

            migrationBuilder.CreateIndex(
                name: "fk_usuario_tipo_notificacion_usuario_idx",
                table: "usuario_tipo_notificacion",
                column: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admin_notificacion");

            migrationBuilder.DropTable(
                name: "detalle_funcionalidad");

            migrationBuilder.DropTable(
                name: "notificacion");

            migrationBuilder.DropTable(
                name: "usuario_comuna");

            migrationBuilder.DropTable(
                name: "usuario_evaluacion");

            migrationBuilder.DropTable(
                name: "usuario_evento");

            migrationBuilder.DropTable(
                name: "usuario_horario");

            migrationBuilder.DropTable(
                name: "usuario_tipo_deporte");

            migrationBuilder.DropTable(
                name: "usuario_tipo_notificacion");

            migrationBuilder.DropTable(
                name: "funcionalidad");

            migrationBuilder.DropTable(
                name: "criterio_mvp");

            migrationBuilder.DropTable(
                name: "evento");

            migrationBuilder.DropTable(
                name: "rol");

            migrationBuilder.DropTable(
                name: "tipo_notificacion");

            migrationBuilder.DropTable(
                name: "categoria_genero");

            migrationBuilder.DropTable(
                name: "centro_deporte");

            migrationBuilder.DropTable(
                name: "metodo_pago");

            migrationBuilder.DropTable(
                name: "tipo_deporte");

            migrationBuilder.DropTable(
                name: "tipo_divisa");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "comuna");

            migrationBuilder.DropTable(
                name: "nacionalidad");

            migrationBuilder.DropTable(
                name: "pais");

            migrationBuilder.DropTable(
                name: "tipo_genero");

            migrationBuilder.DropTable(
                name: "tipo_membresia");

            migrationBuilder.DropTable(
                name: "region");
        }
    }
}
