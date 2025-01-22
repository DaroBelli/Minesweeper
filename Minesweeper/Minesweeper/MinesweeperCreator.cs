using Base.Models.Request;
using Base.Models;

namespace Minesweeper.Minesweeper
{
    public static class MinesweeperCreator
    {
        /// <summary>
        /// Создать поле.
        /// </summary>
        /// <param name="newGame">Данные по игре.</param>
        /// <param name="gameId">Идентификатор игры.</param>
        /// <returns>Список всех позиций на поле.</returns>
        public static List<Position> CreatePositions(NewGameRequest newGame, string gameId)
        {
            var positions = new List<Position>();
            var fields = CreateField(newGame);

            for (int i = 0; i < fields.Length; i++)
            {
                for (int j = 0; j < fields[i].Length; j++)
                {
                    positions.Add(new Position
                    {
                        GameId = gameId,
                        IsOpen = false,
                        Value = fields[i][j],
                        X = i,
                        Y = j
                    });
                }
            }

            return positions;
        }

        /// <summary>
        /// Создать позицию.
        /// </summary>
        /// <param name="newGame">Данные по игре.</param>
        /// <returns>Массив массивов позиций (Поле).</returns>
        private static string[][] CreateField(NewGameRequest newGame)
        {
            var fields = new string[newGame.Width][];

            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = new string[newGame.Height];

                for (int j = 0; j < fields[i].Length; j++)
                {
                    fields[i][j] = "0";
                }
            }

            CreateBombs(fields, newGame);

            return fields;
        }

        /// <summary>
        /// Создать бомбы.
        /// </summary>
        /// <param name="fields">Поле из 0.</param>
        /// <param name="newGame">Данные по игре.</param>
        private static void CreateBombs(string[][] fields, NewGameRequest newGame)
        {
            for (int i = 0; i < newGame.MinesCount; i++)
            {
                var isCreate = CreateBomb(fields, newGame);

                if (!isCreate)
                {
                    i--;
                }
            }
        }

        /// <summary>
        /// Создать бомбу.
        /// </summary>
        /// <param name="fields">Поле.</param>
        /// <param name="newGame">Данные по игре.</param>
        /// <returns>Создана ли бомба.</returns>
        private static bool CreateBomb(string[][] fields, NewGameRequest newGame)
        {
            var random = new Random();

            var x = random.Next(newGame.Width);
            var y = random.Next(newGame.Height);

            if (fields[x][y] != "X")
            {
                fields[x][y] = "X";

                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (i == 0 && j == 0)
                        {
                            continue;
                        }

                        var row = x + i;
                        var col = y + j;

                        if (row > -1 && row < newGame.Width && col > -1 && col < newGame.Height && int.TryParse(fields[row][col], out var value)) 
                        {
                            fields[row][col] = (++value).ToString();
                        }
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Создать поле из пустот.
        /// </summary>
        /// <param name="width">Ширина.</param>
        /// <param name="height">Высота.</param>
        /// <returns>Поле пустот.</returns>
        public static string[][] CreateDefaultField(int width, int height)
        {
            string[][] array = new string[width][];

            for (int i = 0; i < width; i++)
            {
                array[i] = new string[height];

                for (int j = 0; j < height; j++)
                {
                    array[i][j] = " ";
                }
            }

            return array;
        }
    }
}
