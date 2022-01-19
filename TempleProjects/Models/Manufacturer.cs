namespace TempleProjects.Models
{
    public class Manufacturer
    {
        public int Count { get; set; }
        public string Message { get; set; }
        public string SearchCriteria { get; set; }
        public Result[] Results { get; set; }
    }

        public class Result
        {
            public int Make_ID { get; set; }
            public string Make_Name { get; set; }
            public string Mfr_Name { get; set; }
        }

    
}
