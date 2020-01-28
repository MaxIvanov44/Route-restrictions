using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Hosting;

namespace Route_restrictions
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            var myRouteHandler = new RouteHandler(async context =>
            {
                await context.Response.WriteAsync("Routing content");
            });

            var routeBuilder = new RouteBuilder(app, myRouteHandler);
            //routeBuilder.MapRoute
            //(
            //    name: "default",
            //    template: "{controller}/{action}/{id?}",
            //    defaults: new { controller = "Home", action = "Index" },
            //    constraints: new
            //    {
            //      action = new CompositeRouteConstraint(new IRouteConstraint[]
            //      {
            //          new AlphaRouteConstraint(),
            //          new LengthRouteConstraint(1,10),
            //          new RequiredRouteConstraint(),

            //      })
            //    }
            //);

            //routeBuilder.MapRoute("default", "{controller:length(4)}/{action:alpha}/{id:range(4,100)}");

            //routeBuilder.MapRoute(
            //    name: "default",
            //    template: "{controller}/{action:alpha:minlength(5)}/{id?}",
            //    defaults: new { controller = "Home", action = "Index" }
            //    );

            app.UseRouter(routeBuilder.Build());

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Standard content");
            });
        }
    }
}
