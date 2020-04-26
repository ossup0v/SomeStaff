using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Ids
{
  public class ChatInvateId : BaseId
  {
    public UserId By { get; private set; }
    public UserId To { get; private set; }

    public ChatInvateId(UserId by, UserId to) : base(IdKind.ChatInvate)
    { By = by; To = to; }
  }
}
