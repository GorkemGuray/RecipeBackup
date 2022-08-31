using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBackup
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string sourceDir = @"C:\OMRON\Soft-NA\Projects\Current";
            string backupDir = @"C:\Users\Public\GT_recipe_backups";

            //backupDir klasörünün olup olmadığı kontrol ediliyor. Yoksa oluşturuluyor.

            if (!Directory.Exists(backupDir))
            {
                Directory.CreateDirectory(backupDir);

            } else { 

            //Eski reçetler siliniyor.
            //Silinme şartı 6 ay'dan eski olanlar.

                string[] files = Directory.GetFiles(backupDir, "*.csv").Select(file => Path.GetFileName(file)).ToArray();
                foreach (var file in files)
                {
                    string[] file_date = file.Split('.');
                    DateTime fileDateTime = DateTime.Parse(file_date[0]);
                    DateTime agoDateTime = DateTime.Now.AddMonths(-6); // Bu satırı değiştirirsen kaç ay öncesi olduğunu değiştirirsin.

                    int result = DateTime.Compare(fileDateTime, agoDateTime);

                    if (result < 0)
                    {
                        string fileName = fileDateTime.Day.ToString() + '-' + fileDateTime.Month.ToString() + '-' + fileDateTime.Year.ToString() + ".csv";

                        if (File.Exists(Path.Combine(backupDir, fileName)))
                        {
                            try
                            {
                                File.Delete(Path.Combine(backupDir, fileName));
                            }
                            catch { }

                        }
                    }
                }

            }


            // Soft-Na klaösrü içindeki Reçete_Group1.csv dosyası Backup klasörü içine kopyalanıyor.
            // Kopyalanırken adı değiştiriliyor. gün-ay-yıl.csv olacak şekilde.

            string targetFileName = "Reçete_Group1.csv";

            string backupedFileName = DateTime.Now.Day.ToString() + '-' + DateTime.Now.Month.ToString() + '-' + DateTime.Now.Year.ToString() + ".csv";

            try
            {
                if (!File.Exists(Path.Combine(backupDir,backupedFileName)) && File.Exists(Path.Combine(sourceDir,targetFileName)))
                {
                    File.Copy(Path.Combine(sourceDir,targetFileName), Path.Combine(backupDir,backupedFileName), false);
                }
            } catch { }


        }
    }
}
