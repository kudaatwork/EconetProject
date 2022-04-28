using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EconetProject.Models
{
    public class ShopAreaModel
    {
        public string ShopName { get; set; }
        public string AreaName { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public int ShopId { get; set; }
        public int AreaId { get; set; }
        public List<Shop> Shops { get; set; }
    }
}