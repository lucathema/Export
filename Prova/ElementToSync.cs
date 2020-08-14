using System.Collections.Generic;

namespace Prova
{
    class ElementToSync
    {
        public string Name { get; private set; }
        public string Script { get; private set; }
        public bool CommandLineArguments { get; private set; }
        public string Source { get; private set; }
        public bool SearchRecursively { get; private set; }
        public List<string> EndsWith { get; private set; }
        public List<string> NotEndsWith { get; private set; }

        public ElementToSync(string name, string script, bool commandLineArguments, string source, bool searchRecursively, List<string> endsWith, List<string> notEndsWith)
        {
            Name = name;
            Script = script;
            CommandLineArguments = commandLineArguments;
            Source = source;
            SearchRecursively = searchRecursively;
            EndsWith = endsWith;
            NotEndsWith = notEndsWith;
        }
    }
}
