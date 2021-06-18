using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Jeu_Pendu
{
    public class Biblio_Mot
    {
        public int id { get; set; }
        public string texte { get; set; }
        public string CompletIdText { get; set; }
    }
}
