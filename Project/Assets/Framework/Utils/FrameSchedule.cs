using System;
using System.Collections.Generic;
using UnityEngine;
namespace FunPlus.Common.Game 
{
	//优先级
    public enum eFrameActionPriority 
	{
        BirthPoint = 0,
        CellTickFrame,
        Other,
        Replay,
        COUNT,
    }
    
    public interface IFrameActionIndex 
	{
        void Remove();
    }

    public class FrameTickList 
	{
        class ActionHandle : ILinkedListNode<ActionHandle>, IFrameActionIndex 
		{
            
            public Action Action;//到时要触发的事件
            public int DelayFrame;//延迟帧数
            public int CallbackTimes;//回调次数
            public int DeltaFrame;//执行间融帧数
            public bool Deleted;//删除标记
            public bool UseDelay;//是否使用delay
            public bool UseCallbackTimes;//是否使用回调次数

			public int PastFrame;//已经执行的帧数
            
            public LinkedListNode<ActionHandle> Node { get; set; }

			/// <summary>
			/// 初始化
			/// </summary>
			/// <param name="action">Action.</param>
			/// <param name="delayFrame">Delay frame.</param>
			/// <param name="callbackTimes">Callback times.</param>
			/// <param name="deltaFrame">Delta frame.</param>
			public ActionHandle(Action action, int delayFrame, int callbackTimes, int deltaFrame) 
			{
				Action = action;
				DelayFrame = delayFrame;
				UseDelay = DelayFrame > 0;
				CallbackTimes = callbackTimes;
				DeltaFrame = deltaFrame;
				UseCallbackTimes = CallbackTimes > 0;
				Deleted = false;
			}

            public void Remove() 
			{
                Deleted = true;
            }
        }

        LinkedList<ActionHandle> _handles = new LinkedList<ActionHandle>();

		/// <summary>
		/// 注册事件
		/// </summary>
		/// <returns>The action.</returns>
		/// <param name="action">要执行的函数</param>
		/// <param name="delayFrame">延迟帧数</param>
		/// <param name="callbackTimes">回调次数</param>
		/// <param name="deltaFrame">执行间隔</param>
        public IFrameActionIndex RegisterAction(Action action, int delayFrame, int callbackTimes, int deltaFrame) 
		{
            ActionHandle ah = new ActionHandle(action, delayFrame, callbackTimes, deltaFrame);
            _handles.AddNode(ah);
            return ah;
        }

		/// <summary>
		/// 每帧的检查
		/// </summary>
        public void Tick() 
		{
            LinkedListNode<FrameTickList.ActionHandle> node = _handles.First;
            do 
			{
                if (node == null) 
				{
                    break;
                }
                ActionHandle handle = node.Value;
                if (handle.Deleted) //如果handle可以删除
				{
                    LinkedListNode<ActionHandle> saveNode = node;
                    node = saveNode.Next;//下一个结点
                    _handles.Remove(saveNode);
                }
                else 
				{
                    if (handle.UseDelay) //如何使用delay
					{
                        --handle.DelayFrame;
                        if (handle.DelayFrame > 0) 
						{
                            node = node.Next;
                            continue;
                        }
                        else 
						{
                            handle.UseDelay = false;
                            handle.PastFrame = handle.DeltaFrame;
                        }
                    }

                    if (handle.DeltaFrame < 0) //如果间融时间小于0
					{
                        handle.Action();
                        if (handle.UseCallbackTimes) 
						{
                            --handle.CallbackTimes;
                        }
                    }
                    else if (handle.PastFrame >= handle.DeltaFrame) //经过的时间
					{
                        handle.PastFrame = 0;
                        handle.Action();
                        if (handle.UseCallbackTimes) 
						{
                            --handle.CallbackTimes;
                        }
                    }
                    else 
					{
                        ++handle.PastFrame;
                    }

                    if (handle.UseCallbackTimes) //是否有回调次数
					{
                        if (handle.CallbackTimes <= 0) 
						{
                            handle.Deleted = true;
                            continue;
                        }
                    }
                    node = node.Next;//下一个
                }
            } while (true);
        }
    }

    public class FrameSchedule 
	{
        public int FrameCount { get; private set; }
        public float FixedDeltaTime { get; private set; }
       // bool _started;
        GameObject _gameObject;
        static FrameSchedule _inst = null;

		FrameTickList[] m_tickLst;//fixupdata更新事件,key为优先级
		FrameTickList m_tickDisplayLst;//updata更新事件

		public FrameSchedule() 
		{
			FrameCount = 0;
			FixedDeltaTime = Time.fixedDeltaTime;
			//_started = false;
			m_tickLst = new FrameTickList[(int)eFrameActionPriority.COUNT];
			for (int i = 0; i < (int)eFrameActionPriority.COUNT; ++i) 
			{
				m_tickLst[i] = new FrameTickList();
			}
			m_tickDisplayLst = new FrameTickList();
		}

        public static FrameSchedule Inst{
            get {
                if (_inst == null) {
                    Binder binder = UnityEngine.Object.FindObjectOfType<Binder>();
                    if (binder == null) {
                        GameObject obj = new GameObject(typeof(FrameSchedule).Name);
                        binder = obj.AddComponent<Binder>();
                        _inst = new FrameSchedule();
                        binder.Inst = _inst;
                        _inst._gameObject = obj;
                        obj.SetActive(false);
                    }
                }
                return _inst;
            }
        }

		 class Binder : MonoBehaviour {
            public FrameSchedule Inst;

            void FixedUpdate() {
                FrameSchedule._inst.FixedUpdate();
            }

            void Update() {
                FrameSchedule._inst.Update();
            }
            void OnDestroy() {
                FrameSchedule._inst._gameObject = null;
                FrameSchedule._inst = null;
            }
        }

        public void Start() 
		{
            if (_gameObject != null) 
			{
                _gameObject.SetActive(true);
            }
        }

		public void OnDestroy() 
		{
           	_gameObject = null;
            FrameSchedule._inst = null;
        }
        /// <summary>
        /// 注册按一定间隔时间调用的函数
        /// </summary>
        /// <returns>The action.</returns>
        /// <param name="priority">Priority.</param>
        /// <param name="action">Action.</param>
        /// <param name="deltaFrame">Delta frame.</param>
        public IFrameActionIndex RegisterAction(eFrameActionPriority priority, Action action, int deltaFrame = -1) 
		{
            return m_tickLst[(int)priority].RegisterAction(action, 0, 0, deltaFrame);
        }
        /// <summary>
        /// 注册固定回调次数的函数
        /// </summary>
        /// <returns>The action.</returns>
        /// <param name="priority">Priority.</param>
        /// <param name="action">Action.</param>
        /// <param name="callbackTimes">Callback times.</param>
        /// <param name="deltaFrame">Delta frame.</param>
        public IFrameActionIndex RegisterAction(eFrameActionPriority priority, Action action, int callbackTimes, int deltaFrame = -1) 
		{
            return m_tickLst[(int)priority].RegisterAction(action, 0, callbackTimes, deltaFrame);
        }
        /// <summary>
        /// 注册延时回调函数
        /// </summary>
        /// <returns>The delay action.</returns>
        /// <param name="priority">Priority.</param>
        /// <param name="action">Action.</param>
        /// <param name="delayFrame">Delay frame.</param>
        /// <param name="deltaFrame">Delta frame.</param>
        public IFrameActionIndex RegisterDelayAction(eFrameActionPriority priority, Action action, int delayFrame, int deltaFrame = -1) 
		{
            return m_tickLst[(int)priority].RegisterAction(action, delayFrame, 1, deltaFrame);
        }
        /// <summary>
        /// 注册延时并且指定回调次数的函数
        /// </summary>
        /// <returns>The delay action.</returns>
        /// <param name="priority">Priority.</param>
        /// <param name="action">Action.</param>
        /// <param name="delayFrame">Delay frame.</param>
        /// <param name="callbackTimes">Callback times.</param>
        /// <param name="deltaFrame">Delta frame.</param>
        public IFrameActionIndex RegisterDelayAction(eFrameActionPriority priority, Action action, int delayFrame, int callbackTimes, int deltaFrame = -1)
		{
            return m_tickLst[(int)priority].RegisterAction(action, delayFrame, callbackTimes, deltaFrame);
        }

		/// <summary>
		/// 注册事件
		/// </summary>
		/// <returns>The display action.</returns>
		/// <param name="action">Action.</param>
        public IFrameActionIndex RegisterDisplayAction(Action action) 
		{
            return m_tickDisplayLst.RegisterAction(action, 0, 0, -1);
        }

		/// <summary>
		/// 删除对应的帧事件
		/// </summary>
		/// <param name="actionIndex">Action index.</param>
        public void RemoveAction(IFrameActionIndex actionIndex) 
		{
            if (actionIndex != null) 
			{
                actionIndex.Remove();
            }
        }

        public void FixedUpdate() 
		{
			if (_gameObject == null || !_gameObject.activeSelf) 
			{
				return;
			}
			
            ++FrameCount;
            for (int i = 0; i < m_tickLst.Length; ++i) 
			{
                m_tickLst[i].Tick();
            }
        }

        public void Update() 
		{
            //// 先处理logic -> display的事件通知
            //Queue<Action> actions = ActionQueue.Inst.Actions;
            //while (actions.Count > 0) {
            //    Action action = actions.Dequeue();
            //    action();
            //}

			if (_gameObject == null || !_gameObject.activeSelf) 
			{
				return;
			}

            // 处理display update
            m_tickDisplayLst.Tick();
        }

  //      public float FrameToTime(int frame) 
		//{
        //    return frame * FixedDeltaTime;
        //}
    }
}
