using dojo.API;
using dojo.BaseContent;
using modNinja;

namespace dojo.Core
{
    public static class ClassesManager
    {
        public static void Init()
        {
            ClassRegistry.AddClass(new ClassReference("Knight", typeof(Knight)) {Description = "Base life & damage. Hability deals double damage."});
            ClassRegistry.AddClass(new ClassReference("Mage", typeof(Mage)) {Description = "80 health & Attack 8. Habilty attack at 12 blocks."});
            ClassRegistry.AddClass(new ClassReference("Shadow Ninja", typeof(Ninja)) {Description = "Base life & heath. Hability teleport to random position."});
        }
    }

}