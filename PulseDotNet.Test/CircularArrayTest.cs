using System;
using System.Collections.Generic;
using PulseDotNet.Collection;
using Xunit;

namespace PulseDotNet.Test
{
    public class CircularArrayTest
    {
        [Fact]
        public void FillElements()
        {
            var compareArray = new []{1, 2, 3, 4, 5};
            var array = new CircularArray<int>(5);
            Fill(array, 5);
            // elements contain numbers 1 through 5
            TestRead(array, compareArray);
        }

        [Fact]
        public void OverfillElements()
        {
            var compareArray = new []{3, 4, 5, 6, 7};
            var array = new CircularArray<int>(5);
            Fill(array, 7);
            // 1 and 2 have been evicted, 3 is the starting number
            TestRead(array, compareArray);
        }

        [Fact]
        public void RemoveInt()
        {
            var compareArray = new []{1, 2, 4, 5, 0};
            var array = new CircularArray<int>(5);
            Fill(array, 5);
            array.Remove(3);
            TestRead(array, compareArray);
        }

        [Fact]
        public void RemoveObject()
        {
            var compareArray = new []{"one", "three" , "four", null};
            var array = new CircularArray<string>(new []{"one", "two", "three", "four"});           
            array.Remove("two");
            TestRead(array, compareArray);
        }

        [Fact]
        public void RemoveAt()
        {
            var compareArray = new []{"one", "three" , "four", null};
            var array = new CircularArray<string>(new []{"one", "two", "three", "four"});           
            array.RemoveAt(1);
            TestRead(array, compareArray);
        }
        
        [Fact]
        public void IndexerAssignment()
        {           
            var array = new CircularArray<string>(3);
            array[2] = "assigned";
            CheckEnumerator(array, new[]{null, null, "assigned"});
            array.Add("pusher");
            CheckEnumerator(array, new[]{"pusher", null, null});
            array[2] = "assigned";
            CheckEnumerator(array, new[]{"pusher", null, "assigned"});            
        }

        [Fact]        
        public void IndexerAssignent_OutOfRange()
        {
            var array = new CircularArray<string>(3);
            Assert.Throws<IndexOutOfRangeException>(() => array[-1] = "can't assign this");
            Assert.Throws<IndexOutOfRangeException>(() => array[3] = "can't assign this");            
        }
        
        [Fact]
        public void CopyTo()
        {
            var compareArray = new []{1, 2, 3, 4, 5};
            var array = new CircularArray<int>(5);
            Fill(array, 5);
            var copyArray = new int[5];
            array.CopyTo(copyArray, 0);
            CheckEnumerator(copyArray, compareArray);
        }

        [Fact]
        public void CopyTo_Sliced()
        {            
            var compareArray = new []{3, 4, 5, 6, 7};
            var array = new CircularArray<int>(5);
            Fill(array, 7);
            var copyArray = new int[5];
            array.CopyTo(copyArray, 0);
            CheckEnumerator(copyArray, compareArray);
        }

        [Fact]
        public void CopyTo_DestinationTooSmall()
        {
            var array = new CircularArray<int>(5);
            Assert.Throws<ArgumentException>(() => array.CopyTo(new int[3], 0));
        }
        
        [Fact]
        public void ContainsTrue()
        {
            var array = new CircularArray<string>(new []{"three", "little", "pigs"});
            Assert.True(array.Contains("pigs"));
        }
        
        [Fact]
        public void ContainsFalse()
        {
            var array = new CircularArray<string>(new []{"three", "little", "pigs"});
            Assert.False(array.Contains("wolves"));
        }
        
        void Fill(ICollection<int> array, int fillQuantity)
        {
            for(int c = 1; c <= fillQuantity; c++)
            {
                array.Add(c);
            }
        }                

        void TestRead<T>(CircularArray<T> array, T[] compareArray)
        {
            CheckIndexer(array, compareArray);
            CheckEnumerator(array, compareArray);
        }
        
        void CheckIndexer<T>(CircularArray<T> array, T[] compareArray)
        {
            for (int idx = 0; idx < array.Count; idx++)
            {
                Assert.Equal(compareArray[idx], array[idx]);
            }
        }

        void CheckEnumerator<T>(IEnumerable<T> array, T[] compareArray)
        {
            // ReSharper disable once TooWideLocalVariableScope
            int compareIdx = 0;
            foreach (T arrayValue in array)
            {
                // ReSharper disable once RedundantAssignment
                Assert.Equal(compareArray[compareIdx++], arrayValue);
            }
        }
    }
}