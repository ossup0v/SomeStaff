using App.Common.Ids;
using System;

namespace App.Common.Managers
{
  public class IdsManager
  {
    private static ulong _chatIdCounter;
    private static ulong _userIdCounter;
    private static ulong _chatInvateIdCounter;
    public static ulong CreateNewId(IdKind idKind)
    {
      switch (idKind)
      {
        case IdKind.User:
          _userIdCounter++;
          return _userIdCounter;
        case IdKind.Chat:
          _chatIdCounter++;
          return _chatIdCounter;
        case IdKind.Invate:
          _chatInvateIdCounter++;
          return _chatInvateIdCounter;
        default:
          //TODO (Osipov);
          throw new Exception();
      }
    }
  }
}
