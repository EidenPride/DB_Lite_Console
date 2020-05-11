using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DB_Lite_Console
{
    class Program
    {
        //DB - классы
        private class RecorderSetup
        {
            public string SetupName { get; set; }
            public string SetupValstring { get; set; }
            public int SetupValint { get; set; }
            public bool SetupValbool { get; set; }
            public DateTime SetupValDateTime { get; set; }
            public long SetupVallong { get; set; }
        }
        private class CamsSetup
        {
            public string CamID { get; set; }
            public string CamName { get; set; }
            public string CamIP { get; set; }
            public string CamDescription { get; set; }
            public string CamLogin { get; set; }
            public string CamPassword { get; set; }
            public bool camAutoRecconect { get; set; }
        }
        //DB - классы

        static string PROGRAM_DIR = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        static LiteDatabase db = null;
        static void Main(string[] args)
        {
            db = new LiteDatabase(@PROGRAM_DIR + @"\data.dat");
            IEnumerable<string> collections = db.GetCollectionNames();
            int c = collections.Count();

            if (c == 0)
            {
                var _RecorderSetup = db.GetCollection<RecorderSetup>("RecorderSetup");
                // Create unique index in SetupName field
                _RecorderSetup.EnsureIndex(x => x.SetupName, true);

                var set_recorderURL = new RecorderSetup
                {
                    SetupName = "recorderURL",
                    SetupValstring = ""
                };
                _RecorderSetup.Insert(set_recorderURL);
                var set_recorderURLPort = new RecorderSetup
                {
                    SetupName = "recorderURLPort",
                    SetupValint = 8085
                };
                _RecorderSetup.Insert(set_recorderURLPort);
                var set_recorderLogin = new RecorderSetup
                {
                    SetupName = "recorderLogin",
                    SetupValstring = "admin"
                };
                _RecorderSetup.Insert(set_recorderLogin);
                var set_recorderPassword = new RecorderSetup
                {
                    SetupName = "recorderPassword",
                    SetupValstring = ""
                };
                _RecorderSetup.Insert(set_recorderPassword);
                var set_recorderArchiveDir = new RecorderSetup
                {
                    SetupName = "recorderArchiveDir",
                    SetupValstring = @"F:\cam_arch"
                };
                _RecorderSetup.Insert(set_recorderArchiveDir);

                //Потом удалить нахуй!!! только для теста
                var _CamsSetup = db.GetCollection<CamsSetup>("CamsSetup");
                // Create unique index in SetupName field
                _CamsSetup.EnsureIndex(x => x.CamID, true);
                var set_Cam = new CamsSetup
                {
                    CamID = Guid.NewGuid().ToString(),
                    CamName = "Camera1",
                    CamIP = "rtsp://192.168.1.16",
                    CamDescription = "Camera1 - подвал",
                    CamLogin = "admin",
                    CamPassword = "123456",
                    camAutoRecconect = false
                };
                _CamsSetup.Insert(set_Cam);
                set_Cam = new CamsSetup
                {
                    CamID = Guid.NewGuid().ToString(),
                    CamName = "Camera2",
                    CamIP = "rtsp://192.168.1.17",
                    CamDescription = "Camera2 - подвал",
                    CamLogin = "admin",
                    CamPassword = "123456",
                    camAutoRecconect = true
                };
                _CamsSetup.Insert(set_Cam);
                //Потом удалить нахуй!!! только для теста
            }
            else 
            {
                Console.WriteLine("Данные регистратора");
                Console.WriteLine("----------------------------------------------------------");
                var RecorderSetup = db.GetCollection<RecorderSetup>("RecorderSetup");
                var query = RecorderSetup.FindAll();
                foreach (var setup in query)
                {
                    switch (setup.SetupName)
                    {
                        case "recorderURL":
                            Console.WriteLine("recorderURL - "+setup.SetupValstring);
                            break;
                        case "recorderURLPort":
                            Console.WriteLine("recorderURLPort - " + setup.SetupValint);
                            break;
                        case "recorderLogin":
                            Console.WriteLine("recorderLogin - " + setup.SetupValstring);
                            break;
                        case "recorderPassword":
                            Console.WriteLine("recorderPassword - " + setup.SetupValstring);
                            break;
                        case "recorderArchiveDir":
                            Console.WriteLine("recorderArchiveDir - " + setup.SetupValstring);
                            break;
                        default:
                            break;
                    }
                }

                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine("Данные камер");
                Console.WriteLine("----------------------------------------------------------");
                var CamsSetup = db.GetCollection<CamsSetup>("CamsSetup");
                var cam_query = CamsSetup.FindAll();
                int i = 0;

                foreach (var _cam in cam_query)
                {
                    Console.WriteLine("Камера "+ (i+1));
                    Console.WriteLine("CamUID - " + _cam.CamID);
                    Console.WriteLine("CamName - " + _cam.CamName);
                    Console.WriteLine("CamIP - " + _cam.CamIP);
                    Console.WriteLine("CamDescription - " + _cam.CamDescription);
                    Console.WriteLine("CamLogin - " + _cam.CamLogin);
                    Console.WriteLine("CamPassword - " + _cam.CamPassword);
                    Console.WriteLine("camAutoRecconect - " + _cam.camAutoRecconect);
                    Console.WriteLine("----------------------------------------------------------");
                    i++;
                }
            }

            String readline = null;

            while (readline == null)
            {
                readline = Console.ReadLine();
                if (readline == null) Thread.Sleep(500);
            }
        }
    }
}
