using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TempleProjects.Models;
using System.Net.Http.Json;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace TempleProjects.Controllers
{
    public class HomeController : Controller
    {
        protected readonly IConfiguration _config;
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();

      
     
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }


        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            ConnectionString = _config.GetConnectionString("DefaultConnectionString");
            ProviderName = "System.Data.SqlClient";
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async System.Threading.Tasks.Task<IActionResult> VehiclList()
        {
            VehicleMakes vehicles = new VehicleMakes();
           
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://parallelum.com.br/fipe/api/v1/carros/marcas/59/modelos"))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    vehicles = JsonConvert.DeserializeObject<VehicleMakes>(apiResponse);


                }
                //= await httpClient.GetFromJsonAsync<VehicleMakes>("https://parallelum.com.br/fipe/api/v1/carros/marcas/59/modelos");

            }

            return View(vehicles.modelos);

        }


        public async System.Threading.Tasks.Task<IActionResult> ModellList()
        {
            List<Vehicle> cat = new List<Vehicle>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://parallelum.com.br/fipe/api/v1/carros/marcas"))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    cat = JsonConvert.DeserializeObject<List<Vehicle>>(apiResponse);

                    foreach(var item in cat)
                    {
                        item.nome.ToString();
                        item.codigo.ToString();
                        using (SqlConnection con = new SqlConnection(ConnectionString))
                        {
                            con.Open();
                            SqlCommand cmd2 = new SqlCommand("insertvehicles", con);
                            cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd2.Parameters.AddWithValue("@nome", item.nome.ToString());
                            cmd2.Parameters.AddWithValue("@codigo", item.codigo.ToString());
                            cmd2.ExecuteNonQuery();
                        }
                      

                    }


                }
            }
            return View(cat);
        }

        public async System.Threading.Tasks.Task<IActionResult> ManufacturerlList()
        {
            Manufacturer cat = new Manufacturer();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://vpic.nhtsa.dot.gov/api/vehicles/GetMakeForManufacturer/honda?format=json"))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    cat = JsonConvert.DeserializeObject<Manufacturer>(apiResponse);


                }
            }
            return View(cat.Results);
        }


    }
}
