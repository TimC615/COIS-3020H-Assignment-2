using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment2_Q2_Ternary
{
    public interface IContainer<T>
    {
        void MakeEmpty();
        bool Empty();
        int Size();
    }

    //-------------------------------------------------------------------------

    public interface ITrie<T> : IContainer<T>
    {
        bool Insert(string key, T value);
        T Value(string key);
    }

    //-------------------------------------------------------------------------

    class Trie<T> : ITrie<T>
    {
        private Node root;                 // Root node of the Trie
        private int size;                  // Number of values in the Trie

        class Node
        {
            public char ch;                // Character of the key
            public T value;                // Value at Node; otherwise default
            public Node low, middle, high; // Left, middle, and right subtrees

            // Node
            // Creates an empty Node
            // All children are set to null
            // Time complexity:  O(1)

            public Node(char ch)
            {
                this.ch = ch;
                value = default(T);
                low = middle = high = null;
            }
        }

        // Trie
        // Creates an empty Trie
        // Time complexity:  O(1)

        public Trie()
        {
            MakeEmpty();
            size = 0;
        }

        // Public Insert
        // Calls the private Insert which carries out the actual insertion
        // Returns true if successful; false otherwise

        public bool Insert(string key, T value)
        {
            return Insert(ref root, key, 0, value);
        }

        // Private Insert
        // Inserts the key/value pair into the Trie
        // Returns true if the insertion was successful; false otherwise
        // Note: Duplicate keys are ignored

        private bool Insert(ref Node p, string key, int i, T value)
        {
            if (p == null)
                p = new Node(key[i]);

            // Current character of key inserted in left subtree
            if (key[i] < p.ch)
                return Insert(ref p.low, key, i, value);

            // Current character of key inserted in right subtree
            else if (key[i] > p.ch)
                return Insert(ref p.high, key, i, value);

            else if (i + 1 == key.Length)
            // Key found
            {
                // But key/value pair already exists
                if (!p.value.Equals(default(T)))
                    return false;
                else
                {
                    // Place value in node
                    p.value = value;
                    size++;
                    return true;
                }
            }

            else
                // Next character of key inserted in middle subtree
                return Insert(ref p.middle, key, i + 1, value);
        }

        // Value
        // Returns the value associated with a key; otherwise default

        public T Value(string key)
        {
            int i = 0;
            Node p = root;

            while (p != null)
            {
                // Search for current character of the key in left subtree
                if (key[i] < p.ch)
                    p = p.low;

                // Search for current character of the key in right subtree           
                else if (key[i] > p.ch)
                    p = p.high;

                else // if (p.ch == key[i])
                {
                    // Return the value if all characters of the key have been visited 
                    if (++i == key.Length)
                        return p.value;

                    // Move to next character of the key in the middle subtree   
                    p = p.middle;
                }
            }
            return default(T);   // Key too long
        }

        // Contains
        // Returns true if the given key is found in the Trie; false otherwise

        public bool Contains(string key)
        {
            int i = 0;
            Node p = root;

            while (p != null)
            {
                // Search for current character of the key in left subtree
                if (key[i] < p.ch)
                    p = p.low;

                // Search for current character of the key in right subtree           
                else if (key[i] > p.ch)
                    p = p.high;

                else // if (p.ch == key[i])
                {
                    // Return true if the key is associated with a non-default value; false otherwise 
                    if (++i == key.Length)
                        return !p.value.Equals(default(T));

                    // Move to next character of the key in the middle subtree   
                    p = p.middle;
                }
            }
            return false;        // Key too long
        }

        // MakeEmpty
        // Creates an empty Trie
        // Time complexity:  O(1)

        public void MakeEmpty()
        {
            root = null;
        }

        // Empty
        // Returns true if the Trie is empty; false otherwise
        // Time complexity:  O(1)

        public bool Empty()
        {
            return root == null;
        }

        // Size
        // Returns the number of Trie values
        // Time complexity:  O(1)

        public int Size()
        {
            return size;
        }

        // Public Print
        // Calls private Print to carry out the actual printing

        public void Print()
        {
            Print(root, "");
        }

        // Private Print
        // Outputs the key/value pairs ordered by keys 

        private void Print(Node p, string key)
        {
            if (p != null)
            {
                Print(p.low, key);
                if (!p.value.Equals(default(T)))
                    Console.WriteLine(key + p.ch + " " + p.value);
                Print(p.middle, key + p.ch);
                Print(p.high, key);
            }
        }

        public List<string> PartialMatch(string pattern)
        {
            var partMatch = new List<string>();
            
            //creates an instance of the print method to loop through the entirity of the user's input key
            PartMatchPrint(root, pattern, null, 0);

            return partMatch;
        }

        private void PartMatchPrint(Node p, string key, string output, int x)
        {
            if (p != null)
            {
                //checks if the entirity of the user input has been read and outputs a valid key
                if (x == key.Length)
                {
                    if (p.value.Equals(default(T)))
                    {
                        Console.WriteLine(output + " " + p.value);
                    }
                    return;
                }
                else
                {
                    //if a '.' is found, create an instance of the print method for all 3 options for traversing the tree
                    if (key[x].Equals('.'))
                    {
                        PartMatchPrint(p.middle, key, output + p.ch, x + 1);
                        PartMatchPrint(p.low, key, output, x);
                        PartMatchPrint(p.high, key, output, x);
                    }
                    //this else statement is triggered if the current key value is not a '.'
                    else
                    {
                        //if the current key matches the value of the current node, go to the middle node
                        if (p.ch.Equals(key[x]))
                        {
                            PartMatchPrint(p.middle, key, output + p.ch, x + 1);
                        }
                        else
                        {
                            //if the current key is greater than the node character, go to the high node
                            if (key[x] > p.ch)
                            {
                                PartMatchPrint(p.high, key, output, x);
                            }
                            else
                            {
                                //defaults to travelling to the low node of no other if statement is triggered
                                PartMatchPrint(p.low, key, output, x);
                            }
                        }
                    }
                }
            }
        }

        public List<string> Autocomplete(string prefix)
        {
            var auto = new List<string>();
            Node p = root;
            int x = 0;
            while (p != null)
            {
                // Search for current character of the key in left subtree
                if (prefix[x] < p.ch)
                    p = p.low;

                // Search for current character of the key in right subtree           
                else if (prefix[x] > p.ch)
                    p = p.high;

                else // if (p.ch == key[i])
                {
                    // Return the value if all characters of the key have been visited 
                    if (++x == prefix.Length)
                    {
                        if (p.middle == null || !p.value.Equals(default(T)))
                            Console.WriteLine(p.value);
                        Print(p.middle, prefix);
                        return auto;
                    }

                    // Move to next character of the key in the middle subtree   
                    p = p.middle;
                }
            }
            return auto;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Trie<int> T;
            T = new Trie<int>();
            List<string> L;

            T.Insert("bag", 10);
            T.Insert("bat", 20);
            T.Insert("cab", 70);
            T.Insert("bagel", 30);
            T.Insert("beet", 40);
            T.Insert("abc", 60);

            T.Print();

            Console.WriteLine("\n\nEnter a pattern to return all matching keys.\nA key consists of letters and periods where letters have to match and periods are wildcards.");
            Console.Write("User Input: ");
            string partial = Console.ReadLine();

            List<string> partialAnswer = new List<string>();
            partialAnswer = T.PartialMatch(partial);
            foreach (var key in partialAnswer)
                Console.WriteLine(key);


            Console.WriteLine("\n\nEnter the first section of a key that will be autocompleted.");
            Console.Write("User Input: ");
            string autoInput = Console.ReadLine();

            List<string> auto = new List<string>();
            auto = T.Autocomplete(autoInput);
            foreach (var key in auto)
                Console.WriteLine(key);

            Console.ReadKey();
        }
    }
}
