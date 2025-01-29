using Base.Models.Response;
using Base.Models;
using Database;
using Microsoft.EntityFrameworkCore;

namespace Minesweeper.Minesweeper
{
    public class MinesweeperInfoGameGetter
    {
        /// <summary>
        /// Открыть позицию и сделать действия связанные с ней.
        /// </summary>
        /// <param name="getPosition">Выбранная позиция.</param>
        /// <param name="gameInfo">Информация по игре.</param>
        /// <param name="positions">Все позиции.</param>
        /// <param name="context">Контекст БД.</param>
        /// <param name="cancellationToken">Токен.</param>
        public async Task OpenPositionAsync(Position getPosition, GameInfoResponse gameInfo, Position[] positions, MinesweeperContext context, CancellationToken cancellationToken)
        {
            if (getPosition.IsOpen)
            {
                throw new Exception("Ячейка уже была открыта");
            }
            else if (getPosition.Value == "X")
            {
                for (int i = 0; i < positions.Length; i++)
                {
                    positions[i].IsOpen = true;
                }

                gameInfo.IsCompleted = true;
                context.Positions.UpdateRange(positions);
            }
            else if (getPosition.Value == "0")
            {
                OpenZeroAsync(getPosition, gameInfo, positions, context);
            }
            else
            {
                getPosition.IsOpen = true;

                context.Positions.Update(getPosition);
            }

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Открыть ячейку с 0
        /// </summary>
        /// <param name="getPosition">Выбранная позиция.</param>
        /// <param name="gameInfo">Информация по игре.</param>
        /// <param name="positions">Все позиции.</param>
        /// <param name="context">Контекст БД.</param>
        private static void OpenZeroAsync(Position getPosition, GameInfoResponse gameInfo, Position[] positions, MinesweeperContext context)
        {
            var openedZeroPositions = positions
                .Where(x => x.IsOpen && x.Value == "0")
                .Select(x => x.Id)
                .ToList();

            getPosition.IsOpen = true;
            context.Update(getPosition);

            OpenAroundZero(getPosition, gameInfo, positions, openedZeroPositions, context);
        }

        /// <summary>
        /// Открыть все ячейки рядом с выбранной 0.
        /// </summary>
        /// <param name="zeroPosition">Позиция с 0.</param>
        /// <param name="gameInfo">Информация по игре.</param>
        /// <param name="positions">Все позиции.</param>
        /// <param name="openedZeroPositions">Список открытых позиций с 0.</param>
        /// <param name="context">Контекст БД.</param>
        private static void OpenAroundZero(Position zeroPosition, GameInfoResponse gameInfo, Position[] positions, List<int> openedZeroPositions, MinesweeperContext context)
        {
            openedZeroPositions.Add(zeroPosition.Id);

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    var row = zeroPosition.X + i;
                    var col = zeroPosition.Y + j;

                    var aroundPosition = positions
                        .Where(x => x.X == row && x.Y == col)
                        .FirstOrDefault();

                    if (aroundPosition != null && !openedZeroPositions.Where(x => x == aroundPosition.Id).Any() && !aroundPosition.IsOpen)
                    {
                        if (aroundPosition.Value == "0")
                        {
                            OpenAroundZero(aroundPosition, gameInfo, positions, openedZeroPositions, context);
                        }

                        aroundPosition.IsOpen = true;
                        context.Update(aroundPosition);
                    }
                }
            }
        }

        /// <summary>
        /// Получить актуальный массив массивов всех позиций (Поле).
        /// </summary>
        /// <param name="gameInfo">Информация по игре.</param>
        /// <param name="context">Контекст БД.</param>
        /// <param name="cancellationToken">Токен.</param>
        /// <returns>Поле.</returns>
        public async Task<string[][]> GetFieldAsync(GameInfoResponse gameInfo, string? getPositionValue, MinesweeperContext context, CancellationToken cancellationToken)
        {
            var positions = await context.Positions
                .Where(x => x.GameId == gameInfo.GameId)
                .ToArrayAsync(cancellationToken);

            var field = new string[gameInfo.Width][];

            if (!positions.Where(x => !x.IsOpen && x.Value != "X").Any() && getPositionValue != "X")
            {
                gameInfo.IsCompleted = true;

                for (int i = 0; i < positions.Length; i++)
                {
                    if (positions[i].Value == "X")
                    {
                        positions[i].Value = "M";
                        positions[i].IsOpen = true;
                    }
                }

                var positionsBomb = positions.Where(x => x.Value == "M").ToArray();

                context.UpdateRange(positionsBomb);
            }

            for (int i = 0; i < gameInfo.Width; i++)
            {
                field[i] = new string[gameInfo.Height];

                for (int j = 0; j < gameInfo.Height; j++)
                {
                    var position = positions.Where(x => x.X == i && x.Y == j).FirstOrDefault();

                    field[i][j] = position != null && position.IsOpen ? position.Value : " ";
                }
            }

            return field;
        }
    }
}
