using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Deserialize
{
    struct CustomStringBuilder
    {
        const int LongSizeShift = 2;

        int BufferIx;
        char[] Buffer;

        // This method only works if the two arrays are 8-byte/4-char/1-long aligned (ie. size is a multiple; .NET handles
        //   putting them in the right alignment, we just have to guarantee the size)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe void ArrayCopyAligned(char[] smaller, char[] larger)
        {
            fixed (char* fromPtrFixed = smaller)
            fixed (char* intoPtrFixed = larger)
            {
                var fromPtr = (long*)fromPtrFixed;
                var intoPtr = (long*)intoPtrFixed;

                var longLen = smaller.Length >> LongSizeShift;

                while (longLen > 0)
                {
                    *intoPtr = *fromPtr;
                    fromPtr++;
                    intoPtr++;
                    longLen--;
                }
            }
        }

        // This can handle arbitrary arrays, but is slower than ArrayCopyAligned
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe void ArrayCopy(char* fromPtrFixed, int fromLength, char* intoPtrFixed)
        {
            var fromPtr = fromPtrFixed;
            var intoPtr = intoPtrFixed;

            var copyLongs = fromLength >> LongSizeShift;
            var fromLongPtr = (long*)fromPtr;
            var intoLongPtr = (long*)intoPtr;
            while (copyLongs > 0)
            {
                *intoLongPtr = *fromLongPtr;
                intoLongPtr++;
                fromLongPtr++;
                copyLongs--;
            }

            var copyInt = (fromLength & 0x2) != 0;
            var copyChar = (fromLength & 0x1) != 0;
            if (copyInt)
            {
                var fromIntPtr = (int*)fromLongPtr;
                var intoIntPtr = (int*)intoLongPtr;
                *intoIntPtr = *fromIntPtr;
                fromIntPtr++;
                intoIntPtr++;

                if (copyChar)
                {
                    var fromCharPtr = (char*)fromIntPtr;
                    var intoCharPtr = (char*)intoIntPtr;
                    *intoCharPtr = *fromCharPtr;
                }
            }
            else
            {
                if (copyChar)
                {
                    var fromCharPtr = (char*)fromLongPtr;
                    var intoCharPtr = (char*)intoLongPtr;
                    *intoCharPtr = *fromCharPtr;
                }
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void AssureSpace(int neededSpace)
        {
            if (Buffer == null)
            {
                Buffer = new char[((neededSpace >> LongSizeShift) + 1) << LongSizeShift];
                return;
            }

            var desiredSize = BufferIx + neededSpace;

            if (Buffer.Length > desiredSize) return;

            var newBuffer = new char[((desiredSize >> LongSizeShift) + 1) << LongSizeShift];
            ArrayCopyAligned(Buffer, newBuffer);
            Buffer = newBuffer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Append(string str)
        {
            var newChars = str.Length;
            AssureSpace(newChars);

            fixed (char* fixedBufferPtr = Buffer)
            fixed (char* fixedStrPtr = str)
            {
                var copyInto = fixedBufferPtr + BufferIx;
                ArrayCopy(fixedStrPtr, newChars, copyInto);
            }

            BufferIx += str.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(char c)
        {
            AssureSpace(1);

            Buffer[BufferIx] = c;
            BufferIx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Append(char[] chars, int start, int len)
        {
            var newChars = len;
            AssureSpace(newChars);

            fixed (char* fixedBufferPtr = Buffer)
            fixed (char* fixedCharsPtr = chars)
            {
                var bufferPtr = fixedBufferPtr + BufferIx;
                var strPtr = fixedCharsPtr + start;

                ArrayCopy(strPtr, len, bufferPtr);
            }

            BufferIx += len;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe string StaticToString()
        {
            return new string(Buffer, 0, BufferIx);
        }

        public override string ToString()
        {
            if (Buffer == null) return "";

            return StaticToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            BufferIx = 0;
        }
    }
}
