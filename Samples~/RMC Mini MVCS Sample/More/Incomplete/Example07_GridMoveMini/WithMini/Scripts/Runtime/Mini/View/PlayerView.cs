using System;
using System.Collections;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Samples.GridMove.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;
using UnityEngine.Events;

namespace RMC.Core.Architectures.Mini.Samples.GridMove.WithMini.Mini.View
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The View handles user interface and user input
    /// </summary>
    public class PlayerView: MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector] 
        public readonly UnityEvent OnMoveComplete = new UnityEvent();

        
        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }

        
        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;
        private Coroutine _moveCoroutine;
        
        [Range(0.5f, 5f)][SerializeField] 
        private float _moveSpeed = 3;
        
        [Range(0.5f, 5f)][SerializeField] 
        private float _moveArcHeight = 1;

        [SerializeField] 
        private AnimationCurve _moveArcCurve;

        
        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;
                
                //
                GridMoveModel gridMoveModel = Context.ModelLocator.GetItem<GridMoveModel>();
                gridMoveModel.GridCellIndex.OnValueChanged.AddListener(
                    (p, c) => GridMoveModel_GridCellIndex_OnValueChanged(p, c));

            }
        }
        
        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }
        
        
        //  Unity Methods ---------------------------------

        
        //  Methods ---------------------------------------
        public void Move(Vector3 position)
        {
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
                _moveCoroutine = null;
            }
            _moveCoroutine = StartCoroutine(Move_Coroutine(position));
        }
        
        
        /// <summary>
        /// Move the player in an arc (like a human hand raising a chess piece
        /// when moving it between squares) to the destination
        /// </summary>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        private IEnumerator Move_Coroutine(Vector3 targetPosition)
        {
            Vector3 nextPosition;
            float distanceOriginal = Vector3DistanceWithoutY(transform.position, targetPosition);
            float distanceCurrent;
            float yOriginal = transform.position.y;
            do
            {
                distanceCurrent = Vector3DistanceWithoutY(transform.position, targetPosition);
                float percentageJourney = distanceCurrent / distanceOriginal;
                float t = Time.deltaTime * _moveSpeed / 2;
                float yOffset = _moveArcCurve.Evaluate(percentageJourney) * _moveArcHeight;
                
                nextPosition = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * _moveSpeed / 2);
                transform.position = Vector3.Lerp(nextPosition, targetPosition, t);
                transform.position = new Vector3(
                    transform.position.x,
                    yOriginal + yOffset,
                    transform.position.z);
                
                yield return null;
            } while (distanceCurrent > 0.01f);
            
            transform.position = targetPosition;
            OnMoveComplete.Invoke();
        }
        
        private float Vector3DistanceWithoutY (Vector3 v1, Vector3 v2)
        {
            float xDiff = v1.x - v2.x;
            float zDiff = v1.z - v2.z;
            return Mathf.Sqrt((xDiff * xDiff) + (zDiff * zDiff));
        }

        
        //  Event Handlers --------------------------------
        private void GridMoveModel_GridCellIndex_OnValueChanged(
            int previousValue, int currentValue)
        {
            RequireIsInitialized();
            
            //TODO Remove this?

        }
    }
}