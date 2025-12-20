// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.JavaScript.NodeApi.Runtime;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

// This part of the class includes the constructor and private helper methods.
// See the other parts of this class for the actual imported APIs.
[SuppressUnmanagedCodeSecurity]
public unsafe partial class NodejsRuntime : JSRuntime
{
    private static unsafe string? PtrToStringUTF8(byte* ptr)
    {
        if (ptr == null) return null;
        int length = 0;
        while (ptr[length] != 0) length++;
        return Encoding.UTF8.GetString(ptr, length);
    }

    private static nint StringToHGlobalUtf8(string? s)
    {
        if (s == null) return default;
        byte[] bytes = Encoding.UTF8.GetBytes(s);
        nint ptr = Marshal.AllocHGlobal(bytes.Length + 1);
        Marshal.Copy(bytes, 0, ptr, bytes.Length);
        Marshal.WriteByte(ptr, bytes.Length, 0);
        return ptr;
    }

    private static nint StringsToHGlobalUtf8(string?[]? s, out int count)
    {
        nint array_ptr = default;
        count = 0;

        if (s != null)
        {
            count = s.Length;
            array_ptr = Marshal.AllocHGlobal(count * sizeof(nint));
            for (int i = 0; i < count; i++)
            {
                nint ptr = StringToHGlobalUtf8(s[i]);
                Marshal.WriteIntPtr(array_ptr + i * sizeof(nint), ptr);
            }
        }

        return array_ptr;
    }

    private static void FreeStringsHGlobal(nint array_ptr, int count)
    {
        if (array_ptr != default)
        {
            for (int i = 0; i < count; i++)
            {
                nint ptr = Marshal.ReadIntPtr(array_ptr + i * sizeof(nint));
                if (ptr != default)
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }

            Marshal.FreeHGlobal(array_ptr);
        }
    }
}
