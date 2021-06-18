using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Jeu_Pendu
{
    public class Biblio_DAO
    {
        string s = "";
        public Biblio_DAO(string chaine)
        {
            s = chaine;
        }

        public void Insert(Biblio mot)
        {
            SQLiteConnection _connect = new SQLiteConnection(s);
            _connect.Open();
            SQLiteCommand requete = new SQLiteCommand("INSERT INTO Mots (texte) VALUES ('@texte')", _connect);
            requete.Parameters.AddWithValue("@id", mot.id);
            requete.Parameters.AddWithValue("@texte", mot.texte);
            requete.ExecuteNonQuery();
            _connect.Close();
        }

        public void Update(Biblio mot)
        {
            SQLiteConnection _connect = new SQLiteConnection(s);
            _connect.Open();
            SQLiteCommand requete = new SQLiteCommand("UPDATE Mots SET texte=@texte WHERE id =@id", _connect);
            requete.Parameters.AddWithValue("@id", mot.id);
            requete.Parameters.AddWithValue("@texte", mot.texte);
            requete.ExecuteNonQuery();
            _connect.Close();
        }

        public void Delete(Biblio mot)
        {
            SQLiteConnection _connect = new SQLiteConnection(s);
            _connect.Open();
            SQLiteCommand requete = new SQLiteCommand("Delete from Mots where id=@id", _connect);
            requete.Parameters.AddWithValue("@id", mot.id);
            requete.ExecuteNonQuery();
            _connect.Close();
        }

        public Biblio Find(int id)
        {
            SQLiteConnection _connect = new SQLiteConnection(s);
            _connect.Open();
            Biblio m = null;
            SQLiteCommand requete = new SQLiteCommand("Select * from Mots where id=@id ", _connect);
            requete.Parameters.AddWithValue("@id", id);
            SQLiteDataReader lecture = requete.ExecuteReader();
            if (lecture.Read())
            {
                m = new Biblio();
                m.id = Convert.ToInt32(lecture["id"]);
                m.texte = Convert.ToString(lecture["texte"]);
            }
            lecture.Close();
            _connect.Close();
            return m;
        }

        public Biblio NbreCol()
        {
            SQLiteConnection _connect = new SQLiteConnection(s);
            _connect.Open();
            Biblio m = null;
            SQLiteCommand requete = new SQLiteCommand("Select Count(*) from Mots", _connect);
            SQLiteDataReader lecture = requete.ExecuteReader();
            if (lecture.Read())
            {
                m = new Biblio();
                m.id = Convert.ToInt32(lecture["Count(*)"]);
            }
            lecture.Close();
            _connect.Close();
            return m;
        }
    }
}
