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
        private readonly MemoryReaderPrivate p;
        public MemoryReaderPrivate Manager { get => p; }
        private readonly string pn;
        public MemoryReader(string processName, Process process)
        {
            pn = processName;
            p = new MemoryReaderPrivate(process, AppAccess.PROCESS_VM_READ | AppAccess.PROCESS_VM_WRITE | AppAccess.PROCESS_VM_OPERATION);
        }

        public T Read<T>(int[] offsets, int lastAddress) where T : struct => p.Read<T>(p.GetModule(pn), offsets, lastAddress);
        public T Read<T>(int[] offsets, bool twoReader = true) where T : struct => p.Read<T>(p.GetModule(pn), offsets, twoReader);
        public T Read<T>(long address) where T : struct => p.Read<T>(address);

        public string ReadString(int[] offsets, int lastAddress) => p.ReadString(p.GetModule(pn), offsets, lastAddress);
        public string ReadString(int[] offsets) => p.ReadString(p.GetModule(pn), offsets);
        public string ReadString(long address, int len) => p.ReadString(address, len);

        public void Write<T>(int address, T value) => p.Write<T>(address, value);
        public void Write<T>(int[] address, T value) => p.Write<T>(p.GetModule(pn), address, value);
        public void Write<T>(int[] address, int la, T value) => p.Write<T>(p.GetModule(pn), address, la, value);
    }
}
