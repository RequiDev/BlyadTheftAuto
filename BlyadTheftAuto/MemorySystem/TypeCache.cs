using System;
using System.Runtime.InteropServices;

namespace BlyadTheftAuto.MemorySystem
{
    internal static class TypeCache<T>
    {
        public static readonly int Size;
        public static readonly Type Type;
        public static readonly TypeCode TypeCode;

        static TypeCache()
        {
            Type = typeof(T);

            if (Type.IsEnum)
                Type = Type.GetEnumUnderlyingType();

            if (Type == typeof(IntPtr))
            {
                Size = IntPtr.Size;
            }
            else if (Type == typeof(bool))
            {
                Size = 1;
            }
            else
            {
                Size = Marshal.SizeOf<T>();
            }

            TypeCode = Type.GetTypeCode(Type);
        }
    }
}
