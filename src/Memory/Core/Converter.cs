using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GDRPC.Memory.Core
{
    class Converter
    {
        /// <summary>
        /// Convert bytes to type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Type to bytes.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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
}
