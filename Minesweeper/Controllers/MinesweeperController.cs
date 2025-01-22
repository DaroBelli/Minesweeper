using Microsoft.AspNetCore.Mvc;
using Base.Models.Request;
using Minesweeper.Services.Minesweeper;

namespace Minesweeper.Controllers
{
    [ApiController]
    public class MinesweeperController : ControllerBase
    {
        /// <summary>
        /// Запрос новый игры.
        /// </summary>
        [Route("new")]
        [HttpPost]
        public async Task<IActionResult> NewGameRequest([FromServices] IMinesweeperService svc, NewGameRequest request)
        {
            try
            {
                var tokenSource = new CancellationTokenSource();
                var gameInfo = await svc.CreateNewGameAsync(request, tokenSource.Token);

                return Ok(gameInfo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Запрос текущего состояния игры.
        /// </summary>
        [Route("turn")]
        [HttpPost]
        public async Task<IActionResult> GameTurnRequest([FromServices] IMinesweeperService svc, GameTurnRequest gameTurnRequest)
        {
            try
            {
                var tokenSource = new CancellationTokenSource();
                var gameInfo = await svc.GetInfoGameAsync(gameTurnRequest, tokenSource.Token);

                return Ok(gameInfo);
            }
            catch (Exception e) 
            {
                return BadRequest(e.Message);
            }
        }
    }
}
