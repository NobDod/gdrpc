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
        public MemoryReaderPrivate Manager { get => p; }
        private string pn;
        public MemoryReader(string processName, Process process)
        {
            pn = processName;
            p = new MemoryReaderPrivate(process, AppAccess.PROCESS_VM_READ | AppAccess.PROCESS_VM_WRITE | AppAccess.PROCESS_VM_OPERATION);
        }

        public T Read<T>(int[] offsets, int lastAddress) where T : struct
        {
            return p.Read<T>(p.GetModule(pn), offsets, lastAddress);
        }
        public T Read<T>(int[] offsets, bool twoReader = true) where T : struct
        {
            return p.Read<T>(p.GetModule(pn), offsets, twoReader);
        }
        public T Read<T>(long address) where T : struct
        {
            return p.Read<T>(address);
        }
        public void Write<T>(int address, T value)
        {
            p.Write<T>(address, value);
        }
        public void Write<T>(int[] address, T value)
        {
            p.Write<T>(p.GetModule(pn), address, value);
        }
        public void Write<T>(int[] address, int la, T value)
        {
            p.Write<T>(p.GetModule(pn), address, la, value);
        }
    }
}
