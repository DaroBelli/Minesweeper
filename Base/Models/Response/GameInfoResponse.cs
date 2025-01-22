using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Base.Models.Request;

namespace Base.Models.Response
{
    [Table("game_info")]
    public class GameInfoResponse : NewGameRequest
    {
        /// <summary>
        /// Идентификатор игры.
        /// </summary>
        [Key]
        [JsonPropertyName("game_id")]
        [Column("game_id")]
        public required string GameId { get; set; }

        /// <summary>
        /// Завершена игра или нет.
        /// </summary>
        [Column("completed")]
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Поле.
        /// </summary>
        [NotMapped]
        public required string[][] Field { get; set; }
    }
}
