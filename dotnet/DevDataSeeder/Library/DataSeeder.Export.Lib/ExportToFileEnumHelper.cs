using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace DataSeeder.Export.Library
{

    public enum FileCompressionTypeEnum
    {
        [Description("None")]
        None = 0,
        [Description(".gzip")]
        gzip
        //zip ,
        //lzop
    }

    public enum ExportToFileTypeEnum
    {
        [Description(".csv")]
        csv = 0,
        [Description(".txt")]
        txt,
        [Description(".json")]
        json
    }

    public enum ColumnDelimiter
    {
          [Description("1.None")]
          NONE = 0
        , [Description("2.{CR}{LF}")]
          CarriageReturnLineFeed
        , [Description("3.{CR}")]
          CarriageReturn
        , [Description("4.{LF}")]
          LineFeed
        , [Description("5.Semicolon {;}")]
          Semicolon
        , [Description("6.Colon {:}")]
          Colon
        , [Description("7.Comma {,}")]
          Comma
        , [Description("8.Tab {t}")]
          Tab
        , [Description("9.Vertical Bar {|}")]
          VerticalBar
    }

    public enum RowDelimiters
    {
        [Description("1.None")]
        NONE = 0
        , [Description("2.{CR}{LF}")]
        CarriageReturnLineFeed
        , [Description("3.{CR}")]
        CarriageReturn
        , [Description("4.{LF}")]
        LineFeed
        , [Description("5.Semicolon {;}")]
        Semicolon
        , [Description("6.Colon {:}")]
        Colon
        , [Description("7.Comma {,}")]
        Comma
        , [Description("8.Tab {t}")]
        Tab
        , [Description("9.Vertical Bar {|}")]
        VerticalBar
    }

    public enum DateFormat
    {
        [Description("None")]
        None,
        [Description("MMddyyyy")]
        MMddyyyy,
        [Description("MM_dd_yyyy")]
        MM_dd_yyyy,
        [Description("MMddyyyyHHmmss")]
        MMddyyyyHHmmss,
        [Description("MMddyyyyHHmmssfff")]
        MMddyyyyHHmmssfff,
        [Description("MMddyyyy_HHmmssfff")]
        MMddyyyy_HHmmssfff,
        [Description("yyyyMMdd")]
        yyyyMMdd,
        [Description("yyyy_MM_dd")]
        yyyy_MM_dd,
        [Description("yyyyMMddHHmmss")]
        yyyyMMddHHmmss,
        [Description("yyyyMMddHHmmssfff")]
        yyyyMMddHHmmssfff,
        [Description("yyyyMMdd_HHmmssfff")]
        yyyyMMdd_HHmmssfff
    }

    public static class EnumHelper
    {
        public static string GetDescription<T>(T enumerationValue)
        //where T : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();
        }
        public static Dictionary<int, string> EnumDictionary<T>()
        {
            Dictionary<int, string> RetDict = new Dictionary<int, string>();

            if (!typeof(T).IsEnum)
                throw new ArgumentException("Type must be an enum");
            foreach (T v in Enum.GetValues(typeof(T)))
            {
                RetDict.Add((int)(object)v, GetDescription(v));
            }

            return RetDict;

            //return Enum.GetValues(typeof(T))
            //    .Cast<T>()
            //    .ToDictionary(t => (int)(object)t, t => t.ToString());
        }
    }

    public static class ExportToFileEnumHelper
    {
        public static SortedDictionary<int, string> FileEncodingTypeStrings()
        {
            SortedDictionary<int, string> RetDictionary = new SortedDictionary<int, string>();

            foreach (System.Text.EncodingInfo e in System.Text.Encoding.GetEncodings())
            {
                if (SupportedEncoding().Contains(e.DisplayName))
                    RetDictionary.Add(e.CodePage, e.DisplayName);
            }

            return RetDictionary;
        }

        private static List<string> SupportedEncoding()
        {
            List<string> AllowedEncodingList = new List<string>()
            {
                "Unicode (UTF-7)",
                "Unicode (UTF-8)",
                "Unicode (Big-Endian)",
                "Unicode",
                "Unicode (UTF-32)",
                "US-ASCII"
            };

            // return
            return AllowedEncodingList;
        }

        public static string ColumnDelimiterToString(ColumnDelimiter Coldelimiter)
        {
            string strRet = string.Empty;

            switch (Coldelimiter)
            {
                case ColumnDelimiter.CarriageReturnLineFeed:
                    strRet = System.Environment.NewLine;
                    break;
                case ColumnDelimiter.CarriageReturn:
                    strRet = "\r";
                    break;
                case ColumnDelimiter.LineFeed:
                    strRet = "\n";
                    break;
                case ColumnDelimiter.Semicolon:
                    strRet = ";";
                    break;
                case ColumnDelimiter.Colon:
                    strRet = ":";
                    break;
                case ColumnDelimiter.Comma:
                    strRet = ",";
                    break;
                case ColumnDelimiter.Tab:
                    strRet = "\t";
                    break;
                case ColumnDelimiter.VerticalBar:
                    strRet = "|";
                    break;
                default:
                    strRet = "";
                    break;
            }

            return strRet;
        }

        public static string RowDelimiterToString(RowDelimiters Rowdelimiter)
        {
            string strRet = string.Empty;

            switch (Rowdelimiter)
            {
                case RowDelimiters.CarriageReturnLineFeed:
                    strRet = System.Environment.NewLine;
                    break;
                case RowDelimiters.CarriageReturn:
                    strRet = "\r";
                    break;
                case RowDelimiters.LineFeed:
                    strRet = "\n";
                    break;
                case RowDelimiters.Semicolon:
                    strRet = ";";
                    break;
                case RowDelimiters.Colon:
                    strRet = ":";
                    break;
                case RowDelimiters.Comma:
                    strRet = ",";
                    break;
                case RowDelimiters.Tab:
                    strRet = "\t";
                    break;
                case RowDelimiters.VerticalBar:
                    strRet = "|";
                    break;
                default:
                    strRet = "";
                    break;
            }

            return strRet;
        }

        public static SortedDictionary<int, string> CompressionFileTypeStrings()
        {
            SortedDictionary<int, string> FileCmpDictionary = new SortedDictionary<int, string>
            {
                { (int)FileCompressionTypeEnum.None, "None" },
                { (int)FileCompressionTypeEnum.gzip, ".gzip" }
            };
            //FileCmpDictionary.Add((int)FileCompressionTypeEnum.lzop, ".lzop");
            //FileCmpDictionary.Add((int)FileCompressionTypeEnum.zip, ".zip" );

            return FileCmpDictionary;
        }
    }
}
