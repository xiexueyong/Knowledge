using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HedgehogTeam.EasyTouch
{
    public class EasyTouchEvents : MonoBehaviour
    {
        public class ET_TapEvent : UnityEvent<Vector3> {}
        public class ET_TapEventV2 : UnityEvent<Vector2> {}
        
        [SerializeField]
        bool isTouchStartOn=false;
        [SerializeField]
        bool isTouchDownOn=false; 
        [SerializeField]
        bool isTouchUpOn=false;
        [SerializeField]
        bool isDragOn=false;
        [SerializeField]
        bool isSimpleTapOn=false;
        
        public ET_TapEvent onTouchStart = new ET_TapEvent();
        public ET_TapEvent onTouchDown= new ET_TapEvent();
        public ET_TapEvent onTouchUp = new ET_TapEvent();
        public ET_TapEventV2 onDrag=new  ET_TapEventV2();
        public ET_TapEvent onSimpleTap=new ET_TapEvent();
        void Awake()
        {
            if (isTouchStartOn)
            {
                EasyTouch.On_TouchStart += On_TouchStart;
            }
            if (isTouchDownOn)
            {
                EasyTouch.On_TouchDown += On_TouchDown;
            }
            if (isTouchUpOn)
            {
                EasyTouch.On_TouchUp += On_TouchUp;
            }
            if (isSimpleTapOn)
            {
                EasyTouch.On_SimpleTap += On_SimpleTap;
            }
            if (isDragOn)
            {
                EasyTouch.On_Drag += On_Drag;
            }
        }

        void On_TouchStart(Gesture gesture)
        {
            if (onTouchStart != null)
                onTouchStart.Invoke(GetWorldPosition(gesture));
        }

        void On_TouchDown(Gesture gesture)
        {
            if (onTouchDown != null)
                onTouchDown.Invoke(GetWorldPosition(gesture));
        }

        void On_TouchUp(Gesture gesture)
        {
            if (onTouchUp != null)
                onTouchUp.Invoke(GetWorldPosition(gesture));
        }

        void On_SimpleTap(Gesture gesture)
        {
            if (onSimpleTap!=null)
            {
                onSimpleTap.Invoke(Vector3.zero);
            }
        }

        void On_Drag(Gesture gesture)
        {
            if (onDrag!=null)
            {
                onDrag.Invoke(gesture.deltaPosition);
            }
        }


        Vector3 GetWorldPosition(Gesture gesture)
        {
             return gesture.GetTouchToWorldPoint(gesture.position);
        }

        void OnDestroy()
        {
            EasyTouch.On_TouchStart -= On_TouchStart;
            EasyTouch.On_TouchDown -= On_TouchDown;
            EasyTouch.On_TouchUp -= On_TouchUp;
            EasyTouch.On_SimpleTap -= On_SimpleTap;
            EasyTouch.On_Drag -= On_Drag;
        }
    }
}