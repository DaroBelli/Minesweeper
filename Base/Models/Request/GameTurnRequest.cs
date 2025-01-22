using System.Text.Json.Serialization;

namespace Base.Models.Request
{
    public class GameTurnRequest
    {
        /// <summary>
        /// Id игры.
        /// </summary>
        [JsonPropertyName("game_id")]
        public string GameId { get; set; } = string.Empty;

        /// <summary>
        /// Номер строки.
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Номер столбца.
        /// </summary>
        public int Col { get; set; }
    }
}
