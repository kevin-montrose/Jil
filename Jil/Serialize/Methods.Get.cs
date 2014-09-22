using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Serialize
{
    static partial class Methods
    {
        public static MethodInfo GetCustomISO8601ToString(bool needThunkWriter)
        {
            return
                !needThunkWriter ?
                    CustomISO8601ToString :
                    CustomISO8601ToString_ThunkWriter;
        }

        public static MethodInfo GetCustomWriteIntUnrolledSigned(bool needThunkWriter)
        {
            return
                !needThunkWriter ?
                    CustomWriteIntUnrolledSigned :
                    CustomWriteIntUnrolledSigned_ThunkWriter;
        }

        public static MethodInfo GetCustomWriteInt(bool needThunkWriter)
        {
            return
                !needThunkWriter ?
                    CustomWriteInt :
                    CustomWriteInt_ThunkWriter;
        }

        public static MethodInfo GetCustomWriteUIntUnrolled(bool needThunkWriter)
        {
            return
                !needThunkWriter ?
                    CustomWriteUIntUnrolled :
                    CustomWriteUIntUnrolled_ThunkWriter;
        }

        public static MethodInfo GetCustomWriteUInt(bool needThunkWriter)
        {
            return
                !needThunkWriter ?
                    CustomWriteUInt :
                    CustomWriteUInt_ThunkWriter;
        }

        public static MethodInfo GetCustomWriteLong(bool needThunkWriter)
        {
            return
                !needThunkWriter ?
                    CustomWriteLong :
                    CustomWriteLong_ThunkWriter;
        }

        public static MethodInfo GetCustomWriteULong(bool needThunkWriter)
        {
            return
                !needThunkWriter ?
                    CustomWriteULong :
                    CustomWriteULong_ThunkWriter;
        }

        public static MethodInfo GetProxyFloat(bool needThunkWriter)
        {
            return
                !needThunkWriter ?
                    ProxyFloat :
                    ProxyFloat_ThunkWriter;
        }

        public static MethodInfo GetProxyDouble(bool needThunkWriter)
        {
            return
                !needThunkWriter ?
                    ProxyDouble :
                    ProxyDouble_ThunkWriter;
        }

        public static MethodInfo GetProxyDecimal(bool needThunkWriter)
        {
            return
                !needThunkWriter ?
                    ProxyDecimal :
                    ProxyDecimal_ThunkWriter;
        }

        public static MethodInfo GetWriteGuid(bool needThunkWriter)
        {
            return
                !needThunkWriter ?
                    WriteGuid :
                    WriteGuid_ThunkWriter;
        }

        public static MethodInfo GetWriteEncodedStringWithQuotesWithoutNullsInlineJSONPUnsafe(bool needThunkWriter)
        {
            return
                !needThunkWriter ?
                    WriteEncodedStringWithQuotesWithoutNullsInlineJSONPUnsafe :
                    WriteEncodedStringWithQuotesWithoutNullsInlineJSONPUnsafe_ThunkWriter;
        }

        public static MethodInfo GetWriteEncodedStringWithQuotesWithoutNullsInlineUnsafe(bool needThunkWriter)
        {
            return
                !needThunkWriter ?
                    WriteEncodedStringWithQuotesWithoutNullsInlineUnsafe :
                    WriteEncodedStringWithQuotesWithoutNullsInlineUnsafe_ThunkWriter;
        }

        public static MethodInfo GetWriteEncodedStringWithQuotesWithNullsInlineJSONPUnsafe(bool needThunkWriter)
        {
            return
                !needThunkWriter ?
                    WriteEncodedStringWithQuotesWithNullsInlineJSONPUnsafe :
                    WriteEncodedStringWithQuotesWithNullsInlineJSONPUnsafe_ThunkWriter;
        }

        public static MethodInfo GetWriteEncodedStringWithQuotesWithNullsInlineUnsafe(bool needThunkWriter)
        {
            return
                !needThunkWriter ?
                    WriteEncodedStringWithQuotesWithNullsInlineUnsafe :
                    WriteEncodedStringWithQuotesWithNullsInlineUnsafe_ThunkWriter;
        }

        public static MethodInfo GetWriteEncodedStringWithoutNullsInlineJSONPUnsafe(bool needThunkWriter)
        {
            return
                !needThunkWriter ?
                    WriteEncodedStringWithoutNullsInlineJSONPUnsafe :
                    WriteEncodedStringWithoutNullsInlineJSONPUnsafe_ThunkWriter;
        }

        public static MethodInfo GetWriteEncodedStringWithoutNullsInlineUnsafe(bool needThunkWriter)
        {
            return
                !needThunkWriter ?
                    WriteEncodedStringWithoutNullsInlineUnsafe :
                    WriteEncodedStringWithoutNullsInlineUnsafe_ThunkWriter;
        }

        public static MethodInfo GetWriteEncodedStringWithNullsInlineJSONPUnsafe(bool needThunkWriter)
        {
            return
                !needThunkWriter ?
                    WriteEncodedStringWithNullsInlineJSONPUnsafe :
                    WriteEncodedStringWithNullsInlineJSONPUnsafe_ThunkWriter;
        }

        public static MethodInfo GetWriteEncodedStringWithNullsInlineUnsafe(bool needThunkWriter)
        {
            return
                !needThunkWriter ?
                    WriteEncodedStringWithNullsInlineUnsafe :
                    WriteEncodedStringWithNullsInlineUnsafe_ThunkWriter;
        }
    }
}
