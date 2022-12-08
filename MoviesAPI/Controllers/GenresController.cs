using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entitties;
using MoviesAPI.Filters;
using MoviesAPI.Helpers;

namespace MoviesAPI.Controllers
{
    [Route("api/genres")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public class GenresController: ControllerBase
    {
        private readonly ILogger<GenresController> logger;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GenresController(ILogger<GenresController> logger, 
            ApplicationDbContext context,
            IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet] // ==> api/genres
        public async Task<ActionResult<List<GenreDTO>>> Get([FromQuery] PaginationDTO paginationsDTO)
        {
            var queryble = context.Genres.AsQueryable();
            await HttpContext.InsertParametersPaginationHeader(queryble);
            var genres = await queryble.OrderBy(x => x.Name).Paginate(paginationsDTO).ToListAsync();
            return mapper.Map<List<GenreDTO>>(genres);
        }

        [HttpGet("all")] // ==> api/genres
        [AllowAnonymous]
        public async Task<ActionResult<List<GenreDTO>>> Get()
        {
            var genres = await context.Genres.OrderBy(x => x.Name).ToListAsync();
            return mapper.Map<List<GenreDTO>>(genres);
        }

        [HttpGet("{Id:int}", Name = "getGenre")]//, Name = "getGenre"
        public async Task<ActionResult<GenreDTO>> Get(int Id) 
        {
            var genre = await context.Genres.FirstOrDefaultAsync(x => x.Id == Id);

            if(genre == null)
            {
                return NotFound();
            }

            return mapper.Map<GenreDTO>(genre);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GenreCreationDTO genreCreationDTO)
        {
            var genre = mapper.Map<Genre>(genreCreationDTO);
            //context.Genres.Add(genre);
            context.Add(genre); //we can ommit the "Genres" from "context.Genres.Add(genre)" because the entityframework is smart enough to know that if we use genre of type Genre so it belong to table Genres
                        
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] GenreCreationDTO genreCreationDTO)
        {
            var genre = await context.Genres.FirstOrDefaultAsync(x => x.Id == id);
            if(genre == null)
            {
                return NotFound();
            }

            genre = mapper.Map(genreCreationDTO, genre);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await context.Genres.AnyAsync(x => x.Id == id);
            if (!exists)
                return NotFound();

            context.Remove(new Genre() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
