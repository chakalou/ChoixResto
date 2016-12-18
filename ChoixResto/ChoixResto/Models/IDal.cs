using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChoixResto.Models
{
    //Interface data acces layer (couche d'accès aux données de la bdd)
    public interface IDal:IDisposable
    {
        /*Restos*/
        void CreerRestaurant(string nom, string telephone);
        void ModifierRestaurant(int id, string nom, string telephone);
        bool RestaurantExiste(string nom);
        List<Resto> ObtientTousLesRestaurants();

        /*Utilisateur*/
        Utilisateur ObtenirUtilisateur(int id);
        Utilisateur ObtenirUtilisateur(string idStr);
        int AjouterUtilisateur(string nom, string motdepasse);
        Utilisateur Authentifier(string nom, string motdepasse);

        void AjouterVote(int idSondage, int idResto, int idUtilisateur);

        int CreerUnSondage();

        List<Resultats> ObtenirLesResultats(int idSondage);

        bool ADejaVote(int idSondage, string idStr);
    }
}