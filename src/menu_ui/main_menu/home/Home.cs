using Godot;
using System;


namespace UI.MainMenu
{
	public class Home : MultiPage.Page
	{
		[Signal]
		public delegate void PlayLevelRequested();

		private Button _btnPlay;

		public override void _Ready()
		{
			_btnPlay = GetNode<Button>("Buttons/BtnPlay");
		}

		private void OnBtnPlayPressed()
		{
			if (IsActive)
			{
				EmitSignal(nameof(ChangePageRequested), "LevelSelect");
			}
		}

		private void OnBtnExitPressed()
		{
			if(IsActive)
			{
				GetTree().Quit();
			}
		}

		private void OnBtnSettingsPressed()
		{
			if(IsActive)
			{
				EmitSignal(nameof(ChangePageRequested), "SettingsPage");
			}
		}

		private void OnBtnControlsPressed()
		{
			if(IsActive)
			{
				EmitSignal(nameof(ChangePageRequested), "ControlsPage");
			}
		}

		private void OnBtnCreditsPressed()
		{
			if(IsActive)
			{
				EmitSignal(nameof(ChangePageRequested), "CreditsPage");
			}
		}
	}
}

