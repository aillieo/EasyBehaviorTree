using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private Bullet bullet;

    [SerializeField]
    private Hero hero;

    public int heroCount = 2;

    public int mapSize = 100;

    private List<Hero> heroes = new List<Hero>();

    static private GameManager mInstance;

    static public GameManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = GameObject.Find("GameManager").GetComponent<GameManager>();
            }

            return mInstance;
        }
    }

    public Bullet GetBullet(Transform transformToAlign = null)
    {
        Bullet b = GameObject.Instantiate(bullet, Instance.transform);
        // b.transform.SetParent(Instance.transform);
        if (transformToAlign)
        {
            b.transform.position = transformToAlign.position;
            b.transform.rotation = transformToAlign.rotation;
        }
        return b;
    }

    public void SendDamage(Vector3 position, int damage)
    {
        foreach (var h in heroes)
        {
            if (h == null || !h.alive)
            {
                continue;
            }
            if ((h.transform.position - position).sqrMagnitude < 0.1f)
            {
                h.OnDamage(damage);
            }
        }
        heroes.RemoveAll(hero => hero == null|| !hero.alive);
    }

    public void RecycleBullet(Bullet bullet)
    {
        GameObject.Destroy(bullet.gameObject);
    }

    public Hero SpawnHero()
    {
        Hero h = GameObject.Instantiate(hero, Instance.transform);
        h.transform.position =
            Vector3.right * UnityEngine.Random.Range(- mapSize/2, mapSize/2) +
            Vector3.forward * UnityEngine.Random.Range(-mapSize / 2, mapSize / 2);
        heroes.Add(h);
        return h;
    }

    public void RemoveHero(Hero hero)
    {
        GameObject.Destroy(hero.gameObject);
    }

    public ReadOnlyCollection<Hero> GetHeroes()
    {
        return heroes.AsReadOnly();
    }

    private void Start()
    {
        for (int i = 0; i < heroCount; ++i)
        {
            SpawnHero();
        }
    }
}