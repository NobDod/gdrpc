using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

/* 
 * Geometry Dash api читатель с дополнением. Класс приватный, потому-что
 * я в любой момент могу что-то поменять :)
 */
namespace GDRPC.Memory
{
    class MemoryReaderPrivate
    {
        #region Static

        public static readonly int PTR_LEN = 4;
        #endregion

        #region Dll import
        [DllImport("kernel32.dll")]
        protected static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        protected static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        protected static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, out int lpNumberOfBytesWritten);
        #endregion

        public string ProcessName { get; protected set; }

        public int BytesRead;
        public int BytesWrite;

        public Process Game;
        public IntPtr GameHandle;

        public bool Initialize(Process process, int access, string processName)
        {
            ProcessName = processName;
            if (process == null)
            {
                Process[] processes = Process.GetProcessesByName(processName);
                if (processes.Length > 0)
                    Game = Process.GetProcessesByName(processName)[0];
                else
                    return false;
            }
            else
            {
                Game = process;
            }
            GameHandle = OpenProcess(access, false, Game.Id);
            return true;
        }

        public IntPtr GetModuleAddress(string moduleFullName)
        {
            foreach (ProcessModule module in Game.Modules)
            {
                if (moduleFullName == module.ModuleName)
                    return module.BaseAddress;
            }
            return (IntPtr)(-1);
        }

        public ProcessModule GetModule(string moduleFullName)
        {
            try
            {
                foreach (ProcessModule module in Game.Modules)
                {
                    if (moduleFullName == module.ModuleName)
                        return module;
                }
            }
            catch
            {

            }
            return null;
        }

        public T Read<T>(long address) where T : struct
        {
            try
            {
                int ByteSize = Marshal.SizeOf(typeof(T));
                byte[] buffer = new byte[ByteSize];
                ReadProcessMemory((int)GameHandle, (int)address, buffer, buffer.Length, ref this.BytesRead);
                return this.BytesToStructure<T>(buffer);
            }
            catch
            {
                return default;
            }
        }

        //TODO: Not the best option
        public T Read<T>(ProcessModule module, int[] offsets) where T : struct
        {
            try
            {
                IntPtr[] pointers = new IntPtr[offsets.Length];
                pointers[0] = this.Read<IntPtr>(IntPtr.Add(module.BaseAddress, offsets[0]).ToInt64());
                for (int i = 1; i < offsets.Length; i++)
                    pointers[i] = this.Read<IntPtr>(IntPtr.Add(pointers[i - 1], offsets[i]).ToInt64());
                if (offsets.Length > 1)
                    return this.Read<T>(IntPtr.Add(pointers[offsets.Length - 2], offsets[offsets.Length - 1]).ToInt64());
                return this.Read<T>(IntPtr.Add(module.BaseAddress, offsets[0]).ToInt64());
            }
            catch
            {
                return default;
            }
        }


        public int newAddress(long[] addresss)
        {
            long address = addresss[0];
            try
            {
                for (int i = 1; i < addresss.Length; i++)
                    address += addresss[i];
            }
            catch
            {

            }
            return (int)address;
        }
        /// <summary>
        /// todo: 64 length not best, ставьте свой крч
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public string ReadString(long address, int length)
        {
            byte[] buffer = new byte[length];
            ReadProcessMemory((int)GameHandle, (int)address, buffer, length, ref this.BytesRead);
            return Encoding.ASCII.GetString(buffer);
        }

        public string ReadString2(long address, int length = 32768)
        {
            byte[] buffer = new byte[length];
            ReadProcessMemory((int)GameHandle, (int)address, buffer, length, ref this.BytesRead);
            return Encoding.ASCII.GetString(buffer);
        }


        public void Write<T>(int address, T value)
        {
            byte[] buffer = this.StructureToBytes(value);

            WriteProcessMemory((int)GameHandle, address, buffer, buffer.Length, out this.BytesWrite);
        }

        protected T BytesToStructure<T>(byte[] bytes) where T : struct
        {
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                return (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }
        }

        protected byte[] StructureToBytes(object value)
        {
            int size = Marshal.SizeOf(value);
            byte[] array = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(value, ptr, true);
            Marshal.Copy(ptr, array, 0, size);
            Marshal.FreeHGlobal(ptr);
            return array;
        }
    }

    class AppAccess
    {
        public const int PROCESS_CREATE_PROCESS = 0x0080;
        public const int PROCESS_CREATE_THREAD = 0x0002;
        public const int PROCESS_DUP_HANDLE = 0x0040;
        public const int PROCESS_QUERY_INFORMATION = 0x0400;
        public const int PROCESS_QUERY_LIMITED_INFORMATION = 0x1000;
        public const int PROCESS_SET_INFORMATION = 0x0200;
        public const int PROCESS_SET_QUOTA = 0x0100;
        public const int PROCESS_SUSPEND_RESUME = 0x0800;
        public const int PROCESS_TERMINATE = 0x0001;
        public const int PROCESS_VM_OPERATION = 0x0008;
        public const int PROCESS_VM_READ = 0x0010;
        public const int PROCESS_VM_WRITE = 0x0020;
        public const long SYNCHRONIZE = 0x00100000L;
    }
}