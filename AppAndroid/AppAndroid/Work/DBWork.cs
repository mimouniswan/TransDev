using System;
using SQLite.Net;
using SQLite.Net.Platform.XamarinAndroid;
using AppAndroid.Data;
using System.Collections.Generic;

namespace AppAndroid.Work
{
    public class DBWork
    {
        private string _RootPath = FileWork.RootPath;
        private string _DirectoryPath = "/Android/data/AppAndroid.AppAndroid/files/";
        private string _DBName = "Transdev.sqlite";
        private string _Path = "";
        private SQLiteConnection _Conn;

        public DBWork()
        {
            _Path = _RootPath + _DirectoryPath + _DBName;
            _Conn = new SQLiteConnection(new SQLitePlatformAndroid(), _Path);
        }

        /// <summary>
        /// Création de toute les tables.
        /// </summary>
        /// <returns></returns>
        public string DBCreateDB()
        {
            string sr = "";

            try
            {
                // Suppression - tables
                _Conn.DropTable<Check>();
                _Conn.DropTable<BusIncident>();
                _Conn.DropTable<Incident>();
                _Conn.DropTable<Bus>();
                _Conn.DropTable<Controleur>();
                _Conn.DropTable<Conducteur>();

                // Création - tables
                _Conn.CreateTable<Conducteur>();
                _Conn.CreateTable<Controleur>();
                _Conn.CreateTable<Bus>();
                _Conn.CreateTable<Incident>();
                _Conn.CreateTable<BusIncident>();
                _Conn.CreateTable<Check>();

                sr = "Tables crée avec succès !\n" + _Path;
                //Debug.WriteLine(_Path);
            }
            catch (Exception e)
            {
                sr = "Impossible de créé les tables";
            }

            return sr;
        }

        public string DBCreateConducteur(string nom, string mdp)
        {
            string sr = "";

            try {
                _Conn.Insert(new Conducteur() { Nom = nom, MdP = mdp });
                sr = "Conducteur créé avec succès";
            }
            catch(Exception e)
            {
                sr = "Une erreur c'est produit : Impossible de créer un conducteur";
            }

            return sr;
        }

        public string DBCreateControleur(string nom)
        {
            string sr = "";

            try
            {
                _Conn.Insert(new Controleur() { Nom = nom });
                sr = "Controleur créé avec succès";
            }
            catch (Exception e)
            {
                sr = "Une erreur c'est produit : Impossible de créer un controleur";
            }

            return sr;
        }

        public string DBCreateBus(int numero, string couleur)
        {
            string sr = "";

            try
            {
                _Conn.Insert(new Bus() { Number = numero, Color = couleur });
                sr = "Bus créé avec succès";
            }
            catch (Exception e)
            {
                sr = "Une erreur c'est produit : Impossible de créer un bus";
            }

            return sr;
        }

        public string DBCreateCheck(Controleur controleur, Conducteur conducteur, Bus bus, List<Incident> incidents)
        {
            string sr = "";

            try
            {
                Check tmpCheck = new Check() { IdBus = bus.Id, IdConducteur = conducteur.Id, IdControleur = controleur.Id, Date = DateTime.Now.ToString() };

                // Liaison avec Incident
                foreach (Incident inc in incidents)
                {
                    _Conn.Insert(inc);
                    _Conn.Insert(new BusIncident() { BusId = bus.Id, IncidentId = inc.Id });
                }

                _Conn.Insert(tmpCheck);

                sr = "Check créé avec succès !";
            }
            catch(Exception e)
            {
                sr = "Une erreur c'est produit : Impossible de créer le check";
            }

            return sr;
        }

        public Incident GetIncident(Controleur controleur, Conducteur conducteur, string type, int gravite, int etat, string dateCreation, string dateMaJ, string observation, int x, int y, string picture)
        {
            return new Incident() { IdConducteur = conducteur.Id, IdCreationCotroleur = controleur.Id, IdMaJControleur = controleur.Id, Type = type, Gravite = gravite, Etat = etat, DateCreation = dateCreation, DateMaJ = dateMaJ, Observation = observation, X = x, Y = y, Picture = picture };
        }

        public string DBSelectConducteur(int id)
        {
            string sr = "", w ="";

            if(id != 0)
                w = $" WHERE Id={id}";

            try
            {
                string q = $"SELECT * FROM Conducteur" + w;

                // Selection - table Message
                var query = _Conn.Query<Conducteur>(q);

                foreach (var item in query)
                {
                    sr += $"{item.Id} : {item.Nom}@{item.MdP}\n";
                    //Debug.WriteLine(sr);
                }
            }
            catch (Exception e)
            {
                sr = "Il n'y a aucune table.";
            }

            return sr;
        }

        public string DBSelectControleur(int id)
        {
            string sr = "", w = "";

            if (id != 0)
                w = $" WHERE Id={id}";

            try
            {
                string q = $"SELECT * FROM Controleur" + w;

                // Selection - table Message
                var query = _Conn.Query<Controleur>(q);

                foreach (var item in query)
                {
                    sr += $"{item.Id} : {item.Nom}\n";
                    //Debug.WriteLine(sr);
                }
            }
            catch (Exception e)
            {
                sr = "Il n'y a aucune table.";
            }

            return sr;
        }

        public string DBSelectBus(int id)
        {
            string sr = "", w = "";

            if (id != 0)
                w = $" WHERE Id={id}";

            try
            {
                string q = $"SELECT * FROM Bus" + w;

                // Selection - table Message
                var query = _Conn.Query<Bus>(q);

                foreach (var item in query)
                {
                    sr += $"{item.Id} : {item.Number}@{item.Color}\n";
                    //Debug.WriteLine(sr);
                }
            }
            catch (Exception e)
            {
                sr = "Il n'y a aucune table.";
            }

            return sr;
        }

        public string DBSelectCheck(int id)
        {
            string sr = "", w = "";

            if (id != 0)
                w = $" WHERE Id={id}";

            try
            {
                string q = $"SELECT * FROM Check" + w;

                // Selection - table Message
                var query = _Conn.Query<Check>(q);

                foreach (var item in query)
                {
                    sr += $"{item.Id} : {item.Date}@{item.Controleur}\n";
                    //Debug.WriteLine(sr);
                }
            }
            catch (Exception e)
            {
                sr = "Il n'y a aucune table.";
            }

            return sr;
        }
    }
}