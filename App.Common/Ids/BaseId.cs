using App.Common.Managers;
using System;

namespace App.Common.Ids
{
  public abstract class BaseId
  {
    public string Prefix { get; private set; }
    public ulong Id { get; private set; }
    public string FullId => Prefix + Id;
    public IdKind IdKind { get; private set; }

    public BaseId(IdKind idKind)
    {
      this.Id = IdsManager.CreateNewId(idKind);
      this.IdKind = IdKind;
      switch (idKind)
      {
        case IdKind.User:
          Prefix = "U_";
          break;
        case IdKind.Chat:
          Prefix = "Chat_";
          break;
        case IdKind.ChatInvate:
          Prefix = "ChatInvate_";
          break;
        default:
          //TODO (Osipov);
          throw new Exception();
      }
    }

    public override string ToString()
    {
      return FullId;
    }
  }
}
