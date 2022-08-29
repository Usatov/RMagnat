using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResourceMagnat.Data;
using ResourceMagnat.Dto;
using ResourceMagnat.Models;

namespace ResourceMagnat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MainDbContext context;
        private readonly SessionDbContext sessions;
        private readonly IMapper mapper;

        public UserController(MainDbContext _context, SessionDbContext _sessions, IMapper _mapper)
        {
            context = _context;
            sessions = _sessions;
            mapper = _mapper;
        }

        
        [HttpGet]
        public IEnumerable<User> Get() => context.Users.OrderBy(i => i.Id).ToList();


        //[HttpGet("uid")]
        //[ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        ////public async Task<IActionResult> GetByUid(string uid)
        ////{
        ////    var user = await context.Users.FirstOrDefaultAsync(i => i.Uid == uid);
        ////    return user == null ? NotFound() : Ok(user);
        ////}

        //public IActionResult GetByUid(string uid)
        //{
        //    var user = context.Users.FirstOrDefault(i => i.Uid == uid);
        //    return user == null ? NotFound() : Ok(user);
        //}

        //[HttpGet("id")]
        //[ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public IActionResult GetById(int id)
        //{
        //    var user = context.Users.FirstOrDefault(i => i.Id == id);
        //    return user == null ? NotFound() : Ok(user);
        //}

        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        ////public async Task<IActionResult> Create(User user)
        ////{
        ////    await context.Users.AddAsync(user);
        ////    await context.SaveChangesAsync();
        ////    return CreatedAtAction(nameof(GetByUid), new { uid = user.Uid }, user);
        ////}

        //public IActionResult Create(User user)
        //{
        //    context.Users.AddAsync(user);
        //    context.SaveChangesAsync();
        //    return CreatedAtAction(nameof(GetByUid), new { uid = user.Uid }, user);
        //}


        /// <summary>
        /// Возвращает пользователя по его UID. Если такого пользователя нет - создаёт его.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpGet("get/{uid}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        public IActionResult GetOrCreate(string uid)
        {
            var user = context.Users.FirstOrDefault(i => i.Uid == uid) ?? CreateUser(uid);

            user.UpdateCoins();
            context.SaveChanges();

            var result = mapper.Map<UserDto>(user);
            result.SessionId = CreateSession(user);

            return Ok(result);
        }

        private User CreateUser(string uid)
        {
            // Создаём пользователя
            var user = new User(uid);

            context.Users.Add(user);
            context.SaveChanges();

            // Добавляем ему начальное здание
            var building = new Building
            {
                UserId = user.Id,
                BuildingTypeId = 1,
                X = 3,
                Y = 2,
                Level = 1
            };
            context.Buildings.Add(building);
            context.SaveChanges();

            // Если зависимость не подгружается, загружаем её вручную
            //if (building.BuildingType == null) 
            //    building.BuildingType = context.BuildingTypes.First(i => i.Id == building.BuildingTypeId);

            // Пересчитываем доход в секунду
            user.UpdateCoinsPerSecond(context);
            // context.Entry(user).State = EntityState.Modified;
            context.SaveChanges();

            return user;
        }

        private string CreateSession(User user)
        {
            var existSesion = sessions.Sessions.FirstOrDefault(i => i.UserId == user.Id);
            if (existSesion == null)
            {
                existSesion = new Session
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id
                };
                sessions.Sessions.Add(existSesion);
                sessions.SaveChanges();
            }
            return existSesion.Id;
        }
    }
}
