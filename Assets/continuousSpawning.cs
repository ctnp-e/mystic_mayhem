using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class continuousSpawning : MonoBehaviour
{

    public static continuousSpawning instance;

    [SerializeField]
    int budget = 100;


    [Header("Used throughout to figure out what to spawn")]
    //Prefab to cost
    [SerializedDictionary("Enemy Type", "Cost")]
    public SerializedDictionary<GameObject, int> enemyCosts;
    Queue<KeyValuePair<GameObject,int>> orderToPurchase = new Queue<KeyValuePair<GameObject, int>>();

    //Only to actually 
    [SerializeField]
    List<KeyValuePair<GameObject, int>> currentlyPurchasableEnemies = new List<KeyValuePair<GameObject, int>>();
    [SerializeField]
    List<float> spawnProbabilities = new List<float>();
    [SerializeField]
    List<int> spawnedEnemies = new List<int>();

    private void Awake()
    {
        if (instance != null && instance != this) 
        {
            Destroy(this);
        }
        else 
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        var sortedDict = from i in enemyCosts orderby i.Value ascending select i;

        foreach (var i in sortedDict) 
        {
            orderToPurchase.Enqueue(i);
        }

        int orderCount = orderToPurchase.Count;
        for(int i = 0; i < orderCount; i++)
        { 
            if (orderToPurchase.Peek().Value <= budget)
            {
                currentlyPurchasableEnemies.Add(orderToPurchase.Dequeue());
            }
            else break;
        }
        updateProbabilities();

        while (budget > 0) 
        {
            SpawnEnemy(Vector3.zero);
        }
    }

    public void receiveCredits(int amount) 
    {
        budget = amount;

        updatePurchasableEnemies();
        foreach (var i in currentlyPurchasableEnemies) 
        {
            Debug.Log("Enemy purchasable: " + i);
        }

        SpawnEnemy(new Vector3(0, 0, 0));
    }

    void updatePurchasableEnemies() 
    {
        if (orderToPurchase.Count > 0) 
        {
            if (orderToPurchase.Peek().Value <= budget) 
            {    
                currentlyPurchasableEnemies.Add(orderToPurchase.Dequeue());
                updateProbabilities();
            }
        }
    }

    void updateProbabilities() 
    {
        int medianIndex = currentlyPurchasableEnemies.Count / 2;
        for (int i = 0; i < currentlyPurchasableEnemies.Count; i++) 
        {
            float distance = Mathf.Abs(i - medianIndex);
            float probability = 1f / (distance + 1);
            spawnProbabilities.Add(probability);
        }

        //Normalize probabilities
        float totalProbability = spawnProbabilities.Sum();
        for(int i = 0; i < spawnProbabilities.Count; i++) 
        {
            spawnProbabilities[i] /= totalProbability;
        }
    }


    //Location could be somewhere in the tile map or something
    //Pretty messy syntax but I think it will help make it easier to use in editor
    void SpawnEnemy(Vector3 location)
    {

        while (budget > 0)
        {
            float rand = Random.Range(0, 1f);
            float cumulativeProbability = 0f;

            for (int i = 0; i < enemyCosts.Count; i++)
            {
                spawnedEnemies.Add(0);
                cumulativeProbability += spawnProbabilities[i];

                if (rand < cumulativeProbability)
                {
                    int cost = enemyCosts[currentlyPurchasableEnemies[i].Key];
                    if (cost <= budget)
                    {
                        int maxSpawnCount = budget / cost;
                        int spawnCount = Random.Range(1, maxSpawnCount + 1);
                        Debug.Log("spawn count : " + spawnCount);
                        for (int j = 0; j < spawnCount; j++)
                        {
                            //Change with function
                            spawnedEnemies[i] += 1;
                            Debug.Log("Spawned Enemy " + i);
                        }

                        budget -= (cost * spawnCount);
                    }
                    break;
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}