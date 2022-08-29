using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResourceMagnat.Data;
using ResourceMagnat.Dto;
using ResourceMagnat.Models;

namespace ResourceMagnat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        private readonly MainDbContext context;
        private readonly SessionDbContext sessions;
        private readonly IMapper mapper;

        public BuildingController(MainDbContext _context, SessionDbContext _sessions, IMapper _mapper)
        {
            context = _context;
            sessions = _sessions;
            mapper = _mapper;
        }

        private int? GetUserIdBySession(string sid)
        {
            var session = sessions.Sessions.FirstOrDefault(i => i.Id == sid);
            return session?.UserId;
        }

        /// <summary>
        /// Возвращает список всех возможных зданий
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BuildingTypeDto>), StatusCodes.Status200OK)]
        public IEnumerable<BuildingTypeDto> List()
        {
            return mapper.Map<IEnumerable<BuildingTypeDto>>(context.BuildingTypes.OrderBy(i => i.Id));
        }

        /// <summary>
        /// Возвращает список зданий для пользователя
        /// </summary>
        /// <param name="sid">Идентификатор сессии</param>
        /// <returns></returns>
        [HttpGet("own/{sid}")]
        [ProducesResponseType(typeof(IEnumerable<BuildingDto>), StatusCodes.Status200OK)]
        public IEnumerable<BuildingDto> Get(string sid)
        {
            var userId = GetUserIdBySession(sid);
            if (userId == null) return new List<BuildingDto>();
            
            var buildings = context.Buildings.Where(i => i.UserId == userId);

            return mapper.Map<IEnumerable<BuildingDto>>(buildings);
        }

        [HttpPost("add/{sid}")]
        [ProducesResponseType(typeof(IEnumerable<BuildingDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Add(string sid, BuildingDto buildingDto)
        {
            var userId = GetUserIdBySession(sid);
            if (userId == null) return NotFound("Session not found");

            var existBuilding = context.Buildings.FirstOrDefault(i => i.UserId == userId && i.X == buildingDto.X && i.Y == buildingDto.Y);
            if (existBuilding != null) return BadRequest("Building already exist");

            var user = context.Users.FirstOrDefault(i => i.Id == userId);
            if (user != null)
            {
                buildingDto.UserId = (int)userId;
                buildingDto.Level = 1;
                var building = mapper.Map<Building>(buildingDto);
                user.UpdateCoins();

                if (user.Coins >= building.GetCoinsCost(context))
                {
                    context.Buildings.Add(building);
                    context.SaveChanges();

                    user.UpdateCoinsPerSecond(context);
                    user.Coins -= building.GetCoinsCost(context);
                }
                context.SaveChanges();
            }

            return Ok();
        }

        [HttpGet("up/{sid}/{id}")]
        [ProducesResponseType(typeof(IEnumerable<BuildingDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Up(string sid, int id)
        {
            var userId = GetUserIdBySession(sid);
            if (userId == null) return NotFound("Session not found");

            var existBuilding = context.Buildings.FirstOrDefault(i => i.Id == id);
            if (existBuilding == null) return NotFound("Building not found");
            if (existBuilding.UserId != userId) return NotFound("Building is not yours");

            var user = context.Users.FirstOrDefault(i => i.Id == userId);
            if (user != null)
            {
                user.UpdateCoins();
                if (user.Coins >= existBuilding.GetCoinsCost(context))
                {
                    user.Coins -= existBuilding.GetCoinsCost(context);
                    existBuilding.Level++;
                    user.UpdateCoinsPerSecond(context);
                }
                context.SaveChanges();
            }

            return Ok();
        }

        [HttpGet("down/{sid}/{id}")]
        [ProducesResponseType(typeof(IEnumerable<BuildingDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Down(string sid, int id)
        {
            var userId = GetUserIdBySession(sid);
            if (userId == null) return NotFound("Session not found");

            var existBuilding = context.Buildings.FirstOrDefault(i => i.Id == id);
            if (existBuilding == null) return NotFound("Building not found");
            if (existBuilding.UserId != userId) return NotFound("Building is not yours");

            var user = context.Users.FirstOrDefault(i => i.Id == userId);
            if (user != null)
            {
                user.UpdateCoins();
                user.Coins += existBuilding.GetCoinsCost(context);
                existBuilding.Level--;
                user.UpdateCoinsPerSecond(context);
                
                context.SaveChanges();
            }

            return Ok();
        }

        [HttpGet("remove/{sid}/{id}")]
        [ProducesResponseType(typeof(IEnumerable<BuildingDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Remove(string sid, int id)
        {
            var userId = GetUserIdBySession(sid);
            if (userId == null) return NotFound("Session not found");

            var existBuilding = context.Buildings.FirstOrDefault(i => i.Id == id);
            if (existBuilding == null) return NotFound("Building not found");
            if (existBuilding.UserId != userId) return NotFound("Building is not yours");

            var user = context.Users.FirstOrDefault(i => i.Id == userId);
            if (user != null)
            {
                user.UpdateCoins();
                while (existBuilding.Level >= 1)
                {
                    user.Coins += existBuilding.GetCoinsCost(context);
                    existBuilding.Level--;
                }
                
                context.Buildings.Remove(existBuilding);
                user.UpdateCoinsPerSecond(context);

                context.SaveChanges();
            }

            return Ok();
        }
    }
}
