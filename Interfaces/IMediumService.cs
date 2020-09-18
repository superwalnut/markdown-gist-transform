using System;
using System.Threading.Tasks;
using MarkdownToGist.Models;

namespace MarkdownToGist.Interfaces
{
    public interface IMediumService
    {
        Task<MediumArticle> Publish(string content, string authToken, bool isPublished = false);
    }
}
