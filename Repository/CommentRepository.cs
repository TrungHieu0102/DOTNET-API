using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Comment;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public CommentRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            await _dbContext.Comments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(x=>x.Id == id);
            if (comment == null){
                return null;
            }
            _dbContext.Remove(comment);
            await _dbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _dbContext.Comments.Include(a=>a.AppUserId).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _dbContext.Comments.Include(a=>a.AppUserId).FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<Comment?> UpdateAsync(int id, Comment comment)
        {
            var commentModel = await _dbContext.Comments.FindAsync(id);
            if(commentModel == null){
                return null;
            }
            commentModel.Title = comment.Title;
            commentModel.Content= comment.Content;
            await _dbContext.SaveChangesAsync();
            return commentModel;
        }
    }
}