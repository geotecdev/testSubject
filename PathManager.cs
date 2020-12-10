using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TiPS_to_Viewpoint_App_v3.Models;

namespace TiPS_to_Viewpoint_App_v3.Data
{
    public static class PathManager
    {
        static readonly string fileName = "PathOptions.txt";

        public static void SavePath(string nickName, string path, bool isDefault = false)
        {
            var newContent = "";
            var content = GetMockDB(fileName);

            if (Directory.Exists(path))
            {
                if (isDefault)
                {
                    newContent = nickName + "|" + path + Environment.NewLine + content;
                }
                else
                {
                    newContent = content + Environment.NewLine + nickName + "|" + path;
                }
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.Write(newContent);
                }
                MessageBox.Show("New Path Successfully Added");
            }
            else
            {
                MessageBox.Show("Invalid path, link not saved");
                return;
            }
        }

        private static string GetMockDB(string fileName)
        {

            using (StreamReader reader = new StreamReader(fileName))
            {
                return reader.ReadToEnd();
            }
        }

        public static ObservableCollection<PathOption> GetPathOptions()
        {
            var options = new ObservableCollection<PathOption>();
            var mockDB = GetMockDB(fileName); //C:\\tipspayrollapp2\\
            var lines = mockDB.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                var optionItems = line.Split('|');
                options.Add(new PathOption(optionItems[0], optionItems[1]));
            }
            return options;

        }

        public static string NewDirectoryStructure(PathOption option)
        {
            string directoryStructure = "==========================" + Environment.NewLine +
                                "EXPORT PATH //////////////////////" + Environment.NewLine +
                                "==========================" + Environment.NewLine;
            string indent = "";
            string driveMarker = ">";
            var firstLetter = option.FullPath.Substring(0, 1);
            var modPath = option.FullPath.Substring(1, option.FullPath.Length - 1);
            var folderList = modPath.Split('\\').ToList();
            if (folderList[0] == ":")
            {
                folderList[0] = firstLetter;
            }
            foreach (var folder in folderList)
            {
                directoryStructure += indent + driveMarker + ">" + folder + Environment.NewLine;
                indent += "  ";
                driveMarker = "";
            }

            return directoryStructure;
        }
    }
}
