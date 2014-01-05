using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyFilesSearch
{
    public class Exercicio3
    {
        public class SearchResult
        {
            public int total;
            public int matching_sequence;
            public List<String> paths;

            public SearchResult()
            {
                total = 0;
                matching_sequence = 0;
                paths = new List<string>();
            }
        }

        /* public static IEnumerable<SearchResult> Find_By_Sequence(String root_Directory, String extension,
            String char_sequence)
        {
            var taskCount = Environment.ProcessorCount;
            List<Task<SearchResult>> tasks = new List<Task<SearchResult>>();
            System.IO.DirectoryInfo rd = new DirectoryInfo(root_Directory);
            String[] files = WalkDirectoryTree(rd, extension, seq).ToArray();

            var blockSize = files.Length/taskCount;
            for (int i = 0; i < taskCount; i++)
            {
                int taskID = i;
                tasks.Add(Task<SearchResult>.Factory.StartNew(() =>
                {
                    SearchResult result = new SearchResult();
                    int count = 0;
                    int startIndex = taskID*blockSize;
                    int endIndex = (files.Length%2 != 0 && taskID == (taskCount - 1))
                        ? (startIndex + blockSize + 1)
                        : (startIndex + blockSize);
                    result.total = endIndex - startIndex;

                    for (int index = startIndex; index < endIndex; index++)
                    {
                        String fileName = Path.GetFileName(files[index]).ToLower();
                        if (char_sequence.Equals("*") || fileName.Contains(char_sequence))
                        {
                            count++;
                            result.paths.Add(files[index]);
                        }
                    }
                    result.matching_sequence = count;
                    return result;
                }));
            }
            while (tasks.Count > 0)
            {
                Task<Task<SearchResult>> tr = Task.WhenAny(tasks);
                tasks.Remove(tr.Result);
                SearchResult r = tr.Result.Result;
                yield return r;
            }
        }*/

        public static IEnumerable<SearchResult> Find_By_Sequence(DirectoryInfo root, String extension, String seq)
        {
            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;
            
                subDirs= root.GetDirectories();

            List<String> filePaths = new List<string>();
            var taskCount = Environment.ProcessorCount;
            List<Task<SearchResult>> tasks = new List<Task<SearchResult>>();
            
            // First, process all the files directly under this folder 

            foreach (System.IO.DirectoryInfo dirInfo in subDirs)
            {
                if (dirInfo != null &&
                    !(dirInfo.Attributes.HasFlag(FileAttributes.Hidden) ||
                      dirInfo.Attributes.HasFlag(FileAttributes.System)))
                {
                    foreach (var sr in Find_By_Sequence(dirInfo, extension, seq))
                    {
                        yield return sr;
                    }
                }
            }


            try
            {
                files = root.GetFiles(extension);
            }
            catch (UnauthorizedAccessException e)
            {
/*ABAFATOR*/
            }

            catch (System.IO.DirectoryNotFoundException e)
            {
                /*ABAFATOR*/
            }

            if (files != null)
            {
                var blockSize = files.Length / taskCount;
                for (int i = 0; i < taskCount; i++)
                {
                    int taskID = i;
                    tasks.Add(Task<SearchResult>.Factory.StartNew(() =>
                    {
                        SearchResult result = new SearchResult();
                        int count = 0;
                        int startIndex = taskID*blockSize;
                        int endIndex = (files.Length%2 != 0 && taskID == (taskCount - 1))
                            ? (startIndex + blockSize + 1)
                            : (startIndex + blockSize);
                        result.total = endIndex - startIndex;

                        for (int index = startIndex; index < endIndex; index++)
                        {
                            String fileName = Path.GetFileName(files[index].FullName).ToLower();
                            if (seq.Equals("*") || fileName.Contains(seq))
                            {
                                count++;
                                result.paths.Add(files[index].FullName);
                            }
                        }
                        result.matching_sequence = count;
                        return result;
                    }));
                }

                while (tasks.Count > 0)
                {
                    Task<Task<SearchResult>> tr = Task.WhenAny(tasks);
                    tasks.Remove(tr.Result);
                    SearchResult r = tr.Result.Result;
                    yield return r;
                }
            }
        }
    }
}