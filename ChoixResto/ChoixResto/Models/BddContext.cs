using System.Data.Entity;

namespace ChoixResto.Models
{
    public class BddContext:DbContext
    {
        public DbSet<Sondage> Sondages { get; set; }
        public DbSet<Resto> Restos { get; set; }
        public DbSet<Utilisateur> Utilisateurs { get; set; }
    }
}