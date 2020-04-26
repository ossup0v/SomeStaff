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
    public DateTime CurretTime;
    public UserId Creator;
    public string ChatPassword;
    public bool IsOpen;
    public AddChatMsg(UserId creator, string chatPassword, DateTime currentTime, bool isOpen = false)
    {
      Creator = creator;
      ChatPassword = chatPassword;
      CurretTime = currentTime;
      IsOpen = isOpen;
    }
  }

  public class RemoveChatMsg
  {
    public DateTime CurrentTime;
    public ChatId ChatId;
    public UserId From;
    public RemoveChatMsg(ChatId chatId, UserId from, DateTime currentTime)
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

  //(Osipov) maybe need to separate open chat and close chat (with password)
  public class Chat
  {
    //(Osipov): maybe change to logger manager<TIdType, TPermissionType> or somthing like that, 
    //(Osipov): нужно добавить битовый флаг который бы означал что может делать в чате каждая роль, ее может настраивать только создатель чата
    public readonly ChatId ChatId;
    public string Password { get; private set; }
    private bool _isOpen;
    private DateTime _createdTime;
    private List<UserId> _blackList = new List<UserId>();
    private List<UserId> _usersAlreadyJoined = new List<UserId>();
    private Dictionary<UserId, UserChatInfo> _userInfo = new Dictionary<UserId, UserChatInfo>();
    private List<ChatInvateId> _chatInvateIds = new List<ChatInvateId>();

    //Creator can be not one? Creator can be changed? 
    public Chat(UserId creator, string password, bool isOpen, DateTime currentTime)
    {
      var newUser = new UserChat
      {
        Id = creator,
      };
      _userInfo.Add(creator, new UserChatInfo(newUser, ChatPermission.Creator));
      Password = password;
      //maybe use special behavior for close/open chat
      _isOpen = isOpen;
      _createdTime = currentTime;
    }

    public ChatInvateId CreateInvate(UserId by, UserId to)
    {
      return new ChatInvateId(by, to);
    }

    public bool JoinWithInvate(ChatInvateId chatInvate)
    {
      return true;
    }

    public bool TryToJoin(string password, UserId userToJoin)
    {
      if (_isOpen)
      {
        return true;
      }
      if (password == Password)
      {
        return true;
      }
      return false;
    }

    public bool TryToAdd(UserId from, UserId to, UserChatInfo userToAdd)
    {
      if (_userInfo.ContainsKey(to))
        //(Osipov): log
        return false;
      if (!CheckPermissions(from, userToAdd.Permission))
        return false;

      //In chat can add moder >= 
      if (!CheckPermissions(from, ChatPermission.Default))
        return false;

      _userInfo.Add(to, userToAdd);
      return true;
    }

    public bool TryToRemove(UserId from, UserId to, string cause = null)
    {
      //(Osipov): Log and write cause. Maybe add some static class "causes"
      if (!CheckPermissions(from, to))
        return false;
      else
        return true;
    }

    public bool TryToSetPermission(UserId from, UserId to, ChatPermission permission)
    {
      var fromP = GetUserPermission(from);

      //(Osipov) log?
      if (!CheckPermissions(fromP, permission))
        return false;
      if (!CheckPermissions(from, to))
        return false;

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
      LeaveChatDo(to, true);
      return true;
    }

    public void LeaveChat(UserId userId)
    {
      LeaveChatDo(userId, false);
    }

    private bool CheckPermissions(UserId from, ChatPermission toP)
    {
      var fromP = GetUserPermission(from);

      return CheckPermissions(fromP, toP);
    }

    private bool CheckPermissions(ChatPermission fromP, UserId to)
    {
      var toP = GetUserPermission(to);

      return CheckPermissions(fromP, toP);
    }

    private bool CheckPermissions(UserId from, UserId to)
    {
      var fromP = GetUserPermission(from);
      var toP = GetUserPermission(to);

      return CheckPermissions(fromP, toP);
    }

    //(Osipov): Todo [CanBeNull]
    private ChatPermission GetUserPermission(UserId id)
    {
      if (!_userInfo.TryGetValue(id, out var userI))
      {
        //(Osipov): log, throw,         userI = default (UserInfo)?
      }

      return userI?.Permission ?? ChatPermission.Undefined;
    }

    /// <summary>
    /// From permissions need be > to ermissions
    /// </summary>
    private bool CheckPermissions(ChatPermission from, ChatPermission to)
    {
      return from > to;
    }

    /// <param name="isFroceLeave">banned or not?</param>
    private void LeaveChatDo(UserId userId, bool isFroceLeave)
    {
      var index = _usersAlreadyJoined.IndexOf(userId);
      if (index == -1)
      {
        //(Osipov): Todo!
        throw new Exception();
        return;
      }
      else
        _usersAlreadyJoined.RemoveAt(index);
      if (isFroceLeave) { }
      //(Osipov): One type log
      else { }
      //(Osipov): Second type log
    }
  }

  public class UserChatInfo
  {
    public UserChat User;
    public ChatPermission Permission;

    public UserChatInfo(UserChat user, ChatPermission permission)
    {
      User = user;
      Permission = permission;
    }
  }

  /// <summary>
  /// Login storaged in UserManager, this not for global User, whit only for chats
  /// </summary>
  public class UserChat
  {
    public UserId Id;
  }

  public enum ChatPermission
  {
    Undefined = 0,
    //(Osipov): maybe we will need some space
    Default = 5,
    Moderator = 10,
    Admin = 50,
    Creator = 100
  }
}
