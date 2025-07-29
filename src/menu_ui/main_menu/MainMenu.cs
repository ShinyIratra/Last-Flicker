using Godot;
using System;
using GameGeneral;


namespace UI.MainMenu
{
	public class MainMenu : MultiPage.Manager
	{
		[Signal]
		public delegate void ChangePageRequested(string pageName);

		public override void _Ready()
		{
			base._Ready();

			Home homePage = (Home)_pages["Home"];
			homePage.Connect(nameof(Home.PlayLevelRequested), this, nameof(OnPlayLevelRequested));
		}

		private void OnPlayLevelRequested()
		{
			EmitSignal(nameof(ChangePageRequested), "LevelSelect");
		}
	}
}

