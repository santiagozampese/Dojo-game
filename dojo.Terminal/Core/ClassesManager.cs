using dojo.API;
using dojo.BaseContent;
using modNinja;

namespace dojo.Core
{
    public static class ClassesManager
    {
        public static void Init()
        {
            ClassRegistry.AddClass(new ClassReference("Knight", typeof(Knight)));
            ClassRegistry.AddClass(new ClassReference("Mage", typeof(Mage)));
            ClassRegistry.AddClass(new ClassReference("Shadow Ninja", typeof(Ninja)));
        }
    }

}