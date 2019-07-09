using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoScreenController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine("LogoTransition");
    }

    IEnumerator LogoTransition()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(1);
    }
}
