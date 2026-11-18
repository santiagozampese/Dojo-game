using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Loader;
using dojo.API;

namespace dojo.Core
{
    public static class DllLoader
    {    
        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "DLLs mods are extern and are not trimmed")]
        [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "mod types are preserved in extern DLL")]
        public static List<ClassReference> LoadMods(string dir, bool loadModPacks = true)
        {
            var findedClasses = new List<ClassReference>();
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (loadModPacks)
            {
                foreach (var d in Directory.GetDirectories(dir))
                {
                    string[] modpackDlls = Directory.GetFiles(d, "*.dll");
                    if (modpackDlls.Length > 0)
                    {   
                        var modPack = new Modpack();  
                        modPack.Name = Path.GetFileNameWithoutExtension(d);              
                        modPack.Mods = LoadMods(d, false);
                        
                        Program.modpacks.Add(modPack);
                    }
                }
            }

            string[] filesDll = Directory.GetFiles(dir, "*.dll");
            foreach (string pathDll in filesDll)
            {
                try
                {
                    var context = new AssemblyLoadContext(Path.GetFileNameWithoutExtension(pathDll));
                    Assembly assemblyMod = context.LoadFromAssemblyPath(Path.GetFullPath(pathDll));

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
                    Environment.Exit(1);
                }
            }
            
            return findedClasses;
        }
    }
}