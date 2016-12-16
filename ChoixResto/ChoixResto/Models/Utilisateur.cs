using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChoixResto.Models
{
    public class Utilisateur
    {
        public int Id { get; set; }
        [Required, MaxLength(80)]
        public string Prenom { get; set; }
        [Required, MaxLength(80)]
        public string motdepass { get; set; }
    }
}