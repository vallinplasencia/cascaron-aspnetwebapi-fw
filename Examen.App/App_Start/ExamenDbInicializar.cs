using Bogus;
using Examen.AccesoDatos.Context;
using Examen.App.Util.Seguridad;
using Examen.Dominio.Entidades;
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

            SalvarCategorias();
            SalvarResponsables();
            SalvarActivos();

            SalvarUsuarios();

            base.Seed(context);
        }

        private void SalvarUsuarios()
        {
            //Salvando roles de usuarios
            var roleSuperAdmin = new AppRole { Name = TiposRole.SuperAdmin };
            var roleAdmin = new AppRole { Name = TiposRole.Admin };
            var roleUsuario = new AppRole { Name = TiposRole.Usuario };

            var roleManager = new ApplicationRoleManager(new RoleStore<AppRole>(db));
            roleManager.Create(roleSuperAdmin);
            roleManager.Create(roleAdmin);
            roleManager.Create(roleUsuario);


            //Salvando los usuarios iniciales de la app
            var superAdmin = new AppUser { Email = "superadmin@nauta.cu", UserName = "superadmin" };
            var admin = new AppUser { Email = "admin@nauta.cu", UserName = "admin" };
            var usuario = new AppUser { Email = "usuario@nauta.cu", UserName = "usuario" };

            var userManager = new ApplicationUserManager(new UserStore<AppUser>(db));
            userManager.Create(superAdmin, "Superadmin123.");
            userManager.Create(admin, "Admin123.");
            userManager.Create(usuario, "Usuario123.");

            //userManager.AddToRole(superAdmin.Id, roleSuperAdmin.Name);
            userManager.AddToRoles(superAdmin.Id, roleSuperAdmin.Name, roleAdmin.Name, roleUsuario.Name);
            userManager.AddToRoles(admin.Id, roleAdmin.Name, roleUsuario.Name);
            userManager.AddToRole(usuario.Id, roleUsuario.Name);
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