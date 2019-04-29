using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject berryPrefab;
    [SerializeField] int maxSlimesOwned = 4;
    [SerializeField] LayerMask whatIsSlime = new LayerMask();
    GameObject[] slimesOwnedList = new GameObject[4];
    GameObject playerObject;
    public static GameManager instance = null;

    int berryCount = 0;
    int slimesOwned = 0;    

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CollectBerry ()
    {
        berryCount++;
    }

    public void ThrowBerry()
    {
        if (berryCount > 0)
        {
            Vector3 berryPos;

            if(playerObject.GetComponent<CharacterController2D>().IsFacingRight())
                berryPos = new Vector3(playerObject.transform.position.x + 1, playerObject.transform.position.y - 0.9f, 0);
            else
                berryPos = new Vector3(playerObject.transform.position.x - 1, playerObject.transform.position.y - 0.9f, 0);

            GameObject berry = Instantiate(berryPrefab, berryPos, Quaternion.identity, null);
            berry.name = "Berry";
            berryCount--;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(berry.transform.position, 10, whatIsSlime);

            if (colliders.Length > 0)
            {
                SlimeAI bestOption = colliders[0].gameObject.GetComponent<SlimeAI>();

                float bestOptionDistance = Vector3.Distance(berry.transform.position, bestOption.transform.position);

                for (int i = 1; i < colliders.Length; i++)
                {
                    float distanceToSlime = Vector3.Distance(berry.transform.position, colliders[i].transform.position);

                    // If current slime is free and it's distance to berry is less than best option
                    if (distanceToSlime < bestOptionDistance)
                    {
                        bestOption = colliders[i].gameObject.GetComponent<SlimeAI>();
                        bestOptionDistance = Vector3.Distance(berry.transform.position, bestOption.transform.position);
                    }
                }

                bestOption.SetTarget(berry.transform);
                bestOption.CatchBerry();
            }
        }
    }

    public bool CatchSlime (GameObject newSlime)
    {
        if (slimesOwned < maxSlimesOwned)
        {
            slimesOwnedList[slimesOwned] = newSlime;
            newSlime.GetComponent<SlimeAI>().SetTarget(playerObject.transform.GetChild(slimesOwned));
            slimesOwned++;
            return true;
        } else
        {
            return false;
        }
    }

    public void StartGame ()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame ()
    {
        Application.Quit();
    }

    public void FreeSlime (GameObject slime)
    {
        for (int i = 0; i < maxSlimesOwned; i++)
        {
            if(slimesOwnedList[i] == slime)
            {
                for (int j = i + 1; j < maxSlimesOwned; j++)
                {
                    slimesOwnedList[j - 1] = slimesOwnedList[j];
                }
                
                break;
            }
        }

        slimesOwned--;
    }
}
