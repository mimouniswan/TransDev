using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;

namespace AppAndroid.Work
{
    public static class FileWork
    {
        public static string RootPath { get; } = Android.OS.Environment.ExternalStorageDirectory.Path;

        /// <summary>
        /// Création d'un fichier dans l'emplacement cible.
        /// </summary>
        /// <param name="name">Nom du fichier (avec son extension).</param>
        /// <param name="fileContent">Le contenue du fichier.</param>
        /// <param name="path">*Le chemin a partir de la racine de la mémoire interne de l'appareil.</param>
        /// <returns></returns>
        public static string CreateFile(string name, string fileContent, string path = "/Android/data/AppAndroid.AppAndroid/files/")
        {
            string sr = "";

            try {
                Java.IO.File f = new Java.IO.File(RootPath + $"{path}{name}");

                using (var streamWriter = new StreamWriter(f.Path, false))
                    streamWriter.WriteLine(fileContent);

                using (var streamReader = new StreamReader(f.Path))
                {
                    string content = streamReader.ReadToEnd();
                    System.Diagnostics.Debug.WriteLine(content);

                    sr = $"Contenu fichier :\n{content}\n";
                }
            }
            catch (Exception e)
            {
                sr = "Une erreur est survenue lors de la création du fichier.";
            }

            return sr;
        }
    }
}