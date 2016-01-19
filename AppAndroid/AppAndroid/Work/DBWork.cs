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
        private string _DBName = "transdev.db";
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
                _Conn.DropTable<CheckUp>();
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
                _Conn.CreateTable<CheckUp>();

                sr = "Tables crée avec succès !\n" + _Path;
                //Debug.WriteLine(_Path);
            }
            catch (Exception)
            {
                sr = "Impossible de créé les tables";
            }

            return sr;
        }

        ////////////////////// OUTILS ///////////////////////////////
        /// <summary>
        /// Créer un objet Incident. Utile pour la création de Check.
        /// </summary>
        /// <param name="IdControleur">ID du controleur.</param>
        /// <param name="IdConducteur">ID du conducteur.</param>
        /// <param name="type">Le type d'incident.</param>
        /// <param name="gravite">la gravité de l'incident.</param>
        /// <param name="etat">L'état de l'incident.</param>
        /// <param name="dateCreation">La date de création de l'incident.</param>
        /// <param name="observation">Des observation.</param>
        /// <param name="x">Position X.</param>
        /// <param name="y">Position Y.</param>
        /// <param name="picture">Chemin de l'image.</param>
        /// <returns></returns>
        public Incident CreateAndGetIncident(int IdControleur, int IdConducteur, string type, int gravite, int etat, string dateCreation, string observation, int x, int y, string picture)
        {
            return new Incident() { IdConducteur = IdControleur, IdCreationCotroleur = IdConducteur, IdMaJControleur = IdConducteur, Type = type, Gravite = gravite, Etat = etat, DateCreation = dateCreation, DateMaJ = dateCreation, Observation = observation, X = x, Y = y, Picture = picture };
        }

        /// <summary>
        /// Récupère un objet Bus.
        /// </summary>
        /// <param name="id">ID du bus que l'on veut récupérer.</param>
        /// <returns></returns>
        public List<Bus> GetBus(int id = 0)
        {
            List<Bus> result = new List<Bus>();
            string w = "";

            if (id != 0)
                w = $"WHERE Id={id}";

            try
            {
                string q = $"SELECT * FROM Bus {w}";

                // Selection - table Message
                result = _Conn.Query<Bus>(q);
            }
            catch (Exception) { }

            return result;
        }

        /// <summary>
        /// Récupère un objet Conducteur.
        /// </summary>
        /// <param name="id">ID du conducteur que l'on veut récupérer.</param>
        /// <returns></returns>
        public List<Conducteur> GetConducteur(int id = 0)
        {
            List<Conducteur> result = new List<Conducteur>();
            string w = "";

            if (id != 0)
                w = $"WHERE Id={id}";

            try
            {
                string q = $"SELECT * FROM Conducteur {w}";

                // Selection - table Message
                result = _Conn.Query<Conducteur>(q);
            }
            catch (Exception) { }

            return result;
        }

        /// <summary>
        /// Récupère un objet Controleur.
        /// </summary>
        /// <param name="id">ID du controleur que l'on veut récupérer.</param>
        /// <returns></returns>
        public List<Controleur> GetControleur(int id = 0)
        {
            List<Controleur> result = new List<Controleur>();
            string w = "";

            if (id != 0)
                w = $"WHERE Id={id}";

            try
            {
                string q = $"SELECT * FROM Controleur {w}";

                // Selection - table Message
                result = _Conn.Query<Controleur>(q);
            }
            catch (Exception) { }

            return result;
        }

        ////////////////////// INSERT ///////////////////////////////
        /// <summary>
        /// Créer un coducteur en BDD.
        /// </summary>
        /// <param name="nom">Nom du conducteur.</param>
        /// <param name="mdp">Mot de passe.</param>
        /// <returns></returns>
        public string DBInsertConducteur(string nom, string mdp)
        {
            string sr = "";

            try {
                _Conn.Insert(new Conducteur() { Nom = nom, MdP = mdp });
                sr = "Conducteur créé avec succès";
            }
            catch(Exception)
            {
                sr = "Une erreur c'est produit : Impossible de créer un conducteur";
            }

            return sr;
        }

        /// <summary>
        /// Créer un controleur en BDD.
        /// </summary>
        /// <param name="nom">Nom du controleur.</param>
        /// <returns></returns>
        public string DBInsertControleur(string nom)
        {
            string sr = "";

            try
            {
                _Conn.Insert(new Controleur() { Nom = nom });
                sr = "Controleur créé avec succès";
            }
            catch (Exception)
            {
                sr = "Une erreur c'est produit : Impossible de créer un controleur";
            }

            return sr;
        }

        /// <summary>
        /// Créer un bus en BDD.
        /// </summary>
        /// <param name="numero">Numéro du bus.</param>
        /// <param name="couleur">Couleur du bus.</param>
        /// <returns></returns>
        public string DBInsertBus(int numero, string couleur)
        {
            string sr = "";

            try
            {
                _Conn.Insert(new Bus() { Number = numero, Color = couleur });
                sr = "Bus créé avec succès";
            }
            catch (Exception)
            {
                sr = "Une erreur c'est produit : Impossible de créer un bus";
            }

            return sr;
        }

        /// <summary>
        /// Créer un check en BDD.
        /// </summary>
        /// <param name="IdControleur">ID du controleur.</param>
        /// <param name="IdConducteur">ID du conducteur.</param>
        /// <param name="IdBus">ID du bus.</param>
        /// <param name="incidents">Liste des incidents.</param>
        /// <returns></returns>
        public string DBInsertCheck(int IdControleur, int IdConducteur, int IdBus, List<Incident> incidents)
        {
            string sr = "";

            try
            {
                CheckUp tmpCheck = new CheckUp() { IdBus = IdBus, IdConducteur = IdConducteur, IdControleur = IdControleur, Date = DateTime.Now.ToString() };
                _Conn.Insert(tmpCheck);

                // Liaison avec Incident
                foreach (Incident inc in incidents)
                {
                    _Conn.Insert(inc);
                    _Conn.Insert(new BusIncident() { IdCheck = tmpCheck.Id, IdBus = IdBus, IdIncident = inc.Id });
                }

                sr = "Check créé avec succès !";
            }
            catch(Exception)
            {
                sr = "Une erreur c'est produit : Impossible de créer le check";
            }

            return sr;
        }

        ////////////////////// SELECT ///////////////////////////////
        /// <summary>
        /// Récupère les informations d'un conducteur.
        /// </summary>
        /// <param name="id">ID du conducteur.</param>
        /// <returns></returns>
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
            catch (Exception)
            {
                sr = "Il n'y a aucune table.";
            }

            return sr;
        }

        /// <summary>
        /// Récupère les informations d'un controleur.
        /// </summary>
        /// <param name="id">ID du controleur.</param>
        /// <returns></returns>
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
            catch (Exception)
            {
                sr = "Il n'y a aucune table.";
            }

            return sr;
        }

        /// <summary>
        /// Récupère les informations d'un bus.
        /// </summary>
        /// <param name="id">ID du bus.</param>
        /// <returns></returns>
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
            catch (Exception)
            {
                sr = "Il n'y a aucune table.";
            }

            return sr;
        }

        /// <summary>
        /// Récupère les informations d'un check.
        /// </summary>
        /// <param name="id">ID du check.</param>
        /// <returns></returns>
        public string DBSelectCheck(int id)
        {
            string sr = "", w = "";

            try
            {
                var checkTmp = _Conn.Find<CheckUp>(id);
                var busIncidentTmp = _Conn.Query<BusIncident>($"SELECT * FROM BusIncident WHERE IdCheck={checkTmp.Id}");
                var busTmp = _Conn.Query<Bus>($"SELECT * FROM Bus WHERE Id={busIncidentTmp[0].IdBus}");

                int i = 0;
                foreach (var item in busIncidentTmp)
                {
                    if (i > 0)
                        w += "OR ";
                    else
                        w += "WHERE ";

                    w += $"Id={item.IdIncident} ";
                    i++;
                }

                var incidentTmp = _Conn.Query<Incident>($"SELECT * FROM Incident {w}");

                sr += $"{checkTmp.Id} : Bus[{busTmp[0].Id}],\n";

                foreach (var item in incidentTmp)
                {
                    sr += $"Incident[{item.Id}],\n";
                    //Debug.WriteLine(sr);
                }
            }
            catch (Exception)
            {
                sr = "Il n'y a aucune table.";
            }

            return $"{sr}";
        }

        ////////////////////// DELETE ///////////////////////////////
        /// <summary>
        /// Suppression d'un conducteur.
        /// </summary>
        /// <param name="id">ID du conducteur.</param>
        /// <returns></returns>
        public string DBDeleteConducteur(int id)
        {
            string sr = "";

            try
            {
                var query = _Conn.Delete<Conducteur>(id);

                sr = $"Suppression du conducteur d'ID : {id}\n";
            }
            catch (Exception)
            {
                sr = "Il n'y a aucune table.";
            }

            return sr;
        }

        /// <summary>
        /// Suppression d'un controleur.
        /// </summary>
        /// <param name="id">ID du controleur</param>
        /// <returns></returns>
        public string DBDeleteControleur(int id)
        {
            string sr = "";

            try
            {
                var query = _Conn.Delete<Controleur>(id);

                sr = $"Suppression du controleur d'ID : {id}\n";
            }
            catch (Exception)
            {
                sr = "Il n'y a aucune table.";
            }

            return sr;
        }

        /// <summary>
        /// Suppression d'un bus.
        /// </summary>
        /// <param name="id">ID du bus.</param>
        /// <returns></returns>
        public string DBDeleteBus(int id)
        {
            string sr = "";

            try
            {
                var query = _Conn.Delete<Bus>(id);

                sr = $"Suppression du bus d'ID : {id}\n";
            }
            catch (Exception)
            {
                sr = "Il n'y a aucune table.";
            }

            return sr;
        }

        /// <summary>
        /// Supprimme le check, les liaisons entre le bus et les incidents et les incidents. Ne supprime pas le bus.
        /// </summary>
        /// <param name="id">ID du check.</param>
        /// <returns></returns>
        public string DBDeleteCheck(int id)
        {
            string sr = "", w ="";

            try
            {
                var checkTmp = _Conn.Find<CheckUp>(id);
                var busIncidentTmp = _Conn.Query<BusIncident>($"SELECT * FROM BusIncident WHERE IdCheck={checkTmp.Id}"); 
                var busTmp = _Conn.Query<Bus>($"SELECT * FROM Bus WHERE Id={busIncidentTmp[0].IdBus}");

                int i = 0;
                foreach (var item in busIncidentTmp)
                {
                    if (i > 0)
                        w += "OR ";
                    else
                        w += "WHERE ";

                    w += $"Id={item.IdIncident} ";
                    i++;
                }

                var incidentTmp = _Conn.Query<Incident>($"SELECT * FROM Incident {w}");

                // SUPPRESSION
                _Conn.Delete(checkTmp);

                foreach(var item in busIncidentTmp)
                    _Conn.Delete(item);

                foreach (var item in incidentTmp)
                    _Conn.Delete(item);

                sr = $"Suppression du check d'ID : {id}\n";
            }
            catch (Exception)
            {
                sr = "Il n'y a aucune table.";
            }

            return $"{sr}";
        }

        ////////////////////// UPDATE ///////////////////////////////
        /// <summary>
        /// Mise à jour d'un conducteur.
        /// </summary>
        /// <param name="id">ID du conducteur.</param>
        /// <returns></returns>
        public string DBUpdateConducteur(int id, string nom, string mdp)
        {
            string sr = "";

            try
            {
                var query = _Conn.Query<Conducteur>($"UPDATE Conducteur SET Nom = \"{nom}\", MdP=\"{mdp}\" WHERE ID = {id}");

                sr = $"Mise à jour du conducteur d'ID : {id}\n";
            }
            catch (Exception)
            {
                sr = "Il n'y a aucune table.";
            }

            return sr;
        }

        /// <summary>
        /// Mise à jour d'un controleur.
        /// </summary>
        /// <param name="id">ID du controleur</param>
        /// <returns></returns>
        public string DBUpdateControleur(int id, string nom)
        {
            string sr = "";

            try
            {
                var query = _Conn.Query<Controleur>($"UPDATE Controleur SET Nom = \"{nom}\" WHERE ID = {id}");

                sr = $"Mise à jour du controleur d'ID : {id}\n";
            }
            catch (Exception)
            {
                sr = "Il n'y a aucune table.";
            }

            return sr;
        }

        /// <summary>
        /// Mise à jour d'un bus.
        /// </summary>
        /// <param name="id">ID du bus.</param>
        /// <returns></returns>
        public string DBUpdateBus(int id, int numero, string couleur)
        {
            string sr = "";

            try
            {
                var query = _Conn.Query<Bus>($"UPDATE Bus SET Number = {numero}, Color=\"{couleur}\" WHERE ID = {id}");

                sr = $"Mise à jour du bus d'ID : {id}\n";
            }
            catch (Exception)
            {
                sr = "Il n'y a aucune table.";
            }

            return sr;
        }

        /// <summary>
        /// Met à jour d'un check.
        /// </summary>
        /// <param name="id">ID du check.</param>
        /// <returns></returns>
        public string DBUpdateCheck(int id, string date)
        {
            string sr = "";

            try
            {
                var query = _Conn.Query<CheckUp>($"UPDATE CheckUp SET Date = \"{date}\" WHERE ID = {id}");

                sr = $"Mise à jour du check d'ID : {id}\n";
            }
            catch (Exception)
            {
                sr = "Il n'y a aucune table.";
            }

            return sr;
        }

        /// <summary>
        /// Met à jour d'un incident.
        /// </summary>
        /// <param name="id">ID de l'incident.</param>
        /// <returns></returns>
        public string DBUpdateIncident(int id, string type, int gravite, int etat, string dateMaj, string observation, int x, int y, string picture)
        {
            string sr = "";

            try
            {
                var query = _Conn.Query<Incident>($"UPDATE Incident SET Type = \"{type}\", Gravite = {gravite}, Etat = {etat}, DateMaJ = \"{dateMaj}\", Observation = \"{observation}\", X = {x}, Y = {y}, Picture = \"{picture}\" WHERE ID = {id}");

                sr = $"Mise à jour de l'incident d'ID : {id}\n";
            }
            catch (Exception)
            {
                sr = "Il n'y a aucune table.";
            }

            return sr;
        }
    }
}