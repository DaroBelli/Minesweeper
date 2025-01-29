using Base.Models.Response;
using Base.Models.Request;
using Database;
using Minesweeper.Minesweeper;
using Microsoft.EntityFrameworkCore;
using Minesweeper.Interfaces;

namespace Minesweeper.Repositories.Minesweeper
{
    public class MinesweeperRepository(MinesweeperContext context) : IMinesweeperRepository
    {
        readonly MinesweeperContext context = context;
        public async Task<GameInfoResponse> CreateNewGameAsync(NewGameRequest newGame, CancellationToken cancellationToken)
        {
            if (newGame.Width > 31)
            {
                throw new ArgumentException("Ширина не должна превышать 30");
            }

            if (newGame.Height > 31)
            {
                throw new ArgumentException("Высота не должна превышать 30");
            }

            if (newGame.Width < 2)
            {
                throw new ArgumentException("Высота не должна быть меньше 2");
            }

            if (newGame.Height < 2)
            {
                throw new ArgumentException("Высота не должна быть меньше 2");
            }

            if (newGame.Width * newGame.Height < newGame.MinesCount)
            {
                throw new ArgumentException("Количество мин не должно превышать размер установленного поля");
            }

            var gameId = Guid.NewGuid().ToString();

            var gameInfo = new GameInfoResponse()
            {
                IsCompleted = false,
                GameId = gameId,
                Height = newGame.Height,
                MinesCount = newGame.MinesCount,
                Width = newGame.Width,
                Field = MinesweeperCreator.CreateDefaultField(newGame.Width, newGame.Height)
            };

            await context.GameInfoResponses.AddAsync(gameInfo, cancellationToken);
            await context.Positions.AddRangeAsync(MinesweeperCreator.CreatePositions(newGame, gameId), cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return gameInfo;
        }

        public async Task<GameInfoResponse> GetInfoGameAsync(GameTurnRequest gameTurnRequest, CancellationToken cancellationToken)
        {
            var gameInfo = await context.GameInfoResponses
                    .Where(x => x.GameId == gameTurnRequest.GameId)
                    .FirstOrDefaultAsync(cancellationToken)
                ?? throw new NullReferenceException("Игра не найдена");

            if (gameInfo.IsCompleted)
            {
                throw new Exception("Игра уже завершена");
            }

            if (gameTurnRequest.Row >= gameInfo.Width || gameTurnRequest.Col >= gameInfo.Height)
            {
                throw new ArgumentException("Позиция больше размера поля");
            }

            var positions = await context.Positions
                .Where(x => x.GameId == gameTurnRequest.GameId)
                .ToArrayAsync(cancellationToken);

            var getPosition = positions
                .Where(x => x.X == gameTurnRequest.Row && x.Y == gameTurnRequest.Col)
                .FirstOrDefault();

            var getter = new MinesweeperInfoGameGetter();

            if (getPosition != null)
            {
                await getter.OpenPositionAsync(getPosition, gameInfo, positions, context, cancellationToken);
            }

            gameInfo.Field = await getter.GetFieldAsync(gameInfo, getPosition?.Value, context, cancellationToken);

            if (gameInfo.IsCompleted)
            {
                context.GameInfoResponses.Update(gameInfo);
            }

            await context.SaveChangesAsync(cancellationToken);

            return gameInfo;
        }
    }
}
