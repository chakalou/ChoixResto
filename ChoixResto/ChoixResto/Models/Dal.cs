using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ChoixResto.Models
{
    public class Dal : IDal
    {
        private BddContext bdd;

        public Dal()
        {
            bdd = new BddContext();
        }
        public int AjouterUtilisateur(string nom, string motdepasse)
        {
            /*On ajoute l'utilisateur à la base de données*/
            string str = EncodeMD5(motdepasse);
            bdd.Utilisateurs.Add(new Utilisateur { Prenom = nom, MotDePasse = str });
            bdd.SaveChanges();

            /*On va le chercher et on retrourne son id si ok*/
            Utilisateur newutil = bdd.Utilisateurs.FirstOrDefault(n => n.Prenom == nom && n.MotDePasse == str);
            if (newutil != null)
                return newutil.Id;
            else
                return 0;

        }
        public Utilisateur Authentifier(string nom, string motDePasse)
        {
            string str = EncodeMD5(motDePasse);
            Utilisateur util = bdd.Utilisateurs.FirstOrDefault(u => u.Prenom == nom && u.MotDePasse == str);
            return util;

        }

        public Utilisateur ObtenirUtilisateur(int id)
        {
            Utilisateur util = bdd.Utilisateurs.FirstOrDefault(u => u.Id == id);
            return util;
        }

        public Utilisateur ObtenirUtilisateur(string idStr)
        {
            int id;

            if (!int.TryParse(idStr, out id))
                return null;
            Utilisateur util = bdd.Utilisateurs.FirstOrDefault(u => u.Id == id);
            return util;

        }

        public int CreerUnSondage()
        {
            Sondage sondage = new Sondage();
            sondage.Date = DateTime.Today;
            sondage.Votes = new List<Vote>();
            int count = bdd.Sondages.Count();
            bdd.Sondages.Add(sondage);
            bdd.SaveChanges();
            int id = count + 1;
            if (id != bdd.Sondages.Count())
                return 0;
            else
                return id;
        }

        public void AjouterVote(int idSondage, int idResto, int idUtilisateur)
        {
            Vote vote = new Vote();
            /*On récupère le sondage*/
            Sondage sondage = bdd.Sondages.FirstOrDefault(x => x.Id == idSondage);
            if (sondage == null)
                return;

            /*On récupère le resto*/
            vote.Resto = bdd.Restos.FirstOrDefault(x => x.Id == idResto);
            if (vote.Resto == null)
                return;
            /*On récupère l'utilisateur*/
            vote.Utilisateur = bdd.Utilisateurs.FirstOrDefault(x => x.Id == idUtilisateur);
            if (vote.Utilisateur == null)
                return;

            /*On sauvegarde*/
            bdd.Sondages.FirstOrDefault(x => x.Id == idSondage).Votes.Add(vote);
            bdd.SaveChanges();

        }

        public List<Resultats> ObtenirLesResultats(int idSondage)
        {
            List<Resultats> liste = new List<Resultats>();

            Sondage sondage = bdd.Sondages.FirstOrDefault(x => x.Id == idSondage);
            if (sondage != null)
            {
                foreach (Vote vote in sondage.Votes)
                {
                    Resultats res = liste.FirstOrDefault(x => x.Nom == vote.Resto.Nom && x.Telephone == vote.Resto.Telephone);
                    if (res != null)
                        res.NombreDeVotes++;
                    else
                    {
                        res = new Resultats();
                        res.Nom = vote.Resto.Nom;
                        res.Telephone = vote.Resto.Telephone;
                        res.NombreDeVotes = 1;

                        liste.Add(res);
                    }

                }
            }

            //liste.OrderBy(x => x.Nom);

            return liste;
        }

        public bool ADejaVote(int idSondage, string idStr)
        {
            int id;
            
            if (!int.TryParse(idStr, out id))
                return false;

            Sondage sondage = bdd.Sondages.FirstOrDefault(x => x.Id == idSondage);

            if (sondage.Votes.Where(x => x.Utilisateur.Id == id).ToList().Count > 0)
                return true;
            else
                return false;
        }

        public List<Resto> ObtientTousLesRestaurants()
        {
            return bdd.Restos.ToList();
        }

        public void CreerRestaurant(string nom, string telephone)
        {
            bdd.Restos.Add(new Resto { Nom = nom, Telephone = telephone });
            bdd.SaveChanges();
        }
        public void ModifierRestaurant(int id, string nom, string telephone)
        {
            Resto restoTrouve = bdd.Restos.FirstOrDefault(resto => resto.Id == id);
            if (restoTrouve != null)
            {
                restoTrouve.Nom = nom;
                restoTrouve.Telephone = telephone;
                bdd.SaveChanges();
            }
        }
        public bool RestaurantExiste(string nom)
        {
            return bdd.Restos.Any(resto => string.Compare(resto.Nom, nom, StringComparison.CurrentCultureIgnoreCase) == 0);
        }

        public void Dispose()
        {
            bdd.Dispose();
        }

        private string EncodeMD5(string motDePasse)
        {
            string motDePasseSel = "ChoixResto" + motDePasse + "ASP.NET MVC";
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.Default.GetBytes(motDePasseSel)));
        }
    }
}