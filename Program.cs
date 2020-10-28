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
            var client = new MegaApiClient();

            client.LoginAnonymous();

            Uri link = new Uri("https://mega.nz/folder/CsFjmCAD#a2uE2YVum3dV1lSSbPOXKg");
            var nodes = client.GetNodesFromLink(link);

            foreach (var node in nodes.Where(x => x.Type == NodeType.File))
            {
                Console.WriteLine($"Downloading {node.Name}");
                try
                {
                    client.DownloadFile(node, node.Name);
                    Console.WriteLine(node.Name);
                }
                catch (IOException)
                {
                    Console.WriteLine("System.IO.IOException: download failed!");
                }
                Console.WriteLine($"Finsished downloading {node.Name}");
            }
        }
    }
}
