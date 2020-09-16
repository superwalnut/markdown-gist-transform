using System;
using System.Threading.Tasks;
using MarkdownToGist.Models;

namespace MarkdownToGist.Interfaces
{
    public interface IGistService
    {
        Task<Gist> Create(string file, string content, string authToken);
    }
}
