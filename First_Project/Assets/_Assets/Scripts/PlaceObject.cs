using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;

    [RequireComponent(typeof(ARRaycastManager))]
    public class PlaceObject : MonoBehaviour
    {
        public float BetweenDistance = 0.3f;
        
        private static System.Random rnd;
        static List<ARRaycastHit> hits = new List<ARRaycastHit>();

        ARRaycastManager rayCastManager;
        
        [SerializeField]
        [Tooltip("Instantiates this prefab on a plane at the touch location.")]
        GameObject firstPlacedPrefab;
        
        [SerializeField]
        [Tooltip("Instantiates this prefab on a plane at the touch location.")]
        GameObject secondPlacedPrefab;

        UnityEvent placementUpdate;

        [SerializeField]
        GameObject visualObject;

        /// <summary>
        /// The prefab to instantiate on touch.
        /// </summary>
        public GameObject firstBird
        {
            get { return firstPlacedPrefab; }
            set { firstPlacedPrefab = value; }
        }
        
        public GameObject secondBird
        {
            get { return secondPlacedPrefab; }
            set { secondPlacedPrefab = value; }
        }
        /// <summary>
        /// The object instantiated as a result of a successful raycast intersection with a plane.
        /// </summary>
        public GameObject firstSpawnedObject { get; private set; }
        public GameObject secondSpawnedObject { get; private set; }

        void Awake()
        {
            rayCastManager = GetComponent<ARRaycastManager>();
            
            rnd = new System.Random();

            if (placementUpdate == null)
                placementUpdate = new UnityEvent();

                placementUpdate.AddListener(DiableVisual);
        }

        bool TryGetTouchPosition(out Vector2 touchPosition)
        {
            if (Input.touchCount > 0)
            {
                touchPosition = Input.GetTouch(0).position;
                return true;
            }

            touchPosition = default;
            return false;
        }

        void Update()
        {
            if (firstSpawnedObject != null && secondSpawnedObject != null)
            {
                var distance = Vector3.Distance(firstSpawnedObject.transform.position, secondSpawnedObject.transform.position);
                if (distance < BetweenDistance)
                {
                    Animator animator = firstSpawnedObject.GetComponent<Animator>();
                    animator.SetBool("IsAttacking", true);
                    
                    animator = secondSpawnedObject.GetComponent<Animator>();
                    animator.SetBool("IsAttacking", true);
                   
                    // Make the birds face each other
                    Vector3 direction = secondSpawnedObject.transform.position - firstSpawnedObject.transform.position;
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    firstSpawnedObject.transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
                    secondSpawnedObject.transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y + 180, 0);
                }
                else
                {
                    Animator animator = firstSpawnedObject.GetComponent<Animator>();
                    animator.SetBool("IsAttacking", false);
                    
                    animator = secondSpawnedObject.GetComponent<Animator>();
                    animator.SetBool("IsAttacking", false);
                }
            }
            
            if (!TryGetTouchPosition(out Vector2 touchPosition))
                return;

            if (rayCastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one
                // will be the closest hit.
                var hitPose = hits[0].pose;
            
                if (firstSpawnedObject == null)
                {
                    Quaternion rotation = Quaternion.Euler(0, 0f, 0f);
                    firstSpawnedObject = Instantiate(firstPlacedPrefab, hitPose.position, rotation);
                }
                else if (secondSpawnedObject == null)
                {
                    Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
                    secondSpawnedObject = Instantiate(secondPlacedPrefab, hitPose.position, rotation);
                }
                else
                {
                    Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
                    Destroy(secondSpawnedObject);
                    secondSpawnedObject = Instantiate(secondPlacedPrefab, hitPose.position, rotation);
                }
                placementUpdate.Invoke();
            }
        }
        public void DiableVisual()
        {
            visualObject.SetActive(false);
        }
    }