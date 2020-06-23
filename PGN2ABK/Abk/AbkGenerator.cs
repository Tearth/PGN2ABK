using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using PGN2ABK.Pgn;

namespace PGN2ABK.Abk
{
    public class AbkGenerator
    {
        public void Save(string path, IEnumerable<IntermediateEntry> intermediateEntries)
        {
            var entriesQueue = new Queue<IntermediateEntry>(intermediateEntries);
            var siblingsCountQueue = new Queue<int>();
            var currentSiblingsCount = intermediateEntries.Count() - 1;
            var currentIndex = 900;
            var freeIndex = currentIndex + intermediateEntries.Count();

            using (var fileStream = new FileStream(path, FileMode.OpenOrCreate))
            using (var binaryWriter = new BinaryWriter(fileStream))
            {
                binaryWriter.BaseStream.Seek(currentIndex * 28, SeekOrigin.Begin);
                while (entriesQueue.TryDequeue(out var intermediateEntry))
                {
                    var nextSibling = currentSiblingsCount > 0 ? currentIndex + 1 : -1;
                    var abkEntry = new AbkEntry(intermediateEntry, freeIndex, nextSibling);

                    if (intermediateEntry.Children.Count > 0)
                    {
                        intermediateEntry.Children.ForEach(p => entriesQueue.Enqueue(p));
                        siblingsCountQueue.Enqueue(intermediateEntry.Children.Count - 1);
                        freeIndex += intermediateEntry.Children.Count;
                    }

                    currentSiblingsCount--;
                    if (currentSiblingsCount < 0 && entriesQueue.Count > 0)
                    {
                        currentSiblingsCount = siblingsCountQueue.Dequeue();
                    }

                    var bytes = AbkEntryToBytes(abkEntry);
                    binaryWriter.Write(bytes);

                    currentIndex++;
                }
            }
        }

        private byte[] AbkEntryToBytes(AbkEntry entry)
        {
            var size = Marshal.SizeOf(entry);
            var array = new byte[size];
            var ptr = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(entry, ptr, true);
            Marshal.Copy(ptr, array, 0, size);
            Marshal.FreeHGlobal(ptr);

            return array;
        }
    }
}
