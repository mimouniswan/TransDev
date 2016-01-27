using System;
using SQLite.Net;
using SQLite.Net.Platform.XamarinAndroid;
using SQLiteNetExtensions.Extensions;
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
        /// Cr�ation de toute les tables.
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
                _Conn.DropTable<Model>();
                _Conn.DropTable<Controleur>();
                _Conn.DropTable<Conducteur>();

                // Cr�ation - tables
                _Conn.CreateTable<Conducteur>();
                _Conn.CreateTable<Controleur>();
                _Conn.CreateTable<Bus>();
                _Conn.CreateTable<Model>();
                _Conn.CreateTable<Incident>();
                _Conn.CreateTable<BusIncident>();
                _Conn.CreateTable<CheckUp>();

                sr = "Tables cr�e avec succ�s !\n" + _Path;
                //Debug.WriteLine(_Path);
            }
            catch (Exception)
            {
                sr = "Impossible de cr�� les tables";
            }

            return sr;
        }

        ////////////////////// OUTILS ///////////////////////////////
        /// <summary>
        /// Cr�er un objet Incident. Utile pour la cr�ation de Check.
        /// </summary>
        /// <param name="IdControleur">ID du controleur.</param>
        /// <param name="IdConducteur">ID du conducteur.</param>
        /// <param name="type">Le type d'incident.</param>
        /// <param name="gravite">la gravit� de l'incident.</param>
        /// <param name="etat">L'�tat de l'incident.</param>
        /// <param name="dateCreation">La date de cr�ation de l'incident.</param>
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
        /// R�cup�re un objet Bus.
        /// </summary>
        /// <param name="id">ID du bus que l'on veut r�cup�rer.</param>
        /// <returns></returns>
        public List<Bus> GetBus(int id = 0)
        {
            List<Bus> result = new List<Bus>();
            string w = "";

            if (id != 0) w = $"WHERE Id={id}";

            try
            {
                string q = $"SELECT * FROM Bus {w}";

                result = _Conn.Query<Bus>(q);
            }
            catch (Exception) { }

            return result;
        }

        /// <summary>
        /// R�cup�re un objet Mod�le.
        /// </summary>
        /// <param name="id">ID du model que l'on veut r�cup�rer.</param>
        /// <returns></returns>
        public List<Model> GetModel(int id = 0)
        {
            List<Model> result = new List<Model>();
            string w = "";

            if (id != 0) w = $"WHERE Id={id}";

            try
            {
                string q = $"SELECT * FROM Model {w}";

                result = _Conn.Query<Model>(q);
            }
            catch (Exception) { }

            return result;
        }

        /// <summary>
        /// R�cup�re un objet Conducteur.
        /// </summary>
        /// <param name="id">ID du conducteur que l'on veut r�cup�rer.</param>
        /// <returns></returns>
        public List<Conducteur> GetConducteur(int id = 0)
        {
            List<Conducteur> result = new List<Conducteur>();
            string w = "";

            if (id != 0) w = $"WHERE Id={id}";

            try
            {
                string q = $"SELECT * FROM Conducteur {w}";

                result = _Conn.Query<Conducteur>(q);
            }
            catch (Exception) { }

            return result;
        }

        /// <summary>
        /// R�cup�re un objet Controleur.
        /// </summary>
        /// <param name="id">ID du controleur que l'on veut r�cup�rer.</param>
        /// <returns></returns>
        public List<Controleur> GetControleur(int id = 0)
        {
            List<Controleur> result = new List<Controleur>();
            string w = "";

            if (id != 0) w = $"WHERE Id={id}";

            try
            {
                string q = $"SELECT * FROM Controleur {w}";

                result = _Conn.Query<Controleur>(q);
            }
            catch (Exception) { }

            return result;
        }

        /// <summary>
        /// R�cup�re une liste de tableau de string pour l'historique des incidents.
        /// </summary>
        /// <param name="desc">True pour faire une s�lection descendante.</param>
        /// <returns></returns>
        public List<string[]> GetHistIncident(bool desc = false)
        {
            List<string[]> results = new List<string[]>();

            string o = "";
            if (desc) o = "Desc"; else o = "Asc";

            try
            {
                List<Incident> incidents = _Conn.Query<Incident>($"SELECT * FROM Incident ORDER BY DateMaJ {o}");
                List<BusIncident> busIncidents = new List<BusIncident>();
                List<Bus> bus = new List<Bus>();
                List<Conducteur> conducteurs = new List<Conducteur>();

                foreach (var item in incidents)
                {
                    conducteurs = _Conn.Query<Conducteur>($"SELECT* FROM Conducteur WHERE Id = {item.IdConducteur}");

                    busIncidents = _Conn.Query<BusIncident>($"SELECT* FROM BusIncident WHERE IdIncident = {item.Id}");

                    bus = _Conn.Query<Bus>($"SELECT* FROM Bus WHERE Id = {busIncidents[0].IdBus}");

                    string[] r = new string[4] { bus[0].Number.ToString(), item.Observation, conducteurs[0].Name, item.DateMaJ };

                    results. Add(r);
                }
            }
            catch(Exception) { }

            return results;
        }

        #region Insert

        ////////////////////// INSERT ///////////////////////////////
        /// <summary>
        /// Cr�er un conducteur en BDD.
        /// </summary>
        /// <param name="nom">Nom du conducteur.</param>

        /// <returns></returns>
        public string DBInsertConducteur(string nom)
        {
            string sr = "";

            try {
                _Conn.Insert(new Conducteur() { Name = nom });
                sr = "Conducteur cr�� avec succ�s";
            }
            catch(Exception)
            {
                sr = "Une erreur c'est produit : Impossible de cr�er un conducteur";
            }

            return sr;
        }

        /// <summary>
        /// Cr�er un controleur en BDD.
        /// </summary>
        /// <param name="nom">Nom du controleur.</param>
        /// <param name="mdp">Mot de passe.</param>
        /// <returns></returns>
        public string DBInsertControleur(string nom, string mdp)
        {
            string sr = "";

            try
            {
                _Conn.Insert(new Controleur() { Name = nom, MdP = mdp });
                sr = "Controleur cr�� avec succ�s";
            }
            catch (Exception)
            {
                sr = "Une erreur c'est produit : Impossible de cr�er un controleur";
            }

            return sr;
        }

        /// <summary>
        /// Cr�er un bus en BDD.
        /// </summary>
        /// <param name="numero">Num�ro du bus.</param>
        /// <param name="couleur">Couleur du bus.</param>
        /// <returns></returns>
        public string DBInsertBus(int numero, string couleur, int idModel)
        {
            string sr = "";

            try
            {
                _Conn.Insert(new Bus() { Number = numero, Color = couleur, IdModel = idModel});
            }
            catch (Exception)
            {
                sr = "Une erreur c'est produit : Impossible de cr�er un bus";
            }

            return sr;
        }

        /// <summary>
        /// Cr�er un check en BDD.
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

                sr = "Check cr�� avec succ�s !";
            }
            catch(Exception)
            {
                sr = "Une erreur c'est produit : Impossible de cr�er le check";
            }

            return sr;
        }

        /// <summary>
        /// Cr�er un mod�le de bus en BDD.
        /// </summary>
        /// <param name="name">nom du mod�le.</param>
        /// <returns></returns>
        public string DBInsertModel(string name)
        {
            string sr = "";

            try
            {
                _Conn.Insert(new Model() { Name = name });
                sr = "Mod�le cr�� avec succ�s";
            }
            catch (Exception)
            {
                sr = "Une erreur c'est produit : Impossible de cr�er un mod�le";
            }

            return sr;
        }
        #endregion

        #region Select

        ////////////////////// SELECT ///////////////////////////////
        /// <summary>
        /// R�cup�re les informations d'un conducteur.
        /// </summary>
        /// <param name="id">ID du conducteur.</param>
        /// <returns></returns>
        public string DBSelectConducteur(int id)
        {
            string sr = "", w ="";

            if(id != 0) w = $" WHERE Id={id}";

            try
            {
                string q = $"SELECT * FROM Conducteur" + w;

                var query = _Conn.Query<Conducteur>(q);

                foreach (var item in query)
                    sr += $"{item.Id} : {item.Name}\n";
            }
            catch (Exception)
            {
                sr = "Il n'y a aucune table.";
            }

            return sr;
        }

        /// <summary>
        /// R�cup�re les informations d'un controleur.
        /// </summary>
        /// <param name="id">ID du controleur.</param>
        /// <returns></returns>
        public string DBSelectControleur(int id)
        {
            string sr = "", w = "";

            if (id != 0) w = $" WHERE Id={id}";

            try
            {
                string q = $"SELECT * FROM Controleur" + w;

                var query = _Conn.Query<Controleur>(q);

                foreach (var item in query)
                    sr += $"{item.Id} : {item.Name}@{item.MdP}\n";
            }
            catch (Exception)
            {
                sr = "Il n'y a aucune table.";
            }

            return sr;
        }

        /// <summary>
        /// R�cup�re les informations d'un bus.
        /// </summary>
        /// <param name="id">ID du bus.</param>
        /// <returns></returns>
        public string DBSelectBus(int id)
        {
            string sr = "", w = "";

            if (id != 0) w = $" WHERE Id={id}";

            try
            {
                string q = $"SELECT * FROM Bus" + w;

                var query = _Conn.Query<Bus>(q);

                foreach (var item in query)
                    sr += $"{item.Id} : {item.Number}@{item.Color}\n";
            }
            catch (Exception)
            {
                sr = "Il n'y a aucune table.";
            }

            return sr;
        }

        /// <summary>
        /// R�cup�re les informations d'un check.
        /// </summary>
        /// <param name="id">ID du check.</param>
        /// <returns></returns>
        public string DBSelectCheck(int id)
        {
            string sr = "", w = "";

            try
            {
                var checkTmp = _Conn.Query<CheckUp>($"SELECT * FROM CheckUp WHERE Id={id}");
                var busIncidentTmp = _Conn.Query<BusIncident>($"SELECT * FROM BusIncident WHERE IdCheck={id}");
                var busTmp = _Conn.Query<Bus>($"SELECT * FROM Bus WHERE Id={busIncidentTmp[0].IdBus}");

                int i = 0;
                foreach (var item in busIncidentTmp)
                {
                    if (i > 0) w += "OR ";
                    else w += "WHERE ";

                    w += $"Id={item.IdIncident} ";
                    i++;
                }

                var incidentTmp = _Conn.Query<Incident>($"SELECT * FROM Incident {w}");

                sr += $"{id} : Bus[{busTmp[0].Id}],\n";

                foreach (var item in incidentTmp)
                    sr += $"Incident[{item.Id}],\n";
            }
            catch (Exception)
            {
                sr = "Il n'y a aucune table.";
            }

            return $"{sr}";
        }

        /// <summary>
        /// R�cup�re les informations d'un mod�le.
        /// </summary>
        /// <param name="id">ID du mod�le.</param>
        /// <returns></returns>
        public string DBSelectModel(int id)
        {
            string sr = "", w = "";

            if (id != 0) w = $" WHERE Id={id}";

            try
            {
                string q = $"SELECT * FROM Model" + w;

                var query = _Conn.Query<Model>(q);

                foreach (var item in query)
                    sr += $"{item.Id} : {item.Name}\n";
            }
            catch (Exception)
            {
                sr = "Il n'y a aucune table.";
            }

            return sr;
        }



        #endregion

        #region Delete

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
                    if (i > 0) w += "OR ";
                    else w += "WHERE ";

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

        /// <summary>
        /// Suppression d'un mod�le.
        /// </summary>
        /// <param name="id">ID du mod�le.</param>
        /// <returns></returns>
        public string DBDeleteModel(int id)
        {
            string sr = "";

            try
            {
                var query = _Conn.Delete<Model>(id);

                sr = $"Suppression du mod�le d'ID : {id}\n";
            }
            catch (Exception)
            {
                sr = "Il n'y a aucune table.";
            }

            return sr;
        }

        #endregion

        #region Update

        ////////////////////// UPDATE ///////////////////////////////
        /// <summary>
        /// Mise � jour d'un conducteur.
        /// </summary>
        /// <param name="id">ID du conducteur.</param>
        /// <returns></returns>
        public string DBUpdateConducteur(int id, string nom)
        {
            string sr = "";

            try
            {
                var query = _Conn.Query<Conducteur>($"UPDATE Conducteur SET Name = \"{nom}\" WHERE ID = {id}");

                sr = $"Mise � jour du conducteur d'ID : {id}\n";
            }
            catch (Exception)
            {
                sr = "Il n'y a aucune table.";
            }

            return sr;
        }

        /// <summary>
        /// Mise � jour d'un controleur.
        /// </summary>
        /// <param name="id">ID du controleur</param>
        /// <returns></returns>
        public string DBUpdateControleur(int id, string nom, string mdp)
        {
            string sr = "";

            try
            {
                var query = _Conn.Query<Controleur>($"UPDATE Controleur SET Name = \"{nom}\", Mdp = \"{mdp}\" WHERE ID = {id}");

                sr = $"Mise � jour du controleur d'ID : {id}\n";
            }
            catch (Exception)
            {
                sr = "Il n'y a aucune table.";
            }

            return sr;
        }

        /// <summary>
        /// Mise � jour d'un bus.
        /// </summary>
        /// <param name="id">ID du bus.</param>
        /// <returns></returns>
        public string DBUpdateBus(int id, int numero, string couleur, int idModel)
        {
            string sr = "";

            try
            {
                var query = _Conn.Query<Bus>($"UPDATE Bus SET Number = {numero}, Color=\"{couleur}\", IdModel =\"{idModel}\" WHERE ID = {id}");

                sr = $"Mise � jour du bus d'ID : {id}\n";
            }
            catch (Exception)
            {
                sr = "Il n'y a aucune table.";
            }

            return sr;
        }

        /// <summary>
        /// Met � jour d'un check.
        /// </summary>
        /// <param name="id">ID du check.</param>
        /// <returns></returns>
        public string DBUpdateCheck(int id, string date)
        {
            string sr = "";

            try
            {
                var query = _Conn.Query<CheckUp>($"UPDATE CheckUp SET Date = \"{date}\" WHERE ID = {id}");

                sr = $"Mise � jour du check d'ID : {id}\n";
            }
            catch (Exception)
            {
                sr = "Il n'y a aucune table.";
            }

            return sr;
        }

        /// <summary>
        /// Met � jour d'un incident.
        /// </summary>
        /// <param name="id">ID de l'incident.</param>
        /// <returns></returns>
        public string DBUpdateIncident(int id, string type, int gravite, int etat, string dateMaj, string observation, int x, int y, string picture)
        {
            string sr = "";

            try
            {
                var query = _Conn.Query<Incident>($"UPDATE Incident SET Type = \"{type}\", Gravite = {gravite}, Etat = {etat}, DateMaJ = \"{dateMaj}\", Observation = \"{observation}\", X = {x}, Y = {y}, Picture = \"{picture}\" WHERE ID = {id}");

                sr = $"Mise � jour de l'incident d'ID : {id}\n";
            }
            catch (Exception)
            {
                sr = "Il n'y a aucune table.";
            }

            return sr;
        }

        /// <summary>
        /// Mise � jour d'un mod�le.
        /// </summary>
        /// <param name="id">ID du mod�le</param>
        /// <returns></returns>
        public string DBUpdateModel(int id, string name)
        {
            string sr = "";

            try
            {
                var query = _Conn.Query<Model>($"UPDATE Model SET Name = \"{name}\" WHERE ID = {id}");

                sr = $"Mise � jour du mod�le d'ID : {id}\n";
            }
            catch (Exception)
            {
                sr = "Il n'y a aucune table.";
            }

            return sr;
        }
        #endregion
    }
}