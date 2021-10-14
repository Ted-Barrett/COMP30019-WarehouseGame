using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Spawns and moves forklifts along a given path. Takes a list of objects as the input, which are the path pieces.
/// They must have a PathInfo script attached.
///
/// Rules:
/// - Forklifts must start on a straight piece (this should be placed off screen)
/// - Only nodes at which the forklift must start doing something different need to be given to this script
/// - The end of a forklift track piece without a white line is the point at which that section 'starts'
/// </summary>
public class ForkliftSpawningScript : MonoBehaviour
{
    // list of path nodes (track pieces in scene)
    [SerializeField] private GameObject[] pathNodes;
    private PathInfo.PathTypeEnum[] nodeTypes;
    private PathInfo[] pathScripts;

    [SerializeField] private GameObject forkliftPrefab;

    // parameters determining the movement and spawning behaviour.
    [SerializeField] private int clusterSize;
    [SerializeField] private float speed;
    [SerializeField] private float clusterInterval;

    private int currentCluster = 0;
    private float _spawnInterval;

    // length of forklift, used to spawn forklifts one after another, but without colliding.
    private readonly float _forkliftLength = 1.865f;

    // stores all forklifts which are currently spawned. Forklifts destroy themselves when they become invisible, and
    // they are then removed from _spawnedForklifts at the start of Update()
    private Dictionary<GameObject, ForkliftInfoScript> _spawnedForklifts =
        new Dictionary<GameObject, ForkliftInfoScript>();

    // offset from track point at which the forklift is spawned.
    private static readonly Vector3 TrackOffset = new Vector3(0.0f, 0.1f, 0.0f);

    // the most recent time at which a forklift was spawned
    private float _lastSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        var forklift = Instantiate(forkliftPrefab, pathNodes[0].transform.position + TrackOffset, Quaternion.identity);
        _spawnedForklifts.Add(forklift, forklift.GetComponent<ForkliftInfoScript>());
        _lastSpawnTime = Time.time;
        _spawnInterval = _forkliftLength / speed;

        nodeTypes = new PathInfo.PathTypeEnum[pathNodes.Length];
        pathScripts = new PathInfo[pathNodes.Length];
        for (int i = 0; i < pathNodes.Length; i++)
        {
            pathScripts[i] = pathNodes[i].GetComponent<PathInfo>();
            nodeTypes[i] = pathScripts[i].GETType();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // checks if it's time to spawn another forklift
        if (Time.time > _lastSpawnTime + _spawnInterval &&
            (currentCluster < clusterSize - 1 || Time.time > _lastSpawnTime + clusterInterval))
        {
            var forklift = Instantiate(forkliftPrefab, pathNodes[0].transform.position + TrackOffset,
                Quaternion.identity);
            _spawnedForklifts.Add(forklift, forklift.GetComponent<ForkliftInfoScript>());
            _lastSpawnTime = Time.time;

            currentCluster += 1;
            currentCluster = currentCluster % clusterSize;
        }

        // clears all destroyed forklifts from the dictionary
        foreach (var forklift in new List<GameObject>(_spawnedForklifts.Keys))
        {
            if (forklift.gameObject.IsDestroyed())
            {
                _spawnedForklifts.Remove(forklift);
            }
        }

        // loops through all forklifts and updates their position based on what track piece they are on
        foreach (var forklift in _spawnedForklifts)
        {
            // checks if the track piece needs to be advanced,
            // as long as the forklift has not passed the final track piece
            if (forklift.Value.CurrentNode != pathNodes.Length - 1)
            {
                // checks if the forklift has passed the start point of the next track piece
                if (Vector3.Dot(
                    pathNodes[forklift.Value.CurrentNode + 1].transform.position - forklift.Key.transform.position,
                    forklift.Key.transform.forward) < 0.0f)
                {
                    forklift.Value.CurrentNode += 1;
                }
            }

            // if the track piece the forklift is on is straight, just go forward a bit
            if (nodeTypes[forklift.Value.CurrentNode] == PathInfo.PathTypeEnum.Straight)
            {
                forklift.Key.transform.rotation = pathNodes[forklift.Value.CurrentNode].transform.rotation;
                forklift.Key.transform.Translate(new Vector3(0f, 0f, speed * Time.deltaTime));
            }
            // if the track piece the forklift is on is a corner:
            else if (nodeTypes[forklift.Value.CurrentNode] == PathInfo.PathTypeEnum.Corner)
            {
                var originalPos = forklift.Key.transform.position;
                // vector from center of curve to forklift's position
                var fromPivot = originalPos - pathNodes[forklift.Value.CurrentNode].transform.position;
                // uses cross product to determine if the forklift needs to rotate clockwise or counterclockwise
                float direction = Vector3.Dot(Vector3.Cross(Vector3.up, fromPivot), forklift.Key.transform.forward);
                if (direction > 0)
                {
                    direction = 1.0f;
                }
                else
                {
                    direction = -1.0f;
                }

                // calculates rotation to be applied about pivot point
                var rotation = Quaternion.Euler(0,
                    (float) (direction * speed / pathScripts[forklift.Value.CurrentNode].radius * 180.0f / Math.PI) * Time.deltaTime, 0);
                
                // moves and rotates forklift
                forklift.Key.transform.position = originalPos - fromPivot + rotation * fromPivot;
                forklift.Key.transform.Rotate(new Vector3(0.0f, rotation.eulerAngles.y, 0.0f));
            }
            else
            {
                throw new NotImplementedException("This type of track has not been implemented.");
            }
        }
    }
}