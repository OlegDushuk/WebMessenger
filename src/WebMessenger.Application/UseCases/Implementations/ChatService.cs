using AutoMapper;
using WebMessenger.Application.Common.Enums;
using WebMessenger.Application.Common.Models;
using WebMessenger.Application.UseCases.Interfaces;
using WebMessenger.Shared.DTOs.Responses;
using WebMessenger.Core.Entities;
using WebMessenger.Core.Enums;
using WebMessenger.Core.Interfaces.Repositories;
using WebMessenger.Core.Interfaces.Services;
using WebMessenger.Shared.DTOs.Requests;

namespace WebMessenger.Application.UseCases.Implementations;

public class ChatService(
  IUserRepository userRepository,
  IChatRepository chatRepository,
  IChatMemberRepository chatMemberRepository,
  IChatMessageRepository messageRepository,
  IClientConnectionService clientConnectionService,
  IMapper mapper) : IChatService
{
  public async Task<Result<List<ChatDto>>> GetChats(string userEmail)
  {
    var user = await userRepository.GetByEmailAsync(userEmail);
    if (user == null)
      return Result<List<ChatDto>>.Failure(
        ErrorType.NotFound,
        "USER_BY_THIS_EMAIL_NOT_FOUND"
      );
    
    var userMembers = await chatMemberRepository.GetMembersByUserId(user.Id);
    List<ChatDto> chatsDto = [];
    
    foreach (var member in userMembers)
    {
      var chatDto = mapper.Map<ChatDto>(member.Chat);
      
      chatDto.CurrentMember = mapper.Map<ChatMemberDto>(member);
      
      if (member.Chat.Type == ChatType.Personal)
      {
        var otherMember = (await chatMemberRepository.GetOtherPrivateChatMemberByChatId(member.Chat.Id, user.Id))!;
        chatDto.OtherMember = mapper.Map<ChatMemberDto>(otherMember);
      }
      
      var lastMessage = await messageRepository.GetLastByChatId(chatDto.Id);
      if (lastMessage != null)
      {
        var lastMessageSender = await chatMemberRepository.GetMemberByIdAsync(lastMessage.MemberId);
        if (lastMessageSender != null)
        {
          chatDto.LastMessage = mapper.Map<ChatMessageDto>(lastMessage);
          chatDto.LastMessageSender = mapper.Map<ChatMemberDto>(lastMessageSender);
        }
      }
      
      chatsDto.Add(chatDto);
    }

    return Result<List<ChatDto>>.Success(chatsDto);
  }

  public async Task<Result<List<ChatMessageDto>>> GetChatHistory(string userEmail, Guid chatId, int page, int pageSize)
  {
    var user = await userRepository.GetByEmailAsync(userEmail);
    if (user == null)
      return Result<List<ChatMessageDto>>.Failure(
        ErrorType.NotFound,
        "USER_BY_THIS_EMAIL_NOT_FOUND"
      );
    
    var history = (await messageRepository.GetAllByChatId(chatId, page, pageSize))
      .Select(mapper.Map<ChatMessageDto>)
      .ToList();
    
    return Result<List<ChatMessageDto>>.Success(history);
  }

  public async Task<Result<List<ChatMemberDto>>> GetChatMembers(string userEmail, Guid chatId)
  {
    var user = await userRepository.GetByEmailAsync(userEmail);
    if (user == null)
      return Result<List<ChatMemberDto>>.Failure(
        ErrorType.NotFound,
        "USER_BY_THIS_EMAIL_NOT_FOUND"
      );

    var members = (await chatMemberRepository.GetMembersByChatId(chatId))
      .Select(mapper.Map<ChatMemberDto>)
      .ToList();
    
    return Result<List<ChatMemberDto>>.Success(members);
  }

  public async Task<Result<ChatDto>> CreatePrivateChat(string creatorEmail, string userNameSecondUser)
  {
    var creator = await userRepository.GetByEmailAsync(creatorEmail);
    if (creator == null)
      return Result<ChatDto>.Failure(
        ErrorType.NotFound,
        "USER_BY_THIS_EMAIL_NOT_FOUND"
      );
    
    var otherUser = await userRepository.GetByUserNameAsync(userNameSecondUser);
    if (otherUser == null)
      return Result<ChatDto>.Failure(
        ErrorType.NotFound,
        "USER_BY_THIS_USERNAME_NOT_FOUND"
      );

    if (await chatRepository.ExistsPrivateChatAsync(creator.Id, otherUser.Id))
      return Result<ChatDto>.Failure(
        ErrorType.Conflict,
        "CHAT_BY_THIS_USERS_ALREADY_EXISTS"
      );

    var chat = new Chat
    {
      Id = Guid.NewGuid(),
      CreatedAt = DateTime.UtcNow,
      Type = ChatType.Personal
    };
    await chatRepository.CreateChatAsync(chat);

    var creatorMember = new ChatMember
    {
      Id = Guid.NewGuid(),
      UserId = creator.Id,
      ChatId = chat.Id,
    };
    await chatMemberRepository.AddMemberAsync(creatorMember);
    
    var otherMember = new ChatMember
    {
      Id = Guid.NewGuid(),
      UserId = otherUser.Id,
      ChatId = chat.Id,
    };
    await chatMemberRepository.AddMemberAsync(otherMember);
    
    var chatDto = mapper.Map<ChatDto>(chat);
    
    var creatorMemberDto = mapper.Map<ChatMemberDto>(creatorMember);
    var otherMemberDto = mapper.Map<ChatMemberDto>(otherMember);
    
    chatDto.CurrentMember = otherMemberDto;
    chatDto.OtherMember = creatorMemberDto;
    
    await clientConnectionService.NotifyUserCreatedPrivateChat(otherUser.Id, chatDto);
    
    chatDto.CurrentMember = creatorMemberDto;
    chatDto.OtherMember = otherMemberDto;
    
    return Result<ChatDto>.Success(chatDto);
  }
  
  public async Task<Result<ChatDto>> CreateGroupChat(string creatorEmail, CreateGroupChatDto dto)
  {
    var user = await userRepository.GetByEmailAsync(creatorEmail);
    if (user == null)
      return Result<ChatDto>.Failure(
        ErrorType.NotFound,
        "USER_BY_THIS_EMAIL_NOT_FOUND"
      );
    
    if (string.IsNullOrWhiteSpace(dto.Name))
      return Result<ChatDto>.Failure(
        ErrorType.Validation,
        "CHAT_NAME_IS_REQUIRED"
      );
    
    var chatId = Guid.NewGuid();
    var chat = new Chat
    {
      Id = chatId,
      CreatedAt = DateTime.UtcNow,
      Type = ChatType.Group,
      GroupDetails = new GroupDetails
      {
        Id = chatId,
        Name = dto.Name,
        Bio = dto.Bio,
        Avatar = dto.Avatar,
        Type = mapper.Map<GroupType>(dto.Type),
      }
    };
    await chatRepository.CreateChatAsync(chat);

    var member = new ChatMember
    {
      UserId = user.Id,
      ChatId = chat.Id,
      Role = ChatMemberRole.Admin
    };
    await chatMemberRepository.AddMemberAsync(member);
    
    var chatDto = mapper.Map<ChatDto>(chat);
    chatDto.CurrentMember = mapper.Map<ChatMemberDto>(member);

    return Result<ChatDto>.Success(chatDto);
  }
  
  public async Task<Result> UpdateGroupChat(string updaterEmail, UpdateGroupChatDto dto)
  {
    // TODO: Validation

    var groupDetails = await chatRepository.GetGroupDetailsByIdAsync(dto.Id);
    
    if (dto.Name != null)
      groupDetails.Name = dto.Name;
    
    if (dto.Bio != null)
      groupDetails.Bio = dto.Bio;
    
    if (dto.Avatar != null)
      groupDetails.Avatar = dto.Avatar;
    
    if (dto.Type != null)
      groupDetails.Type = (GroupType)dto.Type;

    return Result.Success;
  }
  
  public async Task<Result<ChatDto>> JoinToGroup(string userEmail, Guid chatId)
  {
    // TODO: Validation

    var user = await userRepository.GetByEmailAsync(userEmail);
    var chat = await chatRepository.GetChatByIdAsync(chatId);
    
    var member = new ChatMember
    {
      UserId = user.Id,
      ChatId = chat.Id,
      Role = ChatMemberRole.User
    };
    await chatMemberRepository.AddMemberAsync(member);
    
    var chatDto = mapper.Map<ChatDto>(chat);
    chatDto.CurrentMember = mapper.Map<ChatMemberDto>(member);
    
    return Result<ChatDto>.Success(chatDto);
  }

  public async Task<Result> AddMemberToGroup(string adminEmail, string newMemberUserName,  Guid chatId)
  {
    // TODO: Validation
    
    var user = await userRepository.GetByUserNameAsync(newMemberUserName);
    var chat = await chatRepository.GetChatByIdAsync(chatId);

    var member = new ChatMember
    {
      UserId = user.Id,
      ChatId = chat.Id,
      Role = ChatMemberRole.User
    };
    
    await chatMemberRepository.AddMemberAsync(member);
    
    // TODO: Notify Client
    
    return Result.Success;
  }

  public async Task<Result> MuteMemberInGroup(string adminEmail, string memberUserName, Guid chatId)
  {
    // TODO: Validation
    var user = await userRepository.GetByUserNameAsync(memberUserName);
    
    var member = await chatMemberRepository.GetMemberAsync(chatId, user.Id);
    member.IsMuted = true;
    await chatMemberRepository.UpdateMemberAsync(member);
    
    // TODO: Notify Client
    
    return Result.Success;
  }

  public async Task<Result> DeleteMemberFromGroup(string adminEmail, string memberUserName, Guid chatId)
  {
    // TODO: Validation
    
    var user = await userRepository.GetByUserNameAsync(memberUserName);
    
    var member = await chatMemberRepository.GetMemberAsync(chatId, user.Id);
    await chatMemberRepository.DeleteMemberAsync(member);
    
    // TODO: Notify Client
    
    return Result.Success;
  }

  public async Task<Result> CleanHistoryChat(string cleanerEmail, Guid chatId)
  {
    // TODO: Validation
    
    var chat = await chatRepository.GetChatByIdAsync(chatId);
    
    var messages = await messageRepository.GetAllByChatId(chatId);
    foreach (var msg in messages)
    {
      await messageRepository.DeleteAsync(msg);
    }
    
    // TODO: Notify Client
    
    return Result.Success;
  }

  public async Task<Result> DeleteChat(string deleterEmail, Guid chatId)
  {
    // TODO: Validation
    return Result.Success;
    
    var chat = await chatRepository.GetChatByIdAsync(chatId);

    await chatRepository.DeleteChatAsync(chat);
    
    // TODO: Notify Client
    
  }
  
  public async Task<Result<ChatMessageDto>> SendMessage(SendMessageDto dto)
  {
    var member = await chatMemberRepository.GetMemberByIdAsync(dto.MemberId);
    if (member == null)
      return Result<ChatMessageDto>.Failure(ErrorType.NotFound, "Member not found");
    
    var message = new ChatMessage
    {
      Id = Guid.NewGuid(),
      SendAt = DateTime.UtcNow,
      ChatId = member.ChatId,
      MemberId = member.Id,
      Content = dto.Content,
      IsRead = false,
      IsEdited = false,
      NumberOfLikes = 0
    };
    await messageRepository.CreateAsync(message);
    
    var messageDto = mapper.Map<ChatMessageDto>(message);
    await clientConnectionService.SendMessageToChat(messageDto);
    
    return Result<ChatMessageDto>.Success(messageDto);
  }
  
  public async Task<Result> LikeMessage(string likerEmail, Guid messageId)
  {
    // TODO: Validation
    
    var message = await messageRepository.GetById(messageId);
    message.NumberOfLikes++;
    await messageRepository.UpdateAsync(message);
    
    // TODO: Notify Client
    
    return Result.Success;
  }

  public async Task<Result> EditMessage(string editorEmail, Guid messageId, string newMessage)
  {
    // TODO: Validation
    
    var message = await messageRepository.GetById(messageId);
    message.Content = newMessage;
    message.IsEdited = true;
    await messageRepository.UpdateAsync(message);
    
    // TODO: Notify Client
    
    return Result.Success;
  }
  
  public async Task<Result> DeleteMessage(string deleterEmail, Guid messageId)
  {
    // TODO: Validation
    
    var message = await messageRepository.GetById(messageId);
    await messageRepository.DeleteAsync(message);
    
    // TODO: Notify Client
    
    return Result.Success;
  }
}