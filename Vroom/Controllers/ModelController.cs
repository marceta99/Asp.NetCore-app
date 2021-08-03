using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vroom.Models;
using Vroom.Models.ViewModel;

namespace Vroom.Controllers
{
    [Authorize(Roles = "Admin,Executive")]

    public class ModelController : Controller
    {
        private readonly VroomDbContext _db;

        // [BindProperty]
        //public static ModelViewModel ModelVM { get; set; }
        public static ModelViewModel modelVM { get; set; }



        public ModelController(VroomDbContext db)
        {
            _db = db;
            /*   ModelVM = new ModelViewModel
               {
                   Makes = _db.Makes.ToList(),


               };*/
        }
        public IActionResult Index()
        {
            var model = _db.Models.Include(m => m.Make);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            /* ViewBag.dbMakes = _db.Makes.Select(m => new SelectListItem()
             {

             }  ); */
            modelVM = new ModelViewModel()
            {
                Makes = _db.Makes.ToList(),
                Id = 0,
                Name = "pocetno ime"
            };
            return View(modelVM);
        }
        [HttpPost, ActionName("Create")]
        public IActionResult CreatePost(ModelViewModel mm)
        {


            if (ModelState.IsValid)
            {

                List<Make> allMakes = _db.Makes.ToList();
                Make pomocnaMarka = new Make();

                if (allMakes.Count > 0)
                {
                    for (int i = 0; i < allMakes.Count; i++)
                    {
                        if (allMakes[i].Id == mm.Id)
                        {
                            pomocnaMarka = allMakes[i];
                        }
                    }

                    var newModel = new Model
                    {
                        Name = mm.Name,
                        Make = pomocnaMarka,
                        MakeId = pomocnaMarka.Id
                    };


                    _db.Models.Add(newModel);
                    _db.SaveChanges();

                    return RedirectToAction("Index", "Model");
                }
                return Create();
            }
            return Create(); 
        }



        [HttpGet]
        public IActionResult Edit(int id)
        {
            Model trenutniModel = _db.Models.Include(m => m.Make).SingleOrDefault(m => m.Id == id);
       
            if (trenutniModel == null)
            {
                return NotFound();
            }
            ModelViewModel MvmModel = new ModelViewModel()
            {
                Makes = _db.Makes.ToList(),
                Id = trenutniModel.Id ,
                Name = trenutniModel.Name,
                ModelId = id
            };
            ViewBag.IdModel = id; 
            return View(MvmModel);
        }

        [HttpPost]
        public IActionResult Edit(ModelViewModel Mvm)
        {
            var stariModel = _db.Models.Find(Mvm.ModelId);
            var novaMarka = _db.Makes.Find(Mvm.Id);

            if (stariModel != null)
            {
                var noviModel = new Model()
                {
                    Id = stariModel.Id,
                    Name = Mvm.Name,
                    Make = novaMarka,
                    MakeId = novaMarka.Id
                };


                if (ModelState.IsValid)
                {
                    _db.Models.Remove(stariModel);

                    _db.Models.Update(noviModel);
                    _db.SaveChanges();

                    return RedirectToAction("Index", "Model");
                }
                return View(Mvm);

            }
            else
            {
                return NotFound(); 
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {

            Model model = _db.Models.Find(id);

            if (model != null)
            {
                _db.Models.Remove(model);
                _db.SaveChanges();

                return RedirectToAction("Index", "Model");
            }
            return NotFound();
        }
    
        [AllowAnonymous]
        [HttpGet("api/models/{MakeId}")]
        public IEnumerable<Model> Models(int MakeId)
        {
            return _db.Models.ToList().Where(m=>m.MakeId == MakeId) ; 
        }

    } 
}
