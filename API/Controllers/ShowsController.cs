using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Show;
using Application.Show.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : BaseController
    {
        // GET: api/<ShowsController>
        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<List.ShowsEnvelope>> List(int? limit, int? offset, string sortedRating, 
            string sortedChannel, string sortedGenere, string keywords, string language, 
            string genere, string channel, string day, string time)
        {
            return await Mediator.Send(new List.Query(limit,  offset,  sortedRating,
             sortedChannel,  sortedGenere,  keywords,  language,
             genere,  channel,  day,  time));
        }

        // GET api/<ShowsController>/5
        [HttpGet("{id}")]
        //[Authorize]
        public async Task<ActionResult<ShowDto>> Details(int id)
        {
            return await Mediator.Send(new Details.Query { Id = id });
        }
    }
}
