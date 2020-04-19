using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using Feladat.Models;
using System.Linq.Dynamic;

namespace Feladat.Controllers
{
    public static class QueryHelper
    {

        public static IEnumerable<FileModel> PageByOptions(this IEnumerable<FileModel> query, Dictionary<string, object> options)
        {
            if (options.ContainsKey("skip"))
            {
                var skip = Convert.ToInt32(options["skip"]);
                var take = Convert.ToInt32(options["take"]);
                query = query
                    .Skip(skip)
                    .Take(take);
            }
            return query;
        }

        public static IEnumerable<FileModel> SortByOptions(this IEnumerable<FileModel> query, Dictionary<string, object> options)
        {
            if (options.ContainsKey("sortOptions") && options["sortOptions"] != null)
            {
                var sortOptions = JObject.Parse(JArray.FromObject(options["sortOptions"])[0].ToString());
                var columnName = (string)sortOptions.SelectToken("selector");
                var descending = (bool)sortOptions.SelectToken("desc");

                if (descending)
                    columnName += " DESC";
                query = query.OrderBy(columnName);
            }
            return query;
        }


        public static IEnumerable<FileModel> FilterByOptions(this IEnumerable<FileModel> query, string filterOptions)
        {
            var filterTree = JArray.Parse(filterOptions);
            return ReadExpression(query, filterTree);

        }

        public static IEnumerable<FileModel> ReadExpression(IEnumerable<FileModel> source, JArray array)
        {
            if (array[0].Type == JTokenType.String)
                return FilterQuery(source,
                    array[0].ToString(),
                    array[1].ToString(),
                    array[2].ToString());
            else
            {
                for (int i = 0; i < array.Count; i++)
                {
                    if (array[i].ToString().Equals("and"))
                        continue;
                    source = ReadExpression(source, (JArray)array[i]);
                }
                return source;
            }
        }

        public static IEnumerable<FileModel> FilterQuery(IEnumerable<FileModel> source, string ColumnName, string Clause, string Value)
        {
            switch (Clause)
            {
                case "=":
                    Value = System.Text.RegularExpressions.Regex.IsMatch(Value, @"^\d+$") ? Value : String.Format("\"{0}\"", Value);
                    source = source.Where(String.Format("{0} == {1}", ColumnName, Value));
                    break;
                case "contains":
                    source = source.Where(ColumnName + ".Contains(@0)", Value);
                    break;
                case "startswitch":
                    source = source.Where(string.Format("!{0}.StartsWith(\"{1}\")", ColumnName, Value));
                    break;
                case "notcontains":
                    source = source.Where("!" + ColumnName + ".Contains(@0)", Value);
                    break;
                case "endswith":
                    source = source.Where(string.Format("!{0}.EndsWith(\"{1}\")", ColumnName, Value));
                    break;
                case "<>":
                    Value = System.Text.RegularExpressions.Regex.IsMatch(Value, @"^\d+$") ? Value : String.Format("\"{0}\"", Value);
                    source = source.Where(String.Format("{0} != {1}", ColumnName, Value));
                    break;
                default:
                    break;
            }
            return source;
        }
    }
}