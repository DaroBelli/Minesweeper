using System.ComponentModel.DataAnnotations.Schema;

namespace Base.Models
{
    [Table("position")]
    public class Position
    {
        /// <summary>
        /// Идентификатор позиции.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор игры.
        /// </summary>
        public required string GameId { get; set; }

        /// <summary>
        /// X координата позиции.
        /// </summary>
        public required int X { get; set; }

        /// <summary>
        /// Y координата позиции.
        /// </summary>
        public required int Y { get; set; }

        /// <summary>
        /// Значение.
        /// </summary>
        public required string Value { get; set; }

        /// <summary>
        /// Открыта позиция или нет.
        /// </summary>
        [Column("is_open")]
        public required bool IsOpen { get; set; }
    }
}
