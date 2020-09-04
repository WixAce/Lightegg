using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILoadScene : MonoBehaviour {

    [SerializeField] private string _scene="Fragment";
    
    void Awake()=> GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene(_scene); });

}
