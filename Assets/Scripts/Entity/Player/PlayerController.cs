using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class PlayerController : MonoBehaviour
{
    public LayerMask controllerCollisionMask;

    private Rigidbody rigidBody;
    private bool canBeControlled = false;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        Cursor.visible = false;
    }

    #region CONTROLLING

    private void Update()
    {
        if(!canBeControlled) return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        
        if(Physics.Raycast(ray, out rayHit, 100.0f, controllerCollisionMask)){
            Vector3 movePoint = rayHit.point;
            transform.position = ClampPointToScreenBorders(movePoint);
        }
    }

    private Vector3 ClampPointToScreenBorders(Vector3 point){
        Vector3 result = point;
        Vector3 upperLeftCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.transform.position.y));
        Vector3 lowerRightCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Camera.main.transform.position.y));

        result.x = Mathf.Clamp(result.x, upperLeftCorner.x, lowerRightCorner.x);
        result.y = 0.0f;
        result.z = Mathf.Clamp(result.z, lowerRightCorner.z, upperLeftCorner.z);

        return result;
    }

    #endregion


    #region 

    public void SetControlling(bool value){
        canBeControlled = value;
    }

    #endregion
}
