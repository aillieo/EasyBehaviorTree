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

    [SerializeField]
    private Pool pool;

    [SerializeField]
    private GameObject mapObj;

    [SerializeField]
    private Transform sceneRoot;

    [SerializeField]
    private Camera cam;

    public int initHeroCount = 20;

    public float heroCreateInterval = 2f;

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
        Bullet b = pool.Get(bullet, sceneRoot);
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
        pool.Recycle(bullet);
    }

    public Hero SpawnHero()
    {
        Hero h = pool.Get(hero, sceneRoot);
        h.transform.position =
            Vector3.right * UnityEngine.Random.Range(- mapSize/2, mapSize/2) +
            Vector3.forward * UnityEngine.Random.Range(-mapSize / 2, mapSize / 2);
        heroes.Add(h);
        h.Restart();
        return h;
    }

    public void RemoveHero(Hero hero)
    {
        pool.Recycle(hero);
    }

    public ReadOnlyCollection<Hero> GetHeroes()
    {
        return heroes.AsReadOnly();
    }

    private void Start()
    {
        mapObj.transform.localScale = new Vector3(mapSize, mapSize, 1f);
        cam.transform.localPosition = new Vector3(0, 5f, 7.5f) * mapSize / 10;

        for (int i = 0; i < initHeroCount; ++i)
        {
            SpawnHero();
        }
        StartCoroutine(KeepCreatingNewHero(new WaitForSeconds(heroCreateInterval)));
    }

    private IEnumerator KeepCreatingNewHero(WaitForSeconds wait)
    {
        while(true)
        {
            yield return wait;
            SpawnHero();
        }
    }
}