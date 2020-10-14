using Galaxy.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace TAS.Commands
{
    public class ListSaveStates : ICommand
    {
        public override string Name => "ls";

        public override void Run(string[] tokens)
        {
            HashSet<string> files;
            if (tokens.Length == 0)
                files = new HashSet<string>(Directory.GetFiles(Constants.SaveStatePath, "*.json"));
            else if (tokens.Length == 1)
            {
                files = new HashSet<string>(ParseToken(tokens[0]));
            }
            else
            {
                files = new HashSet<string>();
                foreach (var token in tokens)
                {
                    files.UnionWith(ParseToken(token));
                }
            }

            List<string> results = new List<string>(files);
            results.Sort();
            foreach(string s in results)
            {
                Write("\t{0}", s.Replace(Constants.SaveStatePath + "\\", ""));
            }
        }
        
        public override string[] ParseToken(string token)
        {
            string[] results;
            if (!token.EndsWith(".json", StringComparison.CurrentCulture))
                token += "*.json";
            try
            {
                results = new List<string>(Directory.GetFiles(Constants.SaveStatePath, token)).ToArray();
            } catch(DirectoryNotFoundException)
            {
                return new string[] { string.Format("\tls{0}: no such file or directory", token) };
            }
            return results;
        }

        public override string[] HelpText()
        {
            return new string[] {
                string.Format("usage: \"{0} [<file>|...]\" to list save state files", Name),
                "\tMultiple file names can be searched, as well as wildcards * and ?",
                "\tAn automatic * wildcard is appended unless you explicitly use the .json extension"
            };
        }
    }


    public class CopySaveStates : ICommand
    {
        public override string Name => "cp";

        public override void Run(string[] tokens)
        {
            if (tokens.Length != 2)
            {
                Write("\tcp requires 2 inputs: cp <src> <dst>");
                return;
            }

            string src = Path.Combine(Constants.SaveStatePath, tokens[0]);
            if (!src.EndsWith(".json", StringComparison.CurrentCulture))
                src += ".json";
            string dst = Path.Combine(Constants.SaveStatePath, tokens[1]);
            if (!dst.EndsWith(".json", StringComparison.CurrentCulture))
                dst += ".json";
            if (!File.Exists(src))
            {
                Write("\tsrc \"{0}\" not found", tokens[0]);
                return;
            }
            File.Copy(src, dst);
            SaveState.ChangeSaveStatePrefix(dst, tokens[1]);
        }
        public override string[] HelpText()
        {
            return new string[] { 
                string.Format("usage: \"{0} <src> <dst>\" to copy src file to dst", Name),
                "\t.json extension is automatically appended if not provided"
            };
        }
    }

    public class RemoveSaveStates : ICommand
    {
        public override string Name => "rm";

        public override void Run(string[] tokens)
        {
            if (tokens.Length == 0)
            {
                Write("\trm requires at least 1 input");
                return;
            }
            string[] files;
            if (tokens.Length == 1)
            {
                files = ParseToken(tokens[0]);
            }
            else
            {
                List<string> allFiles = new List<string>();
                foreach (var token in tokens)
                {
                    allFiles.AddRange(ParseToken(token));
                }
                files = allFiles.ToArray();
            }

            foreach(string file in files)
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
        }

        public override string[] ParseToken(string token)
        {
            if (token.Contains("*") || token.Contains("?"))
            {
                return new ListSaveStates().ParseToken(token);
            }
            string path = Path.Combine(Constants.SaveStatePath, token.Trim());
            if (!path.EndsWith(".json", StringComparison.CurrentCulture))
                path += ".json";
            return new string[] { path };
        }
        public override string[] HelpText()
        {
            return new string[] { 
                string.Format("usage: \"{0} <file> [...]\" to remove 1 or more files", Name),
                "\tMultiple file names can be passed, as well as wildcards * and ?",
                "\t.json extension is automatically appended if not provided"
            };
        }
    }

    public class MoveSaveStates : ICommand
    {
        public override string Name => "mv";

        public override void Run(string[] tokens)
        {
            if (tokens.Length != 2)
            {
                Write("\tmv requires 2 inputs: mv <src> <dst>");
                return;
            }

            string src = Path.Combine(Constants.SaveStatePath, tokens[0]);
            if (!src.EndsWith(".json", StringComparison.CurrentCulture))
                src += ".json";
            string dst = Path.Combine(Constants.SaveStatePath, tokens[1]);
            if (!dst.EndsWith(".json", StringComparison.CurrentCulture))
                dst += ".json";

            if (!File.Exists(src))
            {
                Write("\tsrc \"{0}\" not found", tokens[0]);
                return;
            }
            if (dst.Equals(src))
                return;
            if (File.Exists(dst))
                File.Delete(dst);
            File.Move(src, dst);
            SaveState.ChangeSaveStatePrefix(dst, tokens[1]);
        }

        public override string[] HelpText()
        {
            return new string[] { 
                string.Format("usage: \"{0} <src> <dst>\" to move src file to dst", Name),
                "\t.json extension is automatically appended if not provided"
            };
        }
    }
}
