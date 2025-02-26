using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCamera : MonoBehaviour
{
    public void ReparentToObject(Transform obj, bool assignObjTransform = false){
        transform.parent = obj;
        if(assignObjTransform){
            transform.position = obj.position;
            transform.rotation = obj.rotation;
        }
    }

    public void ReparentToRootScene(){
        transform.parent = null;
        SceneManager.MoveGameObjectToScene(this.gameObject, GameRoot.instance.GetRootScene());
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }
}
