using Sigil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Serialize
{
   interface ISerializeOptions
   {
      bool PrettyPrint { get; }
      bool ExcludeNulls { get; }
      DateTimeFormat DateFormat { get; }
      bool JSONP { get; }
      bool IncludeInherited { get; }
      UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get; }
      bool DontCoerceToDateOnSerialize { get; }
      SerializationNameFormat SerializationNameFormat { get; }
   }

   static class TypeCache<TOptions, T>
       where TOptions : ISerializeOptions, new()
   {
      static readonly object ThunkInitLock = new object();
      static volatile bool ThunkBeingBuilt = false;
      public static volatile Action<TextWriter, T, int> Thunk;
      public static Exception ThunkExceptionDuringBuild;

      static readonly object StringThunkInitLock = new object();
      static volatile bool StringThunkBeingBuilt = false;
      public static volatile StringThunkDelegate<T> StringThunk;
      public static Exception StringThunkExceptionDuringBuild;

      public static Action<TextWriter, T, int> Get()
      {
         Load();
         return Thunk;
      }

      public static void Load()
      {
         if (Thunk != null) return;

         lock (ThunkInitLock)
         {
            if (Thunk != null || ThunkBeingBuilt) return;
            ThunkBeingBuilt = true;

            var opts = new TOptions();

            Thunk = InlineSerializerHelper.Build<T>(typeof(TOptions), pretty: opts.PrettyPrint, excludeNulls: opts.ExcludeNulls, dateFormat: opts.DateFormat, jsonp: opts.JSONP, includeInherited: opts.IncludeInherited, dateTimeBehavior: opts.DateTimeKindBehavior, dontCoerceToDateOnSerialize: opts.DontCoerceToDateOnSerialize, serializationNameFormat: opts.SerializationNameFormat, exceptionDuringBuild: out ThunkExceptionDuringBuild);
         }
      }

      public static StringThunkDelegate<T> GetToString()
      {
         LoadToString();
         return StringThunk;
      }

      public static void LoadToString()
      {
         if (StringThunk != null) return;

         lock (StringThunkInitLock)
         {
            if (StringThunk != null || StringThunkBeingBuilt) return;
            StringThunkBeingBuilt = true;

            var opts = new TOptions();

            StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(TOptions), pretty: opts.PrettyPrint, excludeNulls: opts.ExcludeNulls, dateFormat: opts.DateFormat, jsonp: opts.JSONP, includeInherited: opts.IncludeInherited, dateTimeBehavior: opts.DateTimeKindBehavior, dontCoerceToDateOnSerialize: opts.DontCoerceToDateOnSerialize, serializationNameFormat: opts.SerializationNameFormat, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
         }
      }
   }

   // Start OptionsGeneration.linq generated content
   class MicrosoftStyle : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrint : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleExcludeNulls : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleJSONP : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintExcludeNulls : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintJSONP : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleExcludeNullsJSONP : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleExcludeNullsInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleExcludeNullsUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleExcludeNullsCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleExcludeNullsDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleJSONPInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleJSONPUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleJSONPCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleJSONPDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsJSONP : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintJSONPInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintJSONPUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintJSONPCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintJSONPDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleExcludeNullsJSONPInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleExcludeNullsJSONPUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleExcludeNullsJSONPCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleExcludeNullsJSONPDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleExcludeNullsInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleExcludeNullsInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleExcludeNullsInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleExcludeNullsUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleExcludeNullsUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleExcludeNullsCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleJSONPInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleJSONPInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleJSONPInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleJSONPUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleJSONPUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleJSONPCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsJSONPUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsJSONPCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsJSONPDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintJSONPInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintJSONPInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintJSONPInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintJSONPUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintJSONPUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintJSONPCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleExcludeNullsJSONPInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleExcludeNullsJSONPInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleExcludeNullsJSONPUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleExcludeNullsJSONPUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleExcludeNullsInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleExcludeNullsInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleJSONPInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleJSONPInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleJSONPInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleJSONPUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsJSONPUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsJSONPUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintJSONPInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintJSONPInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintJSONPInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintJSONPUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleExcludeNullsJSONPInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStyleExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStyleExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601 : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrint : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601ExcludeNulls : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601JSONP : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601Inherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601Utc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601CamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601DontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintExcludeNulls : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintJSONP : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601ExcludeNullsJSONP : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601ExcludeNullsInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601ExcludeNullsUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601ExcludeNullsCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601ExcludeNullsDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601JSONPInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601JSONPUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601JSONPCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601JSONPDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601InheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601InheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601InheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601UtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601UtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601CamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintExcludeNullsJSONP : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintExcludeNullsInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintExcludeNullsUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintExcludeNullsCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintExcludeNullsDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintJSONPInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintJSONPUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintJSONPCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintJSONPDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601ExcludeNullsJSONPInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601ExcludeNullsJSONPUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601ExcludeNullsJSONPCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601ExcludeNullsJSONPDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601ExcludeNullsInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601ExcludeNullsInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601ExcludeNullsInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601ExcludeNullsUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601ExcludeNullsUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601ExcludeNullsCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601JSONPInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601JSONPInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601JSONPInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601JSONPUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601JSONPUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601JSONPCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601InheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601InheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601InheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601UtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintExcludeNullsJSONPUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintExcludeNullsJSONPCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintExcludeNullsJSONPDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintExcludeNullsInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintExcludeNullsInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintExcludeNullsInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintExcludeNullsUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintExcludeNullsUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintExcludeNullsCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintJSONPInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintJSONPInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintJSONPInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintJSONPUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintJSONPUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintJSONPCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601ExcludeNullsJSONPInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601ExcludeNullsJSONPInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601ExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601ExcludeNullsJSONPUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601ExcludeNullsJSONPUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601ExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601ExcludeNullsInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601ExcludeNullsInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601ExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601ExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601JSONPInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601JSONPInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601JSONPInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601JSONPUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601InheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintExcludeNullsJSONPInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintExcludeNullsJSONPInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintExcludeNullsJSONPUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintExcludeNullsJSONPUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintExcludeNullsInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintExcludeNullsInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintJSONPInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintJSONPInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintJSONPInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintJSONPUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601ExcludeNullsJSONPInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601ExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601ExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601ExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601ExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601JSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintExcludeNullsJSONPInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class ISO8601PrettyPrintExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601ExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class ISO8601PrettyPrintExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class Milliseconds : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrint : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsExcludeNulls : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsJSONP : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintExcludeNulls : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintJSONP : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsExcludeNullsJSONP : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsExcludeNullsInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsExcludeNullsUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsExcludeNullsCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsExcludeNullsDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsJSONPInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsJSONPUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsJSONPCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsJSONPDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintExcludeNullsJSONP : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintExcludeNullsInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintExcludeNullsUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintExcludeNullsCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintExcludeNullsDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintJSONPInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintJSONPUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintJSONPCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintJSONPDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsExcludeNullsJSONPInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsExcludeNullsJSONPUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsExcludeNullsJSONPCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsExcludeNullsJSONPDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsExcludeNullsInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsExcludeNullsInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsExcludeNullsInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsExcludeNullsUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsExcludeNullsUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsExcludeNullsCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsJSONPInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsJSONPInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsJSONPInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsJSONPUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsJSONPUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsJSONPCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintExcludeNullsJSONPUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintExcludeNullsJSONPCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintExcludeNullsJSONPDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintExcludeNullsInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintExcludeNullsInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintExcludeNullsInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintExcludeNullsUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintExcludeNullsUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintExcludeNullsCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintJSONPInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintJSONPInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintJSONPInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintJSONPUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintJSONPUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintJSONPCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsExcludeNullsJSONPInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsExcludeNullsJSONPInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsExcludeNullsJSONPUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsExcludeNullsJSONPUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsExcludeNullsInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsExcludeNullsInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsJSONPInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsJSONPInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsJSONPUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintExcludeNullsJSONPInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintExcludeNullsJSONPInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintExcludeNullsJSONPUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintExcludeNullsJSONPUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintExcludeNullsInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintExcludeNullsInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintJSONPInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintJSONPInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintJSONPInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintJSONPUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsExcludeNullsJSONPInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintExcludeNullsJSONPInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class MillisecondsPrettyPrintExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class MillisecondsPrettyPrintExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123 : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrint : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123ExcludeNulls : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123JSONP : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123Inherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123Utc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123CamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123DontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintExcludeNulls : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintJSONP : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123ExcludeNullsJSONP : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123ExcludeNullsInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123ExcludeNullsUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123ExcludeNullsCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123ExcludeNullsDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123JSONPInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123JSONPUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123JSONPCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123JSONPDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123InheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123InheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123InheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123UtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123UtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123CamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintExcludeNullsJSONP : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintExcludeNullsInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintExcludeNullsUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintExcludeNullsCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintExcludeNullsDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintJSONPInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintJSONPUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintJSONPCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintJSONPDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123ExcludeNullsJSONPInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123ExcludeNullsJSONPUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123ExcludeNullsJSONPCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123ExcludeNullsJSONPDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123ExcludeNullsInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123ExcludeNullsInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123ExcludeNullsInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123ExcludeNullsUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123ExcludeNullsUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123ExcludeNullsCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123JSONPInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123JSONPInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123JSONPInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123JSONPUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123JSONPUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123JSONPCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123InheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123InheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123InheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123UtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintExcludeNullsJSONPUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintExcludeNullsJSONPCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintExcludeNullsJSONPDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintExcludeNullsInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintExcludeNullsInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintExcludeNullsInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintExcludeNullsUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintExcludeNullsUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintExcludeNullsCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintJSONPInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintJSONPInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintJSONPInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintJSONPUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintJSONPUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintJSONPCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123ExcludeNullsJSONPInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123ExcludeNullsJSONPInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123ExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123ExcludeNullsJSONPUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123ExcludeNullsJSONPUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123ExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123ExcludeNullsInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123ExcludeNullsInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123ExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123ExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123JSONPInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123JSONPInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123JSONPInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123JSONPUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123InheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintExcludeNullsJSONPInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintExcludeNullsJSONPInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintExcludeNullsJSONPUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintExcludeNullsJSONPUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintExcludeNullsInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintExcludeNullsInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintJSONPInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintJSONPInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintJSONPInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintJSONPUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123ExcludeNullsJSONPInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123ExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123ExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123ExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123ExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123JSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintExcludeNullsJSONPInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class RFC1123PrettyPrintExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123ExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class RFC1123PrettyPrintExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class Seconds : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrint : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsExcludeNulls : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsJSONP : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintExcludeNulls : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintJSONP : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsExcludeNullsJSONP : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsExcludeNullsInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsExcludeNullsUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsExcludeNullsCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsExcludeNullsDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsJSONPInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsJSONPUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsJSONPCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsJSONPDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintExcludeNullsJSONP : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintExcludeNullsInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintExcludeNullsUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintExcludeNullsCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintExcludeNullsDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintJSONPInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintJSONPUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintJSONPCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintJSONPDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsExcludeNullsJSONPInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsExcludeNullsJSONPUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsExcludeNullsJSONPCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsExcludeNullsJSONPDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsExcludeNullsInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsExcludeNullsInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsExcludeNullsInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsExcludeNullsUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsExcludeNullsUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsExcludeNullsCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsJSONPInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsJSONPInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsJSONPInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsJSONPUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsJSONPUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsJSONPCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintExcludeNullsJSONPUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintExcludeNullsJSONPCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintExcludeNullsJSONPDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintExcludeNullsInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintExcludeNullsInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintExcludeNullsInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintExcludeNullsUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintExcludeNullsUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintExcludeNullsCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintJSONPInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintJSONPInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintJSONPInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintJSONPUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintJSONPUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintJSONPCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsExcludeNullsJSONPInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsExcludeNullsJSONPInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsExcludeNullsJSONPUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsExcludeNullsJSONPUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsExcludeNullsInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsExcludeNullsInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsJSONPInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsJSONPInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsJSONPUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintExcludeNullsJSONPInheritedUtc : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintExcludeNullsJSONPInheritedCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintExcludeNullsJSONPUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintExcludeNullsJSONPUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintExcludeNullsInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintExcludeNullsInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintJSONPInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintJSONPInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintJSONPInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintJSONPUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsExcludeNullsJSONPInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintExcludeNullsJSONPInheritedUtcCamelCase : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return false; } }
   }

   class SecondsPrettyPrintExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.Verbatim; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return false; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return false; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return false; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return false; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }

   class SecondsPrettyPrintExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize : ISerializeOptions
   {
      public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
      public bool PrettyPrint { get { return true; } }
      public bool ExcludeNulls { get { return true; } }
      public bool JSONP { get { return true; } }
      public bool IncludeInherited { get { return true; } }
      public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
      public SerializationNameFormat SerializationNameFormat { get { return SerializationNameFormat.CamelCase; } }
      public bool DontCoerceToDateOnSerialize { get { return true; } }
   }
   // End OptionsGeneration.linq generated content
}