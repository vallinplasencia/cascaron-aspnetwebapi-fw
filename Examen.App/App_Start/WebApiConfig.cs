using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Examen.App.Util;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace Examen.App
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Resolver para la inyeccion de dependencia
            config.DependencyResolver = new UnityResolver();

            //Establesco q las respuestas json esten en formato CamelCase.
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();





            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
