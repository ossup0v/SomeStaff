using App.Common.Extentions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TeleSharp.TL;
using TeleSharp.TL.Messages;
using TLSharp.Core;

namespace Temp.SomeStaff
{
  class Program
  {
    int high = 20;
    int weight = 30;
    int[] X = new int[20];
    int[] Y = new int[20];

    int counter = 0;

    int fruitX;
    int fruitY;

    int parts = 3;

    public ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
    public char key = 'w';

    Random random = new Random();

    Program()
    {
      X[0] = 5;
      Y[0] = 5;
      Console.CursorVisible = false;
      fruitX = random.Next(2, (weight - 2));
      fruitY = random.Next(2, (high - 2));
    }

    public void WriteBoard()
    {
      Console.Clear();
      for (int i = 1; i <= (weight + 2); i++)
      {
        Console.SetCursorPosition(i, 1);
        Console.Write("-");
      }
      for (int i = 1; i <= (weight + 2); i++)
      {
        Console.SetCursorPosition(i, (high + 2));
        Console.Write("-");
      }
      for (int i = 1; i <= (high + 1); i++)
      {
        Console.SetCursorPosition(1, i);
        Console.Write("-");
      }
      for (int i = 1; i <= (high + 1); i++)
      {
        Console.SetCursorPosition((weight + 2), i);
        Console.Write("-");
      }
    }
    public void Input()
    {
      if (Console.KeyAvailable)
      {
        keyInfo = Console.ReadKey(true);
        key = keyInfo.KeyChar;
      }
    }

    public void WritePoint(int x, int y)
    {
      Console.SetCursorPosition(x, y);
      Console.Write("#");
    }
    public void Logic()
    {
      if (X[0] == fruitX && Y[0] == fruitY)
      {
        parts++; // длина змеи
        counter++; // счётчик
        fruitX = random.Next(2, (weight - 2)); // генерируются фрукты 
        fruitY = random.Next(2, (high - 2));

      }
      for (int i = parts; i > 1; i--)
      {
        X[i - 1] = X[i - 2];
        Y[i - 1] = Y[i - 2];
      }
      switch (key)
      {
        case 'w':
          Y[0]--;
          break;
        case 's':
          Y[0]++;
          break;
        case 'a':
          X[0]--;
          break;
        case 'd':
          X[0]++;
          break;
      }
      for (int i = 0; i <= (parts - 1); i++)
      {
        WritePoint(X[i], Y[i]);
        WritePoint(fruitX, fruitY);
        if (i == parts)
        {
          Console.SetCursorPosition((weight + 5), 5);
          Console.Write("You WIN");
        }
      }
      Thread.Sleep(150);
    }
    public void Winner()
    {

    }
    public void Counter()
    {
      Console.SetCursorPosition(weight + 5, 1);
      Console.Write("# = " + counter);
    }
    private static readonly PerformanceCounter _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
    static void Main(string[] args)
    {
      //var array = new int[] { 3, 5, 432, -1, 4, 0, 5, 15, 3, 25, 6, 53, 4, 5, 1 };
      //Console.WriteLine(string.Join(" ", MeMergeSort(array)));
      //Console.WriteLine(string.Join(" ", MeMergeSort(array,true)));

      //var marchPredictionService = new MatchPredictionService();
      //var a = marchPredictionService.IsHaveNewCachedPrediction.Result;
      //var notify = marchPredictionService.GetPrediction();
      //var a2 = _cpuCounter.NextValue();
      //Console.WriteLine(a2);
      var a = new List<int> { 1, 2, 3, 4 };
      var b = new List<int> { 3, 4, 5, 6 };
      Console.WriteLine(string.Join(" ", a.Except(b).Select(em=> em.ToString())));
      Console.ReadLine();
    }

    public class MatchPredictionService
    {
      public Task<bool> IsHaveNewCachedPrediction => CheckForNewPrediction();

      public MatchPredictionService()
      {
        _client = new HttpClient();
      }

      public string GetPrediction()
      {
        return _cachedPrediction;
      }

      private async Task<bool> CheckForNewPrediction()
      {
        var html = await _client.GetStringAsync(_uri);
        //some logic here
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);
        var matchesDiv = htmlDocument.DocumentNode.Descendants("div")
          .Where(node => node.GetAttributeValue("class", "")
            .Contains("upcomingMatch"))
          .ToList();

        foreach (var matchDiv in matchesDiv)
        {

        }


        _cachedPrediction = html;
        return true;
      }

      private HttpClient _client;
      private Uri _uri = new Uri(_uriString);
      private const string _uriString = "https://www.hltv.org/matches";
      private string _cachedPrediction = "empty";
    }

    public static int[] SelectionSort(int[] a)
    {
      for (int i = 0; i < a.Length; i++)
      {
        var smallestNum = int.MaxValue;
        var temp = 0;
        var lastJ = -1;
        for (int j = i; j < a.Length; j++)
        {
          if (a[j] < smallestNum)
          {
            smallestNum = a[j];
            lastJ = j;
          }
        }
        temp = a[i];
        a[i] = smallestNum;
        a[lastJ] = temp;
      }
      return a;
    }



    public static int[] MeMergeSort(int[] array, bool decrease = false)
    {
      return MeMergeSort(array, 0, array.Length - 1, decrease);
    }

    private static int[] MeMergeSort(int[] array, int startIndex, int lastIndex, bool decrease = false)
    {
      if (startIndex < lastIndex)
      {
        var middleIndex = (startIndex + lastIndex) / 2;
        MeMergeSort(array, startIndex, middleIndex);
        MeMergeSort(array, middleIndex + 1, lastIndex);
        MeMerge(array, startIndex, middleIndex, lastIndex, decrease);
      }

      return array;
    }

    private static void MeMerge(int[] array, int lowI, int middleI, int highI, bool decrease = false)
    {
      Func<int, int, bool> compare;
      if (decrease)
        compare = new Func<int, int, bool>((a, b) => a < b);
      else
        compare = new Func<int, int, bool>((a, b) => a > b);

      var leftI = lowI;
      var rightI = middleI + 1;
      var I = 0;
      var tempArray = new int[highI - lowI + 1];

      while ((leftI <= middleI) && (rightI <= highI))
      {
        if (array[leftI] > array[rightI])
        {
          tempArray[I] = array[leftI];
          leftI++;
        }
        else
        {
          tempArray[I] = array[rightI];
          rightI++;
        }

        I++;
      }

      for (var i = leftI; i <= middleI; i++)
      {
        tempArray[I] = array[i];
        I++;
      }

      for (var i = rightI; i <= highI; i++)
      {
        tempArray[I] = array[i];
        I++;
      }

      for (var i = 0; i < tempArray.Length; i++)
      {
        array[lowI + i] = tempArray[i];
      }
    }












    private static int[] MergeSort(int[] array)
    {
      return MergeSort(array, 0, array.Length - 1);
    }

    private static int[] MergeSort(int[] array, int startIndex, int lastIndex)
    {
      if (startIndex < lastIndex)
      {
        var middleIndex = (startIndex + lastIndex) / 2;
        MergeSort(array, startIndex, middleIndex);
        MergeSort(array, middleIndex + 1, lastIndex);
        Merge(array, startIndex, middleIndex, lastIndex);
      }
      return array;
    }

    private static void Merge(int[] array, int lowIndex, int middleIndex, int highIndex)
    {
      var left = lowIndex;
      var right = middleIndex + 1;
      var tempArray = new int[highIndex - lowIndex + 1];
      var index = 0;

      while ((left <= middleIndex) && (right <= highIndex))
      {
        if (array[left] < array[right])
        {
          tempArray[index] = array[left];
          left++;
        }
        else
        {
          tempArray[index] = array[right];
          right++;
        }

        index++;
      }

      for (var i = left; i <= middleIndex; i++)
      {
        tempArray[index] = array[i];
        index++;
      }

      for (var i = right; i <= highIndex; i++)
      {
        tempArray[index] = array[i];
        index++;
      }

      for (var i = 0; i < tempArray.Length; i++)
      {
        array[lowIndex + i] = tempArray[i];
      }
    }

    private static void ShowMatrix(double[,] matrix)
    {
      for (int i = 0; i < matrix.GetLength(0); i++)
      {
        if (i == 0)
          Console.Write("  " + i.ToString() + " ");
        else
          Console.Write(i.ToString() + " ");
      }

      Console.WriteLine();
      for (int i = 0; i < matrix.GetLength(0); i++)
      {
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
          if (j == i && j == 0)
            Console.Write(i.ToString() + "   ");
          else if (j == 0)
            Console.Write(i.ToString() + " " + matrix[i, j].ToString() + " ");
          else if (j == i)
            Console.Write("  ");
          else
            Console.Write(matrix[i, j].ToString() + " ");
        }
        Console.WriteLine();
      }
    }
  }
}
public class Algorithm
{
  public static int[] InsertionSort1(int[] array)
  {
    for (int j = 1; j < array.Length; j++)
    {
      var key = array[j];
      var i = j - 1;
      while (i >= 0 && array[i] >= key)
      {
        array[i + 1] = array[i];
        i--;
      }
      array[i + 1] = key;
    }
    return array;
  }

  public static int[] InsertionSort2(int[] array)
  {
    for (int j = 1; j < array.Length; j++)
    {
      var key = array[j];
      var i = j - 1;
      while (i >= 0 && array[i] <= key)
      {
        array[i + 1] = array[i];
        i--;
      }
      array[i + 1] = key;
    }
    return array;
  }

  public static int? FindFirstIndex(int[] array, int valueToFind)
  {
    for (int i = 0; i < array.Length; i++)
      if (array[i] == valueToFind)
        return i;

    return null;
  }

  public static byte[] MergeArrays(byte[] a, byte[] b, byte radix)
  {
    var c = new byte[a.Length + 1];

    for (byte i = 0; i < a.Length; i++)
    {
      var res = (byte)unchecked(a[i] + b[i]);

      if (res >= radix)
      {
        c[i] += (byte)(res % radix);
        c[i + 1] = 1;
      }
      else
        c[i] += res;
    }

    return c;
  }
}

public class Vertex
{
  public int Number { get; set; }

  public Vertex(int number)
  {
    Number = number;
  }

  public override string ToString()
  {
    return Number.ToString();
  }

  public bool Equals(Vertex other)
  {
    return other?.Number == this.Number;
  }

  public override bool Equals(object other)
  {
    var vOther = other as Vertex;

    if (vOther == null)
      return false;

    return this.Equals(vOther);
  }
}

public class Edge
{
  public Vertex From { get; set; }
  public Vertex To { get; set; }
  public double Weight { get; set; }
  public bool IsOriented { get; set; }

  public Edge(Vertex from, Vertex to, double weight = 1, bool isOriented = false)
  {
    From = from;
    To = to;
    Weight = weight;
    IsOriented = isOriented;
  }

  public override string ToString()
  {
    return $"F_{From.ToString()} T_{To.ToString()} W_{Weight.ToString()} IsOriented{IsOriented.ToString()}";
  }
}

public class Graph
{
  public IReadOnlyCollection<Edge> Edges { get { return _edges; } }

  public void Add(Edge edge)
  {
    _edges.Add(edge);
    _edgesInternal.Add(edge);
    if (!edge.IsOriented)
      _edgesInternal.Add(new Edge(edge.From, edge.To, edge.Weight));
  }

  private List<Edge> _edges = new List<Edge>();
  private List<Edge> _edgesInternal = new List<Edge>();


  public double[,] GetMatrix()
  {
    var countOfVertexes = this.CountOfVertexes();
    var matrix = new double[countOfVertexes, countOfVertexes];

    foreach (var edge in _edgesInternal)
    {
      var row = edge.From.Number;
      var colomn = edge.To.Number;

      matrix[row, colomn] = edge.Weight;
    }

    return matrix;
  }

  public List<Vertex> GetAdjectedVertexs(Vertex from)
  {
    var result = new List<Vertex>();

    foreach (var edge in _edgesInternal)
      if (edge.From.Equals(from))
        if (!result.Contains(edge.To))
          result.Add(edge.To);

    return result;
  }

  public bool IsHaveWayTo(Vertex from, Vertex to)
  {
    var result = new List<Vertex> { from };

    for (int i = 0; i < result.Count; i++)
    {
      var cV = result[i];
      foreach (var v in GetAdjectedVertexs(cV))
        if (!result.Contains(v))
          result.Add(v);
    }

    return result.Contains(to);
  }

  private int CountOfVertexes()
  {
    var counter = new List<int>();
    foreach (var edge in _edgesInternal)
    {
      if (!counter.Contains(edge.From.Number))
        counter.Add(edge.From.Number);

      if (!counter.Contains(edge.To.Number))
        counter.Add(edge.To.Number);
    }
    return counter.Count;
  }

  public abstract class Id
  {
    public readonly int id;
    protected virtual string _prefix { get; set; }

    public Id(idTypes idType)
    {
      switch (idType)
      {
        case idTypes.User:
          _prefix = "Us_";
          break;
        case idTypes.Manager:
          _prefix = "Man_";
          break;
        case idTypes.SomeShit:
          _prefix = "Shit_";
          break;
        default:
          _prefix = "WTF_";
          break;
      }
      id = IdManager.AddId();
    }

    public override string ToString()
    {
      return _prefix + id;
    }
  }

  public class UserId : Id
  {
    public UserId() : base(idTypes.User) { }
  }

  public class ManagerId : Id
  {
    public ManagerId() : base(idTypes.Manager) { }
  }

  public class ShitId : Id
  {
    public ShitId() : base(idTypes.SomeShit) { }
  }


  public enum idTypes
  {
    User,
    Manager,
    SomeShit
  }


  public static class IdManager
  {
    public static int idCounter { get; private set; } = 1;

    /// <returns>new id</returns>
    public static int AddId()
    {
      idCounter++;
      return idCounter;
    }
  }
}























































public static class ex
{
  /// <returns>Item1 - work itself, item2 - all the rest.</returns>
  public static Tuple<string, string> GetFirstWord(this string str, string separator = " ")
  {
    if (!str.Contains(separator))
      return new Tuple<string, string>(string.Empty, string.Empty);
    else
    {
      var indexOf = str.IndexOf(separator);
      if (indexOf == -1)
        return new Tuple<string, string>(str, string.Empty);
      var indexOfLastCharSep = indexOf + separator.Length;
      var firstW = str.Substring(0, indexOf);
      var onther = "";
      if (indexOf != -1 && str.Length >= indexOfLastCharSep)
        onther = str.Substring(indexOfLastCharSep, str.Length - (indexOfLastCharSep));
      return new Tuple<string, string>(firstW, onther);
    }
  }
}
public class A
{
  public class Some
  {
    public Some curr;
    public string somename;

    public Some(string somename)
    {
      curr = this;
      this.somename = somename;
    }
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
