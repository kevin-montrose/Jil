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
        const int BufferSizeShift = sizeof(long) / sizeof(char);

        int BufferIx;
        char[] Buffer;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe void ArrayCopyFast(char[] smaller, char[] larger)
        {
            fixed (char* fromPtrFixed = smaller)
            fixed (char* intoPtrFixed = larger)
            {
                var fromPtr = (long*)fromPtrFixed;
                var intoPtr = (long*)intoPtrFixed;

                var longLen = smaller.Length >> BufferSizeShift;

                while (longLen > 0)
                {
                    *intoPtr = *fromPtr;
                    fromPtr++;
                    intoPtr++;
                    longLen--;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void AssureSpace(int neededSpace)
        {
            if (Buffer == null)
            {
                Buffer = new char[((neededSpace >> BufferSizeShift) + 1) << BufferSizeShift];
                return;
            }

            var desiredSize = BufferIx + neededSpace;

            if (Buffer.Length > desiredSize) return;

            var newBuffer = new char[((desiredSize >> BufferSizeShift) + 1) << BufferSizeShift];
            Array.Copy(Buffer, newBuffer, Buffer.Length);
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
                var bufferPtr = fixedBufferPtr + BufferIx;
                var strPtr = fixedStrPtr;

                while (newChars > 0)
                {
                    *bufferPtr = *strPtr;
                    bufferPtr++;
                    strPtr++;
                    newChars--;
                }
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

                while (newChars > 0)
                {
                    *bufferPtr = *strPtr;
                    bufferPtr++;
                    strPtr++;
                    newChars--;
                }
            }

            BufferIx += len;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteTo(TextWriter writer)
        {
            writer.Write(Buffer, 0, BufferIx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe string StaticToString()
        {
            return new string(Buffer, 0, BufferIx);
        }

        public override string ToString()
        {
            return StaticToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            BufferIx = 0;
        }
    }
}
