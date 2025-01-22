using Base.Models.Response;
using Base.Models.Request;

namespace Minesweeper.Services.Minesweeper
{
    public interface IMinesweeperService
    {
        /// <summary>
        /// Создать новую игру.
        /// </summary>
        /// <param name="newGame">Данные по новой игре.</param>
        /// <param name="cancellationToken">Токен.</param>
        /// <returns>Данные о созданной игре.</returns>
        Task<GameInfoResponse> CreateNewGameAsync(NewGameRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Получить актуальные данные по игре.
        /// </summary>
        /// <param name="gameTurnRequest">Данные о ходе игрока.</param>
        /// <param name="cancellationToken">Токен.</param>
        /// <returns>Данные о текущей игре.</returns>
        Task<GameInfoResponse> GetInfoGameAsync(GameTurnRequest gameTurnRequest, CancellationToken cancellationToken);
    }
}
