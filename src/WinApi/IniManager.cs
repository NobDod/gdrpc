using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GDRPC.WinApi
{
    class IniManager
    {
        [DllImport("kernel32")]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        private readonly string _path;

        public IniManager(string path)
        {
            if (!File.Exists(path))
                File.AppendAllText(path, "");
            _path = new FileInfo(path).FullName;
        }

        public string Read(string section, string key, int len=255)
        {
            StringBuilder obj = new StringBuilder(len);
            GetPrivateProfileString(section, key, "", obj, len, this._path);
            return obj.ToString();
        }

        public bool ReadInt(string section, string key, out int result, int len = 255) => int.TryParse(this.Read(section, key, len), out result);

        public void Write(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, this._path);
        }

        public bool IsKey(string section, string key) => this.Read(section, key).Length > 0;
        public void RemoveKey(string section, string key) => this.Write(section, key, null);
        public void RemoveSection(string section) => this.Write(section, null, null);
    }
}
