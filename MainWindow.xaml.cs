using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Jeu_Pendu
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Biblio_Mot_DAO Biblio_Mot_DAO = new Biblio_Mot_DAO("Data Source=dictionnaire_de_mot.db3;");

        public MainWindow()
        {
            InitializeComponent();
        }

        // Variables initiales
        string MOT;
        string[] recherche;
        string[] solution;
        string l;
        int v = 11;
        int aide = 0;
        int niveau = 1;
        string Aide_O;
        string Langue = "Français";
        string identique = "";

        // Selectionne un mot aleatoirement dans la base de donnée et de renvoi sous forme du string MOT
        private string GetMot()
        {
            string MOT = "";
            int nombrec = 0;
            do
            {
                Biblio_Mot_DAO Biblio_Mot_DAO = new Biblio_Mot_DAO("Data Source=dictionnaire_de_mot.db3;");
                if (Langue == "Français")
                    nombrec = Biblio_Mot_DAO.F_NombreColonne();
                else if (Langue == "Latin")
                    nombrec = Biblio_Mot_DAO.L_NombreColonne();
                Random NombreAlea = new Random();
                int Rand = NombreAlea.Next(1, nombrec);
                if (Langue == "Français")
                    MOT = Biblio_Mot_DAO.F_Find(Rand).texte;
                else if (Langue == "Latin")
                    MOT = Biblio_Mot_DAO.L_Find(Rand).texte;
            } while (identique == MOT); // Verifie que le mot n'est pas le même que le precedent
            identique = MOT;
            return MOT;
        }

        // Lancement de l'application
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MOT = GetMot();
            CreateTable();
            LNiveau.Content = "Niveau : " + niveau;
            BtnSuivant.Visibility = Visibility.Hidden;
            FR_Click(null, null);
        }

        // Créer des tableaux correspondant au mot recherché
        private void CreateTable()
        {
            recherche = new string[MOT.Length];
            solution = new string[MOT.Length];

            aide = (MOT.Length / 4);
            M_aide.Header = "Aide : " + aide;
            Help.Header = "Help (" + aide + ")";

            // Remplissage des tableaux
            for (int i = 0; i < MOT.Length; i++)
            {
                solution[i] = MOT.Substring(i, 1);
                recherche[i] = " • ";
            }

            // Affichage du squelette 
            MOT_RECHERCHER.Content = null;

            for (int j = 0; j < recherche.Length; j++)
            {
                MOT_RECHERCHER.Content += recherche[j].ToString();
            }
        }

        // Initialise un nouveau mot
        public void ResetMot()
        {
            aide = (MOT.Length / 4);
            M_aide.Header = "Aide : " + aide;
            Help.Header = "Help (" + aide +")";
            MOT = GetMot();
            int taille = MOT.Length;
            recherche = new string[taille];
            solution = new string[taille];
            FullVie();
        }

        // Gestion des lettres au clavier
        private void Generique_Click(string lettre)
        {
            l = lettre.ToLower();
            foreach (Button b in this.grid1.Children.OfType<Button>())
                if (b.Name == lettre.ToUpper() && (A.IsEnabled)) // Verifie que les lettres sont activées (si A est active alors les autres le sont aussi)
                    b.Background = CorrespondanceLettreMot(MOT, l, solution, recherche);
        }

        // Gestion des lettres au click
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            if (A.IsEnabled)
                Generique_Click(b.Content.ToString());
        }

        // Correspondance Lettre Mot
        private SolidColorBrush CorrespondanceLettreMot(string m, string l, string[] tableau1, string[] tableau2)
        {
            // Defini les couleurs
            var vert = new SolidColorBrush(Colors.YellowGreen);
            var rouge = new SolidColorBrush(Colors.Crimson);

            // Verifie si la lettre existe dans le mot et si oui assigne la valeur 1 à c
            int c = 0;
            for (int i = 0; i < m.Length; i++)
            {
                if (l == tableau1[i])
                {
                    tableau2[i] = " " + tableau1[i] + " ";
                    c = 1;
                    // Permet d'actualiser l'affichage du mot enigme
                    MOT_RECHERCHER.Content = null;
                    for (int u = 0; u < recherche.Length; u++)
                    {
                        MOT_RECHERCHER.Content += recherche[u].ToString();
                    }
                }
            }
            // Verifie la similitude des deux tableaux et si c'est la cas annonce la victoire
            Identique(m, tableau1, tableau2);
            // Si la lettre existe dans le mot sont background devient vert sinon rouge
            if (c == 1)
                return vert;
            else
            {
                v = v - 1;
                if (v <= 3)
                    M_aide.IsEnabled = false;
                if (v <= 0)
                {
                    var perdu = new SolidColorBrush(Colors.Firebrick);
                    MOT_RECHERCHER.Foreground = perdu;
                    MOT_RECHERCHER.Content = "Perdu ! L'enigme était : ";
                    for (int i = 0; i < solution.Length; i++)
                    {
                        MOT_RECHERCHER.Content += solution[i].ToString();
                    }
                    DisabledLettres();
                    BtnSuivant.Visibility = Visibility.Visible;
                    BtnSuivant.Content = "New Game";
                    NiveauUn();
                    M_aide.IsEnabled = false;
                }
                NombreVie(v);
                return rouge;
            }
        }

        // Verifie la similitude des deux tableaux et si c'est la cas annonce la victoire
        private void Identique(string m, string[] tableau1, string[] tableau2)
        {
                var gagne = new SolidColorBrush(Colors.Green);

                int p = 0;
                for (int i = 0; i < m.Length; i++)
                {
                    string v = tableau1[i];
                    string w = tableau2[i];
                    if (" " + v + " " == w)
                        p++;
                }
                if (p == tableau1.Length)
                {
                    MOT_RECHERCHER.Foreground = gagne;
                    MOT_RECHERCHER.Content = "Bravo ! Vous avez trouvé le mot : " + m;
                    GainNiveau();
                    M_aide.IsEnabled = false;
                    DisabledLettres();
                    BtnSuivant.Visibility = Visibility.Visible;
                    BtnSuivant.Content = "Continue";
            }
        }

        // Lance la procedure d'aide qui revele une lettre de l'enigme
        private void HelpPlease(int n, string[] tableau1, string[] tableau2)
        {
            for (int i = 0; i < n; i++)
            {
                if (tableau1[i] == " • ")
                    {
                        string v = tableau2[i];
                        Generique_Click(v);
                        MOT_RECHERCHER.Content = null;
                        i = n;
                    }
            }

            for (int u = 0; u < recherche.Length; u++)
            {
                MOT_RECHERCHER.Content += recherche[u].ToString();
            }
            CorrespondanceLettreMot(MOT, l, solution, recherche);
        }

        // Gestion des inputs clavier
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
                M_newgame_Click(null, null);
            else if (e.Key == Key.F1)
                M_aide_Click(null, null);
            else if (e.Key == Key.F12)
                M_solution_Click(null, null);
            else if (e.Key == Key.Space)
                BtnSuivant_Click(null, null);
            else if (e.Key == Key.F11)
                D_options_Click(null, null);
            else 
                Generique_Click(e.Key.ToString()); // Transmet la valeur de la touche préssée en format string dans la fonction Generique_Click    
        }

        // Bouton suivant
        private void BtnSuivant_Click(object sender, RoutedEventArgs e)
        {
            if (BtnSuivant.IsVisible)
            {
                if ((string)BtnSuivant.Content == "Continue")
                {
                    NewGame();
                }
                else if ((string)BtnSuivant.Content == "New Game")
                {
                    NewGame();
                    NiveauUn();
                }   
            }
        }

        // Initialise une nouvelle partie
        private void M_newgame_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
            NiveauUn();       
        }

        // Active le module d'aide
        private void M_aide_Click(object sender, RoutedEventArgs e)
        {
            if (M_aide.IsEnabled)
            {
                if (aide > 0)
                {
                    HelpPlease(MOT.Length, recherche, solution);
                    aide += -1;
                    M_aide.Header = "Aide : " + aide;
                    Help.Header = "Help (" + aide + ")";
                    v += -2;
                }
                if (aide <= 0)
                {
                    M_aide.Header = "Aide : " + aide;
                    Help.Header = "Help (" + aide + ")";
                    M_aide.IsEnabled = false;
                }
                if (v <= 3)
                    M_aide.IsEnabled = false;
                NombreVie(v);
            }
        }

        // Initialise une nouvelle partie 
        private void NewGame()
        {
            SolidColorBrush Noir = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            MOT_RECHERCHER.Foreground = Noir;
            v = 11;
            EnabledLettres();
            ResetMot();
            MOT_RECHERCHER.Content = null;
            CreateTable();
            ResetColor();
            FullVie();
            BtnSuivant.Visibility = Visibility.Hidden;
            AideDifficulty();
        }

        // Genere la solution et l'affiche
        private void M_solution_Click(object sender, RoutedEventArgs e)
        {
            var perdu = new SolidColorBrush(Colors.Firebrick);
            MOT_RECHERCHER.Content = null;
            MOT_RECHERCHER.Foreground = perdu;
            MOT_RECHERCHER.Content = "Perdu ! L'enigme était : ";
            for (int i = 0; i < solution.Length; i++)
            {
                MOT_RECHERCHER.Content += solution[i].ToString();
            }
            DisabledLettres();
            BtnSuivant.Visibility = Visibility.Visible;
            BtnSuivant.Content = "New Game";
            NiveauUn();
            M_aide.IsEnabled = false;
            Outvie();
        }

        // Ouvre la fenetre d'option de la bibliotheque
        private void D_options_Click(object sender, RoutedEventArgs e)
        {
            dico page2 = new dico();
            page2.ShowDialog();
        }

        // Met le niveau à 1
        private void NiveauUn()
        {
            niveau = 1;
            LNiveau.Content = "Niveau : " + niveau;
            pg_bar.Value = 0;
        }

        // Gere la progression de niveau
        private void GainNiveau()
        {
            if (A.IsEnabled)
            {
                pg_bar.Value += 20;
                // Si la barre passe à 100 elle retourne à 0 en ajoutant un niveau
                if (pg_bar.Value == 100)
                {
                    niveau += 1;
                    LNiveau.Content = "Niveau : " + niveau;
                    pg_bar.Value = 0;
                }
            }
        }

        // Active le mode difficile (sans aide)
        private void Hard_Click(object sender, RoutedEventArgs e)
        {
            Hard.IsEnabled = false;
            Easy.IsEnabled = true; 
            M_aide.IsEnabled = false;
            Aide_O = "off";
        }

        // Active le mode facile (avec aide)
        private void Easy_Click(object sender, RoutedEventArgs e)
        {
            Hard.IsEnabled = true;
            Easy.IsEnabled = false;  
            M_aide.IsEnabled = true;
            Aide_O = "on";
        }

        // Verifie le mode de difficulté defini la presente de l'aide
        private void AideDifficulty()
        {
            if (Aide_O == "off")
                M_aide.IsEnabled = false;
            else
                M_aide.IsEnabled = true;
        }


        // Affiche avec un systeme de couleur le nombre de vies restantes

        private void NombreVie(int v)
        {
            if (v >= 1)
            {
                for (int i = 11; i >= v + 1; i--)
                {
                    string lettre = "v" + i;
                    foreach (Rectangle r in this.grid1.Children.OfType<Rectangle>())
                        if (r.Name == lettre)
                            r.Visibility = Visibility.Hidden;
                }
            }
            else
                Outvie();
            
        }

        // Remet le nombre de vies au max
        private void FullVie()
        {
            for (int i = 11; i >= 1; i--)
            {
                foreach (Rectangle r in this.grid1.Children.OfType<Rectangle>())
                    r.Visibility = Visibility.Visible;
            }
        }

        // Met le nombre de vies à 0
        private void Outvie()
        {
            for (int i = 11; i >= 1; i--)
            {
                foreach (Rectangle r in this.grid1.Children.OfType<Rectangle>())
                    r.Visibility = Visibility.Hidden;
            }
        }

        // Reset la couleur de toutes les lettres
        private void ResetColor()
        {
            SolidColorBrush Blanc = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            for (char i = 'A'; i <= 'Z'; i++)
            {
                string lettre = i.ToString();
                foreach (Button b in this.grid1.Children.OfType<Button>())
                    if (b.Name == lettre)
                        b.Background = Blanc;
            }
        }

        // Desactive toutes les lettres
        private void DisabledLettres()
        {
            for (char i = 'A'; i <= 'Z'; i++)
            {
                string lettre = i.ToString();
                foreach (Button b in this.grid1.Children.OfType<Button>())
                    if (b.Name == lettre)
                        b.IsEnabled = false;
            }
        }

        // Active toutes les lettres
        private void EnabledLettres()
        {
            for (char i = 'A'; i <= 'Z'; i++)
            {
                string lettre = i.ToString();
                foreach (Button b in this.grid1.Children.OfType<Button>())
                    if (b.Name == lettre)
                        b.IsEnabled = true;
            }
        }

        // Active le français, active le bouton latin
        private void FR_Click(object sender, RoutedEventArgs e)
        {
            Langue = "Français";
            Langues.Header = "Langues (Français)";
            LA.IsEnabled = true;
            FR.IsEnabled = false;
            NewGame();
        }

        // Active le latin, active le bouton français
        private void LA_Click(object sender, RoutedEventArgs e)
        {
            Langue = "Latin";
            Langues.Header = "Langues (Latin)";
            LA.IsEnabled = false;
            FR.IsEnabled = true;
            NewGame();
        }
    }
}
