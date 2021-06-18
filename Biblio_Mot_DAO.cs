using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Jeu_Pendu
{
    public class Biblio_Mot_DAO
    {
        string s = "";
        public Biblio_Mot_DAO(string chaine)
        {
            s = chaine;
        }
        /////////////////////////////// FRANCAIS ///////////////////////////////


        // Permet de trouver un mot dans la bibliothèque grace à son id 
        public Biblio_Mot F_Find(int id)
        {
            SQLiteConnection _connect = new SQLiteConnection(s);
            _connect.Open();
            Biblio_Mot BM = null;
            SQLiteCommand requete = new SQLiteCommand("select * from Français where id=@id", _connect);
            requete.Parameters.AddWithValue("@id", id);
            SQLiteDataReader lecture = requete.ExecuteReader();
            if (lecture.Read())
            {
                BM = new Biblio_Mot();
                BM.id = Convert.ToInt32(lecture["id"]);
                BM.texte = Convert.ToString(lecture["texte"]);
            }
            lecture.Close();
            _connect.Close();
            return BM;
        }

        // Retourne le nombre de ligne dans la bibliothèque
        public int F_NombreColonne()
        {
            string resultat = "";
            int c;
            SQLiteConnection _connect = new SQLiteConnection(s);
            _connect.Open();
            SQLiteCommand requete = new SQLiteCommand("select count(id) from Français", _connect);
            SQLiteDataReader lecture = requete.ExecuteReader();

            while (lecture.Read())
                resultat = (lecture["count(id)"]).ToString();

            Int32.TryParse(resultat, out c);

            lecture.Close();
            _connect.Close();
            return c;
        }

        // Verifie l'existance ou non du mot que l'utilisateur veut ajouter dans la bibliothèque
        public int F_Doublons(Biblio_Mot bibl)
        {
            string resultat = "";
            int c;

            SQLiteConnection _connect = new SQLiteConnection(s);
            _connect.Open();
            SQLiteCommand verif = new SQLiteCommand("select count(*) from Français where texte=@texte", _connect);
            verif.Parameters.AddWithValue("@texte", bibl.texte);
            SQLiteDataReader lecture = verif.ExecuteReader();

            while (lecture.Read())
                resultat = (lecture["count(*)"]).ToString();

            Int32.TryParse(resultat, out c);

            lecture.Close();
            _connect.Close();
            return c;
        }

        // Créer la liste des mots dans la bibliothèque
        public List<Biblio_Mot> F_List()
        {
            SQLiteConnection _connect = new SQLiteConnection(s);
            _connect.Open();
            List<Biblio_Mot> Liste = new List<Biblio_Mot>();
            SQLiteCommand requete = new SQLiteCommand("select * from Français", _connect);
            SQLiteDataReader lecture = requete.ExecuteReader();
            while (lecture.Read())
            {
                Biblio_Mot B_M = new Biblio_Mot();
                B_M.id = Convert.ToInt32(lecture["id"]);
                B_M.texte = Convert.ToString(lecture["texte"]);
                B_M.CompletIdText = B_M.id.ToString() + " - " + B_M.texte.ToString();
                Liste.Add(B_M);
            }
            lecture.Close();
            _connect.Close();
            return Liste;
        }

        // Ajout de mot dans la bibliothèque
        public void F_Insert(Biblio_Mot bibl)
        {
            SQLiteConnection _connect = new SQLiteConnection(s);
            _connect.Open();
            SQLiteCommand requete = new SQLiteCommand("insert into Français (texte) values (@texte)", _connect);
            requete.Parameters.AddWithValue("@texte", bibl.texte);
            requete.ExecuteNonQuery();
            _connect.Close();
        }

        // Modification de mot dans la bibliothèque
        public void F_Update(Biblio_Mot bibl)
        {
            SQLiteConnection _connect = new SQLiteConnection(s);
            _connect.Open();
            SQLiteCommand requete = new SQLiteCommand("update Français set texte=@texte where id=@id", _connect);
            requete.Parameters.AddWithValue("@id", bibl.id);
            requete.Parameters.AddWithValue("@texte", bibl.texte);
            requete.ExecuteNonQuery();
            _connect.Close();
        }

        /////////////////////////////// LATIN ///////////////////////////////


        // Permet de trouver un mot dans la bibliothèque grace à son id 
        public Biblio_Mot L_Find(int id)
        {
            SQLiteConnection _connect = new SQLiteConnection(s);
            _connect.Open();
            Biblio_Mot BM = null;
            SQLiteCommand requete = new SQLiteCommand("select * from Latin where id=@id", _connect);
            requete.Parameters.AddWithValue("@id", id);
            SQLiteDataReader lecture = requete.ExecuteReader();
            if (lecture.Read())
            {
                BM = new Biblio_Mot();
                BM.id = Convert.ToInt32(lecture["id"]);
                BM.texte = Convert.ToString(lecture["texte"]);
            }
            lecture.Close();
            _connect.Close();
            return BM;
        }

        // Retourne le nombre de ligne dans la bibliothèque
        public int L_NombreColonne()
        {
            string resultat = "";
            int c;
            SQLiteConnection _connect = new SQLiteConnection(s);
            _connect.Open();
            SQLiteCommand requete = new SQLiteCommand("select count(id) from Latin", _connect);
            SQLiteDataReader lecture = requete.ExecuteReader();

            while (lecture.Read())
                resultat = (lecture["count(id)"]).ToString();

            Int32.TryParse(resultat, out c);

            lecture.Close();
            _connect.Close();
            return c;
        }

        // Verifie l'existance ou non du mot que l'utilisateur veut ajouter dans la bibliothèque
        public int L_Doublons(Biblio_Mot bibl)
        {
            string resultat = "";
            int c;

            SQLiteConnection _connect = new SQLiteConnection(s);
            _connect.Open();
            SQLiteCommand verif = new SQLiteCommand("select count(*) from Latin where texte=@texte", _connect);
            verif.Parameters.AddWithValue("@texte", bibl.texte);
            SQLiteDataReader lecture = verif.ExecuteReader();

            while (lecture.Read())
                resultat = (lecture["count(*)"]).ToString();

            Int32.TryParse(resultat, out c);

            lecture.Close();
            _connect.Close();
            return c;
        }

        // Créer la liste des mots dans la bibliothèque
        public List<Biblio_Mot> L_List()
        {
            SQLiteConnection _connect = new SQLiteConnection(s);
            _connect.Open();
            List<Biblio_Mot> Liste = new List<Biblio_Mot>();
            SQLiteCommand requete = new SQLiteCommand("select * from Latin", _connect);
            SQLiteDataReader lecture = requete.ExecuteReader();
            while (lecture.Read())
            {
                Biblio_Mot B_M = new Biblio_Mot();
                B_M.id = Convert.ToInt32(lecture["id"]);
                B_M.texte = Convert.ToString(lecture["texte"]);
                B_M.CompletIdText = B_M.id.ToString() + " - " + B_M.texte.ToString();
                Liste.Add(B_M);
            }
            lecture.Close();
            _connect.Close();
            return Liste;
        }

        // Ajout de mot dans la bibliothèque
        public void L_Insert(Biblio_Mot bibl)
        {
            SQLiteConnection _connect = new SQLiteConnection(s);
            _connect.Open();
            SQLiteCommand requete = new SQLiteCommand("insert into Latin (texte) values (@texte)", _connect);
            requete.Parameters.AddWithValue("@texte", bibl.texte);
            requete.ExecuteNonQuery();
            _connect.Close();
        }

        // Modification de mot dans la bibliothèque
        public void L_Update(Biblio_Mot bibl)
        {
            SQLiteConnection _connect = new SQLiteConnection(s);
            _connect.Open();
            SQLiteCommand requete = new SQLiteCommand("update Latin set texte=@texte where id=@id", _connect);
            requete.Parameters.AddWithValue("@id", bibl.id);
            requete.Parameters.AddWithValue("@texte", bibl.texte);
            requete.ExecuteNonQuery();
            _connect.Close();
        }
    }
}
