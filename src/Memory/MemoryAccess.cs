namespace GDRPC.Memory
{
    /// <summary>
    /// Access memory list.
    /// </summary>
    public enum MemoryAccess : long
    {
        /// <summary>
        /// Access to create process in process.
        /// </summary>
        PROCESS_CREATE_PROCESS = 0x0080,

        /// <summary>
        /// Access to create thread in process.
        /// </summary>
        PROCESS_CREATE_THREAD = 0x0002,

        /// <summary>
        /// Access to copy handles. For getting information use QUERY_INROMATION OR LIMITED_INFORMATION.
        /// </summary>
        PROCESS_DUP_HANDLE = 0x0040,

        /// <summary>
        /// Get information by application
        /// </summary>
        PROCESS_QUERY_INFORMATION = 0x0400,

        /// <summary>
        /// Get limited information by application
        /// </summary>
        PROCESS_QUERY_LIMITED_INFORMATION = 0x1000,

        /// <summary>
        /// Set information to application.
        /// </summary>
        PROCESS_SET_INFORMATION = 0x0200,

        /// <summary>
        /// Set quota information
        /// </summary>
        PROCESS_SET_QUOTA = 0x0100,

        /// <summary>
        /// Suspend application or resume.
        /// </summary>
        PROCESS_SUSPEND_RESUME = 0x0800,

        /// <summary>
        /// Terminate application (close)
        /// </summary>
        PROCESS_TERMINATE = 0x0001,

        /// <summary>
        /// Access to operation to virtual memory.
        /// </summary>
        PROCESS_VM_OPERATION = 0x0008,

        /// <summary>
        /// Read virtual memory.
        /// </summary>
        PROCESS_VM_READ = 0x0010,

        /// <summary>
        /// Write virtual memory.
        /// </summary>
        PROCESS_VM_WRITE = 0x0020,

        /// <summary>
        /// ?
        /// </summary>
        SYNCHRONIZE = 0x00100000L
    }

    public enum MemoryEnumHelper
    {
        /// <summary>
        /// for getting length. no change.
        /// </summary>
        WIN_LEN_MEM_STRING_DATA = 0x10
    }


    /// <summary>
    /// only private
    /// </summary>
    class MemoryAccessConverter
    {
        /// <summary>
        /// Convert enum memory to long.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static long EnumToLong(params MemoryAccess[] a)
        {
            long b = 0;
            foreach (MemoryAccess c in a)
            {
                //first.
                if (b == 0)
                {
                    b = (long)c;
                    continue;
                }
                //next first.
                b = b | (long)c;
            }
            return b;
        }
    }
}
