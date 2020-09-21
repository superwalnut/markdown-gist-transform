using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarkdownToGist.Interfaces
{
    public interface IMarkdownService
    {
        Task<Dictionary<string, string>> ParseCodeToGist(string fileName, string markdown, string authToken);
    }
}
