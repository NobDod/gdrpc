using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDRPC.Memory
{
    class MemoryReader
    {
        private MemoryReaderPrivate p;
        public MemoryReader(Process process)
        {
            p = new MemoryReaderPrivate();
            p.Initialize(process, AppAccess.PROCESS_VM_READ | AppAccess.PROCESS_VM_WRITE | AppAccess.PROCESS_VM_OPERATION, process.ProcessName);
        }

        public T Read<T>(int[] offsets) where T : struct
        {
            return p.Read<T>(p.GetModule(p.Game.ProcessName), offsets);
        }
    }
}
