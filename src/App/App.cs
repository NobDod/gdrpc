﻿using System;
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

        //stop v2
        private static bool _Stopping = false;
        public static bool HasStopped { get => _Stopping; }

        //is game loaded
        private static bool _gameLoaded = false;
        public static bool GameLoaded { get => _gameLoaded; }

        //default gdrpc
        public static Discord.RichPresence defaultRpc = new Discord.RichPresence
        {
            Assets = new Discord.Assets
            {
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
            _Stopping = false;

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
                {
                    Log.WriteLine("[App]: Failed to parsing discord id application. Stoping");
                    Stop();
                    return;
                }
            }

            //запусk gdrpc
            while (true)
            {
                if (_Stopping || _tm == "")
                    break;
                //если упал процесс или его вообще нет то го процесс делать
                if (_gp == null || _gm == null || _gp.HasExited)
                {
                    _gameLoaded = false;
                    //дискорд был унитилизирован?
                    if (Config.IsKey("p", "_disinit"))
                    {
                        Discord.Discord.Deinitialize();
                        Config.RemoveKey("p", "_disinit");
                        await Sleeping();
                    }
                    await ProcessInitialize();

                    //expection fix.
                    if (_gp == null)
                        break;

                    //дискорд унитилизирован.
                    if (!ulong.TryParse(Config.Read("g", "appID"), out ulong discordAppId))
                    {
                        Log.WriteLine("[App]: Failed to parsing discord id application. Stoping");
                        break;
                    }
                    Discord.Discord.Initialize(discordAppId);
                    Discord.Discord.SetPresence(defaultRpc);
                    Config.Write("p", "_disinit", "+");
                    await Sleeping();

                    //game loaded
                    _gameLoaded = true;
                    continue;
                }
                try
                {
                    if (GM.Reader.Level.IsOpened)
                        await AppLevel.Run();
                    else if (GM.Reader.Editor.IsOpened)
                        await AppEditor.Run();
                    else
                        await AppMenu.Run();
                }
                catch
                {
                    Log.WriteLine("[App]: Memory Reader getted exception.");
                }
                
                await Task.Delay(150);
            }

            Stop();
        }

        /// <summary>
        /// Stop GDRPC
        /// </summary>
        public static void Stop()
        {
            _gameLoaded = false;
            _Stopping = true;
            if (_tm != "" && _im != null)
            {
                if (Config.IsKey("p", "_disinit"))
                {
                    try { Discord.Discord.Deinitialize(); } catch { }
                    Config.RemoveKey("p", "_disinit");
                }
                string tmPa = System.IO.Path.GetTempPath() + "\\HopixTeam\\GDRPC";
                try { if (System.IO.Directory.Exists(tmPa)) System.IO.Directory.Delete(tmPa, true); } catch { }
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
            //limt counter or if process null stop.
            if (_gp == null)
                return;

            _gm = Memory.MemoryReader.Create(_gp, Memory.MemoryAccess.PROCESS_VM_OPERATION, Memory.MemoryAccess.PROCESS_VM_WRITE, Memory.MemoryAccess.PROCESS_VM_READ);
            defaultRpc.Timestamps.Start = DateTime.UtcNow;
            Log.WriteLine("[GameFinder]: Process finded. PID: " + _gp.Id);
        }

        private static async Task Sleeping()
        {
#if DEBUG
            Log.WriteLine("[Debug]: wait task: 3000");
            await Task.Delay(3000);
#else
            await Task.Delay(5000);
#endif
        }
    }
}
