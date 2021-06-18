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
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace Jeu_Pendu
{
    /// <summary>
    /// Logique d'interaction pour dico.xaml
    /// </summary>
    public partial class dico : Window
    {
        Biblio_Mot_DAO Biblio_Mot_DAO = new Biblio_Mot_DAO("Data Source=dictionnaire_de_mot.db3;");
        Regex ValidMot = new Regex(@"^[a-z]{5,}$");

        public dico()
        {
            InitializeComponent();
        }

        // Variables
        string Langue = "Français";

        // Rafraichie la liste des mots dans la listbox
        private void RefreshList()
        {
            if (Langue == "Français")
            {
                ListeMot.ItemsSource = Biblio_Mot_DAO.F_List();
                ListeMot.DisplayMemberPath = "CompletIdText";
                ListeMot.SelectedValuePath = "id";
            }
            else if (Langue == "Latin")
            {
                ListeMot.ItemsSource = Biblio_Mot_DAO.L_List();
                ListeMot.DisplayMemberPath = "CompletIdText";
                ListeMot.SelectedValuePath = "id";
            }

        }

        // Met à blanc le textbox
        private void TextBoxClear()
        {
            MotText.Text = null;
        }

        // Charge la listbox des mots de la bibliothèque au lancement de la fenetre
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ListeMot.ItemsSource = Biblio_Mot_DAO.F_List();
            ListeMot.DisplayMemberPath = "CompletIdText";
            ListeMot.SelectedValuePath = "id";
        }

        // Envoi le texte du mot selectionné dans le textbox
        private void ListeMot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListeMot.SelectedIndex != -1)
            {
                if (Langue == "Français")
                {
                    Biblio_Mot Bm = new Biblio_Mot();
                    Bm = Biblio_Mot_DAO.F_Find((int)ListeMot.SelectedValue);
                    MotText.Text = Bm.texte;
                }
                else if (Langue == "Latin")
                {
                    Biblio_Mot Bm = new Biblio_Mot();
                    Bm = Biblio_Mot_DAO.L_Find((int)ListeMot.SelectedValue);
                    MotText.Text = Bm.texte; 
                }
            }
        }

        // Ajoute le mot si il est valide et si il n'existe pas déjà dans la bibliothèque
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (ValidMot.IsMatch(MotText.Text))
                {  
                    if (Langue == "Français")
                    {
                        Biblio_Mot Bem = new Biblio_Mot();
                        Bem.texte = MotText.Text;
                        int v = Biblio_Mot_DAO.F_Doublons(Bem);
                        if (v > 0)
                            MessageBox.Show("Impossible : le mot fait déjà partie de la bibliothéque.", "Doublons");
                        else
                        {
                            Biblio_Mot_DAO.F_Insert(Bem);
                            RefreshList();
                            TextBoxClear();
                        }
                    }
                    else if (Langue == "Latin")
                    {
                        Biblio_Mot Bem = new Biblio_Mot();
                        Bem.texte = MotText.Text;
                        int v = Biblio_Mot_DAO.L_Doublons(Bem);
                        if (v > 0)
                            MessageBox.Show("Impossible : le mot fait déjà partie de la bibliothéque.", "Doublons");
                        else
                        {
                            Biblio_Mot_DAO.L_Insert(Bem);
                            RefreshList();
                            TextBoxClear();
                        }
                    }
                }
            else
                TextError.Text = "Le mot que vous voulez ajouter est incorrect. \nSeules les lettres minuscule et sans accent sont valide. \nLe mot doit faire 5 caractères minimum.";
        }

        // Modifie le mot selectionné
        private void BtnPoly_Click(object sender, RoutedEventArgs e)
        {
            if (ValidMot.IsMatch(MotText.Text))
            {
                if (Langue == "Français")
                {
                    int k = (int)ListeMot.SelectedValue;
                    Biblio_Mot Bam = new Biblio_Mot();
                    Bam.id = k;
                    Bam.texte = MotText.Text;
                    int v = Biblio_Mot_DAO.L_Doublons(Bam);
                    if (v > 0)
                        MessageBox.Show("Impossible : le mot fait déjà partie de la bibliothéque.", "Doublons");
                    else
                    {
                        Biblio_Mot_DAO.F_Update(Bam);
                        RefreshList();
                        TextBoxClear();
                    }
                }
                else if (Langue == "Latin")
                {
                    int k = (int)ListeMot.SelectedValue;
                    Biblio_Mot Bam = new Biblio_Mot();
                    Bam.id = k;
                    Bam.texte = MotText.Text;

                    int v = Biblio_Mot_DAO.L_Doublons(Bam);
                    if (v > 0)
                        MessageBox.Show("Impossible : le mot fait déjà partie de la bibliothéque.", "Doublons");
                    else
                    {
                        Biblio_Mot_DAO.L_Update(Bam);
                        RefreshList();
                        TextBoxClear();
                    }
                }
            }
            else
                TextError.Text = "Le mot que vous voulez modifier est incorrect. \nSeules les lettres minuscule et sans accent sont valide. \nLe mot doit faire 5 caractères minimum";
        }

        // Supprime le contenu du textbox error en cas de modification du mot
        private void MotText_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextError.Text = null;
        }

        // Refresh la liste de mot selon la langue choisie
        private void Cb_Langue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Cb_Langue.SelectedIndex == 0)
                Langue = "Français";
            else if (Cb_Langue.SelectedIndex == 1)
                Langue = "Latin";
            RefreshList();
        }
    }
}
