using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TranDuc;

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
        if (musicSource == null || soundSource == null)
        {
            Debug.LogError("AudioSource not assigned in AudioManager!");
        }
    }

    void Start()
    {
        SetMusicVolume(DataManager.Instance.GameData.MusicVolume);
        SetSoundVolume(DataManager.Instance.GameData.SoundVolume);
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;
            DataManager.Instance.GameData.MusicVolume = volume;
        }
    }

    public void SetSoundVolume(float volume)
    {
        if (soundSource != null)
        {
            soundSource.volume = volume;
            DataManager.Instance.GameData.SoundVolume = volume;
        }
    }

    public void ResetDefault()
    {
        if (musicSource != null)
            musicSource.volume = 0.5f;
        if (soundSource != null)
            soundSource.volume = 0.5f;
        SetMusicVolume(0.5f);
        SetSoundVolume(0.5f);
    }

    public void PlayMusicInGame()
    {
        if (musicSource != null && musicInGame != null)
        {
            PlayMusicGame(musicInGame);
        }
    }

    public void PlayMusicStartGame()
    {
        if (musicSource != null && musicStartGame != null)
        {
            PlayMusicGame(musicStartGame);
        }
    }

    public void PlayMusicSelectRoom()
    {
        if (musicSource != null && musicSelectRoom != null)
        {
            PlayMusicGame(musicSelectRoom);
        }
    }
    private void PlayMusicGame(AudioClip clip)
    {
        musicSource.loop = true;
        musicSource.clip = clip;
        musicSource.volume = 0f;
        musicSource.Play();
        musicSource.DOFade(DataManager.Instance.GameData.MusicVolume, 0.5f).SetUpdate(true);
    }

    public void StopMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.DOFade(0f, 0.5f).OnComplete(() =>
            {
                musicSource.Stop();
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
                soundSource.PlayOneShot(sound, soundSource.volume);
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
}