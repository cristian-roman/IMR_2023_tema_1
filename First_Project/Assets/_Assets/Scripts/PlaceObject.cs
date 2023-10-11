using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;

    /// <summary>
    /// Listens for touch events and performs an AR raycast from the screen touch point.
    /// AR raycasts will only hit detected trackables like feature points and planes.
    ///
    /// If a raycast hits a trackable, the <see cref="placedPrefab"/> is instantiated
    /// and moved to the hit position.
    /// </summary>
    [RequireComponent(typeof(ARRaycastManager))]
    public class PlaceObject : MonoBehaviour
    {
		public MeetAnimation birdsInteraction;

        private static System.Random rnd;
        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        ARRaycastManager m_RaycastManager;
        
        [SerializeField]
        [Tooltip("Instantiates this prefab on a plane at the touch location.")]
        GameObject m_firstPlacedPrefab;
        
        [SerializeField]
        [Tooltip("Instantiates this prefab on a plane at the touch location.")]
        GameObject m_secondPlacedPrefab;

        UnityEvent placementUpdate;

        [SerializeField]
        GameObject visualObject;

        /// <summary>
        /// The prefab to instantiate on touch.
        /// </summary>
        public GameObject firstBird
        {
            get { return m_firstPlacedPrefab; }
            set { m_firstPlacedPrefab = value; }
        }
        
        public GameObject secondBird
        {
            get { return m_firstPlacedPrefab; }
            set { m_secondPlacedPrefab = value; }
        }
        /// <summary>
        /// The object instantiated as a result of a successful raycast intersection with a plane.
        /// </summary>
        public GameObject firstSpawnedObject { get; private set; }
        public GameObject secondSpawnedObject { get; private set; }

        void Awake()
        {
            m_RaycastManager = GetComponent<ARRaycastManager>();
            
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
            if (!TryGetTouchPosition(out Vector2 touchPosition))
                return;

            if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one
                // will be the closest hit.
                var hitPose = s_Hits[0].pose;
            
                if (firstSpawnedObject == null)
                {
                    Quaternion rotation = Quaternion.Euler(0, 180f, 0f);
                    firstSpawnedObject = Instantiate(m_firstPlacedPrefab, hitPose.position, rotation);
					birdsInteraction.SetShape(firstSpawnedObject);
					birdsInteraction.SetAnimator(firstSpawnedObject.GetComponent<Animator>());
                    
                }
                else if (secondSpawnedObject == null)
                {
                    Quaternion rotation = Quaternion.Euler(0f, 180f, 0f);
                    secondSpawnedObject = Instantiate(m_secondPlacedPrefab, hitPose.position, rotation);
					birdsInteraction.SetShape(secondSpawnedObject);
					birdsInteraction.SetAnimator(secondSpawnedObject.GetComponent<Animator>());
                }
                else
                {
                    switch((int)rnd.Next(2))
                    {
                        case 0:
                            firstSpawnedObject.transform.position = hitPose.position;
                            break;
                        default:
                            secondSpawnedObject.transform.position = hitPose.position;
                            break;
                    }
                }

                placementUpdate.Invoke();
            }
        }

        public void DiableVisual()
        {
            visualObject.SetActive(false);
        }
    }