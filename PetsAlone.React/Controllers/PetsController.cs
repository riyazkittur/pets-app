using Microsoft.AspNetCore.Mvc;
using PetsAlone.Core;

namespace PetsAlone.React.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetsController : ControllerBase
    {
        private readonly PetsDbContext _petsDbContext;
        public PetsController(PetsDbContext petsDbContext)
        {
            _petsDbContext = petsDbContext;
        }

        [HttpGet("all")]
        public object GetAll()
        {
            var petsService = new PetsService();
            var result = petsService.GetAll(_petsDbContext);
            return result;
        }
    }
}