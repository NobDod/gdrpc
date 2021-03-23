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
#if !DEBUG
            limitCounter = 0;//reset.
#endif
            while (true)
            {
                Process[] prNames = Process.GetProcessesByName(name);
                if (prNames.Count() > 0)
                    return prNames[0];
#if !DEBUG
                if (limitCounter > 2)
                    return null;
                Log.WriteLine("[GameFinder]: Limit counter: {0}/3", limitCounter + 1);
                limitCounter++;
#endif
                await Task.Delay(2000);
            }
        }
    }
}
