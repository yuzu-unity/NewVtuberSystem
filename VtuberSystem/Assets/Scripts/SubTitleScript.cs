using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SubTitleScript : MonoBehaviour {

    public Prefab[] prefabs;
    private GameObject tmp;
    public KeyCode deleteObj=KeyCode.Escape;
    public Image image;
	// Use this for initialization
	void Start () {
        if (image.enabled)
        {
            image.enabled = false;
        }
	}

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (Input.GetKeyDown(prefabs[i].keyCode))
            {
                
                image.sprite = prefabs[i].sprite;
                if (!image.enabled)
                {
                    image.enabled = true;
                }
                if (prefabs[i].destroTime > 0)
                {
                    StopCoroutine(DeleteSprite(prefabs[i].destroTime));
                }
            }

        }

        if (Input.GetKeyDown(deleteObj))
        {
          
            if (image.enabled)
            {
                image.enabled = false;
            }
        }
    }

    [System.Serializable]
    public class Prefab
    {
        public KeyCode keyCode;
        public Sprite sprite;
        public float destroTime;

    }

    IEnumerator DeleteSprite(float time)
    {

        yield return new WaitForSeconds(time);
        image.sprite = null;
        if (image.enabled)
        {
            image.enabled = false;
        }

    }
}
