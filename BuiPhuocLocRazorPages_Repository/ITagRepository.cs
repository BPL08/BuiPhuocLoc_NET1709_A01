using BuiPhuocLocRazorPages_BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuiPhuocLocRazorPages_Repository
{
    public interface ITagRepository
    {
          Task<List<Tag>> GetAllTagsAsync();

         Task<Tag?> GetTagByIdAsync(int tagId);
        Task<Tag?> GetTagByNameAsync(string tagName);

        Task CreateTagAsync(Tag tag);
        Task UpdateTagAsync(Tag tag);

        Task<bool> DeleteTagAsync(int tagId);

        Task<List<Tag>> GetTagsByNewsArticleIdAsync(string newsArticleId);

       
            Task<List<Tag>> GetTagsByIdsAsync(List<int> tagIds);
        

    }
}
