using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
namespace GDRPC.App
{
    class ProcessFinder
    {
#if !DEBUG
        private static int limitCounter = 0;
#endif
        /// <summary>
        /// Бесконечный наход процессора, если найден вернем процесс
        /// </summary>
        /// <param name="name">Его имя (без exe)</param>
        /// <returns>Process</returns>
        public static async Task<Process> FindProcess(string name)
        {
            while (true)
            {
                Process[] prNames = Process.GetProcessesByName(name);
                if (prNames.Count() > 0)
                    return prNames[0];
                await Task.Delay(2000);
            }
        }
    }
}
