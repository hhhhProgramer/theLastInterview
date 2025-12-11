using System;
using System.Collections.Generic;
using Godot;

namespace Package.Audio
{
	/// <summary>
	/// Clase que maneja la configuración del audio del juego
	/// Almacena y gestiona los ajustes de volumen y silenciamiento
	/// Incluye sistema de volúmenes individuales por archivo de audio
	/// </summary>
	public class AudioConfig
	{
		private const string MUSIC_VOLUME_KEY = "audio_music_volume";
		private const string SOUND_EFFECTS_VOLUME_KEY = "audio_sound_effects_volume";
		private const string MUSIC_MUTED_KEY = "audio_music_muted";
		private const string SOUND_EFFECTS_MUTED_KEY = "audio_sound_effects_muted";
		private const string INDIVIDUAL_VOLUMES_KEY = "audio_individual_volumes";
		
		/// <summary>
		/// Evento que se dispara cuando cambia el volumen de la música
		/// </summary>
		public event Action<float> MusicVolumeChanged;
		
		/// <summary>
		/// Evento que se dispara cuando cambia el volumen de los efectos de sonido
		/// </summary>
		public event Action<float> SoundEffectsVolumeChanged;
		
		/// <summary>
		/// Evento que se dispara cuando cambia el estado de silenciamiento de la música
		/// </summary>
		public event Action<bool> MusicMutedChanged;
		
		/// <summary>
		/// Evento que se dispara cuando cambia el estado de silenciamiento de los efectos
		/// </summary>
		public event Action<bool> SoundEffectsMutedChanged;
		
		/// <summary>
		/// Evento que se dispara cuando cambia el volumen individual de un archivo
		/// </summary>
		public event Action<string, float> IndividualVolumeChanged;
		
		/// <summary>
		/// Volumen de la música (0.0 a 1.0)
		/// </summary>
		public float MusicVolume 
		{ 
			get => _musicVolume;
			set
			{
				_musicVolume = Mathf.Clamp(value, 0.0f, 1.0f);
				SaveMusicVolume();
				MusicVolumeChanged?.Invoke(_musicVolume);
			}
		}
		
		/// <summary>
		/// Volumen de los efectos de sonido (0.0 a 1.0)
		/// </summary>
		public float SoundEffectsVolume 
		{ 
			get => _soundEffectsVolume;
			set
			{
				_soundEffectsVolume = Mathf.Clamp(value, 0.0f, 1.0f);
				SaveSoundEffectsVolume();
				SoundEffectsVolumeChanged?.Invoke(_soundEffectsVolume);
			}
		}
		
		/// <summary>
		/// Indica si la música está silenciada
		/// </summary>
		public bool IsMusicMuted 
		{ 
			get => _isMusicMuted;
			set
			{
				_isMusicMuted = value;
				SaveMusicMuted();
				MusicMutedChanged?.Invoke(_isMusicMuted);
			}
		}
		
		/// <summary>
		/// Indica si los efectos de sonido están silenciados
		/// </summary>
		public bool IsSoundEffectsMuted 
		{ 
			get => _isSoundEffectsMuted;
			set
			{
				_isSoundEffectsMuted = value;
				SaveSoundEffectsMuted();
				SoundEffectsMutedChanged?.Invoke(_isSoundEffectsMuted);
			}
		}
		
		private float _musicVolume = 0.4f;
		private float _soundEffectsVolume = 0.8f;
		private bool _isMusicMuted = false;
		private bool _isSoundEffectsMuted = false;
		
		/// <summary>
		/// Diccionario que almacena volúmenes individuales por nombre de archivo
		/// El valor representa el volumen máximo del archivo (0.0 a 1.0)
		/// </summary>
		private Dictionary<string, float> _individualVolumes;
		
		/// <summary>
		/// Constructor que carga la configuración guardada
		/// </summary>
		public AudioConfig()
		{
			_individualVolumes = new Dictionary<string, float>();
			LoadConfiguration();
		}
		
		/// <summary>
		/// Carga la configuración de audio desde el almacenamiento local
		/// </summary>
		private void LoadConfiguration()
		{
			try
			{
				// Cargar volúmenes
				if (ProjectSettings.HasSetting(MUSIC_VOLUME_KEY))
				{
					_musicVolume = (float)ProjectSettings.GetSetting(MUSIC_VOLUME_KEY);
				}
				
				if (ProjectSettings.HasSetting(SOUND_EFFECTS_VOLUME_KEY))
				{
					_soundEffectsVolume = (float)ProjectSettings.GetSetting(SOUND_EFFECTS_VOLUME_KEY);
				}
				
				// Cargar estados de silenciamiento
				if (ProjectSettings.HasSetting(MUSIC_MUTED_KEY))
				{
					_isMusicMuted = (bool)ProjectSettings.GetSetting(MUSIC_MUTED_KEY);
				}
				
				if (ProjectSettings.HasSetting(SOUND_EFFECTS_MUTED_KEY))
				{
					_isSoundEffectsMuted = (bool)ProjectSettings.GetSetting(SOUND_EFFECTS_MUTED_KEY);
				}
				
				// Cargar volúmenes individuales
				LoadIndividualVolumes();
				
 
 
 
 
 
 
			}
			catch (Exception)
			{
				 
			}
		}
		
		/// <summary>
		/// Carga los volúmenes individuales desde el almacenamiento local
		/// </summary>
		private void LoadIndividualVolumes()
		{
			try
			{
				if (ProjectSettings.HasSetting(INDIVIDUAL_VOLUMES_KEY))
				{
					var volumesData = ProjectSettings.GetSetting(INDIVIDUAL_VOLUMES_KEY);
					// Por ahora, cargamos volúmenes individuales vacíos
					// TODO: Implementar carga desde ProjectSettings cuando se resuelva el problema de tipos
 
				}
				
 
			}
			catch (Exception)
			{
				 
			}
		}
		
		/// <summary>
		/// Guarda los volúmenes individuales en el almacenamiento local
		/// </summary>
		private void SaveIndividualVolumes()
		{
			try
			{
				var volumesData = new Godot.Collections.Dictionary();
				foreach (var kvp in _individualVolumes)
				{
					volumesData[kvp.Key] = kvp.Value;
				}
				
				ProjectSettings.SetSetting(INDIVIDUAL_VOLUMES_KEY, volumesData);
				ProjectSettings.Save();
 
			}
			catch (Exception)
			{
				 
			}
		}
		
		/// <summary>
		/// Guarda el volumen de la música en el almacenamiento local
		/// </summary>
		private void SaveMusicVolume()
		{
			try
			{
				ProjectSettings.SetSetting(MUSIC_VOLUME_KEY, _musicVolume);
				ProjectSettings.Save();
 
			}
			catch (Exception)
			{
				 
			}
		}
		
		/// <summary>
		/// Guarda el volumen de los efectos de sonido en el almacenamiento local
		/// </summary>
		private void SaveSoundEffectsVolume()
		{
			try
			{
				ProjectSettings.SetSetting(SOUND_EFFECTS_VOLUME_KEY, _soundEffectsVolume);
				ProjectSettings.Save();
 
			}
			catch (Exception)
			{
				 
			}
		}
		
		/// <summary>
		/// Guarda el estado de silenciamiento de la música
		/// </summary>
		private void SaveMusicMuted()
		{
			try
			{
				ProjectSettings.SetSetting(MUSIC_MUTED_KEY, _isMusicMuted);
				ProjectSettings.Save();
 
			}
			catch (Exception)
			{
				 
			}
		}
		
		/// <summary>
		/// Guarda el estado de silenciamiento de los efectos de sonido
		/// </summary>
		private void SaveSoundEffectsMuted()
		{
			try
			{
				ProjectSettings.SetSetting(SOUND_EFFECTS_MUTED_KEY, _isSoundEffectsMuted);
				ProjectSettings.Save();
 
			}
			catch (Exception)
			{
				 
			}
		}
		
		/// <summary>
		/// Establece el volumen individual para un archivo de audio específico
		/// </summary>
		/// <param name="fileName">Nombre del archivo de audio (ej: "tir.ogg")</param>
		/// <param name="maxVolume">Volumen máximo del archivo (0.0 a 1.0)</param>
		public void SetIndividualVolume(string fileName, float maxVolume)
		{
			try
			{
				if (string.IsNullOrEmpty(fileName))
				{
					 
					return;
				}
				
				var clampedVolume = Mathf.Clamp(maxVolume, 0.0f, 1.0f);
				_individualVolumes[fileName] = clampedVolume;
				
				SaveIndividualVolumes();
				IndividualVolumeChanged?.Invoke(fileName, clampedVolume);
				
 
			}
			catch (Exception)
			{
				 
			}
		}
		
		/// <summary>
		/// Obtiene el volumen individual para un archivo de audio específico
		/// </summary>
		/// <param name="fileName">Nombre del archivo de audio</param>
		/// <returns>Volumen máximo del archivo (1.0 si no está configurado)</returns>
		public float GetIndividualVolume(string fileName)
		{
			if (string.IsNullOrEmpty(fileName))
				return 1.0f;
			
			return _individualVolumes.TryGetValue(fileName, out float volume) ? volume : 1.0f;
		}
		
		/// <summary>
		/// Elimina la configuración de volumen individual para un archivo
		/// </summary>
		/// <param name="fileName">Nombre del archivo de audio</param>
		public void RemoveIndividualVolume(string fileName)
		{
			try
			{
				if (string.IsNullOrEmpty(fileName))
					return;
				
				if (_individualVolumes.Remove(fileName))
				{
					SaveIndividualVolumes();
					IndividualVolumeChanged?.Invoke(fileName, 1.0f);
 
				}
			}
			catch (Exception)
			{
				 
			}
		}
		
		/// <summary>
		/// Obtiene todos los volúmenes individuales configurados
		/// </summary>
		/// <returns>Diccionario con los volúmenes individuales</returns>
		public Dictionary<string, float> GetAllIndividualVolumes()
		{
			return new Dictionary<string, float>(_individualVolumes);
		}
		
		/// <summary>
		/// Obtiene el volumen efectivo para un tipo de audio específico
		/// Considera tanto el volumen como el estado de silenciamiento
		/// </summary>
		/// <param name="audioType">Tipo de audio para el cual obtener el volumen</param>
		/// <returns>Volumen efectivo (0.0 si está silenciado, volumen configurado en caso contrario)</returns>
		public float GetEffectiveVolume(AudioType audioType)
		{
			return audioType switch
			{
				AudioType.Music => _isMusicMuted ? 0.0f : _musicVolume,
				AudioType.SoundEffects => _isSoundEffectsMuted ? 0.0f : _soundEffectsVolume,
				_ => 0.0f
			};
		}
		
		/// <summary>
		/// Obtiene el volumen efectivo para un archivo de audio específico
		/// Aplica la regla de tres: volumenGeneral * volumenIndividual
		/// </summary>
		/// <param name="audioType">Tipo de audio (música o efectos)</param>
		/// <param name="fileName">Nombre del archivo de audio</param>
		/// <returns>Volumen efectivo calculado</returns>
		public float GetEffectiveVolumeForFile(AudioType audioType, string fileName)
		{
			try
			{
				// Obtener volumen base del tipo de audio
				float baseVolume = GetEffectiveVolume(audioType);
				
				// Si está silenciado, retornar 0
				if (baseVolume <= 0.0f)
					return 0.0f;
				
				// Obtener volumen individual del archivo (1.0 si no está configurado)
				float individualVolume = GetIndividualVolume(fileName);
				
				// Aplicar regla de tres: volumenGeneral * volumenIndividual
				float effectiveVolume = baseVolume * individualVolume;
				
				// Asegurar que no exceda el volumen individual
				effectiveVolume = Mathf.Min(effectiveVolume, individualVolume);
				
				return effectiveVolume;
			}
			catch (Exception)
			{
				 
				return GetEffectiveVolume(audioType); // Fallback al volumen base
			}
		}
		
		/// <summary>
		/// Restablece la configuración de audio a los valores por defecto
		/// </summary>
		public void ResetToDefaults()
		{
			try
			{
				_musicVolume = 0.7f;
				_soundEffectsVolume = 0.8f;
				_isMusicMuted = false;
				_isSoundEffectsMuted = false;
				
				// Limpiar volúmenes individuales
				_individualVolumes.Clear();
				
				SaveMusicVolume();
				SaveSoundEffectsVolume();
				SaveMusicMuted();
				SaveSoundEffectsMuted();
				SaveIndividualVolumes();
				
 
			}
			catch (Exception)
			{
				 
			}
		}
	}
}
