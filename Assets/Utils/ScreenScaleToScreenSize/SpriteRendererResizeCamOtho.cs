using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRendererResizeCamOtho : MonoBehaviour
{

        // Start is called before the first frame update
    
 
//            var height = Camera.main.orthographicSize * 2.0;
//            var width = height * Screen.width / Screen.height;
//            transform.localScale = new Vector3 ((float)width, (float)height, 0.1f);
//
//        public int defaultWidth = 1080, defaultHeight = 1920;
//        public Vector3 scale;

        public SpriteRenderer sprite;
 
        // Use this for initialization
        void Start()
        {
                var sr = sprite;
                if (sr == null) return;
     
                transform.localScale = new Vector3(1,1,1);
     
                var width = sr.sprite.bounds.size.x;
                var height = sr.sprite.bounds.size.y;
     
                var worldScreenHeight = Camera.main.orthographicSize * 2.0;
                var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
                transform.localScale = new Vector3 ( (float)worldScreenWidth / width,  transform.localScale.y, 0.1f);
                //adjust height with this code in above vector3 (float)worldScreenHeight / height;
                
        }

}
