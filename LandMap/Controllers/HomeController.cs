using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LandMap.Data;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Models;
using System.Diagnostics;

namespace LandMap.Controllers
{
    public class HomeController : Controller
    {
        private static bool firstTime = true;
        private readonly LandDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(LandDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Lands
        public async Task<IActionResult> Index()
        {
            //Debug.Assert(firstTime);
            firstTime = false;

            ViewData["PopupTitle"] = "Карточка земельного участка";

            ViewBag.LandRightTypes = _context.LandRightType.Select(t => new { t.Id, t.Name }).ToList();
            ViewBag.LandTypes = _context.LandType.Select(t => new { t.Id, t.Name }).ToList();

            return View(new Models.LandModel
            {
                Lands = await GetLands(),
                Selected = new Models.Land()
            });
        }


        [HttpGet]
        public async Task<IActionResult> Lands()
        {

            var model = await GetLands();
            return PartialView("_LandsPartial", new Models.LandModel
            {
                Lands = await GetLands(),
                Selected = new Models.Land()
            });

        }


        private async Task<List<Models.Land>> GetLands()
        {

            var landsList = await _context
                .Land.Select(l => 
                    new Models.Land(l)).ToListAsync();

            var coords = landsList.Where(l => !String.IsNullOrEmpty(l.Coordinates))
                .Select(l => new Models.GeoObject
                {
                    Coords = JsonConvert.DeserializeObject<double[][][]>(l.Coordinates),
                    Id = l.Id,
                    Name = l.Name
                }).ToArray();

            landsList.Where(l => !String.IsNullOrEmpty(l.Coordinates)).ToList().ForEach(l =>
            {
                double[][][] coordsArray = JsonConvert.DeserializeObject<double[][][]>(l.Coordinates);
                var center = Geometry.AverageCenter(coordsArray);
                l.Center = Helper.DoubleToJson(center);
                l.LandTypeName = GetLandType(l.LandTypeId);
                l.LandRightTypeName = GetLandRightType(l.LandRightTypeId);
            });

            var script =
                System.IO.File.ReadAllText(
                    Path.Combine(_hostingEnvironment.WebRootPath, "js\\polygonViewer.js"), System.Text.Encoding.UTF8);

            string s = JsonConvert.SerializeObject(coords);
            ViewData["script"] = String.Format(@"var coords = {0}; {1}", s, script);

            return landsList;
        }

        protected string GetLandType(int? landTypeId)
        {
            return _context.LandType.FirstOrDefault(t => t.Id == landTypeId).Name;
        }

        protected string GetLandRightType(int? landRightTypeId)
        {
            return _context.LandRightType.FirstOrDefault(t => t.Id == landRightTypeId).Name;
        }

        // GET: Lands/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var land = await _context.Land
                .Include(l => l.LandRightType)
                .Include(l => l.LandType)
                .Select(l => new Models.Land(l))
                .FirstOrDefaultAsync(m => m.Id == id);

            if (land == null)
            {
                return NotFound();
            }

            return PartialView("_EditorPartial", new Models.LandModel
            {
                Lands = await GetLands(),
                Selected = land
            });

        }

        [HttpPost]
        public async Task<IActionResult> _EditorPartial(Models.LandModel landModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Models.Land land = landModel.Selected;

            try

            {
                if (land.Id == 0)
                {
                    _context.Land.Add(new Land
                    {
                        Name = land.Name,
                        CadastralNumber = land.CadastralNumber,
                        InventoryNumber = land.InventoryNumber,
                        LandRightTypeId = land.LandRightTypeId,
                        LandTypeId = land.LandTypeId,
                        Coordinates = land.Coordinates,
                    });
                }
                else
                {
                    Land dataLand = await _context.Land.FirstOrDefaultAsync(l => l.Id == land.Id);
                    dataLand.CadastralNumber = land.CadastralNumber;
                    dataLand.InventoryNumber = land.InventoryNumber;
                    dataLand.LandRightTypeId = land.LandRightTypeId;
                    dataLand.LandTypeId = land.LandTypeId;
                    dataLand.Name = land.Name;
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)

            {

            }

            Models.LandModel model = new Models.LandModel
            {
                Lands = await GetLands(),
                Selected = land
            };

            return PartialView("_LandsPartial", model);

        }

        // GET: Lands/Create
        public IActionResult Create()
        {
            ViewData["LandRightTypeId"] = new SelectList(_context.LandRightType, "Id", "Name");
            ViewData["LandTypeId"] = new SelectList(_context.LandType, "Id", "Name");
            return View();
        }

        // POST: Lands/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,InventoryNumber,CadastralNumber,LandTypeId,LandRightTypeId,Coordinates")] Land land)
        {
            if (ModelState.IsValid)
            {
                _context.Add(land);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LandRightTypeId"] = new SelectList(_context.LandRightType, "Id", "Name", land.LandRightTypeId);
            ViewData["LandTypeId"] = new SelectList(_context.LandType, "Id", "Name", land.LandTypeId);
            return View(land);
        }

        // GET: Lands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var land = await _context.Land.FindAsync(id);
            if (land == null)
            {
                return NotFound();
            }
            ViewData["LandRightTypeId"] = new SelectList(_context.LandRightType, "Id", "Name", land.LandRightTypeId);
            ViewData["LandTypeId"] = new SelectList(_context.LandType, "Id", "Name", land.LandTypeId);
            return View(land);
        }

        // POST: Lands/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,InventoryNumber,CadastralNumber,LandTypeId,LandRightTypeId,Coordinates")] Land land)
        {
            if (id != land.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(land);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LandExists(land.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["LandRightTypeId"] = new SelectList(_context.LandRightType, "Id", "Name", land.LandRightTypeId);
            ViewData["LandTypeId"] = new SelectList(_context.LandType, "Id", "Name", land.LandTypeId);
            return View(land);
        }

        // GET: Lands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var land = await _context.Land
                .Include(l => l.LandRightType)
                .Include(l => l.LandType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (land == null)
            {
                return NotFound();
            }

            return View(land);
        }

        // POST: Lands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var land = await _context.Land.FindAsync(id);
            _context.Land.Remove(land);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LandExists(int id)
        {
            return _context.Land.Any(e => e.Id == id);
        }
    }

}
