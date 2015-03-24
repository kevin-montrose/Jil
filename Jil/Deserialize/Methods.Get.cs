using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Deserialize
{
    static partial class Methods
    {
        public static MethodInfo GetConsumeWhiteSpace(bool readingFromString)
        {
            return
                !readingFromString ?
                    ConsumeWhiteSpace :
                    ConsumeWhiteSpaceThunkReader;
        }

        public static MethodInfo GetReadUInt8(bool readingFromString)
        {
            return
                !readingFromString ?
                    ReadUInt8 :
                    ReadUInt8ThunkReader;
        }

        public static MethodInfo GetReadInt8(bool readingFromString)
        {
            return
                !readingFromString ?
                    ReadInt8 :
                    ReadInt8ThunkReader;
        }

        public static MethodInfo GetReadInt16(bool readingFromString)
        {
            return
                !readingFromString ?
                    ReadInt16 :
                    ReadInt16ThunkReader;
        }

        public static MethodInfo GetReadUInt16(bool readingFromString)
        {
            return
                !readingFromString ?
                    ReadUInt16 :
                    ReadUInt16ThunkReader;
        }

        public static MethodInfo GetReadInt32(bool readingFromString)
        {
            return
                !readingFromString ?
                    ReadInt32 :
                    ReadInt32ThunkReader;
        }

        public static MethodInfo GetReadUInt32(bool readingFromString)
        {
            return
                !readingFromString ?
                    ReadUInt32 :
                    ReadUInt32ThunkReader;
        }
    }
}
