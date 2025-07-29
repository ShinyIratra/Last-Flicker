using Godot;
using ZombieHoardGame.PlayerCharacter;

namespace Perks
{
	[Tool]
	public class Juggernaug : Perks
	{
		public Juggernaug()
		{
			Name = "Juggernaug";
			Description = "Augmente la vie maximale de 50 points.";
			BuyCost = 1000;
		}

		public override void ApplyEffect(Player player)
		{
			HealthSystem.Health playerHealth = player.GetHealth();
			playerHealth.PointsMax += 50;
			playerHealth.RestoreToMax();
		}
	}
}
