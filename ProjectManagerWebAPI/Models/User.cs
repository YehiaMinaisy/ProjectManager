using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagerWebAPI.Models
{
    [Table("AspNetUsers")]
    public class User : IdentityUser
    {

        public virtual ICollection<TaskUser> TaskUsers { get; set; } = new List<TaskUser>();
    }
}
