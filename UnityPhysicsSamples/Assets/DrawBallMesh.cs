using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBallMesh : MonoBehaviour
{

    public List<Vector3> positions;

    public List<Vector3> simulatedPositions;

    public void OnDrawGizmos()
    {
        /*      positions = BallMoveScript.ballPositions;

              for (int i = 0; i < positions.Count;i++)
              {
                  Gizmos.DrawWireSphere(positions[i], 0.5f);
              }

              Gizmos.color = Color.red;
              Gizmos.DrawWireSphere(BallMoveScript.loadState,0.5f);

              simulatedPositions = BallMoveScript.simulatedBallPositions;
              Gizmos.color = Color.blue;
              for (int i = 0; i < simulatedPositions.Count; i++)
              {
                  Gizmos.DrawWireSphere(simulatedPositions[i], 0.5f);
              }

              Gizmos.color = Color.black;
              Gizmos.DrawWireSphere(BallMoveScript.simulationEndPosition,0.5f);*/

        Gizmos.color = Color.red;
       // Gizmos.DrawWireSphere(RollBacker.rollBackSpot,0.5f);
        Gizmos.color = Color.blue;
       // simulatedPositions = RollBacker.simulationPositions;
       /* for (int i = 0; i < simulatedPositions.Count; i++)
        {
            Gizmos.DrawWireSphere(simulatedPositions[i], 0.5f);
        }*/
    }
}
