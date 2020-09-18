using System;
using System.Threading.Tasks;
using MarkdownToGist.Models;

namespace MarkdownToGist.Interfaces
{
    public interface IDevToService
    {
        Task<DevToArticle> Publish(string content, string apiKey, bool isPublished = false);
    }
}
