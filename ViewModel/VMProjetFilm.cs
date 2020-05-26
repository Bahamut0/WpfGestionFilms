using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WpfProjetFilm.Model;

namespace WpfProjetFilm.ViewModel
{
    class VMProjetFilm  : IDisposable
        
    {
        private static EFProjetFilmContainer dtc;

        private static ObservableCollection<Film> liste_films;
        private static ObservableCollection<Pays> liste_pays;
        private static ObservableCollection<Genre> liste_genre;
        

        public ObservableCollection<Film> Films { get => liste_films; set => liste_films = value; }
        public ObservableCollection<Pays> PaysListe { get => liste_pays; set => liste_pays = value; }
        public ObservableCollection<Genre> Genres { get => liste_genre; set => liste_genre = value; }


        public VMProjetFilm()
        {
            if (dtc == null)
            {
                dtc = new EFProjetFilmContainer();
            }
            if (liste_films == null)
            {
                liste_films = new ObservableCollection<Film>(dtc.film);
            }
            if (liste_genre == null)
            {
                liste_genre = new ObservableCollection<Genre>(dtc.genre);
            }
            if (liste_pays == null)
            {
                liste_pays = new ObservableCollection<Pays>(dtc.pays);
            }
        }

        public static void AjoutFilm(Film film)
        {
            dtc.film.Add(film);
            liste_films.Add(film);
            int i = dtc.SaveChanges();
        }
        public static void ModifFilm(Film f)
        {
           
            Film filmAmodifier = dtc.film.Where(x => x.Id == f.Id).SingleOrDefault();
            filmAmodifier.Titre = f.Titre;
            filmAmodifier.Affiche = f.Affiche;
            filmAmodifier.Annee = f.Annee;
            filmAmodifier.Genre = f.Genre;
            filmAmodifier.Pays = f.Pays;
            dtc.SaveChanges();
            
        }
        public static void Supp(Film f)
        {
            
            Film filmAsupprimer = dtc.film.Where(t => t.Id == f.Id).SingleOrDefault();
            dtc.film.Remove(filmAsupprimer);
            dtc.SaveChanges();
            liste_films.Remove(f);
            
        }
        public static ObservableCollection<Film> RechercherFilm(string critere, string selection)
        {
            var listeARetourner = new ObservableCollection<Film>();
            var listeARetournerGenre = new ObservableCollection<Genre>();
            switch (selection)
            {
               
                case "Titre":                  
                    var _rechercheParTitre = dtc.film.Where(x => x.Titre.ToLower().Contains(critere)).ToList();
                    listeARetourner = new ObservableCollection<Film> (_rechercheParTitre);
                    break;
                case "Synopsis":                   
                    var _rechercheParSynopsis = dtc.film.Where(x => x.Synopsis.ToLower().Contains(critere)).ToList();
                    listeARetourner = new ObservableCollection<Film>(_rechercheParSynopsis);
                    break;
                case "Année":
                    int _code;
                    if (int.TryParse(critere, out _code)) 
                    {                      
                        if (_code != 0)
                        {
                            var _rechercheParAnnee = dtc.film.Where(x => x.Annee == _code).ToList();
                            listeARetourner = new ObservableCollection<Film>(_rechercheParAnnee);
                        }
                    }                   
                    break;
                /*    
               case "Genre":

                    //faire une première requête où l'on sélectionne le libelle des genre comme comparaison de critère de recherche
                   
                    var filmGenre = dtc.film.ToList().Join(dtc.genre.ToList(),
                                            film => film.Id,
                                            act => act.Id,
                                            (fm, gr) => new { fm.Id, fm.Titre, fm.Synopsis, fm.Genre, gr.Libelle }
                                            );

                    var filmGenre2 = from unFilm in dtc.film
                                                join unFilmGenre in dtc.genre
                                                on unFilm.Id equals unFilmGenre.Id
                                                select new { unFilm.Titre, unFilm.Synopsis, unFilm.Annee, unFilmGenre.Id }
                                            ;
                    var _listGenreARetourner = dtc.film.ToList().Join(dtc.genre.ToList(),
                                               film => film.Id,
                                               genre => genre.Id,
                                               (fl, ge) => new { fl.Id, ge.Libelle }
                                               ).Where(x => x.Libelle.ToLower().Contains(critere)).ToList();


                    listeARetourner = new ObservableCollection<Film>(filmGenre.Where(x => x.Libelle.ToLower().Contains(critere)).ToList());
                    
                    break;
                    */
                    
                default:
                    break;
            }
            liste_films.Clear();

                foreach (var item in listeARetourner)
                {
                    liste_films.Add(item);
                }
             
            return listeARetourner;

        }
        public static List<Film> ImporterFilm()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            EFProjetFilmContainer dtx = new EFProjetFilmContainer();
            List<Film> _films = new List<Film>();
            ofd.Title = "Selectionner le fichier à chgarger";
            if (ofd.ShowDialog() == true)
            {
                StreamReader stream = new StreamReader(ofd.FileName, Encoding.Default);
                string contenu = stream.ReadToEnd();
               
                string[] lignes = contenu.Split('\n');
                stream.Close();
                stream.Dispose();
                for (int i = 0; i < lignes.Length; i++)
                {
                    // pour chaque ligne, on récupère les cellules
                    string[] cellules = lignes[i].Split('|'); 
                                                              
                    if (cellules.Length == 5) // on a une ligne valide
                    {

                        Film f = new Film()
                        {
                            Titre = cellules[1],
                            Synopsis = cellules[2],
                            Annee = int.Parse(cellules[3]),

                        };
                        dtx.film.Add(f);
                    }
                }
                dtx.SaveChanges();
                _films = dtx.film.ToList();
            }
            dtx.Dispose();

            return _films;
        }
        public static void ImporterGenre()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            EFProjetFilmContainer dtx = new EFProjetFilmContainer();
            if (ofd.ShowDialog() == true)
            {
                StreamReader stream = new StreamReader(ofd.FileName, Encoding.Default);

                string contenu = stream.ReadToEnd();
                string[] lignes = contenu.Split('\n');

                for (int i = 0; i < lignes.Length; i++)
                {
                    string[] cellules = lignes[i].Split('|');
                  
                    if (cellules.Length == 2)
                    {
                        Genre _g = new Genre()
                        {
                            Libelle = cellules[1]
                        };
                        dtx.genre.Add(_g);

                    }

                }

            }
            dtx.SaveChanges();
            dtx.Dispose();
        }
        public static void ImporterFilmGenre()
        {
            {
                OpenFileDialog ofd = new OpenFileDialog();
                EFProjetFilmContainer dtx = new EFProjetFilmContainer();
                if (ofd.ShowDialog() == true)
                {
                    StreamReader stream = new StreamReader(ofd.FileName, Encoding.Default);

                    string contenu = stream.ReadToEnd();
                    string[] lignes = contenu.Split('\n');

                    for (int i = 0; i < lignes.Length; i++)
                    {
                        string[] cellules = lignes[i].Split('|');
                        //   MessageBox.Show(cellules.Length + "");
                      
                        if (cellules.Length == 2)
                        {
                            int k = int.Parse(cellules[0]);
                            int j = int.Parse(cellules[1]);
                            
                            Film f = dtx.film.Where(x => x.Id == k).SingleOrDefault();
                            Genre g = dtx.genre.Where(x => x.Id ==  j).SingleOrDefault();
                            f.Genre.Add(g);
                            g.Film.Add(f);
                            dtx.SaveChanges(); 
                        }

                    }

                }
                dtx.SaveChanges();
                dtx.Dispose();
            }
     
        }
        public static List<Pays> ImporterPays()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            EFProjetFilmContainer dtx = new EFProjetFilmContainer();
            List<Pays> _pays = new List<Pays>();
            ofd.Title = "Selectionner le fichier à chgarger";
            if (ofd.ShowDialog() == true)
            {
                StreamReader stream = new StreamReader(ofd.FileName, Encoding.Default);
                string contenu = stream.ReadToEnd();
                
                string[] lignes = contenu.Split('\n');
                stream.Close();
                stream.Dispose();
                for (int i = 0; i < lignes.Length; i++)
                {
                    
                    string[] cellules = lignes[i].Split('|');
                                                              
                    if (cellules.Length >=2) 
                    {
                       // Drapeau drapeau= dtx.drapeau.Where(x => x.Id == int.Parse(cellules[2])).SingleOrDefault();

                        Pays p = new Pays()
                        {
                                          
                            Id = int.Parse(cellules[0]),
                            Libelle = cellules[1],
                            // Drapeau = drapeau

                        };
                        dtx.pays.Add(p);
                    }
                }
                dtx.SaveChanges();
                _pays = dtx.pays.ToList();
            }
            dtx.Dispose();

            return _pays;
        }
        public static void ImporterFilmPays()
        {
            {
                OpenFileDialog ofd = new OpenFileDialog();
                EFProjetFilmContainer dtx = new EFProjetFilmContainer();
                if (ofd.ShowDialog() == true)
                {
                    StreamReader stream = new StreamReader(ofd.FileName, Encoding.Default);

                    string contenu = stream.ReadToEnd();
                    string[] lignes = contenu.Split('\n');

                    for (int i = 0; i < lignes.Length; i++)
                    {
                        string[] cellules = lignes[i].Split('|');
                    

                        if (cellules.Length == 2)
                        {
                           
                            int k = int.Parse(cellules[0]);
                            int j = int.Parse(cellules[1]);
                            Film f = dtx.film.Where(x => x.Id == k).SingleOrDefault();
                            Pays p = dtx.pays.Where(x => x.Id == j).SingleOrDefault();
                            
                                f.Pays.Add(p);
                                p.Film.Add(f);
                                dtx.SaveChanges();
                                                         
                        }

                    }

                }
                dtx.SaveChanges();
                dtx.Dispose();
            }

        }
        public static void ImporterDrapeau()
        {
            {
                OpenFileDialog ofd = new OpenFileDialog();
                EFProjetFilmContainer dtx = new EFProjetFilmContainer();
                if (ofd.ShowDialog() == true)
                {
                    StreamReader stream = new StreamReader(ofd.FileName, Encoding.Default);

                    string contenu = stream.ReadToEnd();
                    string[] lignes = contenu.Split('\n');
                    

                    for (int i = 0; i < lignes.Length; i++)
                    {
                        string[] cellules = lignes[i].Split('|');
                        //   MessageBox.Show(cellules.Length + "");
                        if (cellules.Length == 2)
                        {
                            
                            Drapeau _g = new Drapeau()
                            {
                                Id = int.Parse(cellules[0]),
                                CheminImage = cellules[1],
                                //Pays = dtx.pays.Where(x => x.Id == int.Parse(cellules[0])).SingleOrDefault()
                        };
                            dtx.drapeau.Add(_g);

                        }

                    }

                }
                dtx.SaveChanges();
                dtx.Dispose();
            }

        }
        public void Dispose()
        {
            dtc.Dispose();
        }
        
    }
}
