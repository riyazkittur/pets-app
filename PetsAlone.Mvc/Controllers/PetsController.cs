using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PetsAlone.Core;
using PetsAlone.Mvc.Models;

namespace PetsAlone.Mvc.Controllers
{
    public class PetsController : Controller
    {
        private readonly PetsDbContext _petsDbContext;
        public PetsController(PetsDbContext petsDbContext)
        {
            _petsDbContext = petsDbContext;
        }
        
        public IActionResult Index()
        {
            var service = new PetsService();
            var result = service.GetAll(_petsDbContext);
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}