using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalScore : MonoBehaviour {

    public int score = 0;
    public int clearScore = 500;

    public Text text;
    public GameObject winScreen;

    public GameObject Particle;

    private void Start()
    {
        text.text = "Score: " + score.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Crystal")
        {
            score += 100;
            text.text = "Score: " + score.ToString();

            Instantiate(Particle, collision.transform.position, collision.transform.rotation);
            Destroy(collision.gameObject);

            if(score >= clearScore)
            {
                // win !!
                winScreen.SetActive(true);
            }
        }
    }
}
