// This file adapted from: http://svn.apache.org/viewvc/xmlgraphics/commons/trunk/src/java/org/apache/xmlgraphics/util/DoubleFormatUtil.java?revision=1346428&view=co
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Serialize
{
    partial class Utils
    {
        /*
         * Licensed to the Apache Software Foundation (ASF) under one or more
         * contributor license agreements.  See the NOTICE file distributed with
         * this work for additional information regarding copyright ownership.
         * The ASF licenses this file to You under the Apache License, Version 2.0
         * (the "License"); you may not use this file except in compliance with
         * the License.  You may obtain a copy of the License at
         *
         *      http://www.apache.org/licenses/LICENSE-2.0
         *
         * Unless required by applicable law or agreed to in writing, software
         * distributed under the License is distributed on an "AS IS" BASIS,
         * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
         * See the License for the specific language governing permissions and
         * limitations under the License.
         */
        static class FastDoubleFormatter
        {
            private static readonly long[] POWERS_OF_TEN_LONG = new long[19];
            private static readonly double[] POWERS_OF_TEN_DOUBLE = new double[30];
            static FastDoubleFormatter()
            {
                POWERS_OF_TEN_LONG[0] = 1L;
                for (int i = 1; i < POWERS_OF_TEN_LONG.Length; i++)
                {
                    POWERS_OF_TEN_LONG[i] = POWERS_OF_TEN_LONG[i - 1] * 10L;
                }
                for (int i = 0; i < POWERS_OF_TEN_DOUBLE.Length; i++)
                {
                    POWERS_OF_TEN_DOUBLE[i] = double.Parse("1e" + i);
                }
            }

            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            private static bool IsRoundedToZero(double source, int decimals, int precision)
            {
                return source == 0.0 || Math.Abs(source) < 4.999999999999999 / TenPowDouble(Math.Max(decimals, precision) + 1);
            }

            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            private static double TenPowDouble(int n)
            {
                return n < POWERS_OF_TEN_DOUBLE.Length ? POWERS_OF_TEN_DOUBLE[n] : Math.Pow(10, n);
            }

            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void FormatDoubleFast(double source, int decimals, int precision, TextWriter target)
            {
                if (IsRoundedToZero(source, decimals, precision))
                {
                    target.Write('0');
                    return;
                }
                else
                {
                    if (double.IsNaN(source) || double.IsInfinity(source))
                    {
                        target.Write(source.ToString());
                        return;
                    }
                }

                var isPositive = source >= 0.0;
                source = Math.Abs(source);
                var scale = (source >= 1.0) ? decimals : precision;

                var intPart = (long)Math.Floor(source);
                var tenScale = TenPowDouble(scale);
                var fracUnroundedPart = (source - intPart) * tenScale;
                long fracPart = (long)Math.Round(fracUnroundedPart);
                if (fracPart >= tenScale)
                {
                    intPart++;
                    fracPart = (long)Math.Round(fracPart - tenScale);
                }
                if (fracPart != 0L)
                {
                    while (fracPart % 10L == 0L)
                    {
                        fracPart = fracPart / 10L;
                        scale--;
                    }
                }

                if (intPart != 0L || fracPart != 0L)
                {
                    if (!isPositive)
                    {
                        target.Write('-');
                    }

                    target.Write(intPart);
                    if (fracPart != 0L)
                    {
                        target.Write('.');

                        while (scale > 0 && fracPart < TenPowDouble(--scale))
                        {
                            target.Write('0');
                        }

                        target.Write(fracPart);
                    }
                }
                else
                {
                    target.Write('0');
                }
            }
        }
    }
}
