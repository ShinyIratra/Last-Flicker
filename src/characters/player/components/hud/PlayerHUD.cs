using Godot;
using System;


namespace ZombieHoardGame.PlayerCharacter
{
	public class PlayerHUD : Control
	{
		private Label _labelGunAmmo;
		private Label _labelPoints;
		private Label _labelHealth;
		private ProgressBar _progressBarReload;
		private Label _labelRoundNumber;
		private Label _labelInteractiveText;
		private ColorRect _crosshair;
		private ShaderMaterial _crosshairShader;
		private TextureRect _healthEffect;
		private HealthSystem.Health _trackedHealth;
		private AudioStreamPlayer _hurtAudio;


		public override void _Ready()
		{
			CacheNodeReferences();
			_crosshairShader = (ShaderMaterial)_crosshair.Material;
			_crosshairShader.SetShaderParam("edgeLength", GetViewportRect().Size.y);
			_progressBarReload.Hide();
		}

		public void SetHealthComponent(HealthSystem.Health playerHealth)
		{
			_trackedHealth = playerHealth;
			playerHealth.Connect(nameof(HealthSystem.Health.PointsChanged), this, nameof(OnHealthValueChanged));
			UpdateHealthValue((int)playerHealth.Points); // Met à jour au démarrage
		}

		public void UpdateAmmoLabel(int loaded, int remaining)
		{
			_labelGunAmmo.Text = $"{loaded} / {remaining}";
		}

		public void ShowReloadBar(float time)
		{
			SceneTreeTween tween = GetTree().CreateTween();
			tween.TweenCallback(_progressBarReload, "show");
			tween.TweenProperty(_progressBarReload, "value", 100f, time).From(0f);
			tween.TweenCallback(_progressBarReload, "hide");
			tween.Play();
		}

		public void UpdatePointsValue(int value)
		{
			_labelPoints.Text = value.ToString();
		}

		public void UpdateHealthValue(int value)
		{
			_labelHealth.Text = value.ToString();
		}

		public void UpdateRoundNumber(int number)
		{
			float zoomTime = 0.2f;
			float fadeTime = 0.6f;
			float totalTime = zoomTime + fadeTime;

			_labelRoundNumber.Text = number.ToString();

			SceneTreeTween tween = GetTree().CreateTween();

			tween.TweenProperty(_labelRoundNumber, "rect_scale", new Vector2(1, 1.3f), zoomTime).From(new Vector2(1, 1));
			tween.TweenProperty(_labelRoundNumber, "rect_scale", new Vector2(1, 1), fadeTime);

			tween.Parallel().TweenProperty(_labelRoundNumber, "modulate", new Color(0.7f, 0.7f, 0.7f), totalTime).From(Colors.White);

			tween.Parallel().TweenProperty(_labelRoundNumber, "rect_position", _labelRoundNumber.RectPosition + new Vector2(0, -5), 0.1f).From(_labelRoundNumber.RectPosition);
			tween.TweenProperty(_labelRoundNumber, "rect_position", _labelRoundNumber.RectPosition, 0.1f);

			tween.Play();
		}

		public void UpdateInteractiveText(String text)
		{
			if (text == null)
			{
				_labelInteractiveText.Text = "";
				_labelInteractiveText.Hide();
			}
			else
			{
				_labelInteractiveText.Text = text;
				_labelInteractiveText.Show();
			}
		}

		public void UpdateCrosshair(float spread, float camFOV)
		{
			float viewportSizeY = GetViewportRect().Size.y;
			float pixlesPerDegree = viewportSizeY / camFOV;
			int crosshairDiamiter = (int)Mathf.Floor(pixlesPerDegree * spread); // in pixles

			_crosshairShader.SetShaderParam("innerDiamiter", crosshairDiamiter);
		}


		private void OnHealthValueChanged()
		{
			float effectStrength = 1 - _trackedHealth.PointsPercentage;
			_healthEffect.Modulate = new Color(1, 1, 1, effectStrength);
			if (effectStrength > 0)
			{
				_hurtAudio.Play();
				_hurtAudio.VolumeDb = GD.Linear2Db(effectStrength);
			}
			else
			{
				_hurtAudio.Stop();
			}

			UpdateHealthValue((int)_trackedHealth.Points); // Ajoute cette ligne
		}

		private void CacheNodeReferences()
		{
			_labelGunAmmo = GetNode<Label>("GunAmmo");
			_labelPoints = GetNode<Label>("Points");
			_labelHealth = GetNode<Label>("Health");
			_progressBarReload = GetNode<ProgressBar>("ReloadProgress");
			_labelRoundNumber = GetNode<Label>("RoundNumber");
			_labelInteractiveText = GetNode<Label>("InteractiveText");
			_crosshair = GetNode<ColorRect>("Crosshair");
			_healthEffect = GetNode<TextureRect>("HealthEffect");
			_hurtAudio = GetNode<AudioStreamPlayer>("HurtHeartBeat");
		}
	}
}

