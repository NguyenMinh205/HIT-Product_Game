using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    public AudioSource MusicSource => musicSource;
    [SerializeField] private AudioSource soundSource;
    public AudioSource SoundSource => soundSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip musicStartGame;
    [SerializeField] private AudioClip musicSelectRoom;
    [SerializeField] private AudioClip musicInGame;
    [SerializeField] private AudioClip selectCharacter;
    [SerializeField] private AudioClip playerTurn;
    [SerializeField] private AudioClip enemyTurn;
    [SerializeField] private AudioClip coin;
    [SerializeField] private AudioClip clawDrop;
    [SerializeField] private AudioClip clawMagnetDrop;
    [SerializeField] private AudioClip moveInMap;
    [SerializeField] private AudioClip enterRoom;
    [SerializeField] private AudioClip defeat;
    [SerializeField] private AudioClip victory;
    [SerializeField] private AudioClip reward;

    [Header("Player Audio Clips")]
    [SerializeField] private AudioClip playerAttack;
    [SerializeField] private AudioClip playerHit;
    [SerializeField] private AudioClip playerBuff;
    [SerializeField] private AudioClip playerPick;
    [SerializeField] private AudioClip playerSelect;

    [Header("Enemy Audio Clips")]
    [SerializeField] private AudioClip enemyAttack;
    [SerializeField] private AudioClip enemyHit;
    [SerializeField] private AudioClip enemyBuff;

    protected override void Awake()
    {
        base.KeepActive(true);
        base.Awake();
    }

    void Start()
    {
        GameData.Instance.LoadStartGameData();
        SetMusicVolume(GameData.Instance.startData.musicVolume);
        SetSoundVolume(GameData.Instance.startData.soundVolume);
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;
            GameData.Instance.startData.musicVolume = volume;
            GameData.Instance.SaveStartGameData();
        }
    }

    public void SetSoundVolume(float volume)
    {
        if (soundSource != null)
        {
            soundSource.volume = volume;
            GameData.Instance.startData.soundVolume = volume;
            GameData.Instance.SaveStartGameData();
        }
    }

    public void ResetDefault()
    {
        if (musicSource != null)
            musicSource.volume = 0.5f;
        if (soundSource != null)
            soundSource.volume = 0.5f;
        GameData.Instance.startData.musicVolume = 0.5f;
        GameData.Instance.startData.soundVolume = 0.5f;
        GameData.Instance.SaveStartGameData();
    }

    public void PlayMusicInGame()
    {
        if (musicSource != null && !musicSource.isPlaying && musicInGame != null)
        {
            musicSource.clip = musicInGame;
            musicSource.volume = 0f;
            musicSource.Play();
            musicSource.DOFade(GameData.Instance.startData.musicVolume, 0.5f).SetUpdate(true);
        }
    }

    public void PlayMusicStartGame()
    {
        if (musicSource != null && !musicSource.isPlaying && musicStartGame != null)
        {
            musicSource.clip = musicStartGame;
            musicSource.volume = 0f;
            musicSource.Play();
            musicSource.DOFade(GameData.Instance.startData.musicVolume, 0.5f).SetUpdate(true);
        }
    }

    public void PlayMusicSelectRoom()
    {
        if (musicSource != null && !musicSource.isPlaying && musicSelectRoom != null)
        {
            musicSource.clip = musicSelectRoom;
            musicSource.volume = 0f;
            musicSource.Play();
            musicSource.DOFade(GameData.Instance.startData.musicVolume, 0.5f).SetUpdate(true);
        }
    }

    public void StopMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.DOFade(0f, 0.5f).OnComplete(() =>
            {
                musicSource.Stop();
                musicSource.clip = null;
            }).SetUpdate(true);
        }
    }

    public void PlaySFX(AudioClip sound, bool repeat = false)
    {
        if (sound != null && soundSource != null)
        {
            if (repeat)
            {
                soundSource.loop = true;
                soundSource.clip = sound;
                soundSource.Play();
            }
            else
            {
                soundSource.loop = false;
                soundSource.PlayOneShot(sound, GameData.Instance.startData.soundVolume);
            }
        }
    }

    public void StopSFX()
    {
        if (soundSource != null && soundSource.isPlaying)
        {
            soundSource.Stop();
            soundSource.loop = false;
            soundSource.clip = null;
        }
    }

    public void PlaySoundClickButton() { PlaySFX(buttonClick); }
    public void PlaySelectCharacter() { PlaySFX(selectCharacter); }
    public void PlayMoveSound() { PlaySFX(moveInMap); }
    public void PlayCoin() { PlaySFX(coin); }
    public void PlayRewardSound() { PlaySFX(reward); }
    public void PlayClawDropSound() { PlaySFX(clawDrop); }
    public void PlayClawMagnetDropSound() { PlaySFX(clawMagnetDrop); }
    public void PlayEnterRoomSound() { PlaySFX(enterRoom); }
    public void PlayVictorySound() { PlaySFX(victory); }
    public void PlayDefeatSound() { PlaySFX(defeat); }
    public void PlayPlayerTurn() { PlaySFX(playerTurn); }
    public void PlayEnemyTurn() { PlaySFX(enemyTurn); }
    public void PlayPlayerAttack() { PlaySFX(playerAttack); }
    public void PlayPlayerHit() { PlaySFX(playerHit); }
    public void PlayPlayerBuff() { PlaySFX(playerBuff); }
    public void PlayPlayerPick() { PlaySFX(playerPick); }
    public void PlayEnemyAttack() { PlaySFX(enemyAttack); }
    public void PlayEnemyHit() { PlaySFX(enemyHit); }
    public void PlayEnemyBuff() { PlaySFX(enemyBuff); }

    private void OnDestroy()
    {
        // Cleanup if needed in future
    }
}