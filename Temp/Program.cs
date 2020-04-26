using Akka.Persistence;
using App.Common.Ids;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Temp
{
  public class ChatManagerActor : ReceivePersistentActor
  {
    public override string PersistenceId => throw new NotImplementedException();
    private List<Chat> _chats = new List<Chat>();

    public ChatManagerActor()
    {
      Ready();
    }

    private void Ready()
    {
      Command<AddChatMsg>(AddChat, null);
      Command<RemoveChatMsg>(RemoveChat, null);
    }

    private void AddChat(AddChatMsg msg)
    {
      _chats.Add(new Chat(msg.Creator, msg.ChatPassword, msg.IsOpen, msg.CurretTime));
    }

    private void RemoveChat(RemoveChatMsg msg)
    {
      var chatToRemove = _chats.FirstOrDefault(chat => chat.ChatId == msg.ChatId);
      if (chatToRemove == null)
      {
        //TODO
      }
      //TODO Check user permissions
      _chats.Remove(chatToRemove);
    }
  }

  public class AddChatMsg
  {
    public TimeSpan CurretTime;
    public UserId Creator;
    public ChatPassword ChatPassword;
    public bool IsOpen;
    public AddChatMsg(UserId creator, ChatPassword chatPassword, TimeSpan currentTime, bool isOpen = false)
    {
      Creator = creator;
      ChatPassword = chatPassword;
      CurretTime = currentTime;
      IsOpen = isOpen;
    }
  }

  public class RemoveChatMsg
  {
    public TimeSpan CurrentTime;
    public ChatId ChatId;
    public UserId From;
    public RemoveChatMsg(ChatId chatId, UserId from, TimeSpan currentTime)
    {
      From = from;
      ChatId = chatId;
      CurrentTime = currentTime;
    }
  }

  public class ChatManager
  {
    private Dictionary<ChatId, Chat> _chats = new Dictionary<ChatId, Chat>();

    public ChatManager() { }

    public ChatId AddChat(Chat chat)
    {
      var id = new ChatId();
      _chats[id] = chat;
      return id;
    }
  }

  public class Chat
  {
    public readonly ChatId ChatId;
    public ChatPassword Password { get; private set; }
    private bool _isOpen;
    private TimeSpan _createdTime;
    private List<UserId> _blackList = new List<UserId>();
    private Dictionary<UserId, ChatPermission> userPermission = new Dictionary<UserId, ChatPermission>();
    private List<ChatInvateId> _chatInvateIds = new List<ChatInvateId>();

    //Creator can be not one? Creator can be changed? 
    public Chat(UserId creator, ChatPassword password, bool isOpen, TimeSpan currentTime)
    {
      userPermission.Add(creator, ChatPermission.Creator);
      Password = password;
      _isOpen = isOpen;
      _createdTime = currentTime;
    }

    public bool JoinWithInvate(UserId userToJoin, ulong invateId)
    {
      return true;
    }

    public bool TryToJoin(ChatPassword password, UserId userToJoin)
    {
      if (_isOpen)
      {
        return true;
      }
      if (password.Password == Password.Password)
      {
        return true;
      }
      return false;
    }

    public bool TryToAdd(UserId from, UserId to, ChatPermission permission = ChatPermission.Default)
    {
      return true;
    }

    public bool TryToRemove(UserId from, UserId to, string cause = null)
    {
      return true;
    }

    public bool TryToSetPermission(UserId from, UserId to, ChatPermission permission)
    {
      return true;
    }

    public bool TryToUnbanUser(UserId from, UserId to, string cause = null)
    {
      var userToUnBan = _blackList.FirstOrDefault(user => user.Id == to.Id);
      _blackList.Remove(userToUnBan);
      return true;
    }


    public bool TryToBanUser(UserId from, UserId to, string cause = null)
    {
      _blackList.Add(to);
      return true;
    }
  }

  public class UserChat
  {
    public UserId Id;
    public string Login;
    public string Password;
  }

  public class ChatPassword
  {
    public string Password;
  }

  public enum ChatPermission
  {
    Default,
    Moderator,
    Admin,
    Creator
  }
}
