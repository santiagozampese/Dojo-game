namespace dojo.API
{
    public static class ClassRegistry
    {   
        private static readonly List<ClassReference> _classes = new();
        public static IReadOnlyList<ClassReference> classes => _classes.AsReadOnly();
        public static void AddClass(ClassReference reference)
        {
            _classes.Add(reference);
        }
    }
}