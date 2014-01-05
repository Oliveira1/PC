using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyServer
{

    public class Exercicio3
    {

    public class SearchResult 
    {
        public int total;
        public int matching_sequence;
        public List<String> paths;

    }

        public static SearchResult Find_By_Sequence(String root_Directory, String extension, String char_sequence)
        {
            var taskCount = Environment.ProcessorCount;
            var tasks = new Task<SearchResult>[taskCount];
            String[] files = Directory.GetFiles(root_Directory, extension, SearchOption.AllDirectories);

            var blockSize = files.Length/taskCount;
            for (int i = 0; i < taskCount; i++)
            {
                int taskID = i;
                tasks[taskID] = Task<SearchResult>.Factory.StartNew(() =>
                {
                    SearchResult result=new SearchResult();
                    int count = 0;
                    int total = 0;
                    int startIndex = taskID*blockSize;
                    int endIndex = (files.Length%2 == 1 && taskID == taskCount - 1)
                        ? startIndex + blockSize
                        : startIndex + blockSize + 1;
                        count = endIndex - startIndex;
                        result.total = count;
                    if (char_sequence.Equals("*"))
                    {
                        result.matching_sequence = count;
                    }
                
                    for (int index = startIndex; index <endIndex; index++)
                    {
                        if (char_sequence.Equals("*"))
                        {
                            result.paths.Add(files[i].ToString());
                        }
                        else  if (files[i].Contains(char_sequence))
                        {
                            count++;
                            result.paths.Add(files[i].ToString());
                        }
                    }
                    return result;
                });
            }
          var taskResult= Task<SearchResult>.Factory.ContinueWhenAll(tasks, (antecedents) =>
            {
                SearchResult c=new SearchResult();
                c.total = 0;
                for (int i = 0; i < taskCount; i++)
                {
                    Task<SearchResult> t= antecedents[i];
                   c.total= t.Result.total;
                    c.matching_sequence = t.Result.matching_sequence;
                   c.paths.AddRange(t.Result.paths);
                }
         
                
                return new SearchResult();
            });
            return taskResult.Result;
        }
    }
}
