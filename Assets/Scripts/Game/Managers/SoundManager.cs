using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
	public enum SFXType {
		None,
		Pop,
		Select,
		Deselect,
		Grow,
		NewCard,
		TreeAwake
	}

	public static SoundManager Instance { get; private set; }

	[SerializeField] AudioSource m_soundPrefab;
	
	[SerializeField] List<AudioClip> m_pop;
	[SerializeField] AudioClip m_select;
	[SerializeField] AudioClip m_deselect;
	[SerializeField] AudioClip m_newCard;
	[SerializeField] AudioClip m_treeAwake;
	
	[SerializeField] List<AudioClip> m_grow;
	
	
	[SerializeField, Header("Volume tuning")]
	public List<VolumeOverride> volumeOverrides;
	// {
	// 	{SFXType.ShootFireball, 0.20f},
	// 	{SFXType.ExplodeFireball, 0.10f},
	// 	{SFXType.ShootPistol, 0.1f},
	// 	{SFXType.ShootPistol2, 0.1f}
	// }
	
	[Serializable]
	public class VolumeOverride
	{
		public SFXType sfxType;
		public float volumeMul;
	}
	
	private void Awake ()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	public void PlaySound ( SFXType sfxType)
	{
		AudioClip targetClip = null;

		switch (sfxType)
		{
			case SFXType.None:
				break;
			case SFXType.Pop:
				targetClip = m_pop.Random();
				break;
			case SFXType.Select:
				targetClip = m_select;
				break;
			case SFXType.Deselect:
				targetClip = m_deselect;
				break;
			case SFXType.Grow:
				targetClip = m_grow.Random();
				break;
			case SFXType.NewCard:
				targetClip = m_newCard;
				break;
			case SFXType.TreeAwake:
				targetClip = m_treeAwake;
				break;
			default:
				break;
		}
		
		if(!targetClip)
			return;

		float volume = 1;
		var volumeOverride = volumeOverrides.FirstOrDefault(r => r.sfxType == sfxType);
		if (volumeOverride != null)
			volume = volumeOverride.volumeMul;
		
		AudioSource newAudioSource = Instantiate(m_soundPrefab);
		newAudioSource.clip = targetClip;
		newAudioSource.pitch = Random.Range(0.9f, 1.1f);
		newAudioSource.volume = newAudioSource.volume * volume;
		newAudioSource.Play();
		
		Destroy(newAudioSource.gameObject, newAudioSource.clip.length + 0.1f);
	}
}
