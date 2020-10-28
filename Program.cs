using CG.Web.MegaApiClient;
using System;
using System.IO;
using System.Linq;

namespace mp3MegaSync
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileNumber = 0;
            var client = new MegaApiClient();

            client.LoginAnonymous();

            DirectoryInfo info = new DirectoryInfo(@".\");
            FileInfo[] files = info.GetFiles("*.txt").OrderBy(p => p.Name).ToArray();
            foreach (FileInfo file in files)
            {
                Console.WriteLine(file);
            }

            Uri link = new Uri("https://mega.nz/folder/CsFjmCAD#a2uE2YVum3dV1lSSbPOXKg");

            var nodes = client.GetNodesFromLink(link).OrderBy(s => s.Name);

            foreach (var node in nodes.Where(x => x.Type == NodeType.File))
            {
                Console.WriteLine($"Downloading {node.Name}");
                try
                {
                    Console.WriteLine(fileNumber);

                    try
                    {
                        if (node.ModificationDate != files[fileNumber].LastWriteTime)
                            File.Delete(files[fileNumber].Name);
                    }
                    catch(IndexOutOfRangeException){}

                    client.DownloadFile(node, node.Name);

                    File.SetLastWriteTime(node.Name, node.ModificationDate.Value);
                    //if (node.ModificationDate.ToString() != files[fileNumber].LastWriteTime.ToString())
                    //{
                    //    Console.WriteLine(node.Name);
                    //    Console.WriteLine(node.ModificationDate);
                    //    Console.WriteLine(files[fileNumber].Name);
                    //    Console.WriteLine(files[fileNumber].LastWriteTime);
                    //}
                    //Temporary code block for debugging
                }
                catch (IOException)
                {
                    Console.WriteLine("Download failed!\nSystem.IO.IOExcepction: you already have this file");
                }
                fileNumber++;
            }
        }
    }
}
