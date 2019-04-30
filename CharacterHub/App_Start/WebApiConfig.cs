using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CharacterHub
{
    public static class WebApiConfig
    {
		public static void Register(HttpConfiguration config)
		{
            var jsonFormatter = config.Formatters.JsonFormatter;
            config.MapHttpAttributeRoutes();
			jsonFormatter.SupportedMediaTypes
				.Add(new MediaTypeHeaderValue("text/html"));
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            jsonFormatter.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
            config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
            // Enable CORS
            var cors = new EnableCorsAttribute("http://localhost:4200", "*", "*");
            config.EnableCors(cors);
		}
	}
}
