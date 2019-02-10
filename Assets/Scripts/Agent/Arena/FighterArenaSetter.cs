using UnityEngine;

public class FighterArenaSetter : MonoBehaviour
{
    public ArenaFighterChooser ArenaFighterChooser;

    private void Start()
    {
        ArenaFighterChooser.OnChooseFighter += SetPlayerFighterPos;
    }

    public void SetPlayerFighterPos(Fighter fighter)
    {
        Vector3 initPos = new Vector3(-5f, 3f, 0f);
        fighter.transform.position = initPos;

        float minPartDist = 99999f;

        foreach (GameObject part in fighter.BodyParts.getParts())
        {
            float partDist = distToFloor(part.transform);
            minPartDist = partDist < minPartDist ? partDist : minPartDist;
        }

        minPartDist -= .1f;
        initPos.y -= minPartDist;
        initPos.x = -1.5f;
        fighter.transform.position = initPos;
    }

    public void SetEnemyFighterPos(Fighter fighter)
    {
        fighter.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        Vector3 initPos = new Vector3(5f, 3f, 0f);
        fighter.transform.position = initPos;

        float minPartDist = 99999f;

        foreach (GameObject part in fighter.BodyParts.getParts())
        {
            float partDist = distToFloor(part.transform);
            minPartDist = partDist < minPartDist ? partDist : minPartDist;
        }

        minPartDist -= .1f;
        initPos.y -= minPartDist;
        initPos.x = 1.5f;

        fighter.transform.position = initPos;
    }
    
    public static float distToFloor(Transform transform)
    {
        int layerMask = 1 << LayerMask.NameToLayer("Ground");
        //layerMask = ~layerMask;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1000, layerMask, QueryTriggerInteraction.Ignore))
        {
            //Debug.DrawRay(transform.position, Vector3.down * hit.distance);
            return hit.distance;
        }
        else
        {
            return 0;
        }
    }
}