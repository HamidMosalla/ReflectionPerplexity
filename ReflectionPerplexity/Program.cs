using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var person2 = new Person
            {
                FirstName = "Hamid",
                LastName = "Mosalla",
                Experience = new Experience
                {
                    ExperienceType = "Work",
                    Expedience = new Expedience
                    {
                        Sophistry = "Straw man",
                        Exuberance = new Exuberance
                        {
                            ExType = "Very Bubbly"
                        }
                    }
                }
            };

            var attribNames = new[] {"RequEstingParty", "requestingParty2"};

            var propName2 = new Person()
                .GetType()
                .GetProperties()
                .Where(a => a.GetCustomAttributes()
                    .Where(c=> c is JsonPropertyNameAttribute)
                    .Cast<JsonPropertyNameAttribute>()
                    .Any(a=> attribNames.Select(a=> a.ToLower()).Contains(a.Name.ToLower())))
                .ToList();

            var parentObject = "experience.expedience.exuberance.exType".Remove("experience.expedience.exuberance.exType".LastIndexOf('.'));
            var exuberance = GetSubPropertyValue(person2, parentObject);
            var exType = GetSubProperty(person2, "experience.expedience.exuberance.exType");



            exType.SetValue(exuberance, "Mia", null);

            var names = new Person().GetType().GetProperties().Select(a => a.Name).ToList();

            var json = File.ReadAllText(@"C:\Users\Hamid\source\repos\ConsoleApp1\ConsoleApp1\query.json");

            var result = JsonSerializer.Deserialize<Result>(json);

            var person = new Person { FirstName = "Hamid", LastName = "Mosalla" };

            MergeHighlightedItems(person, result);

            int num = 1;
        }

        public static PropertyInfo GetSubProperty(object value, string treeProperty)
        {
            var properties = treeProperty.Split('.');

            foreach (var property in properties.Take(properties.Length - 1))
            {
                value = value.GetType().GetProperty(property, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)?.GetValue(value);

                if (value == null)
                {
                    return null;
                }
            }

            return value.GetType().GetProperty(properties[^1], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        }

        public static object GetSubPropertyValue(object givenValue, string treeProperty)
        {
            var properties = treeProperty.Split('.');
            return properties.Aggregate(givenValue, (current, property) => current.GetType().GetProperty(property, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)?.GetValue(current));
        }

        public static object GetPropertyValue(object src, string propName)
        {
            if (src == null) throw new ArgumentException("Value cannot be null.", "src");
            if (propName == null) throw new ArgumentException("Value cannot be null.", "propName");

            if (propName.Contains("."))
            {
                var temp = propName.Split(new[] { '.' }, 2);
                return GetPropertyValue(GetPropertyValue(src, temp[0]), temp[1]);
            }
            else
            {
                var prop = src.GetType().GetProperty(propName);
                return prop != null ? prop.GetValue(src, null) : null;
            }
        }

        private static Person MergeHighlightedItems(Person per, Result result)
        {
            per.GetType()
                .GetProperties()
                .Join(
                    result.Highlight.Keys,
                    source => source.Name.ToLower(),
                    highlightedItem => highlightedItem.ToLower(),
                    (s, h) => new { Source = s, HighlitedItem = h })
                .ToList().ForEach(a => a.Source.SetValue(per, result.Highlight[a.HighlitedItem][0], null));

            return per;
        }
    }
}
