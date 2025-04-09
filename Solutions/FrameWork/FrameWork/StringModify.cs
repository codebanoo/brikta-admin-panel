using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace FrameWork
{
    public static class StringModify
    {
        public static string RemoveLastCharacters(string myStr, int count = 1)
        {
            try
            {
                if (myStr.Length > 0)
                {
                    myStr = myStr.Substring(0, myStr.Length - count);
                }
                return myStr;
            }
            catch (Exception exc)
            {
                return "error";
            }
        }

        public static string RemoveFirstCharacters(string myStr, int count = 1)
        {
            try
            {
                if (myStr.Length > 0)
                {
                    myStr = myStr.Substring(count, myStr.Length);
                }
                return myStr;
            }
            catch (Exception exc)
            {
                return "error";
            }
        }

        public static string SafeFarsiStr(string input)
        {
            string data = input.Replace("ی", "ی").Replace("ک", "ک");
            return data;
        }

        public static bool RecognizeUtf8(string str)
        {
            try
            {
                int asciiBytesCount = System.Text.Encoding.ASCII.GetByteCount(str);
                int unicodBytesCount = System.Text.Encoding.UTF8.GetByteCount(str);
                if (asciiBytesCount != unicodBytesCount)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return true;
            }
        }

        public static Encoding GetEncoding(string str)
        {
            // Read the BOM
            var bom = new byte[str.Length];
            bom = GetBytes(str);

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;
            return Encoding.ASCII;
        }

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static string RemoveHTMLTags(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public static string ReplaceLastOccurrence(string Source, string Find, string Replace)
        {
            int place = Source.LastIndexOf(Find);

            if (place == -1)
                return Source;

            string result = Source.Remove(place, Find.Length).Insert(place, Replace);
            return result;
        }

        public static string FirstCharToUpper(this string input) =>
            input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
            };

        public static string FirstCharToLower(this string input) =>
            input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => string.Concat(input[0].ToString().ToLower(), input.AsSpan(1))
            };

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}