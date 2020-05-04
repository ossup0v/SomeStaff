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
    private List<LoggerManager<ChatId, UserId>> _chats = new List<LoggerManager<ChatId, UserId>>();

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
      _chats.Add(new LoggerManager<ChatId, UserId>(msg.Creator, msg.ChatPassword, msg.IsOpen, msg.CurretTime));
    }

    private void RemoveChat(RemoveChatMsg msg)
    {
      //var chatToRemove = _chats.FirstOrDefault(chat => chat.ChatId == msg.ChatId);
      //if (chatToRemove == null)
      //{
      //  //TODO
      //}
      ////TODO Check user permissions
      //_chats.Remove(chatToRemove);
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
    private Dictionary<ChatId, LoggerManager<ChatId, UserId>> _chats = new Dictionary<ChatId, LoggerManager<ChatId, UserId>>();

    public ChatManager() { }

    public ChatId AddChat(LoggerManager<ChatId, UserId> chat)
    {
      var id = new ChatId();
      _chats[id] = chat;
      return id;
    }
  }

  public class Chat
  {
    private readonly LoggerManager<ChatId, UserId> _loggerManager;
    private readonly DateTime _createdTime;
    public Chat(UserId creator, string password, bool isOpen, DateTime currentTime)
    {
      _loggerManager = new LoggerManager<ChatId, UserId>(creator, password, isOpen, currentTime);
      _createdTime = currentTime;
    }
  }

  /// <typeparam name="TIdManager">on example ChatId</typeparam>
  /// <typeparam name="TOwnerId">on example UserId</typeparam>
  public class LoggerManager<TIdManager, TOwnerId> where TIdManager : BaseId where TOwnerId : BaseId
  {
    public readonly TIdManager Id;
    private string _password;
    private bool _isOpen;
    private readonly DateTime _createdTime;
    private readonly List<TOwnerId> _blackList = new List<TOwnerId>();
    private readonly List<TOwnerId> _ownersAlreadyJoined = new List<TOwnerId>();
    private readonly Dictionary<TOwnerId, OwnerInfo> _userInfo = new Dictionary<TOwnerId, OwnerInfo>();
    private readonly List<InvateId<TOwnerId>> _chatInvateIds = new List<InvateId<TOwnerId>>();

    //Creator can be not one? Creator can be changed? 
    public LoggerManager(TOwnerId creator, string password, bool isOpen, DateTime currentTime)
    {
      var newOwner = new OwnerInfo(creator) { Permission = Permission.Creator};

      _userInfo.Add(creator, newOwner);
      _password = password;
      //maybe use special behavior for close/open chat
      _isOpen = isOpen;
      _createdTime = currentTime;
    }

    public InvateId<TOwnerId> CreateInvate(TOwnerId by, TOwnerId to)
    {
      return new InvateId<TOwnerId>(by, to);
    }

    public bool JoinWithInvate(InvateId<TOwnerId> chatInvate)
    {
      return true;
    }

    public bool TryToJoin(string password, TOwnerId ownerToJoin)
    {
      if (_isOpen)
      {
        return true;
      }
      if (password == _password)
      {
        return true;
      }
      return false;
    }

    public bool TryToAdd(TOwnerId from, TOwnerId to, OwnerInfo userToAdd)
    {
      if (_userInfo.ContainsKey(to))
        //(Osipov): log
        return false;
      if (!CheckPermissions(from, userToAdd.Permission))
        return false;

      //In chat can add moder >= 
      if (!CheckPermissions(from, Permission.Default))
        return false;

      _userInfo.Add(to, userToAdd);
      return true;
    }

    public bool TryToRemove(TOwnerId from, TOwnerId to, string cause = null)
    {
      //(Osipov): Log and write cause. Maybe add some static class "causes"
      if (!CheckPermissions(from, to))
        return false;
      else
        return true;
    }

    public bool TryToSetPermission(TOwnerId from, TOwnerId to, Permission permission)
    {
      var fromP = GetUserPermission(from);

      //(Osipov) log?
      if (!CheckPermissions(fromP, permission))
        return false;
      if (!CheckPermissions(from, to))
        return false;

      return true;
    }

    public bool TryToUnbanOwner(TOwnerId from, TOwnerId to, string cause = null)
    {
      var userToUnBan = _blackList.FirstOrDefault(owner => owner.ToString() == to.ToString());
      _blackList.Remove(userToUnBan);
      return true;
    }

    public bool TryToBanOwner(TOwnerId from, TOwnerId to, string cause = null)
    {
      _blackList.Add(to);
      LeaveChatDo(to, true);
      return true;
    }

    public void LeaveChat(TOwnerId userId)
    {
      LeaveChatDo(userId, false);
    }

    #region private methods

    private bool CheckPermissions(TOwnerId from, Permission toP)
    {
      var fromP = GetUserPermission(from);

      return CheckPermissions(fromP, toP);
    }

    private bool CheckPermissions(Permission fromP, TOwnerId to)
    {
      var toP = GetUserPermission(to);

      return CheckPermissions(fromP, toP);
    }

    private bool CheckPermissions(TOwnerId from, TOwnerId to)
    {
      var fromP = GetUserPermission(from);
      var toP = GetUserPermission(to);

      return CheckPermissions(fromP, toP);
    }

    //(Osipov): Todo [CanBeNull]
    private Permission GetUserPermission(TOwnerId ownerId)
    {
      if (!_userInfo.TryGetValue(ownerId, out var ownerI))
      {
        //(Osipov): log, throw,         userI = default (UserInfo)?
      }

      return ownerI?.Permission ?? Permission.Undefined;
    }

    /// <summary>
    /// From permissions need be > to ermissions
    /// </summary>
    private bool CheckPermissions(Permission from, Permission to)
    {
      return from > to;
    }

    /// <param name="isFroceLeave">banned or not?</param>
    private void LeaveChatDo(TOwnerId ownerId, bool isFroceLeave)
    {
      var index = _ownersAlreadyJoined.IndexOf(ownerId);
      if (index == -1)
      {
        //(Osipov): Todo!
        throw new Exception();
        return;
      }
      else
        _ownersAlreadyJoined.RemoveAt(index);
      if (isFroceLeave) { }
      //(Osipov): One type log
      else { }
      //(Osipov): Second type log
    }

    public class OwnerInfo
    {
      public TOwnerId Id { get; private set; }
      public Permission Permission { get; set; }
      public OwnerInfo(TOwnerId id)
      {
        Id = id;
      }
    }

#endregion

  }

  /// <summary>
  /// Global permissions?
  /// </summary>
  public enum Permission
  {
    Undefined = 0,
    //(Osipov): maybe we will need some space
    Default = 5,
    Moderator = 10,
    Admin = 50,
    Creator = 100
  }
}
