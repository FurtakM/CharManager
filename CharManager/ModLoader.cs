using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace CharManager
{
    class ModLoader
    {
        private List<string> ModsList;
        private List<string> ModsNamesList;

        public ModLoader()
        {
            this.ModsList = new List<string>();
            this.ModsNamesList = new List<string>();

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            if (!Directory.Exists(@"" + baseDir + "/mods/"))
                return;
            
            DirectoryInfo ModsDir = new DirectoryInfo(@"" + baseDir + "/mods/");
            FileInfo[] IniFiles = ModsDir.GetFiles("*.ini");
            DirectoryInfo[] ModDirectories = ModsDir.GetDirectories();

            if (IniFiles.Length == 0)
                return;
            
            foreach (FileInfo file in IniFiles)
            {
                var ModName = Path.GetFileNameWithoutExtension(file.Name);

                foreach (DirectoryInfo dir in ModDirectories)
                {
                    if (ModName == dir.Name)
                    {
                        this.ModsList.Add(dir.FullName);
                        this.ModsNamesList.Add(dir.Name);
                        break;
                    }
                }
            }
        }

        public List<string> GetModsList()
        {
            return this.ModsList;
        }

        public List<string> GetModsNamesList()
        {
            return this.ModsNamesList;
        }

        public static Bitmap GetModImage(string Path)
        {
            Bitmap DefaultImage = CharManager.Properties.Resources.default_image;

            if (String.IsNullOrEmpty(Path))
                return DefaultImage;

            var ImagePath = Path + "/modimage.jpg";

            if (!File.Exists(ImagePath))
                return DefaultImage;

            return new Bitmap(ImagePath);
        }
    }
}
