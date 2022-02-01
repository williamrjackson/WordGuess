using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Wrj
{
    public class WordList : MonoBehaviour
    {
        private HashSet<string> wordSet;
        private HashSet<string> commonSet;

        private static WordList _instance;
        public static WordList Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject();
                    go.name = "WordList";
                    _instance = go.AddComponent<WordList>();
                }
                return _instance;
            }
        }
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                Init();
            }
        }

        public void Init()
        {
            TextAsset wordListTextAsset = Resources.Load("5-letter-words", typeof(TextAsset)) as TextAsset;
            TextAsset commonTextAsset = Resources.Load("common", typeof(TextAsset)) as TextAsset;
            wordSet = new HashSet<string>(wordListTextAsset.text.Split(","[0]));
            commonSet = new HashSet<string>(commonTextAsset.text.Split("\n"[0]));
            Debug.Log($"Word list loaded: \n\tDictionary of {string.Format("{0:n0}", wordSet.Count)} words.");
        }

        public static bool CheckWord(string word)
        {
            return Instance.wordSet.Contains(word.ToLower());
        }
        public static string RandomWord()
        {
            List<string> commonList = Instance.commonSet.ToList();
            return commonList[UnityEngine.Random.Range(0, Instance.commonSet.Count)];
        }
        public static string WordOfTheDay()
        {
            List<string> words = Instance.commonSet.ToList();
            DateTime today = DateTime.UtcNow.Date;
            Int64 todayInt = today.ToBinary();
            Int64 wordIndex = todayInt % (Int64)words.Count;
            int nWordIndex = Mathf.Abs((int)wordIndex);
            string wordOtD = words[(int)nWordIndex];
            return wordOtD;
        }
        public static List<string> GetPossibleWords(string chars, int minLength = 3)
        {
            chars = chars.ToLower();
            List<string> combinations = CharCombinations(chars.ToLower());
            List<string> results = new List<string>();
            foreach (string item in combinations)
            {
                if (item.Length >= minLength && CheckWord(item))
                {
                    results.Add(item.ToUpper());
                }
            }
            return results;
        }
        public static List<string> GetRandomWords(int count, int minLength = 3, int maxLength = 7)
        {
            List<string> wordSetList = Instance.wordSet.ToList();
            List<string> results = new List<string>();

            for (int i = 0; i < count; i++)
            {
                string word = "";
                while (word.Length > maxLength || word.Length < minLength || results.Contains(word))
                {
                    word = wordSetList[UnityEngine.Random.Range(0, wordSetList.Count)];
                }
                results.Add(word);
            }
            return results;
        }
        /// http://stackoverflow.com/questions/7802822/all-possible-combinations-of-a-list-of-values
        public static List<string> CharCombinations(char[] inputCharArray, int minimumItems = 1,
                                                        int maximumItems = int.MaxValue)
        {
            int nonEmptyCombinations = (int)Mathf.Pow(2, inputCharArray.Length) - 1;
            List<string> listOfLists = new List<string>(nonEmptyCombinations + 1);

            // Optimize generation of empty combination, if empty combination is wanted
            if (minimumItems == 0)
                listOfLists.Add("");

            if (minimumItems <= 1 && maximumItems >= inputCharArray.Length)
            {
                // Simple case, generate all possible non-empty combinations
                for (int bitPattern = 1; bitPattern <= nonEmptyCombinations; bitPattern++)
                    listOfLists.Add(GenerateCombination(inputCharArray, bitPattern));
            }
            else
            {
                // Not-so-simple case, avoid generating the unwanted combinations
                for (int bitPattern = 1; bitPattern <= nonEmptyCombinations; bitPattern++)
                {
                    int bitCount = CountBits(bitPattern);
                    if (bitCount >= minimumItems && bitCount <= maximumItems)
                        listOfLists.Add(GenerateCombination(inputCharArray, bitPattern));
                }
            }

            return listOfLists;
        }
        public static List<string> CharCombinations(string input, int minItems = 1, int maxItems = int.MaxValue)
        {
            return CharCombinations(input.ToCharArray(), minItems, maxItems);
        }

        private static string GenerateCombination(char[] inputList, int bitPattern)
        {
            string thisCombination = string.Empty;// new List<T>(inputList.Length);
            for (int j = 0; j < inputList.Length; j++)
            {
                if ((bitPattern >> j & 1) == 1)
                    thisCombination += inputList[j];
            }
            return thisCombination;
        }

        /// <summary>
        /// Sub-method of CharCombinations() method to count the bits in a bit pattern. Based on this:
        /// https://graphics.stanford.edu/~seander/bithacks.html#CountBitsSetKernighan
        /// </summary>
        private static int CountBits(int bitPattern)
        {
            int numberBits = 0;
            while (bitPattern != 0)
            {
                numberBits++;
                bitPattern &= bitPattern - 1;
            }
            return numberBits;
        }
    }
}
