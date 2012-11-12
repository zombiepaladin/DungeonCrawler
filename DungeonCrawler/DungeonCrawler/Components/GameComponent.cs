#region File Description
//-----------------------------------------------------------------------------
// GameComponent.cs
//
// Author: Nathan Bean
// Modified : Joshua Zavala
//            BinaryNode & BinarySearchTree - Assignment 6 & 7
// Modified: Nick Boen - Added call to get Key list, Sam Fike implemented this first
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
//Samuel Fike and Jiri Malina: Created a virtual HandleTrigger method
#endregion

#region Using Statements
using System.Collections.Generic;
using System;
using System.Collections; //to use ArrayList
using System.Linq; //to Cast ArrayList
#endregion

namespace DungeonCrawler.Components
{
    #region DictionaryImplementation
    
    /// <summary>
    /// A generic data structure for holding Structs 
    /// representing a single aspect of an Entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GameComponent<T>
    {
        #region Private and Protected Variables

        /// <summary>
        /// A Dictionary of T, keyed by EntityIDs
        /// Using a Dictionary gives us O(1) random
        /// access, and we can supply an interator
        /// from the value array
        /// </summary>
        protected Dictionary<uint, T> elements;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new generic GameComponent
        /// </summary>
        public GameComponent()
        {
            elements = new Dictionary<uint, T>();
        }

        /// <summary>
        /// Creates a new generic GameComponent with initial capacity n
        /// </summary>
        /// <param name="initialCapacity">The initial capacity</param>
        public GameComponent(int initialCapacity)
        {
            elements = new Dictionary<uint, T>(initialCapacity);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the component for the specified entity
        /// </summary>
        /// <param name="entityID">The entity ID</param>
        /// <returns>The component</returns>
        public T this[uint entityID]
        {
            set { elements[entityID] = value; }
            get { return elements[entityID]; }
        }


        /// <summary>
        /// Gets all components of this type in the game
        /// </summary>
        public IEnumerable<T> All
        {
            get { return elements.Values; }
        }

        public IEnumerable<uint> Keys
        {
            get { return elements.Keys; }
        }

        /// <summary>
        /// Adds the supplied component to the specified entity
        /// </summary>
        /// <param name="entityID">The entity ID</param>
        /// <param name="component">The component</param>
        public void Add(uint entityID, T component)
        {
            elements.Add(entityID, component);
        }


        /// <summary>
        /// Removes the component from the specified entity
        /// </summary>
        /// <param name="entityID">The entity ID</param>
        public void Remove(uint entityID)
        {
            elements.Remove(entityID);
        }


        /// <summary>
        /// Returns if the specified entity has the component
        /// </summary>
        /// <param name="entityID">The entity ID</param>
        /// <returns>
        /// true if the entity possesses the component,
        /// false otherwise
        /// </returns>
        public bool Contains(uint entityID)
        {
            return elements.ContainsKey(entityID);
        }

        public virtual void HandleTrigger(uint entityID, string type)
        {
            throw new Exception("Handle Trigger is not implemented");
        }
        #endregion
    }
    
    #endregion

    #region BinaryTreeArrayImplementation
    /*
    /// <summary>
    /// A generic data structure for holding Structs 
    /// representing a single aspect of an Entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GameComponent<T>
    {
        #region Private and Protected Variables

        /// <summary>
        /// A Dictionary of T, keyed by EntityIDs
        /// Using a Dictionary gives us O(1) random
        /// access, and we can supply an interator
        /// from the value array
        /// </summary>
        //protected Dictionary<uint, T> elements;

        /// <summary>
        /// Uses a hybrid Binary Search Tree and 
        /// ArrayList combination to perform the
        /// same operations of the previously used
        /// Dictionary. The advantage is able to
        /// maintain a fast O(log n) random access 
        /// and has a higher degree of caching and
        /// locality due to the data structure.
        /// </summary>
        protected BinarySearchTree elements;
        protected List<T> arrayList;
        private int index;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new generic GameComponent
        /// </summary>
        public GameComponent()
        {
            //elements = new Dictionary<uint, T>();
            elements = new BinarySearchTree();
            arrayList = new List<T>();
            index = -1;
        }

        /// <summary>
        /// Creates a new generic GameComponent with initial capacity n
        /// </summary>
        /// <param name="initialCapacity">The initial capacity</param>
        public GameComponent(int initialCapacity)
        {
            //elements = new Dictionary<uint, T>(initialCapacity);

            elements = new BinarySearchTree();
            arrayList = new List<T>(initialCapacity);
            index = -1;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets/sets the component for the specified entity.
        /// </summary>
        /// <param name="entityID">The entity ID</param>
        /// <returns>The component</returns>
        public T this[uint entityID]
        {
            //set { elements[entityID] = value; }
            //get{ return elements[entityID]; }
            get
            {
                if (index == -1) return default(T);
                else
                {
                    //index = elements.Find(entityID);
                    return arrayList.ElementAt(elements.Find(entityID));
                }
            }
            set
            {
                //should be saved to a temp int?
                int temp = elements.Find(entityID);
                if (temp == -1)
                    this.Add(entityID, value);
                else
                    arrayList[temp] = value;
            }
        }


        /// <summary>
        /// Gets all components of this type in the game
        /// </summary>
        public IEnumerable<T> All
        {
            //get { return elements.Values; }
            get 
            {
                return arrayList;
            }
        }

        /// <summary>
        /// Adds the supplied component to the specified entity
        /// </summary>
        /// <param name="entityID">The entity ID</param>
        /// <param name="component">The component</param>
        public void Add(uint entityID, T component)
        {
            //elements.Add(entityID, component);
            index++;
            arrayList.Add(component);
            //arrayList[index] = component;
            elements.Insert(entityID, index);
        }


        /// <summary>
        /// Removes the component from the specified entity
        /// </summary>
        /// <param name="entityID">The entity ID</param>
        public void Remove(uint entityID)
        {
            //elements.Remove(entityID);
            int temp = elements.Find(entityID);
            arrayList[temp] = arrayList[index];
            arrayList.RemoveAt(index);
            index--;
            elements.Remove(entityID);
        }


        /// <summary>
        /// Returns if the specified entity has the component
        /// </summary>
        /// <param name="entityID">The entity ID</param>
        /// <returns>
        /// true if the entity possesses the component,
        /// false otherwise
        /// </returns>
        public bool Contains(uint entityID)
        {
            //return elements.ContainsKey(entityID);
            return elements.Contains(entityID);
        }

        public virtual void HandleTrigger(uint entityID, string type)
        {
            throw new Exception("Handle Trigger is not implemented");
        }
        #endregion
    }
    */
    #endregion

    /// <summary>
    /// Author: Joshua Zavala
    ///         BinaryNode - Assignment 6 & 7
    /// 
    /// This class contains the BinaryNode definitions. The root is
    /// an unit element and contains a int for location indexing, a
    /// left BinaryNode, and a right BinaryNode.
    /// 
    /// This data structure is a modified interpretation 
    /// from the book "Data Structures & Algorithm Analysis in JAVA" 
    /// by Mark Allen Weiss
    ///
    /// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
    /// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
    /// Released under the Microsoft Permissive Licence
    /// </summary>
    public class BinaryNode
    {
        #region variable declarations
        /// <summary>
        /// Root data for the node.
        /// </summary>
        uint element;
        /// <summary>
        /// Reference to an index in ArrayList.
        /// </summary>
        int loc;
        /// <summary>
        /// Declaration for a left child.
        /// </summary>
        private BinaryNode left;
        /// <summary>
        /// Declaration for a right child.
        /// </summary>
        private BinaryNode right;
        #endregion

        #region constructors
        /// <summary>
        /// Creates a single BinaryNode with null children.
        /// </summary>
        /// <param name="element">Data for the root.</param>
        /// <param name="loc">Reference location in ArrayList.</param>
        public BinaryNode(uint element, int loc)
        {
            this.element = element;
            this.loc = loc;
            this.left = null;
            this.right = null;
        }

        /// <summary>
        /// Creates a single BinaryNode with defined children.
        /// </summary>
        /// <param name="element">Data for the root.</param>
        /// <param name="loc">Reference location in ArrayList</param>
        /// <param name="left">Left child node.</param>
        /// <param name="right">Right child node.</param>
        public BinaryNode(uint element, int loc, BinaryNode left, BinaryNode right)
        {
            this.element = element;
            this.loc = loc;
            this.left = left;
            this.right = right;
        }
        #endregion

        #region Getters/Setters
        /// <summary>
        /// All getters/setters contain the
        /// same functionality: allow a variable
        /// to be retrieved or re-defined.
        /// </summary>
        public uint Element
        {
            get { return element; }
            set { element = value; }
        }
        public int Location
        {
            get { return loc; }
            set { loc = value; }
        }
        public BinaryNode Left
        {
            get { return left; }
            set { left = value; }
        }
        public BinaryNode Right
        {
            get { return right; }
            set { right = value; }
        }
        #endregion
    }

    /// <summary>
    /// Author: Joshua Zavala
    ///         BinaryNode - Assignment 6 & 7
    /// 
    /// This class contains the BinarySearchTree definitions. 
    /// The root is a single BinaryNode
    /// 
    /// This data structure is a modified interpretation 
    /// from the book "Data Structures & Algorithm Analysis in JAVA" 
    /// by Mark Allen Weiss
    ///
    /// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
    /// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
    /// Released under the Microsoft Permissive Licence
    /// </summary>
    public class BinarySearchTree
    {
        #region variables
        /// <summary>
        /// Declaration of a single BinaryNode
        /// called root. All others are built from
        /// this root.
        /// </summary>
        private BinaryNode root;
        #endregion

        #region constructors
        /// <summary>
        /// Default constructor for a BinarySearchTree (BST)
        /// The tree is default to null.
        /// </summary>
        public BinarySearchTree() { this.root = null; }
        #endregion

        #region public methods
        /// <summary>
        /// Sets this.root node to null.
        /// </summary>
        public void MakeEmpty() { this.root = null; }
        /// <summary>
        /// A check to see if this root node is empty.
        /// </summary>
        /// <returns>A bool: true is empty, false is not empty.</returns>
        public bool IsEmpty()
        {
            return this.root == null ? true : false;
        }
        /// <summary>
        /// Searches BST to determine if a particular
        /// id exists within the BST.
        /// </summary>
        /// <param name="id">Id to search for.</param>
        /// <returns>True if found; false otherwise.</returns>
        public bool Contains(uint id)
        {
            if (Find(id) != -1) return true;
            else return false;
        }
        /// <summary>
        /// Searches for a particular element inside the BST.
        /// </summary>
        /// <param name="id">The id to search for.</param>
        /// <returns>The id of the found element.</returns>
        public int Find(uint id)
        {
            return ElementAt(Find(id, root));
        }
        /// <summary>
        /// Inserts a new element into the BST.
        /// </summary>
        /// <param name="id">The id to be inserted.</param>
        public void Insert(uint id, int loc)
        {
            root = Insert(id, loc, root);
        }
        /// <summary>
        /// Searches for and removes the node of a given id.
        /// </summary>
        /// <param name="id">The id to be removed.</param>
        public void Remove(uint id)
        {
            root = Remove(id, root);
        }
        /// <summary>
        /// NOT USED: Displays all the BST values.
        /// (Contains code for a console output)
        /// </summary>
        public void PrintTree()
        {
            if (IsEmpty()) Console.WriteLine("Empty tree.");
            else PrintTree(root);
        }
        #endregion

        #region private methods
        /// <summary>
        /// Retrieves an ArrayList index from a BinaryNode.
        /// </summary>
        /// <param name="t">BinaryNode representing the element to check.</param>
        /// <returns>Returns an integer if exists, -1 otherwise.</returns>
        private int ElementAt(BinaryNode t)
        {
            return t == null ? -1 : t.Location;
        }
        /// <summary>
        /// Recursively searches the BST for a requested id.
        /// </summary>
        /// <param name="id">The id to find in the BST.</param>
        /// <param name="t">The node to be recursively tested.</param>
        /// <returns>Returns a null BinaryNode if empty; non-null
        /// BinaryNode otherwise.</returns>
        private BinaryNode Find(uint id, BinaryNode t)
        {
            if (t == null) return null;
            if (id < t.Element) return Find(id, t.Left);
            else if (id > t.Element) return Find(id, t.Right);
            else return t;
        }
        /// <summary>
        /// NOT USED: Returns the largest node in the tree.
        /// </summary>
        /// <param name="t">Node to test.</param>
        /// <returns>Largest node in the tree.</returns>
        private BinaryNode FindMax(BinaryNode t)
        {
            if (t != null)
            {
                while (t.Right != null) { t = t.Right; }
            }
            return t;
        }
        /// <summary>
        /// Recursively searches for the smallest node
        /// in the BST.
        /// </summary>
        /// <param name="t">Node to be tested.</param>
        /// <returns>Returns smallest node in BST.</returns>
        private BinaryNode FindMin(BinaryNode t)
        {
            if (t == null) return null;
            else if (t.Left == null) return t;
            return FindMin(t.Left);
        }
        /// <summary>
        /// Recursively calls and inserts a new
        /// BinaryNode into the BST.
        /// </summary>
        /// <param name="id">BinaryNode root data.</param>
        /// <param name="loc">Reference to an ArrayList index.</param>
        /// <param name="t">Current node to be checked for nullity.</param>
        /// <returns>Returns a new BinaryNode with the root data, reference
        /// to an ArrayList index, and null children.</returns>
        private BinaryNode Insert(uint id, int loc, BinaryNode t)
        {
            if (t == null) t = new BinaryNode(id, loc, null, null);
            else if (id < t.Element)
            {
                t.Left = Insert(id, loc, t.Left);
            }
            else if (id > t.Element)
            {
                t.Right = Insert(id, loc, t.Right);
            }
            return t;
        }
        /// <summary>
        /// Recursively searches for and removes an
        /// BinaryNode from the BST.
        /// </summary>
        /// <param name="id">Id of the element to be removed.</param>
        /// <param name="t">BinaryNode to test.</param>
        /// <returns>BinaryNode to be removed.</returns>
        private BinaryNode Remove(uint id, BinaryNode t)
        {
            if (t == null) return t; //not found
            if (id < t.Element) t.Left = Remove(id, t.Left); //recursive left
            else if (id > t.Element) t.Right = Remove(id, t.Right); //recursive right
            else if (t.Left != null && t.Right != null)
            {
                t.Element = FindMin(t.Right).Element;
                t.Right = Remove(id, t.Right);
            }
            else t = (t.Left != null) ? t.Left : t.Right;
            return t;
        }
        /// <summary>
        /// NOT USED
        /// Recursively prints out each BinaryNode's element data
        /// in the BST, in order.
        /// </summary>
        /// <param name="t">Node to print.</param>
        private void PrintTree(BinaryNode t)
        {
            if (t != null)
            {
                PrintTree(t.Left);
                Console.WriteLine(t.Element);
                PrintTree(t.Right);
            }
        }
        #endregion
    }

}