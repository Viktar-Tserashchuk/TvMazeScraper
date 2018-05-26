using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TvMazeScraper.Core.DataAccess;

namespace TvMazeScraper.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ShowsController : Controller
    {
        private const int PageSize = 10;
        private const int MinPageNumber = 0;
        private readonly IUnitOfWork unitOfWork;

        public ShowsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page)
        {
            if (page < MinPageNumber)
            {
                page = MinPageNumber;
            }
            
            var res = await unitOfWork
                .ShowRepository
                .GetAsync(page * PageSize, PageSize);
            var result = Mapper.Map<IEnumerable<Model.ShowDto>>(res);
            return Ok(result);
        }
    }
}
