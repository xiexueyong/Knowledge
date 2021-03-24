using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FunPlus.Common.Game 
{
    public interface ILinkedListNode<T> 
	{
        LinkedListNode<T> Node { get; set; }
    }

    public static class LinkedListExtend 
	{
        public static T AddNode<T>(this LinkedList<T> inst, T t) where T : ILinkedListNode<T> 
		{
            t.Node = inst.AddLast(t);
            return t;
        }

        public static void RemoveNode<T>(this LinkedList<T> inst, T t) where T : ILinkedListNode<T> 
		{
            inst.Remove(t.Node);
            t.Node = null;
        }
    }
}
