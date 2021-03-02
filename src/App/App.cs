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

        //temp bool
        private static bool[] temp = new bool[5];

        //default gdrpc
        public static Discord.RichPresence defaultRpc = new Discord.RichPresence
        {
            Assets = new Discord.Assets
            {
                LargeImageText = "Geometry Dash",
                LargeImageKey = "logo",
            },
            Timestamps = new Discord.Timestamps
            {
                Start = new DateTime()
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
                _tm = System.IO.Path.GetTempPath() + "HopixTeam\\GDRPC\\";
                //temp подярок
                if (System.IO.Directory.Exists(_tm))
                    if (System.IO.Directory.GetDirectories(_tm).Count() > 10)
                        System.IO.Directory.Delete(_tm, true);
                for (int i = 0; i < 2; i++)
                    _tm += System.IO.Path.GetRandomFileName() + "\\";
                if (!System.IO.Directory.Exists(_tm))
                    System.IO.Directory.CreateDirectory(_tm);

                //достаем конфинг
                string conf = _tm + System.IO.Path.GetRandomFileName();
                System.IO.File.AppendAllText(conf, Resources.Reader.ReadFileFromResource("config.ini"));
                _im = new WinApi.IniManager(conf);

                //check config
                Log.WriteLine("[App]: Config with temp directory initialized. Temp dir: " + _tm);
                if (!_im.IsKey("g", "appID"))
                    AppRunner.MessageBoxFast.Error("Failed to loading GDRPC: extracting config failed.", true);
            }

            //запусk gdrpc
            while (true)
            {
                if (_tm == "")
                    break;
                //если упал процесс или его вообще нет то го процесс делать
                if (_gp == null || _gm == null || _gp.HasExited)
                {
                    //дискорд был унитилизирован?
                    if (Config.IsKey("p", "_disinit"))
                    {
                        Discord.Discord.Deinitialize();
                        Config.RemoveKey("p", "_disinit");
                        await Sleeping();
                    }
                    await ProcessInitialize();

                    //дискорд унитилизирован.
                    Discord.Discord.Initialize(Config.Read("g", "appID"));
                    Discord.Discord.SetPresence(defaultRpc);
                    Config.Write("p", "_disinit", "+");
                    await Sleeping();
                    continue;
                }

                if (GM.Reader.Level.IsOpened)
                    await AppLevel.Run();
                else if (GM.Reader.Editor.IsOpened)
                    await AppEditor.Run();
                else
                {
                    Discord.RichPresence rpc = App.defaultRpc;
                    rpc.Details = "In menu";
                    Discord.Discord.SetPresence(rpc);
                }    

                await Task.Delay(500);
            }
        }

        /// <summary>
        /// Stop GDRPC
        /// </summary>
        public static void Stop(bool removeDirectory=true)
        {
            string tmPa = System.IO.Path.GetTempPath() + "\\HopixTeam\\GDRPC";
            if (System.IO.Directory.Exists(tmPa) && removeDirectory)
                System.IO.Directory.Delete(tmPa, true);
            if (Config.IsKey("p", "_disinit"))
            {
                Discord.Discord.Deinitialize();
                Config.RemoveKey("p", "_disinit");
            }
            _tm = "";
            _im = null;
            _gp = null;
            _gm = null;
            Log.WriteLine("[App]: Stopped");
        }

        //privates
        private static async Task ProcessInitialize()
        {
            Log.WriteLine("[GameFinder]: Finding process with name " + _im.Read("g", "processName"));
            _gp = await ProcessFinder.FindProcess(_im.Read("g", "processName"));
            _gm = new Memory.MemoryReader(_im.Read("g", "processName") + _im.Read("g", "ext"), _gp);
            defaultRpc.Timestamps.Start = DateTime.UtcNow;
            Log.WriteLine("[GameFinder]: Process finded. PID: " + _gp.Id);
        }

        private static async Task Sleeping()
        {
#if DEBUG
            Log.WriteLine("[Debug]: Debug mode setted delay to 1000");
            await Task.Delay(1000);
#else
            await Task.Delay(5000);
#endif
        }
    }
}
