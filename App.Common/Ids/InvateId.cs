using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Ids
{
  public class InvateId<TOwnerId> : BaseId
  {
    public TOwnerId By { get; private set; }
    public TOwnerId To { get; private set; }

    public InvateId(TOwnerId by, TOwnerId to) : base(IdKind.Invate)
    { By = by; To = to; }
  }
}
