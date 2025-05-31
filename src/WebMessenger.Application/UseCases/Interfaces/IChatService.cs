using WebMessenger.Application.Common.Models;
using WebMessenger.Shared.DTOs.Requests;
using WebMessenger.Shared.DTOs.Responses;

namespace WebMessenger.Application.UseCases.Interfaces;

public interface IChatService
{
  Task<Result<List<ChatDto>>> GetChats(string userEmail);
  Task<Result<List<ChatMessageDto>>> GetChatHistory(string userEmail, Guid chatId, int page, int pageSize);
  Task<Result<List<ChatMemberDto>>> GetChatMembers(string userEmail, Guid chatId);
  
  Task<Result<ChatDto>> CreatePrivateChat(string creatorEmail, string userNameSecondUser);
  
  Task<Result<ChatDto>> CreateGroupChat(string creatorEmail, CreateGroupChatDto dto);
  Task<Result> UpdateGroupChat(string updaterEmail, UpdateGroupChatDto dto);
  
  Task<Result<ChatDto>> JoinToGroup(string userEmail, Guid chatId);
  
  Task<Result> AddMemberToGroup(string adminEmail, string newMemberUserName, Guid chatId);
  Task<Result> MuteMemberInGroup(string adminEmail, string memberUserName, Guid chatId);
  Task<Result> DeleteMemberFromGroup(string adminEmail, string memberUserName, Guid chatId);
  
  Task<Result> CleanHistoryChat(string cleanerEmail, Guid chatId);
  Task<Result> DeleteChat(string deleterEmail, Guid chatId);
  
  Task<Result<ChatMessageDto>> SendMessage(SendMessageDto dto);
  Task<Result> LikeMessage(string likerEmail, Guid messageId);
  Task<Result> EditMessage(string editorEmail, Guid messageId, string newMessage);
  Task<Result> DeleteMessage(string deleterEmail, Guid messageId);
}