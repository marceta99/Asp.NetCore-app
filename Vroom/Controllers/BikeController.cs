using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vroom.Models;
using Vroom.Models.ViewModel;
using System.IO;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Authorization;

namespace Vroom.Controllers
{
    [Authorize(Roles ="Admin,Executive")]
    public class BikeController : Controller
    {
        private readonly VroomDbContext _db;
        private readonly HostingEnvironment _hostingEnvironment; 

        public BikeController(VroomDbContext db,HostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment; 
        }

        [AllowAnonymous]
        public IActionResult Index(string searchString,int pageNumber = 1 , int pageSize =2)
        {
          
            int ExcludeRecords = (pageSize * pageNumber) - pageSize;

            var Bikes = _db.Bikes.Include(m => m.Make).Include(m => m.Model)
                .Skip(ExcludeRecords).Take(pageSize);
           
            if (!string.IsNullOrEmpty(searchString))
            {
                Bikes = Bikes.Where(b => b.Make.Name.Contains(searchString) || b.Model.Name.Contains(searchString)); 
            }


            return View(Bikes);
        }
        [HttpGet]
        public IActionResult Create()
        {
            BikeViewModel BVM = new BikeViewModel()
            {
                Makes = _db.Makes.ToList(),
                Models = _db.Models.ToList(),
                Bike = new Bike()
            };

            ViewBag.Currencies = new string[]{ "USD","EUR","DIN"}; 

            return View(BVM); 
        }

        [HttpPost]
        public IActionResult Create(BikeViewModel Bvm)
        {
          
            List<Make> allMakes = _db.Makes.ToList();
            List<Model> allModels = _db.Models.ToList();
            Make pomocnaMarka = new Make();
            Model pomocniModel = new Model();
           
            if (allMakes.Count > 0 && allModels.Count > 0)
            {
                for (int i = 0; i < allMakes.Count; i++)
                {
                    if (allMakes[i].Id == Bvm.Bike.MakeID)
                    {
                        pomocnaMarka = allMakes[i];
                    }
                }
                for (int i = 0; i < allModels.Count; i++)
                {
                    if (allModels[i].Id == Bvm.Bike.ModelID)
                    {
                        pomocniModel = allModels[i];
                    }
                }
                Bike newBike = Bvm.Bike;
                var BikesModel =_db.Models.Find(Bvm.Bike.ModelID);

                newBike.Model = BikesModel; 

               // if (ModelState.IsValid)
                //{
                    _db.Bikes.Add(newBike);
                    _db.SaveChanges();

                    //bikeId koji smo sacuvali na serveru
                    var BikeId = newBike.Id;

                    //uzimamo wwwrooth path da bi sacuvali img fajl na serveru
                    string wwwrootPath = _hostingEnvironment.WebRootPath;

                    //uzimamo fajl tj sliku koja je uplodovana preko forme 
                    var files = HttpContext.Request.Form.Files;

                    //uzimamo referencu od DbSet za bike koji smo upravo sacuvali u bazi
                    var SavedBike = _db.Bikes.Find(BikeId);

                    if (files.Count != 0)
                    {
                        var imagePath = @"images\bike\";
                        var Extension = Path.GetExtension(files[0].FileName);
                        var RealtiveImagePath = imagePath + BikeId + Extension;
                        var AbsouluteImagePath = Path.Combine(wwwrootPath, RealtiveImagePath);


                        //uplodujemo file na server
                        using (var fileStream = new FileStream(AbsouluteImagePath, FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        SavedBike.ImagePath = RealtiveImagePath;
                        newBike = SavedBike;
                        _db.Bikes.Update(SavedBike);

                        _db.SaveChanges();


                        return RedirectToAction("Index", "Bike");
                    }

                //}
                //return Create();
                
               
            }
            return Create();

            //  }
            //  return Create();
            // } return View(ModelVM); 
        }

        public IActionResult Delete(int id)
        {
            Bike bike = _db.Bikes.Find(id);  
            if(bike == null)
            {
                return NotFound(); 
            }
            _db.Bikes.Remove(bike);
            _db.SaveChanges();
            return RedirectToAction("Index", "Bike"); 
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {

            BikeViewModel Bvm = new BikeViewModel()
            {
                Makes = _db.Makes.ToList(),
                Models = _db.Models.ToList(),
                Bike = _db.Bikes.Find(id)
            };
            ViewBag.Currencies = new string[] { "USD", "EUR", "DIN" };

            Bvm.Models = _db.Models.Where(m => m.MakeId == Bvm.Bike.MakeID); 

            if (Bvm.Bike == null)
            {
                return NotFound(); 
            }
            return View(Bvm); 
        }

        [HttpPost]
        public IActionResult Edit(BikeViewModel Bvm)
        {
            Bike newBike = Bvm.Bike;
            var BikesModel = _db.Models.Find(Bvm.Bike.ModelID);

            newBike.Model = BikesModel;

            _db.Bikes.Update(newBike);
            _db.SaveChanges();

            //bikeId koji smo sacuvali na serveru
            var BikeId = newBike.Id;

            //uzimamo wwwrooth path da bi sacuvali img fajl na serveru
            string wwwrootPath = _hostingEnvironment.WebRootPath;

            //uzimamo fajl tj sliku koja je uplodovana preko forme 
            var files = HttpContext.Request.Form.Files;

            //uzimamo referencu od DbSet za bike koji smo upravo sacuvali u bazi
            var SavedBike = _db.Bikes.Find(BikeId);

            if (files.Count != 0)
            {
                var imagePath = @"images\bike\";
                var Extension = Path.GetExtension(files[0].FileName);
                var RealtiveImagePath = imagePath + BikeId + Extension;
                var AbsouluteImagePath = Path.Combine(wwwrootPath, RealtiveImagePath);


                //uplodujemo file na server
                using (var fileStream = new FileStream(AbsouluteImagePath, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                SavedBike.ImagePath = RealtiveImagePath;
                newBike = SavedBike;
                _db.Bikes.Update(newBike);

                _db.SaveChanges();


                return RedirectToAction("Index", "Bike");
            }
            return RedirectToAction("Index", "Bike");


        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var bike = _db.Bikes.Find(id);

            if (bike != null)
            {
                return View(bike);
            }
            return NotFound(); 
        }




    }
}
