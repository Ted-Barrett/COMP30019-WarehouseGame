using System.Collections.Generic;
using UnityEngine;

public class BoxCollector : MonoBehaviour
{
    
    [SerializeField]
    private BoxType boxeTypesToCollect;

    [SerializeField]
    private ScoreScript score;

    [SerializeField]
    private BezosFlasherScript bezosFlasher;

    [SerializeField]
    private int pointsPerBox;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        // handle
        IBoxContainer boxContainer = other.gameObject.GetComponent<IBoxContainer>();
        if (boxContainer != null) 
        {
            List<Box> boxes = boxContainer.UnloadBoxes(boxeTypesToCollect);
            foreach (var b in boxes)
            {
                if (b.GivenScore)
                {
                    continue;
                }
                b.giveScore();

                var particleEffect = b.GetComponent<PickableItem>().getCollectEffect();
                Instantiate(particleEffect, b.transform.position + new Vector3(0.0f,-0.5f,0.0f), Quaternion.identity);
                Destroy(b.gameObject);
                score.AddScore(pointsPerBox);
                bezosFlasher.BoxCollected();
            }
        }
        else
        {
            Box box = other.gameObject.GetComponent<Box>();
            if (box != null && box.Type == boxeTypesToCollect && box.RigidBody.isKinematic == false && box.GivenScore == false)
            {
                box.giveScore();
                var particleEffect = box.GetComponent<PickableItem>().getCollectEffect();
                Instantiate(particleEffect, box.transform.position + new Vector3(0.0f,-0.5f,0.0f), Quaternion.identity);
                Destroy(box.gameObject);
                score.AddScore(pointsPerBox);
                bezosFlasher.BoxCollected();
            }
        }
    }
}
