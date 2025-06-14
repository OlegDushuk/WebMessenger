using WebMessenger.Application.Common.Models;
using WebMessenger.Shared.DTOs.Responses;

namespace WebMessenger.Application.UseCases.Interfaces;

public interface ISearchService
{
  Task<Result<List<SearchItemDto>>> FindUsers(string searchRequest);
  Task<Result<List<SearchItemDto>>> FindChats(string searchRequest);
}