using Examen.AccesoDatos.Repositorios;
using Examen.Dominio.Abstracto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Unity;
using Unity.Exceptions;

namespace Examen.App.Util
{
    public class UnityResolver : IDependencyResolver
    {
        protected IUnityContainer container;

        public UnityResolver(IUnityContainer container = null)
        {
            if (container == null)
            {
                this.container = new UnityContainer();
                //throw new ArgumentNullException("container");
            }
            else
            {
                this.container = container;
            }
            this.addBinders();
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new List<object>();
            }
        }

        public IDependencyScope BeginScope()
        {
            var child = container.CreateChildContainer();
            return new UnityResolver(child);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            container.Dispose();
        }

        private void addBinders()
        {
            container.RegisterType<ICategoriaRepo, CategoriaRepo>();
            container.RegisterType<IResponsableRepo, ResponsableRepo>();
            container.RegisterType<IActivoRepo, ActivoRepo>();
            //container.RegisterType<ICategoriaRepo, CategoriaRepo>(new HierarchicalLifetimeManager());
            //container.RegisterInstance(typeof(IXxxRepository), new XxxRepository());
        }
    }
}