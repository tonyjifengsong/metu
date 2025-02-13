using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace System
{
    public static class GroupsExtentions
    {
    }
    public static class StringExtension
    {
        public static ChineseString AsChineseString(this string s) { return new ChineseString(s); }
        public static ConvertableString AsConvertableString(this string s) { return new ConvertableString(s); }
        public static RegexableString AsRegexableString(this string s) { return new RegexableString(s); }
    }
    public class ChineseString
    {
        private string s;
        public ChineseString(string s) { this.s = s; }
        //转全角
        public string ToSBC(string input) { throw new NotImplementedException(); }
        //转半角
        public string ToDBC(string input) { throw new NotImplementedException(); }
        //获取汉字拼音首字母
        public string GetChineseSpell(string input) { throw new NotImplementedException(); }
    }
    public class ConvertableString
    {
        private string s;
        public ConvertableString(string s) { this.s = s; }
        public bool IsInt(string s) { throw new NotImplementedException(); }
        public bool IsDateTime(string s) { throw new NotImplementedException(); }
        public int ToInt(string s) { throw new NotImplementedException(); }
        public DateTime ToDateTime(string s) { throw new NotImplementedException(); }
    }
    public class RegexableString
    {
        private string s;
        public RegexableString(string s) { this.s = s; }
        public bool IsMatch(string s, string pattern) { throw new NotImplementedException(); }
        public string Match(string s, string pattern) { throw new NotImplementedException(); }
        public string Relplace(string s, string pattern, MatchEvaluator evaluator) { throw new NotImplementedException(); }
    }

    public interface IExtensions<V>
    {
        V GetValue();
    }

    public static class ExtensionGroup
    {
        private static Dictionary<Type, Type> cache = new Dictionary<Type, Type>();

        public static T As<T>(this string v) where T : IExtensions<string>
        {
            return As<T, string>(v);
        }

        public static T As<T, V>(this V v) where T : IExtensions<V>
        {
            Type t;
            Type valueType = typeof(V);
            if (cache.ContainsKey(valueType))
            {
                t = cache[valueType];
            object result = Activator.CreateInstance(t, v);
                return (T)result;
            }
            
            return default(T);
        }
      
    }

}
