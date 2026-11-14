using System.Reflection;
using dojo.API;

namespace dojo.Core
{
    public static class DllLoader
    {    
        public static List<ClassReference> LoadMods(string dir)
        {
            var findedClasses = new List<ClassReference>();
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string[] filesDll = Directory.GetFiles(dir, "*.dll");

            foreach (string pathDll in filesDll)
            {
                try
                {
                    Assembly assemblyMod = Assembly.LoadFrom(pathDll);

                    foreach (Type type in assemblyMod.GetExportedTypes())
                    {
                        if (typeof(Player).IsAssignableFrom(type) && !type.IsAbstract)
                        {
                            var attribute = type.GetCustomAttribute<ModAttribute>();
                            string name = attribute!.Name ?? type.Name;
                            string description = attribute!.Description ?? "Class added by mod";

                            findedClasses.Add(new ClassReference(name, type) { Description = description });
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error to load mod {Path.GetFileName(pathDll)}: {e.Message}");
                }
            }
            
            return findedClasses;
        }
    }
}