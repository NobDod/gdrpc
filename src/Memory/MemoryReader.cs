using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GDRPC.Memory
{
    /// <summary>
    /// Официально переписано для GDRPC.
    /// by NobDod
    /// github.com/nobdod/memoryreader
    /// </summary>
    public class MemoryReader
    {
        /// <summary>
        /// Create with process name.
        /// </summary>
        /// <param name="processName">Process name</param>
        /// <param name="access">Access list for application. Example read memory, write memory.</param>
        /// <returns>MemoryReader. If null, process not found.</returns>
        public static MemoryReader Create(string processName, params MemoryAccess[] access) => Create(Core.ProcessData.getProcess(processName), access);

        /// <summary>
        /// Create with process id.
        /// </summary>
        /// <param name="processId">Process id</param>
        /// <param name="access">Access list for application. Example read memory, write memory.</param>
        /// <returns>MemoryReader. If null, process not found.</returns>
        public static MemoryReader Create(int processId, params MemoryAccess[] access) => Create(Core.ProcessData.getProcess(processId), access);

        /// <summary>
        /// Create with process source.
        /// </summary>
        /// <param name="process">Process source.</param>
        /// <param name="access">Access list for application. Example read memory, write memory.</param>
        /// <returns>MemoryReader. If null, process not found.</returns>
        public static MemoryReader Create(Process process, params MemoryAccess[] access)
        {
            //access not be null
            if (access == null || access.Count() < 1)
                throw new ArgumentNullException("access");
            if (process == null || process.HasExited)
                return null;

            //create process
            IntPtr ph = Core.ImportLibrary.OpenProcess((int)MemoryAccessConverter.EnumToLong(access), false, process.Id);

            //handle consturct
            int i = (int)MemoryAccessConverter.EnumToLong(access);
            return new MemoryReader(process.ProcessName + ".exe", process, ph);
        }

        //construct
        private IntPtr _processHandler;
        private Process _process;
        private string _mainModuleName;
        private int[] _bytesSystem;
        //base
        public Process Process { get => _process; }
        /// <summary>
        ///	process
        /// </summary>
        public IntPtr ProcessHandler { get => _processHandler; }
        /// <summary>
        ///	process handler address
        /// </summary>
        public string ProcessMainModuleName { get => _mainModuleName; }
        /// <summary>
        ///	Last bytes readed
        /// </summary>
        public int BytesReaded { get => _bytesSystem[0]; }
        /// <summary>
        ///	Last bytes writed
        /// </summary>
        public int BytesWrited { get => _bytesSystem[1]; }

        /// <summary>
        /// Initialize memory reader
        /// </summary>
        /// <param name="mmn">main module name</param>
        /// <param name="process">get process</param>
        /// <param name="op">open process</param>
        public MemoryReader(string mmn, Process process, IntPtr op)
        {
            this._process = process;
            this._processHandler = op;
            this._mainModuleName = mmn;
            this._bytesSystem = new int[2];
        }

        #region Converter
        /// <summary>
        /// Convert offests to int ptr.
        /// </summary>
        /// <param name="offsets">addresss</param>
        /// <param name="baseAddress">if -1, we a automatic find base address in main module :)</param>
        /// <param name="useReader">recomended true, for good getting full address :)</param>
        /// <returns>new IntPtr</returns>
        public IntPtr ConvertLayers(int[] offsets, IntPtr baseAddress)
        {
            IntPtr result = IntPtr.Add(baseAddress, offsets[0]);
            for (int i = 1; i < offsets.Count(); i++) result = IntPtr.Add(this.Read<IntPtr>(result.ToInt64()), offsets[i]);
            return result;
        }
        #endregion

        #region Reading
        /// <summary>
        /// Reading address
        /// </summary>
        /// <param name="address">address</param>
        /// <returns>T</returns>
        public T Read<T>(long address) where T : struct
        {
            try
            {
                byte[] buffer = new byte[Marshal.SizeOf(typeof(T))];
                Core.ImportLibrary.ReadProcessMemory((int)this._processHandler, (int)address, buffer, buffer.Length, ref this._bytesSystem[0]);
                return Core.Converter.BytesToStructure<T>(buffer);
            }
            catch { return default; }
        }

        /// <summary>
        /// Reading address
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="module">process module</param>
        /// <param name="offests">offests</param>
        /// <param name="normalReader">if true, you will not need to read it twice, we will do it and return the full value, not the address. </param>
        /// <returns>result. if bad, returned a default result.</returns>
        public T Read<T>(ProcessModule module, params int[] offests) where T : struct
        {
            try
            {
                IntPtr address = this.ConvertLayers(offests, module.BaseAddress);
                System.Diagnostics.Debug.WriteLine(address.ToInt64());
                return this.Read<T>(address.ToInt64());
            }
            catch
            {
                return default;
            }
        }


        /// <summary>
        /// Reading address
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="offests">offests</param>
        /// <returns>result. if bad, returned a default result.</returns>
        public T Read<T>(params int[] offests) where T : struct => this.Read<T>(Core.ProcessData.GetModule(this._process, this._mainModuleName), offests);

        /// <summary>
        /// Reading address and return string.
        /// </summary>
        /// <param name="address">address</param>
        /// <param name="len">length of string</param>
        /// <returns>if null, failed to reading :)</returns>
        public string ReadString(long address, int len)
        {
            try
            {
                byte[] bytes = new byte[len];
                Core.ImportLibrary.ReadProcessMemory((int)this._processHandler, (int)address, bytes, bytes.Length, ref this._bytesSystem[0]);
                return System.Text.Encoding.ASCII.GetString(bytes).Replace("\0", string.Empty);
            }
            catch { return null; }
        }

        /// <summary>
        /// Reading address and return string.
        /// </summary>
        /// <param name="offests">offests</param>
        /// <param name="module">process module</param>
        /// <returns>if null, failed to reading :)</returns>
        public string ReadString(ProcessModule module, params int[] offests)
        {
            try
            {
                IntPtr address = this.ConvertLayers(offests, module.BaseAddress);
                int len = this.Read<int>(IntPtr.Add(address, (int)MemoryEnumHelper.WIN_LEN_MEM_STRING_DATA).ToInt64());
                return this.ReadString(address.ToInt64(), len);
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Reading address and return string.
        /// </summary>
        /// <param name="offests">offests</param>
        /// <returns>if null, failed to reading :)</returns>
        public string ReadString(params int[] offests) => this.ReadString(Core.ProcessData.GetModule(this._process, this._mainModuleName), offests);
        #endregion

        #region Write
        /// <summary>
        /// Write to memory.
        /// </summary>
        /// <typeparam name="T">type to write</typeparam>
        /// <param name="address">address</param>
        /// <param name="value">value</param>
        public void Write<T>(long address, T value)
        {
            try
            {
                byte[] buffer = Core.Converter.StructureToBytes(value);
                Core.ImportLibrary.WriteProcessMemory((int)this._processHandler, (int)address, buffer, buffer.Length, out this._bytesSystem[1]);
            }
            catch { }
        }

        /// <summary>
        /// Write to memory
        /// </summary>
        /// <typeparam name="T">type to write</typeparam>
        /// <param name="processModule">process module</param>
        /// <param name="value">value</param>
        /// <param name="offests">address list</param>
        public void Write<T>(ProcessModule processModule, T value, params int[] offests) => this.Write<T>(this.ConvertLayers(offests, processModule.BaseAddress).ToInt64(), value);

        /// <summary>
        /// Write to memory
        /// </summary>
        /// <typeparam name="T">type to write</typeparam>
        /// <param name="processModule">process module</param>
        /// <param name="value">value</param>
        public void Write<T>(T value, params int[] offests) => this.Write<T>(Core.ProcessData.GetModule(this._process, this._mainModuleName), value, offests);
        #endregion
    }
}
