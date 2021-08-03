using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vroom.Models;

namespace Vroom.Controllers
{
    [Authorize(Roles ="Admin,Executive")]
    public class MakeController : Controller
    {
        private readonly VroomDbContext _db;

        public MakeController(VroomDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.Makes.ToList()); 
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(); 
        }
        [HttpPost]
        public IActionResult Create(Make make)
        {
            if (ModelState.IsValid)
            {
                _db.Makes.Add(make);
                _db.SaveChanges();

                return RedirectToAction("Index", "Make");
            }
            return View(make); 
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var make = _db.Makes.Find(id);
            
            if(make == null)
            {
                return NotFound(); 
            }
            _db.Makes.Remove(make);
            _db.SaveChanges();
            return RedirectToAction("Index","Make"); 

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var make = _db.Makes.Find(id); 

            if(make == null)
            {
                return NotFound(); 
            }
            return View(make); 
        }

        [HttpPost]
        public IActionResult Edit(Make make)
        {
            var staraMarka = _db.Makes.Find(make.Id);
            if (ModelState.IsValid)
            {
                _db.Makes.Remove(staraMarka);//ovo sam ja nesto improvizovao jer nije htelo da mi radi update pa sam prvo obirsao onaj stari model i
                                          //  posle samo ubacio taj novi na istom Id-u tako da sam u sustini postigao istu stvar 
                _db.Update(make);
                _db.SaveChanges();
                return RedirectToAction("Index", "Make"); 
            }
            return View(make); 
        }

    }
}
