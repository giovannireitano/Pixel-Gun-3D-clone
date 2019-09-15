using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class TakingDamage : MonoBehaviourPunCallbacks
{

    [Header("Damage Related Stuff")]
    private float startHealth = 100f;

    public float health;
    public float currentHealth;
    
    public Image healthBar;
    GameObject[] Players;

    private void Start()
    {
        health = startHealth;
        healthBar.fillAmount = health / startHealth;
        Players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < Players.Length; i++)
        {
            Players[i].GetComponent<PhotonView>().RPC("refresh", RpcTarget.AllBufferedViaServer, health);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (photonView.IsMine)
            {
                photonView.RPC("refreshHealth", RpcTarget.AllBufferedViaServer, health);
            }
        }
    }

    [PunRPC]
    public void TakeDamage(float _damage, PhotonMessageInfo info)
    {
        Debug.Log(health);
        this.health -= _damage;
        currentHealth = health;
        health = currentHealth;
       // this.health = health;
        healthBar.fillAmount = health / 100f;

        if (this.health <= 0f)
        {
            Die(); 
        }
    }

    [PunRPC]
    public void refresh(float _health)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("refreshHealth", RpcTarget.AllBufferedViaServer, health);
        }
    }

    [PunRPC]
    public void refreshHealth(float _health)
    {
            health = _health;
            healthBar.fillAmount = health / 100f;
    }

    void Die()
    {
        if (photonView.IsMine)
        {
            PixelGunGameManager.instance.LeaveRoom();


        }
    }
  
}
