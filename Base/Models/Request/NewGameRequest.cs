using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Base.Models.Request
{
    public class NewGameRequest
    {
        /// <summary>
        /// Ширина.
        /// </summary>
        [Column("width")]
        public required int Width { get; set; }

        /// <summary>
        /// Высота.
        /// </summary>
        [Column("height")] 
        public required int Height { get; set; }

        /// <summary>
        /// Количество мин.
        /// </summary>
        [JsonPropertyName("mines_count")]
        [Column("mines_count")]
        public required int MinesCount { get; set; }
    }
}
