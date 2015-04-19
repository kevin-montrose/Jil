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

        public static MethodInfo GetReadInt64(bool readingFromString)
        {
            return
                !readingFromString ?
                    ReadInt64 :
                    ReadInt64ThunkReader;
        }

        public static MethodInfo GetReadUInt64(bool readingFromString)
        {
            return
                !readingFromString ?
                    ReadUInt64 :
                    ReadUInt64ThunkReader;
        }

        public static MethodInfo GetReadDoubleFast(bool readingFromString)
        {
            return
                !readingFromString ?
                    ReadDoubleFast :
                    ReadDoubleFastThunkReader;
        }

        public static MethodInfo GetReadDoubleCharArray(bool readingFromString)
        {
            return
                !readingFromString ?
                    ReadDoubleCharArray :
                    ReadDoubleCharArrayThunkReader;
        }

        public static MethodInfo GetReadDouble(bool readingFromString)
        {
            return
                !readingFromString ?
                    ReadDouble :
                    ReadDoubleThunkReader;
        }

        public static MethodInfo GetReadSingleFast(bool readingFromString)
        {
            return
                !readingFromString ?
                    ReadSingleFast :
                    ReadSingleFastThunkReader;
        }

        public static MethodInfo GetReadSingleCharArray(bool readingFromString)
        {
            return
                !readingFromString ?
                    ReadSingleCharArray :
                    ReadSingleCharArrayThunkReader;
        }

        public static MethodInfo GetReadSingle(bool readingFromString)
        {
            return
                !readingFromString ?
                    ReadSingle :
                    ReadSingleThunkReader;
        }

        public static MethodInfo GetReadDecimalFast(bool readingFromString)
        {
            return
                !readingFromString ?
                    ReadDecimalFast :
                    ReadDecimalFastThunkReader;
        }

        public static MethodInfo GetReadDecimalCharArray(bool readingFromString)
        {
            return
                !readingFromString ?
                    ReadDecimalCharArray :
                    ReadDecimalCharArrayThunkReader;
        }

        public static MethodInfo GetReadDecimal(bool readingFromString)
        {
            return
                !readingFromString ?
                    ReadDecimal :
                    ReadDecimalThunkReader;
        }

        public static MethodInfo GetReadEncodedChar(bool readingFromString)
        {
            return
                !readingFromString ?
                    ReadEncodedChar :
                    ReadEncodedCharThunkReader;
        }

        public static MethodInfo GetReadEncodedStringWithCharArray(bool readingFromString)
        {
            return
                !readingFromString ?
                    ReadEncodedStringWithCharArray :
                    ReadEncodedStringWithCharArrayThunkReader;
        }

        public static MethodInfo GetReadEncodedStringWithBuffer(bool readingFromString)
        {
            return
                !readingFromString ?
                    ReadEncodedStringWithBuffer :
                    ReadEncodedStringWithBufferThunkReader;
        }

        public static MethodInfo GetReadEncodedString(bool readingFromString)
        {
            return
                !readingFromString ?
                    ReadEncodedString :
                    ReadEncodedStringThunkReader;
        }

        public static MethodInfo GetReadGuid(bool readingFromString)
        {
            return
                !readingFromString ?
                    ReadGuid :
                    ReadGuidThunkReader;
        }

        public static MethodInfo GetParseEnum(bool readingFromString)
        {
            return
                !readingFromString ?
                    ParseEnum :
                    ParseEnumThunkReader;
        }

        public static MethodInfo GetReadFlagsEnum(bool readingFromString)
        {
            return
                !readingFromString ?
                    ReadFlagsEnum :
                    ReadFlagsEnumThunkReader;
        }

        public static MethodInfo GetReadSkipWhitespace(bool readingFromString)
        {
            return
                !readingFromString ?
                    ReadSkipWhitespace :
                    ReadSkipWhitespaceThunkReader;
        }

        public static MethodInfo GetSkip(bool readingFromString)
        {
            return
                !readingFromString ?
                    Skip :
                    SkipThunkReader;
        }

        public static MethodInfo GetSkipEncodedString(bool readingFromString)
        {
            return
                !readingFromString ?
                    SkipEncodedString :
                    SkipEncodedStringThunkReader;
        }
    }
}
