using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTeProject.BackEnd.Entities;
using MyTeProject.BackEnd.Entities.User;
using MyTeProject.BackEnd.Entities.WBSEntities;

namespace MyTeProject.BackEnd.Controllers.WBSControllers
{
    [Authorize]
    [Route("v1/[controller]")]
    [ApiController]
    public class UserWBSController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;


        public UserWBSController(AppDbContext dbContext, UserManager<AppUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        // Post de novos registros para o usuário autenticado
        [HttpPost]
        public async Task<ActionResult> Post(IList<int> idsWBSModels)
        {
            // Buscar ID do user atual (como a classe possuí authorize, sabemos que não será nulo)
            AppUser? appUser = await _userManager.GetUserAsync(HttpContext.User);


            // Excluir suas UserWBS
            IList<UserWBS> currentWBSs = await _dbContext.UserWBS
                                                            .Include(e => e.User)
                                                            .Where(e => e.User.Id == appUser.Id)
                                                            .ToListAsync();
            _dbContext.UserWBS.RemoveRange(currentWBSs);

            // Adicionar as que ele enviou
            foreach (var idWbsModel in idsWBSModels)
            {
                WBS wbsEntity = await _dbContext.WBS.FindAsync(idWbsModel);

                if (wbsEntity == null)
                {
                    return BadRequest($"WBS of id {idWbsModel} was not found");
                }

                await _dbContext.UserWBS.AddAsync(ConvertModelToEntity(appUser, wbsEntity));
            }

            await _dbContext.SaveChangesAsync();

            return Ok(idsWBSModels);
        }

        private UserWBS ConvertModelToEntity(AppUser user, WBS wbs)
        {
            return new UserWBS
            {
                User = user,
                WBS = wbs
            };
        }
    }
}
