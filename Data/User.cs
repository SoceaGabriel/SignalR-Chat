using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SignalR_Chat.Data
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; } 
        public string Name { get; set; }
    }
}
