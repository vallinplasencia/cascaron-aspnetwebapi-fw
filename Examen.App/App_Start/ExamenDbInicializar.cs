using Bogus;
using Examen.AccesoDatos.Context;
using Examen.App.Util.Seguridad;
using Examen.Dominio.Entidades;
using Examen.Dominio.Util;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Examen.App.App_Start
{
    public class ExamenDbInicializar : DropCreateDatabaseIfModelChanges<AppDbContext>
    {
        private AppDbContext db;
        protected override void Seed(AppDbContext context)
        {
            db = context;

            SalvarUsuariosyTrabajadores();
            SalvarActividades();



            //SalvarCategorias();
            //SalvarResponsables();
            //SalvarActivos();



            base.Seed(context);
        }


        //Guarda en la BD a los trabajadores y los usuarios de esos trabajadores
        private void SalvarUsuariosyTrabajadores()
        {
            var emailsDeUsuarios = new[] {
                "admin@nauta.cu",
                "pepe@nauta.cu",
                "juan@nauta.cu",

                "usuario@nauta.cu",
                "chichou@nauta.cu",
                "kukou@nauta.cu"
            };

            var faker = new Faker();
            var trabajadores = GenerarTrabajadores(emailsDeUsuarios.Length);
            for (int i = 0; i < emailsDeUsuarios.Length; i++)
            {
                trabajadores[i].User = new AppUser
                {
                    Email = emailsDeUsuarios[i],
                    UserName = emailsDeUsuarios[i],
                };
                //Asignano oleatoriamente Jefe
                if (i != 0 && faker.Random.Bool())
                {
                    trabajadores[i].Jefe = trabajadores[i - 1];
                }
            }
            db.Trabajadores.AddRange(trabajadores);
            db.SaveChanges();

            //Agrgando clave a los usuarios
            var userManager = new ApplicationUserManager(new UserStore<AppUser>(db));
            for (int i = 0; i < trabajadores.Count; i++)
            {
                userManager.AddPassword(trabajadores[i].UserId, "Admin123.");
            }


        }

        //Guarda en la BD a los actividade
        private void SalvarActividades()
        {
            var faker = new Faker();
            var actividades = GenerarActividades();
            var trabajadores = db.Trabajadores.ToArray();

            //Asigandole el trabajador que creo la actividad y al q se le asigno dicha actividad
            foreach (var act in actividades)
            {
                var creadoId = faker.PickRandom(trabajadores).UserId;
                act.CreadaPorId = creadoId;

                if (faker.Random.Bool())
                {
                    var asignarId = faker.PickRandom(trabajadores).UserId;
                    while (creadoId == asignarId)
                    {
                        asignarId = faker.PickRandom(trabajadores).UserId;
                    }
                    act.AsignadaAId = asignarId;
                }
            }
            db.Actividades.AddRange(actividades);
            db.SaveChanges();
        }

        //No lo utilizo
        private void SalvarUsuarios()
        {
            //Salvando roles de usuarios
            var roleAdmin = new AppRole { Name = TiposRole.Admin };
            var roleUsuario = new AppRole { Name = TiposRole.Usuario };

            var roleManager = new ApplicationRoleManager(new RoleStore<AppRole>(db));
            roleManager.Create(roleAdmin);
            roleManager.Create(roleUsuario);

            var trabajadores = GenerarTrabajadores();

            //Salvando los usuarios iniciales de la app            
            var admin = new AppUser
            {
                Email = "admin@nauta.cu",
                UserName = "admin@nauta.cu"
            };
            var adminJuan = new AppUser
            {
                Email = "juan@nauta.cu",
                UserName = "juan@nauta.cu"
            };
            var adminPepe = new AppUser
            {
                Email = "pepe@nauta.cu",
                UserName = "pepe@nauta.cu"
            };

            var usuario = new AppUser
            {
                Email = "usuario@nauta.cu",
                UserName = "usuario@nauta.cu"
            };
            var usuarioManolo = new AppUser
            {
                Email = "manolou@nauta.cu",
                UserName = "manolou@nauta.cu"
            };
            var usuarioChicho = new AppUser
            {
                Email = "chichou@nauta.cu",
                UserName = "chichou@nauta.cu"
            };

            var userManager = new ApplicationUserManager(new UserStore<AppUser>(db));
            userManager.Create(admin, "Admin123.");
            userManager.Create(adminPepe, "Pepe123.");
            userManager.Create(adminJuan, "Juan123.");

            userManager.Create(usuario, "Usuario123.");
            userManager.Create(usuarioManolo, "Manolou123.");
            userManager.Create(usuarioChicho, "Chichou123.");


            userManager.AddToRoles(admin.Id, roleAdmin.Name);
            userManager.AddToRoles(adminPepe.Id, roleAdmin.Name);
            userManager.AddToRoles(adminJuan.Id, roleAdmin.Name);

            userManager.AddToRole(usuario.Id, roleUsuario.Name);
            userManager.AddToRole(usuarioManolo.Id, roleUsuario.Name);
            userManager.AddToRole(usuarioChicho.Id, roleUsuario.Name);
        }


        //********************************************//
        //Genera el listado de trabajadores
        private List<Trabajador> GenerarTrabajadores(int cantidad = 30)
        {
            var trabajadoresFaker = new Faker<Trabajador>();
            trabajadoresFaker.Locale = "es";

            trabajadoresFaker.Rules((f, c) =>
            {
                c.Nombre = f.Person.FullName;
            });

            return trabajadoresFaker.Generate(cantidad);
        }

        //Genera el listado de actividades
        private List<Actividad> GenerarActividades(int cantidad = 30)
        {
            var faker = new Faker();
            var tareas = new[] {
                new Tarea{Nombre= "Tarea 1" },
                new Tarea{Nombre= "Tarea # 2" },
                new Tarea{Nombre= "Tarea no 3" },
                new Tarea{Nombre= "Tarea otra 4" },
                new Tarea{Nombre= "Tarea mas 5" },
                new Tarea{Nombre= "Tarea numero 6" },
                new Tarea{Nombre= "Tarea noo 7" },
                new Tarea{Nombre= "Tarea NO 8" },
                new Tarea{Nombre= "Tarea otra mas 9" },
                new Tarea{Nombre= "Tarea ### 10" },
            };



            var actividadesFaker = new Faker<Actividad>();
            actividadesFaker.Locale = "es";

            actividadesFaker.Rules((f, a) =>
            {

                a.Titulo = f.Random.Word();
                a.Descripcion = f.Random.Words();
                a.Estado = f.PickRandom<EstadosActividad>();
                a.FechaRegistro = f.Date.Between(DateTime.Today.AddDays(-20), DateTime.Today.AddDays(-1));
                a.Tareas = new List<Tarea>();

                int cantTareasIni = faker.Random.Int(0, tareas.Length - 2);
                int cantTareasFin = faker.Random.Int(cantTareasIni, tareas.Length - 1);
                var porcientos = arrPorcientoTareaDeActividade(cantTareasFin - cantTareasIni);

                int k = 0;
                for (int i = cantTareasIni; i < cantTareasFin; i++)
                {
                    Tarea t = new Tarea { Nombre = tareas[i].Nombre, Porcentaje = porcientos[k++] };
                    a.Tareas.Add(t);
                }

            });

            return actividadesFaker.Generate(cantidad);
        }


        /// <summary>
        /// Genera un arreglo con los porcientos q se le debe de asignar a cada tarea
        /// para q su suma de 100%
        /// </summary>
        /// <param name="cantTareaAct"> Cantdad de tareas de la actividad</param>
        /// <returns></returns>
        private int[] arrPorcientoTareaDeActividade(int cantTareaAct)
        {
            int d = cantTareaAct;
            var porcentages = new int[cantTareaAct];
            int sumatoria = 0;


            for (int i = 0; i < cantTareaAct; i++)
            {
                var random = new Random();
                if (i == (cantTareaAct - 1))
                {
                    porcentages[i] = 100 - sumatoria;
                }
                else
                {
                    int div = (100 - sumatoria) / d;
                    int valor = random.Next(1, div);
                    sumatoria += valor;
                    porcentages[i] = valor;
                    d--;
                }
            }
            return porcentages;
        }








        private void SalvarCategorias()
        {
            db.Categorias.AddRange(GenerarCategorias());
            db.SaveChanges();
        }

        private void SalvarResponsables()
        {
            db.Responsables.AddRange(GenerarResponsables());
            db.SaveChanges();
        }

        private void SalvarActivos()
        {
            db.Activos.AddRange(GenerarActivos());
            db.SaveChanges();
        }

        //Genera el listado de categorias
        private List<Categoria> GenerarCategorias()
        {
            var categorias = new[] {
                "Servidores Fisicos",
                "Servidores Logicos",
                "Routers Primario",
                "Routers Secundarios",
                "Aplicativos CORE",
                "Aplicativos Seguridad",
                "Aplicativos TI",
                "Procesos",
                "Switch",
                "Enlace"
            };

            var faker = new Faker("es");
            List<Categoria> list = new List<Categoria>(categorias.Length);

            foreach (var cat in categorias)
            {
                list.Add(new Categoria { Nombre = cat, Valor = faker.Random.Decimal(1, 1000) });
            }
            return list;

            //var categorias = new[] { "", "" };
            //var categoriasFaker = new Faker<Categoria>();
            //categoriasFaker.Locale = "es";
            //categoriasFaker.Rules((f, c)=> {
            //        c.Nombre = 
            //});
            //categoriasFaker
            //    .RuleFor(c => c.Nombre, f => f.PickRandom(categorias))
            //    .RuleFor(c => c.Valor, f => f.Random.Decimal(1, 1000));

            //return categoriasFaker.Generate(cantidad);
        }

        //Genera el listado de responsables
        private List<Responsable> GenerarResponsables(int cantidad = 30)
        {
            var responsablesFaker = new Faker<Responsable>();
            responsablesFaker.Locale = "es";

            responsablesFaker.Rules((f, c) =>
            {
                c.Nombre = f.Person.FullName;
                c.Email = f.Person.Email;
            });

            return responsablesFaker.Generate(cantidad);
        }

        //Genera el listado de activos
        private List<Activo> GenerarActivos(int cantidad = 30)
        {
            var categorias = db.Categorias.ToArray();
            int categSize = categorias.Length - 1;

            var responsables = db.Responsables.ToArray();
            int respSize = categorias.Length - 1;

            var activosFaker = new Faker<Activo>();
            activosFaker.Locale = "es";

            activosFaker.Rules((f, c) =>
            {
                c.Nombre = f.Random.Words(3);
                c.EsPrincipal = f.Random.Bool();
                c.FechaAlta = f.Date.Between(DateTime.Today.AddDays(-20), DateTime.Today.AddDays(-10));
                c.Unidades = f.Random.Int(1, 100);
                if (f.Random.Bool())
                {
                    c.FechaBaja = DateTime.Today.AddDays(-f.Random.Int(0, 9));
                }
                c.CategoriaId = categorias[f.Random.Int(0, categSize)].Id;
                c.ResponsableId = responsables[f.Random.Int(0, respSize)].Id;

            });

            return activosFaker.Generate(cantidad);
        }
    }
}