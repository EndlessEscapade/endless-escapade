using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EEMod.Extensions;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod
{
    /*
     * The purpose of this class is to allow reutilization of arrays of a given type
     * 
     * for example, if a huge array of strings is created, it could be given to the GC and
     * it would be allocated and claimed constantly which could be expensive or it could 
     * be reused either for the next time is needed or when something else requires an array
     * close the the size
     */
    static class BufferPool
    {
        public static BufferRequest<T> Request<T>(int capacity)
        {
            return BufferPool<T>.Request(capacity);
        }
        public static void Reuse<T>(BufferRequest<T> buffer)
        {
            BufferPool<T>.Reuse(buffer);
        }
    }
    static class BufferPool<T>
    {
        static List<BufferRequest<T>> reusedBuffers;
        public static BufferRequest<T> Request(int capacity)
        {
            if (reusedBuffers != null)
            {
                for (int i = 0; i < reusedBuffers.Count; i++)
                {
                    var buffer = reusedBuffers[i];
                    if (buffer != null && buffer.Capacity >= capacity && buffer.TryClaim())
                    {
                        reusedBuffers.RemoveAt(i);
                        return buffer;
                    }
                }
            }
            return new BufferRequest<T>(capacity);
        }
        public static void Reuse(BufferRequest<T> buffer)
        {
            buffer.Free(true);
            if (reusedBuffers == null)
                reusedBuffers = new List<BufferRequest<T>>(4);
            reusedBuffers.Add(buffer);
        }
    }
    public class BufferRequest<T>
    {
        private T[] _array;
        public T[] Arr
        {
            get
            {
                if (isClaimed)
                    return _array;
                return null;
            }
        }
        public int bufferIndex;
        public int Capacity => _array.Length;
        //int capacity;
        //int requestedSize;
        //public int RequestedSize => requestedSize;

        public BufferRequest(int capacity)
        {
            _array = new T[capacity];
            //this.capacity = capacity;
        }

        bool isClaimed;
        public bool IsClaimed => isClaimed;

        public bool TryClaim()
        {
            if (!isClaimed)
            {
                isClaimed = true;
                return true;
            }
            return false;
        }

        public void Reuse()
        {
            BufferPool<T>.Reuse(this);
        }

        public void Free(bool clear = true)
        {
            isClaimed = false;
            if (clear)
                ClearArray();
        }

        public void ClearArray() => Array.Clear(_array, 0, _array.Length);
    }
        //static TreeNode<BufferRequest<T>>[] nodes;
        //static int beginIndex;
        //static int reusedBufferCount;

        /*static Node<BufferRequest<T>>[] reusedNodes;

        struct Node<TNode>
        {
            public int next;
            public int previous;
            public TNode value;
        }*/

        /*struct TreeNode<TNode>
        {
            public int left;
            public int right;
            public int parent;
            public TNode value;
        }*/

        //public static BufferRequest<T> Request(int capacity)
        //{
        //    int d = beginIndex;

        //    D:
        //    ref Node<BufferRequest<T>> node = ref reusedNodes[d];
        //    if(node.value.Capacity < capacity || !node.value.TryClaim()) // the current node is smaller than the capacity or somehow already claimed
        //    {
        //        if(node.next != 0) // has next
        //        {
        //            d = node.next;
        //            goto D; // redo 
        //        }
        //        // no next node, there's no buffer with that capacity
        //        node.next = d = GetIndexOrExpand();

        //        node = ref reusedNodes[d - 1];
        //        node.value = new BufferRequest<T>();// new buffer
        //    }
        //    // the current node has a bigger or equal capacity
        //    if(d > 1) // 
        //    reusedNodes[d - 1].next = node.next;
        //    BufferRequest<T> buffer = node.value;
        //    node = default; // delete
        //    return buffer;
        //}
        //public static void Reuse(BufferRequest<T> buffer)
        //{
        //    int d = 
        //}
        //private static int GetIndexOrExpand()
        //{
        //    for(int i = 0; i < reusedNodes.Length; i++)
        //    {
        //        if (reusedNodes[i].value == null)
        //            return i;
        //    }
        //    int length = reusedNodes.Length;
        //    Array.Resize(ref reusedNodes, reusedNodes.Length + 10);
        //    return length;
        //}
        //private static int GetAvaliableNodeIndex()
        //{
        //    for (int i = 0; i < reusedNodes.Length; i++)
        //    {
        //        if (reusedNodes[i].value == null)
        //            return i;
        //    }
        //    return -1;
        //}

        /*private static int GetAvaliableIndex()
        {
            for(int i = 0; i < nodes.Length; i++)
            {
                ref TreeNode<BufferRequest<T>> node = ref nodes[i];
                if(node.value == null)
                {
                    return i;
                }
            }
            return -1;
        }*/

        //private static void Add(int index, BufferRequest<T> request)
        //{
        /*int capacity = request.Capacity;

        ref TreeNode<BufferRequest<T>> node = ref nodes[beginIndex];
        int d = beginIndex;
        int previous = d;*/

        // nodes will go from left to right
        // smaller to highest
        // 
        //        100
        //
        //      
        //        200
        //       /
        //     100
        // if a new node with a higher capacity is inserted, that one will be the new root
        // if a node with a smaller capacity is inserted, it will try to go to the left
        //
        //       300
        //      /
        //    200
        //    /  
        //  100
        //
        //
        //      300
        //     /
        //   200
        //   / \
        // 100 150
        // 
        // if a node with a capacity smaller than one node but bigger than the one on the left
        // the node will be inserted on the right
        //
        //
        //     300
        //     /
        //   200
        //   / \
        // 150 175
        // /
        //100 
        // like the root, if a node is bigger than the 2 children but smaller than the parnent it 
        // will replace it, and the child node will go to the left

        /*A:

        if(node.value == null) // node is empty
        {
            node.value = request;
        }
        else if(node.value.Capacity < capacity) // current capacity is bigger
        {
            if(node.left == 0) // no left node
            {
                d = GetAvaliableIndex();
                node.left = d + 1;
            }

            node = ref nodes[node.left];
        }
        else
        {

        }*/
        //}
        /*private static bool TryFind(int capacity, out BufferRequest<T> request)
        {

        }*/
        /*private static void Insert(int index)
        {

        }*/

    /*
     * The purpose of this class is to provide a way to reuse objects like arrays
     * 
     * InstancePool will be a base for other InstancePool<T>s
     * each InstancePool<T> will have it's own implementation on how to get items
     * 
     * The pools will only hold items and try to get if avaliable
     * 
     */
    //class InstancePool
    //{

    //}
    //class InstancePool<T>
    //{
    //    List<IPoolObject<T>> objects;
    //    public IPoolObject<T> Get
    //}
    //static class InstancePools<TPool> where TPool : InstancePool
    //{
    //    private static Func<TPool> _factory;
    //    private static TPool _instance;
    //    public static TPool Instance
    //    {

    //    }
    //    static InstancePools()
    //    {
    //    }
    //    public static void SetPoolInstance(TPool instance)
    //    {
    //        _instance = instance;
    //    }
    //    public static void SetFactory(Func<TPool> factory)
    //    {
    //        _factory = factory;
    //    }
    //}
    //class PoolObject<T> : IPoolObject<T>
    //{
    //    protected T obj;
    //    public bool IsClaimed => claimed;
    //    protected bool claimed;

    //    public bool TryClaim()
    //    {
    //        if (!claimed)
    //        {
    //            claimed = true;
    //            return true;
    //        }
    //        return false;
    //    }
    //}
    //interface IPoolObject
    //{
    //    bool IsClaimed { get; }
    //    bool TryClaim();
    //    void Release();
    //    void Reuse();
    //}
    //interface IPoolObject<T> : IPoolObject
    //{
    //    bool TryGet(out T instance);
    //}

    ///// <summary>
    /////
    ///// </summary>
    //public class BufferPool
    //{
    //    public static event Action OnClear;
    //    /// <summary>
    //    /// Request a buffer with the given capacity
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="capacity">The capacity of the buffer</param>
    //    /// <returns></returns>
    //    public static BufferPoolBuffer<T> Request<T>(int capacity, bool clearBuffer = true)
    //    {
    //        return BufferPool<T>.Request(capacity);
    //    }
    //    /// <summary>
    //    /// Puts a buffer avaliable for reuse
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="buffer">The buffer</param>
    //    /// <param name="clear">If the buffer's array should be cleared before put for reuse</param>
    //    public static void Reuse<T>(BufferPoolBuffer<T> buffer, bool clear = true)
    //    {
    //        BufferPool<T>.Reuse(buffer, clear);
    //    }
    //    public static void ClearBuffers()
    //    {
    //        OnClear?.Invoke();
    //    }
    //}
    //public class BufferPoolBuffer<T> : IDisposable
    //{
    //    public readonly T[] array;
    //    public int Capacity => requestedCapacity;
    //    public bool Claimed => claimed;
    //    bool claimed;
    //    bool removedFinalizer;
    //    protected int requestedCapacity;

    //    public BufferPoolBuffer(int capacity) : this(new T[capacity])
    //    {

    //    }

    //    public BufferPoolBuffer(T[] array)
    //    {
    //        this.array = array;
    //        requestedCapacity = array.Length;
    //    }

    //    public T this[int index]
    //    {
    //        get
    //        {
    //            //if (index < 0 || index > capacity)
    //            //throw new IndexOutOfRangeException("Index");
    //            return array[index];
    //        }
    //        set
    //        {
    //            //if (index < 0 || index > capacity)
    //            //throw new IndexOutOfRangeException("Index");
    //            array[index] = value;
    //        }
    //    }

    //    public T Get(int index)
    //    {
    //        return array[index];
    //    }

    //    public void Set(int index, T value)
    //    {
    //        array[index] = value;
    //    }

    //    public ref T GetRef(int index)
    //    {
    //        return ref array[index];
    //    }

    //    public bool TryClaim(int requestedCapacity)
    //    {
    //        if (!Claimed)
    //        {
    //            this.requestedCapacity = requestedCapacity;
    //            //state |= BufferPoolBufferState.Claimed;
    //            //claimed = true;
    //            claimed = true;

    //            if (removedFinalizer)
    //                GC.ReRegisterForFinalize(this);

    //            return true;
    //        }
    //        return false;
    //    }

    //    public void Release(bool clear = true)
    //    {
    //        //claimed = false;
    //        //Claimed = false;
    //        claimed = false;
    //        if (clear)
    //            Array.Clear(array, 0, array.Length);
    //    }

    //    public void Reuse(bool clear = true)
    //    {
    //        BufferPool.Reuse(this, clear);
    //    }

    //    public void ClearArray()
    //    {
    //        Array.Clear(array, 0, array.Length);
    //    }

    //    public void Dispose()
    //    {
    //        Dispose(true);
    //        GC.SuppressFinalize(this);
    //        removedFinalizer = true;
    //    }

    //    protected virtual void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            BufferPool.Reuse(this);
    //        }
    //        Release(true);
    //    }

    //    ~BufferPoolBuffer() => Dispose(false);

    //    [Flags]
    //    public enum BufferPoolBufferState : byte
    //    {
    //        Avaliable = 0x0,
    //        Claimed = 0x1,
    //        Cleared = 0x2,
    //        Disposed = 0x4,

    //    }
    //}
    //public class BufferPool<T>
    //{
    //    //internal static WeakReference<BufferPoolBuffer<T>>[] buffers;
    //    //internal static WeakReference<T[]> arrays;
    //    static BufferPoolBuffer<T>[] reusedBuffers;
    //    static int reusedBufferCount;

    //    static BufferPool()
    //    {
    //        BufferPool.OnClear += Clear;
    //    }
    //    //private static int count;

    //    /*struct TreeNode<TNode>
    //    {
    //        public int left;
    //        public int right;
    //        public TNode value;
    //    }
    //    static TreeNode<BufferPoolBuffer<T>>[] nodes; */

    //    public static BufferPoolBuffer<T> Request(int capacity)
    //    {
    //        BufferPoolBuffer<T> buffer;

    //        /*int n = 1;

    //        ref TreeNode<BufferPoolBuffer<T>> node = ref nodes[n];
    //        if(node.value.Capacity < capacity)
    //        {
    //            if(nodes[n].left > 0)
    //            n = nodes[n].left;
    //        }
    //        else
    //        {
    //            n = nodes[n].right;
    //        }*/
    //        /*
    //         *       100
    //         *    
    //         *
    //         *       200
    //         *      /
    //         *    100
    //         *    
    //         *    
    //         *        300
    //         *       /   \
    //         *     100   200
    //         *    
    //         *
    //         */

    //        if (reusedBuffers != null)
    //        {
    //            for (int i = 0; i < reusedBufferCount; i++)
    //            {
    //                var reusedBuffer = reusedBuffers[i];
    //                if (reusedBuffer.Capacity >= capacity)
    //                {
    //                    if (!reusedBuffer.TryClaim())
    //                        continue;

    //                    reusedBuffers[i] = reusedBuffers[reusedBufferCount];
    //                    reusedBuffers[reusedBufferCount] = null;
    //                    reusedBufferCount--;

    //                    return reusedBuffer;
    //                }
    //            }
    //        }
    //        buffer = new BufferPoolBuffer<T>(capacity);
    //        buffer.IsInUse = true;
    //        return buffer;



    //        /*BufferPoolBuffer<T> buffer;
    //        if (buffers is null)
    //        {
    //            buffer = new BufferPoolBuffer<T>(capacity);
    //            buffers = new WeakReference<BufferPoolBuffer<T>>[] { new WeakReference<BufferPoolBuffer<T>>(buffer) };
    //            return buffer;
    //        }
    //        for (int i = 0; i < count; i++)
    //        {
    //            if (buffers[i].TryGetTarget(out buffer))
    //            {
    //                if (!buffer.IsInUse && buffer.Capacity >= capacity)
    //                {
    //                    buffer.IsInUse = true;
    //                    return buffer;
    //                }
    //            }
    //            else
    //            {
    //                Swap(ref buffers[count], ref buffers[i]);
    //                count--;
    //            }
    //        }
    //        buffer = new BufferPoolBuffer<T>(new T[capacity], capacity);
    //        EnsureCapacity(ref buffers, count + 1);
    //        if (buffers[count] is null)
    //            buffers[count] = new WeakReference<BufferPoolBuffer<T>>(buffer);
    //        else
    //            buffers[count].SetTarget(buffer);
    //        count++;*/
    //    }
    //    public static void Reuse(BufferPoolBuffer<T> buffer, bool clear = true)
    //    {
    //        buffer.Release(clear);

    //        EnsureCapacity(ref reusedBuffers, reusedBufferCount + 1);
    //        reusedBuffers[reusedBufferCount] = buffer;
    //        reusedBufferCount++;
    //    }
    //    public static void Clear()
    //    {
    //        reusedBufferCount = 0;
    //        if (reusedBuffers != null)
    //            Array.Clear(reusedBuffers, 0, reusedBuffers.Length);
    //        reusedBuffers = null;
    //    }
    //    /*private static void Swap<TArr>(ref TArr A, ref TArr B)
    //    {
    //        TArr temp = A;
    //        B = A;
    //        A = temp;
    //    }*/
    //    private static void EnsureCapacity<TArr>(ref TArr[] array, int capacity)
    //    {
    //        if (array is null)
    //        {
    //            array = new TArr[capacity];
    //        }
    //        else if (array.Length < capacity)
    //        {
    //            if (array.Length * 2 < capacity)
    //                Array.Resize(ref array, capacity);
    //            else
    //                Array.Resize(ref array, array.Length * 2);
    //        }
    //    }
    //}
}
