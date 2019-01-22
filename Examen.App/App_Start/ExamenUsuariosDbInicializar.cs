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
    public class ExamenUsuariosDbInicializar : DropCreateDatabaseIfModelChanges<AppDbContext>
    {
        private AppDbContext db;
        protected override void Seed(AppDbContext context)
        {
            db = context;

            SalvarUsuarios();

            base.Seed(context);
        }


        //Guarda en la BD a los roles
        private void SalvarUsuarios()
        {
            //Salvando roles de usuarios
            var roleAdmin = new AppRole { Name = TiposRole.Admin };
            var roleUsuario = new AppRole { Name = TiposRole.Usuario };

            var roleManager = new ApplicationRoleManager(new RoleStore<AppRole>(db));
            roleManager.Create(roleAdmin);
            roleManager.Create(roleUsuario);

            //Email de los usuarios de cada trabajador
            var emailsDeUsuarios = new[] {
                "admin@nauta.cu",
                "usuario@nauta.cu",
            };

            var faker = new Faker();
            var trabajadores = new Trabajador[] { new Trabajador { Nombre = "Admin" }, new Trabajador { Nombre = "Usuario" } };
            for (int i = 0; i < emailsDeUsuarios.Length; i++)
            {
                trabajadores[i].User = new AppUser
                {
                    Email = emailsDeUsuarios[i],
                    UserName = emailsDeUsuarios[i]
                };
            }
            trabajadores[0].Jefe = trabajadores[0]; 
            trabajadores[1].Jefe = trabajadores[0]; 
            db.Trabajadores.AddRange(trabajadores);
            db.SaveChanges();

            //Agrgando clave a los usuarios
            var userManager = new ApplicationUserManager(new UserStore<AppUser>(db));
            for (int i = 0; i < trabajadores.Length; i++)
            {
                userManager.AddPassword(trabajadores[i].UserId, "Admin123.");

                var appUser = trabajadores[i].User;
                int idxArrova = appUser.Email.IndexOf("@");
                string usuario = appUser.Email.Substring(0, idxArrova);

                if (usuario == "usuario")
                {
                    userManager.AddToRoles(appUser.Id, roleUsuario.Name);
                }
                else
                {
                    userManager.AddToRoles(appUser.Id, roleAdmin.Name);
                }
            }
        }

    }
}