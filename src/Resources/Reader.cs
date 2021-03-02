using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GDRPC.Resources
{
    class Reader
    {
        public static string ReadFileFromResource(string fname)
        {
            Assembly _assembly = Assembly.GetExecutingAssembly();
            string[] ress = _assembly.GetManifestResourceNames();
            foreach (string name in ress)
            {
                string fileName = string.Join(".", name.Split('.').Skip(2));
                if (fileName == fname)
                {
                    using (Stream stream = _assembly.GetManifestResourceStream(name))
                    using (StreamReader reader = new StreamReader(stream))
                        return reader.ReadToEnd();
                }
            }
            return "";
        }

        public static bool BinaryGetFromResource(string directory, string fname)
        {
            Assembly _assembly = Assembly.GetExecutingAssembly();
            string[] ress = _assembly.GetManifestResourceNames();
            foreach (string name in ress)
            {
                string fileName = string.Join(".", name.Split('.').Skip(2));
                if (fileName == fname)
                {
                    using (Stream stream = _assembly.GetManifestResourceStream(name))
                    {
                        try
                        {
                            byte[] bytes = new byte[stream.Length];
                            stream.Read(bytes, 0, bytes.Length);
                            File.WriteAllBytes(directory, bytes);
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }
    }
}
