using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Threading;
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


        public static IEnumerable<SearchResult> Find_By_Sequence(DirectoryInfo root, string extension, string seq,
            CancellationTokenSource cts)
        {
            FileInfo[] files = null;
            DirectoryInfo[] subDirs = null;

            subDirs = root.GetDirectories();

            List<String> filePaths = new List<string>();
            var taskCount = Environment.ProcessorCount;
            List<Task<SearchResult>> tasks = new List<Task<SearchResult>>();

            // First, process all the files directly under this folder 
            //Procura todos os Directorios neste folder e verifica se há algum com permissoes especiais, se houver não chama este método recursivamente
            foreach (DirectoryInfo dirInfo in subDirs)
            {
                if (cts.IsCancellationRequested) yield return null;


                if (dirInfo != null &&
                    !(dirInfo.Attributes.HasFlag(FileAttributes.Hidden) ||
                      dirInfo.Attributes.HasFlag(FileAttributes.System)))
                {
                    foreach (var sr in Find_By_Sequence(dirInfo, extension, seq, cts))
                        yield return sr;
                }
            }


            try
            {
                files = root.GetFiles(extension);
            }
            catch (UnauthorizedAccessException e)
            {
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
            }

            //Se existirem ficheiros então cria tasks consoante o numero de processadores para procurarem os ficheiros que facam match.
            if (files != null)
            {
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
                            String fileName = Path.GetFileName(files[index].FullName).ToLower();
                            if (seq.Equals("*") || fileName.Contains(seq))
                            {
                                count++;
                                result.paths.Add(files[index].FullName);
                            }
                        }
                        result.matching_sequence = count;
                        return result;
                    }, cts.Token));
                }

                while (tasks.Count > 0 && !cts.IsCancellationRequested)
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