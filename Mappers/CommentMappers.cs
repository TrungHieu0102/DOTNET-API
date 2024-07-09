using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMappers
    {
        //GET
        public static CommentDTO ToCommentDto(this Comment commentModel)
        {
            return new CommentDTO
            {
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreatedOn = commentModel.CreatedOn,
                CreatedBy = commentModel.appUser.UserName,
                StockId = commentModel.StockId
            };
        }
        //POST
        public static Comment ToCommentCreate(this CreateUpdateCommentRequestDto comentDto, int stockId)
        {
            return new Comment
            {
                Title = comentDto.Title,
                Content = comentDto.Content,
                StockId = stockId
            };
        }
         public static Comment ToCommentCreate(this CreateUpdateCommentRequestDto comentDto)
        {
            return new Comment
            {
                Title = comentDto.Title,
                Content = comentDto.Content,
            };
        }
     
    }
}