using System;
using System.Collections.Generic;
using System;
using SQLite.Net;
using SQLite.Net.Platform.XamarinAndroid;
using AppAndroid.Data;
using System.IO;

namespace AppAndroid.Work
{
    public class DBWork
    {
        private Java.IO.File _RootPath = Android.OS.Environment.ExternalStorageDirectory;
        private string _DirectoryPath = "/Android/data/AppAndroid.AppAndroid/files/Transdev.sqlite";
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
        public string[] DBCreateDB()
        {
            DateTimeOffset debut = DateTimeOffset.Now;
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

                _Conn.Insert(new Conducteur() { Nom = "Bidule", MdP = "Pass" });

                sr = "Create tables successfully !\n" + _Path;
                //Debug.WriteLine(_Path);
            }
            catch (Exception e)
            {
                sr = "Impossible to create table";
            }

            TimeSpan span = DateTimeOffset.Now - debut;
            float totalSecond = (span.Minutes * 60) + span.Seconds + ((float)span.Milliseconds / 1000);

            string se = "Temps d'exécution du test : " + totalSecond.ToString() + "s";

            return new string[2] { se, sr };
        }

        /// <summary>
        /// Sélection de N lignes ou toute.
        /// </summary>
        /// <param name="n">Nombre de lignes. Toute si 0.</param>
        /// <returns>t[0] - Temps d'exécution | t[1] - String de résultat</returns>
        public string[] DBSelectConducteur(int id)
        {
            DateTimeOffset debut = DateTimeOffset.Now;
            string sr = "";

            try
            {
                string q = $"SELECT * FROM Conducteur WHERE Id={id}";

                // Selection - table Message
                var query = _Conn.Query<Conducteur>(q);

                foreach (var item in query)
                {
                    sr += $"{item.Id} - {item.Nom}@{item.MdP}\n";
                    //Debug.WriteLine(sr);
                }
            }
            catch (Exception e)
            {
                sr = "Il n'y a aucune table.";
            }

            TimeSpan span = DateTimeOffset.Now - debut;
            float totalSecond = (span.Minutes * 60) + span.Seconds + ((float)span.Milliseconds / 1000);

            string se = "Temps d'exécution du test : " + totalSecond.ToString() + "s";

            return new string[2] { se, sr };
        }

        /// <summary>
        /// Suppression d'une donnée.
        /// </summary>
        /// <param name="n">Index de la suppression.</param>
        /// <returns>t[0] - Temps d'exécution | t[1] - String de résultat</returns>
        public string[] DBDeleteTest(Table table, int id)
        {
            DateTimeOffset debut = DateTimeOffset.Now;
            string sr = "";

            try
            {
                if (id == 0)
                    sr = "Enter ID";
                else {
                    _Conn.Delete<Table>(id);
                    sr = "Delete successfully !";
                }
            }
            catch (Exception e)
            {
                sr = "Impossible to delete data";
            }

            TimeSpan span = DateTimeOffset.Now - debut;
            float totalSecond = (span.Minutes * 60) + span.Seconds + ((float)span.Milliseconds / 1000);

            string se = "Temps d'exécution du test : " + totalSecond.ToString() + "s";

            return new string[2] { se, sr };
        }

        /// <summary>
        /// Mise à jour d'une donnée.
        /// </summary>
        /// <param name="n">Index de la donnée.</param>
        /// <returns>t[0] - Temps d'exécution | t[1] - String de résultat</returns>
        public string[] DBUpdateTest(Table table, int id, string content)
        {
            DateTimeOffset debut = DateTimeOffset.Now;
            string sr = "";

            try
            {
                _Conn.Query<Table>($"UPDATE {table.ToString()} SET Content=\"Update {content}\" WHERE Id={id}");
                sr = "Update successfully !";
            }
            catch (Exception e)
            {
                sr = "Impossible to update data";
            }

            TimeSpan span = DateTimeOffset.Now - debut;
            float totalSecond = (span.Minutes * 60) + span.Seconds + ((float)span.Milliseconds / 1000);

            string se = "Temps d'exécution du test : " + totalSecond.ToString() + "s";

            return new string[2] { se, sr };
        }
    }
}