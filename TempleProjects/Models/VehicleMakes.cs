using System.Collections.Generic;

namespace TempleProjects.Models
{
    public class VehicleMakes
    {
       public Vehicle[] modelos { get; set; }

    }
    public class Vehicle
    {
        public string nome { get; set; }
        public string codigo { get; set; }
    }

}
