using Godot;
using ZombieHoardGame.PlayerCharacter;

namespace Perks
{
    public class Perks : Resource
    {
        [Export]
        public int BuyCost;

        [Export]
        public string Description;

        [Export]
        public string Name;

        /// <summary>
        /// Applies the perk effect to the player.
        /// </summary>
        public virtual void ApplyEffect(Player player) { }
    }
}