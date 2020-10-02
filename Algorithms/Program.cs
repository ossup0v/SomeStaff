using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Linq;

namespace Algorithms
{
  public class Program
  {
    public static void Main()
    {
      SortAlgorithms.ProcessAlgorithm(SortAlgorithms.AType.QuickSort);
      SortAlgorithms.ProcessAlgorithm(SortAlgorithms.AType.HeapSort);
      SortAlgorithms.ProcessAlgorithm(SortAlgorithms.AType.BubbleSort);
      Console.ReadLine();
    }
  }

  public static class SortAlgorithms
  {
    private static Stopwatch _stopwatch = new Stopwatch();
    private static List<int> a = new List<int>() { -98, 770, -350, -620, 325, -12, -595, 340, -492, -898, -834, 609, -6, -319, 545, 112, -213, 257, -549, -869 };

    public static void ProcessAlgorithm(AType type, bool sortDescending = true)
    {
      switch (type)
      {
        case AType.BubbleSort:
          ProcessAlgorithmInternal(BubbleSort, sortDescending);
          break;
        case AType.QuickSort:
          ProcessAlgorithmInternal(QuickSort, sortDescending);
          break;
        case AType.HeapSort:
          ProcessAlgorithmInternal(HeapSort, sortDescending);
          break;
        default:
          break;
      }
    }

    #region Algorithms

    private static List<int> BubbleSort (List<int> mass, bool sortDescending)
    {
      var result = new List<int>();
      foreach (var item in mass)
        result.Add(int.MinValue);

      for (int i = 0; i < mass.Count; i++)
      {
        for (int j = i; j < mass.Count; j++)
        {
          if (sortDescending)
          {
            if (result[i] >= mass[j])
            {
              var temp = result[i];
              result[i] = mass[j];
              mass[j] = temp;
            }
          }
          else
          {
            if (result[i] <= mass[j])
            {
              var temp = result[i];
              result[i] = mass[j];
              mass[j] = temp;
            }
          }
        }
      }

      return result;
    }

    private static List<int> QuickSort(List<int> mass, bool sortDescending)
    { return new List<int>(); }

    private static List<int> HeapSort(List<int> mass, bool sortDescending)
    { return new List<int>(); }

    #endregion

    private static void ProcessAlgorithmInternal(
        Func<List<int>, bool, List<int>> algorithm
      , bool sortDescending = true)
    {
      StartTime();
      if (Check(algorithm(a, sortDescending)))
        Console.WriteLine("Algorithm was completed correct");
      else
        Console.WriteLine("Algorithm was completed incorrect");

      StopTimeAndShowResult();
    }

    private static bool Check(List<int> sortedMass, bool sortDescending = true)
    {
      if (sortedMass == null)
        return false;
      if (!sortedMass.Any() || sortedMass.Count == 1)
        return true;

      for (int i = 1; i < sortedMass.Count; i++)
        if (sortDescending)
        {
          if (!(sortedMass[i - 1] <= sortedMass[i]))
            return false;
        }
        else
        {
          if (!(sortedMass[i - 1] >= sortedMass[i]))
            return false;
        }

      return true;
    }

    private static void StartTime()
    {
      _stopwatch.Start();
    }

    private static void StopTimeAndShowResult()
    {
      _stopwatch.Stop();
      Console.WriteLine("ticks processing:" + _stopwatch.ElapsedTicks.ToString());
      _stopwatch.Reset();
    }

    public enum AType
    {
      BubbleSort,
      QuickSort,
      HeapSort
    }
  }
}