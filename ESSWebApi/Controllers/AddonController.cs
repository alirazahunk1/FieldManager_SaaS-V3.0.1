using CZ.Api.Base;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace ESSWebApi.Controllers
{
    public class AddonController : BaseController
    {
        [HttpGet("getAssemblies")]
        public IActionResult Index()
        {
            List<Module> modules = new List<Module>();

            var assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies();

            foreach (var assembly in assemblies.Where(x => x.Name.Contains("CZ.")))
            {
                var module = new Module
                {
                    Name = assembly.Name,
                    Version = assembly.Version.ToString()
                };

                modules.Add(module);
            }

            return Ok(modules);
        }


        class Module
        {
            public string Name { get; set; }

            public string Version { get; set; }
        }
    }
}
