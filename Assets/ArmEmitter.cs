using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmEmitter : MonoBehaviour
{
    ParticleSystem particleSystem;
    // Start is called before the first frame update
    void Start()
    {
        particleSystem = this.GetComponent<ParticleSystem>();
    }
    void OnEnable()
    {
        /*
        ParticleSystem particleSystem = this.GetComponent<ParticleSystem>();
        particleSystem.Clear();
        particleSystem.Play();
        */
    }

    void OnDisable()
    {
      //  Destroy(gameObject);
    }


    // Update is called once per frame
    void Update()
    {

        if (!particleSystem.isPlaying) particleSystem.Play();



    }
}
