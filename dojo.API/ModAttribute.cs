namespace dojo.API
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ModAttribute : Attribute
    {
        public string Name {get;}
        public string Description {get;}
        public ModAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}