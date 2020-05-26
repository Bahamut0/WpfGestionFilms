using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using WpfProjetFilm.Model;
using WpfProjetFilm.ViewModel;

namespace WpfProjetFilm
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        
        private ObservableCollection<Genre> genres;
        private ObservableCollection<Pays> liste_pays;
      
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            genres = new ObservableCollection<Genre>();
            var cb = sender as CheckBox;
            if (cb != null )
            {
                var dtcGenre = cb.DataContext;
                lbxGenre.SelectedItem = dtcGenre;              
                foreach (Genre item in lbxGenre.SelectedItems)
                {
                    genres.Add(item);                 
                }
            }
        }

        // Faire une méthode IsChecked qui permettra de récupérer les sélections des checkbox en fonction du film sélectionné
        private void CheckBox_ClickPays(object sender, RoutedEventArgs e)
        {

            liste_pays = new ObservableCollection<Pays>();
            var cb = sender as CheckBox;
            if (cb != null)
            {
                var dtcPays = cb.DataContext;
                lbxPays.SelectedItem = dtcPays;

                foreach (var item in lbxPays.SelectedItems)
                {
                    liste_pays.Add((Pays)item);
                }

            }
        }

        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            //Deux images ont été définies avec des Z-Index différents.
            // _img permet de choisir l'image source qui n'a pas de valeur nulle 
            string _img;

            if (string.IsNullOrEmpty(ImgFilmAvatar.Source.ToString()) )
            {
                _img = ImgFilm.Source.ToString();
            }
            else
            {
                _img = ImgFilmAvatar.Source.ToString();
            }

            //Objet film nouvellement créé
            Film film = new Film()
            {
                Titre = TbxTitre.Text,
                Synopsis = TbxSynopsis.Text,
                Annee = int.Parse(TbxAnnee.Text),
                Affiche = _img,
                Genre = genres,
                Pays= liste_pays
            };

            //Selection possible à partir d'un combobox
            Genre g = (Genre)CbxGenre.SelectedItem;
            Pays p = (Pays)CbxPays.SelectedItem;

            if (g != null)
            {
                genres.Add(g);
               
            }
            if(p != null)
            {
                liste_pays.Add(p);
            }

            // Vérification
            if (string.IsNullOrEmpty(TbxTitre.Text))
            {
                MessageBox.Show("Le titre est manquant");
            }
            if (string.IsNullOrEmpty(TbxSynopsis.Text))
            {
                MessageBox.Show("Le synopsis est manquant");
            }
            // Ajout du film
            try
            {
                VMProjetFilm.AjoutFilm(film);
            }
            catch (Exception error)
            {

                MessageBox.Show("Le film n'a pas pu être ajouté : " + error);
            }

    }
        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VMProjetFilm.ModifFilm((Film)LbxFilm.SelectedItem);
                

            }
            catch (Exception error)
            {

                MessageBox.Show("Le film n'a pas pu être modifié : "+ error);
            }
            

        }
        private void BtnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VMProjetFilm.Supp((Film)LbxFilm.SelectedItem);
            }
            catch (Exception error)
            {

                MessageBox.Show("Le film n'a pas pu être supprimé : " + error);
            }
            
        }

        private void tbxRechercher_TextChanged(object sender, TextChangedEventArgs e)
        {
           
            if (!string.IsNullOrEmpty(tbxRechercher.Text))
            {
                // selection sans les mots et espaces avants la valeur spécifiée par le combobox
                string selection= cbxRechercher.SelectedValue.ToString().Remove(0,37).Trim();               
                VMProjetFilm.RechercherFilm(tbxRechercher.Text, selection);
            }


        }
        private void ImgFilm_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Fichiers tout types d'image |*.png|*.bmp|*.jpeg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                ImgFilm.Source = new BitmapImage(new Uri(openFileDialog.FileName));

            }
        }

        private void btnImporterFilm_Click(object sender, RoutedEventArgs e)
        {
            VMProjetFilm.ImporterFilm();
        }

        private void btnImporterGenre_Click(object sender, RoutedEventArgs e)
        {
            VMProjetFilm.ImporterGenre();
        }

        private void btnImporterPays_Click(object sender, RoutedEventArgs e)
        {
            VMProjetFilm.ImporterPays();
        }

        private void btnImporterFilmPays_Click(object sender, RoutedEventArgs e)
        {
            VMProjetFilm.ImporterFilmPays();
        }

        private void BtnImporterFilmGenre_Click(object sender, RoutedEventArgs e)
        {
            VMProjetFilm.ImporterFilmGenre();
        }

        private void BtnImporterDrapeau_Click(object sender, RoutedEventArgs e)
        {
            VMProjetFilm.ImporterDrapeau();
        }

        private void tbxRechercher_MouseLeave(object sender, MouseEventArgs e)
        {
            tbxRechercher.Text = "";
        }

        private void LbxFilm_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ImgFilm.Source != null)
            {
                ImgFilmAvatar.Visibility = Visibility.Hidden;
            }
        }

        private void ImgFilmAvatar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Fichiers tout types d'image |*.png|*.bmp|*.jpeg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                ImgFilmAvatar.Source = new BitmapImage(new Uri(openFileDialog.FileName));

            }
        }

       
    }
}
