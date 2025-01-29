using Microsoft.AspNetCore.Mvc;
using Base.Models.Request;
using Minesweeper.Interfaces;

namespace Minesweeper.Controllers
{
    [ApiController]
    public class MinesweeperController(IMinesweeperRepository minesweeperRepository) : ControllerBase
    {
        readonly CancellationTokenSource tokenSource = new();

        /// <summary>
        /// Запрос новый игры.
        /// </summary>
        [Route("new")]
        [HttpPost]
        public async Task<IActionResult> NewGameRequest(NewGameRequest request)
        {
            try
            {
                var gameInfo = await minesweeperRepository.CreateNewGameAsync(request, tokenSource.Token);

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
        public async Task<IActionResult> GameTurnRequest(GameTurnRequest gameTurnRequest)
        {
            try
            {
                var gameInfo = await minesweeperRepository.GetInfoGameAsync(gameTurnRequest, tokenSource.Token);

                return Ok(gameInfo);
            }
            catch (Exception e) 
            {
                return BadRequest(e.Message);
            }
        }
    }
}
