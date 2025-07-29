using Godot;
using System;
using System.Collections.Generic;
using Weapons;
using Perks;
using ZombieHoardGame.PlayerCharacter;

namespace ZombieHoardGame
{
	public class PerksBuy : Area, IInteractable
	{

		/// Properties - public, protected, private ///
		[Export]
		public Perks.Perks Perk { 
			get{ return _perk; }
			private set { _perk = value; }
		}

		// /// Fields - protected or private ///
		// private Sprite3D _iconSprite;
		// private AnimationPlayer _animPlayer;
		// private AudioStreamPlayer3D _audioUsed;
		private int _buyPerkCost;
		private Perks.Perks _perk;

		public override void _Ready()
		{
			// _iconSprite = GetNode<Sprite3D>("Sprite3D");
			// _animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
			// _audioUsed = GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D");
			
			UpdatePerkInfo();
		}

		public void Use(int playerPoints, Player player, out int useCost)
		{
			useCost = -1;

			if (playerPoints >= _buyPerkCost)
			{
				useCost = _buyPerkCost;
				Flash(true);
				_perk.ApplyEffect(player);
			}
			else
			{
				Flash(false);
			}
		}


		public String GetInteractText()
		{
			return _perk != null ? $"Press E to buy perk {_perk.Name} for {_buyPerkCost}" : "Test Perk";
		}

		private void Flash(bool flashGood)
		{
			// if (flashGood)
			// {
			// 	_animPlayer.Play("flash_good");
			// 	_audioUsed.Play();
			// }
			// else
			// {
			// 	_animPlayer.Play("flash_bad");
			// }
		}

		private void UpdatePerkInfo()
		{
			_buyPerkCost = _perk.BuyCost;
		}
	}
}

