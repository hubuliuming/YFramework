/****************************************************
    文件：Node.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/


using System;

namespace YC
{
    public class LinkedList<T>
    {
        private class Node
        {
            public T t;
            public Node Next;

            public Node(T t, Node next)
            {
                this.Next = next;
            }

            public Node(T t)
            {
                this.t = t;
                this.Next = null;
            }
        }

        private Node head;
        private int count;

        public LinkedList()
        {
            head = null;
            count = 0;
        }

        public int Count => count;
        public bool IsEmpty => count == 0;

        public void Add(int index, T t)
        {
            if (index <0 || index > count)
            {
                throw new ArgumentException("索引越界,当前索引值为："+index+"长度为："+count);
            }

            if (index == 0)
                head = new Node(t, head);
            else
            {
                Node temp = head;
                for (int i = 0; i < index -1; i++)
                {
                    temp.Next = head;
                }
                
                temp.Next = new Node(t, temp.Next);
            }

            count++;
        }

        public void AddFirst(T t) => Add(0, t);
        public void AddLast(T t) => Add(count, t);

        public T Get(int index)
        {
            if (index <0 || index > count)
            {
                throw new ArgumentException("索引越界,当前索引值为："+index+"长度为："+count);
            }

            Node curNode = head;
            for (int i = 0; i < index; i++)
            {
                curNode = curNode.Next;
            }

            return curNode.t;
        }

        public T GetFist() => Get(0);
        public T GetLast() => Get(count - 1);

        public void Set(int index, T newT)
        {
            if (index <0 || index > count)
            {
                throw new ArgumentException("索引越界,当前索引值为："+index+"长度为："+count);
            }

            Node curNode = head;
            for (int i = 0; i < index; i++)
            {
                curNode = curNode.Next;
            }

            curNode.t = newT;
        }

        public bool Contains(T t)
        {
            Node curNode = head;
            while (curNode != null)
            {
                if (curNode.t.Equals(t))
                    return true;
                curNode = curNode.Next;
            }

            return false;
        }
    }
}