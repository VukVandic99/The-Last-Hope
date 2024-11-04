 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    //  prekopiramo sve iz globalReferenca i dobijamo nov singleton
    public AudioSource shootChannel;

    public AudioClip m4Shot;
    public AudioClip P1911Shot;
    public AudioClip m107Shot;
    public AudioClip AK47Shot;
    public AudioClip RifleShot;

    public AudioSource reloadChannel;

    public AudioClip reloadingSound1911;
    public AudioClip reloadingSoundM4;
    public AudioClip reloadingSoundM107;
    public AudioClip reloadingSoundUzi;
    public AudioClip reloadingSoundAK47;
    public AudioClip reloadingSoundRifle;

    public AudioSource empyMagazineSound1911;

    //  zvukovi za zombie, single clip sto znaci kao za puske moramo ga staviti u source
    public AudioClip zombieChase;
    public AudioClip zombieAttack;
    public AudioClip zombieHurt;
    public AudioClip zombieDeath;
    //  zatim pravimo kanal na koji se svi ovi zvukovi pokrecu, kao za puske
    public AudioSource zombieChannel;
    //  popravka na zuvk
    public AudioSource zombieChannelHurtDeath;

    //  zvuk za plejera kad umire i kada ga udari zombie
    public AudioSource playeChannel;
    public AudioClip playerHit;
    public AudioClip playerDeath;

    //  dodavanje muzike
    public AudioClip playerDeadMusic;
    public AudioClip playerVictoryMusic;

    public AudioSource actionBackgroundMusic;
    public AudioClip actionMusic;
    private void Awake()
    {
        //  ovo je pravljenje singletona
        if (Instance != null && Instance != this)
        {
            //  ako postoji instanca unistavamo je, treba nam samo jedna
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void PlayShootingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol1911:
                shootChannel.PlayOneShot(P1911Shot);
                break;
            case WeaponModel.M4:
                shootChannel.PlayOneShot(m4Shot);
                break;
            case WeaponModel.M107:
                shootChannel.PlayOneShot(m107Shot);
                break;
            case WeaponModel.Uzi:
                shootChannel.PlayOneShot(P1911Shot);
                break;
            case WeaponModel.AK47:
                shootChannel.PlayOneShot(AK47Shot);
                break;
            case WeaponModel.Rifle:
                shootChannel.PlayOneShot(RifleShot);
                break;
        }
    }
    public void PlayReloadingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol1911:
                reloadChannel.PlayOneShot(reloadingSound1911);
                break;
            case WeaponModel.M4:
                reloadChannel.PlayOneShot(reloadingSoundM4);
                break;
            case WeaponModel.M107:
                shootChannel.PlayOneShot(reloadingSoundUzi);
                break;
            case WeaponModel.Uzi:
                shootChannel.PlayOneShot(reloadingSoundUzi);
                break;
            case WeaponModel.AK47:
                shootChannel.PlayOneShot(reloadingSoundAK47);
                break;
            case WeaponModel.Rifle:
                shootChannel.PlayOneShot(reloadingSoundRifle);
                break;
        }
    }
}
/*public void PlayReloadingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol1911:
                reloadingSound1911.Play();
                break;
            case WeaponModel.M4:
                reloadingSoundM4.Play();
                break;
            case WeaponModel.M107:
                reloadingSoundM107.Play();
                break;
            case WeaponModel.Uzi:
                reloadingSoundUzi.Play();
                break;
        }
    }*/
