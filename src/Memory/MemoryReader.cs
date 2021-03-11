using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GDRPC.Memory
{
    /// <summary>
    /// Официально переписано для GDRPC.
    /// by NobDod
    /// github.com/nobdod/memoryreader
    /// </summary>
    public class MemoryReader
    {
        class Library
        {
            public const int WIN_ADDRESS = 0x10;
            public static IntPtr GetModuleAddress(Process process, string moduleFullName)
            {
                foreach (ProcessModule module in process.Modules)
                {
                    if (moduleFullName == module.ModuleName)
                        return module.BaseAddress;
                }
                return (IntPtr)(-1);
            }

            public static bool GetProcess(string processName, out Process process)
            {
                Process[] processes = Process.GetProcessesByName(processName);
                if (processes.Length > 0)
                    process = Process.GetProcessesByName(processName)[0];
                else
                    process = null;
                return (process == null);
            }

            [DllImport("kernel32.dll")]
            public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

            [DllImport("kernel32.dll")]
            public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, ref int lpNumberOfBytesRead);

            [DllImport("kernel32.dll")]
            public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, out int lpNumberOfBytesWritten);
        }

        class BytesConvert
        {
            public static T BytesToStructure<T>(byte[] bytes) where T : struct
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
            public static byte[] StructureToBytes(object value)
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

        private readonly int[] bytes = new int[2];
        private readonly Process process;
        private IntPtr handle;
        private readonly int access = AppAccess.PROCESS_VM_READ | AppAccess.PROCESS_VM_WRITE | AppAccess.PROCESS_VM_OPERATION;

        #region Public read
        /// <summary>
        /// Байтов было прочитано.
        /// </summary>
        public int BytesRead { get => bytes[0]; }

        /// <summary>
        /// Байтов было записано.
        /// </summary>
        public int BytesWrited { get => bytes[1]; }

        /// <summary>
        /// Процесс игры
        /// </summary>
        public Process GameProcess { get => process; }

        /// <summary>
        /// Адрес для чтения или записи процесса.
        /// </summary>
        public IntPtr GameHandleProcess { get => handle; }
        #endregion

        #region Inititlizer
        public MemoryReader(Process process)
        {
            this.process = process;
            Init(process);
        }

        public MemoryReader(Process process, int access)
        {
            this.process = process;
            this.access = access;
            Init(process);
        }

        public MemoryReader(string processName)
        {
            while (!Library.GetProcess(processName, out this.process)) Task.Delay(500).ConfigureAwait(false);
            Init(process);
        }

        public MemoryReader(string processName, int access)
        {
            while (!Library.GetProcess(processName, out this.process)) Task.Delay(500).ConfigureAwait(false);
            this.access = access;
            Init(process);
        }

        private void Init(Process process)
        {
            handle = Library.OpenProcess(this.access, false, process.Id);
        }
        #endregion

        #region Base
        public ProcessModule GetModule(string moduleFullName)
        {
            foreach (ProcessModule module in process.Modules)
            {
                if (moduleFullName == module.ModuleName)
                    return module;
            }
            return null;
        }
        #endregion

        #region Reader
        /// <summary>
        /// Прочитать адрес
        /// </summary>
        /// <typeparam name="T">тип адреса, в котором как раз будет вернут результат.</typeparam>
        /// <param name="address">адрес</param>
        /// <returns></returns>
        public T Read<T>(long address) where T : struct
        {
            try
            {
                byte[] buffer = new byte[Marshal.SizeOf(typeof(T))];
                Library.ReadProcessMemory((int)this.handle, (int)address, buffer, buffer.Length, ref this.bytes[0]);
                return BytesConvert.BytesToStructure<T>(buffer);
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Прочитать адреса через оффсеты. Является хорошим вариантом для чтение нескольких адресов, где просто так их не сложить. 
        /// Минус этого может быть только нагрузка на систему (но не факт)
        /// </summary>
        /// <typeparam name="T">тип адреса, в котором как раз будет вернут результат.</typeparam>
        /// <param name="addresss">оффсет адреса</param>
        /// <param name="twoReader">Вы можете указать вместо IntPtr (и повторно прочитать) эту переменную на правда, и всё! Мы за вас это сделаем!</param>
        /// <returns></returns>
        public T Read<T>(ProcessModule module, int[] addresss, bool twoReader = false) where T : struct
        {
            try
            {
                //да да да, оптимизированная версия, а не тысячи IntPtr[]:)
                //получаем основной адрес
                IntPtr addressToRead = IntPtr.Add(module.BaseAddress, addresss[0]);
                //если есть еще оффесты, добавляем их.
                for (int i = 1; i < addresss.Count(); i++)
                    addressToRead = IntPtr.Add(this.Read<IntPtr>(addressToRead.ToInt64()), addresss[i]);
                //последнее чтение
                if (!twoReader)
                    return this.Read<T>(addressToRead.ToInt64());
                return this.Read<T>(this.Read<IntPtr>(addressToRead.ToInt64()).ToInt64());
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Сразу добавляем адрес для чтение (поскольку для некоторых оффестов надо получить для начало IntPtr а потом повторно его прочитать уже в другом типе)
        /// Вот пример. Зачем вам надо "прочитать(ptr.add(прочитать(модуль)..." 
        /// </summary>
        /// <typeparam name="T">тип адреса, в котором как раз будет вернут результат.</typeparam>
        /// <param name="address">оффсет адреса</param>
        /// <param name="lastAddress">последний адрес который надо прочитать</param>
        /// <param name="twoReader">Вы можете указать вместо IntPtr (и повторно прочитать) эту переменную на правда, и всё! Мы за вас это сделаем!</param>
        /// <returns></returns>
        public T Read<T>(ProcessModule module, int[] address, int lastAddress) where T : struct
            => this.Read<T>(IntPtr.Add(this.Read<IntPtr>(module, address, false), lastAddress).ToInt64());
        
        /// <summary>
        /// Прочитать память и вернуть значение. 
        /// </summary>
        /// <param name="address">адрес</param>
        /// <param name="len">длина</param>
        /// <returns></returns>
        public string ReadString(long address, int len)
        {
            byte[] bytes = new byte[len];
            Library.ReadProcessMemory((int)this.handle, (int)address, bytes, bytes.Length, ref this.bytes[0]);
            return System.Text.Encoding.ASCII.GetString(bytes).Replace("\0", string.Empty);
        }

        /// <summary>
        /// Прочитать память через адреса. В случае исключения, будет вернут пустой ответ ("")
        /// Встроен автоматический подсчёт длины текста.
        /// </summary>
        /// <param name="addresss">оффсет адреса</param>
        /// <returns></returns>
        public string ReadString(ProcessModule module, int[] addresss)
        {
            try
            {
                IntPtr memoryToRead = this.Read<IntPtr>(module, addresss, false);
                int size_memoryToRead = this.Read<int>(IntPtr.Add(memoryToRead, Library.WIN_ADDRESS).ToInt64());
                if (size_memoryToRead < Library.WIN_ADDRESS)
                    return this.ReadString(memoryToRead.ToInt64(), size_memoryToRead);
                memoryToRead = this.Read<IntPtr>(memoryToRead.ToInt64());
                return this.ReadString(memoryToRead.ToInt64(), size_memoryToRead);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Прочитать память через адреса в котором еще требуется дополнительное чтение. В случае исключения, будет вернут пустой ответ ("")
        /// Встроен автоматический подсчёт длины текста.
        /// </summary>
        /// <param name="addresss">оффсет адреса</param>
        /// <param name="lastAddress">последний адрес который надо прочитать</param>
        /// <returns></returns>
        public string ReadString(ProcessModule module, int[] addresss, int lastAddress)
        {
            try
            {
                IntPtr memoryToRead = IntPtr.Add(this.Read<IntPtr>(module, addresss, false), lastAddress);
                int size_memoryToRead = this.Read<int>(IntPtr.Add(memoryToRead, Library.WIN_ADDRESS).ToInt64());
                if (size_memoryToRead < Library.WIN_ADDRESS)
                    return this.ReadString(memoryToRead.ToInt64(), size_memoryToRead);
                memoryToRead = this.Read<IntPtr>(memoryToRead.ToInt64());
                return this.ReadString(memoryToRead.ToInt64(), size_memoryToRead);

            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region Write
        /// <summary>
        /// Запись в память игры.
        /// </summary>
        /// <typeparam name="T">тип, который нужен для записи</typeparam>
        /// <param name="address">адрес</param>
        /// <param name="value">значение</param>
        public void Write<T>(long address, T value)
        {
            byte[] buffer = BytesConvert.StructureToBytes(value);
            Library.WriteProcessMemory((int)this.handle, (int)address, buffer, buffer.Length, out this.bytes[1]);
        }

        /// <summary>
        /// Запись в память игры, в виде как чтение. Рекомендуем задействовать эту функцию, оно само посчитает адрес за вас!
        /// Если у вас не работает, попробуйте обычную версию, или версию с расчётом последнего адреса.
        /// </summary>
        /// <typeparam name="T">тип, который нужен для записи</typeparam>
        /// <param name="addresss">адреса</param>
        /// <param name="value">значение</param>
        public void Write<T>(ProcessModule module, int[] addresss, T value)
        {
            this.Write(this.Read<IntPtr>(module, addresss, false).ToInt64(), value);
            Console.WriteLine("writed");
        }

        /// <summary>
        /// Сразу добавляем адрес для записи (поскольку для некоторых оффестов надо получить для начало IntPtr а потом повторно его прочитать уже в другом типе)
        /// </summary>
        /// <typeparam name="T">тип, который нужен для записи</typeparam>
        /// <param name="addresss">адреса</param>
        /// <param name="lastAddress">последний адрес который надо прочитать</param>
        /// <param name="value">значение</param>
        /// <returns></returns>
        public void Write<T>(ProcessModule module, int[] addresss, int lastAddress, T value)
            => this.Write<T>(IntPtr.Add(this.Read<IntPtr>(module, addresss, false), lastAddress).ToInt64(), value);
        #endregion
    }
}
