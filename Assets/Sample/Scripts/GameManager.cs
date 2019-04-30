using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private Bullet bullet;

    [SerializeField]
    private Hero hero;

    public int heroCount = 2;

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

    public void RecycleBullet(Bullet bullet)
    {
        GameObject.Destroy(bullet.gameObject);
    }

    public Hero SpawnHero()
    {
        Hero h = GameObject.Instantiate(hero, Instance.transform);
        h.transform.position = Vector3.right * Random.Range(-100, 100) + Vector3.forward * Random.Range(-100, 100);
        return h;
    }

    private void Start()
    {
        for (int i = 0; i < heroCount; ++i)
        {
            SpawnHero();
        }
    }
}