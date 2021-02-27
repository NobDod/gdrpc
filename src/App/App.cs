using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDRPC.App
{
    class App
    {
        //temp directory
        private static string _tm = "";
        public static string TempDir { get => _tm; }

        //ini manager
        private static WinApi.IniManager _im;
        public static WinApi.IniManager Config { get => _im; }

        //game process
        private static System.Diagnostics.Process _gp;
        public static System.Diagnostics.Process GameProcess { get => _gp; }

        //game manager
        private static Memory.MemoryReader _gm;
        public static Memory.MemoryReader GameManager { get => _gm; }


        //default gdrpc
        public static Discord.RichPresence defaultRpc = new Discord.RichPresence
        {
            Assets = new Discord.Assets
            {
                LargeImageText = "test",
                LargeImageKey = "slogo",
            },
            Timestamps = new Discord.Timestamps
            {
                Start = DateTime.UtcNow
            }
        };

        /// <summary>
        /// Run GDRPC
        /// </summary>
        /// <returns></returns>
        public static async Task Run()
        {
            //темп папка
            if (_tm == "")
            {
                _tm = System.IO.Path.GetTempPath() + "GDRPC\\";
                for (int i = 0; i < 2; i++)
                    _tm += Utils.RandomText.Run(16 + i) + "\\";
                if (!System.IO.Directory.Exists(_tm))
                    System.IO.Directory.CreateDirectory(_tm);

                //достаем конфинг
                string conf = _tm + Utils.RandomText.Run(32);
                System.IO.File.AppendAllText(conf, Resources.Reader.ReadFileFromResource("config.ini"));
                _im = new WinApi.IniManager(conf);
            }

            //запусk gdrpc
            while (true)
            {
                //если упал процесс или его вообще нет то го процесс делать
                if (_gp == null || _gm == null || _gp.HasExited)
                {
                    //дискорд был унитилизирован?
                    if (Config.IsKey("p", "_disinit"))
                    {
                        Discord.Discord.Deinitialize();
                        Config.RemoveKey("p", "_disinit");
                    }
                    await ProcessInitialize();

                    //дискорд унитилизирован.
                    Discord.Discord.Initialize(Config.Read("g", "appID"));
                    Discord.Discord.SetPresence(defaultRpc);
                    Config.Write("p", "_disinit", "+");
                    await Task.Delay(5000);
                    continue;
                }

                await Task.Delay(1500);

                Console.WriteLine(GM.Reader.SceneID);
            }
        }

        /// <summary>
        /// Stop GDRPC
        /// </summary>
        public static void Stop()
        {
            string tmPa = System.IO.Path.GetTempPath() + "GDRPC";
            if (System.IO.Directory.Exists(tmPa))
                System.IO.Directory.Delete(tmPa, true);
            _tm = "";
            _im = null;
            _gp = null;
            _gm = null;
        }

        //privates
        private static async Task ProcessInitialize()
        {
            Console.WriteLine("GP find");
            _gp = await ProcessFinder.FindProcess(_im.Read("g", "processName"));
            _gm = new Memory.MemoryReader(_im.Read("g", "processName") + _im.Read("g", "ext"), _gp);
            Console.WriteLine("Debug PID: " + _gp.Id);
        }
    }
}
