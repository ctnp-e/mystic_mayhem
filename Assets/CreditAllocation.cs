using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

    public enum scalingType
    {
        LINEAR,
        LOGARITHMIC,
        QUADRATIC,
        EXPONENTIAL
    }

public class CreditAllocation : MonoBehaviour
{
    [Header("Credits")]
    [SerializeField]
    int startingAmount = 100;
    [SerializeField]
    int increment = 10;
    [SerializeField]
    float amountOfCreditsToGive = 0;
    [SerializeField]
    int upperCap = 500;
    [SerializeField]
    scalingType type;

    [SerializeField]
    int interval;

    [SerializeField]
    float logExp;
    [SerializeField]
    float exponential;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("allocateCredits");

    }

    IEnumerator allocateCredits() 
    {
        while (true) 
        {
            yield return new WaitForSecondsRealtime(interval);
            switch (type) 
            {
                case scalingType.LINEAR:
                    amountOfCreditsToGive += increment;
                    break;
                case scalingType.QUADRATIC:
                    amountOfCreditsToGive += increment * increment;
                    break;
                case scalingType.LOGARITHMIC:
                    amountOfCreditsToGive += Mathf.Log(logExp, increment);
                    break;
                case scalingType.EXPONENTIAL:
                    amountOfCreditsToGive += Mathf.Pow(exponential, increment);
                    break;
            }
            if (amountOfCreditsToGive > upperCap) 
            {
                amountOfCreditsToGive = upperCap;
            }
            else 
            {
                increment += increment;
            }
            Debug.Log("Giving this many credits " + (startingAmount + amountOfCreditsToGive));
            continuousSpawning.instance.receiveCredits(startingAmount + (int)amountOfCreditsToGive);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
