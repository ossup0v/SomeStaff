using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Temp
{
  public class Character
  {
    private readonly StateMachine _stateMachine;

    public Character(StateMachine stateMachine)
    {
      _stateMachine = stateMachine;

    }
  }

  public class CharactorInfo
  {
    public float Health;
    public float Defence;
    public float Damage;

  }
}
