using Godot;
using System;
using System.Collections.Generic;
using GameGeneral;

namespace UI.MainMenu
{
	public class LevelSelect : MultiPage.Page
	{
		private const string LevelDirectoryPath = "res://src/levels/";
		private const string PlaceholderImagePath = "res://src/levels/placeholder.png";

		private List<string> _levelPaths = new List<string>();
		private int _currentLevelIndex = 0;

		private Button _btnBack;
		private Button _btnNext;
		private Button _btnLaunch;
		private Button _btnBackHome; // Ajouté
		private TextureRect _textureRect;
		private Label _levelName;

		public override void _Ready()
		{
			CacheNodeReferences();
			LoadLevelPaths();
			UpdateLevelDisplay();
		}

		private void CacheNodeReferences()
		{
			_btnBack = GetNode<Button>("Container/Panel/BtnBack");
			_btnNext = GetNode<Button>("Container/Panel/BtnNext");
			_btnLaunch = GetNode<Button>("Container/Panel/BtnLaunch");
			_btnBackHome = GetNode<Button>("Container/HBoxContainer/BtnBackHome");
			_textureRect = GetNode<TextureRect>("Container/Panel/TextureRect");
			_levelName = GetNode<Label>("Container/Panel/LevelName");

			_btnBack.Connect("pressed", this, nameof(OnBtnBackPressed));
			_btnNext.Connect("pressed", this, nameof(OnBtnNextPressed));
			_btnLaunch.Connect("pressed", this, nameof(OnBtnLaunchPressed));
			_btnBackHome.Connect("pressed", this, nameof(OnBtnBackHomePressed));
		}

		private void LoadLevelPaths()
		{
			Godot.Directory dir = new Godot.Directory();
			if (dir.Open(LevelDirectoryPath) == Error.Ok)
			{
				dir.ListDirBegin();
				string fileName;
				while ((fileName = dir.GetNext()) != "")
				{
					if (fileName.EndsWith(".tscn"))
					{
						_levelPaths.Add(LevelDirectoryPath + fileName);
					}
				}
				dir.ListDirEnd();
			}

			if (_levelPaths.Count == 0)
			{
				GD.PrintErr("No levels found in directory: " + LevelDirectoryPath);
			}
		}

		private void UpdateLevelDisplay()
		{
			if (_levelPaths.Count == 0)
			{
				_textureRect.Texture = GD.Load<Texture>(PlaceholderImagePath);
				_levelName.Text = "No Levels Available";
				return;
			}

			string currentLevelPath = _levelPaths[_currentLevelIndex];
			string levelName = System.IO.Path.GetFileNameWithoutExtension(currentLevelPath);

			string levelDir = System.IO.Path.GetDirectoryName(currentLevelPath).Replace("\\", "/") + "/";
			string imagePath = levelDir + levelName + ".png";

			Texture levelTexture = null;
			if (GD.Load(imagePath) is Texture tex)
				levelTexture = tex;
			else
				levelTexture = GD.Load<Texture>(PlaceholderImagePath);

			_textureRect.Texture = levelTexture;
			_levelName.Text = levelName;
		}

		private void OnBtnBackPressed()
		{
			if (_levelPaths.Count == 0) return;

			_currentLevelIndex = (_currentLevelIndex - 1 + _levelPaths.Count) % _levelPaths.Count;
			UpdateLevelDisplay();
		}

		private void OnBtnNextPressed()
		{
			if (_levelPaths.Count == 0) return;

			_currentLevelIndex = (_currentLevelIndex + 1) % _levelPaths.Count;
			UpdateLevelDisplay();
		}

		private void OnBtnLaunchPressed()
		{
			if (_levelPaths.Count == 0) return;

			string selectedLevelPath = _levelPaths[_currentLevelIndex];
			Main.Instance.TransitionToLevel(selectedLevelPath);
		}

		private void OnBtnBackHomePressed()
		{
			if (IsActive)
			{
				EmitSignal(nameof(ChangePageRequested), "Home");
			}
		}
	}
}

