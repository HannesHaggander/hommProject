using UnityEngine;
using System.Collections;

public class MasterObject : MonoBehaviour {
    public static MasterObject me = null;

	void Awake () {
        // makes sure that there only is one instance of the master object
	    if(me == null)
        {
            me = this;
            DontDestroyOnLoad(gameObject);
        } else if(me != this)
        {
            Destroy(gameObject);
        }
	}
}
