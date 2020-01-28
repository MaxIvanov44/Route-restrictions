using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Threading.Tasks;

namespace Route_restrictions
{
    public class PositionConstraint : IRouteConstraint
    {
        string[] positions = new[] { "admin", "director", "accountant" };
        public bool Match(HttpContext httpContext, IRouter route, string routeKey,
        RouteValueDictionary values, RouteDirection routeDirection)
        {
            return positions.Contains(values[routeKey]?.ToString().ToLowerInvariant());
        }
    }
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RouteOptions>(options =>
                options.ConstraintMap.Add("position", typeof(PositionConstraint)));
        }
        //public void Configure(IApplicationBuilder app)
        //{
        //    var myRouteHandler = new RouteHandler(async context =>
        //    {
        //        await context.Response.WriteAsync("Routing content");
        //    });

        //    var routeBuilder = new RouteBuilder(app, myRouteHandler);
        //    //routeBuilder.MapRoute
        //    //(
        //    //    name: "default",
        //    //    template: "{controller}/{action}/{id?}",
        //    //    defaults: new { controller = "Home", action = "Index" },
        //    //    constraints: new
        //    //    {
        //    //      action = new CompositeRouteConstraint(new IRouteConstraint[]
        //    //      {
        //    //          new AlphaRouteConstraint(),
        //    //          new LengthRouteConstraint(1,10),
        //    //          new RequiredRouteConstraint(),

        //    //      })
        //    //    }
        //    //);

        //    //routeBuilder.MapRoute("default", "{controller:length(4)}/{action:alpha}/{id:range(4,100)}");

        //    //routeBuilder.MapRoute(
        //    //    name: "default",
        //    //    template: "{controller}/{action:alpha:minlength(5)}/{id?}",
        //    //    defaults: new { controller = "Home", action = "Index" }
        //    //    );
        //    routeBuilder.MapRoute(
        //        "default",
        //        "{controller}/{action}/{id}",
        //        new { controller = "Home", action = "Index" },
        //        new { id = new PositionConstraint() }
        //          );

        //    app.UseRouter(routeBuilder.Build());

        //    app.Run(async (context) =>
        //    {
        //        await context.Response.WriteAsync("Standard content");
        //    });

        //}

        public void Configure(IApplicationBuilder app)
        {
            var myRouteHandler = new RouteHandler(HandleAsync);

            var routeBuilder = new RouteBuilder(app, myRouteHandler);

            routeBuilder.MapRoute(
                "default",
                "{controller}/{action}/{id:position?}");

            app.UseRouter(routeBuilder.Build());

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Standard content");
            });
        }

        private async Task HandleAsync(HttpContext context)
        {
            var routeValues = context.GetRouteData().Values;
            var action = routeValues["action"].ToString();
            var controller = routeValues["controller"].ToString();
            string id = routeValues["id"]?.ToString();
            await context.Response.WriteAsync($"controller: {controller} | action: {action} | id: {id}");
        }
    }
}
