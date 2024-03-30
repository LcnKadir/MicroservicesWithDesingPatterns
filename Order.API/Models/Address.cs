using Microsoft.EntityFrameworkCore;

namespace Order.API.Models
{
    [Owned] //Adress class will be inside the Order Table. //Adress class'ı Order Tablosunun içinde yer alacak.
    public class Address
    {
        public string Line { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
    }
}
