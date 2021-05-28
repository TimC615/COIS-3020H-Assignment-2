using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment1_Q1_RWay
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
        bool Remove(string key);
        T Value(string key);
    }

    //-------------------------------------------------------------------------

    class Trie<T> : ITrie<T>
    {
        private Node root;          // Root node of the Trie

        class Node
        {
            public T value;         // Value at Node; otherwise default
            public int numValues;   // Number of descendent values of a Node 
            public Node[] child;    // Branching for each letter 'a' .. 'z'

            // Node
            // Creates an empty Node
            // All children are set to null by default
            // Time complexity:  O(1)

            public Node()
            {
                value = default(T);
                numValues = 0;
                child = new Node[26];
            }
        }

        // Trie
        // Creates an empty Trie
        // Time complexity:  O(1)

        public Trie()
        {
            MakeEmpty();
        }

        // Public Insert
        // Calls the private Insert which carries out the actual insertion
        // Returns true if successful; false otherwise

        public bool Insert(string key, T value)
        {
            return Insert(root, key, 0, value);
        }

        // Private Insert
        // Inserts the key/value pair into the Trie
        // Returns true if the insertion was successful; false otherwise
        // Note: Duplicate keys are ignored
        // Time complexity:  O(L) where L is the length of the key

        private bool Insert(Node p, string key, int j, T value)
        {
            int i;

            if (j == key.Length)
            {
                if (p.value.Equals(default(T)))
                {
                    // Sets the value at the Node
                    p.value = value;
                    p.numValues++;
                    return true;
                }
                // Duplicate keys are ignored (unsuccessful insertion)
                else
                    return false;
            }
            else
            {
                // Maps a character to an index
                i = Char.ToLower(key[j]) - 'a';

                // Creates a new Node if the link is null
                // Note: Node is initialized to the default value
                if (p.child[i] == null)
                    p.child[i] = new Node();

                // If the inseration is successful
                if (Insert(p.child[i], key, j + 1, value))
                {
                    // Increase number of descendent values by one
                    p.numValues++;
                    return true;
                }
                else
                    return false;
            }
        }

        // Value
        // Returns the value associated with a key; otherwise default
        // Time complexity:  O(L) where L is the length of the key

        public T Value(string key)
        {
            int i;
            Node p = root;

            // Traverses the links character by character
            foreach (char ch in key)
            {
                i = Char.ToLower(ch) - 'a';
                if (p.child[i] == null)
                    return default(T);    // Key is too long
                else
                    p = p.child[i];
            }
            return p.value;               // Returns the value or default
        }

        // Public Remove
        // Calls the private Remove that carries out the actual deletion
        // Returns true if successful; false otherwise

        public bool Remove(string key)
        {
            return Remove(root, key, 0);
        }

        // Private Remove
        // Removes the value associated with the given key
        // Time complexity:  O(L) where L is the length of the key

        private bool Remove(Node p, string key, int j)
        {
            int i;

            // Key not found
            if (p == null)
                return false;

            else if (j == key.Length)
            {
                // Key/value pair found
                if (!p.value.Equals(default(T)))
                {
                    p.value = default(T);
                    p.numValues--;
                    return true;
                }
                // No value with associated key
                else
                    return false;
            }

            else
            {
                i = Char.ToLower(key[j]) - 'a';

                // If the deletion is successful
                if (Remove(p.child[i], key, j + 1))
                {
                    // Decrease number of descendent values by one and
                    // Remove Nodes with no remaining descendents
                    if (p.child[i].numValues == 0)
                        p.child[i] = null;
                    p.numValues--;
                    return true;
                }
                else
                    return false;
            }
        }

        // MakeEmpty
        // Creates an empty Trie
        // Time complexity:  O(1)

        public void MakeEmpty()
        {
            root = new Node();
        }

        // Empty
        // Returns true if the Trie is empty; false otherwise
        // Time complexity:  O(1)

        public bool Empty()
        {
            return root.numValues == 0;
        }

        // Size
        // Returns the number of Trie values
        // Time complexity:  O(1)

        public int Size()
        {
            return root.numValues;
        }

        // Public Print
        // Calls private Print to carry out the actual printing

        public void Print()
        {
            Print(root, "");
        }

        // Private Print
        // Outputs the key/value pairs ordered by keys
        // Time complexity:  O(S) where S is the total length of the keys

        private void Print(Node p, string key)
        {
            int i;

            if (p != null)
            {
                //if (!p.value.Equals(default(T)))
                Console.WriteLine(key + " " + p.value + " " + p.numValues);
                for (i = 0; i < 26; i++)
                    Print(p.child[i], key + (char)(i + 'a'));
            }
        }

        public List<string> PartialMatch(string pattern)
        {
            var partMatch = new List<string>();
            //starts part match search at the root
            PartMatchPrint(root, pattern, null, 0);

            return partMatch;
        }

        private void PartMatchPrint(Node p, string key, string output, int x)
        {
            //used to add letters to the output string
            const string letters = "abcdefghijklmnopqrstuvwxyz";
            if (p != null)
            {
                //checks if the total input string has been read
                if (x == key.Length)
                    Console.WriteLine(output + " " + p.value + " " + p.numValues);
                else
                {
                    //if current letter is a period, recursively connect to a print method for each letter of the alphabet
                    if (key[x].Equals('.'))
                    {
                        for (int i = 0; i < 26; i++)
                            PartMatchPrint(p.child[i], key, output + (char)(i + 'a'), x + 1);
                    }
                    else
                    {
                        //converts the current ketter in the key to a number
                        int letterSelect = Char.ToLower(key[x]) - 'a';

                        //sends the node related to the current letter in the key to a new instance of the print method
                        PartMatchPrint(p.child[letterSelect], key, output + letters[letterSelect], x + 1);
                    }
                }
            }
        }

        public List<string> Autocomplete(string prefix)
        {
            var auto = new List<string>();
            Node p = root;
            int x;

            //creates a starting point to print off of
            foreach (char letter in prefix)
            {
                x = Char.ToLower(letter) - 'a';
                if (p.child[x] == null)
                {
                    auto.Add("The key supplied is too long");    // Key is too long
                    return auto;
                }
                else
                    p = p.child[x];
            }

            //creates an instance of the print method for each letter of the alphabet starting at the node related
            //to the partial key the user has inputted
            if (p != null)
            {
                Console.WriteLine(prefix + " " + p.value + " " + p.numValues);
                for (int i = 0; i < 26; i++)
                    Print(p.child[i], prefix + (char)(i + 'a'));
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

            Console.WriteLine("\nInsertions for Tree");
            T.Insert("Brian", 10);
            T.Insert("Brianna", 20);
            T.Insert("Yvonne", 70);
            T.Insert("Adrian", 30);
            T.Insert("Adam", 40);
            T.Insert("B", 50);
            T.Insert("George", 60);
            Console.WriteLine(T.Size());

            Console.WriteLine("\nDeletions for Tree");
            T.Remove("B");
            T.Remove("Bill");
            T.Remove("Brian");
            T.Remove("Adam");
            Console.WriteLine(T.Size());

            Console.WriteLine("\nPrint Statement");
            T.Print();

            Console.WriteLine("\n\n\nEnter a pattern to return all matching keys.\nA key consists of letters and periods where letters have to match and periods are wildcards.");
            Console.Write("User Input: ");
            string partial = Console.ReadLine();
            T.PartialMatch(partial);

            Console.WriteLine("\n\n\nEnter the first section of a key that will be autocompleted.");
            Console.Write("User Input: ");
            string autoInput = Console.ReadLine();
            T.Autocomplete(autoInput);

            Console.ReadKey();
        }
    }
}