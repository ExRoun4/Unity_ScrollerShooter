using UnityEngine;

public class LevelFinishGameLine : MonoBehaviour
{

    #region BEHAVIOR

    private void Update()
    {
        if(MainLevelManagement.instance.GetGameState() != MainLevelManagement.GAME_STATE.STARTED) return;

        PlayerBase player = GameRoot.instance.GetActivePlayer();
        if(player.transform.position.z >= transform.position.z){
            PlayerCrossedLine();
        }
    }

    private void PlayerCrossedLine(){
        MainLevelManagement.instance.ProduceFinishGame(MainLevelManagement.GameEndReasons.WIN);
    }

    #endregion


    #region EDITOR

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(1.0f, 0.25f, 1.0f));
    }

    #endregion
}
