using App.Common.Extentions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TeleSharp.TL;
using TeleSharp.TL.Messages;
using TLSharp.Core;

namespace Temp.SomeStaff
{
  class Program
  {
    static void Main(string[] args)
    {
      //var sides = new List<double> { 1.5, 2.3, 4.3, 3, 11 };
      //Console.WriteLine(LibCore.GetArea(sides.ToArray()));
      //sides = new List<double> { 1.5, 2.3, 4.3, 3 };
      //Console.WriteLine(LibCore.GetArea(sides.ToArray()));
      //sides = new List<double> { 1.99, 2.321, 10.5 };
      //Console.WriteLine(LibCore.GetArea(sides.ToArray()));
      //sides = new List<double> { 1.99, 2.321, 2.5 };
      //Console.WriteLine(LibCore.GetArea(sides.ToArray()));
      //sides = new List<double> { 1.4, 2.132 };
      //Console.WriteLine(LibCore.GetArea(sides.ToArray()));
      //sides = new List<double> { 2.3 };
      //Console.WriteLine(LibCore.GetArea(sides.ToArray()));
      //var triangle = (Triangle)LibCore.GetFigure(new List<double> { 1.99, 2.321, 2.5 }.ToArray());
      //Console.WriteLine("Is right " + triangle.IsRightTriangle);
      //triangle = (Triangle)LibCore.GetFigure(new List<double> { 5, 4, 3 }.ToArray());
      //Console.WriteLine("Is right " + triangle.IsRightTriangle);
      //Game.GameDo();
      //Do().Wait();



      var stateMachine = new StateMachine();
      stateMachine.AddState(new List<StateType> {
          StateType.Fall
        , StateType.Idle
        , StateType.Jump
        , StateType.Run
        , StateType.Walk });
      stateMachine.Do();
      stateMachine.ChangeState(StateType.Run);
      stateMachine.Do();
      stateMachine.ChangeState(StateType.Walk);
      stateMachine.Do();
      stateMachine.Prev.Do();
      stateMachine.Do();
      stateMachine.ChangeState(StateType.Fall);
      stateMachine.Do();
      stateMachine.ChangeState(StateType.Jump);
      Console.ReadLine();
    }

    public static int[,] Init2RankMass(int x, int y)
    {
      var mass = new int[x, y];
      for (int i = 0; i < x; i++)
        for (int j = 0; j < y; j++)
          mass[i, j] = 0;

      for (int i = 0, j = 0; i < x; i++, j++)
      {
        if (i == j)
          mass[i, j] = 2;
      }



      var tempX = x - 1;
      var tempY = 0;
      for (int i = 0; i < x; i++)
      {
        if (mass[tempX, tempY] == 2)
          mass[tempX, tempY] = 8;
        else
          mass[tempX, tempY] = 4;
        tempX--;
        tempY++;
      }

      return mass;
    }

    private const int apiId = 1241335;
    private const string apiHash = "1f45fba06f7cee620765f6d43bf5978d";
    private const string ChatName = "Sample";

    public static int FindLenghtOfNumber(int x, int n)
    {
      var valueOfNumber = 1;
      var stepen = 0;

      while (valueOfNumber < x)
      {
        valueOfNumber *= n;
        stepen++;
      }

      var lenghtOfArray = stepen;
      lenghtOfArray = x == valueOfNumber ? lenghtOfArray + 1 : lenghtOfArray;
      return lenghtOfArray;
    }





    public static byte[] TranslateToNBase(int num
      , int baseOfNum)
    {
      var lenghtOfNum = FindLenghtOfNumber(num, baseOfNum);
      var numInNBase = new byte[lenghtOfNum];
      for (int i = 0; i < lenghtOfNum; i++)
      {
        numInNBase[i] = (byte)(num % baseOfNum);
        num /= baseOfNum;
      }

      return numInNBase.Reverse().ToArray();
    }

    public static async Task Do()
    {
      var client = new TelegramClient(apiId, apiHash);
      await client.ConnectAsync();
      var hash = await client.SendCodeRequestAsync("79165223104");
      var code = Console.ReadLine();
      var user = await client.MakeAuthAsync("79165223104", hash, code);
      var dialogs = (TLDialogs)await client.GetUserDialogsAsync();

      foreach (var element in dialogs.Chats)
      {
        TLChat chat = element as TLChat;
        if (chat != null && chat.Title == ChatName)
        {
          await client.SendMessageAsync(new TLInputPeerChat() { ChatId = chat.Id }, "Some restart message");
        }
      }
    }

    private static string Uri = "http://localhost:90/Kingdom/KingdomsAlive";

    public static bool CheckKindgomsOnline()
    {
      var result = false;
      var httpClient = new HttpClient();
      httpClient.BaseAddress = new Uri(Uri);

      var response = httpClient.GetAsync("").Result;
      if (response.IsSuccessStatusCode)
      {
        var @string = response.Content.ReadAsStringAsync().Result;
        bool.TryParse(@string, out result);
      }
      else
      {
        Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
      }

      httpClient.Dispose();
      return result;
    }
  }



  public class State
  {
    public readonly string Title;
    public readonly string Defenition;
    public readonly List<State> NextStates;
    public string AvailableIndexes
    {
      get
      {
        var result = NextStates.Count == 0 ? "nope" : NextStates.Count == 1 ? $"0" : $"0 - {NextStates.Count - 1}";
        return result;
      }
    }
    public State CurrentState { get; private set; }
    public State PreviousState { get; private set; }

    public State(string title, string defenition)
    {
      Title = title;
      Defenition = defenition;
      NextStates = new List<State>();
      CurrentState = this;
      PreviousState = null;
    }

    public State(State other)
    {
      Title = other.Title;
      Defenition = other.Defenition;
      NextStates = other.NextStates;
      CurrentState = other.CurrentState;
      PreviousState = other.PreviousState;
    }

    public void AddState(State state)
    {
      CurrentState.NextStates.Add(state);
    }

    public List<State> ShowAvailableStates()
    {
      return CurrentState.NextStates;
    }

    public bool TryChangeState(ushort index)
    {
      if (NextStates.Count <= index)
        return false;

      PreviousState = new State(CurrentState);
      CurrentState = NextStates[index];
      return true;
    }
  }

  public class Game
  {
    public static void GameDo()
    {
      //0
      var state1 = new State("для выбора следующего состояния напишите 0", "Defenition1");
      state1.AddState(new State("5", "state 5"));

      var stateMachine = new State("Title", "Defenition");
      stateMachine.AddState(state1);
      stateMachine.AddState(new State("для выбора следующего состояния напишите 1", "Defenition2"));

      var availableStates = stateMachine.ShowAvailableStates();
      restart:
      Console.WriteLine("choose you next state !");
      Console.WriteLine("0 - " + (availableStates.Count - 1));

      ushort.TryParse(Console.ReadLine(), out var index);

      if (!stateMachine.TryChangeState(index))
      {
        Console.WriteLine("Ты чо быдло ? выбирай нормально");
        goto restart;
      }
      else
      {
        availableStates = stateMachine.ShowAvailableStates();
        Console.WriteLine("choose you next state !");
        Console.WriteLine(string.Join(" ", availableStates.Select(x => x.Title)));
        goto restart;
      }
    }
  }



















































  public class LibCore
  {
    private static Func<double, double> ZeroOrMore = (O) => Math.Max(O, 0.0);
    private static Dictionary<int, Figure> map = new Dictionary<int, Figure>
    {
      [1] = new Circle(),
      [2] = new Square(),
      [3] = new Triangle(),
      [4] = new Rectangle(),
    };

    public static double GetArea(double[] sides)
    {
      var result = 0.0;
      var sidesAmount = sides.Length;

      Figure figure;
      if (map.TryGetValue(sidesAmount, out figure))
      {
        figure.SetSides(sides);
        return figure.Area();
      }
      else
      {
        Console.WriteLine($"We has no solution for {sidesAmount} sides amount");
        result = double.NaN;
      }
      var a = new object();

      return result;
    }

    public static Figure GetFigure(double[] sides)
    {
      var sidesAmount = sides.Length;

      if (map.TryGetValue(sidesAmount, out var figure))
      {
        figure.SetSides(sides);
        return figure;
      }
      else
      {
        Console.WriteLine($"We has no solution for {sidesAmount} sides amount");
      }

      return null;
    }
  }

  public abstract class Figure
  {
    public double[] Sides { get; protected set; }
    public virtual void SetSides(double[] sides)
    {
      Sides = sides;
    }
    public double Area()
    {
      if (Sides != default(double[]))
        return Area(Sides);
      else
        return double.NaN;
    }

    protected virtual double Area(double[] sides)
    {
      throw new NotImplementedException();
    }

    public int SidesAmount => Sides.Length;
  }

  public class Triangle : Figure
  {
    public bool IsRightTriangle { get; private set; }

    private static Func<double, double> ZeroOrMore = (O) => Math.Max(O, 0.0);

    public override void SetSides(double[] sides)
    {
      base.SetSides(sides);
      IsRightTriangle = CalculateIsRight(sides[0], sides[1], sides[2]);
    }

    protected override double Area(double[] sides)
    {
      var A = sides[0];
      var B = sides[1];
      var C = sides[2];
      if (!CheckForCorrectness(A, B, C))
      {
        Console.WriteLine($"Not currect sides {sides.ToDebugString()}");
        return double.NaN;
      }
      var HP = CalculateHalfPerimeter(sides);

      return ZeroOrMore(Math.Pow(HP * (HP - A) * (HP - B) * (HP - C), 0.5));
    }

    private static double CalculateHalfPerimeter(double[] sides)
    {
      return CalculatePerimeter(sides) / 2;
    }

    private static double CalculatePerimeter(double[] sides)
    {
      var result = 0.0;
      foreach (var side in sides)
      {
        result += side;
      }
      return result;
    }

    private static bool CalculateIsRight(double A, double B, double C)
    {
      var biggestSide = 0.0;
      var smallSide1 = 0.0;
      var smallSide2 = 0.0;

      if (A > B && A > C)
      {
        biggestSide = A;
        smallSide1 = B;
        smallSide2 = C;
      }
      else if (B > A && B > C)
      {
        biggestSide = B;
        smallSide1 = A;
        smallSide2 = C;
      }
      else if (C > A && C > B)
      {
        biggestSide = C;
        smallSide1 = B;
        smallSide2 = A;
      }

      return Math.Pow(biggestSide, 2) == Math.Pow(smallSide1, 2) + Math.Pow(smallSide2, 2);
    }

    private static bool CheckForCorrectness(double A, double B, double C)
    {
      if (A > B + C)
        return false;
      if (B > C + A)
        return false;
      if (C > B + A)
        return false;

      return true;
    }
  }

  public class Circle : Figure
  {
    protected override double Area(double[] sides)
    {
      //this is R
      var side = sides[0];
      return Math.PI * Math.Pow(side, 2);
    }
  }

  public class Square : Figure
  {
    protected override double Area(double[] sides)
    {
      return sides[0] * sides[1];
    }
  }

  public class Rectangle : Figure
  {
    private static Func<double, double> ZeroOrMore = (O) => Math.Max(O, 0.0);
    protected override double Area(double[] sides)
    {
      var A = sides[0];
      var B = sides[1];
      var C = sides[2];
      var D = sides[3];
      var HP = CalculateHalfPerimeter(sides);

      return ZeroOrMore(Math.Pow(HP * (HP - A) * (HP - B) * (HP - C) * (HP - D), 0.5));
    }

    private static double CalculateHalfPerimeter(double[] sides)
    {
      return CalculatePerimeter(sides) / 2;
    }

    private static double CalculatePerimeter(double[] sides)
    {
      var result = 0.0;
      foreach (var side in sides)
      {
        result += side;
      }
      return result;
    }
  }
}
