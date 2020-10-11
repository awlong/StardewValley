using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TAS.Wrappers
{
    public class ResetWrapper
    {
        // Build a list of all Static Constructors and static field initializers
        public static List<ConstructorInfo> StaticConstructors = null;
        public static Dictionary<Type, List<FieldInfo>> StaticFields = null;
        public static Assembly ExecutingAssembly;
        

        static ResetWrapper()
        {
            ExecutingAssembly = Assembly.GetExecutingAssembly();
            BuildStaticConstructors();
            BuildDefaultStaticInitializer();
        }

        public static void Reset()
        {
            Controller.Reset();

            // reinit every static var to its initial default state
            CallDefaultStaticInitializer();
            CallStaticConstructors();
        }

        public static void BuildStaticConstructors()
        {
            StaticConstructors = new List<ConstructorInfo>();
            var allTypes = ExecutingAssembly.GetTypes();
            foreach (var t in allTypes)
            {
                if (ValidType(t))
                {
                    ConstructorInfo ctor = t.GetConstructor(BindingFlags.Static | BindingFlags.NonPublic, null, new Type[0], null);
                    if (ctor != null)
                    {
                        StaticConstructors.Add(ctor);
                    }
                }
            }
        }

        public static void CallStaticConstructors()
        {
            foreach(ConstructorInfo ctor in StaticConstructors)
            {
                ctor.Invoke(null, null);
            }
        }

        public static void BuildDefaultStaticInitializer()
        {
            StaticFields = new Dictionary<Type, List<FieldInfo>>();

            var allTypes = ExecutingAssembly.GetTypes();
            foreach (var t in allTypes)
            {
                if (ValidType(t))
                {
                    foreach (FieldInfo f in t.GetFields())
                    {
                        if (f.IsStatic && !f.IsLiteral)
                        {
                            if (!StaticFields.ContainsKey(t))
                            {
                                StaticFields.Add(t, new List<FieldInfo>());
                            }
                            StaticFields[t].Add(f);
                        }
                    }
                }
            }
        }

        public static void CallDefaultStaticInitializer()
        {
            foreach(var typeFields in StaticFields)
            {
                foreach(var field in typeFields.Value)
                {
                    field.SetValue(null, null);
                }
            }
        }



        public static bool ValidType(Type t)
        {
            // Clear EVERYTHING in the StardewValley namespace
            string[] safeNamespaces = { "StardewValley" };
            string[] unsafeNamespaces = { "TAS" };
            // Program/DateTime retain info about the current state
            // <>c is due to templated class objects
            // SerializableDictionary`2 is due to <TKey,TValue> templating
            string[] unsafeTypes =
            {
                "Program", "DateTime", "<>c", "SerializableDictionary`2"
            };

            if (t.Namespace == null)
                return false;

            // ensure the type is in the right namespaces
            if (!TypeInNamespaces(t, safeNamespaces))
                return false;
            if (TypeInNamespaces(t, unsafeNamespaces))
                return false;

            // and doesn't match one of the types we want to retain
            foreach(var u in unsafeTypes)
            {
                if (t.Name == u)
                    return false;
            }
            return true;
        }

        public static bool TypeInNamespaces(Type t, string[] spaces)
        {
            bool valid = false;
            foreach(var space in spaces)
            {
                if (t.Namespace.Contains(space))
                    valid = true;
            }
            return valid;
        }
    }
}
