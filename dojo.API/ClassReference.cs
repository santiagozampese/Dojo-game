namespace dojo.API
{
    public class ClassReference : Attribute
    {
        public string Name = "";
        public string? Description = "";
        private readonly Type? _playerType;
        public ClassReference(string name, Type type)
        {
            if (!typeof(Player).IsAssignableFrom(type) || type.IsAbstract)
            {
                throw new ArgumentException($"The type {type.Name} must inherit of Player and should not be abstract");
            }

            Name = name;
            _playerType = type;
        }
        public Player Instantiate(int playerNumber)
        {
            Player player = (Player)Activator.CreateInstance(_playerType!)!;
            player.playerNumber = playerNumber;
            player.Name = Name;
            return player;
        }
    }
}