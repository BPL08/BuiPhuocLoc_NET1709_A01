using BuiPhuocLocRazorPages_BO;
using BuiPhuocLocRazorPages_DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuiPhuocLocRazorPages_Repository
{
    public class TagRepository:ITagRepository
    {
        public async Task<List<Tag>> GetAllTagsAsync() =>
            await TagDAO.Instance.GetAllTagsAsync();

        public async Task<Tag?> GetTagByIdAsync(int tagId) =>
            await TagDAO.Instance.GetTagByIdAsync(tagId);

        public async Task<Tag?> GetTagByNameAsync(string tagName) =>
            await TagDAO.Instance.GetTagByNameAsync(tagName);

        public async Task CreateTagAsync(Tag tag)
        {
            var existingTag = await TagDAO.Instance.GetTagByNameAsync(tag.TagName);
            if (existingTag != null)
            {
                throw new InvalidOperationException($"Tag '{tag.TagName}' already exists.");
            }
            await TagDAO.Instance.CreateTagAsync(tag);
        }

        public async Task UpdateTagAsync(Tag tag)
        {
            var existingTag = await TagDAO.Instance.GetTagByIdAsync(tag.TagId);
            if (existingTag == null)
            {
                throw new KeyNotFoundException($"Tag ID '{tag.TagId}' not found.");
            }
            await TagDAO.Instance.UpdateTagAsync(tag);
        }

        public async Task<bool> DeleteTagAsync(int tagId)
        {
            var tag = await TagDAO.Instance.GetTagByIdAsync(tagId);
            if (tag == null)
            {
                throw new KeyNotFoundException($"Tag ID '{tagId}' not found.");
            }

            if (tag.NewsArticles?.Any() == true)
            {
                throw new InvalidOperationException($"Cannot delete tag '{tag.TagName}' as it's linked to news articles.");
            }

            return await TagDAO.Instance.DeleteTagAsync(tagId);
        }

        public async Task<List<Tag>> GetTagsByNewsArticleIdAsync(string newsArticleId)
            => await TagDAO.Instance.GetTagsByNewsArticleIdAsync(newsArticleId);

        public async Task<List<Tag>> GetTagsByIdsAsync(List<int> tagIds)
             => await TagDAO.Instance.GetTagsByIdsAsync(tagIds);

    }
}
