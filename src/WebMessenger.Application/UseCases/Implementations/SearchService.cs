using AutoMapper;
using WebMessenger.Application.Common.Enums;
using WebMessenger.Application.Common.Models;
using WebMessenger.Application.UseCases.Interfaces;
using WebMessenger.Core.Interfaces.Repositories;
using WebMessenger.Shared.DTOs.Responses;

namespace WebMessenger.Application.UseCases.Implementations;

public class SearchService(
  IUserRepository userRepository,
  IChatRepository chatRepository,
  IMapper mapper
) : ISearchService
{
  public async Task<Result<List<SearchItemDto>>> FindUsers(string searchRequest)
  {
    var user = await userRepository.GetByUserNameAsync(searchRequest);
    if (user == null)
      return Result<List<SearchItemDto>>.Failure(ErrorType.NotFound, "Not found");
    
    var dto = mapper.Map<SearchItemDto>(user);

    return Result<List<SearchItemDto>>.Success([dto]);
  }

  public async Task<Result<List<SearchItemDto>>> FindChats(string searchRequest)
  {
    var chats = await chatRepository.FindBySearchRequestAsync(searchRequest);
    if (chats.Count == 0)
      return Result<List<SearchItemDto>>.Failure(ErrorType.NotFound, "Not found");
    
    var items = chats.Select(mapper.Map<SearchItemDto>).ToList();
    
    return Result<List<SearchItemDto>>.Success(items);
  }
}