using System;
using System.Collections.Generic;
using System.Text;

namespace Lazorm
{
    /// <summary>
    /// 名前の変換クラス
    /// </summary>
    public class NameConverter
    {
        /// <summary>
        /// 単数形に変換する
        /// 不完全
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Singularize(string source)
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("children", "child");
            dictionary.Add("cruces", "crux");
            dictionary.Add("feet", "foot");
            dictionary.Add("knives", "knife");
            dictionary.Add("leaves", "leaf");
            dictionary.Add("lice", "louse");
            dictionary.Add("men", "man");
            dictionary.Add("media", "medium");
            dictionary.Add("mice", "mouse");
            dictionary.Add("oases", "oasis");
            dictionary.Add("people", "person");
            dictionary.Add("phenomena", "phenomenon");
            dictionary.Add("seamen", "seaman");
            dictionary.Add("snowmen", "snowman");
            dictionary.Add("teeth", "tooth");
            dictionary.Add("women", "woman");

            if (dictionary.ContainsKey(source))
                return dictionary[source];

            if (source.Length <= 3)
                return source;

            if (source.Substring(source.Length - 3, 3) == "ies")
                return source.Substring(0, source.Length - 3) + "y";
            else if (source.Substring(source.Length - 1, 1) == "s")
                return source.Substring(0, source.Length - 1);
            else
                return source;
        }

        //public static string Pluralize(string source)
        //{
        //    var dictionary = new Dictionary<string, string>();
        //    dictionary.Add("child", "children");
        //    dictionary.Add("crux", "cruces");
        //    dictionary.Add("foot", "feet");
        //    dictionary.Add("knife", "knives");
        //    dictionary.Add("leaf", "leaves");
        //    dictionary.Add("louse", "lice");
        //    dictionary.Add("man", "men");
        //    dictionary.Add("medium", "media");
        //    dictionary.Add("mouse", "mice");
        //    dictionary.Add("oasis", "oases");
        //    dictionary.Add("person", "people");
        //    dictionary.Add("phenomenon", "phenomena");
        //    dictionary.Add("seaman", "seamen");
        //    dictionary.Add("snowman", "snowmen");
        //    dictionary.Add("tooth", "teeth");
        //    dictionary.Add("woman", "women");

        //}
    }
}
