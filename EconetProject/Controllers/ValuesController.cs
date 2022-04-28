using EconetProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EconetProject.Controllers
{
    public class ValuesController : ApiController
    {
        EconetProjectEntities db = new EconetProjectEntities();

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        [Route("api/CreateShop")]
        [HttpPost]
        public ShopAreaModel CreateShops(ShopAreaModel shopArea)
        {
            try
            {
                if (shopArea != null)
                {
                    var newArea = db.Areas.Where(x => x.AreaName == shopArea.AreaName).FirstOrDefault();

                    if (newArea != null) // Area in the system
                    {
                        var newlyInsertedArea = db.Areas.Where(x => x.AreaName == newArea.AreaName).LastOrDefault();

                        var newShop = db.Shops.LastOrDefault();

                        newShop.ShopName = shopArea.ShopName;
                        newShop.AreaId = newlyInsertedArea.Id;
                        db.Shops.Add(newShop);
                        db.SaveChanges();

                        shopArea.Status = "Success";
                        shopArea.Message = "Shop Added Successfully in the system";
                        shopArea.ShopId = newShop.Id;
                        shopArea.AreaId = newlyInsertedArea.Id;
                    }
                    else // Area not in the system
                    {   
                        newArea = new Area();

                        newArea.AreaName = shopArea.AreaName;
                        db.Areas.Add(newArea);
                        db.SaveChanges();

                        var newlyInsertedArea = db.Areas.Where(x => x.AreaName == newArea.AreaName).FirstOrDefault();

                        var newShop = db.Shops.Where(x => x.AreaId == newlyInsertedArea.Id).FirstOrDefault();

                        if (newShop != null) // Shop in the system
                        {
                            shopArea.Status = "Failed";
                            shopArea.Message = "Shop slready in the system!";
                        }
                        else // Shop not in the system
                        {
                            newShop = new Shop();

                            newShop.ShopName = shopArea.ShopName;
                            newShop.AreaId = newlyInsertedArea.Id;
                            db.Shops.Add(newShop);
                            db.SaveChanges();

                            shopArea.Status = "Success";
                            shopArea.Message = "Shop successfully added in the System!";
                            shopArea.ShopId = newShop.Id;
                            shopArea.AreaId = newlyInsertedArea.Id;
                        }                       
                    }
                }
                else
                {
                    shopArea.Status = "Failed";
                    shopArea.Message = "Your request came as null!";
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);

                shopArea.Status = "Failed";
                shopArea.Message = "There was a fatal error in creating your shop. See log for Details!";
            }

            return shopArea;
        }

        [Route("api/RetriveShops")]
        [HttpPost]
        public ShopAreaModel RetriveShops(ShopAreaModel shopArea)
        {
            //ShopAreaModel shopArea = new ShopAreaModel();

            try
            {
                if (shopArea.AreaId != 0 || !string.IsNullOrEmpty(shopArea.AreaName))
                {
                    if (shopArea.AreaId > 0)
                    {
                        var shops = db.Shops.Where(x => x.AreaId == shopArea.AreaId).ToList();

                        if (shops != null)
                        {
                            shopArea.Status = "Success";
                            shopArea.Message = "Shops successfully retrieved!";
                            shopArea.Shops = shops;
                        }
                        else
                        {
                            shopArea.Status = "Failed";
                            shopArea.Message = "Could not find Shop Name";
                        }
                    }
                    else
                    {
                        shopArea.Status = "Failed";
                        shopArea.Message = "Could not find Shop Name";
                    }

                    if (!string.IsNullOrEmpty(shopArea.AreaName))
                    {
                        var areaSearched = db.Areas.Where(x => x.AreaName == shopArea.AreaName).FirstOrDefault();

                        if (areaSearched != null)
                        {
                            var shops = db.Shops.Where(x => x.AreaId == areaSearched.Id).ToList();

                            if (shops != null)
                            {
                                shopArea.Status = "Success";
                                shopArea.Message = "Shops successfully retrieved!";
                                shopArea.Shops = shops;
                            }
                            else
                            {
                                shopArea.Status = "Failed";
                                shopArea.Message = "Could not find Shop Names";
                            }
                        }
                        else
                        {
                            shopArea.Status = "Failed";
                            shopArea.Message = "Could not find Area Name";
                        }
                    }
                    else
                    {
                        shopArea.Status = "Failed";
                        shopArea.Message = "Could not find Area Name";
                    }
                }
                else
                {
                    shopArea.Status = "Failed";
                    shopArea.Message = "Did not receive the shop Id or area name";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }            

            return shopArea;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
