using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ConsoleApp1
{
    public class Person
    {
        [JsonPropertyName("requestingParty")]
        public string FirstName { get; set; }

        [JsonPropertyName("requestingParty2")]
        public string LastName { get; set; }

        public Experience Experience { get; set; }
    }

    public class Experience
    {
        public string ExperienceType { get; set; }

        public Expedience Expedience { get; set; }
    }

    public class Expedience
    {
        public string Sophistry { get; set; }

        public Exuberance Exuberance { get; set; }
    }

    public class Exuberance
    {
        public string ExType { get; set; }
    }

    public class Result
    {
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("highlight")]
        public Dictionary<string, List<string>> Highlight { get; set; }
    }
}
