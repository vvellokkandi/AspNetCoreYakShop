using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


using YakShop.Common.Models;

namespace YakShop.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var items = new HerdList()
            //{
            //    Herds = new List<LabYak>() {
            //        new LabYak() {
            //            Name = "Betty-1",
            //            Age = 4,
            //            Sex = "f"
            //        },
            //        new LabYak() {
            //            Name = "Betty-2",
            //            Age = 8,
            //            Sex = "f"
            //        }
            //    }
            //};

            //XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            //ns.Add("", "");

            //string fileName = "test.txt";
            //TextWriter writer = new StreamWriter(fileName);
            //XmlSerializer ser = new XmlSerializer(typeof(HerdList));
            //ser.Serialize(writer, items, ns);  

            //CreateWebHostBuilder(args);

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", optional: true)
                .Build();

            var host = WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseConfiguration(config)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }

        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) )
        //{

        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>()
        //        .UseKestrel(options =>
        //        {
        //    // Set properties and call methods on options
        //        }).;
        //}
    }
}
