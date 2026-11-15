using dojo.API;
using dojo.BaseContent;
using modNinja;

namespace dojo.Core
{
    public static class ClassesManager
    {
        public static void Init()
        {
            ClassRegistry.AddClass(new ClassReference("Knight", typeof(Knight)) {Description = "Hability deals double damage."});
            ClassRegistry.AddClass(new ClassReference("Mage", typeof(Mage)) {Description = "Habilty attack at 12 blocks."});
            ClassRegistry.AddClass(new ClassReference("Shadow Ninja", typeof(Ninja)) {Description = "Hability teleport to random position."});
        }
    }

}